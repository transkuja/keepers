using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class IngameUI : MonoBehaviour
{
    private enum PanelShortcutChildren { Image, HpGauge, HungerGauge, MentalHealthGauge, ActionPoints };

    // KeeperPanel
    [Header("Character Panel")]
    //public GameObject goSelectedKeeperPanel;
    // Inventory
    public GameObject goSelectedKeeperPanel;
    public GameObject Panel_Inventories;

    // StatsPanel
    public GameObject goStats;

    // Turn Panel
    [Header("Turn Panel")]
    public GameObject TurnPanel;
    public GameObject TurnButton;
    public float buttonRotationSpeed = 1.0f;

    // ActionPanel
    [Header("Action Panel")]
    private GameObject goActionPanelQ;
    private Canvas worldSpaceCanvas;

    public GameObject goMoralPanel;


    // ContentQuest
    public GameObject goContentQuestParent;
    // ContentQuest
    public GameObject goContentPanneauParent;

    // ShortcutPanel
    [Header("ShortcutPanel")]
    public GameObject goShortcutKeepersPanel;

    [Header("Confirmation Panel")]
    public GameObject goConfirmationPanel;


    // Quentin
    [Header("Foreign UI Components")]
    [SerializeField] private UIIconFeedback uiIconFeedBack;

    [HideInInspector]
    public bool isTurnEnding = false;

    #region Accessors
    public UIIconFeedback UiIconFeedBack
    {
        get
        {
            return uiIconFeedBack;
        }

        set
        {
            uiIconFeedBack = value;
        }
    }
    #endregion

    public void Start()
    {
        //CreateShortcutPanel();
        //CreateKeepersInventoryPanels();
       // GameManager.Instance.PrisonerInstance.prisonerFeedingPanel = CreatePrisonerFeedingPanel(GameManager.Instance.PrisonerInstance.gameObject);
        // TODO : rustine pour que ça marche quand on relance le jeu
        GameObject worldSpaceUI = Instantiate(GameManager.Instance.PrefabUtils.WorldSpaceUIprefab);
        worldSpaceCanvas = worldSpaceUI.transform.GetChild(0).GetComponent<Canvas>();
        goActionPanelQ = worldSpaceUI.transform.GetChild(0).GetChild(0).gameObject;
        //UpdateShortcutPanel();
    }

    public void ResetIngameUI()
    {
        GameManager.Instance.Ui.ClearActionPanel();
        GameManager.Instance.Ui.HideInventoryPanels();

        // A caracter is selected
        //if (GameManager.Instance.ListOfSelectedKeepersOld[0] != null)
        //{
        //    if (!GameManager.Instance.ListOfSelectedKeepersOld[0].IsAlive)
        //    {
        //        GameManager.Instance.Ui.HideSelectedKeeperPanel();
        //    } else
        //    {
        //        GameManager.Instance.Ui.ShowSelectedKeeperPanel();
        //    }
        //}
    }

    #region Action
    public void UpdateActionPanelUIQ(InteractionImplementer ic)
    {
        if (GameManager.Instance == null) { return; }
        if (goActionPanelQ == null) { return; }
        if (GameManager.Instance.ListOfSelectedKeepers.Count == 0) { return; }

        //Clear
        ClearActionPanel();

        goActionPanelQ.GetComponent<RectTransform>().localPosition = Vector3.zero;

        // Actions
        for (int i = 0; i < ic.listActionContainers.Count; i++)
        {
            bool bIsForbiden = ic.listActionContainers[i].strName == "Quest" && !GameManager.Instance.GetFirstSelectedKeeper().Data.Behaviours[(int)BehavioursEnum.CanSpeak];
            bIsForbiden = bIsForbiden || ic.listActionContainers[i].strName == "Moral" && !GameManager.Instance.GetFirstSelectedKeeper().Data.Behaviours[(int)BehavioursEnum.CanSpeak];
            if (!bIsForbiden)
            {
                GameObject goAction = Instantiate(GameManager.Instance.PrefabUtils.PrefabActionUI, goActionPanelQ.transform);
                goAction.name = ic.listActionContainers[i].strName;

                goAction.GetComponent<RectTransform>().localPosition = Vector3.zero;
                goAction.GetComponent<RectTransform>().localRotation = Quaternion.identity;

                Button btn = goAction.GetComponent<Button>();

                int n = i;

                // TODO @Remi, cette merde bug quand on change de perso selectionné et qu'on rentre dans le trigger du panneau >:(
                int nbActionRestantKeeper = GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Behaviour.Keeper>().ActionPoints;

                if (ic.listActionContainers[i].costAction > nbActionRestantKeeper)
                {
                    goAction.GetComponent<Image>().color = Color.grey;
                }

                if (ic.listActionContainers[i].costAction > 0)
                {
                    GameObject actionPoints = Instantiate(GameManager.Instance.PrefabUtils.PrefabActionPoint, goAction.transform);
                    actionPoints.transform.localScale = Vector3.one;
                    actionPoints.transform.localPosition = Vector3.zero;
                    actionPoints.transform.localRotation = Quaternion.identity;

                    actionPoints.transform.GetComponentInChildren<Text>().text =  ic.listActionContainers[i].costAction + "/" + nbActionRestantKeeper;
                    if (ic.listActionContainers[i].costAction > nbActionRestantKeeper)
                        actionPoints.transform.GetComponentInChildren<Text>().color = Color.red;

                }

                int iParam = ic.listActionContainers[n].iParam;
                btn.onClick.AddListener(() => {
                    ic.listActionContainers[n].action(iParam);
                    GameManager.Instance.Ui.ClearActionPanel();
                });

                btn.transform.GetComponentInChildren<Image>().sprite = ic.listActionContainers[i].sprite;
                btn.transform.GetComponentInChildren<Image>().transform.localScale = Vector3.one;
            }
        }

        worldSpaceCanvas.transform.SetParent(GameManager.Instance.GoTarget.transform);
        worldSpaceCanvas.GetComponent<BillboardForWorldSpaceUI>().RecalculateActionCanvas();
    }

    public void ClearActionPanel()
    {
        if (goActionPanelQ != null && goActionPanelQ.transform.childCount > 0)
        {
            foreach (Image ActionPanel in goActionPanelQ.GetComponentsInChildren<Image>())
            {
                Destroy(ActionPanel.gameObject);
            }
        }
    }

    // TODO : rendre cette fonction générique avec target et panel where 
    public void MoralBuffActionTextAnimation(int amount)
    {
        goMoralPanel.gameObject.SetActive(true);
        // TODO : Ajouter la taille pion ? 
        goMoralPanel.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.GoTarget.transform.position);
        if (amount < 0)
        {
            goMoralPanel.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteMoralDebuff;
            goMoralPanel.GetComponentInChildren<Text>().color = Color.red;
            goMoralPanel.GetComponentInChildren<Text>().text = "";
        } else
        {
            goMoralPanel.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteMoralBuff;
            goMoralPanel.GetComponentInChildren<Text>().color = Color.green;
            goMoralPanel.GetComponentInChildren<Text>().text = "+ ";
        }
        goMoralPanel.GetComponentInChildren<Text>().text += amount.ToString();

        StartCoroutine(MoralPanelNormalState());
    }

    private IEnumerator MoralPanelNormalState()
    {
        for (float f = 3.0f; f >= 0; f -= 0.1f)
        {
            Vector3 decal = new Vector3(0.0f, f, 0.0f);
            goMoralPanel.transform.position += decal;
            yield return null;
        }
        goMoralPanel.gameObject.SetActive(false);
    }


    public void BuffActionTextAnimation(GameObject goStatBuff, int amount)
    {
        //goStatBuff.gameObject.SetActive(true);
        //if (amount < 0)
        //{
        //    //goStatBuff.GetComponent<Image>().sprite = spriteMoralDebuff;
        //    goStatBuff.GetComponentInChildren<Text>().color = Color.red;
        //    goStatBuff.GetComponentInChildren<Text>().text = "";
        //}
        //else
        //{
        //    //goStatBuff.GetComponent<Image>().sprite = spriteMoralBuff;
        //    goStatBuff.GetComponentInChildren<Text>().color = Color.green;
        //    goStatBuff.GetComponentInChildren<Text>().text = "+ ";
        //}
        //goStatBuff.GetComponentInChildren<Text>().text += amount.ToString();

        //StartCoroutine(BuffPanelNormalState(goStatBuff));
    }

    private IEnumerator BuffPanelNormalState(GameObject goStatBuff)
    {
        for (float f = 3.0f; f >= 0; f -= 0.1f)
        {
            yield return null;
        }
        goStatBuff.gameObject.SetActive(false);
    }
    #endregion

    #region Turn
    public void EndTurn()
    {
        if (!isTurnEnding)
        {
            AnimateButtonOnClick();
        }
    }
    
    private void AnimateButtonOnClick()
    {
        //for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        //{
        //    if (GameManager.Instance.AllKeepersList[i].ActionPoints > 0)
        //    {
        //        GoToCharacter(i);
        //        SelectedKeeperActionText.GetComponent<Text>().color = Color.green;
        //        SelectedKeeperActionText.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        //        StartCoroutine(TextAnimationNormalState());
        //        return;
        //    }
        //}

        // Activation de l'animation au moment du click
        Animator anim_button = TurnButton.GetComponent<Animator>();
  
        anim_button.speed = buttonRotationSpeed;
        anim_button.enabled = true;
        isTurnEnding = false;
    }
    #endregion

    #region ShortcutPanel
    public void ToggleShortcutPanel()
    {
        goShortcutKeepersPanel.SetActive(!goShortcutKeepersPanel.activeSelf);
    }
    #endregion

    #region SelectedKeeper
    public void ShowSelectedKeeperPanel()
    {
        goSelectedKeeperPanel.SetActive(true);
    }


    public void ZeroActionTextAnimation()
    {
        //SelectedKeeperActionText.GetComponent<Text>().color = Color.red;
        //SelectedKeeperActionText.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        StartCoroutine(TextAnimationNormalState());
    }

    private IEnumerator TextAnimationNormalState()
    {
        //yield return new WaitForSeconds(1);
        //SelectedKeeperActionText.GetComponent<Text>().color = Color.white;
        //SelectedKeeperActionText.transform.localScale = Vector3.one;
        yield return null;
    }

    public void DecreaseActionTextAnimation(int amount)
    {
        //DecreasedActionText.GetComponent<Text>().text = "- " + amount.ToString();
        //DecreasedActionText.gameObject.SetActive(true);
        //StartCoroutine(DecreaseTextNormalState());
    }

    private IEnumerator DecreaseTextNormalState()
    {
        //Vector3 origin = DecreasedActionText.transform.position;
        //for (float f = 3.0f; f >= 0; f -= 0.1f)
        //{
        //    Vector3 decal = new Vector3(0.0f, f, 0.0f);
        //    DecreasedActionText.transform.position += decal;
        //    yield return null;
        //}
        //DecreasedActionText.transform.position = origin;
        //DecreasedActionText.gameObject.SetActive(false);
        yield return null;
    }
    #endregion

    #region Keepers_Inventory
    //public void CreateKeepersInventoryPanels()
    //{
    //    foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
    //    {
    //        ki.GetComponent<Behaviour.Inventory>().InventoryPanel = CreateInventoryPanel(ki.gameObject);
    //    }
    //}

    //public GameObject CreateInventoryPanel(GameObject pi)
    //{
    //    if (pi.GetComponent<PNJInstance>() == null && pi.GetComponent<KeeperInstance>() == null && pi.GetComponent<LootInstance>() == null) return null;
    //    GameObject owner = null;
    //    Sprite associatedSprite = null;
    //    string name = "";
    //    int nbSlots = 0;
    //    GameObject goInventory = Instantiate(GameManager.Instance.PrefabUtils.PrefabInventaireUI, Panel_Inventories.transform);
    //    if (pi.GetComponent<PNJInstance>() != null)
    //    {
    //        PNJInstance pnjInstance = pi.GetComponent<PNJInstance>();
    //        associatedSprite = pnjInstance.Pnj.AssociatedSprite;
    //        name = pnjInstance.Pnj.CharacterName;
    //        owner = pnjInstance.gameObject;
    //        nbSlots = pnjInstance.Pnj.nbSlot;
    //    }
    //    else if (pi.GetComponent<KeeperInstance>() != null)
    //    {
    //        KeeperInstance keeperInstance = pi.GetComponent<KeeperInstance>();
    //        associatedSprite = keeperInstance.Keeper.AssociatedSprite;
    //        name = keeperInstance.Keeper.CharacterName;
    //        owner = keeperInstance.gameObject;
    //        nbSlots = keeperInstance.Keeper.nbSlot;
    //    }
    //    else if (pi.GetComponent<LootInstance>() != null)
    //    {
    //        LootInstance lootInstance = pi.GetComponent<LootInstance>();

    //        // TMP
    //        associatedSprite = GameManager.Instance.SpriteUtils.spriteLoot;
    //        goInventory.transform.GetChild(0).gameObject.SetActive(false);
    //        owner = lootInstance.gameObject;
    //        nbSlots = lootInstance.nbSlot;
    //        name = "Loot";
    //        Debug.Log(nbSlots);
    //    }
    //    else
    //    {
    //        return null;
    //    }


    //    goInventory.transform.localPosition = Vector3.zero;
    //    goInventory.transform.localScale = Vector3.one;
    //    goInventory.name = "Inventory_" + name;
    //    goInventory.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = associatedSprite;

    //    goInventory.transform.GetChild(1).GetComponent<InventoryOwner>().Owner = pi.gameObject;
    //    goInventory.SetActive(false);

    //    for (int i = 0; i < nbSlots; i++)
    //    {
    //        //Create Slots
    //        GameObject currentgoSlotPanel = Instantiate(GameManager.Instance.PrefabUtils.PrefabSlotUI, Vector3.zero, Quaternion.identity) as GameObject;
    //        currentgoSlotPanel.transform.SetParent(goInventory.transform.GetChild(1).transform);

    //        currentgoSlotPanel.transform.localPosition = Vector3.zero;
    //        currentgoSlotPanel.transform.localScale = Vector3.one;
    //        currentgoSlotPanel.name = "Slot" + i;
    //    }

    //    return goInventory;
    //}

    //public GameObject CreatePrisonerFeedingPanel(GameObject pi)
    //{
    //    if (pi.GetComponent<PrisonerInstance>() == null) return null;
    //    GameObject owner = null;
    //    Sprite associatedSprite = null;
    //    string name = "";
    //    int nbSlots = 0;
    //    PrisonerInstance prisoner = pi.GetComponent<PrisonerInstance>();
    //    GameObject goInventory = Instantiate(GameManager.Instance.PrefabUtils.PrefabPrisonerFeedingUI, Panel_Inventories.transform);

    //    associatedSprite = prisoner.Prisoner.AssociatedSprite;
    //    name = prisoner.Prisoner.CharacterName;
    //    owner = prisoner.gameObject;
    //    nbSlots = prisoner.FeedingSlotsCount;

    //    goInventory.transform.localPosition = Vector3.zero;
    //    goInventory.transform.localScale = Vector3.one;
    //    goInventory.name = "Prisoner_" + name + "_Miam_miam";
    //    goInventory.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = associatedSprite;

    //    goInventory.transform.GetChild(1).GetComponent<InventoryOwner>().Owner = pi.gameObject;
    //    goInventory.SetActive(false);

    //    for (int i = 0; i < nbSlots; i++)
    //    {
    //        //Create Slots
    //        GameObject currentgoSlotPanel = Instantiate(GameManager.Instance.PrefabUtils.PrefabSlotUI, Vector3.zero, Quaternion.identity) as GameObject;
    //        currentgoSlotPanel.transform.SetParent(goInventory.transform.GetChild(1).transform);

    //        currentgoSlotPanel.transform.localPosition = Vector3.zero;
    //        currentgoSlotPanel.transform.localScale = Vector3.one;
    //        currentgoSlotPanel.name = "Slot" + i;
    //        currentgoSlotPanel.transform.SetAsFirstSibling();
    //    }
    //    int nbChildren = goInventory.transform.GetChild(1).childCount;
    //    Button confirmButton = goInventory.transform.GetChild(1).GetChild(nbChildren-1).GetComponent<Button>();
    //    confirmButton.onClick.RemoveAllListeners();
    //    //confirmButton.onClick.AddListener(prisoner.ProcessFeeding);

    //    return goInventory;
    //}

    public void HideInventoryPanels()
    {
        for (int i = 0; i <Panel_Inventories.transform.childCount; i++)
        {
            Panel_Inventories.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //public void UpdatePrisonerFeedingPanel(GameObject pi)
    //{

    //    if (pi.GetComponent<Behaviour.Inventory>() == null || pi.GetComponent<PrisonerInstance>() == null) return;
    //    GameObject owner = null;
    //    Sprite associatedSprite = null;
    //    string name = "";
    //    GameObject inventoryPanel = null;
    //    int nbSlot = 0;
    //    PrisonerInstance prisoner = pi.GetComponent<PrisonerInstance>();
    //    associatedSprite = prisoner.Prisoner.AssociatedSprite;
    //    name = prisoner.Prisoner.CharacterName;
    //    inventoryPanel = prisoner.prisonerFeedingPanel;
    //    owner = prisoner.gameObject;
    //    nbSlot = prisoner.FeedingSlotsCount;
        

    //    if (owner.GetComponent<Behaviour.Inventory>() != null && owner.GetComponent<Behaviour.Inventory>().Items != null)
    //    {
    //        ItemContainer[] inventory = pi.GetComponent<Behaviour.Inventory>().Items;
    //        for (int i = 0; i < inventory.Length; i++)
    //        {
    //            GameObject currentSlot = inventoryPanel.transform.GetChild(1).GetChild(i).gameObject;
    //            if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
    //            {
    //                Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
    //            }

    //        }

    //        for (int i = 0; i < inventory.Length; i++)
    //        {
    //            if (inventory[i] != null && inventory[i].Item != null && inventory[i].Item.Id != null)
    //            {
    //                GameObject currentSlot = inventoryPanel.transform.GetChild(1).GetChild(i).gameObject;
    //                GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabItemUI);
    //                go.transform.SetParent(currentSlot.transform);
    //                go.GetComponent<ItemInstance>().ItemContainer = inventory[i];
    //                go.name = inventory[i].ToString();

    //                go.GetComponent<Image>().sprite = inventory[i].Item.InventorySprite;
    //                go.transform.localScale = Vector3.one;

    //                go.transform.position = currentSlot.transform.position;
                    

    //                if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
    //                {
    //                    go.transform.GetComponentInChildren<Text>().text = inventory[i].Quantity.ToString();
    //                }
    //                else
    //                {
    //                    go.transform.GetComponentInChildren<Text>().text = "";
    //                }
    //            }
    //        }
    //    }
    //}
    #endregion
}

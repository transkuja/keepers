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
    public GameObject goInventory;
    public GameObject goEquipement;
    public GameObject Panel_Inventories;
    public GameObject statsPanelTooltip;
    public GameObject prisonerFeedingPanel;
    // StatsPanel
    public GameObject goStats;
    public Text SelectedKeeperActionText;
    public Text DecreasedActionText;

    // Turn Panel
    [Header("Turn Panel")]
    public GameObject TurnPanel;
    public GameObject TurnButton;
    public float buttonRotationSpeed = 1.0f;

    // ActionPanel
    [Header("Action Panel")]
    public GameObject goActionPanelQ;
    private Canvas worldSpaceCanvas;

    public GameObject goMoralPanel;
    public GameObject goMentalHeathBuffOnStatPanel;
    public GameObject goHPBuffOnStatPanel;
    public GameObject goHungerBuffOnStatPanel;


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
            //bool bIsForbiden = ic.listActionContainers[i].strName == "Escort" && !GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable;
            //bIsForbiden = bIsForbiden || ic.listActionContainers[i].strName == "Unescort" && GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable;
            //bIsForbiden = bIsForbiden || ic.listActionContainers[i].strName == "Quest" && !GameManager.Instance.ListOfSelectedKeepers[0].isAbleToImproveMoral;
            //bIsForbiden = bIsForbiden || ic.listActionContainers[i].strName == "Moral" && !GameManager.Instance.ListOfSelectedKeepers[0].isAbleToImproveMoral;
            //if (!bIsForbiden)
            //{
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
            //}
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
        goStatBuff.gameObject.SetActive(true);
        if (amount < 0)
        {
            //goStatBuff.GetComponent<Image>().sprite = spriteMoralDebuff;
            goStatBuff.GetComponentInChildren<Text>().color = Color.red;
            goStatBuff.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            //goStatBuff.GetComponent<Image>().sprite = spriteMoralBuff;
            goStatBuff.GetComponentInChildren<Text>().color = Color.green;
            goStatBuff.GetComponentInChildren<Text>().text = "+ ";
        }
        goStatBuff.GetComponentInChildren<Text>().text += amount.ToString();

        StartCoroutine(BuffPanelNormalState(goStatBuff));
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

    //public void CreateShortcutPanel()
    //{
    //    if (GameManager.Instance == null) { return; }
    //    if (goShortcutKeepersPanel == null) { return; }

    //    int nbCaracters = GameManager.Instance.AllKeepersListOld.Count;

    //    for (int i = 0; i < nbCaracters; i++)
    //    {
    //        KeeperInstance currentSelectedCharacter = GameManager.Instance.AllKeepersListOld[i];

    //        Sprite associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
    //        Sprite deadSprite = currentSelectedCharacter.Keeper.DeadSprite;

    //        if (associatedSprite != null)
    //        {
    //            GameObject goKeeper = Instantiate(GameManager.Instance.PrefabUtils.PrefabShorcutCharacter, goShortcutKeepersPanel.transform);

    //            goKeeper.name = "Panel_Shortcut_" + currentSelectedCharacter.Keeper.CharacterName;
    //            goKeeper.transform.GetChild((int)PanelShortcutChildren.Image).GetComponent<Image>().sprite = associatedSprite;
    //            goKeeper.transform.localScale = Vector3.one;
    //            int n = i;
    //            goKeeper.GetComponent<Button>().onClick.AddListener(() => GoToCharacter(n));


    //        }
    //    }
    //}

    //public void GoToCharacter(int i)
    //{
    //    if ( i == -1)
    //    {
    //        // Go to prisoner
    //        Camera.main.GetComponent<CameraManager>().UpdateCameraPosition();
    //        GameManager.Instance.Ui.HideInventoryPanels();
    //    }
    //    // Next keeper
    //    else
    //    {
    //        // Do not go to keeper if dead
    //        if (!GameManager.Instance.AllKeepersListOld[i].IsAlive)
    //            return;

    //        GameManager.Instance.ClearListKeeperSelected();
    //        KeeperInstance nextKeeper = GameManager.Instance.AllKeepersListOld[i];
    //        GameManager.Instance.ListOfSelectedKeepersOld.Add(nextKeeper);
    //        nextKeeper.IsSelected = true;

    //        Camera.main.GetComponent<CameraManager>().UpdateCameraPositionOld(nextKeeper);
    //        GameManager.Instance.Ui.ShowSelectedKeeperPanel();
    //        GameManager.Instance.Ui.ClearActionPanel();
    //        GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
    //        GameManager.Instance.Ui.UpdateActionText();
    //        HideInventoryPanels();
    //    }
    //}

    public void UpdateShortcutPanel()
    {
        if (GameManager.Instance == null) { return; }
        if (goShortcutKeepersPanel == null) { return; }

        // nb Character + Ashley
        for (int i = 0; i < goShortcutKeepersPanel.transform.childCount; i++)
        {
            if (i == 0)
            {
                /*
                PrisonerInstance prisonner = GameManager.Instance.PrisonerInstance;
                // Update HP
                goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HpGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = prisonner.CurrentHp / 100.0f;
                // Update Hunger
                goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HungerGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = prisonner.CurrentHunger / 100.0f;
                */
            }
            else
            {
                PawnInstance currentCharacter = GameManager.Instance.AllKeepersList[i-1];

                if (currentCharacter != null)
                {
                    // Handle character death
                    if (!currentCharacter.GetComponent<Behaviour.Mortal>().IsAlive)
                    {

                        // Do this process once only
                        if (goShortcutKeepersPanel.transform.GetChild(i).childCount > 4)
                        {
                            // Update HP
                            goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HpGauge).GetChild(0).GetComponent<Image>().color = Color.grey;
                            goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.MentalHealthGauge).GetChild(0).GetComponent<Image>().color = Color.grey;
                            goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HungerGauge).GetChild(0).GetComponent<Image>().color = Color.grey;
                            // Update Action Points
                            Destroy(goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.ActionPoints).gameObject);

                            // Change image from alive to dead
                            goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.Image).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteSupport;

                            GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabImageUI);
                            go.GetComponent<Image>().sprite = currentCharacter.GetComponent<Behaviour.Mortal>().DeadSprite;
                            go.transform.SetParent(goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.Image).transform);
                            go.transform.localScale = Vector3.one * 0.75f;

                            // TODO fix this ugly shit again @Anthony @Rémi
                            go.GetComponent<RectTransform>().localPosition = new Vector3(37.5f, -37.5f, 0.0f);


                        }
                    }
                    // Standard UI update for alive characters
                    else
                    {
                        // Update HP
                        goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HpGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.GetComponent<Behaviour.Mortal>().CurrentHp / (float)currentCharacter.GetComponent<Behaviour.Mortal>().MaxHp;
                        // Update Hunger
                        goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HungerGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.GetComponent<Behaviour.HungerHandler>().CurrentHunger / (float)currentCharacter.GetComponent<Behaviour.HungerHandler>().Data.MaxHunger;
                        // Update MentalHealth
                        goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.MentalHealthGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.GetComponent<Behaviour.MentalHealthHandler>().CurrentMentalHealth / (float)currentCharacter.GetComponent<Behaviour.MentalHealthHandler>().Data.MaxMentalHealth;

                        // Update Action Points
                        goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.ActionPoints).gameObject.GetComponentInChildren<Text>().text = currentCharacter.GetComponent<Behaviour.Keeper>().ActionPoints.ToString();
                    }
                }
            }

        }
    }
    #endregion

    #region SelectedKeeper
    public void ShowSelectedKeeperPanel()
    {
        goSelectedKeeperPanel.SetActive(true);
    }


    public void UpdateSelectedKeeperPanel()
    {
        //if (GameManager.Instance == null) { return; }
        //if (goInventory == null) { return; }
        //if (GameManager.Instance.ListOfSelectedKeepers.Count == 0) { return; }

        //KeeperInstance currentSelectedKeeper = GameManager.Instance.ListOfSelectedKeepersOld[0];

        ///*
        //Stats
        //*/
        //Sprite associatedSprite = currentSelectedKeeper.Keeper.AssociatedSprite;
        //if (associatedSprite != null)
        //{
        //    goStats.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
        //}
        //// Hunger
        //goStats.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentSelectedKeeper.CurrentHunger / (float)currentSelectedKeeper.Keeper.MaxHunger;
        //// HP
        //goStats.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentSelectedKeeper.CurrentHp / (float)currentSelectedKeeper.Keeper.MaxHp;
        //// Mental Health
        //goStats.transform.GetChild(5).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentSelectedKeeper.CurrentMentalHealth / (float)currentSelectedKeeper.Keeper.MaxMentalHealth;
        ///*
        //Inventory
        //*/
        //// Destroy Existing
        //foreach (Slot holder in goInventory.transform.GetComponentsInChildren<Slot>())
        //{
        //    Destroy(holder.gameObject);
        //}

        //// Create Selected Keeper's one
        //goInventory.name = "Panel_Inventory" + currentSelectedKeeper.Keeper.CharacterName;
        //goInventory.GetComponent<InventoryOwner>().Owner = currentSelectedKeeper.gameObject;

        //int nbSlot = currentSelectedKeeper.Keeper.nbSlot;

        //for (int i = 0; i < nbSlot; i++)
        //{
        //    GameObject currentSlot = Instantiate(GameManager.Instance.PrefabUtils.PrefabSlotUI);

        //    ItemContainer[] inventory = currentSelectedKeeper.GetComponent<Behaviour.Inventory>().Items;
        //    if (inventory != null && inventory.Length > 0 && i < inventory.Length && inventory[i] != null && inventory[i].Item != null && inventory[i].Item.Id != null)
        //    {
        //        GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabItemUI);
        //        go.transform.SetParent(currentSlot.transform);
        //        go.GetComponent<ItemInstance>().ItemContainer = inventory[i];
        //        go.name = inventory[i].Item.ItemName;

        //        go.GetComponent<Image>().sprite = inventory[i].Item.InventorySprite;
        //        go.transform.localScale = Vector3.one;

        //        go.transform.position = currentSlot.transform.position;
        //        go.transform.SetAsFirstSibling();

        //        if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
        //        {
        //            go.transform.GetComponentInChildren<Text>().text = inventory[i].Quantity.ToString();
        //        }
        //        else
        //        {
        //            go.transform.GetComponentInChildren<Text>().text = "";
        //        }
        //    }
        //    currentSlot.transform.SetParent(goInventory.transform);
        //    currentSlot.transform.localScale = Vector3.one;
        //}

        //goInventory.GetComponent<GridLayoutGroup>().constraintCount = nbSlot;

        ///*
        //Equipement
        //*/
        //// Destroy Existing
        //ItemContainer[] equipement = currentSelectedKeeper.Equipment;
        //for (int i = 0; i < equipement.Length; i++)
        //{
        //    GameObject currentSlot = goEquipement.transform.GetChild(i).gameObject;
        //    if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
        //    {
        //        Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
        //    }

        //    goEquipement.transform.GetComponent<InventoryOwner>().Owner = currentSelectedKeeper.gameObject;
        //    if (equipement[i] != null && equipement[i].Item != null)
        //    {
        //        GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabItemUI);
        //        go.transform.SetParent(currentSlot.transform);
        //        go.GetComponent<ItemInstance>().ItemContainer = equipement[i];
        //        go.name = equipement[i].ToString();

        //        go.GetComponent<Image>().sprite = equipement[i].Item.InventorySprite;
        //        go.transform.localScale = Vector3.one;

        //        go.transform.position = currentSlot.transform.position;
        //        go.transform.SetAsFirstSibling();


        //        go.transform.GetComponentInChildren<Text>().text = "";
        //    }
        //}
    }

    public void CycleThroughKeepersButtonHandler(int direction)
    {
        if (GameManager.Instance.AllKeepersList != null)
        {
            if (GameManager.Instance.ListOfSelectedKeepers != null)
            {
                if (GameManager.Instance.ListOfSelectedKeepers.Count <= 0)
                {
                    // tmp
                    GameManager.Instance.ListOfSelectedKeepers.Add(GameManager.Instance.AllKeepersList[0]);
                }
                // Get first selected
                PawnInstance currentKeeperSelected = GameManager.Instance.ListOfSelectedKeepers[0];
                int currentKeeperSelectedIndex = GameManager.Instance.AllKeepersList.FindIndex(x => x == currentKeeperSelected);

                // Next keeper
                GameManager.Instance.ClearListKeeperSelected();
                PawnInstance nextKeeper = null;
                int nbIterations = 1;
                while (nextKeeper == null && nbIterations <= GameManager.Instance.AllKeepersList.Count)
                {
                    if ((currentKeeperSelectedIndex + direction* nbIterations) % GameManager.Instance.AllKeepersList.Count < 0)
                    {
                        nextKeeper = GameManager.Instance.AllKeepersList[GameManager.Instance.AllKeepersList.Count - 1];
                    }
                    else
                    {
                        nextKeeper = GameManager.Instance.AllKeepersList[(currentKeeperSelectedIndex + direction*nbIterations) % GameManager.Instance.AllKeepersList.Count];
                    }

                    if (!nextKeeper.GetComponent<Behaviour.Mortal>().IsAlive)
                    {
                        nextKeeper = null;
                    }
                    nbIterations++;
                }

                GameManager.Instance.ListOfSelectedKeepers.Add(nextKeeper);
                nextKeeper.GetComponent<Behaviour.Keeper>().IsSelected = true;

                //Camera.main.GetComponent<CameraManager>().UpdateCameraPosition(nextKeeper);

            }
                

        }
        GameManager.Instance.Ui.ClearActionPanel();
        HideInventoryPanels();

        GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
    }

    public void ZeroActionTextAnimation()
    {
        SelectedKeeperActionText.GetComponent<Text>().color = Color.red;
        SelectedKeeperActionText.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        StartCoroutine(TextAnimationNormalState());
    }

    private IEnumerator TextAnimationNormalState()
    {
        yield return new WaitForSeconds(1);
        SelectedKeeperActionText.GetComponent<Text>().color = Color.white;
        SelectedKeeperActionText.transform.localScale = Vector3.one;
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
        Vector3 origin = DecreasedActionText.transform.position;
        for (float f = 3.0f; f >= 0; f -= 0.1f)
        {
            Vector3 decal = new Vector3(0.0f, f, 0.0f);
            DecreasedActionText.transform.position += decal;
            yield return null;
        }
        DecreasedActionText.transform.position = origin;
        DecreasedActionText.gameObject.SetActive(false);

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

    public GameObject CreatePrisonerFeedingPanel(GameObject pi)
    {
        if (pi.GetComponent<PrisonerInstance>() == null) return null;
        GameObject owner = null;
        Sprite associatedSprite = null;
        string name = "";
        int nbSlots = 0;
        PrisonerInstance prisoner = pi.GetComponent<PrisonerInstance>();
        GameObject goInventory = Instantiate(GameManager.Instance.PrefabUtils.PrefabPrisonerFeedingUI, Panel_Inventories.transform);

        associatedSprite = prisoner.Prisoner.AssociatedSprite;
        name = prisoner.Prisoner.CharacterName;
        owner = prisoner.gameObject;
        nbSlots = prisoner.FeedingSlotsCount;

        goInventory.transform.localPosition = Vector3.zero;
        goInventory.transform.localScale = Vector3.one;
        goInventory.name = "Prisoner_" + name + "_Miam_miam";
        goInventory.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = associatedSprite;

        goInventory.transform.GetChild(1).GetComponent<InventoryOwner>().Owner = pi.gameObject;
        goInventory.SetActive(false);

        for (int i = 0; i < nbSlots; i++)
        {
            //Create Slots
            GameObject currentgoSlotPanel = Instantiate(GameManager.Instance.PrefabUtils.PrefabSlotUI, Vector3.zero, Quaternion.identity) as GameObject;
            currentgoSlotPanel.transform.SetParent(goInventory.transform.GetChild(1).transform);

            currentgoSlotPanel.transform.localPosition = Vector3.zero;
            currentgoSlotPanel.transform.localScale = Vector3.one;
            currentgoSlotPanel.name = "Slot" + i;
            currentgoSlotPanel.transform.SetAsFirstSibling();
        }
        int nbChildren = goInventory.transform.GetChild(1).childCount;
        Button confirmButton = goInventory.transform.GetChild(1).GetChild(nbChildren-1).GetComponent<Button>();
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(prisoner.ProcessFeeding);

        return goInventory;
    }

    public void HideInventoryPanels()
    {
        for (int i = 0; i <Panel_Inventories.transform.childCount; i++)
        {
            Panel_Inventories.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void UpdatePrisonerFeedingPanel(GameObject pi)
    {

        if (pi.GetComponent<Behaviour.Inventory>() == null || pi.GetComponent<PrisonerInstance>() == null) return;
        GameObject owner = null;
        Sprite associatedSprite = null;
        string name = "";
        GameObject inventoryPanel = null;
        int nbSlot = 0;
        PrisonerInstance prisoner = pi.GetComponent<PrisonerInstance>();
        associatedSprite = prisoner.Prisoner.AssociatedSprite;
        name = prisoner.Prisoner.CharacterName;
        inventoryPanel = prisoner.prisonerFeedingPanel;
        owner = prisoner.gameObject;
        nbSlot = prisoner.FeedingSlotsCount;
        

        if (owner.GetComponent<Behaviour.Inventory>() != null && owner.GetComponent<Behaviour.Inventory>().Items != null)
        {
            ItemContainer[] inventory = pi.GetComponent<Behaviour.Inventory>().Items;
            for (int i = 0; i < inventory.Length; i++)
            {
                GameObject currentSlot = inventoryPanel.transform.GetChild(1).GetChild(i).gameObject;
                if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
                {
                    Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
                }

            }

            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null && inventory[i].Item != null && inventory[i].Item.Id != null)
                {
                    GameObject currentSlot = inventoryPanel.transform.GetChild(1).GetChild(i).gameObject;
                    GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabItemUI);
                    go.transform.SetParent(currentSlot.transform);
                    go.GetComponent<ItemInstance>().ItemContainer = inventory[i];
                    go.name = inventory[i].ToString();

                    go.GetComponent<Image>().sprite = inventory[i].Item.InventorySprite;
                    go.transform.localScale = Vector3.one;

                    go.transform.position = currentSlot.transform.position;
                    

                    if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
                    {
                        go.transform.GetComponentInChildren<Text>().text = inventory[i].Quantity.ToString();
                    }
                    else
                    {
                        go.transform.GetComponentInChildren<Text>().text = "";
                    }
                }
            }
        }
    }
    #endregion
}

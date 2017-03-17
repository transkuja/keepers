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
    public GameObject keeper_inventory_prefab;
    public GameObject Panel_Inventories;
    public GameObject slotPrefab;
    public GameObject itemUI;
    public GameObject statsPanelTooltip;
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
    public GameObject baseActionImage;

    public GameObject goMoralPanel;
    // Actions
    public Sprite spriteMove;
    public Sprite spriteExplore;
    public Sprite spriteHarvest;
    public Sprite spritePick;
    public Sprite spriteTrade;
    public Sprite spriteEscort;
    public Sprite spriteUnescort;

    public Sprite spriteLoot;

    public Sprite spriteMoral;
    public Sprite spriteMoralBuff;
    public Sprite spriteMoralDebuff;

    // ShortcutPanel
    [Header("ShortcutPanel Panel")]
    public GameObject baseKeeperShortcutPanel;
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
        CreateShortcutPanel();
        CreateKeepersInventoryPanels();
    }

    public void Update()
    {
        if (GameManager.Instance != null )
        {
            if (GameManager.Instance.SelectedKeeperNeedUpdate)
            {
                UpdateSelectedKeeperPanel();
            }
            if (GameManager.Instance.ShortcutPanel_NeedUpdate)
            {
                UpdateShortcutPanel();
            }
        }
    }

    // TODO : optimise
    /*
    public void UpdateActionPanelUIOptimize(InteractionImplementer ic)
    {
        if (GameManager.Instance == null) { return; }
        if (goActionPanelQ == null) { return; }

        goActionPanelQ.GetComponent<RectTransform>().position = (Input.mousePosition) + new Vector3(30.0f, 0.0f);

        int i;

        for (i = 0; i < ic.listActionContainers.Count; ++i)
        {
            int n = i;

            if (i < listGoActionPanelButton.Count && listGoActionPanelButton[i] != null)
            {
                listGoActionPanelButton[i].name = ic.listActionContainers[i].strName;

                Button btn = listGoActionPanelButton[i].GetComponent<Button>();

                btn.onClick.RemoveAllListeners();  

                btn.onClick.AddListener(() => { ic.listActionContainers[n].action(); });

                btn.GetComponentInChildren<Text>().text = ic.listActionContainers[i].strName;
            }
            else
            {
                GameObject goAction = Instantiate(baseActionImage, goActionPanelQ.transform);

                goAction.name = ic.listActionContainers[i].strName;

                Button btn = goAction.GetComponent<Button>();

                btn.onClick.AddListener(() => { ic.listActionContainers[n].action(); });

                btn.GetComponentInChildren<Text>().text = ic.listActionContainers[i].strName;

                listGoActionPanelButton.Add(goAction);
            }
        }

        for(int k = listGoActionPanelButton.Count-1; k >= i; k--)
        {
            GameObject goTemp = listGoActionPanelButton[k];
            listGoActionPanelButton.Remove(goTemp);
            Destroy(goTemp);
        }

    }*/

    #region Action
    public void UpdateActionPanelUIQ(InteractionImplementer ic)
    {
        if (GameManager.Instance == null) { return; }
        if (goActionPanelQ == null) { return; }

        //Clear
        ClearActionPanel();

        goActionPanelQ.GetComponent<RectTransform>().position = (Input.mousePosition) + new Vector3(30.0f, 0.0f);

        // Actions
        for (int i = 0; i < ic.listActionContainers.Count; i++)
        {
            bool bIsForbiden = ic.listActionContainers[i].strName == "Escort" && !GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable;
            bIsForbiden = bIsForbiden || ic.listActionContainers[i].strName == "Unescort" && GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable;
            if (!bIsForbiden)
            {
                GameObject goAction = Instantiate(baseActionImage, goActionPanelQ.transform);
                goAction.name = ic.listActionContainers[i].strName;

                goAction.GetComponent<RectTransform>().position = (Input.mousePosition);

                Button btn = goAction.GetComponent<Button>();

                int n = i;
                int iParam = ic.listActionContainers[n].iParam;
                btn.onClick.AddListener(() => {
                    ic.listActionContainers[n].action(iParam);
                    GameManager.Instance.Ui.ClearActionPanel();
                });

                btn.transform.GetComponentInChildren<Image>().sprite = ic.listActionContainers[i].sprite;
            }
        }   
    }

    public void ClearActionPanel()
    {
        if (goActionPanelQ.GetComponentsInChildren<Image>().Length > 0)
        {
            foreach (Image ActionPanel in goActionPanelQ.GetComponentsInChildren<Image>())
            {
                Destroy(ActionPanel.gameObject);
            }
        }
    }

    public void MoralBuffActionTextAnimation(int amount)
    {
        goMoralPanel.gameObject.SetActive(true);
        // TODO : Ajouter la taille pion ? 
        goMoralPanel.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.GoTarget.transform.position);
        if (amount < 0)
        {
            goMoralPanel.GetComponent<Image>().sprite = spriteMoralDebuff;
            goMoralPanel.GetComponentInChildren<Text>().color = Color.red;
            goMoralPanel.GetComponentInChildren<Text>().text = "";
        } else
        {
            goMoralPanel.GetComponent<Image>().sprite = spriteMoralBuff;
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
    public void ToogleShortcutPanel()
    {
        goShortcutKeepersPanel.SetActive(!goShortcutKeepersPanel.activeSelf);
    }

    public void CreateShortcutPanel()
    {
        if (GameManager.Instance == null) { return; }
        if (goShortcutKeepersPanel == null) { return; }

        int nbCaracters = GameManager.Instance.AllKeepersList.Count;

        for (int i = 0; i < nbCaracters; i++)
        {
            KeeperInstance currentSelectedCharacter = GameManager.Instance.AllKeepersList[i];

            Sprite associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
            Sprite deadSprite = currentSelectedCharacter.Keeper.DeadSprite;

            if (associatedSprite != null)
            {
                GameObject goKeeper = Instantiate(baseKeeperShortcutPanel, goShortcutKeepersPanel.transform);

                goKeeper.name = "Panel_Shortcut_" + currentSelectedCharacter.Keeper.CharacterName;
                goKeeper.transform.GetChild((int)PanelShortcutChildren.Image).GetComponent<Image>().sprite = associatedSprite;
                goKeeper.transform.localScale = Vector3.one;
                int n = i;
                goKeeper.GetComponent<Button>().onClick.AddListener(() => GoToCharacter(n));

                UpdateShortcutPanel();
            }
        }
    }

    public void GoToCharacter(int i)
    {
        if ( i == -1)
        {
            // Go to prisoner
            Camera.main.GetComponent<CameraManager>().UpdateCameraPosition();
            GameManager.Instance.Ui.HideInventoryPanels();
        }
        // Next keeper
        else
        {
            // Do not go to keeper if dead
            if (!GameManager.Instance.AllKeepersList[i].IsAlive)
                return;

            GameManager.Instance.ClearListKeeperSelected();
            KeeperInstance nextKeeper = GameManager.Instance.AllKeepersList[i];
            GameManager.Instance.ListOfSelectedKeepers.Add(nextKeeper);
            nextKeeper.IsSelected = true;

            Camera.main.GetComponent<CameraManager>().UpdateCameraPosition(nextKeeper);
            GameManager.Instance.Ui.ShowSelectedKeeperPanel();
            GameManager.Instance.Ui.ClearActionPanel();
            GameManager.Instance.Ui.HideInventoryPanels();
            GameManager.Instance.SelectedKeeperNeedUpdate = true;
            GameManager.Instance.Ui.UpdateActionText();
            HideInventoryPanels();
        }
    }

    public void UpdateShortcutPanel()
    {
        if (GameManager.Instance == null) { return; }
        if (goShortcutKeepersPanel == null) { return; }

        // nb Character + Ashley
        for (int i = 0; i < goShortcutKeepersPanel.transform.childCount; i++)
        {
            if (i == 0)
            {
                PrisonerInstance prisonner = GameManager.Instance.PrisonerInstance;
                // Update HP
                goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HpGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = prisonner.CurrentHp / 100.0f;
                // Update Hunger
                goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HungerGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = prisonner.CurrentHunger / 100.0f;
            }
            else
            {
                KeeperInstance currentCharacter = GameManager.Instance.AllKeepersList[i-1];

                if (currentCharacter != null)
                {
                    // Handle character death
                    if (!currentCharacter.IsAlive)
                    {
                        // Do this process once only
                        if (goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HpGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount > 0)
                        {
                            // Update HP
                            goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HpGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                            // Update Action Points
                            goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.ActionPoints).gameObject.GetComponentInChildren<Text>().text = "0";

                            // Change image from alive to dead
                            goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.Image).GetComponent<Image>().sprite = currentCharacter.Keeper.DeadSprite;
                        }
                    }
                    // Standard UI update for alive characters
                    else
                    {
                        // Update HP
                        goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HpGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.CurrentHp / (float)currentCharacter.Keeper.MaxHp;
                        // Update Hunger
                        goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.HungerGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.CurrentHunger / (float)currentCharacter.Keeper.MaxHunger;
                        // Update MentalHealth
                        goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.MentalHealthGauge).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.CurrentMentalHealth / (float)currentCharacter.Keeper.MaxMentalHealth;

                        // Update Action Points
                        goShortcutKeepersPanel.transform.GetChild(i).GetChild((int)PanelShortcutChildren.ActionPoints).gameObject.GetComponentInChildren<Text>().text = currentCharacter.ActionPoints.ToString();
                    }
                }
            }

        }

        GameManager.Instance.ShortcutPanel_NeedUpdate = false;
    }
    #endregion

    #region SelectedKeeper
    public void ShowSelectedKeeperPanel()
    {
        goSelectedKeeperPanel.SetActive(true);
    }

    public void HideSelectedKeeperPanel()
    {
        goSelectedKeeperPanel.SetActive(false);
    }

    public void UpdateSelectedKeeperPanel()
    {
        if (GameManager.Instance == null) { return; }
        if (goInventory == null) { return; }
        if (GameManager.Instance.ListOfSelectedKeepers.Count == 0) { GameManager.Instance.SelectedKeeperNeedUpdate = false; return; }

        KeeperInstance currentSelectedKeeper = GameManager.Instance.ListOfSelectedKeepers[0];

        /*
        Stats
        */
        Sprite associatedSprite = currentSelectedKeeper.Keeper.AssociatedSprite;
        if (associatedSprite != null)
        {
            goStats.GetComponent<Image>().sprite = associatedSprite;
        }
        // Hunger
        goStats.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentSelectedKeeper.CurrentHunger / (float)currentSelectedKeeper.Keeper.MaxHunger;
        // HP
        goStats.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentSelectedKeeper.CurrentHp / (float)currentSelectedKeeper.Keeper.MaxHp;
        // Mental Health

        /*
        Inventory
        */
        // Destroy Existing
        foreach (Slot holder in goInventory.transform.GetComponentsInChildren<Slot>())
        {
            Destroy(holder.gameObject);
        }

        // Create Selected Keeper's one
        goInventory.name = "Panel_Inventory" + currentSelectedKeeper.Keeper.CharacterName;
        goInventory.GetComponent<InventoryOwner>().Owner = currentSelectedKeeper.gameObject;
        if (currentSelectedKeeper.GetComponent<Inventory>().inventory != null && currentSelectedKeeper.GetComponent<Inventory>().inventory.Length > 0)
        {
            ItemContainer[] inventory = currentSelectedKeeper.GetComponent<Inventory>().inventory;
            int nbSlot = currentSelectedKeeper.gameObject.GetComponent<Inventory>().nbSlot;
            for (int i =0; i< nbSlot; i++)
            {
                GameObject currentSlot = Instantiate(slotPrefab);
     
                if (inventory[i] != null && inventory[i].Item != null && inventory[i].Item.Id != null)
                {
                    GameObject go = Instantiate(itemUI);
                    go.transform.SetParent(currentSlot.transform);
                    go.GetComponent<ItemInstance>().ItemContainer = inventory[i];
                    go.name = inventory[i].Item.ItemName;

                    go.GetComponent<Image>().sprite = inventory[i].Item.InventorySprite;
                    go.transform.localScale = Vector3.one;

                    go.transform.position = currentSlot.transform.position;
                    go.transform.SetAsFirstSibling();

                    if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
                    {
                        go.transform.GetComponentInChildren<Text>().text = inventory[i].Quantity.ToString();
                    }
                    else
                    {
                        go.transform.GetComponentInChildren<Text>().text = "";
                    }
                }
                currentSlot.transform.SetParent(goInventory.transform);
                currentSlot.transform.localScale = Vector3.one;
            }
            
        }
        goInventory.GetComponent<GridLayoutGroup>().constraintCount = currentSelectedKeeper.gameObject.GetComponent<Inventory>().nbSlot;

        /*
        Equipement
        */
        // Destroy Existing
        ItemContainer[] equipement = currentSelectedKeeper.Equipment;
        for (int i = 0; i < equipement.Length; i++)
        {
            GameObject currentSlot = goEquipement.transform.GetChild(i).gameObject;
            if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
            {
                Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
            }

            if (equipement[i] != null && equipement[i].Item != null)
            {
                GameObject go = Instantiate(itemUI);
                go.transform.SetParent(currentSlot.transform);
                go.GetComponent<ItemInstance>().ItemContainer = equipement[i];
                go.name = equipement[i].ToString();

                go.GetComponent<Image>().sprite = equipement[i].Item.InventorySprite;
                go.transform.localScale = Vector3.one;

                go.transform.position = currentSlot.transform.position;
                go.transform.SetAsFirstSibling();


                go.transform.GetComponentInChildren<Text>().text = "";
            }
        }
        
        GameManager.Instance.SelectedKeeperNeedUpdate = false;
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
                KeeperInstance currentKeeperSelected = GameManager.Instance.ListOfSelectedKeepers[0];
                int currentKeeperSelectedIndex = GameManager.Instance.AllKeepersList.FindIndex(x => x == currentKeeperSelected);

                // Next keeper
                GameManager.Instance.ClearListKeeperSelected();
                KeeperInstance nextKeeper = null;
                int nbIterations = 1;
                while (nextKeeper == null && nbIterations < GameManager.Instance.AllKeepersList.Count)
                {
                    if ((currentKeeperSelectedIndex + direction* nbIterations) % GameManager.Instance.AllKeepersList.Count < 0)
                    {
                        nextKeeper = GameManager.Instance.AllKeepersList[GameManager.Instance.AllKeepersList.Count - 1];
                    }
                    else
                    {
                        nextKeeper = GameManager.Instance.AllKeepersList[(currentKeeperSelectedIndex + direction*nbIterations) % GameManager.Instance.AllKeepersList.Count];
                    }

                    if (!nextKeeper.IsAlive)
                    {
                        nextKeeper = null;
                    }
                    nbIterations++;
                }
                GameManager.Instance.ListOfSelectedKeepers.Add(nextKeeper);
                nextKeeper.IsSelected = true;

                Camera.main.GetComponent<CameraManager>().UpdateCameraPosition(nextKeeper);

            }
                

        }
        GameManager.Instance.Ui.ClearActionPanel();
        HideInventoryPanels();

        GameManager.Instance.SelectedKeeperNeedUpdate = true;
        GameManager.Instance.Ui.UpdateActionText();
    }

    public void UpdateActionText()
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count == 0) { return; }

        KeeperInstance currentKeeper = GameManager.Instance.ListOfSelectedKeepers[0];
        SelectedKeeperActionText.text = currentKeeper.ActionPoints.ToString();
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
        DecreasedActionText.GetComponent<Text>().text = "- " + amount.ToString();
        DecreasedActionText.gameObject.SetActive(true);
        StartCoroutine(DecreaseTextNormalState());
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
    public void CreateKeepersInventoryPanels()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            GameObject goInventoryKeeper = Instantiate(keeper_inventory_prefab, Panel_Inventories.transform);
            goInventoryKeeper.transform.localPosition = Vector3.zero;
            goInventoryKeeper.transform.GetChild(1).GetComponent<Image>().sprite = ki.Keeper.AssociatedSprite;
            goInventoryKeeper.name = "Inventory_" + ki.Keeper.CharacterName;
            goInventoryKeeper.transform.GetChild(0).GetComponent<InventoryOwner>().Owner = ki.gameObject;
            goInventoryKeeper.SetActive(false);
            int nbSlots = ki.gameObject.GetComponent<Inventory>().nbSlot;
            for (int i = 0; i < nbSlots; i++)
            {
                //Create Slots
                GameObject currentgoSlotPanel = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                currentgoSlotPanel.transform.SetParent(goInventoryKeeper.transform.GetChild(0).transform);

                currentgoSlotPanel.transform.localPosition = Vector3.zero;
                currentgoSlotPanel.transform.localScale = Vector3.one;
                currentgoSlotPanel.name = "Slot" + i;
            }

            ki.keeperInventoryPanel = goInventoryKeeper;
        }
    }

    public GameObject CreatePNJInventoryPanels(PNJInstance pi)
    {
        GameObject goPNJInventory = Instantiate(keeper_inventory_prefab, Panel_Inventories.transform);
        goPNJInventory.transform.localPosition = Vector3.zero;
        goPNJInventory.transform.GetChild(1).GetComponent<Image>().sprite = pi.Pnj.AssociatedSprite;
        goPNJInventory.name = "Inventory_" + pi.Pnj.CharacterName;
        goPNJInventory.transform.GetChild(0).GetComponent<InventoryOwner>().Owner = pi.gameObject;
        goPNJInventory.SetActive(false);
        int nbSlots = pi.gameObject.GetComponent<Inventory>().nbSlot;
        for (int i = 0; i < nbSlots; i++)
        {
            //Create Slots
            GameObject currentgoSlotPanel = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            currentgoSlotPanel.transform.SetParent(goPNJInventory.transform.GetChild(0).transform);

            currentgoSlotPanel.transform.localPosition = Vector3.zero;
            currentgoSlotPanel.transform.localScale = Vector3.one;
            currentgoSlotPanel.name = "Slot" + i;
        }

        return goPNJInventory;
    }

    public void HideInventoryPanels()
    {
        for (int i = 0; i <Panel_Inventories.transform.childCount; i++)
        {
            Panel_Inventories.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void UpdateInventoryPanel(GameObject pi)
    {
        if (pi.GetComponent<PNJInstance>() == null && pi.GetComponent<KeeperInstance>() == null) return;
        GameObject owner = null;
        GameObject inventoryPanel = null;
        if (pi.GetComponent<PNJInstance>() != null)
        {
            PNJInstance pnjInstance = pi.GetComponent<PNJInstance>();
            inventoryPanel = (pi.GetComponent<PNJInstance>() as PNJInstance).pnjInventoryPanel;
            owner = pnjInstance.gameObject;
        }
        else if (pi.GetComponent<KeeperInstance>() != null)
        {
            KeeperInstance keeperInstance = pi.GetComponent<KeeperInstance>();
            inventoryPanel = (pi.GetComponent<KeeperInstance>() as KeeperInstance).keeperInventoryPanel;
            owner = keeperInstance.gameObject;
        } else
        {
            return;
        }

        if (owner.GetComponent<Inventory>().inventory != null)
        {
            ItemContainer[] inventory = pi.GetComponent<Inventory>().inventory;
            int nbSlot = pi.GetComponent<Inventory>().nbSlot;
            for (int i = 0; i < nbSlot; i++)
            {
                GameObject currentSlot = inventoryPanel.transform.GetChild(0).GetChild(i).gameObject;
                if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
                {
                    Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
                }
                
            }

            for (int i = 0; i < nbSlot; i++)
            {
                if (inventory[i] != null && inventory[i].Item != null && inventory[i].Item.Id != null)
                {
                    GameObject currentSlot = inventoryPanel.transform.GetChild(0).GetChild(i).gameObject;
                    GameObject go = Instantiate(itemUI);
                    go.transform.SetParent(currentSlot.transform);
                    go.GetComponent<ItemInstance>().ItemContainer = inventory[i];
                    go.name = inventory[i].ToString();

                    go.GetComponent<Image>().sprite = inventory[i].Item.InventorySprite;
                    go.transform.localScale = Vector3.one;

                    go.transform.position = currentSlot.transform.position;
                    go.transform.SetAsFirstSibling();

                    if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
                    {
                        go.transform.GetComponentInChildren<Text>().text = inventory[i].Quantity.ToString();
                    } else
                    {
                        go.transform.GetComponentInChildren<Text>().text = "";
                    }
                }
            }
        }
    }
    #endregion
}

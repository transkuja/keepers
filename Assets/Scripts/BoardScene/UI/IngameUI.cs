using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class IngameUI : MonoBehaviour
{
    // KeeperPanel
    [Header("Character Panel")]
    //public GameObject goSelectedKeeperPanel;
    // Inventory
    public GameObject goSelectedKeeperPanel;
    public GameObject goInventory;
    public GameObject goEquipement;
    public GameObject keeper_inventory_prefab;
    public GameObject panel_keepers_inventory;
    public GameObject slotPrefab;
    public GameObject itemUI;
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
    public Sprite spritePick;
    public Sprite spriteTrade;
    public Sprite spriteEscort;
    public Sprite spriteUnescort;

    public Sprite spriteMoral;
    public Sprite spriteMoralBuff;
    public Sprite spriteMoralDebuff;

    // ShortcutPanel
    [Header("ShortcutPanel Panel")]
    public GameObject baseKeeperShortcutPanel;
    public GameObject goShortcutKeepersPanel;


    // Quentin
    //public List<GameObject> listGoActionPanelButton;

    [HideInInspector]
    public bool isTurnEnding = false;


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

                btn.transform.GetChild(0).GetComponentInChildren<Image>().sprite = ic.listActionContainers[i].sprite;
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

            if (associatedSprite != null)
            {
                GameObject goKeeper = Instantiate(baseKeeperShortcutPanel, goShortcutKeepersPanel.transform);

                goKeeper.name = "Panel_Shortcut_" + currentSelectedCharacter.Keeper.CharacterName;
                goKeeper.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
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
            // Go to prisonier
            Camera.main.GetComponent<CameraManager>().UpdateCameraPosition();
            GameManager.Instance.Ui.HideInventoryPanels();
        } else
        {
            // Next keeper
            GameManager.Instance.ClearListKeeperSelected();
            KeeperInstance nextKeeper = GameManager.Instance.AllKeepersList[i];
            GameManager.Instance.ListOfSelectedKeepers.Add(nextKeeper);
            nextKeeper.IsSelected = true;

            Camera.main.GetComponent<CameraManager>().UpdateCameraPosition(nextKeeper);
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
                int f = 1;
                PrisonerInstance prisonner = GameManager.Instance.PrisonerInstance;
                // Update HP
                goShortcutKeepersPanel.transform.GetChild(i).GetChild(f++).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)prisonner.CurrentHp / (float)100;
                // Update Hunger
                goShortcutKeepersPanel.transform.GetChild(i).GetChild(f).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)prisonner.CurrentHunger / (float)100;
            }
            else
            {
                KeeperInstance currentCharacter = GameManager.Instance.AllKeepersList[i-1];

                if (currentCharacter != null)
                {
                    int f = 1;
                    // Update HP
                    goShortcutKeepersPanel.transform.GetChild(i).GetChild(f++).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.CurrentHp / (float)currentCharacter.Keeper.MaxHp;
                    // Update Hunger
                    goShortcutKeepersPanel.transform.GetChild(i).GetChild(f++).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.CurrentHunger / (float)currentCharacter.Keeper.MaxHunger;
                    // Update MentalHealth
                    goShortcutKeepersPanel.transform.GetChild(i).GetChild(f++).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.CurrentMentalHealth / (float)currentCharacter.Keeper.MaxMentalHealth;

                    // Update Action Points
                    goShortcutKeepersPanel.transform.GetChild(i).GetChild(f).gameObject.GetComponentInChildren<Text>().text = currentCharacter.ActionPoints.ToString();
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
        Sprite associatedSprite = currentSelectedKeeper.Keeper.AssociatedSprite;

        foreach (Slot holder in goInventory.transform.GetComponentsInChildren<Slot>())
        {
            DestroyImmediate(holder.gameObject);
        }
        if (associatedSprite != null)
        {
            goStats.GetComponent<Image>().sprite = associatedSprite;
        }
        // Hunger
        goStats.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentSelectedKeeper.CurrentHunger / (float)currentSelectedKeeper.Keeper.MaxHunger;
        // HP
        goStats.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentSelectedKeeper.CurrentHp / (float)currentSelectedKeeper.Keeper.MaxHp;
        // Mental Health
        goStats.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentSelectedKeeper.CurrentMentalHealth / (float)currentSelectedKeeper.Keeper.MaxMentalHealth;

        // Inventory
        goInventory.name = "Panel_Inventory" + currentSelectedKeeper.Keeper.CharacterName;
        goInventory.GetComponent<InventoryOwner>().Owner = currentSelectedKeeper.gameObject;

        if (currentSelectedKeeper.GetComponent<Inventory>().inventory != null && currentSelectedKeeper.GetComponent<Inventory>().inventory.Length > 0)
        {
            ItemContainer[] inventory = currentSelectedKeeper.GetComponent<Inventory>().inventory;
            for (int i =0; i< currentSelectedKeeper.gameObject.GetComponent<Inventory>().nbSlot; i++)
            {
                GameObject currentSlot = Instantiate(slotPrefab);

                if (inventory[i] != null && inventory[i].Item != null)
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
                }
                currentSlot.transform.SetParent(goInventory.transform);
                currentSlot.transform.localScale = Vector3.one;
            }
            
        }
        goInventory.GetComponent<GridLayoutGroup>().constraintCount = currentSelectedKeeper.gameObject.GetComponent<Inventory>().nbSlot;
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
                if ((currentKeeperSelectedIndex + direction) % GameManager.Instance.AllKeepersList.Count < 0)
                {
                    nextKeeper = GameManager.Instance.AllKeepersList[GameManager.Instance.AllKeepersList.Count - 1];
                }
                else
                {
                    nextKeeper = GameManager.Instance.AllKeepersList[(currentKeeperSelectedIndex + direction) % GameManager.Instance.AllKeepersList.Count];
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
            GameObject goInventoryKeeper = Instantiate(keeper_inventory_prefab, panel_keepers_inventory.transform);
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
        }
    }

    internal void ShowInventoryPanels()
    {
        panel_keepers_inventory.transform.GetChild(GameManager.Instance.GoTarget.transform.GetSiblingIndex()).gameObject.SetActive(true);
    }

    internal void HideInventoryPanels()
    {
        for (int i = 0; i <GameManager.Instance.AllKeepersList.Count; i++)
        {
            panel_keepers_inventory.transform.GetChild(i).gameObject.SetActive(false);
        }
  
    }

    internal void UpdateKeeperInventoryPanel()
    {
        if (GameManager.Instance == null) { return; }
        if (panel_keepers_inventory == null) { return; }

        foreach( KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            foreach (ItemInstance holder in panel_keepers_inventory.transform.GetChild(ki.gameObject.transform.GetSiblingIndex()).transform.GetChild(0).transform.GetComponentsInChildren<ItemInstance>())
            {
                DestroyImmediate(holder.gameObject);
            }
            if (ki.GetComponent<Inventory>().inventory != null)
            {
                ItemContainer[] inventory = ki.GetComponent<Inventory>().inventory;

                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] != null && inventory[i].Item != null)
                    {

                        GameObject currentSlot = panel_keepers_inventory.transform.GetChild(ki.gameObject.transform.GetSiblingIndex()).transform.GetChild(0).transform.GetChild(i).gameObject;
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
                        }
                    }
                }

            }
        }
        
        
    }
    #endregion
}

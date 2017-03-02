using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class IngameUI : MonoBehaviour
{
    // KeeperPanel
    [Header("Character Panel")]
    public GameObject goSelectedKeeperPanel;

    // Turn Panel
    [Header("Turn Panel")]
    public GameObject TurnPanel;
    public GameObject TurnButton;
    public float buttonRotationSpeed = 1.0f;

    // ActionPanel
    [Header("Action Panel")]
    public GameObject goActionPanelQ;
    public GameObject baseActionImage;

 
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
                    GameObject.Find("IngameUI").GetComponent<IngameUI>().ClearActionPanel();
                });

                btn.GetComponentInChildren<Text>().text = ic.listActionContainers[i].strName;
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
    #endregion

    #region Turn
    // TODO: @Rémi bouton à corriger (on ne doit pas pouvoir cliquer 2x de suite)
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

                UpdateShortcutPanel();
            }
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
                //Prisoner prisonner = GameManager.Instance.Prisonner ;
                //// Update HP
                //goShortcutKeepersPanel.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)prisonner.CurrentHp / (float)prisonner.MaxHp;
                //// Update Hunger
                //goShortcutKeepersPanel.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)prisonner.ActualHunger / (float)prisonner.MaxHunger;
            }
            else
            {
                Keeper currentCharacter = GameManager.Instance.AllKeepersList[i-1].Keeper;

                if (currentCharacter != null)
                {
                    int f = 1;
                    // Update HP
                    goShortcutKeepersPanel.transform.GetChild(i).GetChild(f++).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.CurrentHp / (float)currentCharacter.MaxHp;
                    // Update Hunger
                    goShortcutKeepersPanel.transform.GetChild(i).GetChild(f++).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.ActualHunger / (float)currentCharacter.MaxHunger;
                    // Update MentalHealth
                    goShortcutKeepersPanel.transform.GetChild(i).GetChild(f++).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentCharacter.ActualMentalHealth / (float)currentCharacter.MaxMentalHealth;

                    // Update Action Points
                    goShortcutKeepersPanel.transform.GetChild(i).GetChild(f).gameObject.GetComponentInChildren<Text>().text = currentCharacter.ActionPoints.ToString();
                }
            }
        }

        GameManager.Instance.ShortcutPanel_NeedUpdate = false;
    }
    #endregion

    #region SelectedKeeper
    public void UpdateSelectedKeeperPanel()
    {
        if (GameManager.Instance == null) { return; }
        if (goSelectedKeeperPanel == null) { return; }

        KeeperInstance currentSelectedKeeper = GameManager.Instance.ListOfSelectedKeepers[0];

        Sprite associatedSprite = currentSelectedKeeper.Keeper.AssociatedSprite;
        //if (associatedSprite != null)
        //{
        //    GameObject goKeeperSlected = Instantiate(baseCharacterImage, goSelectedKeeperPanel.transform);

        //    goKeeper.name = currentSelectedCharacter.Keeper.CharacterName + ".Panel";
        //    goKeeper.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
        //    goKeeper.transform.localScale = Vector3.one;

        //}


        //if (inventoryManager.GetComponent<InventoryManager>().InventoryPanel != null)
        //{
        //    foreach (ItemInstance holder in inventoryManager.GetComponent<InventoryManager>().InventoryPanel.transform.GetComponentsInChildren<ItemInstance>())
        //    {
        //        DestroyImmediate(holder.gameObject);
        //    }
        //    for (int i = 0; i < inventoryManager.GetComponent<KeeperInstance>().Inventory.Length; i++)
        //    {
        //        GameObject currentSlot = inventoryManager.GetComponent<InventoryManager>().SlotList[i];
        //        if (inventoryManager.GetComponent<KeeperInstance>().Inventory[i] != null)
        //        {
        //            GameObject go = Instantiate(itemUI);
        //            go.transform.SetParent(currentSlot.transform);




        //            go.GetComponent<ItemInstance>().itemContainer = inventoryManager.GetComponent<KeeperInstance>().Inventory[i];

        //            go.GetComponent<Image>().sprite = inventoryManager.GetComponent<KeeperInstance>().Inventory[i].item.spirte;
        //            go.transform.localScale = Vector3.one;

        //            go.transform.position = currentSlot.transform.position;
        //            go.transform.SetAsFirstSibling();

        //            go.transform.GetComponentInChildren<Text>().text = inventoryManager.GetComponent<KeeperInstance>().Inventory[i].quantity.ToString();
        //        }
        //    }
        //}


        //GameManager.Instance.SelectedKeeperNeedUpdate = false;
    }
    #endregion
}

﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class IngameUI : MonoBehaviour
{
    // KeeperPanel
    [Header("Character Panel")]
    // Inventory
    public GameObject goSelectedKeeperPanel;
    public GameObject Panel_Inventories;

    // Turn Panel
    [Header("Turn Panel")]
    public GameObject TurnPanel;
    public GameObject TurnButton;
    public float buttonRotationSpeed = 1.0f;

    // ActionPanel
    [Header("Action Panel")]
    [SerializeField]
    private GameObject goActionPanelQ;
    [SerializeField]
    private Canvas worldSpaceCanvas;

    // ContentQuest
    public GameObject goContentQuestParent;
    // Content Panneau
    public GameObject goContentPanneauParent;

    // ShortcutPanel
    [Header("ShortcutPanel")]
    public GameObject goShortcutKeepersPanel;

    [Header("Confirmation Panel")]
    public GameObject goConfirmationPanel;

    public GameObject itemSplitter;

    private Color redClaire;
    private Color greenClaire;


    private Color redFonce;
    private Color greenFonce;


    // Quentin
    [Header("Foreign UI Components")]
    [SerializeField] private UIIconFeedback uiIconFeedBack;

    [HideInInspector]
    public bool isTurnEnding = false;

    [Header("Battle UI")]
    // Battle ui
    public GameObject battleUI;


    public GameObject tooltipItem;
    public GameObject tooltipJauge;
    public GameObject tooltipAction;

    public void Start()
    {

        redClaire = new Color32(0xde, 0x4e, 0x4e, 0xFF);
        greenClaire = new Color32(0x1c, 0xc7, 0x56, 0xFF);

        redFonce = new Color32(0x85, 0x08, 0x08, 0xFF);
        greenFonce = new Color32(0x03, 0x60, 0x41, 0xFF);

    }


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

    public GameObject GoActionPanelQ
    {
        get
        {
            return goActionPanelQ;
        }

        set
        {
            goActionPanelQ = value;
        }
    }
    #endregion

    public void ClearUiOnTurnEnding()
    {
        GameManager.Instance.Ui.ClearActionPanel();
        GameManager.Instance.Ui.HideInventoryPanels();
        ResetActionPanelPosition();
        // A character is selected
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

    public void ResetActionPanelPosition()
    {
        if (goActionPanelQ != null)
            goActionPanelQ.transform.parent.SetParent(transform);
    }

    public void ResetIngameUI()
    {
        for (int i = 0; i < goContentQuestParent.transform.childCount; i++)
        {
            Destroy(goContentQuestParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < goContentPanneauParent.transform.childCount; i++)
        {
            Destroy(goContentPanneauParent.transform.GetChild(i).gameObject);
        }
        ResetActionPanelPosition();
        GameManager.Instance.Ui.TurnButton.transform.parent.gameObject.SetActive(false);

    }

    #region Action
    public void UpdateActionPanelUIQ(InteractionImplementer ic)
    {
    
        if (GameManager.Instance == null) { return; }
        if (goActionPanelQ == null) { return; }

        if (GameManager.Instance.ListOfSelectedKeepers.Count == 0) { return; }

        //Clear
        ClearActionPanel();

        if (goShortcutKeepersPanel != null)
        {
            foreach (PawnInstance pi in GameManager.Instance.AllKeepersList)
            {
                if (pi.GetComponent<Behaviour.Keeper>() != null && pi.GetComponent<Behaviour.Mortal>().IsAlive)
                {
                    pi.GetComponent<Behaviour.Keeper>().ShorcutUI.GetComponent<Button>().interactable = false;
                    pi.GetComponent<Behaviour.Keeper>().ShorcutUI.GetComponent<Image>().raycastTarget = false;

                    int k = 3;
                    if (pi.Data.Behaviours[(int)BehavioursEnum.Archer] == true)
                        k = 2;
                    pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k++).GetComponent<Image>().CrossFadeAlpha(0.3f, 1, true);
                    for (int j = 0; j < pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k).childCount; j++)
                    {
                        pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k).GetChild(j).GetComponent<Image>().CrossFadeAlpha(0.3f, 1, true);
                    }

                    pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(++k).GetComponent<Image>().CrossFadeAlpha(0.3f, 1, true);

                    if (pi.GetComponent<Behaviour.Mortal>() != null)
                        pi.GetComponent<Behaviour.Mortal>().ShortcutHPUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 1, true);
                    if (pi.GetComponent<Behaviour.HungerHandler>() != null)
                        pi.GetComponent<Behaviour.HungerHandler>().ShortcutHungerUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 1, true);
                    if(pi.GetComponent<Behaviour.MentalHealthHandler>() != null)
                        pi.GetComponent<Behaviour.MentalHealthHandler>().ShortcutMentalHealthUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 1, true);
                }

 
            }

            if (GameManager.Instance.PrisonerInstance != null && GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>() != null && GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>().IsAlive)
            {
                GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.GetComponent<Button>().interactable = false;
                GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.GetComponent<Image>().raycastTarget = false;

                GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.transform.GetChild(2).GetComponent<Image>().CrossFadeAlpha(0.3f, 1, true);
                GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.transform.GetChild(4).GetComponent<Image>().CrossFadeAlpha(0.3f, 1, true);
                if (GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>() != null)
                    GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>().ShortcutHPUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 1, true);
                if (GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.HungerHandler>() != null)
                    GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.HungerHandler>().ShortcutHungerUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 1, true);
            }
        }

        goActionPanelQ.GetComponent<RectTransform>().localPosition = Vector3.zero;

        // Actions

        for (int i = 0; i < ic.listActionContainers.Count; i++)
        {
            bool bIsForbiden = ic.listActionContainers[i].strName == "Quest" && !GameManager.Instance.GetFirstSelectedKeeper().Data.Behaviours[(int)BehavioursEnum.CanSpeak] && GameManager.Instance.GoTarget.GetComponent<Behaviour.Monster>() == null;
            bIsForbiden = bIsForbiden || (ic.listActionContainers[i].strName == "Talk" && !GameManager.Instance.GetFirstSelectedKeeper().Data.Behaviours[(int)BehavioursEnum.CanSpeak] && GameManager.Instance.GoTarget.GetComponent<Behaviour.Keeper>() != null);
            bIsForbiden = bIsForbiden || (ic.listActionContainers[i].strName == "Heal" && !GameManager.Instance.GetFirstSelectedKeeper().Data.Behaviours[(int)BehavioursEnum.Healer]);
            if (!bIsForbiden)
            {
                GameObject goAction = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabActionUI, goActionPanelQ.transform);
                goAction.name = ic.listActionContainers[i].strName;

                goAction.GetComponent<RectTransform>().localPosition = Vector3.zero;
                goAction.GetComponent<RectTransform>().localRotation = Quaternion.identity;

                Button btn = goAction.GetComponent<Button>();

                int n = i;

                int costActionTmp = ic.listActionContainers[i].costAction;

                if (ic.listActionContainers[i].strName == "Explore" && GameManager.Instance.GetFirstSelectedKeeper().Data.Behaviours[(int)BehavioursEnum.Explorateur] == true)
                {
                    costActionTmp -= 1;
                }
                int nbActionRestantKeeper = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>().ActionPoints;
                if (costActionTmp > nbActionRestantKeeper)
                {
                    goAction.GetComponent<Image>().color = Color.grey;
                }
                if(ic.listActionContainers[i].strName == "Can't End Game")
                {
                    goAction.GetComponent<Image>().color = Color.grey;
                }
                if (costActionTmp > 0)
                {
                    GameObject actionPoints = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabActionPoint, goAction.transform);
                    actionPoints.transform.localScale = Vector3.one;
                    actionPoints.transform.localPosition = Vector3.zero;
                    actionPoints.transform.localRotation = Quaternion.identity;

                    actionPoints.transform.GetComponentInChildren<Text>().text = costActionTmp + "/" + nbActionRestantKeeper;
                    if (costActionTmp > nbActionRestantKeeper)
                    {
                        actionPoints.transform.GetComponentInChildren<Text>().color = redClaire;
                        actionPoints.transform.GetComponentInChildren<Text>().GetComponent<Outline>().effectColor = redFonce;

                    } else
                    {
                        actionPoints.transform.GetComponentInChildren<Text>().color = greenClaire;
                        actionPoints.transform.GetComponentInChildren<Text>().GetComponent<Outline>().effectColor = greenFonce;
                    }
                }

                int iParam = ic.listActionContainers[n].iParam;
                btn.onClick.AddListener(() =>
                {
                    ic.listActionContainers[n].action(iParam);
                    GameManager.Instance.Ui.ClearActionPanel();
                });

                btn.transform.GetComponentInChildren<Image>().sprite = ic.listActionContainers[i].sprite;
                btn.transform.GetComponentInChildren<Image>().transform.localScale = Vector3.one;
            }
        }
        worldSpaceCanvas.transform.SetParent(GameManager.Instance.GoTarget.GetComponent<Interactable>().Feedback);
        worldSpaceCanvas.transform.localPosition = Vector3.zero;
        worldSpaceCanvas.GetComponent<WorldspaceCanvasCameraAdapter>().RecalculateActionCanvas(Camera.main);
    }

    public void ClearActionPanel()
    {
        if (goShortcutKeepersPanel != null)
        {
            foreach (PawnInstance pi in GameManager.Instance.AllKeepersList)
            {
                if (pi.GetComponent<Behaviour.Keeper>() != null && pi.GetComponent<Behaviour.Mortal>().IsAlive)
                {
                    pi.GetComponent<Behaviour.Keeper>().ShorcutUI.GetComponent<Button>().interactable = true;
                    pi.GetComponent<Behaviour.Keeper>().ShorcutUI.GetComponent<Image>().raycastTarget = true;

                    int k = 3;
                    if (pi.Data.Behaviours[(int)BehavioursEnum.Archer] == true)
                        k = 2;
                    pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k++).GetComponent<Image>().CrossFadeAlpha(1f, 0, true);
                    for (int j = 0; j < pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k).childCount; j++)
                    {
                        pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k).GetChild(j).GetComponent<Image>().CrossFadeAlpha(1f, 0, true);
                    }

                    pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(++k).GetComponent<Image>().CrossFadeAlpha(1f, 0, true);

                    if (pi.GetComponent<Behaviour.Mortal>() != null)
                        pi.GetComponent<Behaviour.Mortal>().ShortcutHPUI.GetComponentInChildren<Image>().CrossFadeAlpha(1f, 0, true);
                    if (pi.GetComponent<Behaviour.HungerHandler>() != null)
                        pi.GetComponent<Behaviour.HungerHandler>().ShortcutHungerUI.GetComponentInChildren<Image>().CrossFadeAlpha(1f, 0, true);
                    if (pi.GetComponent<Behaviour.MentalHealthHandler>() != null)
                        pi.GetComponent<Behaviour.MentalHealthHandler>().ShortcutMentalHealthUI.GetComponentInChildren<Image>().CrossFadeAlpha(1f, 0, true);
                }


            }

            if (GameManager.Instance.PrisonerInstance != null && GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>() != null && GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>().IsAlive)
            {
                GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.GetComponent<Button>().interactable = true;
                GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.GetComponent<Image>().raycastTarget = true;

                GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.transform.GetChild(2).GetComponent<Image>().CrossFadeAlpha(1f, 0, true);
                GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.transform.GetChild(4).GetComponent<Image>().CrossFadeAlpha(1f, 0, true);

                if (GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>() != null)
                    GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>().ShortcutHPUI.GetComponentInChildren<Image>().CrossFadeAlpha(1.0f, 0, true);
                if(GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.HungerHandler>() != null)
                    GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.HungerHandler>().ShortcutHungerUI.GetComponentInChildren<Image>().CrossFadeAlpha(1.0f, 0, true);

            }
        }

        if (goActionPanelQ != null && goActionPanelQ.transform.childCount > 0)
        {
            foreach (Image ActionPanel in goActionPanelQ.GetComponentsInChildren<Image>())
            {
                Destroy(ActionPanel.gameObject);
            }
        }
        if(tooltipAction != null && tooltipAction.activeSelf)
            tooltipAction.SetActive(false);
    }
    #endregion

    #region Turn
    public void EndTurn()
    {
        if (!(GameManager.Instance.CurrentState == GameState.InPause))
        {
            if (!isTurnEnding)
            {
                if (GameManager.Instance.CurrentState != GameState.InTuto)
                    GameManager.Instance.CurrentState = GameState.InPause;
                AnimateButtonOnClick();
            }
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

    public void UpdateDay()
    {
       TurnPanel.transform.GetChild(2).GetComponentInChildren<Text>().text = Translater.EndTurnButtonText() + " " + GameManager.Instance.NbTurn;
    }
    #endregion

    #region ShortcutPanel
    public void ToggleShortcutPanel()
    {
        goShortcutKeepersPanel.SetActive(!goShortcutKeepersPanel.activeSelf);
        if (GameManager.Instance.Ui.goActionPanelQ != null && GameManager.Instance.Ui.goActionPanelQ.transform.childCount > 0)
        {
            if (goShortcutKeepersPanel != null)
            {
                foreach (PawnInstance pi in GameManager.Instance.AllKeepersList)
                {
                    if (pi.GetComponent<Behaviour.Keeper>() != null && pi.GetComponent<Behaviour.Mortal>().IsAlive)
                    {
                        pi.GetComponent<Behaviour.Keeper>().ShorcutUI.GetComponent<Button>().interactable = false;
                        pi.GetComponent<Behaviour.Keeper>().ShorcutUI.GetComponent<Image>().raycastTarget = false;

                        int k = 3;
                        if (pi.Data.Behaviours[(int)BehavioursEnum.Archer] == true)
                            k = 2;
                        pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k++).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                        for (int j = 0; j < pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k).childCount; j++)
                        {
                            pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(k).GetChild(j).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                        }

                        pi.GetComponent<Behaviour.Keeper>().ShorcutUI.transform.GetChild(++k).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);

                        if (pi.GetComponent<Behaviour.Mortal>() != null)
                            pi.GetComponent<Behaviour.Mortal>().ShortcutHPUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                        if (pi.GetComponent<Behaviour.HungerHandler>() != null)
                            pi.GetComponent<Behaviour.HungerHandler>().ShortcutHungerUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                        if (pi.GetComponent<Behaviour.MentalHealthHandler>() != null)
                            pi.GetComponent<Behaviour.MentalHealthHandler>().ShortcutMentalHealthUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                    }


                }

                if (GameManager.Instance.PrisonerInstance != null && GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>() != null && GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>().IsAlive)
                {
                    GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.GetComponent<Button>().interactable = false;
                    GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.GetComponent<Image>().raycastTarget = false;

                    GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.transform.GetChild(2).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                    GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().ShorcutUI.transform.GetChild(4).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);

                    if (GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>() != null)
                        GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>().ShortcutHPUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                    if (GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.HungerHandler>() != null)
                        GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.HungerHandler>().ShortcutHungerUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                }
            }
        }
    }
    #endregion

    #region SelectedKeeper

    public void ZeroActionTextAnimation(Behaviour.Keeper ki)
    {
        foreach(Image i in ki.ShortcutActionPointUi.GetComponentsInChildren<Image>())
        {
            i.color = Color.red;
            //i.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        foreach (Image i in ki.SelectedActionPointsUI.GetComponentsInChildren<Image>())
        {
            i.color = Color.red;
            //i.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        StartCoroutine(TextAnimationNormalState(ki));
    }

    private IEnumerator TextAnimationNormalState(Behaviour.Keeper ki)
    {
        yield return new WaitForSeconds(1);

        foreach (Image i in ki.ShortcutActionPointUi.GetComponentsInChildren<Image>())
        {
            i.color = Color.white;
            //i.transform.localScale = Vector3.one;
        }

        foreach (Image i in ki.SelectedActionPointsUI.GetComponentsInChildren<Image>())
        {
            i.color = Color.white;
            //i.transform.localScale = Vector3.one;
        }

        yield return null;
    }

    public void DecreaseActionTextAnimation(Behaviour.Keeper ki, int amount)
    {
        ki.SelectedActionPointsUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().text = "- " + amount.ToString();
        ki.SelectedActionPointsUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().gameObject.SetActive(true);
        StartCoroutine(DecreaseTextNormalState(ki));
    }

    private IEnumerator DecreaseTextNormalState(Behaviour.Keeper ki)
    {
        Vector3 origin = ki.SelectedActionPointsUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().transform.position;
        for (float f = 3.0f; f >= 0; f -= 0.1f)
        {
            Vector3 decal = new Vector3(0.0f, f, 0.0f);
            ki.SelectedActionPointsUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().transform.position += decal;
            yield return null;
        }
        ki.SelectedActionPointsUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().transform.position = origin;
        ki.SelectedActionPointsUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().gameObject.SetActive(false);
        yield return null;
    }
    #endregion

    public void AutoEscortAshley()
    {
        PawnInstance ashley = GameManager.Instance.PrisonerInstance;
        Tile ashleyTile = ashley.CurrentTile;
        TileManager.Instance.KeepersOnTile[ashleyTile][0].GetComponent<Behaviour.Keeper>().GoListCharacterFollowing.Add(ashley.gameObject);
        Behaviour.Escortable escortComponent = ashley.GetComponent<Behaviour.Escortable>();
        escortComponent.escort = TileManager.Instance.KeepersOnTile[ashleyTile][0].GetComponent<Behaviour.Keeper>();
        escortComponent.IsEscorted = true;
        escortComponent.ActivateIconNearEscort();
    }

    #region Keepers_Inventory
    public void HideInventoryPanels()
    {
        for (int i = 0; i < Panel_Inventories.transform.childCount; i++)
        {
            Panel_Inventories.transform.GetChild(i).gameObject.SetActive(false);
        }

        foreach (PawnInstance k in GameManager.Instance.AllKeepersList) { 
            for (int i = 0; i < k.GetComponent<Behaviour.Inventory>().Data.NbSlot; i++)
            {
                k.GetComponent<Behaviour.Inventory>().SelectedInventoryPanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
        }
    }
    #endregion
}

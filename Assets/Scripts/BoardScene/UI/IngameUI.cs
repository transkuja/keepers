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


    public GameObject mouseFollower;
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
                GameObject goAction = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabActionUI, goActionPanelQ.transform);
                goAction.name = ic.listActionContainers[i].strName;

                goAction.GetComponent<RectTransform>().localPosition = Vector3.zero;
                goAction.GetComponent<RectTransform>().localRotation = Quaternion.identity;

                Button btn = goAction.GetComponent<Button>();

                int n = i;

                int nbActionRestantKeeper = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>().ActionPoints;
                if (ic.listActionContainers[i].costAction > nbActionRestantKeeper)
                {
                    goAction.GetComponent<Image>().color = Color.grey;
                }

                if (ic.listActionContainers[i].costAction > 0)
                {
                    GameObject actionPoints = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabActionPoint, goAction.transform);
                    actionPoints.transform.localScale = Vector3.one;
                    actionPoints.transform.localPosition = Vector3.zero;
                    actionPoints.transform.localRotation = Quaternion.identity;

                    actionPoints.transform.GetComponentInChildren<Text>().text = ic.listActionContainers[i].costAction + "/" + nbActionRestantKeeper;
                    if (ic.listActionContainers[i].costAction > nbActionRestantKeeper)
                        actionPoints.transform.GetComponentInChildren<Text>().color = Color.red;

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

    public void UpdateActionPanelUIForBattle(Behaviour.Fighter selectedFighter)
    {
        if (goActionPanelQ == null) { return; }
        if (GameManager.Instance.ListOfSelectedKeepers.Count == 0) { return; }

        //Clear
        ClearActionPanel();

        goActionPanelQ.GetComponent<RectTransform>().localPosition = Vector3.zero;

        // Actions
        for (int i = 0; i < selectedFighter.BattleInteractions.listActionContainers.Count; i++)
        {
            GameObject goAction = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabActionUI, goActionPanelQ.transform);
            goAction.name = selectedFighter.BattleInteractions.listActionContainers[i].strName;

            goAction.GetComponent<RectTransform>().localPosition = Vector3.zero;
            goAction.GetComponent<RectTransform>().localRotation = Quaternion.identity;
            if (goAction.name == "Attack")
                goAction.AddComponent<ShowAssociatedDice>();

            Button btn = goAction.GetComponent<Button>();

            int n = i;

            int iParam = selectedFighter.BattleInteractions.listActionContainers[n].iParam;
            btn.onClick.AddListener(() => {
                selectedFighter.BattleInteractions.listActionContainers[n].action(iParam);
                GameManager.Instance.Ui.ClearActionPanel();
            });

            btn.transform.GetComponentInChildren<Image>().sprite = selectedFighter.BattleInteractions.listActionContainers[i].sprite;
            btn.transform.GetComponentInChildren<Image>().transform.localScale = Vector3.one;
        }

        worldSpaceCanvas.transform.SetParent(selectedFighter.InteractionsPosition);
        worldSpaceCanvas.transform.localPosition = Vector3.zero;
        worldSpaceCanvas.GetComponent<WorldspaceCanvasCameraAdapter>().RecalculateActionCanvas(Camera.main);
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
        if(tooltipAction.activeSelf)
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
    #endregion

    #region ShortcutPanel
    public void ToggleShortcutPanel()
    {
        goShortcutKeepersPanel.SetActive(!goShortcutKeepersPanel.activeSelf);
    }
    #endregion

    #region SelectedKeeper

    public void ZeroActionTextAnimation(Behaviour.Keeper ki)
    {
        foreach(Image i in ki.ShorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).GetComponentsInChildren<Image>())
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

        foreach (Image i in ki.ShorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).GetComponentsInChildren<Image>())
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

    #region Keepers_Inventory
    public void HideInventoryPanels()
    {
        for (int i = 0; i <Panel_Inventories.transform.childCount; i++)
        {
            Panel_Inventories.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion
}

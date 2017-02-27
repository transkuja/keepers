using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class IngameUI : MonoBehaviour
{
    // CharacterPanel
    [Header("Character Panel")]
    public GameObject CharacterPanel;
    public GameObject baseCharacterImage;

    // Turn Panel
    [Header("Turn Panel")]
    public GameObject TurnPanel;
    public GameObject TurnButton;
    public float buttonRotationSpeed = 1.0f;

    // CharacterPanel
    [Header("Action Panel")]
    public GameObject goActionPanelQ;
    public GameObject baseActionImage;

    // Quentin
    //public List<GameObject> listGoActionPanelButton;

    public bool isTurnEnding = false;

    public void Awake()
    {
        UpdateCharacterPanelUI();

        //listGoActionPanelButton = new List<GameObject>();
    }

    public void Update()
    {
        if (GameManager.Instance != null )
        {
            if (GameManager.Instance.CharacterPanelIngameNeedUpdate)
            {
                UpdateCharacterPanelUI();
            }
        }
    }

    void UpdateCharacterPanelUI()
    {
        if (GameManager.Instance == null){ return; }
        if (CharacterPanel == null) { return; }

        for (int i = 0; i < CharacterPanel.transform.childCount; i++)
        {
            Destroy(CharacterPanel.transform.GetChild(i).gameObject);
        }

        int nbCaracters = GameManager.Instance.AllKeepersList.Count;
        for (int i = 0; i < nbCaracters; i++)
        {
            KeeperInstance currentSelectedCharacter = GameManager.Instance.AllKeepersList[i];

            Sprite associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
            if (associatedSprite != null)
            {
                GameObject goKeeper = Instantiate(baseCharacterImage, CharacterPanel.transform);

                goKeeper.name = currentSelectedCharacter.Keeper.CharacterName + ".Panel";
                goKeeper.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
                goKeeper.transform.localScale = Vector3.one;

                // Stats
                // HP
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponentInChildren<Text>().text = "HP: " + currentSelectedCharacter.Keeper.Hp.ToString();
                // MP
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponentInChildren<Text>().text = "MP: " + currentSelectedCharacter.Keeper.Mp.ToString();
                // Strengh
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponentInChildren<Text>().text = "S: " + currentSelectedCharacter.Keeper.Strength.ToString();
                // Defense
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.GetComponentInChildren<Text>().text = "D: " + currentSelectedCharacter.Keeper.Defense.ToString();
                // Intelligence
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(4).gameObject.GetComponentInChildren<Text>().text = "I: " + currentSelectedCharacter.Keeper.Intelligence.ToString();
                // Spirit
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(5).gameObject.GetComponentInChildren<Text>().text = "S: " + currentSelectedCharacter.Keeper.Spirit.ToString();

                // Status
                // Hunger
                goKeeper.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Text>().text = "H: " + currentSelectedCharacter.Keeper.ActualHunger.ToString();
                // MentalHealth
                goKeeper.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponentInChildren<Text>().text = "MH: " + currentSelectedCharacter.Keeper.ActualMentalHealth.ToString();
            }
        }

        GameManager.Instance.CharacterPanelIngameNeedUpdate = false;
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

    // TODO: @Rémi bouton à corriger (on ne doit pas pouvoir cliquer 2x de suite)
    public void EndTurn()
    {
        if (!isTurnEnding)
        {
            AnimateButtonOnClick();
            EventManager.EndTurnEvent();
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
}

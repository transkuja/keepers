using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintWoman : MonoBehaviour {

    PawnInstance instance;
    private GameObject goHint;
    public string commeSurLePanneau;

    void Awake()
    {
        instance = GetComponent<PawnInstance>();
    }

    // Use this for initialization
    void Start () {
        if (instance.Data.Behaviours[(int)BehavioursEnum.CanSpeak])
            instance.Interactions.Add(new Interaction(Hint), 0, "Talk", GameManager.Instance.SpriteUtils.spriteMoral);


        goHint = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabContentQuestUI, GameManager.Instance.Ui.goContentQuestParent.transform);
        goHint.transform.localPosition = Vector3.zero;
        goHint.transform.localScale = Vector3.one;
        goHint.transform.GetChild(1).GetComponent<Image>().sprite = instance.Data.AssociatedSprite;
        goHint.transform.GetChild(3).GetComponentInChildren<Text>().text = commeSurLePanneau;
        goHint.transform.GetChild(4).GetComponent<Text>().text = "Hint";
        Button close = goHint.transform.GetChild(0).GetComponent<Button>();
        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(CloseBox);
        Button validate = goHint.transform.GetChild(goHint.transform.childCount - 3).GetComponent<Button>();
        if (validate != null)
        {
            validate.onClick.RemoveAllListeners();
            validate.onClick.AddListener(CloseBox);
        }
    }

    public void Hint(int _i)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            GameManager.Instance.Ui.goContentQuestParent.SetActive(true);
            OpenBox();
        }
    }

    void AcceptQuest()
    {
        GameManager.Instance.Ui.goContentQuestParent.SetActive(false);
        CloseBox();
    }
    void OpenBox()
    {
        goHint.SetActive(true);
        GameManager.Instance.CurrentState = GameState.InPause;
    }
    void CloseBox()
    {
        GameManager.Instance.CurrentState = GameState.Normal;
        goHint.SetActive(false);
    }
}

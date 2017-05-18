using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestReminder : MonoBehaviour {

    enum State
    {
        hidden,
        showing = 1,
        shown,
        hidding = -1
    }

    private State state = State.shown;
    private QuestManager qm;
    private RectTransform rt;
    private float fOffsetX;
    private float fPosY;
    private float fLerp = 0;
    private Dictionary<QuestSystem.Quest, GameObject> dicQuestReminder;


    [SerializeField] private float fSpeed = 5;
    [SerializeField] private GameObject prefabQuestElement;
    [SerializeField] private GameObject prefabQuestObjective;
    [SerializeField] private Transform container;
    [SerializeField] private Image imgButton;



    // Use this for initialization
    void Start () {
        qm = GameManager.Instance.QuestManager;
        rt = GetComponent<RectTransform>();
        fOffsetX = rt.sizeDelta.x;
        dicQuestReminder = new Dictionary<QuestSystem.Quest, GameObject>();
        fPosY = rt.anchoredPosition.y;
        //rt.anchoredPosition = Vector3.one * fOffsetX;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Refresh();
        }

        if (state == State.hidding || state == State.showing)
        {
            UpdatePosition();
        }
	}

    public void Init()
    {
        GameObject newObjective;
        GameObject newQuest = Instantiate(prefabQuestElement, container);
        newQuest.transform.localScale = Vector3.one;
        newQuest.GetComponentInChildren<Text>().text = qm.MainQuest.Information.Title;
        for (int i = 0; i < qm.MainQuest.Objectives.Count; i++)
        {
            newObjective = Instantiate(prefabQuestObjective, newQuest.transform);
            newObjective.transform.localScale = Vector3.one;
            newObjective.GetComponentInChildren<Text>().text = qm.MainQuest.Objectives[i].Title;
        }
        dicQuestReminder.Add(qm.MainQuest, newQuest);

        for (int i = 0; i < qm.ActiveQuests.Count; i++)
        {
            newQuest = Instantiate(prefabQuestElement, container);
            newQuest.transform.localScale = Vector3.one;
            newQuest.GetComponentInChildren<Text>().text = qm.ActiveQuests[i].Information.Title;
            //newQuest.GetComponentInChildren<Image>().sprite = qm.ActiveQuests[i].Information
            for (int j = 0; j < qm.ActiveQuests[i].Objectives.Count; j++)
            {
                newObjective = Instantiate(prefabQuestObjective, newQuest.transform);
                newObjective.transform.localScale = Vector3.one;
                //newObjective.GetComponentInChildren<Text>().text = qm.ActiveQuests[i].Objectives[j].Title;
                newObjective.GetComponentInChildren<Text>().text = qm.ActiveQuests[i].Objectives[j].Description;
            }
            dicQuestReminder.Add(qm.ActiveQuests[i], newQuest);
        }
    }

    public void Refresh()
    {
        if (!dicQuestReminder.ContainsKey(qm.MainQuest))
        {
            AddQuest(qm.MainQuest);
        }

        for(int i =0; i< qm.ActiveQuests.Count; i++)
        {
            if (!dicQuestReminder.ContainsKey(qm.ActiveQuests[i]))
            {
                AddQuest(qm.ActiveQuests[i]);
            }
        }
    }

    public void AddQuest()
    {

    }

    public void Toogle()
    {
        if(state == State.hidden)
        {
            Show();
        }
        else if(state == State.shown)
        {
            Hide();
        }
    }

    public void Show(bool force = false)
    {
        if(state != State.shown && state != State.showing)
        {
            if(state == State.hidden)
            {
                fLerp = 0;
                state = State.showing;
            }
        }
    }

    public void Hide(bool force = false)
    {
        if (state != State.hidden && state != State.hidding)
        {
            if (state == State.shown)
            {
                fLerp = 1;
                state = State.hidding;
            }
        }
    }

    private void UpdatePosition()
    {
        fLerp += Time.unscaledDeltaTime * fSpeed * (int)state;
        rt.anchoredPosition = Vector3.Lerp(new Vector3(fOffsetX, fPosY,0), new Vector3(0,fPosY,0), fLerp);

        if(state == State.showing && fLerp >= 1)
        {
            state = State.shown;
            imgButton.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (state == State.hidding && fLerp <= 0)
        {
            state = State.hidden;
            imgButton.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0,0);
        }
    }

    private void AddQuest(QuestSystem.Quest q)
    {
        GameObject newQuest = Instantiate(prefabQuestElement, container);
        newQuest.transform.localScale = Vector3.one;
        newQuest.GetComponentInChildren<Text>().text = q.Information.Title;

        GameObject newObjective;
        for (int i = 0; i < q.Objectives.Count; i++)
        {
            newObjective = Instantiate(prefabQuestObjective, newQuest.transform);
            newObjective.transform.localScale = Vector3.one;
            newObjective.GetComponentInChildren<Text>().text = q.Objectives[i].Title;
        }
        dicQuestReminder.Add(q, newQuest);
    }

    public void AddObjective()
    {

    }
}

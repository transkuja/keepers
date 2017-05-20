

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestReminder : MonoBehaviour {
    private static bool bNeedRefresh = false;
    enum State
    {
        hidden,
        showing = 1,
        shown,
        hidding = -1
    }

    private State state = State.hidden;
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

    [SerializeField] private Sprite iconAshley;
    [SerializeField] private Sprite iconRaspberry;
    [SerializeField] private Sprite iconDucky;

    public static bool BNeedRefresh
    {
        get
        {
            return bNeedRefresh;
        }

        set
        {
            bNeedRefresh = value;
        }
    }

    // Use this for initialization
    void Start () {
        qm = GameManager.Instance.QuestManager;
        rt = GetComponent<RectTransform>();
        fOffsetX = rt.sizeDelta.x - 20;
        dicQuestReminder = new Dictionary<QuestSystem.Quest, GameObject>();
        fPosY = rt.anchoredPosition.y;
	}

	// Update is called once per frame
	void Update () {
        if (state == State.hidding || state == State.showing)
        {
            updatePosition();
        }

        if (bNeedRefresh)
        {
            Refresh();
            bNeedRefresh = false;

        }

        if (Input.GetKeyDown(KeyCode.M) && DebugControls.isDebugModeActive)
        {
            bNeedRefresh = true;
        }
    }

    public void Refresh()
    {
        if (!dicQuestReminder.ContainsKey(qm.MainQuest))
        {
            addQuest(qm.MainQuest);
            refreshQuest(qm.MainQuest);
        }
        else
        {
            refreshQuest(qm.MainQuest);
        }

        for(int i =0; i< qm.ActiveQuests.Count; i++)
        {
            if (!dicQuestReminder.ContainsKey(qm.ActiveQuests[i]))
            {
                addQuest(qm.ActiveQuests[i]);
                refreshQuest(qm.ActiveQuests[i]);
            }
            else
            {
                refreshQuest(qm.ActiveQuests[i]);
            }
        }

        //for(int i = 0; i < qm.CompletedQuests.Count; i++)
        //{
        //    dicQuestReminder[qm.CompletedQuests[i]].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
        //}
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
        Refresh();
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

    public void CallRefresh()
    {
        bNeedRefresh = true;
    }

    private void updatePosition()
    {
        if( state == State.showing && fLerp <= 0)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }


        fLerp += Time.unscaledDeltaTime * fSpeed * (int)state;
        rt.anchoredPosition = Vector3.Lerp(new Vector3(-fOffsetX, fPosY,0), new Vector3(0,fPosY,0), fLerp);

        if (state == State.showing && fLerp >= 1)
        {
            state = State.shown;
        }

        else if (state == State.hidding && fLerp <= 0)
        {
            state = State.hidden;
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }


    private void addQuest(QuestSystem.Quest q)
    {
        GameObject newQuest = Instantiate(prefabQuestElement, container);
        newQuest.transform.localScale = Vector3.one;
        switch (q.Identifier.ID)
        {
            case "main_quest_01":
                newQuest.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = iconAshley;
                break;
            case "side_quest_01":
                newQuest.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = iconRaspberry;
                break;
            case "side_quest_ducklings_01":
                newQuest.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = iconDucky;
                break;
        }
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

    private void refreshQuest(QuestSystem.Quest q)
    {
        bool allComplete = true;
        for (int j = 0; j < q.Objectives.Count; j++)
        {
            dicQuestReminder[q].transform.GetChild(1+j).GetChild(0).GetChild(0).gameObject.SetActive(q.Objectives[j].IsComplete);
            if (!q.Objectives[j].IsComplete)
            {
                allComplete = false;
            }
        }
        if (allComplete)
        {
            dicQuestReminder[q].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
        }
    }
}


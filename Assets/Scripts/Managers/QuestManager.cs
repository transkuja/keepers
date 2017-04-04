using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestManager : MonoBehaviour
{

    private static QuestManager instance = null;

    public QuestDeck CurrentQuestDeck;

    public List<Quest> AvailableQuests;
    public List<Quest> CurrentQuests;

    public static QuestManager Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CurrentQuestDeck = new QuestDeck();
        CurrentQuestDeck.Id = 0;
    }

    public Quest GetQuestByID(int id)
    {
        foreach (Quest quest in AvailableQuests)
        {
            if (quest.Identifier.ID == id)
                return quest;
        }
        return null;
    }
}

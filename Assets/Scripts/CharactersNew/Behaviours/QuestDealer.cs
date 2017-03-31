using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

namespace Behaviour
{
    public class QuestDealer : MonoBehaviour
    {
        PawnInstance instance;
        public GameObject goQuest;
        public GameObject prefabContentQuest;

        // Array of arrays. use: QuestIDs[CurrentQuestDeck] -> Array of Quests IDs for this component in the current quest deck
        // QuestIDs[CurrentQuestDeck].QuestIDs[0] -> The first Quest ID that correspond to this Quest dealer ans the current quest deck
        [SerializeField]
        QuestIDArray[] PossibleQuests;

        Quest questToGive;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            questToGive = QuestManager.Instance.GetQuestByID(PossibleQuests[QuestManager.Instance.CurrentQuestDeck.Id].QuestIDs[0]);
            goQuest = Instantiate(prefabContentQuest, GameManager.Instance.Ui.goContentQuestParent.transform);
            goQuest.transform.localPosition = Vector3.zero;
            goQuest.transform.localScale = Vector3.one;
            instance.Interactions.Add(new Interaction(Quest), 1, "Quest", GameManager.Instance.SpriteUtils.spriteQuest);
        }

        public void Quest(int _i = 0)
        {
            if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
            {
                int costAction = instance.Interactions.Get("Quest").costAction;
                if (GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Keeper>().ActionPoints >= costAction)
                {
                    GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Keeper>().ActionPoints -= (short)costAction;
                    goQuest.SetActive(true);
                }
                else
                {
                    GameManager.Instance.Ui.ZeroActionTextAnimation();
                }
            }
        }
    }
}
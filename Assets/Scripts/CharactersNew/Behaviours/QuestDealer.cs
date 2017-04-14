using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using UnityEngine.UI;

namespace Behaviour
{
    public class QuestDealer : MonoBehaviour
    {
        PawnInstance instance;
        public GameObject goQuest;
        //public GameObject prefabContentQuest;

        // Array of arrays. use: QuestIDs[CurrentQuestDeck] -> Array of Quests IDs for this component in the current quest deck
        // QuestIDs[CurrentQuestDeck].QuestIDs[0] -> The first Quest ID that correspond to this Quest dealer and the current quest deck
        [SerializeField]
        QuestIDArray[] PossibleQuests;
        int currentQuestIndex;
        Quest questToGive;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            currentQuestIndex = 0;

            goQuest = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabContentQuestUI, GameManager.Instance.Ui.goContentQuestParent.transform);
            goQuest.transform.localPosition = Vector3.zero;
            goQuest.transform.localScale = Vector3.one;
            instance.Interactions.Add(new Interaction(Quest), 1, "Quest", GameManager.Instance.SpriteUtils.spriteQuest);

            //questToGive = QuestManager.Instance.GetQuestByID(PossibleQuests[QuestManager.Instance.CurrentQuestDeck.Id].QuestIDs[0]);
            questToGive = GameManager.Instance.MainQuest;
            //Test
            questToGive = GameManager.Instance.MainQuest;
            if (questToGive == null)
            {
                Debug.Log("Quest Not Found");
            }
            else
                InitializeQuest();
            
        }

        public void InitializeQuest()
        {
            questToGive.Init(new QuestIdentifier(questToGive.Identifier.ID, gameObject), questToGive.Information, questToGive.Objectives);
            goQuest.transform.GetChild(goQuest.transform.childCount-1).GetComponent<Text>().text = questToGive.Information.Title;
            goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponentInChildren<Text>().text = questToGive.Information.Dialog;
            Button validate = goQuest.transform.GetChild(goQuest.transform.childCount - 3).GetComponent<Button>();
            if(validate != null)
            {
                validate.onClick.RemoveAllListeners();
                validate.onClick.AddListener(AcceptQuest);
            }
            else
            {
                Debug.Log("Didn't find the button. You probably fucked up with the children selection.");
            }
        }

        public void Quest(int _i = 0)
        {
            if (GameManager.Instance.ListOfSelectedKeepers.Count > 0 && questToGive != null)
            {
                Debug.Log("Hey");
                int costAction = instance.Interactions.Get("Quest").costAction;
                if (GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Keeper>().ActionPoints >= costAction)
                {
                    GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Keeper>().ActionPoints -= (short)costAction;
                    
                    questToGive = null;
                    
                    goQuest.SetActive(true);
                    GameManager.Instance.Ui.goContentQuestParent.SetActive(true);
                }
                else
                {
                    GameManager.Instance.Ui.ZeroActionTextAnimation(GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>());
                }
            }
        }

        void AcceptQuest()
        {
            QuestManager.Instance.CurrentQuests.Add(questToGive);
            GameManager.Instance.Ui.goContentQuestParent.SetActive(false);
            goQuest.SetActive(false);
        }

        public void ChangeQuestToGive(int questIDIndex)
        {
            questToGive = QuestManager.Instance.GetQuestByID(PossibleQuests[QuestManager.Instance.CurrentQuestDeck.Id].QuestIDs[questIDIndex]);
            if (questToGive == null)
            {
                Debug.Log("Quest Not Found");
            }
            else
            {
                InitializeQuest();
            }
            
        }
    }
}
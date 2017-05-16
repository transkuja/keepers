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
        QuestDealerFeedbackUpdater feedbackUpdater;
        //public GameObject prefabContentQuest;

        // Array of arrays. use: QuestIDs[CurrentQuestDeck] -> Array of Quests IDs for this component in the current quest deck
        // QuestIDs[CurrentQuestDeck].QuestIDs[0] -> The first Quest ID that correspond to this Quest dealer and the current quest deck
        //public QuestIDArray[] PossibleQuests;

        //public int currentQuestIndex;
        private Quest questToGive;

        public Quest QuestToGive
        {
            get
            {
                return questToGive;
            }

            set
            {
                questToGive = value;
                if(questToGive != null)
                {
                    if(feedbackUpdater != null)
                    {
                        feedbackUpdater.Init(questToGive);
                    }
                }
            }
        }

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
            feedbackUpdater = GetComponent<QuestDealerFeedbackUpdater>();
        }

        public void Init()
        {
            goQuest = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabContentQuestUI, GameManager.Instance.Ui.goContentQuestParent.transform);
            goQuest.transform.localPosition = Vector3.zero;
            goQuest.transform.localScale = Vector3.one;
            goQuest.transform.GetChild(1).GetComponent<Image>().sprite = instance.Data.AssociatedSprite;
            Button close = goQuest.transform.GetChild(0).GetComponent<Button>();
            close.onClick.RemoveAllListeners();
            close.onClick.AddListener(CloseBox);
            GetComponent<Interactable>().Interactions.Add(new Interaction(Quest), 0, "Quest", GameManager.Instance.SpriteUtils.spriteQuest);
        }

        void BuildQuestPanel()
        {

            goQuest.transform.GetChild(goQuest.transform.childCount - 1).GetComponent<Text>().text = QuestToGive.Information.Title;
            goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponentInChildren<Text>().text = QuestToGive.Information.Dialog;
            goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
            Button validate = goQuest.transform.GetChild(goQuest.transform.childCount - 3).GetComponent<Button>();
            if (validate != null)
            {
                validate.onClick.RemoveAllListeners();
                validate.onClick.AddListener(AcceptQuest);
            }
            else
            {
                Debug.Log("Didn't find the button. You probably fucked up with the children selection.");
            }
        }

        void BuildAlreadyActivePanel()
        {
            goQuest.transform.GetChild(goQuest.transform.childCount - 1).GetComponent<Text>().text = QuestToGive.Information.Title;
            goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponentInChildren<Text>().text = QuestToGive.Information.HintDialog;
            goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
            Button validate = goQuest.transform.GetChild(goQuest.transform.childCount - 3).GetComponent<Button>();
            if (validate != null)
            {
                validate.onClick.RemoveAllListeners();
                validate.onClick.AddListener(CloseBox);
            }
        }

        void BuildEndQuestPanel()
        {
            goQuest.transform.GetChild(goQuest.transform.childCount - 1).GetComponent<Text>().text = QuestToGive.Information.Title;
            goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponentInChildren<Text>().text = QuestToGive.Information.EndDialog;
            goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
            Button validate = goQuest.transform.GetChild(goQuest.transform.childCount - 3).GetComponent<Button>();
            if (validate != null)
            {
                validate.onClick.RemoveAllListeners();
                validate.onClick.AddListener(EndQuest);
            }
        }

        public void Quest(int _i = 0)
        {
            //AudioManager.Instance.PlayOneShot(AudioManager.Instance.quackSound);
            if (GameManager.Instance.ListOfSelectedKeepers.Count > 0 && QuestToGive != null)
            {
                int costAction = GetComponent<Interactable>().Interactions.Get("Quest").costAction;
                if (GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Keeper>().ActionPoints >= costAction)
                {
                    GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Keeper>().ActionPoints -= (short)costAction;
                    if(GameManager.Instance.QuestManager.ActiveQuests.Contains(QuestToGive))
                    {
                        if(QuestToGive.CheckIfComplete())
                        {
                            //Si la quête a été complétée
                            BuildEndQuestPanel();
                            OpenBox();
                        }
                        else
                        {
                            //Si la quête a déjà été acceptée
                            BuildAlreadyActivePanel();
                            OpenBox();
                        }
                    }
                    else
                    {
                        BuildQuestPanel();
                        OpenBox();
                    }
                    
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
            if (feedbackUpdater != null)
                feedbackUpdater.ActivateQuestWaitingFeedback();
            QuestUtility.AcceptQuest(QuestToGive);
            GameManager.Instance.Ui.goContentQuestParent.SetActive(false);
            CloseBox();
        }
        void OpenBox()
        {
            goQuest.SetActive(true);
            GameManager.Instance.CurrentState = GameState.InPause;
        }
        void CloseBox()
        {
            GameManager.Instance.CurrentState = GameState.Normal;
            goQuest.SetActive(false);
        }

        void EndQuest()
        {
            if (feedbackUpdater != null)
                feedbackUpdater.DisableFeedbacks();
            QuestUtility.CompleteQuest(QuestToGive);
            CloseBox();
            // Do things?
        }
    }
}
﻿using System.Collections;
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
        public string level;
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

            feedbackUpdater = GetComponent<QuestDealerFeedbackUpdater>();
        }

        public void Init()
        {
            instance = GetComponent<PawnInstance>();
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
            //if(GameManager.Instance.GetFirstSelectedKeeper().Data.PawnId == "lucky")
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
                        if (feedbackUpdater != null)
                        {
                            feedbackUpdater.Init(questToGive);
                        }
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
            QuestUtility.AcceptQuest(questToGive);
            Debug.Log(questToGive.Objectives[0].Title);
            GameManager.Instance.Ui.goContentQuestParent.SetActive(false);
            CloseBox();
        }
        void OpenBox()
        {
            if (GetComponent<PawnInstance>().Data.PawnId.Contains("duck") && GameManager.Instance.ListOfSelectedKeepers.Count > 0 
                && !GameManager.Instance.GetFirstSelectedKeeper().Data.Behaviours[(int)BehavioursEnum.CanSpeak])
            {
                if(GameManager.Instance.GetFirstSelectedKeeper().Data.PawnId == "lucky")
                {
                    goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponentInChildren<Text>().text =
                        "Quack? Quack! Quack quack quack.\n-Meow? meow mew meow!\n-Quack !\n*Both cats and the mommy duck start to dance happily* ";

                } else
                {
                    goQuest.transform.GetChild(goQuest.transform.childCount - 2).GetComponentInChildren<Text>().text =
                        "Quack? Quack! Quack quack quack.\n-Woof? Waf woof waf!\n-Quack !\n*The dog and the mommy duck start to dance happily* ";
                }

                AudioManager.Instance.PlayDuckChatting();
            }
            goQuest.SetActive(true);
            GameManager.Instance.CurrentState = GameState.InPause;
        }
        void CloseBox()
        {
            // Anim dance
            if (GetComponent<PawnInstance>().Data.PawnId.Contains("duck") && GameManager.Instance.ListOfSelectedKeepers.Count > 0
                && !GameManager.Instance.GetFirstSelectedKeeper().Data.Behaviours[(int)BehavioursEnum.CanSpeak])
            {
                GetComponent<AnimatedPawn>().Anim.SetTrigger("dance");
                GameManager.Instance.GetFirstSelectedKeeper().GetComponent<AnimatedPawn>().Anim.SetTrigger("dance");
            }
            GameManager.Instance.CurrentState = GameState.Normal;
            goQuest.SetActive(false);
        }

        void EndQuest()
        {
            CloseBox();
            if (feedbackUpdater != null)
                feedbackUpdater.DisableFeedbacks();
            QuestUtility.CompleteQuest(QuestToGive);
            
            // Do things?
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class QuestDealer : MonoBehaviour
    {
        PawnInstance instance;
        public GameObject goQuest;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();

            goQuest = Instantiate(GameManager.Instance.PrefabUtils.PrefabContentQuestUI, GameManager.Instance.Ui.goContentQuestParent.transform);
            goQuest.transform.localPosition = Vector3.zero;
            goQuest.transform.localScale = Vector3.one;
        }

        void Start()
        {
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
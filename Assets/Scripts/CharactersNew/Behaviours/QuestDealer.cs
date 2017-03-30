using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class QuestDealer : MonoBehaviour
    {

        public GameObject prefabContentQuest;
        public GameObject goQuest;

        void Awake()
        {
            goQuest = Instantiate(prefabContentQuest, GameManager.Instance.Ui.goContentQuestParent.transform);
            goQuest.transform.localPosition = Vector3.zero;
            goQuest.transform.localScale = Vector3.one;
        }
    }
}
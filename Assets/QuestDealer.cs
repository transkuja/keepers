using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDealer : MonoBehaviour {

    public GameObject prefabContentQuest;
    public GameObject goQuest;

    void Awake()
    {
        goQuest = Instantiate(prefabContentQuest, GameManager.Instance.Ui.baseQuestPanel.transform);
        goQuest.transform.localPosition = Vector3.zero;
    }
}

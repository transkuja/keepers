using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimationButtonClick : MonoBehaviour, IPointerEnterHandler {

    // Update is called once per frame
    public void HandleAnimation() {
        GameManager.Instance.Ui.isTurnEnding = false;
        EventManager.EndTurnEvent();
        GetComponent<Animator>().enabled = false;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            if (GameManager.Instance.AllKeepersList[i].ActionPoints > 0)
            {
                GameManager.Instance.Ui.goShortcutKeepersPanel.SetActive(true);
                // Actions
                GameManager.Instance.Ui.goShortcutKeepersPanel.transform.GetChild(i + 1).GetChild(4).GetComponent<Text>().color = Color.green;
                GameManager.Instance.Ui.goShortcutKeepersPanel.transform.GetChild(i + 1).GetChild(4).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                StartCoroutine(TextAnimationNormalState(i));
                return;
            }
        }
    }

    private IEnumerator TextAnimationNormalState(int _i)
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.Ui.goShortcutKeepersPanel.transform.GetChild(_i + 1).GetChild(4).GetComponent<Text>().color = Color.white;
        GameManager.Instance.Ui.goShortcutKeepersPanel.transform.GetChild(_i + 1).GetChild(4).transform.localScale = Vector3.one;
        yield return null;
    }

}

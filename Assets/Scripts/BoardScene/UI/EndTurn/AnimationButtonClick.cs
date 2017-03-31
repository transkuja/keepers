using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimationButtonClick : MonoBehaviour, IPointerEnterHandler {

    private Light directionalLight;
    private float temps;

    public void Start()
    {
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        AnimationClip ac = null;
        for (int i = 0; i < GetComponent<Animator>().runtimeAnimatorController.animationClips.Length; i++)
        {
            if (GetComponent<Animator>().runtimeAnimatorController.animationClips[i].name == "NewTurn")
            {
                ac = GetComponent<Animator>().runtimeAnimatorController.animationClips[i];
                break;
            }
      
        }
        temps = ac.length;
    }


    public void ChangeLight()
    {
        directionalLight.gameObject.transform.SetParent(transform);
        StartCoroutine(GodsWork());
   
    }

    private IEnumerator GodsWork()
    {

        directionalLight.intensity = Mathf.Clamp(directionalLight.intensity, 0, 1);
        for (float f = temps/2; f >= 0; f -= Time.deltaTime)
        {
            //valeur = 0
            directionalLight.intensity -= Time.deltaTime *2;
            yield return null;
        }

        for (float f = temps / 2; f >= 0; f -= Time.deltaTime)
        {
            // valeur = 1
            directionalLight.intensity += Time.deltaTime*2;
            yield return null;
        }
        directionalLight.transform.SetParent(null);
                    yield return null;
    }

    // Update is called once per frame
    public void HandleAnimation() {
        GameManager.Instance.Ui.TurnPanel.transform.GetChild(2).GetComponentInChildren<Text>().text = "Day " + ++GameManager.Instance.NbTurn;

        GameManager.Instance.Ui.isTurnEnding = false;
        EventManager.EndTurnEvent();
        GetComponent<Animator>().enabled = false;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            PawnInstance pi = GameManager.Instance.AllKeepersList[i];
            if (pi.GetComponent<Behaviour.Keeper>() != null && pi.GetComponent<Behaviour.Mortal>().IsAlive)
            {
                if (pi.GetComponent<Behaviour.Keeper>().ActionPoints > 0)
                {
                    GameManager.Instance.Ui.goShortcutKeepersPanel.SetActive(true);
                    // Actions

                    // If keeper is dead this will be destroy
                    pi.GetComponent<Behaviour.Keeper>().shorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).GetComponent<Text>().color = Color.green;
                    pi.GetComponent<Behaviour.Keeper>().shorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).GetComponent<Text>().transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
          
                    StartCoroutine(TextAnimationNormalState(i));
                    return;
                }
            }
        }
    }

    private IEnumerator TextAnimationNormalState(int _i)
    {
        yield return new WaitForSeconds(1);
        PawnInstance pi = GameManager.Instance.AllKeepersList[_i];
        pi.GetComponent<Behaviour.Keeper>().shorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).GetComponent<Text>().color = Color.white;
        pi.GetComponent<Behaviour.Keeper>().shorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).GetComponent<Text>().transform.localScale = Vector3.one;
        yield return null;
    }
}

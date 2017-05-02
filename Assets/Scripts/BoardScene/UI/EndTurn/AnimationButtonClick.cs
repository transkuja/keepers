using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimationButtonClick : MonoBehaviour {

    private Light directionalLight;
    private float temps;
    private Color baseColor;
    private Color nightColor;
    private float baseExposure;
    private float nightExposure;

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
        baseColor = new Color32(0xFF, 0xF4, 0xD6, 0xFF);
        nightColor = new Color32(0x01, 0x16, 0x56, 0xFF);
        baseExposure = RenderSettings.skybox.GetFloat("_Exposure");
        nightExposure = 1.0f;
    }


    public void ChangeLight()
    {
        directionalLight.gameObject.transform.SetParent(transform);
        StartCoroutine(GodsWork());
   
    }

    private IEnumerator GodsWork()
    {
        float ElapsedTime = 0.0f;
        directionalLight.intensity = Mathf.Clamp(directionalLight.intensity, 0, 1);
        for (float f = temps/2; f >= 0; f -= Time.deltaTime)
        {
            //valeur = 0

            ElapsedTime += Time.deltaTime;
            //directionalLight.intensity -= Time.deltaTime *2;
            directionalLight.color = Color.Lerp(baseColor, nightColor, (ElapsedTime / (temps / 2)));
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(baseExposure, nightExposure, (ElapsedTime / (temps / 2))));
            yield return null;
        }


        ElapsedTime = 0.0f;
        for (float f = temps / 2; f >= 0; f -= Time.deltaTime)
        {
            // valeur = 1
            ElapsedTime += Time.deltaTime;
            //directionalLight.intensity += Time.deltaTime*2;
            directionalLight.color = Color.Lerp(nightColor, baseColor, (ElapsedTime / (temps / 2)));
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(nightExposure, baseExposure, (ElapsedTime / (temps / 2))));
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
        GameManager.Instance.CurrentState = GameState.Normal;

    }

}

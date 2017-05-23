using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerMoodFeedback : MonoBehaviour {
    public bool needsToPlayHunger = false;
    public bool needsToPlayLowMood = false;

    public bool isPlaying = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!isPlaying)
        {
            if(needsToPlayHunger)
            {
                StartCoroutine(PlayHungerFeedback());
                needsToPlayHunger = false;
            }
            else if(needsToPlayLowMood)
            {
                StartCoroutine(PlayLowMoodFeedback());
                needsToPlayLowMood = false;
            }
        }
        else
        {
            if(GameManager.Instance.CurrentState == GameState.InBattle)
            {

            }
        }
	}

    public void HideFeedbacks()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ActivateFeedbacks()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public IEnumerator PlayHungerFeedback()
    {
        isPlaying = true;
        ActivateFeedbacks();
        Image imgChild = transform.GetChild(1).GetComponent<Image>();
        imgChild.sprite = GameManager.Instance.SpriteUtils.hungerFeedback;

        yield return new WaitForSeconds(3.0f);

        HideFeedbacks();
        isPlaying = false;
    }


    public IEnumerator PlayLowMoodFeedback()
    {
        isPlaying = true;
        ActivateFeedbacks();
        Image imgChild = transform.GetChild(1).GetComponent<Image>();
        imgChild.sprite = GameManager.Instance.SpriteUtils.lowMoodFeedback;

        yield return new WaitForSeconds(3.0f);

        HideFeedbacks();
        isPlaying = false;
    }
}

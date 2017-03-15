using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconFeedback : MonoBehaviour {

    [SerializeField] float closeThreshold;
    [SerializeField] float farThreshold;

    [SerializeField] GameObject goPanelIcon;

    bool bIsDisplaying = false;

    bool bIsFarEnough = false;

    // Update is called once per frame
    void Update () {
        if (bIsDisplaying)
        {
            UpdateFeedBack();
        }
        if (!CheckFarEnough())
        {
            DisableFeedback();
        }
	}

    bool CheckFarEnough()
    {
        float fValue = GameManager.Instance.CameraManager.FZoomLerp;

        if (bIsFarEnough)
        {
            if(fValue > closeThreshold)
            {
                bIsFarEnough = false;
                return false;
            }
            return true;
        }
        else
        {
            if(fValue < farThreshold)
            {
                bIsFarEnough = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void TriggerFeedback(Sprite sprite)
    {
        goPanelIcon.SetActive(true);
        goPanelIcon.GetComponentInChildren<Image>().sprite = sprite;
        goPanelIcon.transform.position = Input.mousePosition;
        bIsDisplaying = true;
    }

    void UpdateFeedBack()
    {
        goPanelIcon.transform.position = Input.mousePosition;
    }

    public void DisableFeedback()
    {
        goPanelIcon.SetActive(false);
    }
}

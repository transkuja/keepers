using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGlowCardInfo : MonoBehaviour
{

    private MenuUI menuUI;
    private GameObject goCardInfo;

    public GameObject GoCardInfo
    {
        get
        {
            return goCardInfo;
        }

        set
        {
            goCardInfo = value;
        }
    }

    public void Start()
    {
        menuUI = GameObject.FindObjectOfType<MenuUI>();
    }

    private void OnMouseExit()
    {
        if (menuUI.cardsInfoAreReady && goCardInfo != null)
        {
            if (!goCardInfo.transform.GetChild(0).GetComponent<GlowObjectCmd>().isActiveAndEnabled)
                goCardInfo.transform.GetChild(0).GetComponent<GlowObjectCmd>().enabled = false;
            goCardInfo.transform.GetChild(0).GetComponent<GlowObjectCmd>().UpdateColor(false);
        }

    }

    private void OnMouseOver()
    {
        if (menuUI.cardsInfoAreReady && goCardInfo.transform.GetChild(0) != null)
        {
            if (!goCardInfo.transform.GetChild(0).GetComponent<GlowObjectCmd>().isActiveAndEnabled)
                goCardInfo.transform.GetChild(0).GetComponent<GlowObjectCmd>().enabled = true;
            goCardInfo.transform.GetChild(0).GetComponent<GlowObjectCmd>().UpdateColor(true);
        }

    }

    //public void OnMouseEnter()
    //{
    //    if (menuUI.cardsInfoAreReady && goCardInfo != null)
    //    {
    //        AudioManager.Instance.PlayOneShot(AudioManager.Instance.deselectSound);
    //    }
    //}
}

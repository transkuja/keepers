using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour {

    public MenuUI menuUI;

    //bool bIsOver = false;
    private bool needToBeShown = false;
    private bool needToBeHide = false;

    public void Start()
    {
        menuUI = GameObject.FindObjectOfType<MenuUI>();
    }

    public bool NeedToBeShown
    {
        get
        {
            return needToBeShown;
        }

        set
        {
            needToBeShown = value;       
        }
    }

    public bool NeedToBeHide
    {
        get
        {
            return needToBeHide;
        }

        set
        {
            needToBeHide = value;

        }
    }

    void OnMouseEnter()
    {
        if (!needToBeHide && !NeedToBeShown && !menuUI.ACardIsShown && !menuUI.IsACardInfoMovingForShowing && menuUI.cardsInfoAreReady)
        {
            NeedToBeShown = true;
            menuUI.ACardIsShown = true;
            menuUI.IsACardInfoMovingForShowing = true;
        }
    }


    void OnMouseExit()
    {

        if (!NeedToBeShown && !needToBeHide && menuUI.ACardIsShown && !menuUI.IsACardInfoMovingForShowing && menuUI.cardsInfoAreReady)
        {
            NeedToBeHide = true;
            menuUI.IsACardInfoMovingForShowing = true;
        }
    }
}

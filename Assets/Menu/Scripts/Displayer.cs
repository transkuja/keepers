using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour {

    public MenuUI menuUI;
    public MenuManager menuManager;


    private bool needToBeShown = false;
    private bool isShown = false;

    public void Start()
    {
        menuUI = GameObject.FindObjectOfType<MenuUI>();
        menuManager = GameObject.FindObjectOfType<MenuManager>();
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

    public bool IsShown
    {
        get
        {
            return isShown;
        }

        set
        {
            isShown = value;
        }
    }

    //void OnMouseEnter()
    //{
    //    if (!NeedToBeShown && !menuUI.IsACardInfoMovingForShowing && menuUI.cardsInfoAreReady && !menumanager.GoDeck.GetComponent<Deck>().IsOpen)
    //    {
    //        NeedToBeShown = true;
    //        isShown = true;
    //        menuUI.IsACardInfoMovingForShowing = true;

    //    }
    //}


    //void OnMouseExit()
    //{

    //    if (menuUI.ACardIsShown && !menuUI.IsACardInfoMovingForShowing && menuUI.cardsInfoAreReady && !menumanager.GoDeck.GetComponent<Deck>().IsOpen)
    //    {
    //        NeedToBeShown = false;

    //        menuUI.IsACardInfoMovingForShowing = true;
    //    }
    //}

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (menuManager.CardLevelSelected == -1)
            {
                menuManager.GoDeck.GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(true);
                menuManager.GoDeck.GetComponent<GlowObjectCmd>().enabled = true;
            } else
            {
                if (!NeedToBeShown && !menuUI.ACardInfoIsShown && !menuUI.IsACardInfoMovingForShowing && menuUI.cardsInfoAreReady && !menuManager.GoDeck.GetComponent<Deck>().IsOpen)
                {
                    NeedToBeShown = true;
                    isShown = true;
                    menuUI.IsACardInfoMovingForShowing = true;

                }
                else if (menuUI.ACardInfoIsShown)
                {
                    NeedToBeShown = false;

                    menuUI.IsACardInfoMovingForShowing = true;
                }
            }
     
        }
    }
}

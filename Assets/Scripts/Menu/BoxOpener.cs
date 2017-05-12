using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpener : MonoBehaviour {

    private bool isBoxOpen;
    private MenuManager menuManager;
    private MenuUI menuUi;


    public Animator animatorBox;
    public Animator animatorCam;

    public Light spotLight;
    private float spotIntensityMax;
    public Light directionnalLight;
    private float directionnalIntensityMax;


    [SerializeField]
    Color colorLockOpen;
    [SerializeField]
    Color colorLockClosed;
    [SerializeField]
    public GameObject boxLock;

    public bool IsBoxOpen
    {
        get
        {
            return isBoxOpen;
        }

        set
        {
            isBoxOpen = value;
            if (isBoxOpen == true && !menuManager.HasBeenInit)
            {
                menuManager.HasBeenInit = true;
                menuManager.InitCards();
                menuManager.InitKeepers();
                menuUi.ComputeContentPositions(menuManager.GoCardsLevels);

            }
            menuManager.GoDeck.SetActive(isBoxOpen);
            for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
            {
                GameManager.Instance.AllKeepersList[i].gameObject.SetActive(isBoxOpen);
            }
            menuManager.SetActiveChatBoxes(isBoxOpen);
        }
    }



    // Use this for initialization
    void Start () {
        isBoxOpen = false;
        menuManager = GetComponent<MenuManager>();
        menuUi = GetComponent<MenuUI>();
    }

    public void BoxControls()
    {
        if ((menuManager.ListeSelectedKeepers.Count == 0 && menuManager.CardLevelSelected == -1 && menuManager.DeckOfCardsSelected == string.Empty))
        {
            //if (isBoxOpen)
            //{
            //    //foreach (Opener o in GameObject.FindObjectsOfType<Opener>())
            //    //{
            //    //    o.Fold();
            //    //}
            //}

            IsBoxOpen = !IsBoxOpen;
            animatorBox.SetBool("bOpen", isBoxOpen);
            animatorCam.SetBool("bOpen", isBoxOpen);


            spotLight.enabled = !isBoxOpen;
            directionnalLight.enabled = isBoxOpen;
            UpdateLockAspect();
        }
    }

    public void UpdateLockAspect()
    {

        if (isBoxOpen && (menuManager.ListeSelectedKeepers.Count != 0 && menuManager.CardLevelSelected == -1 && menuManager.DeckOfCardsSelected == string.Empty))
        {
            boxLock.GetComponent<GlowObjectCmd>().GlowColor = colorLockClosed;
        }
        else
        {
            boxLock.GetComponent<GlowObjectCmd>().GlowColor = colorLockOpen;
        }

    }
}

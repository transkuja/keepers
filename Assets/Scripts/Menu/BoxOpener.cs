using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpener : MonoBehaviour {

    private bool isBoxOpen;
    private bool boxIsReady;
    private MenuManager menuManager;
    private MenuUI menuUi;


    public Animator animatorBox;
    public Animator animatorCam;

    public Light spotLight;
    private Vector3 SpotLightTransformPos;
    private Quaternion SpotLightTransformRot;

    public Transform newSpotLightTransform;
    public bool spotlightneedUpdate;
    private float fLerp;
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
                menuUi.ComputeCardLevelPositions(menuManager.GoCardsLevels);
                menuUi.ComputeCardInfoPositions(menuManager.GoCardsInfo);
            }
            //menuManager.GoDeck.SetActive(isBoxOpen);
            if (isBoxOpen == false)
            {
                GlowController.UnregisterObject(menuManager.GoDeck.GetComponent<GlowObjectCmd>());
                menuManager.SetActiveChatBoxes(false);
            }
            else
            {
                GlowController.RegisterObject(menuManager.GoDeck.GetComponent<GlowObjectCmd>());
            }
            for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
            {
                if (isBoxOpen == false)
                {
                    GlowController.UnregisterObject(GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>());
                }
            }

        }
    }

    public bool BoxIsReady
    {
        get
        {
            return boxIsReady;
        }

        set
        {
            boxIsReady = value;
        }
    }

    private void Awake()
    {
        SpotLightTransformPos = spotLight.transform.localPosition;
        SpotLightTransformRot = spotLight.transform.localRotation;
    }

    // Use this for initialization
    void Start () {
        isBoxOpen = false;
        menuManager = GetComponent<MenuManager>();
        menuUi = GetComponent<MenuUI>();

    }

    public void BoxControls()
    {
        if (menuManager.DuckhavebringThebox && !menuUi.ACardIsShown && !menuUi.ACardInfoIsShown && (menuManager.ListeSelectedKeepers.Count == 0 && menuManager.CardLevelSelected == -1 && menuManager.DeckOfCardsSelected == string.Empty))
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

            spotlightneedUpdate = true;
            //spotLight.enabled = !isBoxOpen;
            //directionnalLight.enabled = isBoxOpen;

            // Force reset go cards info
            if ( isBoxOpen == false)
            {
                for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
                {
                    boxIsReady = false;

                    menuUi.hasReachStepOneInfo = false;
                    menuUi.hasReachStepTwoInfo = false;
                    menuUi.indexInfo = 0;
                    menuUi.cardInfofLerp = 0;
                    menuUi.cardsInfoAreReady = false;

                    menuManager.GoCardsInfo[i].transform.localPosition = menuUi.levelCardInfoKeyPoses[i][0].v3Pos + new Vector3(0, i * 0.02f, 0);
                    menuManager.GoCardsInfo[i].transform.localRotation = menuUi.levelCardInfoKeyPoses[i][0].quatRot;
                    GlowController.UnregisterObject(menuManager.GoCardsInfo[i].GetComponentInChildren<GlowObjectCmd>());
                }
            }
            //boxLock.GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(!isBoxOpen);
            UpdateLockAspect();
        }

    }

    public void UpdateLockAspect()
    {
        if (isBoxOpen && (menuUi.ACardIsShown || menuUi.ACardInfoIsShown || menuUi.LevelCardSelected != null ||(menuManager.ListeSelectedKeepers.Count != 0 && menuManager.CardLevelSelected == -1 && menuManager.DeckOfCardsSelected == string.Empty)))
        {
            boxLock.GetComponent<GlowObjectCmd>().GlowColor = colorLockClosed;
        }
        else
        {
            boxLock.GetComponent<GlowObjectCmd>().GlowColor = colorLockOpen;
        }

    }

    public void Update()
    {
        if (spotlightneedUpdate)
        {
            UpdateSpotLightIntensity();
        }
    }

    public void UpdateSpotLightIntensity()
    {
        fLerp += Time.unscaledDeltaTime * 0.8f;

        if (fLerp > 1)
        {
            fLerp = 1;
        }

        if (isBoxOpen)
        {
            spotLight.range = Mathf.Lerp(12.0f, 13.0f, fLerp);
            spotLight.spotAngle = Mathf.Lerp(120.0f, 130.0f, fLerp);
            spotLight.transform.localPosition = Vector3.Lerp(SpotLightTransformPos, newSpotLightTransform.localPosition, fLerp);
            spotLight.transform.localRotation = Quaternion.Lerp(SpotLightTransformRot, newSpotLightTransform.localRotation, fLerp);
        }
        else
        {
            spotLight.range = Mathf.Lerp(13.0f, 12.0f, fLerp);
            spotLight.spotAngle = Mathf.Lerp(130.0f, 120.0f, fLerp);
            spotLight.transform.localPosition = Vector3.Lerp(newSpotLightTransform.localPosition, SpotLightTransformPos, fLerp);
            spotLight.transform.localRotation = Quaternion.Lerp(newSpotLightTransform.localRotation, SpotLightTransformRot, fLerp);

        }

        if (fLerp == 1)
        {
     

            if (isBoxOpen)
            {
                menuUi.whereTheCardInfoiS.Clear();
                menuUi.whereTheCardInfoiSrotation.Clear();
                for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
                {

                    menuUi.whereTheCardInfoiS.Add(menuManager.GoCardsInfo[i].transform.position);
                    menuUi.whereTheCardInfoiSrotation.Add(menuManager.GoCardsInfo[i].transform.rotation);
                    menuManager.GoCardsInfo[i].transform.SetParent(null);
                    GlowController.UnregisterObject(menuManager.GoCardsInfo[i].GetComponentInChildren<GlowObjectCmd>());
                }
         
                boxIsReady = true;
                for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
                {
                    GlowController.UnregisterObject(GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>());
                }
            }

            spotlightneedUpdate = false;
            fLerp = 0;
        }
    }
}

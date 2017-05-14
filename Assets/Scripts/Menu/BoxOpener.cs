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

            }
            //menuManager.GoDeck.SetActive(isBoxOpen);
            if (isBoxOpen == false)
            {
                GlowController.UnregisterObject(menuManager.GoDeck.GetComponent<GlowObjectCmd>());
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
                } else
                {
                    GlowController.RegisterObject(GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>());
                }
            //    GameManager.Instance.AllKeepersList[i].gameObject.SetActive(isBoxOpen);
                    
            }
            menuManager.SetActiveChatBoxes(isBoxOpen);
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

            spotlightneedUpdate = true;
            //spotLight.enabled = !isBoxOpen;
            //directionnalLight.enabled = isBoxOpen;
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

    public void Update()
    {
        if (spotlightneedUpdate)
        {
            UpdateSpotLightIntensity();
        }
    }

    public void UpdateSpotLightIntensity()
    {
        fLerp += Time.unscaledDeltaTime * 3;

        if (fLerp > 1)
        {
            fLerp = 1;
        }

        if (isBoxOpen)
        {
            spotLight.range = Mathf.Lerp(9, 12.0f, fLerp);
            spotLight.spotAngle = Mathf.Lerp(80.0f, 100.0f, fLerp);
            spotLight.transform.localPosition = Vector3.Lerp(SpotLightTransformPos, newSpotLightTransform.localPosition, fLerp);
            spotLight.transform.localRotation = Quaternion.Lerp(SpotLightTransformRot, newSpotLightTransform.localRotation, fLerp);
        }
        else
        {
            spotLight.range = Mathf.Lerp(12.0f, 9.0f, fLerp);
            spotLight.spotAngle = Mathf.Lerp(100.0f, 80.0f, fLerp);
            spotLight.transform.localPosition = Vector3.Lerp(newSpotLightTransform.localPosition, SpotLightTransformPos, fLerp);
            spotLight.transform.localRotation = Quaternion.Lerp(newSpotLightTransform.localRotation, SpotLightTransformRot, fLerp);
        }

        if (fLerp == 1)
        {

            spotlightneedUpdate = false;
            fLerp = 0;
        }
    }
}

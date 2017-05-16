using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {
    public class keyPose
    {
        public Vector3 v3Pos;
        public Quaternion quatRot;

        public keyPose(Vector3 _v3, Quaternion _quat)
        {
            v3Pos = _v3;
            quatRot = _quat;
        }
    }

    private MenuManager menuManager;
    private BoxOpener box;

    public Image startButtonImg;

    // Postion ou devront aller les perso ou les deck
    public GameObject keepersPositions;
    public GameObject keepersPositionTarget;
    public Transform levelDeckPosition;
    public Transform cardInfoStartingPosition;
    public Transform cardInfoStartingPosition2;
    public Transform cardInfoStartingPosition3;
    public List<Transform> cardInfoEndPosition;
    public Transform levelCardSelectedPosition;
    public Transform levelCardSelectedPosition2;


    private GameObject levelCardSelected = null;


    // Position for levelCards
    private bool isACardMoving;
    private List<List<keyPose>> levelCardKeyPoses;
    private List<Vector3> whereTheCardiS;
    private List<Quaternion> whereTheCardiSrotation;
    bool hasReachStepOne = false;
    bool hasReachStepTwo = false;
    private int index = 1;
    public Transform cameraWhere;
    private float carLevelfLerp = 0.0f;
    private bool aCardIsShown;

    public bool initCardInfo;


    // CardInfo
    [HideInInspector]
    private bool isACardInfoMoving;

    [HideInInspector]
    public List<List<keyPose>> levelCardInfoKeyPoses;
    [HideInInspector]
    public List<Vector3> whereTheCardInfoiS;
    [HideInInspector]
    public List<Quaternion> whereTheCardInfoiSrotation;
    [HideInInspector]
    public bool hasReachStepOneInfo = false;
    [HideInInspector]
    public bool hasReachStepTwoInfo = false;
    [HideInInspector]
    public bool hasReachStepThreeInfo = false;
    [HideInInspector]
    public  int indexInfo = 0;
    [HideInInspector]
    public float cardInfofLerp = 0.0f;
    private bool aCardInfoIsReady;

    // Card Info Shown
    private bool aCardInfoIsShown;
    private bool isACardInfoMovingForShowing;
    private float cardInfoShownfLerp;
    private int indexInfoShown = 0;

    // Pawn
    private bool isAPawnMoving;
    private PawnInstance pawnMoving;
    private Transform previousTransform;
    private int firstFreeIndex = -1;
    private int previousIndex = -1;
    private float fLerp = 0.0f;

    // CardSelectd
    private bool aCardLevelSelectedIsMoving;
    private int indexCardSelected = 0;

    public bool ACardIsShown
    {
        get
        {
            return aCardIsShown;
        }

        set
        {
            aCardIsShown = value;
        }
    }

    public bool cardsInfoAreReady
    {
        get
        {
            return aCardInfoIsReady;
        }

        set
        {
            aCardInfoIsReady = value;
        }
    }

    public bool ACardInfoIsShown
    {
        get
        {
            return aCardInfoIsShown;
        }

        set
        {
            aCardInfoIsShown = value;
        }
    }

    public bool IsACardInfoMovingForShowing
    {
        get
        {
            return isACardInfoMovingForShowing;
        }

        set
        {
            isACardInfoMovingForShowing = value;
        }
    }

    public GameObject LevelCardSelected
    {
        get
        {
            return levelCardSelected;
        }

        set
        {
            levelCardSelected = value;
        }
    }

    public bool IsACardInfoMoving
    {
        get
        {
            return isACardInfoMoving;
        }

        set
        {
            isACardInfoMoving = value;
        }
    }

    public bool IsACardMoving
    {
        get
        {
            return isACardMoving;
        }

        set
        {
            isACardMoving = value;
        }
    }
    void Start()
    {
        menuManager = GetComponent<MenuManager>();
        box = GetComponent<BoxOpener>();

        levelCardKeyPoses = new List<List<keyPose>>();
        whereTheCardiS = new List<Vector3>();
        whereTheCardiSrotation = new List<Quaternion>();
        levelCardInfoKeyPoses = new List<List<keyPose>>();
        whereTheCardInfoiS = new List<Vector3>();
        whereTheCardInfoiSrotation = new List<Quaternion>();

        isAPawnMoving = false;
        isACardMoving = false;
        isACardInfoMoving = false;
        aCardIsShown = false;
        aCardInfoIsShown = false;
        aCardInfoIsReady = false;
        aCardLevelSelectedIsMoving = false;
        initCardInfo = false;

    }

    void Update()
    {
        if (box.BoxIsReady && isAPawnMoving)
        {
            UpdateKeepersPosition();
        }

        if (box.BoxIsReady && isACardMoving)
        {
            UpdateCardLevelPositions();
        }

        if (box.BoxIsReady && !isACardInfoMoving && initCardInfo && !aCardInfoIsReady)
        {
            UpdateCardInfoPositions();
        }

        if (box.BoxIsReady && aCardInfoIsReady && isACardInfoMovingForShowing)
        {
            UpdateCardInfoShowingPositions();
        }

        if (box.BoxIsReady && aCardLevelSelectedIsMoving)
        {
            UpdateCardLevelSelectedPosition();
        }
    }

    public void UpdateKeepers(PawnInstance pi, Transform _previousParent)
    {
            pawnMoving = pi;

            previousTransform = _previousParent;
            previousIndex = previousTransform.GetSiblingIndex();
            if (previousTransform == keepersPositions.transform.GetChild(previousIndex))
            {
                for (int i = 0; i < keepersPositionTarget.transform.childCount; i++)
                {
                    if (keepersPositionTarget.transform.GetChild(i).childCount == 0)
                    {
                        firstFreeIndex = keepersPositionTarget.transform.GetChild(i).GetSiblingIndex();
                        break;
                    }
                }
            }
            else if (previousTransform == keepersPositionTarget.transform.GetChild(previousIndex))
            {
                for (int i = 0; i < keepersPositions.transform.childCount; i++)
                {
                    if (keepersPositions.transform.GetChild(i).name == pi.Data.PawnId)
                    {
                        firstFreeIndex = keepersPositions.transform.GetChild(i).GetSiblingIndex();
                        break;
                    }
                }
            }
            isAPawnMoving = true;

            UpdateStartButton();
    
    }

    public void UpdateDeckDisplayed()
    {
        whereTheCardiS.Clear();
        whereTheCardiSrotation.Clear();
        for (int i = 0; i < menuManager.GoCardsLevels.Count; i++)
        {

            whereTheCardiS.Add(menuManager.GoCardsLevels[i].transform.position);
            whereTheCardiSrotation.Add(menuManager.GoCardsLevels[i].transform.rotation);
        }
        isACardMoving = true;

    }

    public void UpdateDeckSelected()
    {

        whereTheCardiS.Clear();
        whereTheCardiSrotation.Clear();
        for (int i = 0; i < menuManager.GoCardsLevels.Count; i++)
        {

            whereTheCardiS.Add(menuManager.GoCardsLevels[i].transform.position);
            whereTheCardiSrotation.Add(menuManager.GoCardsLevels[i].transform.rotation);
        }
        aCardLevelSelectedIsMoving = true;
        indexCardSelected = 0;
    }

    public void UpdateStartButton()
    {
        // TODO : refaire ça
        switch (menuManager.DeckOfCardsSelected)
        {
            case "deck_01":
                startButtonImg.GetComponentInChildren<Text>().text = menuManager.ListeSelectedKeepers.Count + "/1";
                break;
            case "deck_02":
            case "deck_03":
            case "deck_04":
                startButtonImg.GetComponentInChildren<Text>().text = menuManager.ListeSelectedKeepers.Count + "/3";
                break;
            default:
                startButtonImg.GetComponentInChildren<Text>().text = string.Empty;
                break;
        }

        if (menuManager.ListeSelectedKeepers.Count == 0 || menuManager.CardLevelSelected == -1 || menuManager.DeckOfCardsSelected == string.Empty
            || (menuManager.DeckOfCardsSelected == "deck_04" && menuManager.ListeSelectedKeepers.Count != 3)
            || (menuManager.DeckOfCardsSelected == "deck_02" && menuManager.ListeSelectedKeepers.Count != 3))
        {
            startButtonImg.enabled = false;
        }
        else
        {
            startButtonImg.enabled = true;
        }
        // end TODO
    }

    void UpdateKeepersPosition()
    {
        fLerp += Time.unscaledDeltaTime * 5;

        if (fLerp > 1)
        {
            fLerp = 1;
        }

        if (previousTransform == keepersPositions.transform.GetChild(previousIndex))
        {
            pawnMoving.transform.localPosition = Vector3.Lerp(keepersPositions.transform.GetChild(previousIndex).position, keepersPositionTarget.transform.GetChild(firstFreeIndex).position, fLerp);
            pawnMoving.transform.localRotation = Quaternion.Lerp(keepersPositions.transform.GetChild(previousIndex).rotation, keepersPositionTarget.transform.GetChild(firstFreeIndex).rotation, fLerp);
        }
        else if (previousTransform == keepersPositionTarget.transform.GetChild(previousIndex))
        {
            pawnMoving.transform.localPosition = Vector3.Lerp(keepersPositionTarget.transform.GetChild(previousIndex).position, keepersPositions.transform.GetChild(firstFreeIndex).position, fLerp);
            pawnMoving.transform.localRotation = Quaternion.Lerp(keepersPositionTarget.transform.GetChild(previousIndex).rotation, keepersPositions.transform.GetChild(firstFreeIndex).rotation, fLerp);
        }

        if (fLerp == 1)
        {
            if (previousTransform == keepersPositions.transform.GetChild(previousIndex))
                pawnMoving.transform.SetParent(keepersPositionTarget.transform.GetChild(firstFreeIndex));
            else if (previousTransform = keepersPositionTarget.transform.GetChild(previousIndex))
            {
                pawnMoving.transform.SetParent(keepersPositions.transform.GetChild(firstFreeIndex));
            }
            isAPawnMoving = false;
            fLerp = 0;
            pawnMoving = null;
        }
    }

    void UpdateCardLevelPositions()
    {
        carLevelfLerp += Time.unscaledDeltaTime * 3.5f ;

        if (carLevelfLerp > 1)
        {
            carLevelfLerp = 1;
        }

        if(carLevelfLerp > (!menuManager.GoDeck.GetComponent<Deck>().IsOpen ? 0.4 : 0.4f) && !hasReachStepOne)
        {
            whereTheCardiS.Clear();
            whereTheCardiSrotation.Clear();
            for (int i = 0; i < menuManager.GoCardsLevels.Count; i++)
            {

                whereTheCardiS.Add(menuManager.GoCardsLevels[i].transform.position);
                whereTheCardiSrotation.Add(menuManager.GoCardsLevels[i].transform.rotation);
            }
            hasReachStepOne = true;
            carLevelfLerp = 0;
            index = 1;
        }

        if (carLevelfLerp > (!menuManager.GoDeck.GetComponent<Deck>().IsOpen? 0.5 : 0.95f) && !hasReachStepTwo && hasReachStepOne)
        {
            whereTheCardiS.Clear();
            whereTheCardiSrotation.Clear();
            for (int i = 0; i < menuManager.GoCardsLevels.Count; i++)
            {

                whereTheCardiS.Add(menuManager.GoCardsLevels[i].transform.localPosition);
                whereTheCardiSrotation.Add(menuManager.GoCardsLevels[i].transform.localRotation);
            }
            carLevelfLerp = 0;
            hasReachStepTwo = true;
            index = 2;
            if (menuManager.GoDeck.GetComponent<Deck>().IsOpen)
            {
                for (int i = 0; i < menuManager.GoCardsLevels.Count; i++)
                {
                    GlowController.UnregisterObject(menuManager.GoCardsLevels[i].GetComponent<GlowObjectCmd>());
                 
                }
                menuManager.SetActiveChatBoxes(true);
            }
            else
            {
                for (int i = 0; i < menuManager.GoCardsLevels.Count; i++)
                {
                    GlowController.RegisterObject(menuManager.GoCardsLevels[i].GetComponent<GlowObjectCmd>());
   
                }
                menuManager.SetActiveChatBoxes(false);
            }
        }


        for (int i=0; i<menuManager.GoCardsLevels.Count; i++)
        {

            if (!menuManager.GoDeck.GetComponent<Deck>().IsOpen)
            {
                if (menuManager.GoCardsLevels[i] != LevelCardSelected)
                {
                    menuManager.GoCardsLevels[i].transform.position = Vector3.Lerp(whereTheCardiS[i], levelCardKeyPoses[i][index].v3Pos, carLevelfLerp);
                    menuManager.GoCardsLevels[i].transform.rotation = Quaternion.Lerp(whereTheCardiSrotation[i], levelCardKeyPoses[i][index].quatRot, carLevelfLerp);
                }
            }
            else
            {
                if (menuManager.GoCardsLevels[i] != LevelCardSelected)
                {
                    menuManager.GoCardsLevels[i].transform.position = Vector3.Lerp(whereTheCardiS[i], levelCardKeyPoses[i][levelCardKeyPoses[i].Count - (index) - 1].v3Pos, carLevelfLerp);
                    menuManager.GoCardsLevels[i].transform.rotation = Quaternion.Lerp(whereTheCardiSrotation[i], levelCardKeyPoses[i][levelCardKeyPoses[i].Count - (index) - 1].quatRot, carLevelfLerp);
                }
            }
        }
 

        if (carLevelfLerp == 1)
        {
            menuManager.GoDeck.GetComponent<Deck>().IsOpen = !menuManager.GoDeck.GetComponent<Deck>().IsOpen;

            aCardIsShown = !aCardIsShown;
            box.UpdateLockAspect();

            isACardMoving = false;
            carLevelfLerp = 0;
            index = 1;
            hasReachStepOne = false;
            hasReachStepTwo = false;
        }
    }

    public void ComputeCardLevelPositions(List<GameObject> cardLevels)
    {
        levelCardKeyPoses.Clear();
        Vector3 size = Vector3.zero;
        for (int i = 0; i < cardLevels.Count; i++)
        {
            List<keyPose> listKeyPose = new List<keyPose>();
            if (cardLevels[i].GetComponent<MeshRenderer>() == null)
            {
                Debug.Log("Bug"); return;
            }

            size = cardLevels[i].GetComponent<MeshRenderer>().bounds.size;


            float fOrigin = (cardLevels.Count % 2 == 1) ? -((cardLevels.Count / 2) * (size.x)) : -((((cardLevels.Count / 2) - 1) * (size.x)) + (size.x) / 2.0f);

            float fIncrement = size.x + 0.1f;

            listKeyPose.Add(new keyPose(menuManager.GoDeck.transform.position, menuManager.GoDeck.transform.GetChild(0).rotation));
            listKeyPose.Add(new keyPose(menuManager.GoDeck.transform.GetChild(0).position, menuManager.GoDeck.transform.GetChild(0).rotation));
            listKeyPose.Add(new keyPose(new Vector3(cameraWhere.position.x + fOrigin + i * fIncrement, cameraWhere.position.y, cameraWhere.position.z), new Quaternion(cameraWhere.rotation.x, cameraWhere.rotation.y, cameraWhere.rotation.z, cameraWhere.rotation.w)));
            levelCardKeyPoses.Add(listKeyPose);
        }

    }

    public void UpdateCardInfoPositions()
    {
        cardInfofLerp += Time.unscaledDeltaTime * 2.2f;

        if (cardInfofLerp > 1)
        {
            cardInfofLerp = 1;
        }

        if (cardInfofLerp > 0.5 && !hasReachStepOneInfo)
        {
            whereTheCardInfoiS.Clear();
            whereTheCardInfoiSrotation.Clear();
            for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
            {
                whereTheCardInfoiS.Add(menuManager.GoCardsInfo[i].transform.position);
                whereTheCardInfoiSrotation.Add(menuManager.GoCardsInfo[i].transform.rotation);
            }
            hasReachStepOneInfo = true;
            cardInfofLerp = 0;
            indexInfo = 1;
        }

        if (cardInfofLerp > 0.8 && !hasReachStepTwoInfo)
        {
            whereTheCardInfoiS.Clear();
            whereTheCardInfoiSrotation.Clear();
            for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
            {
                whereTheCardInfoiS.Add(menuManager.GoCardsInfo[i].transform.localPosition);
                whereTheCardInfoiSrotation.Add(menuManager.GoCardsInfo[i].transform.localRotation);
            }
            cardInfofLerp = 0;
            hasReachStepTwoInfo = true;
            indexInfo = 2;
        }

        if (cardInfofLerp > 0.8 && !hasReachStepThreeInfo)
        {
            whereTheCardInfoiS.Clear();
            whereTheCardInfoiSrotation.Clear();
            for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
            {
                whereTheCardInfoiS.Add(menuManager.GoCardsInfo[i].transform.localPosition);
                whereTheCardInfoiSrotation.Add(menuManager.GoCardsInfo[i].transform.localRotation);
            }
            cardInfofLerp = 0;
            hasReachStepThreeInfo = true;
            indexInfo = 3;
        }

        for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
        {
                menuManager.GoCardsInfo[i].transform.position = Vector3.Lerp(whereTheCardInfoiS[i], levelCardInfoKeyPoses[i][indexInfo].v3Pos, cardInfofLerp);
                menuManager.GoCardsInfo[i].transform.rotation = Quaternion.Lerp(whereTheCardInfoiSrotation[i], levelCardInfoKeyPoses[i][indexInfo].quatRot, cardInfofLerp);
        }


        if (cardInfofLerp == 1)
        {
            isACardInfoMoving = false;
            cardInfofLerp = 0;
            indexInfo = 0;
            box.UpdateLockAspect();
            hasReachStepOneInfo = false;
            hasReachStepTwoInfo = false;
            hasReachStepThreeInfo = false;
            aCardInfoIsReady = true;
        }
    }

    public void ComputeCardInfoPositions(List<GameObject> cardInfo)
    {
        levelCardInfoKeyPoses.Clear();
        for (int i = 0; i < cardInfo.Count; i++)
        {
            List<keyPose> listKeyPose = new List<keyPose>();

            listKeyPose.Add(new keyPose(cardInfoStartingPosition.position, cardInfoStartingPosition.rotation));
            listKeyPose.Add(new keyPose(cardInfoStartingPosition2.position, cardInfoStartingPosition2.rotation));
            listKeyPose.Add(new keyPose(cardInfoStartingPosition3.position, cardInfoStartingPosition3.rotation));
            listKeyPose.Add(new keyPose(cardInfoEndPosition[i].position, cardInfoEndPosition[i].rotation));
            levelCardInfoKeyPoses.Add(listKeyPose);
        }

    }

    public void UpdateCardInfoShowingPositions()
    {
        if (cardInfoShownfLerp == 0)
        {

            for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
            {
                if (menuManager.GoCardsInfo[i].GetComponentInChildren<Displayer>().NeedToBeShown)
                {
                    GlowController.RegisterObject((menuManager.GoCardsInfo[i].GetComponentInChildren<GlowObjectCmd>()));
                } else
                {
                    GlowController.UnregisterObject((menuManager.GoCardsInfo[i].GetComponentInChildren<GlowObjectCmd>()));
                }
            }

            menuManager.SetActiveChatBoxes(false);
        }

        cardInfoShownfLerp += Time.unscaledDeltaTime * 10f;

        if (cardInfoShownfLerp > 1)
        {
            cardInfoShownfLerp = 1;
        }


        for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
        {
            if (menuManager.GoCardsInfo[i].GetComponentInChildren<Displayer>().NeedToBeShown)
            {

                menuManager.GoCardsInfo[i].transform.position = Vector3.Lerp(levelCardInfoKeyPoses[i][levelCardInfoKeyPoses[i].Count - 1].v3Pos, cameraWhere.transform.position, cardInfoShownfLerp);
                menuManager.GoCardsInfo[i].transform.rotation = Quaternion.Lerp(levelCardInfoKeyPoses[i][levelCardInfoKeyPoses[i].Count-1].quatRot, cameraWhere.transform.rotation, cardInfoShownfLerp);
            }
            else if (ACardInfoIsShown)
            {
                if (menuManager.GoCardsInfo[i].GetComponentInChildren<Displayer>().IsShown) { 
                    menuManager.GoCardsInfo[i].transform.position = Vector3.Lerp(cameraWhere.transform.position, levelCardInfoKeyPoses[i][levelCardInfoKeyPoses[i].Count - 1].v3Pos, cardInfoShownfLerp);
                    menuManager.GoCardsInfo[i].transform.rotation = Quaternion.Lerp(cameraWhere.transform.rotation, levelCardInfoKeyPoses[i][levelCardInfoKeyPoses[i].Count - 1].quatRot, cardInfoShownfLerp);
                }
            }
        }


        if (cardInfoShownfLerp == 1)
        {
            isACardInfoMovingForShowing = false;
            cardInfoShownfLerp = 0;
            indexInfoShown = 0;




            bool isACardShow = false;
            for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
            {
                if (menuManager.GoCardsInfo[i].GetComponentInChildren<Displayer>().NeedToBeShown)
                {
                    ACardInfoIsShown = true;
                    isACardShow = true;
                    //
                    menuManager.GoCardsInfo[i].GetComponentInChildren<Displayer>().NeedToBeShown = false;
                } else
                {
                    //GlowController.UnregisterObject((menuManager.GoCardsInfo[i].GetComponentInChildren<GlowObjectCmd>()));
                    menuManager.GoCardsInfo[i].GetComponentInChildren<Displayer>().IsShown = false;
                }
            }
            if (!isACardShow)
            {
                for (int i = 0; i < menuManager.GoCardsInfo.Count; i++)
                {
                    GlowController.RegisterObject((menuManager.GoCardsInfo[i].GetComponentInChildren<GlowObjectCmd>()));
                    menuManager.SetActiveChatBoxes(true);
                }

                ACardInfoIsShown = false;
            }

            box.UpdateLockAspect();
        }
    }

    public void UpdateCardLevelSelectedPosition()
    {
        if (carLevelfLerp == 0 && !levelCardSelected.GetComponent<CardLevel>().IsSelected && indexCardSelected == 0)
        {
            GlowController.UnregisterObject(levelCardSelected.GetComponent<GlowObjectCmd>());
            for (int i = 0; i < menuManager.GoCardChildren.Count; i++)
            {

                for (int j = 0; j < menuManager.GoCardChildren[i].Count; j++)
                {
                    menuManager.GoCardChildren[i][j].SetActive(false);
                    menuManager.GoCardChildren[i][j].transform.localPosition = Vector3.zero;
                }
            }
        }



        carLevelfLerp += Time.unscaledDeltaTime * 4f;


        if (carLevelfLerp > 1)
        {
            carLevelfLerp = 1;
        }


        if (levelCardSelected.GetComponent<CardLevel>().IsSelected)
        {
            levelCardSelected.transform.position = Vector3.Lerp(cameraWhere.position, levelCardSelectedPosition.position, carLevelfLerp);
            levelCardSelected.transform.rotation = Quaternion.Lerp(cameraWhere.rotation, levelCardSelectedPosition.rotation, carLevelfLerp);
        }
        else if (!levelCardSelected.GetComponent<CardLevel>().IsSelected)
        {
            if (indexCardSelected == 2)
            {
                levelCardSelected.transform.position = Vector3.Lerp(levelCardKeyPoses[0][1].v3Pos, levelCardKeyPoses[0][0].v3Pos, carLevelfLerp);
                levelCardSelected.transform.rotation = Quaternion.Lerp(levelCardKeyPoses[0][1].quatRot, levelCardKeyPoses[0][0].quatRot, carLevelfLerp);
            }
            else if (indexCardSelected == 1)
            {
                levelCardSelected.transform.position = Vector3.Lerp(levelCardSelectedPosition2.position, levelCardKeyPoses[0][1].v3Pos, carLevelfLerp);
                levelCardSelected.transform.rotation = Quaternion.Lerp(levelCardSelectedPosition2.rotation, levelCardKeyPoses[0][1].quatRot, carLevelfLerp);
            } else
            {
                levelCardSelected.transform.position = Vector3.Lerp(levelCardSelectedPosition.position, levelCardSelectedPosition2.position, carLevelfLerp);
                levelCardSelected.transform.rotation = Quaternion.Lerp(levelCardSelectedPosition.rotation, levelCardSelectedPosition2.rotation, carLevelfLerp);
            }

        }

        if (carLevelfLerp == 1)
        {
            if (levelCardSelected.GetComponent<CardLevel>().IsSelected) { 
                for (int i = 0; i < menuManager.GoCardChildren.Count; i++)
                {

                    for (int j = 0; j < menuManager.GoCardChildren[i].Count; j++)
                    {
                        if (levelCardSelected == menuManager.GoCardChildren[i][j].GetComponentInParent<CardLevel>().gameObject)
                        {
                            menuManager.GoCardChildren[i][j].SetActive(true);
                            menuManager.GoCardChildren[i][j].transform.localPosition = new Vector3(0.1f, -0.01f, 0.0f) * (j+1);
                        } else
                        {
                            menuManager.GoCardChildren[i][j].SetActive(false);
                            menuManager.GoCardChildren[i][j].transform.localPosition = Vector3.zero;
                        }
                    }
                 }
                isACardMoving = true;

            }
            else
            {
                if (indexCardSelected == 2)
                {
                    levelCardSelected = null;
                }     
            }

            //box.UpdateLockAspect();
            if(levelCardSelected != null && levelCardSelected.GetComponent<CardLevel>().IsSelected)
            {
                aCardLevelSelectedIsMoving = false;
            } else if (indexCardSelected != 2)
            {
                indexCardSelected++;
       
            } else if (indexCardSelected ==2){
                aCardLevelSelectedIsMoving = false;
            }
            carLevelfLerp = 0;
          

            if(!initCardInfo)
                initCardInfo = true;
        }
    }
}

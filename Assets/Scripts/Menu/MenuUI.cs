using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {
    private MenuManager menuManager;

    // Postion ou devront aller les perso ou les deck
    public GameObject keepersPositions;
    public GameObject keepersPositionTarget;
    public Transform levelDeckPosition;

    // Position for levelCards
    private bool isACardMoving;

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

    private List<List<keyPose>> levelCardKeyPoses;
    private int index = 1;
    public Transform cameraWhere;
    

    // Pawn
    private bool isAPawnMoving;
    private PawnInstance pawnMoving;
    private Transform previousTransform;
    private int firstFreeIndex = -1;
    private int previousIndex = -1;
    private float fLerp = 0.0f;

    void Start()
    {
        menuManager = GetComponent<MenuManager>();
        levelCardKeyPoses = new List<List<keyPose>>();
        isAPawnMoving = false;
        isACardMoving = false;
    }

    void Update()
    {
        if (isAPawnMoving)
        {
            UpdateKeepersPosition();
        }

        if (isACardMoving)
        {

            UpdateCardLevelPositions();
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

    public void UpdateDeckSelected()
    {

        isACardMoving = true;
    }

    public void UpdateStartButton()
    {
        // TODO : refaire ça
        switch (menuManager.DeckOfCardsSelected)
        {
            case "deck_01":
                //startButtonImg.GetComponentInChildren<Text>().text = menuManager.ListeSelectedKeepers.Count + "/1";
                break;
            case "deck_02":
            case "deck_03":
            case "deck_04":
                //startButtonImg.GetComponentInChildren<Text>().text = menuManager.ListeSelectedKeepers.Count + "/3";
                break;
            default:
                //startButtonImg.GetComponentInChildren<Text>().text = string.Empty;
                break;
        }

        if (menuManager.ListeSelectedKeepers.Count == 0 || menuManager.CardLevelSelected == -1 || menuManager.DeckOfCardsSelected == string.Empty
            || (menuManager.DeckOfCardsSelected == "deck_04" && menuManager.ListeSelectedKeepers.Count != 3)
            || (menuManager.DeckOfCardsSelected == "deck_02" && menuManager.ListeSelectedKeepers.Count != 3))
        {
            //startButtonImg.enabled = false;
        }
        else
        {
            //startButtonImg.enabled = true;
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
        fLerp += Time.unscaledDeltaTime * 0.05f ;

        if (fLerp > 1)
        {
            fLerp = 1;
        }

        if( fLerp > 0.2)
        {
            index = 1;
        }

        if (fLerp > 0.4)
        {
            index = 2;
        }


        for (int i=0; i<menuManager.GoCardsLevels.Count; i++)
        {

            if (!menuManager.GoDeck.GetComponent<Deck>().IsOpen)
            {
                menuManager.GoCardsLevels[i].transform.localPosition = Vector3.Lerp(levelCardKeyPoses[i][index-1].v3Pos, levelCardKeyPoses[i][index].v3Pos, fLerp);
                menuManager.GoCardsLevels[i].transform.localRotation = Quaternion.Lerp(levelCardKeyPoses[i][index-1].quatRot, levelCardKeyPoses[i][index].quatRot, fLerp);
            }
            else
            {
                menuManager.GoCardsLevels[i].transform.localPosition = Vector3.Lerp(levelCardKeyPoses[i][levelCardKeyPoses[i].Count -1 -index].v3Pos, levelCardKeyPoses[i][levelCardKeyPoses[i].Count - 1 - index -1].v3Pos, fLerp);
                menuManager.GoCardsLevels[i].transform.localRotation = Quaternion.Lerp(levelCardKeyPoses[i][(levelCardKeyPoses[i].Count - 1) - index].quatRot, levelCardKeyPoses[i][levelCardKeyPoses[i].Count - 1 - index-1].quatRot, fLerp);
            }
        }
 

        if (fLerp == 1)
        {
            menuManager.GoDeck.GetComponent<Deck>().IsOpen = !menuManager.GoDeck.GetComponent<Deck>().IsOpen;
            if (!menuManager.GoDeck.GetComponent<Deck>().IsOpen)
            {
                for (int i = 0; i < menuManager.GoCardsLevels.Count; i++)
                {
                    GlowController.UnregisterObject(menuManager.GoCardsLevels[i].GetComponent<GlowObjectCmd>());
                    menuManager.SetActiveChatBoxes(true);
                }
            } else
            {
                for (int i = 0; i < menuManager.GoCardsLevels.Count; i++)
                {
                    GlowController.RegisterObject(menuManager.GoCardsLevels[i].GetComponent<GlowObjectCmd>());
                    menuManager.SetActiveChatBoxes(false);
                }
            }
         
            //if (previousTransform == keepersPositions.transform.GetChild(previousIndex))
            //    menuManager.GoCardsLevels[i].transform.SetParent(keepersPositionTarget.transform.GetChild(firstFreeIndex));
            //else if (previousTransform = keepersPositionTarget.transform.GetChild(previousIndex))
            //{
            //    pawnMoving.transform.SetParent(keepersPositions.transform.GetChild(firstFreeIndex));
            //}
            isACardMoving = false;
            fLerp = 0;
             index = 1;
        }
    }

    public void ComputeContentPositions(List<GameObject> contentToCompute)
    {
        levelCardKeyPoses.Clear();
        Vector3 size = Vector3.zero;
        for (int i = 0; i < contentToCompute.Count; i++)
        {
            List<keyPose> listKeyPose = new List<keyPose>();
            if (contentToCompute[i].GetComponent<MeshRenderer>() == null)
            {
                Debug.Log("Bug"); return;
            }

            size = contentToCompute[i].GetComponent<MeshRenderer>().bounds.size;


            float fOrigin = (contentToCompute.Count % 2 == 1) ? -((contentToCompute.Count / 2) * (size.x)) : -((((contentToCompute.Count / 2) - 1) * (size.x)) + (size.x) / 2.0f);

            float fIncrement = size.x + 0.1f;

            listKeyPose.Add(new keyPose(menuManager.GoDeck.transform.position, menuManager.GoDeck.transform.rotation));
            listKeyPose.Add(new keyPose(menuManager.GoDeck.transform.GetChild(0).position, menuManager.GoDeck.transform.GetChild(0).rotation));
            listKeyPose.Add(new keyPose(new Vector3(cameraWhere.position.x + fOrigin + i * fIncrement, cameraWhere.position.y, cameraWhere.position.z), new Quaternion(cameraWhere.rotation.x, cameraWhere.rotation.y, cameraWhere.rotation.z, cameraWhere.rotation.w)));
            levelCardKeyPoses.Add(listKeyPose);
        }

    }
}

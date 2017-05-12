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
    private List<Vector3> levelCardsPositions;
    private List<Quaternion> levelCardsRotations;

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
        levelCardsPositions = new List<Vector3>();
        levelCardsRotations = new List<Quaternion>();
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
        fLerp += Time.unscaledDeltaTime * 5;

        if (fLerp > 1)
        {
            fLerp = 1;
        }

        for (int i=0; i<menuManager.GoCardsLevels.Count; i++)
        {

            if (!menuManager.GoDeck.GetComponent<Deck>().IsOpen)
            {
                menuManager.GoCardsLevels[i].transform.localPosition = Vector3.Lerp(menuManager.GoDeck.transform.position, levelCardsPositions[i], fLerp);
                menuManager.GoCardsLevels[i].transform.localRotation = Quaternion.Lerp(menuManager.GoDeck.transform.rotation, levelCardsRotations[i], fLerp);
            }
            else
            {
                menuManager.GoCardsLevels[i].transform.localPosition = Vector3.Lerp(levelCardsPositions[i], menuManager.GoDeck.transform.position, fLerp);
                menuManager.GoCardsLevels[i].transform.localRotation = Quaternion.Lerp(levelCardsRotations[i], menuManager.GoDeck.transform.rotation, fLerp);
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
        }
    }

    public void ComputeContentPositions(List<GameObject> contentToCompute)
    {
        levelCardsPositions.Clear();
        Vector3 size = Vector3.zero;
        for (int i = 0; i < contentToCompute.Count; i++)
        {
            if (contentToCompute[i].GetComponent<MeshRenderer>() == null)
            {
                Debug.Log("Bug"); return;
            }

            size = contentToCompute[i].GetComponent<MeshRenderer>().bounds.size;


            float fOrigin = (contentToCompute.Count % 2 == 1) ? -((contentToCompute.Count / 2) * (size.x)) : -((((contentToCompute.Count / 2) - 1) * (size.x)) + (size.x) / 2.0f);

            float fIncrement = size.x + 0.1f;

            levelCardsPositions.Add(new Vector3(cameraWhere.position.x + fOrigin + i * fIncrement, cameraWhere.position.y, cameraWhere.position.z ));
            levelCardsRotations.Add(new Quaternion(cameraWhere.rotation.x, cameraWhere.rotation.y, cameraWhere.rotation.z, cameraWhere.rotation.w));
        }

    }
}

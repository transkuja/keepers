using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Behaviour;

public class ThrownDiceHandler : MonoBehaviour {

    [SerializeField]
    private Transform dicePositions;

    Dictionary<PawnInstance, Die[]> diceForCurrentThrow = new Dictionary<PawnInstance, Die[]>();
    bool isRunning = false;
    //int stoppedDice = 0;
    Dictionary<PawnInstance, List<GameObject>> diceInstance = new Dictionary<PawnInstance, List<GameObject>>();
    Dictionary<PawnInstance, Face[]> throwResult = new Dictionary<PawnInstance, Face[]>();
    float timerAnimation = 0.0f;
    bool areDiceFeedbacksInitialized = false;
    Dictionary<PawnInstance, List<int>> upFaceIndex = new Dictionary<PawnInstance, List<int>>();

    [SerializeField]
    float rollDiceAnimClip = 0.75f;
    bool areDiceRotatedProperly = false;

    public void InitThrow()
    {
        if (!isRunning)
        {
            GetComponent<UIBattleHandler>().ChangeState(UIBattleState.Actions);
            areDiceFeedbacksInitialized = false;
            areDiceRotatedProperly = false;
            timerAnimation = 0.0f;
            isRunning = false;
            upFaceIndex.Clear();
            diceInstance.Clear();
            for (int i = 0; i < BattleHandler.CurrentBattleKeepers.Length; i++)
            {
                PawnInstance currentKeeper = BattleHandler.CurrentBattleKeepers[i];
                if (currentKeeper.GetComponent<Mortal>().CurrentHp <= 0)
                    continue;

                diceForCurrentThrow.Add(currentKeeper, currentKeeper.GetComponent<Fighter>().Dice);

                // Create dice visuals
                diceInstance.Add(currentKeeper, new List<GameObject>());
                for (int j = 0; j < currentKeeper.GetComponent<Fighter>().Dice.Length; j++)
                {                  
                    Transform diePosition = TileManager.Instance.DicePositionsOnTile.GetChild(i).GetChild(j);
                    diceInstance[currentKeeper].Add(DieBuilder.BuildDie(diceForCurrentThrow[currentKeeper][j], GameManager.Instance.ActiveTile, diePosition.localPosition + diePosition.parent.localPosition));
                }
            }
            if (BattleHandler.isPrisonerOnTile)
            {
                PawnInstance prisoner = GameManager.Instance.PrisonerInstance;

                diceForCurrentThrow.Add(prisoner, prisoner.GetComponent<Fighter>().Dice);

                // Create dice visuals
                diceInstance.Add(prisoner, new List<GameObject>());
                for (int j = 0; j < prisoner.GetComponent<Fighter>().Dice.Length; j++)
                {
                    Transform diePosition = TileManager.Instance.DicePositionsOnTile.GetChild(2).GetChild(j);
                    diceInstance[prisoner].Add(DieBuilder.BuildDie(diceForCurrentThrow[prisoner][j], GameManager.Instance.ActiveTile, diePosition.localPosition + diePosition.parent.localPosition));
                }
            }
        
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.thowDiceSound, 0.8f);
            isRunning = true;
            throwResult = ComputeNotPhysicalResult();

            //GetComponent<UIBattleHandler>().ChangeState(UIBattleState.DiceRolling);
        }
        else
        {
            Debug.LogWarning("A dice throw is already in process.");
        }
    }
    

    // Replace by Invoke with delay
    public void SendDataToBattleHandler()
    {
        BattleHandler.ReceiveDiceThrowData(throwResult, diceInstance);
        diceForCurrentThrow.Clear();
        throwResult.Clear();
    }

    private Dictionary<PawnInstance, Face[]> ComputeNotPhysicalResult()
    {
        Dictionary<PawnInstance, Face[]> results = new Dictionary<PawnInstance, Face[]>();
        foreach (PawnInstance piDice in diceForCurrentThrow.Keys)
        {
            Face[] result = new Face[diceForCurrentThrow[piDice].Length];
            upFaceIndex.Add(piDice, new List<int>());
            for (int i = 0; i < diceForCurrentThrow[piDice].Length; i++)
            {
                int j = Random.Range(0, 6);
                result[i] = diceForCurrentThrow[piDice][i].Faces[j];
                //RotateDie(diceInstance[piDice][i], j+1);
                diceInstance[piDice][i].GetComponentInChildren<Animator>().SetTrigger("startFace6Anim");

                upFaceIndex[piDice].Add(j+1);
            }
            results.Add(piDice, result);
        }

        return results;
    }

    private void RotateDie(GameObject dieToRotate, int upFace)
    {
        dieToRotate.GetComponentInChildren<Animator>().enabled = false;
        if (upFace == (int)DieFaceChildren.Back)
        {
            dieToRotate.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, -90);
            dieToRotate.transform.GetChild(0).localPosition -= dieToRotate.transform.GetChild(0).localPosition.y * Vector3.up;
        }
        else if (upFace == (int)DieFaceChildren.Front)
        {
            dieToRotate.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 90);
            dieToRotate.transform.GetChild(0).localPosition -= dieToRotate.transform.GetChild(0).localPosition.y * Vector3.up;
        }
        else if (upFace == (int)DieFaceChildren.Left)
        {
            dieToRotate.transform.GetChild(0).localEulerAngles = new Vector3(-90, 0, 0);
            dieToRotate.transform.GetChild(0).localPosition -= dieToRotate.transform.GetChild(0).localPosition.y * Vector3.up;

            //   dieToRotate.transform.GetChild(0).Rotate(transform.right, -90, Space.World);
        }
        else if (upFace == (int)DieFaceChildren.Right)
        {
            dieToRotate.transform.GetChild(0).localEulerAngles = new Vector3(90, 0, 0);
            dieToRotate.transform.GetChild(0).localPosition -= dieToRotate.transform.GetChild(0).localPosition.y * Vector3.up;

            //  dieToRotate.transform.GetChild(0).Rotate(transform.right, 90, Space.World);
        }
        else if (upFace == (int)DieFaceChildren.Down)
        {
            dieToRotate.transform.GetChild(0).localEulerAngles = new Vector3(180, 0, 0);
            dieToRotate.transform.GetChild(0).localPosition -= dieToRotate.transform.GetChild(0).localPosition.y * Vector3.up;


            //dieToRotate.transform.GetChild(0).Rotate(transform.right, 180, Space.World);
        }
        else
        {
            dieToRotate.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
            dieToRotate.transform.GetChild(0).localPosition -= dieToRotate.transform.GetChild(0).localPosition.y * Vector3.up;
        }
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            if (timerAnimation >= rollDiceAnimClip/2.0f - 0.1f && !areDiceRotatedProperly)
            {
                foreach (PawnInstance piDice in diceForCurrentThrow.Keys)
                {
                    for (int i = 0; i < diceForCurrentThrow[piDice].Length; i++)
                    {
                        RotateDie(diceInstance[piDice][i], upFaceIndex[piDice][i]);
                    }
                }
                areDiceRotatedProperly = true;
            }

            if (timerAnimation > rollDiceAnimClip / 2.0f)
            {
                PopDiceFeedbacks();
            }

            // TODO: replace the value by the dice rolling animation duration
            if (timerAnimation < rollDiceAnimClip/2.0f + 1.0f)
            {
                timerAnimation += Time.deltaTime;
            }
            else
            {
                timerAnimation = 0.0f;
                isRunning = false;               
                SendDataToBattleHandler();
            }
        }
    }

    void PopDiceFeedbacks()
    {
        if (areDiceFeedbacksInitialized)
            return;

        foreach (PawnInstance pi in throwResult.Keys)
        {
            for (int i = 0; i < throwResult[pi].Length; i++)
            {
                diceInstance[pi][i].GetComponentInChildren<DieFeedback>().PopFeedback(throwResult[pi][i], pi);
            }
        }
        areDiceFeedbacksInitialized = true;
    }

    #region Physical Throw
    // TODO link this function to the button
    //public void SendDataToBattleHandler()
    //{
    //    Face[] throwResult = GatherResults();
    //    BattleHandler.ReceiveDiceThrowData(throwResult);
    //    isRunning = false;
    //    diceForCurrentThrow = null;
    //    for (int i = 0; i < diceInstance.Length; i++)
    //        Destroy(diceInstance[i]);
    //}

    //private Face[] GatherResults()
    //{
    //    Face[] result = new Face[diceInstance.Length];
    //    for (int i = 0; i < diceInstance.Length; i++)
    //    {
    //        // TODO pretty ugly handling, the handler should receive the throw result instead
    //        for (int j = 0; j < diceInstance[i].transform.childCount; j++)
    //        {
    //            if (diceInstance[i].transform.GetChild(j).GetComponent<FaceComponent>().ScoredFace != null)
    //            {
    //                result[i] = diceInstance[i].transform.GetChild(j).GetComponent<FaceComponent>().ScoredFace;
    //                break;
    //            }
    //        }
    //    }
    //    return result;
    //}	
 //   void Update () {
 //       if (isRunning)
 //       {
 //           ShowButton(stoppedDice == diceForCurrentThrow.Length);
 //       }
	//}
    #endregion
}

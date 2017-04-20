using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThrownDiceHandler : MonoBehaviour {

    [SerializeField]
    private Transform dicePositions;

    Dictionary<PawnInstance, Die[]> diceForCurrentThrow = new Dictionary<PawnInstance, Die[]>();
    bool isRunning = false;
    int stoppedDice = 0;
    Dictionary<PawnInstance, List<GameObject>> diceInstance = new Dictionary<PawnInstance, List<GameObject>>();
    Dictionary<PawnInstance, Face[]> throwResult = new Dictionary<PawnInstance, Face[]>();
    float timerAnimation = 0.0f;

    public void InitThrow()
    {
        if (!isRunning)
        {
            for (int i = 0; i < BattleHandler.CurrentBattleKeepers.Length; i++)
            {
                PawnInstance currentKeeper = BattleHandler.CurrentBattleKeepers[i];
                diceForCurrentThrow.Add(currentKeeper, currentKeeper.GetComponent<Behaviour.Fighter>().Dice);

                // Create dice visuals
                diceInstance.Add(currentKeeper, new List<GameObject>());
                for (int j = 0; j < currentKeeper.GetComponent<Behaviour.Fighter>().Dice.Length; j++)
                {
                    
                    Transform diePosition = TileManager.Instance.DicePositionsOnTile.GetChild(i).GetChild(j);
                    diceInstance[currentKeeper].Add(DieBuilder.BuildDie(diceForCurrentThrow[currentKeeper][j], GameManager.Instance.ActiveTile, diePosition.localPosition + diePosition.parent.localPosition));
                }
            }

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
            for (int i = 0; i < diceForCurrentThrow[piDice].Length; i++)
            {
                int j = Random.Range(0, 5);
                result[i] = diceForCurrentThrow[piDice][i].Faces[j];
                // TODO: handle this with animations
                //diceInstance[piDice][i].GetComponent<Animator>().SetTrigger("startFace" + (j + 1) + "Anim");
                RotateDie(diceInstance[piDice][i], j + 1);
            }
            results.Add(piDice, result);
        }

        return results;
    }

    private void RotateDie(GameObject dieToRotate, int upFace)
    {
        if (upFace == (int)DieFaceChildren.Back)
        {
            dieToRotate.transform.Rotate(transform.right, -90);
        }
        else if (upFace == (int)DieFaceChildren.Front)
        {
            dieToRotate.transform.Rotate(transform.right, 90);
        }
        else if (upFace == (int)DieFaceChildren.Left)
        {
            dieToRotate.transform.Rotate(transform.forward, -90);
        }
        else if (upFace == (int)DieFaceChildren.Right)
        {
            dieToRotate.transform.Rotate(transform.forward, 90);
        }
        else if (upFace == (int)DieFaceChildren.Down)
        {
            dieToRotate.transform.Rotate(transform.right, 180);
        }
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            if (timerAnimation < 5.0f)
                timerAnimation += Time.deltaTime;
            else
            {
                timerAnimation = 0.0f;
                isRunning = false;
                SendDataToBattleHandler();
               // GetComponent<UIBattleHandler>().ChangeState(UIBattleState.);
            }
        }
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

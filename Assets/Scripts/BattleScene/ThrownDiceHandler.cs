using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ThrowType { BeginTurn, Attack, Defense, Special }

public class ThrownDiceHandler : MonoBehaviour {

    Dictionary<PawnInstance, Die[]> diceForCurrentThrow = new Dictionary<PawnInstance, Die[]>();
    bool isRunning = false;
    int stoppedDice = 0;
    Dictionary<PawnInstance, List<GameObject>> diceInstance = new Dictionary<PawnInstance, List<GameObject>>();
    Dictionary<PawnInstance, Face[]> throwResult = new Dictionary<PawnInstance, Face[]>();
    ThrowType currentThrowType;
    float timerAnimation = 0.0f;

    public void InitThrow(string type)
    {
        if (!isRunning)
        {
            for (int i = 0; i < BattleHandler.CurrentBattleKeepers.Length; i++)
            {
                diceForCurrentThrow.Add(BattleHandler.CurrentBattleKeepers[i], BattleHandler.CurrentBattleKeepers[i].GetComponent<Behaviour.Fighter>().Dice);
                // TODO: should create the die and set its owner 
                diceInstance.Add(BattleHandler.CurrentBattleKeepers[i], new List<GameObject>());
                for (int j = 0; j < BattleHandler.CurrentBattleKeepers[i].GetComponent<Behaviour.Fighter>().Dice.Length; j++)
                    diceInstance[BattleHandler.CurrentBattleKeepers[i]].Add(new GameObject());

            }

            Vector3 throwPosition = new Vector3(0.0f, 0.5f, 0.0f);

            for (int i = 0; i < diceForCurrentThrow.Count; i++)
            {
              //  diceInstance[i] = DieBuilder.BuildDie(diceForCurrentThrow[i], throwTile, throwPosition + (Vector3.right*i)/5.0f);
            }
            isRunning = true;

            throwResult = ComputeNotPhysicalResult();
            GetComponent<UIBattleHandler>().ChangeState(UIBattleState.DiceRolling);
        }
        else
        {
            Debug.LogWarning("A dice throw is already in process.");
        }
    }

    // Replace by Invoke with delay
    public void SendDataToBattleHandler()
    {
        BattleHandler.ReceiveDiceThrowData(throwResult, currentThrowType);
        diceForCurrentThrow.Clear();
        
        throwResult.Clear();
        foreach (PawnInstance pi in diceInstance.Keys)
        {
            for (int i = 0; i < diceInstance[pi].Count; i++)
                Destroy(diceInstance[pi][i]);
        }
    }

    private void ShowButton(bool show)
    {
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SendDiceResultsButton.SetActive(show);
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
                diceInstance[piDice][i].GetComponent<Animator>().SetTrigger("startFace" + (j + 1) + "Anim");
            }
            results.Add(piDice, result);
        }

        return results;
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
                GetComponent<UIBattleHandler>().ChangeState(UIBattleState.WaitForDiceThrowValidation);
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

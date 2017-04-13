using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ThrowType { BeginTurn, Attack, Defense, Special }

public class ThrownDiceHandler : MonoBehaviour {

    Die[] diceForCurrentThrow;
    bool isRunning = false;
    int stoppedDice = 0;
    GameObject[] diceInstance;
    Face[] throwResult;
    ThrowType currentThrowType;
    string[] animationNames;

    public void InitThrow(string type)
    {
        if (!isRunning)
        {
            diceForCurrentThrow = BattleHandler.CurrentPawnTurn.GetComponent<Behaviour.Fighter>().Dice;
            diceInstance = new GameObject[diceForCurrentThrow.Length];

            for (int i = 0; i < diceForCurrentThrow.Length; i++)
            {
                diceInstance[i] = DieBuilder.BuildDie(diceForCurrentThrow[i]);
            }
            isRunning = true;

            throwResult = ComputeNotPhysicalResult();
            currentThrowType = (ThrowType)Enum.Parse(typeof(ThrowType), type);
            GetComponent<UIBattleHandler>().ChangeState(UIBattleState.DiceRolling);
        }
        else
        {
            Debug.LogWarning("A dice throw is already in process.");
        }
    }

    public void SendDataToBattleHandler()
    {
        BattleHandler.ReceiveDiceThrowData(throwResult, currentThrowType);
        diceForCurrentThrow = null;
        throwResult = null;
        for (int i = 0; i < diceInstance.Length; i++)
            Destroy(diceInstance[i]);
    }

    private void ShowButton(bool show)
    {
        GameManager.Instance.GetBattleUI().GetComponent<UIBattleHandler>().SendDiceResultsButton.SetActive(show);
    }

    private Face[] ComputeNotPhysicalResult()
    {
        Face[] result = new Face[diceForCurrentThrow.Length];
        animationNames = new string[diceForCurrentThrow.Length];

        for (int i = 0; i < diceForCurrentThrow.Length; i++)
        {
            int j = Random.Range(0, 5);
            result[i] = diceForCurrentThrow[i].Faces[j];
            diceInstance[i].GetComponent<Animator>().SetTrigger("startFace" + (j + 1) + "Anim");
            animationNames[i] = "FallOnFace" + (j + 1);
        }
        return result;
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            int nbrOfAnimationsFinished = 0;
            for (int i = 0; i < diceInstance.Length; i++)
            {
                if (!diceInstance[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(animationNames[i]))
                    nbrOfAnimationsFinished++;
            }
            if (nbrOfAnimationsFinished == diceInstance.Length)
            {
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

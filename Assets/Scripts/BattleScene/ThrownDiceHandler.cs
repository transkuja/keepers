using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownDiceHandler : MonoBehaviour {

    Die[] diceForCurrentThrow;
    bool isRunning = false;
    int stoppedDice = 0;
    GameObject[] diceInstance;

    public void InitThrow(Die[] diceForThrow)
    {
        if (!isRunning)
        {
            diceForCurrentThrow = diceForThrow;
            diceInstance = new GameObject[diceForCurrentThrow.Length];

            for (int i = 0; i < diceForCurrentThrow.Length; i++)
            {
                diceInstance[i] = DieBuilder.BuildDie(diceForCurrentThrow[i]);
            }
            isRunning = true;
        }
        else
        {
            Debug.LogWarning("A dice throw is already in process.");
        }
    }

	void Update () {
        if (isRunning)
        {
            ShowButton(stoppedDice == diceForCurrentThrow.Length);
        }
	}

    private void ShowButton(bool show)
    {
        // TODO show/mask validation button
    }

    // TODO link this function to the button
    public void SendDataToBattleHandler()
    {
        Face[] throwResult = GatherResults();
        BattleHandler.ReceiveDiceThrowData(throwResult);
        isRunning = false;
        diceForCurrentThrow = null;
        for (int i = 0; i < diceInstance.Length; i++)
            Destroy(diceInstance[i]);
    }

    private Face[] GatherResults()
    {
        Face[] result = new Face[diceInstance.Length];
        for (int i = 0; i < diceInstance.Length; i++)
        {
            // TODO pretty ugly handling, the handler should receive the throw result instead
            for (int j = 0; j < diceInstance[i].transform.childCount; j++)
            {
                if (diceInstance[i].transform.GetChild(j).GetComponent<FaceComponent>().ScoredFace != null)
                {
                    result[i] = diceInstance[i].transform.GetChild(j).GetComponent<FaceComponent>().ScoredFace;
                    break;
                }
            }
        }
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownDiceHandler : MonoBehaviour {

    Die[] diceForCurrentThrow;
    bool isRunning = false;
    int stoppedDice = 0;

    // isRunning
    // Init(diceForCurrentThrow)
    // build dice
    // throw 'em
    // Update check dice status
    // RollEnded method, clean the handler, send data to battle handler

    public void InitThrow(Die[] diceForThrow)
    {
        if (!isRunning)
        {
            diceForCurrentThrow = diceForThrow;
            for (int i = 0; i < diceForCurrentThrow.Length; i++)
            {
                DieBuilder.BuildDie(diceForCurrentThrow[i]);
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

    public void SendDataToBattleHandler()
    {
        // TODO link this function to the button
        // BattleHandler.ReceiveData();
        isRunning = false;
        diceForCurrentThrow = null;
        // DieBuilder.DestroyDice();
    }
}

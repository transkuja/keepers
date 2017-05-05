using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePause : MonoBehaviour {


    public void OnDisable()
    {
        if (GameManager.Instance.CurrentState != GameState.InTuto || GameManager.Instance.CurrentState != GameState.InBattle)
            GameManager.Instance.CurrentState = GameState.Normal;
    }
}

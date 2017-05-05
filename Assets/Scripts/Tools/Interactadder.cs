using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Interactadder : MonoBehaviour {
	void Start () {
        TileTrigger[] trigger = GetComponentsInChildren<TileTrigger>();
        for(int i = 0; i < trigger.Length; i++)
        {
            trigger[i].gameObject.AddComponent<Interactable>();
        }
	}
}

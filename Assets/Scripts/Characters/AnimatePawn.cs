using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatePawn : MonoBehaviour {
    Animator anim;
    NavMeshAgent agent;
	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("velocity", agent.velocity.magnitude);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class dickMove : MonoBehaviour {

    private NavMeshAgent agent;
    public Transform destination;
    public MenuManager menuManager;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        GetComponent<Behaviour.AnimatedPawn>().Agent.SetDestination(destination.position);

        ChatBox newChatBox = Instantiate(menuManager.PrefabChatox, menuManager.GetComponentInChildren<Canvas>().transform).GetComponent<ChatBox>();
        newChatBox.trTarget = transform;
        newChatBox.SetMode(ChatBox.ChatMode.whoAmI);
        newChatBox.SetEnable(true);
        menuManager.DicPawnChatBox.Add(gameObject, newChatBox);
    }
}

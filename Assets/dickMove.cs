using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class dickMove : MonoBehaviour {

    private NavMeshAgent agent;
    public Transform destination;
    public Transform destination2;
    public MenuManager menuManager;

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        for(int i =0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>() != null)
            {
                transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.SetDestination(new Vector3(transform.GetChild(i).localPosition.x, transform.GetChild(i).localPosition.y, destination.localPosition.z));


                ChatBox chatte = Instantiate(menuManager.PrefabChatox, menuManager.GetComponentInChildren<Canvas>().transform).GetComponent<ChatBox>();
                chatte.trTarget = transform.GetChild(i);
                //newChatBox.SetMode(ChatBox.ChatMode.whoAmI);
                chatte.SetEnable(true);
            }
        }



    }

    public void Update()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>() != null)
            {
                if (!transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.pathPending)
                {
                    if (transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.remainingDistance <= transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.stoppingDistance)
                    {
                        if (!transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.hasPath || transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.velocity.sqrMagnitude == 0f)
                        {
                            // Done
                            if (!menuManager.DuckhavebringThebox)
                            {

                                menuManager.DuckhavebringThebox = true;
                                Invoke("Boom", 5);
                            }
                            //chatte.Say(ChatBox.ChatMode.whoAmI, 0);
                            transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.SetDestination(new Vector3(transform.GetChild(i).localPosition.x, transform.GetChild(i).localPosition.y, destination2.localPosition.z));
                        }
                    }
                }
            }
        }


        // Check if we've reached the destination
       
    }

    public void Boom()
    {

        GameObject.Find("Box").transform.SetParent(null);
        GameObject.Find("Box").transform.localPosition = Vector3.zero;
        GameObject.Find("Box").transform.localRotation = Quaternion.identity;
        GameObject.Find("Box").transform.localScale = Vector3.one;
    }
}

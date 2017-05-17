using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class dickMove : MonoBehaviour {
    public Transform destination;
    public Transform destination2;
    public MenuManager menuManager;
    private ChatBox chatte;
    private bool once;

    // Use this for initialization
    void Start () {
        for (int i =0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>() != null)
            {
                transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.SetDestination(new Vector3(transform.GetChild(i).localPosition.x, transform.GetChild(i).localPosition.y, destination.localPosition.z));

                if (i == 5)
                {
                    chatte = Instantiate(menuManager.PrefabChatox, menuManager.GetComponentInChildren<Canvas>().transform).GetComponent<ChatBox>();
                    chatte.trTarget = transform.GetChild(i);
                    //newChatBox.SetMode(ChatBox.ChatMode.whoAmI);
                    chatte.SetEnable(true);
                }
      
            }
        }

        once = false;
        menuManager.DuckhavebringThebox = false;

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
                            //if (!menuManager.DuckhavebringThebox)
                            //{

                            //    menuManager.DuckhavebringThebox = true;
                     
                            if ( i == 4 && !once)
                            {
                                once = true;
  
                                Invoke("Boom", 1);
                                Invoke("SaySomething", 1);
                            }
                            //}
                            //chatte.Say(ChatBox.ChatMode.whoAmI, 0);
                            //transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.SetDestination(new Vector3(transform.GetChild(i).localPosition.x, transform.GetChild(i).localPosition.y, destination2.localPosition.z));
                        }
                    }
                }
            }
        }


        // Check if we've reached the destination
       
    }

    public void Boom()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>() != null)
            {
                transform.GetChild(i).GetComponent<Behaviour.AnimatedPawn>().Agent.SetDestination(new Vector3(transform.GetChild(i).localPosition.x, transform.GetChild(i).localPosition.y, destination2.localPosition.z));


            }

        }
    }

    public void SaySomething()
    {
        chatte.Say(ChatBox.ChatMode.whoAmI, 6);
    }
}

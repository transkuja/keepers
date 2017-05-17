using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class boxMove : MonoBehaviour {

    private NavMeshAgent agent;
    public Transform destination;
    public MenuManager menuManager;

    // Use this for initialization
    void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.SetDestination(new Vector3(transform.localPosition.x, transform.localPosition.y, destination.localPosition.z));
        }
    }

    public void Update()
    {
        if (agent != null)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        // Done
                        if (!menuManager.DuckhavebringThebox)
                        {
                            Invoke("Boom", 1);
                        }
                    }
                }
            }
        }

        // Check if we've reached the destination

    }

    public void Boom()
    {

        //transform.SetParent(null);
        GetComponent<Animator>().SetTrigger("dropBox");

        //menuManager.GetComponent<BoxOpener>().boxLock.GetComponent<GlowObjectCmd>().UpdateColor(true);


    }

    public void Bite(){
        GlowController.RegisterObject(menuManager.GetComponent<BoxOpener>().boxLock.GetComponent<GlowObjectCmd>());
        menuManager.DuckhavebringThebox = true;
    }
}

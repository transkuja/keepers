using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AggroBehaviour : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KeeperInstance>() != null
            || other.GetComponent<PrisonerInstance>() != null)
        {
            GetComponentInParent<NavMeshAgent>().ResetPath();
            GetComponentInParent<NavMeshAgent>().SetDestination(other.transform.position);
        }
    }
}

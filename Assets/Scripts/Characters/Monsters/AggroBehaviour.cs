using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AggroBehaviour : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponentInParent<KeeperInstance>() != null && other.GetComponentInParent<KeeperInstance>().IsTargetableByMonster == true)
            || (other.GetComponentInParent<PrisonerInstance>() != null && other.GetComponentInParent<PrisonerInstance>().IsTargetableByMonster == true))
        {
            GetComponentInParent<NavMeshAgent>().ResetPath();
            GetComponentInParent<NavMeshAgent>().SetDestination(other.transform.position);
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

public class AggroBehaviour : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.CurrentState == GameState.Normal)
        {
            if (other.GetComponentInParent<Behaviour.Fighter>() != null && other.GetComponentInParent<Behaviour.Fighter>().IsTargetableByMonster == true
                && !other.GetComponentInParent<Behaviour.Fighter>().IsAMonster)
            {
                NavMeshAgent agent = GetComponentInParent<NavMeshAgent>();
                if (agent != null && agent.enabled)
                {
                    agent.ResetPath();
                    agent.SetDestination(other.transform.position);
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.AI;


namespace Behaviour
{
    public class Escortable : MonoBehaviour
    {
        PawnInstance instance;

        //  Escort
        public bool isEscortAvailable = true;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            instance.Interactions.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.SpriteUtils.spriteEscort);
            instance.Interactions.Add(new Interaction(UnEscort), 0, "Unescort", GameManager.Instance.SpriteUtils.spriteUnescort, false);
        }

        public void Escort(int _i = 0)
        {
            // Ne va fonctionner que pour le prisonnier
            GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Keeper>().GoListCharacterFollowing.Add(gameObject);
            GetComponent<NavMeshAgent>().stoppingDistance = 0.75f;
            GetComponent<NavMeshAgent>().avoidancePriority = 80;
            isEscortAvailable = false;

        }

        public void UnEscort(int _i = 0)
        {
            GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Keeper>().GoListCharacterFollowing.Remove(gameObject);
            isEscortAvailable = true;
            GetComponent<NavMeshAgent>().avoidancePriority = 50;
        }

    }
}
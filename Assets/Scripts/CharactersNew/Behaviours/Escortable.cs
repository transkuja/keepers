using UnityEngine;
using UnityEngine.AI;


namespace Behaviour
{
    public class Escortable : MonoBehaviour
    {
        instance = GetComponent<CharacterInstance>();
        instance.Interactions.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.SpriteUtils.spriteEscort);
        instance.Interactions.Add(new Interaction(UnEscort), 0, "Unescort", GameManager.Instance.SpriteUtils.spriteUnescort, false);
    }

        CharacterInstance instance;

        void Start()
        {
            instance = GetComponent<CharacterInstance>();
            instance.Interactions.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.Ui.spriteEscort);
            instance.Interactions.Add(new Interaction(UnEscort), 0, "Unescort", GameManager.Instance.Ui.spriteUnescort, false);
        }

        public void Escort(int _i = 0)
        {
            for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
            {
                GameManager.Instance.AllKeepersList[i].Keeper.GoListCharacterFollowing.Clear();
                GameManager.Instance.AllKeepersList[i].isEscortAvailable = true;
            }
            // Ne va fonctionner que pour le prisonnier
            GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Add(GameManager.Instance.GoTarget.GetComponentInParent<PrisonerInstance>().gameObject);
            GetComponent<NavMeshAgent>().stoppingDistance = 0.75f;
            GetComponent<NavMeshAgent>().avoidancePriority = 80;
            GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable = false;

        }

        public void UnEscort(int _i = 0)
        {
            GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Remove(this.gameObject);
            GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable = true;
            GetComponent<NavMeshAgent>().avoidancePriority = 50;
        }

    }
}
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Behaviour
{
    public class Escortable : MonoBehaviour
    {
        PawnInstance instance;

        // UI
        public GameObject shorcutUI;

        //  Escort
        public bool isEscortAvailable = true;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            instance.Interactions.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.SpriteUtils.spriteEscort);
            instance.Interactions.Add(new Interaction(UnEscort), 0, "Unescort", GameManager.Instance.SpriteUtils.spriteUnescort, false);

            CreateShortcutEscortUI();
        }

        public void Escort(int _i = 0)
        {
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

        public void CreateShortcutEscortUI()
        {
            shorcutUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabShorcutCharacter, GameManager.Instance.Ui.goShortcutKeepersPanel.transform);
            shorcutUI.transform.localScale = Vector3.one;
            shorcutUI.transform.localPosition = Vector3.zero;

            // ? ? 
            shorcutUI.GetComponent<Button>().onClick.AddListener(() => GoToEscorted());
        }

        public void GoToEscorted()
        {
            GameManager.Instance.CameraManager.UpdateCameraPosition(instance);
        }
    }
}
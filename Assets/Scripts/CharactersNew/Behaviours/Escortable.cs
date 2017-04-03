using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Behaviour
{
    public class Escortable : MonoBehaviour
    {
        PawnInstance instance;

        // UI
        private GameObject shorcutUI;

        //  Escort
        public bool isEscortAvailable = true;

        private bool isEscorted = false;
        public Keeper escort;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();

            CreateShortcutEscortUI();
        }

        void Start()
        {
            instance.Interactions.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.SpriteUtils.spriteEscort);
        }

        public void Escort(int _i = 0)
        {
            if (escort != null)
                escort.GoListCharacterFollowing.Remove(gameObject);

            escort = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Keeper>();
            escort.GetComponent<Keeper>().GoListCharacterFollowing.Add(gameObject);
            IsEscorted = true;

            GetComponent<NavMeshAgent>().stoppingDistance = 0.75f;
            GetComponent<NavMeshAgent>().avoidancePriority = 80;
        }

        public void UnEscort(int _i = 0)
        {
            escort.GoListCharacterFollowing.Remove(gameObject);
            escort = null;
            IsEscorted = false;
            GetComponent<NavMeshAgent>().avoidancePriority = 50;
            ActivateEscortAction();
        }

        public void CreateShortcutEscortUI()
        {
            Sprite associatedSprite = instance.Data.AssociatedSprite;
            ShorcutUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabShorcutCharacter, GameManager.Instance.Ui.goShortcutKeepersPanel.transform);
            ShorcutUI.transform.localScale = Vector3.one;
            ShorcutUI.transform.localPosition = Vector3.zero;
            ShorcutUI.transform.GetChild((int)PanelShortcutChildren.Image).GetComponent<Image>().sprite = associatedSprite;

            // ? ? 
            ShorcutUI.GetComponent<Button>().onClick.AddListener(() => GoToEscorted());
        }
        
        public void GoToEscorted()
        {
            GameManager.Instance.CameraManager.UpdateCameraPosition(instance);
        }

        public bool IsEscorted
        {
            get
            {
                return isEscorted;
            }

            set
            {
                isEscorted = value;          
            }
        }

        public GameObject ShorcutUI
        {
            get
            {
                return shorcutUI;
            }

            set
            {
                shorcutUI = value;
            }
        }

        public void UpdateEscortableInteractions()
        {
            if (isEscorted)
            {
                if (GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Keeper>() == escort)
                {
                    ActivateUnescortAction();
                }
                else
                {
                    ActivateEscortAction();
                }
            }
        }

        private void ActivateEscortAction()
        {
            if (instance.Interactions.Get("Escort") == null)
            {
                instance.Interactions.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.SpriteUtils.spriteEscort);
                instance.Interactions.Remove("Unescort");
            }
        }

        private void ActivateUnescortAction()
        {
            instance.Interactions.Add(new Interaction(UnEscort), 0, "Unescort", GameManager.Instance.SpriteUtils.spriteUnescort, false);
            instance.Interactions.Remove("Escort");
        }


    }
}
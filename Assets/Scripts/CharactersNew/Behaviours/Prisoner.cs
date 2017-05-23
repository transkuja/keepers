using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Prisoner : MonoBehaviour
    {

        PawnInstance instance;
        bool isSelected = false;
        [SerializeField]
        private GameObject feedbackSelection;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            GameManager.Instance.RegisterPrisoner(instance);
        }

        public void ProcessFeeding()
        {
            Inventory inv = GetComponent<Inventory>();
            inv.Items[0].Item.UseItem(inv.Items[0], GetComponent<PawnInstance>());
            inv.Items[0].Quantity--;

            if (inv.Items[0] == null || inv.Items[0].Quantity <= 0)
            {
                if (inv.Items[0] != null)
                {
                    InventoryManager.RemoveItem(inv.Items, inv.Items[0]);
                }
            }

            inv.UpdateInventories();  
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;

                if (GameManager.Instance.CurrentState == GameState.InBattle ||
                    (GameManager.Instance.CurrentState == GameState.InTuto && TutoManager.s_instance.StateBeforeTutoStarts == GameState.InBattle))
                {
                    feedbackSelection.SetActive(false);
                    BattleHandler.DeactivateFeedbackSelection(true, false);
                }

                GameManager.Instance.Ui.ClearActionPanel();
            }

        }

        public GameObject FeedbackSelection
        {
            get
            {
                return feedbackSelection;
            }

            set
            {
                feedbackSelection = value;
            }
        }
    }
}
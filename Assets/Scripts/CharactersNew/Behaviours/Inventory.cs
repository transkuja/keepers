using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Inventory : MonoBehaviour
    {
        CharacterInstance instance;

        private List<ItemContainer> items;
        private GameObject inventoryPanel;

        public List<ItemContainer> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }

        public GameObject InventoryPanel
        {
            get
            {
                return inventoryPanel;
            }

            set
            {
                inventoryPanel = value;
            }
        }

        void Start()
        {
            instance = GetComponent<CharacterInstance>();
            instance.Interactions.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.MenuUi.spriteTrade);
            items = new List<ItemContainer>();
        }

        public void Trade(int _i = 0)
        {
            inventoryPanel.SetActive(true);
            GameManager.Instance.Ui.UpdateInventoryPanel(gameObject);
        }
    }
}
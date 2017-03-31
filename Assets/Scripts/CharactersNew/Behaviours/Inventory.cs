using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    [RequireComponent(typeof(PawnInstance))]
    public class Inventory : MonoBehaviour
    {
        [System.Serializable]
        public class InventoryData : ComponentData
        {
            [SerializeField]
            int nbSlot;

            public InventoryData(int _nbSlot = 0)
            {
                nbSlot = _nbSlot;
            }

            public int NbSlot
            {
                get
                {
                    return nbSlot;
                }

                set
                {
                    nbSlot = value;
                }
            }
        }

        PawnInstance instance;

        [SerializeField]
        InventoryData data;
        //int nbSlot;   // OLD WITHOUT DATA
        private ItemContainer[] items;
        private GameObject inventoryPanel;

        [SerializeField]
        List<string> possibleItems;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            instance.Interactions.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.SpriteUtils.spriteTrade);

            // TODO remove
            Init(data.NbSlot);
        }

        public ItemContainer[] Items
        {
            get
            {
                return items;
            }

            set
            {
                if(value != null)
                {
                    data.NbSlot = value.Length;
                }
                else
                {
                    data.NbSlot = 0;
                }
            
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

        /*public int NbSlot 
        {
            get
            {
                return nbSlot;
            }

            set
            {
                nbSlot = value;
            }
        }*/ // OLD WITHOUT DATA

        public InventoryData Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public void Init(int slotCount)
        {
            data.NbSlot = slotCount;
            items = new ItemContainer[slotCount];
        }

        public void Add(ItemContainer item)
        {
            ItemContainer[] temp = items;
            items = new ItemContainer[data.NbSlot];
            for (int i = 0; i < data.NbSlot; i++)
            {
                items[i] = temp[i];
            }
            items[data.NbSlot] = item;
            data.NbSlot++;
        }

        public void ComputeItems()
        {
            items = new ItemContainer[data.NbSlot];
            Item it = null;
            int i = 0;
            foreach (string _IdItem in possibleItems)
            {
                it = GameManager.Instance.Database.getItemById(_IdItem);
                if (Random.Range(0, 10) > it.Rarity)
                {
                    items[i++] = new ItemContainer(it, 1);
                }
                if (i >= data.NbSlot)
                    break;
            }

        }

        public void Trade(int _i = 0)
        {
            inventoryPanel.SetActive(true);
            GameManager.Instance.Ui.UpdateInventoryPanel(gameObject);
        }
    }
}
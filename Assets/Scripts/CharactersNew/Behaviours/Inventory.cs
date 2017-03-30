using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    [RequireComponent(typeof(PawnInstance))]
    public class Inventory : MonoBehaviour
    {
        PawnInstance instance;

        [SerializeField]
        int nbSlot;
        private ItemContainer[] items;
        private GameObject inventoryPanel;

        [SerializeField]
        List<string> possibleItems;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            instance.Interactions.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.SpriteUtils.spriteTrade);

            // TODO remove
            Init(nbSlot);
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
                    NbSlot = value.Length;
                }
                else
                {
                    NbSlot = 0;
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

        public void Init(int slotCount)
        {
            NbSlot = slotCount;
            items = new ItemContainer[slotCount];
        }

        public void Add(ItemContainer item)
        {
            ItemContainer[] temp = items;
            items = new ItemContainer[NbSlot];
            for (int i = 0; i < NbSlot; i++)
            {
                items[i] = temp[i];
            }
            items[NbSlot] = item;
            NbSlot++;
        }

        public void ComputeItems()
        {
            items = new ItemContainer[NbSlot];
            Item it = null;
            int i = 0;
            foreach (string _IdItem in possibleItems)
            {
                it = GameManager.Instance.Database.getItemById(_IdItem);
                if (Random.Range(0, 10) > it.Rarity)
                {
                    items[i++] = new ItemContainer(it, 1);
                }
                if (i >= NbSlot)
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
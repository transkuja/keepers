using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public void Awake()
        {
            instance = GetComponent<PawnInstance>();
            CreateInventoryUI();
        }

        void Start()
        {
            instance.Interactions.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.SpriteUtils.spriteTrade);
            InitInventoryPanel();
            // TODO remove
            Init(data.NbSlot);

            ShowInventoryPanel(false);
        }

        public void Init(int slotCount)
        {
            data.NbSlot = slotCount;
            items = new ItemContainer[slotCount];
        }

        public void InitInventoryPanel()
        {
            GameObject owner = null;
            Sprite associatedSprite = null;
            string name = "";
            int nbSlots = 0;

            if (instance.GetComponent<PawnInstance>() != null)
            {
                PawnInstance pawnInstance = instance.GetComponent<PawnInstance>();
                associatedSprite = pawnInstance.Data.AssociatedSprite;
                name = pawnInstance.Data.PawnName;
                owner = pawnInstance.gameObject;
            }
            else if (instance.GetComponent<LootInstance>() != null)
            {
                LootInstance lootInstance = instance.GetComponent<LootInstance>();

                associatedSprite = GameManager.Instance.SpriteUtils.spriteLoot;
                inventoryPanel.transform.GetChild(0).gameObject.SetActive(false);
                owner = lootInstance.gameObject;
                name = "Loot";
            }
            else
            {
                return;
            }
            nbSlots = data.NbSlot;


            inventoryPanel.transform.localPosition = Vector3.zero;
            inventoryPanel.transform.localScale = Vector3.one;
            inventoryPanel.name = "Inventory_" + name;
            inventoryPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = associatedSprite;

            inventoryPanel.transform.GetChild(1).GetComponent<InventoryOwner>().Owner = instance.gameObject;
            inventoryPanel.SetActive(false);

            for (int i = 0; i < nbSlots; i++)
            {
                //Create Slots
                GameObject currentgoSlotPanel = Instantiate(GameManager.Instance.PrefabUtils.PrefabSlotUI, Vector3.zero, Quaternion.identity) as GameObject;
                currentgoSlotPanel.transform.SetParent(inventoryPanel.transform.GetChild(1).transform);

                currentgoSlotPanel.transform.localPosition = Vector3.zero;
                currentgoSlotPanel.transform.localScale = Vector3.one;
                currentgoSlotPanel.name = "Slot" + i;
            }
        }

        public void UpdateInventoryPanel()
        {
            GameObject owner = null;
            Sprite associatedSprite = null;
            string name = "";
            GameObject inventoryPanel = null;

            if (instance.GetComponent<PawnInstance>() != null)
            {
                PawnInstance pawnInstance = instance.GetComponent<PawnInstance>();
                associatedSprite = pawnInstance.Data.AssociatedSprite;
                name = pawnInstance.Data.PawnName;
                owner = pawnInstance.gameObject;
            }
            else if (instance.GetComponent<LootInstance>() != null)
            {
                LootInstance lootInstance = instance.GetComponent<LootInstance>();

                associatedSprite = GameManager.Instance.SpriteUtils.spriteLoot;
                inventoryPanel.transform.GetChild(0).gameObject.SetActive(false);
                owner = lootInstance.gameObject;
                name = "Loot";
            }
            else
            {
                return;
            }

            if (owner.GetComponent<Behaviour.Inventory>() != null && owner.GetComponent<Behaviour.Inventory>().Items != null)
            {
                ItemContainer[] inventory = owner.GetComponent<Behaviour.Inventory>().Items;
                for (int i = 0; i < inventory.Length; i++)
                {
                    GameObject currentSlot = inventoryPanel.transform.GetChild(1).GetChild(i).gameObject;
                    if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
                    {
                        Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
                    }

                }

                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] != null && inventory[i].Item != null && inventory[i].Item.Id != null)
                    {
                        GameObject currentSlot = inventoryPanel.transform.GetChild(1).GetChild(i).gameObject;
                        GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabItemUI);
                        go.transform.SetParent(currentSlot.transform);
                        go.GetComponent<ItemInstance>().ItemContainer = inventory[i];
                        go.name = inventory[i].ToString();

                        go.GetComponent<Image>().sprite = inventory[i].Item.InventorySprite;
                        go.transform.localScale = Vector3.one;

                        go.transform.position = currentSlot.transform.position;
                        go.transform.SetAsFirstSibling();

                        if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
                        {
                            go.transform.GetComponentInChildren<Text>().text = inventory[i].Quantity.ToString();
                        }
                        else
                        {
                            go.transform.GetComponentInChildren<Text>().text = "";
                        }
                    }
                }
            }
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
            //inventoryPanel.SetActive(true);
            //GameManager.Instance.Ui.UpdateInventoryPanel(gameObject);
            ShowInventoryPanel(true);
        }

        #region UI
        public void CreateInventoryUI()
        {
            inventoryPanel = Instantiate(GameManager.Instance.PrefabUtils.PrefabInventaireUI, GameManager.Instance.Ui.Panel_Inventories.transform);
        }

        public void ShowInventoryPanel(bool isShow)
        {
            inventoryPanel.SetActive(isShow);
        }
        #endregion

        #region Accessors
        public ItemContainer[] Items
        {
            get
            {
                return items;
            }

            set
            {
                if (value != null)
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

        #endregion
    }
}
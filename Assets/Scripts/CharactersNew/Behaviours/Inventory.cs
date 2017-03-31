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
        private GameObject selectedInventoryPanel;

        [SerializeField]
        List<string> possibleItems;

        void Start()
        {
            instance = GetComponent<PawnInstance>();

            if ( instance != null)
            {
                Init(data.NbSlot);
            }
        }

        public void Init(int slotCount)
        {
            data.NbSlot = slotCount;
            items = new ItemContainer[slotCount];

            CreateInventoryUI();
            InitInventoryPanel();

            ShowInventoryPanel(false);

            if (instance != null)
            {
                instance.Interactions.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.SpriteUtils.spriteTrade);

                if (instance.GetComponent<Keeper>() != null)
                {
                    CreateSelectedInventoryPanel();
                    selectedInventoryPanel.name = "Panel_Inventory" + instance.Data.PawnName;
                    InitSelectedInventoryPanel();

                    selectedInventoryPanel.transform.SetParent(instance.GetComponent<Keeper>().selectedPanelUI.transform);
                    selectedInventoryPanel.transform.localScale = Vector3.one;
                    selectedInventoryPanel.transform.localPosition = new Vector3(800, 0, 0);
                } else if (instance.GetComponent<Prisoner>() != null)
                {
                    GameObject button = Instantiate(GameManager.Instance.PrefabUtils.PrefabConfimationButtonUI, inventoryPanel.transform);
    
                    button.GetComponent<Button>().onClick.AddListener(instance.GetComponent<Prisoner>().ProcessFeeding);
                    button.transform.localScale = Vector3.one;
                    button.transform.localPosition = Vector3.zero;
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

        public void CreateSelectedInventoryPanel()
        {
            selectedInventoryPanel = Instantiate(GameManager.Instance.PrefabUtils.PrefabSelectedInventoryUIPanel);
            selectedInventoryPanel.GetComponent<InventoryOwner>().Owner = instance.gameObject;
        }

        public void ShowInventoryPanel(bool isShow)
        {
            inventoryPanel.SetActive(isShow);
        }

        public void InitInventoryPanel()
        {
            GameObject owner = null;
            Sprite associatedSprite = null;
            string name = "";
            int nbSlot = 0;

            if ( instance!= null && instance.GetComponent<PawnInstance>() != null)
            {
                PawnInstance pawnInstance = instance.GetComponent<PawnInstance>();
                associatedSprite = pawnInstance.Data.AssociatedSprite;
                name = pawnInstance.Data.PawnName;
                owner = pawnInstance.gameObject;
                nbSlot = data.NbSlot;
            }
            else if (GetComponent<LootInstance>() != null)
            {
                LootInstance lootInstance = GetComponent<LootInstance>();

                associatedSprite = GameManager.Instance.SpriteUtils.spriteLoot;
                inventoryPanel.transform.GetChild(0).gameObject.SetActive(false);
                owner = lootInstance.gameObject;
                name = "Loot";
                nbSlot = lootInstance.nbSlot;
            }
            else
            {
                return;
            }

            inventoryPanel.transform.localPosition = Vector3.zero;
            inventoryPanel.transform.localScale = Vector3.one;
            inventoryPanel.name = "Inventory_" + name;
            inventoryPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = associatedSprite;

            inventoryPanel.transform.GetChild(1).GetComponent<InventoryOwner>().Owner = owner;

            for (int i = 0; i < nbSlot; i++)
            {
                //Create Slots
                GameObject currentgoSlotPanel = Instantiate(GameManager.Instance.PrefabUtils.PrefabSlotUI, Vector3.zero, Quaternion.identity) as GameObject;
                currentgoSlotPanel.transform.SetParent(inventoryPanel.transform.GetChild(1).transform);

                currentgoSlotPanel.transform.localPosition = Vector3.zero;
                currentgoSlotPanel.transform.localScale = Vector3.one;
                currentgoSlotPanel.name = "Slot" + i;
            }
        }

        public void InitSelectedInventoryPanel()
        {
            PawnInstance pawnInstance = instance.GetComponent<PawnInstance>();
            Sprite associatedSprite = pawnInstance.Data.AssociatedSprite;
            name = pawnInstance.Data.PawnName;
            GameObject owner = pawnInstance.gameObject;

            selectedInventoryPanel.transform.localPosition = Vector3.zero;
            selectedInventoryPanel.transform.localScale = Vector3.one;
            selectedInventoryPanel.name = "Inventory_" + name;

            selectedInventoryPanel.GetComponent<InventoryOwner>().Owner = instance.gameObject;

            for (int i = 0; i < data.NbSlot; i++)
            {
                //Create Slots
                GameObject currentgoSlotPanel = Instantiate(GameManager.Instance.PrefabUtils.PrefabSlotUI, Vector3.zero, Quaternion.identity) as GameObject;
                currentgoSlotPanel.transform.SetParent(selectedInventoryPanel.transform);

                currentgoSlotPanel.transform.localPosition = Vector3.zero;
                currentgoSlotPanel.transform.localScale = Vector3.one;
                currentgoSlotPanel.name = "Slot" + i;
            }

            selectedInventoryPanel.GetComponent<GridLayoutGroup>().constraintCount = data.NbSlot;
        }

        public void UpdateInventory()
        {
            UpdateInventoryPanel();
            if (instance != null && instance.GetComponent<Keeper>() != null)
            {
                UpdateSelectedPanel();
            }
        }

        public void UpdateInventoryPanel()
        {
            GameObject owner = null;
            Sprite associatedSprite = null;
            string name = "";

            if (instance != null && instance.GetComponent<PawnInstance>() != null)
            {
                PawnInstance pawnInstance = instance.GetComponent<PawnInstance>();
                associatedSprite = pawnInstance.Data.AssociatedSprite;
                name = pawnInstance.Data.PawnName;
                owner = pawnInstance.gameObject;
            }
            else if (GetComponent<LootInstance>() != null)
            {
                LootInstance lootInstance = GetComponent<LootInstance>();

                associatedSprite = GameManager.Instance.SpriteUtils.spriteLoot;
                owner = lootInstance.gameObject;
                name = "Loot";
            }
            else
            {
                return;
            }

            for (int i = 0; i < items.Length; i++)
            {
                GameObject currentSlot = inventoryPanel.transform.GetChild(1).GetChild(i).gameObject;
                if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
                {
                    Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
                }
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i].Item != null && items[i].Item.Id != null)
                {
        
                    GameObject currentSlot = inventoryPanel.transform.GetChild(1).GetChild(i).gameObject;
                    GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabItemUI);
                    go.transform.SetParent(currentSlot.transform);
                    go.GetComponent<ItemInstance>().ItemContainer = items[i];
                    go.name = items[i].ToString();

                    go.GetComponent<Image>().sprite = items[i].Item.InventorySprite;
                    go.transform.localScale = Vector3.one;

                    go.transform.position = currentSlot.transform.position;
                    go.transform.SetAsFirstSibling();

                    if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
                    {
                        go.transform.GetComponentInChildren<Text>().text = items[i].Quantity.ToString();
                    }
                    else
                    {
                        go.transform.GetComponentInChildren<Text>().text = "";
                    }
                }
            }
        }

        public void UpdateSelectedPanel()
        {
            int nbSlot = data.NbSlot;

            for (int i = 0; i < items.Length; i++)
            {
                GameObject currentSlot = selectedInventoryPanel.transform.GetChild(i).gameObject;
                if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
                {
                    Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
                }
            }

            for (int i = 0; i < nbSlot; i++)
            {
                GameObject currentSlot = selectedInventoryPanel.transform.GetChild(i).gameObject;
                if (items != null && items.Length > 0 && i < items.Length && items[i] != null && items[i].Item != null && items[i].Item.Id != null)
                {
                    GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabItemUI);
                    go.transform.SetParent(currentSlot.transform);
                    go.GetComponent<ItemInstance>().ItemContainer = items[i];
                    go.name = items[i].Item.ItemName;

                    go.GetComponent<Image>().sprite = items[i].Item.InventorySprite;
                    go.transform.localScale = Vector3.one;

                    go.transform.position = currentSlot.transform.position;
                    go.transform.SetAsFirstSibling();

                    if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
                    {
                        go.transform.GetComponentInChildren<Text>().text = items[i].Quantity.ToString();
                    }
                    else
                    {
                        go.transform.GetComponentInChildren<Text>().text = "";
                    }
                }
                currentSlot.transform.SetParent(selectedInventoryPanel.transform);
                currentSlot.transform.localScale = Vector3.one;
            }

            selectedInventoryPanel.GetComponent<GridLayoutGroup>().constraintCount = nbSlot;
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

        public GameObject SelectedInventoryPanel
        {
            get
            {
                return selectedInventoryPanel;
            }

            set
            {
                selectedInventoryPanel = value;
            }
        }

        #endregion
    }
}
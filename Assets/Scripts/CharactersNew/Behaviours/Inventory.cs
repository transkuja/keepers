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

        private ItemContainer[] items;

        [SerializeField]
        List<string> possibleItems;

        // UI
        private GameObject inventoryPanel;
        private GameObject selectedInventoryPanel;

        void Start()
        {
            instance = GetComponent<PawnInstance>();

            if ( instance != null)
            {
                InitUI(data.NbSlot);
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
                it = GameManager.Instance.ItemDataBase.getItemById(_IdItem);
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
        public void InitUI(int slotCount)
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
                    InitSelectedInventoryPanel();
                }
                else if (instance.GetComponent<Prisoner>() != null)
                {
                    GameObject button = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabConfimationButtonUI, inventoryPanel.transform);

                    button.GetComponent<Button>().onClick.AddListener(instance.GetComponent<Prisoner>().ProcessFeeding);
                    button.GetComponent<Button>().onClick.AddListener(() => inventoryPanel.SetActive(false));
                    button.transform.localScale = Vector3.one;
                    // TMP
                    button.transform.localPosition = new Vector3(0, -200, 0);
                }
            }
        }

        public void CreateInventoryUI()
        {
            inventoryPanel = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabInventaireUI, GameManager.Instance.PrefabUIUtils.PrefabInventaireUI.transform.position, GameManager.Instance.PrefabUIUtils.PrefabInventaireUI.transform.rotation);
            inventoryPanel.transform.SetParent(GameManager.Instance.Ui.Panel_Inventories.transform, false);
            inventoryPanel.name = "Inventory_" + (GetComponent<LootInstance>() != null ? "Loot" : instance.Data.PawnName);
        }

        public void CreateSelectedInventoryPanel()
        {
            selectedInventoryPanel = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabSelectedInventoryUIPanel, GameManager.Instance.PrefabUIUtils.PrefabSelectedInventoryUIPanel.transform.position, GameManager.Instance.PrefabUIUtils.PrefabSelectedInventoryUIPanel.transform.rotation);
            selectedInventoryPanel.GetComponent<InventoryOwner>().Owner = instance.gameObject;
            selectedInventoryPanel.transform.SetParent(instance.GetComponent<Keeper>().SelectedPanelUI.transform, false);
            selectedInventoryPanel.transform.localScale = Vector3.one;
            selectedInventoryPanel.name = "Inventory";
        }

        public void ShowInventoryPanel(bool isShow)
        {
            inventoryPanel.SetActive(isShow);
        }

        public void InitInventoryPanel()
        {
            GameObject owner = null;
            Sprite associatedSprite = null;
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
                nbSlot = lootInstance.nbSlot;
            }
            else
            {
                return;
            }

            inventoryPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = associatedSprite;
            inventoryPanel.transform.GetChild(1).GetComponent<InventoryOwner>().Owner = owner;

            for (int i = 0; i < nbSlot; i++)
            {
                //Create Slots
                GameObject currentgoSlotPanel = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabSlotUI, Vector3.zero, Quaternion.identity) as GameObject;
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
            GameObject owner = pawnInstance.gameObject;
            selectedInventoryPanel.GetComponent<InventoryOwner>().Owner = instance.gameObject;

            for (int i = 0; i < data.NbSlot; i++)
            {
                //Create Slots
                GameObject currentgoSlotPanel = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabSlotUI, Vector3.zero, Quaternion.identity) as GameObject;
                currentgoSlotPanel.transform.SetParent(selectedInventoryPanel.transform);

                currentgoSlotPanel.transform.localPosition = Vector3.zero;
                currentgoSlotPanel.transform.localScale = Vector3.one;
                currentgoSlotPanel.name = "Slot" + i;
            }

            selectedInventoryPanel.GetComponent<GridLayoutGroup>().constraintCount = data.NbSlot;
        }

        public void UpdateInventories()
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
                    GameObject go = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabItemUI);
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
                    GameObject go = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabItemUI);
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

            // Met a jour l'équipement aswell
            instance.GetComponent<Keeper>().UpdateEquipement();
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
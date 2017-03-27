using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickHandler : MonoBehaviour, IPointerClickHandler
{
    private ItemInstance ii;

    public void Start()
    {
        ii = GetComponent<ItemInstance>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int tap = eventData.clickCount;

        if (tap == 2)
        {
            // Only keeper can use items
            GameObject owner = eventData.pointerPress.GetComponentInParent<InventoryOwner>().Owner;
            if (owner.GetComponent<KeeperInstance>() == null 
                    || owner.GetComponent<KeeperInstance>() != GameManager.Instance.ListOfSelectedKeepers[0])
            {
                List<ItemContainer> selectedKeeperInventory = GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Inventory>().List_inventaire;
                int freeSlotIndex = InventoryManager.FindFreeSlot(selectedKeeperInventory, GameManager.Instance.ListOfSelectedKeepers[0].Keeper.nbSlot);
                if (freeSlotIndex != -1)
                {
                    InventoryManager.SwapItemBeetweenInventories(owner.GetComponent<Inventory>().List_inventaire, ii.transform.parent.GetSiblingIndex(), selectedKeeperInventory, freeSlotIndex);

                    Debug.Log("test");
                    // Destroy inventory if it is empty for loot
                    if (owner.GetComponent<LootInstance>() != null)
                    {
                        bool isEmpty = true;
                        for (int i = 0; i < owner.GetComponent<Inventory>().List_inventaire.Count; i++)
                        {
                            if (owner.GetComponent<Inventory>().List_inventaire[i] != null)
                            {
                                isEmpty = false;
                                break;
                            }
                        }
                        if (isEmpty)
                        {
                            if (owner.gameObject.GetComponentInChildren<Canvas>() != null)
                            {
                                owner.gameObject.GetComponentInChildren<Canvas>().transform.SetParent(null);
                            }
                            Destroy(owner.gameObject);

                            // Get the original parent
                            Transform previous = eventData.pointerDrag.GetComponentInParent<DragHandler>().transform;
                            InventoryOwner inventaireDequi = previous.GetComponentInParent<InventoryOwner>();
                            Destroy(inventaireDequi.GetComponentInParent<DragHandlerInventoryPanel>().gameObject);
                        }
                    }
                    GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
                    GameManager.Instance.Ui.UpdateInventoryPanel(owner);

                }
                return;
            }


            ii.ItemContainer.UseItem();
            if (ii.ItemContainer.Quantity <= 0)
            {
                InventoryManager.RemoveItem(owner.GetComponent<Inventory>().List_inventaire, ii.ItemContainer);
            }

            GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        }
    }
}

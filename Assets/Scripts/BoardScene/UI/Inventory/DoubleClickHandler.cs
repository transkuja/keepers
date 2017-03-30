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
            KeeperInstance owner = eventData.pointerPress.GetComponentInParent<InventoryOwner>().Owner.GetComponent<KeeperInstance>();
            if (owner == null 
                    || owner != GameManager.Instance.ListOfSelectedKeepersOld[0])
            {
                GameObject goOwner = eventData.pointerPress.GetComponentInParent<InventoryOwner>().Owner;
                ItemContainer[] selectedKeeperInventory = GameManager.Instance.ListOfSelectedKeepersOld[0].GetComponent<Behaviour.Inventory>().Items;
                int freeSlotIndex = InventoryManager.FindFreeSlot(selectedKeeperInventory);
                if (freeSlotIndex != -1)
                {
                    Behaviour.Inventory ownerInventory = goOwner.GetComponent<Behaviour.Inventory>();
                    InventoryManager.SwapItemBeetweenInventories(ownerInventory.Items, ii.transform.parent.GetSiblingIndex(), selectedKeeperInventory, freeSlotIndex);
                    // Destroy inventory if it is empty for loot
                    if (goOwner.GetComponent<LootInstance>() != null)
                    {
                        bool isEmpty = true;
                        for (int i = 0; i < ownerInventory.Items.Length; i++)
                        {
                            if (ownerInventory.Items[i] != null)
                            {
                                isEmpty = false;
                                break;
                            }
                        }
                        if (isEmpty)
                        {
                            if (goOwner.GetComponentInChildren<Canvas>() != null)
                            {
                                goOwner.GetComponentInChildren<Canvas>().transform.SetParent(null);
                            }
                            Destroy(goOwner.gameObject);

                            // Get the original parent
                            Transform previous = eventData.pointerDrag.GetComponentInParent<DragHandler>().transform;
                            InventoryOwner inventaireDequi = previous.GetComponentInParent<InventoryOwner>();
                            Destroy(inventaireDequi.GetComponentInParent<DragHandlerInventoryPanel>().gameObject);
                        }
                    }
                    GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
                    GameManager.Instance.Ui.UpdateInventoryPanel(goOwner);
                    GameManager.Instance.Ui.UpdatePrisonerFeedingPanel(goOwner);
                }
                return;
            }


            ii.ItemContainer.UseItem(owner);
            if (ii.ItemContainer.Quantity <= 0)
            {
                InventoryManager.RemoveItem(owner.GetComponent<Behaviour.Inventory>().Items, ii.ItemContainer);
            }

            GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        }
    }
}

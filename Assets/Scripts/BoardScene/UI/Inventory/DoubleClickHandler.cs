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
            Behaviour.Keeper owner = eventData.pointerPress.GetComponentInParent<InventoryOwner>().Owner.GetComponent<Behaviour.Keeper>();
            if (owner == null 
                    || owner != GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>())
            {
                GameObject goOwner = eventData.pointerPress.GetComponentInParent<InventoryOwner>().Owner;
                ItemContainer[] selectedKeeperInventory = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Inventory>().Items;
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
                    goOwner.GetComponent<Behaviour.Inventory>().UpdateInventory();
                    GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Inventory>().UpdateInventory();
                    GameManager.Instance.Ui.UpdatePrisonerFeedingPanel(goOwner);
                }
                return;
            }


            ii.ItemContainer.UseItem(owner.GetComponent<PawnInstance>());
            if (ii.ItemContainer.Quantity <= 0)
            {
                InventoryManager.RemoveItem(owner.GetComponent<Behaviour.Inventory>().Items, ii.ItemContainer);
            }

            owner.GetComponent<Behaviour.Inventory>().UpdateInventory();
            //GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        }
    }
}

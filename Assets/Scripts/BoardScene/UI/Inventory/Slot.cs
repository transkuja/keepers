using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{

    public GameObject currentItem
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public bool hasAlreadyAnItem
    {
        get
        {
            if (transform.childCount > 0) return true;
            return false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<DragHandler>() != null 
            && (!hasAlreadyAnItem || (currentItem != null && currentItem.GetComponent<DragHandler>() != null)))
        {
            // Get the original parent
            Transform previous = eventData.pointerDrag.GetComponent<DragHandler>().startParent;
            if (previous.GetComponentInParent<InventoryOwner>() != null && transform.GetComponentInParent<InventoryOwner>() != null)
            {
                //Ou on est ?
                InventoryOwner inventaireDequi = previous.GetComponentInParent<InventoryOwner>();
                InventoryOwner inventaireversqui = transform.GetComponentInParent<InventoryOwner>();

                ItemContainer[] inventoryKeeperDequi = inventaireDequi.Owner.GetComponent<Behaviour.Inventory>().Items;
                ItemContainer[] inventoryKeeperVersqui = inventaireversqui.Owner.GetComponent<Behaviour.Inventory>().Items;

                //Si les inventaires sont differents
                if (inventaireDequi != inventaireversqui)
                {
                    ItemSplitter itemSplitter = GameManager.Instance.Ui.itemSplitter.GetComponent<ItemSplitter>();
                    itemSplitter.inventoryFrom = inventaireDequi;
                    itemSplitter.inventoryTo = inventaireversqui;
                    itemSplitter.originSlot = previous;
                    itemSplitter.targetSlot = transform;
                    itemSplitter.selectedItem = eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer;
                    itemSplitter.uiItem = eventData.pointerDrag.gameObject;

                    ItemContainer itemDragged = eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer;

                    // Split items only if on the same item
                    if (hasAlreadyAnItem)
                    {
                        ItemContainer itemOn = currentItem.GetComponent<ItemInstance>().ItemContainer;

                        if (itemOn.Item.Id == itemDragged.Item.Id)
                        {
                            if (itemDragged.Quantity > 1)
                                itemSplitter.gameObject.SetActive(true);
                            else
                            {
                                InventoryManager.MergeStackables(currentItem.GetComponent<ItemInstance>().ItemContainer, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer);
                                InventoryManager.RemoveItem(inventoryKeeperDequi, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer);
                            }
                        }
                        else
                        {
                            // Auto swap
                            InventoryManager.SwapItemBeetweenInventories(inventoryKeeperDequi, previous.GetSiblingIndex(), inventoryKeeperVersqui, transform.GetSiblingIndex());
                        }
                    }
                    else
                    {
                        //Move the item to the slot
                        //eventData.pointerDrag.transform.SetParent(transform);
                        if (itemDragged.Quantity > 1)
                        {
                            itemSplitter.gameObject.SetActive(true);
                        }
                        else
                        {
                            InventoryManager.SwapItemBeetweenInventories(inventoryKeeperDequi, previous.GetSiblingIndex(), inventoryKeeperVersqui, transform.GetSiblingIndex());
                        }
                    }

                    // Destroy inventory if it is empty for loot
                    if (inventaireDequi.Owner.GetComponent<LootInstance>() != null)
                    {
                        bool isEmpty = true;
                        for (int i = 0; i < inventaireDequi.Owner.GetComponent<Behaviour.Inventory>().Items.Length; i++)
                        {
                            if (inventaireDequi.Owner.GetComponent<Behaviour.Inventory>().Items[i] != null)
                            {
                                isEmpty = false;
                                break;
                            }
                        }
                        if (isEmpty)
                        {
                            if (inventaireDequi.Owner.gameObject.GetComponentInChildren<Canvas>() != null)
                            {
                                inventaireDequi.Owner.gameObject.GetComponentInChildren<Canvas>().transform.SetParent(null);
                            }
                            Destroy(inventaireDequi.Owner.gameObject);
                            Destroy(inventaireDequi.GetComponentInParent<DragHandlerInventoryPanel>().gameObject);
                        }
                    }
                }
                // Si l'inventaire est le meme
                else
                {
                    if (hasAlreadyAnItem)
                    {
              
                        ItemContainer itemDragged = eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer;
                        ItemContainer itemOn = currentItem.GetComponent<ItemInstance>().ItemContainer;

                        if (itemOn.Item.Id == itemDragged.Item.Id)
                        {

                            int quantityLeft = InventoryManager.MergeStackables(currentItem.GetComponent<ItemInstance>().ItemContainer, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer);
                            if (quantityLeft <= 0)
                            {
                                InventoryManager.RemoveItem(inventoryKeeperDequi, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer);
                            }
                        }
                        else
                        {
                            // swap dequi = versqui
                            InventoryManager.SwapItemInSameInventory(inventoryKeeperDequi, previous.GetSiblingIndex(), transform.GetSiblingIndex());
                        }
                    }
                    else
                    {
                        InventoryManager.SwapItemInSameInventory(inventoryKeeperDequi, previous.GetSiblingIndex(), transform.GetSiblingIndex());

                    }
                }

                if (!GameManager.Instance.Ui.itemSplitter.activeSelf)
                {
                    Destroy(eventData.pointerDrag.gameObject);

                    inventaireDequi.Owner.GetComponent<Behaviour.Inventory>().UpdateInventories();
                    inventaireversqui.Owner.GetComponent<Behaviour.Inventory>().UpdateInventories();
                }
            }
            // Drag Characters in battle scene
            else
            {
                // Si ce n'est pas un objet qui est drag ( selection de perso pour le combat)
                if (eventData.pointerDrag.GetComponent<ItemInstance>() == null && transform.parent.GetComponent<InventoryOwner>() == null)
                {
                    // Swap image perso
                    if (hasAlreadyAnItem)
                    {
                        currentItem.transform.SetParent(previous);
                    }
                    eventData.pointerDrag.transform.SetParent(transform);
                }
            }
        }
        else if (eventData.pointerDrag.GetComponent<DragHandlerInventoryPanel>() != null)
        {
            Debug.Log("A panel was drop in a slot");
        }
    }

}

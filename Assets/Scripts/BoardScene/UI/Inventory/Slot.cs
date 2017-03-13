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
        if (eventData.pointerDrag.GetComponent<DragHandler>() != null)
        {
            // Get the original parent
            Transform previous = eventData.pointerDrag.GetComponent<DragHandler>().startParent;

            if (previous.parent.GetComponent<InventoryOwner>() != null && transform.parent.GetComponent<InventoryOwner>() != null)
            {
                //Ou on est ?
                InventoryOwner inventaireDequi = previous.parent.GetComponent<InventoryOwner>();
                InventoryOwner inventaireversqui = transform.parent.GetComponent<InventoryOwner>();

                ItemContainer[] inventoryKeeperDequi = inventaireDequi.Owner.GetComponent<Inventory>().inventory;
                ItemContainer[] inventoryKeeperVersqui = inventaireversqui.Owner.GetComponent<Inventory>().inventory;

                //Si les inventaires sont differents
                if (inventaireDequi != inventaireversqui)
                {
                    if (hasAlreadyAnItem)
                    {
                        ItemContainer itemDragged = eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer;
                        ItemContainer itemOn = currentItem.GetComponent<ItemInstance>().ItemContainer;

                        if ((itemOn.Item.GetType() == itemDragged.GetType()) && itemOn.Item.GetType() == typeof(Ressource) && itemOn.Item.Id == itemDragged.Item.Id)
                        {
                            int quantityLeft = ItemManager.MergeStackables2((currentItem.GetComponent<ItemInstance>().ItemContainer), (eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer));
                            if (quantityLeft > 0)
                            {
                                Debug.Log("test");
                                eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer.Quantity = quantityLeft;
                            }
                            else
                            {
                                Debug.Log("test");
                                ItemManager.RemoveItem(inventoryKeeperDequi, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer);
                            }
                        }
                        else
                        {
                            // Swap
                            ItemManager.SwapItemBeetweenInventories(inventoryKeeperDequi, previous.GetSiblingIndex(), inventoryKeeperVersqui, transform.GetSiblingIndex());

                        }
                    }
                    else
                    {
                        //Move the item to the slot
                        //eventData.pointerDrag.transform.SetParent(transform);

                        ItemManager.RemoveItem(inventoryKeeperDequi, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer);
                        ItemManager.AddItem(inventoryKeeperVersqui, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer, false);

                        ItemManager.MoveItemToSlot(
                             inventoryKeeperVersqui,
                             eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer,
                             transform.GetSiblingIndex()
                         );
                    }
                }
                // Si l'inventaire est le même
                else
                {
                    if (hasAlreadyAnItem)
                    {
              
                        ItemContainer itemDragged = eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer;
                        ItemContainer itemOn = currentItem.GetComponent<ItemInstance>().ItemContainer;

                        if ((itemOn.Item.GetType() == itemDragged.Item.GetType()) && itemOn.Item.GetType() == typeof(Ressource) && itemOn.Item.Id == itemDragged.Item.Id)
                        {
                            int quantityLeft = ItemManager.MergeStackables2(currentItem.GetComponent<ItemInstance>().ItemContainer, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer);
                            if (quantityLeft > 0)
                            {
                                eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer.Quantity = quantityLeft;
                            }
                            else
                            {
                                ItemManager.RemoveItem(inventoryKeeperDequi, eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer);

                            }
                        }
                        else
                        {
                            // swap dequi = versqui
                            ItemManager.SwapItemInSameInventory(inventoryKeeperDequi, previous.GetSiblingIndex(), transform.GetSiblingIndex());
                        }
                    }
                    else
                    {
                        //Move the item to the slot
                        ItemManager.MoveItemToSlot(
                              inventoryKeeperVersqui,
                              eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer,
                              transform.GetSiblingIndex()
                        );

                    }
                }

                Destroy(eventData.pointerDrag.gameObject);
                GameManager.Instance.Ui.UpdateKeeperInventoryPanel();
                GameManager.Instance.SelectedKeeperNeedUpdate = true;
                IngameScreens.Instance.UpdateLootInterface();
            }
            // Drag Characters in battle scene
            else
            {
                // Si ce n'est pas un objet qui est drag
                if (eventData.pointerDrag.GetComponent<ItemInstance>().ItemContainer == null && transform.parent.GetComponent<InventoryOwner>() == null)
                {
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

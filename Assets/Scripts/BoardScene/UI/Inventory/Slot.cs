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

            if (previous.parent.GetComponent<InventoryOwner>() != null && transform.parent.GetComponent<InventoryOwner>() != null)
            {
                //Ou on est ?
                InventoryOwner inventaireDequi = previous.parent.GetComponent<InventoryOwner>();
                InventoryOwner inventaireversqui = transform.parent.GetComponent<InventoryOwner>();

                ItemContainer[] inventoryKeeperDequi = inventaireDequi.Owner.GetComponent<Inventory>().Items;
                ItemContainer[] inventoryKeeperVersqui = inventaireversqui.Owner.GetComponent<Inventory>().Items;

                //Si les inventaires sont differents
                if (inventaireDequi != inventaireversqui)
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
                            // Swap
                            InventoryManager.SwapItemBeetweenInventories(inventoryKeeperDequi, previous.GetSiblingIndex(), inventoryKeeperVersqui, transform.GetSiblingIndex());

                        }
                    }
                    else
                    {
                        //Move the item to the slot
                        //eventData.pointerDrag.transform.SetParent(transform);
                        InventoryManager.SwapItemBeetweenInventories(inventoryKeeperDequi, previous.GetSiblingIndex(), inventoryKeeperVersqui, transform.GetSiblingIndex());
                    }

                    // Destroy inventory if it is empty for loot
                    if (inventaireDequi.Owner.GetComponent<LootInstance>() != null)
                    {
                        bool isEmpty = true;
                        for (int i = 0; i < inventaireDequi.Owner.GetComponent<Inventory>().Items.Length; i++)
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
                // Si l'inventaire est le m�me
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

                Destroy(eventData.pointerDrag.gameObject);

                GameManager.Instance.Ui.UpdateInventoryPanel(inventaireDequi.Owner);
                GameManager.Instance.Ui.UpdateInventoryPanel(inventaireversqui.Owner);
                GameManager.Instance.Ui.UpdatePrisonerFeedingPanel(inventaireDequi.Owner);
                GameManager.Instance.Ui.UpdatePrisonerFeedingPanel(inventaireversqui.Owner);
                GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
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

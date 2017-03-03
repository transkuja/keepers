using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    public GameObject item
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
        /*
        if (eventData.pointerDrag.GetComponent<DragHandler>() != null)
        {
            // Get the original parent
            Transform aux = eventData.pointerDrag.GetComponent<DragHandler>().startParent;

            //Ou on est ?
            GameObject inventaireDequi = aux.parent.parent.gameObject;
            GameObject inventaireversqui = transform.parent.parent.gameObject;

            GameObject dequi = null;
            GameObject versqui = null;
            foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
            {
                if (ki.GetComponent<InventoryManager>().KeeperInventoryPanel == inventaireDequi)
                {
                    dequi = ki.gameObject;
                }
                if (ki.GetComponent<InventoryManager>().KeeperInventoryPanel == inventaireversqui)
                {
                    versqui = ki.gameObject;
                }
            }

            if (dequi == null || versqui == null)
                Debug.Log("gros bug");

            //Si les inventaires sont differents
            if (dequi != versqui)
            {
                if (hasAlreadyAnItem)
                {
                    Item itemDragged = eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item;
                    Item itemOn = item.GetComponent<ItemInstance>().itemContainer.item;

                    if (item.GetComponent<ItemInstance>().itemContainer.item.Stackable && eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item.Stackable && (itemOn.GetType() == itemDragged.GetType()))
                    {
                        int quantityLeft = versqui.GetComponent<InventoryManager>().MergeStackables2(item.GetComponent<ItemInstance>().itemContainer, eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer);
                        if (quantityLeft > 0)
                        {
                            eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.quantity = quantityLeft;
                        }
                        else
                        {
                            dequi.GetComponent<InventoryManager>().RemoveItem(eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item);

                        }
                        Destroy(eventData.pointerDrag.gameObject);

                    }
                    else
                    {
              
                        // Swap
                        //Move the item to the slot
                       // eventData.pointerDrag.transform.SetParent(transform);

                        // Doit swap
                        Debug.Log("Ne swap pas a cause du remove add");

                        dequi.GetComponent<InventoryManager>().RemoveItem(eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item);
                        versqui.GetComponent<InventoryManager>().RemoveItem(item.GetComponent<ItemInstance>().itemContainer.item);

                      



                        //Move the other item to the previous slot
                        //item.transform.SetParent(aux);

              
                        dequi.GetComponent<InventoryManager>().AddItem(item.GetComponent<ItemInstance>().itemContainer, false);
                        versqui.GetComponent<InventoryManager>().AddItem(eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer, false);

                        dequi.GetComponent<InventoryManager>().MoveItemToSlot(
                                eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item,
                                transform.GetSiblingIndex()
                           );


                        versqui.GetComponent<InventoryManager>().MoveItemToSlot(
                            item.GetComponent<ItemInstance>().itemContainer.item,
                            aux.GetSiblingIndex()
                     
                        );

                        Destroy(eventData.pointerDrag.gameObject);
                    }
                }
                else
                {
                    //Move the item to the slot
                    eventData.pointerDrag.transform.SetParent(transform);

                    dequi.GetComponent<InventoryManager>().RemoveItem(eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item);
                    versqui.GetComponent<InventoryManager>().AddItem(eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer, false);

                    versqui.GetComponent<InventoryManager>().MoveItemToSlot(
                         item.GetComponent<ItemInstance>().itemContainer.item,
                         transform.GetSiblingIndex()
                     );
                }
            }
            // Si l'inventaire est le même
            else
            {
                if (hasAlreadyAnItem)
                {
                    Item itemDragged = eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item;
                    Item itemOn = item.GetComponent<ItemInstance>().itemContainer.item;


                    if (item.GetComponent<ItemInstance>().itemContainer.item.Stackable && eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item.Stackable && (itemOn.GetType() == itemDragged.GetType()))
                    {
                        int quantityLeft = versqui.GetComponent<InventoryManager>().MergeStackables2(item.GetComponent<ItemInstance>().itemContainer, eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer);
                        if (quantityLeft > 0)
                        {
                            eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.quantity = quantityLeft;
                        }
                        else
                        {
                            dequi.GetComponent<InventoryManager>().RemoveItem(eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item);
              
                        }
                        Destroy(eventData.pointerDrag.gameObject);
                    }
                    else
                    {
                        // swap dequi = versqui
                        eventData.pointerDrag.transform.SetParent(transform);

                        dequi.GetComponent<InventoryManager>().MoveItemToSlot(
                             eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item,
                             aux.GetSiblingIndex()
                        );


                        //Move the other item to the previous slot
                        item.transform.SetParent(aux);
                        versqui.GetComponent<InventoryManager>().MoveItemToSlot(
                            item.GetComponent<ItemInstance>().itemContainer.item,
                            transform.GetSiblingIndex()
                        );
                    }
                }
                else
                {
                    //Move the item to the slot
                    // swap dequi = versqui
                    eventData.pointerDrag.transform.SetParent(transform);

                    versqui.GetComponent<InventoryManager>().MoveItemToSlot(
                          eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item,
                          transform.GetSiblingIndex()
                    );
                }
            }




            //GameManager.Instance.ui.UpdateInventoryPanel(dequi.GetComponent<InventoryManager>().gameObject);
            //GameManager.Instance.ui.UpdateInventoryPanel(versqui.GetComponent<InventoryManager>().gameObject);

        }
        else if (eventData.pointerDrag.GetComponent<DragHandlerInventoryPanel>() != null)
        {
            Debug.Log("A panel was drop in a slot");
        }
            */
    }

}

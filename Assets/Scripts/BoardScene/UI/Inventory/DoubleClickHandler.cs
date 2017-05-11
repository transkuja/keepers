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
        if (GameManager.Instance.CurrentState == GameState.InTuto)
        {
            TutoManager.MouseClicked = false;
        }
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
                Behaviour.Inventory ownerInventory = goOwner.GetComponent<Behaviour.Inventory>();


                ItemContainer itemDragged = eventData.pointerPress.GetComponent<ItemInstance>().ItemContainer;



               

                if (InventoryManager.AddItemToInventory(selectedKeeperInventory, itemDragged))
                    InventoryManager.RemoveItem(ownerInventory.Items, itemDragged);

                // IF trying to merge 99 with 10 should swap or merge @rémi
                // else
                // foreach item
                //int quantityLeft = InventoryManager.MergeStackables(itemDragged, item);
                //if (quantityLeft <= 0)
                //{
                //    InventoryManager.RemoveItem(ownerInventory.Items, itemDragged);
                //}


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
                        Transform previous = eventData.pointerPress.GetComponentInParent<DragHandler>().transform;
                        InventoryOwner inventaireDequi = previous.GetComponentInParent<InventoryOwner>();
                        Destroy(inventaireDequi.GetComponentInParent<DragHandlerInventoryPanel>().gameObject);
                    }
                }
                    
                goOwner.GetComponent<Behaviour.Inventory>().UpdateInventories();
                GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Inventory>().UpdateInventories();
                //}
                return;
            }


            ii.ItemContainer.UseItem(owner.GetComponent<PawnInstance>());

            if (ii.ItemContainer.Quantity <= 0)
            {
                if (GameManager.Instance.Ui.tooltipItem.activeSelf)
                {
                    GameManager.Instance.Ui.tooltipItem.SetActive(false);
                }
                InventoryManager.RemoveItem(owner.GetComponent<Behaviour.Inventory>().Items, ii.ItemContainer);
                if (GameManager.Instance.CurrentState == GameState.InTuto)
                {
                    TutoManager.MouseClicked = true;
                }
            }

            owner.GetComponent<Behaviour.Inventory>().UpdateInventories();
        }
    }
}

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

        // Only keeper can use items
        if (eventData.pointerPress.transform.parent.parent.GetComponent<InventoryOwner>().Owner.GetComponent<KeeperInstance>() == null || eventData.pointerPress.transform.parent.parent.GetComponent<InventoryOwner>().Owner.GetComponent<KeeperInstance>() != GameManager.Instance.ListOfSelectedKeepers[0]) return;

        if (tap == 2)
        {
            ii.ItemContainer.UseItem();
            if (ii.ItemContainer.Quantity <= 0)
            {
                InventoryManager.RemoveItem(eventData.pointerPress.transform.parent.parent.GetComponent<InventoryOwner>().Owner.GetComponent<Inventory>().List_inventaire, ii.ItemContainer);
            }
            GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        }
    }
}

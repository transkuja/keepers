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
            ii.ItemContainer.UseItem();
            if (ii.ItemContainer.Quantity <= 0)
            {
                InventoryManager.RemoveItem(eventData.pointerPress.transform.parent.parent.GetComponent<InventoryOwner>().Owner.GetComponent<Inventory>().inventory, ii.ItemContainer);
            }
            GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        }
    }
}

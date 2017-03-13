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
            if (ii.ItemContainer.Item.GetType() == typeof(Ressource))
            {
                Ressource ressource = ((Ressource)(ii.ItemContainer.Item));
                ressource.UseItem();
                if (ii.ItemContainer.Quantity <= 0)
                {
                    ItemManager.RemoveItem(eventData.pointerPress.transform.parent.parent.GetComponent<InventoryOwner>().Owner.GetComponent<Inventory>().inventory, ii.ItemContainer);
                }
                GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
            }
        }
    }
}

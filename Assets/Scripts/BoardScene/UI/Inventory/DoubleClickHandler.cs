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
            if (ii.item.GetType() == typeof(Consummable))
            {
                Consummable consommable = ((Consummable)(ii.item));
                consommable.Use(0);
                if (consommable.quantite <= 0)
                {
                    ItemManager.RemoveItem(eventData.pointerPress.transform.parent.parent.GetComponent<InventoryOwner>().Owner.GetComponent<Inventory>().inventory, consommable);
                }
                GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
            }
        }
    }
}

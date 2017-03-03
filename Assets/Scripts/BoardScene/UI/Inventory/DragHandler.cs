using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler{
    Vector3 startPosition;
    Transform startParent;
    Transform absoluteParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if (GameManager.Instance.IsShiftPressed)
        //{
        //    if(eventData.pointerDrag.GetType() == typeof(ItemConsumable))
        //    {
        //        Debug.Log("shiftpressed");
        //        GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<InventoryManager>().AddItem(
        //        eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer, eventData.pointerDrag.GetComponent<ItemInstance>().itemContainer.item.Stackable);
        //    }
        //} else
        //{
            startPosition = transform.position;
            startParent = transform.parent;

            // Force sorting layer a ontopofEverything
            absoluteParent = transform.parent.parent.parent.parent;
            eventData.pointerDrag.gameObject.transform.SetParent(absoluteParent);


        //}


        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == absoluteParent)
        {
            transform.position = startPosition;

            // force the item being drag to reset on his starting position
            transform.SetParent(startParent);
        }

            
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //UIManager.Instance.AfficherTooltip(GetComponentInChildren<ItemInstance>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //UIManager.Instance.MasquerTooltip();
    }
}

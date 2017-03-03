using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandlerInventoryPanel : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    Vector3 startPosition;

    Vector3 offset;
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = Input.mousePosition - transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition - offset ;

    }

}

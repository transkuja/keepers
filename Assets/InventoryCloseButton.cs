using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCloseButton : MonoBehaviour {

    [SerializeField]
    RectTransform panel;

    void Start()
    {
        transform.position = panel.position + Vector3.up * (panel.rect.height / 5) + Vector3.right * (panel.rect.width / 5);
    }


    void Update()
    {
        transform.position = panel.position + Vector3.up * (panel.rect.height / 5) + Vector3.right * (panel.rect.width / 5);
    }
}

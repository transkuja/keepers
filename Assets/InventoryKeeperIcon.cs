using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryKeeperIcon : MonoBehaviour {
    [SerializeField]
    RectTransform panel;

	void Start () {
        transform.position = panel.position + Vector3.up * (panel.rect.height / 5);
	}

    void Update()
    {
        transform.position = panel.position + Vector3.up * (panel.rect.height / 5);
    }
}

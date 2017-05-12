using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class ItemSplitter : MonoBehaviour {

    string regexPattern = "[^0-9]";
    public InputField inputFieldText;
    // inventaire from
    // inventaire to
    public InventoryOwner inventoryFrom;
    public InventoryOwner inventoryTo;
    int quantityToSend;
    public Transform originSlot;
    public Transform targetSlot;
    public ItemContainer selectedItem;
    public GameObject uiItem;

    void OnEnable()
    {
        // on drop
        // or double click
        inputFieldText.text = selectedItem.Quantity.ToString();
        uiItem.transform.position = Input.mousePosition;
        uiItem.GetComponent<DragHandler>().enabled = false;
    }

    void OnDisable()
    {
        UpdateInventories();
        inventoryFrom = null;
        inventoryTo = null;
        originSlot = null;
        targetSlot = null;
        quantityToSend = 0;
        inputFieldText.text = "0";
    }

    public void LeftArrow()
    {
        if (inputFieldText.text != "0")
            inputFieldText.text = (Int32.Parse(inputFieldText.text) - 1).ToString();
    }

    public void RightArrow()
    {
        if (inputFieldText.text != selectedItem.Quantity.ToString())
            inputFieldText.text = (Int32.Parse(inputFieldText.text) + 1).ToString();
    }

    public void InputFieldCapper()
    {
        if (inputFieldText.text.Length > 2)
            inputFieldText.text = inputFieldText.text.Substring(0, 2);

        Regex rgx = new Regex(regexPattern);
        inputFieldText.text = rgx.Replace(inputFieldText.text, "");
        if (Int32.Parse(inputFieldText.text) > selectedItem.Quantity)
            inputFieldText.text = selectedItem.Quantity.ToString();
    }

    public void Split()
    {
        quantityToSend = Int32.Parse(inputFieldText.text);

        int quantityLeft = 0;
        if (targetSlot.GetComponent<Slot>().hasAlreadyAnItem)
        {
            quantityLeft = InventoryManager.MergeStackables(targetSlot.GetComponent<Slot>().currentItem.GetComponent<ItemInstance>().ItemContainer, selectedItem, quantityToSend);
        }
        else
        {
            selectedItem.Quantity -= quantityToSend;
            quantityLeft = selectedItem.Quantity;
            if (quantityToSend > 0)
                InventoryManager.AddItemToInventory(inventoryTo.Owner.GetComponent<Behaviour.Inventory>().Items, new ItemContainer(selectedItem.Item, quantityToSend));
        }

        if (quantityLeft <= 0)
        {
            InventoryManager.RemoveItem(inventoryFrom.Owner.GetComponent<Behaviour.Inventory>().Items, selectedItem);
        }

        gameObject.SetActive(false);
    }

    public void Swap()
    {
        InventoryManager.SwapItemBeetweenInventories(inventoryFrom.Owner.GetComponent<Behaviour.Inventory>().Items, originSlot.GetSiblingIndex(), inventoryTo.Owner.GetComponent<Behaviour.Inventory>().Items, targetSlot.GetSiblingIndex());
        gameObject.SetActive(false);
    }

    void UpdateInventories()
    {
        Destroy(uiItem);
        inventoryFrom.Owner.GetComponent<Behaviour.Inventory>().UpdateInventories();
        inventoryTo.Owner.GetComponent<Behaviour.Inventory>().UpdateInventories();
    }
}

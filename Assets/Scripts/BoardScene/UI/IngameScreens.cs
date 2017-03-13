using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Used to identify game screens in scene.
/// </summary>
public class IngameScreens : MonoBehaviour {
    public GameObject goInventoryLoot;
    public GameObject Slot_Prefab;

    public GameObject itemUI;

    private static IngameScreens instance = null;

    public void Awake()
    {
        instance = this;
    }

    public static IngameScreens Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public void CreateLootInterface()
    {
        int nbSlots = 6;
        for (int i = 0; i < nbSlots; i++)
        {
            //Create Slots
            GameObject currentgoSlotPanel = Instantiate(Slot_Prefab, Vector3.zero, Quaternion.identity) as GameObject;
            currentgoSlotPanel.transform.SetParent(goInventoryLoot.transform);

            currentgoSlotPanel.transform.localPosition = Vector3.zero;
            currentgoSlotPanel.transform.localScale = Vector3.one;
            currentgoSlotPanel.name = "Slot" + i;
        }
    }

    public void UpdateLootInterface()
    {
        if (IngameScreens.Instance == null) { return; }
        if (goInventoryLoot == null) { return; }

        if (goInventoryLoot.transform.childCount > 0)
        {
            foreach (ItemInstance holder in goInventoryLoot.transform.GetChild(0).transform.GetComponentsInChildren<ItemInstance>())
            {
                DestroyImmediate(holder.gameObject);
            }
            if (goInventoryLoot.GetComponentInParent<Inventory>().inventory != null)
            {
                ItemContainer[] inventory = goInventoryLoot.GetComponentInParent<Inventory>().inventory;

                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] != null && inventory[i].Item != null)
                    {
                        GameObject currentSlot = goInventoryLoot.transform.GetChild(i).gameObject;
                        GameObject go = Instantiate(itemUI);
                        go.transform.SetParent(currentSlot.transform);


                        go.GetComponent<ItemInstance>().ItemContainer = inventory[i];
                        go.name = inventory[i].Item.ItemName;

                        go.GetComponent<Image>().sprite = inventory[i].Item.InventorySprite;
                        go.transform.localScale = Vector3.one;

                        go.transform.position = currentSlot.transform.position;
                        go.transform.SetAsFirstSibling();

                        if (go.GetComponent<ItemInstance>().ItemContainer.Item.GetType() == typeof(Ressource))
                        {
                            go.transform.GetComponentInChildren<Text>().text = inventory[i].Quantity.ToString();
                        }
                    }
                }

            }

        }
    }
}

public enum IngameScreensEnum
{
    BattleResultScreens,
    SelectBattleCharactersScreen,
    WinScreen,
    LoseScreen
}

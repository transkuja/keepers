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

            foreach (ItemInstance holder in goInventoryLoot.transform.GetChild(0).transform.GetComponentsInChildren<ItemInstance>())
            {
                DestroyImmediate(holder.gameObject);
            }
            if (goInventoryLoot.GetComponentInParent<Inventory>().inventory != null)
            {
                Item[] inventory = goInventoryLoot.GetComponentInParent<Inventory>().inventory;

                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] != null)
                    {
                        GameObject currentSlot = goInventoryLoot.transform.GetChild(i).gameObject;
                        GameObject go = Instantiate(itemUI);
                        go.transform.SetParent(currentSlot.transform);
                        go.GetComponent<ItemInstance>().item = inventory[i];
                        go.name = inventory[i].ToString();

                        go.GetComponent<Image>().sprite = inventory[i].sprite;
                        go.transform.localScale = Vector3.one;

                        go.transform.position = currentSlot.transform.position;
                        go.transform.SetAsFirstSibling();

                        if (go.GetComponent<ItemInstance>().item.GetType() == typeof(Consummable))
                        {
                            go.transform.GetComponentInChildren<Text>().text = ((Consummable)inventory[i]).quantite.ToString();
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

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField]
    string type;
    [SerializeField]
    string id;
    [SerializeField]
    string itemName;
    [SerializeField]
    string description;
    [SerializeField]
    GameObject ingameVisual;
    [SerializeField]
    Sprite inventorySprite;
    [SerializeField]
    bool isStackable;
    [SerializeField]
    int rarity;

    public string Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public string ItemName
    {
        get
        {
            return itemName;
        }

        set
        {
            itemName = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public Sprite InventorySprite
    {
        get
        {
            return inventorySprite;
        }

        set
        {
            inventorySprite = value;
        }
    }

    public bool IsStackable
    {
        get
        {
            return isStackable;
        }

        set
        {
            isStackable = value;
        }
    }

    public GameObject IngameVisual
    {
        get
        {
            return ingameVisual;
        }

        set
        {
            ingameVisual = value;
        }
    }

    public string Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public int Rarity
    {
        get
        {
            return rarity;
        }

        set
        {
            rarity = value;
        }
    }

    public Item()
    {

    }

    public Item(Item from)
    {
        type = from.Type;
        id = from.Id;
        itemName = from.ItemName;
        description = from.Description;
        ingameVisual = from.IngameVisual;
        inventorySprite = from.InventorySprite;
        isStackable = from.IsStackable;
        rarity = from.rarity;
    }
    public virtual void UseItem(ItemContainer ic, PawnInstance owner, bool isQuantityPreviouslyEqualOne = false)
    {
        Debug.Log("Use item of item type undefined");
    }
}

public enum EquipmentSlot { Weapon, Armor, Soul }
public enum Stat { Strength, HP, MP, Defense, Intelligence, Spirit }
public class Equipment : Item
{
    EquipmentSlot constraint;

    // TODO: Should be arrays
    Stat stat;
    short bonusToStat;

    public EquipmentSlot Constraint
    {
        get
        {
            return constraint;
        }

        set
        {
            constraint = value;
        }
    }

    public Stat Stat
    {
        get
        {
            return stat;
        }

        set
        {
            stat = value;
        }
    }

    public short BonusToStat
    {
        get
        {
            return bonusToStat;
        }

        set
        {
            bonusToStat = value;
        }
    }

    public Equipment()
    {
        IsStackable = false;
        // TODO: retrieve it in json file
        Rarity = 1;
    }

    public override void UseItem(ItemContainer ic, PawnInstance owner, bool isQuantityPreviouslyEqualOne = false)
    {
        //bool isEquiped = EquipementManager.CheckIfItemTypeIsInEquipement(GameManager.Instance.GetFirstSelectedKeeper().Equipment, ic);
        //if (isEquiped)
        //{
        //    int nbSlot = GameManager.Instance.GetFirstSelectedKeeper().Keeper.nbSlot;
        //    EquipementManager.UnequipItem(GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Inventory>().Items, nbSlot, GameManager.Instance.GetFirstSelectedKeeper().Equipment, ((Equipment)ic.Item).constraint);

        //    // Unapply Bonus Stats
        //    EquipementManager.UnapplyStats(GameManager.Instance.GetFirstSelectedKeeper(), ((Equipment)ic.Item));
        //} else
        //{
        //    EquipementManager.EquipItem(GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Inventory>().Items, GameManager.Instance.GetFirstSelectedKeeper().Equipment, ic);

        //    // Apply Bonus stats
        //    EquipementManager.ApplyStats(GameManager.Instance.GetFirstSelectedKeeper(), ((Equipment)ic.Item));
        //}

        //GameManager.Instance.Ui.UpdateShortcutPanel();
    }
}

public enum ResourceFunctions { UpMentalHealth, DecreaseHunger }
public class Ressource : Item
{
    int value;
    public delegate bool Use(int _value, PawnInstance owner);
    Use resourceUse = null;
    ResourceFunctions resourceUseIndex;

    public Use ResourceUse
    {
        get
        {
            return resourceUse;
        }

        private set { }
    }


    public ResourceFunctions ResourceUseIndex
    {
        get { return resourceUseIndex; }

        set
        {
            if (value.Equals(ResourceFunctions.UpMentalHealth))
            {
                resourceUse = UpMentalHealth;
            }
            if (value.Equals(ResourceFunctions.DecreaseHunger))
            {
                resourceUse = DecreaseHunger;
            }
                

            resourceUseIndex = value;

        }
    }

    public int Value
    {
        get
        {
            return value;
        }

        set
        {
            this.value = value;
        }
    }

    public Ressource()
    {
        IsStackable = true;
        // TODO: retrieve it in json file
        Rarity = 5;
    }

    private bool UpMentalHealth(int _value, PawnInstance owner)
    {
        GameManager.Instance.Ui.BuffActionTextAnimation(GameManager.Instance.Ui.goMentalHeathBuffOnStatPanel, _value);
        owner.GetComponent<Behaviour.MentalHealthHandler>().CurrentMentalHealth += (short)_value;
        return true;
    }

    private bool DecreaseHunger(int _value, PawnInstance owner)
    {
        GameManager.Instance.Ui.BuffActionTextAnimation(GameManager.Instance.Ui.goHungerBuffOnStatPanel, _value);
        owner.GetComponent<Behaviour.MentalHealthHandler>().CurrentMentalHealth += (short)_value;
        return true;
    }


    public override void UseItem(ItemContainer ic, PawnInstance owner, bool isQuantityPreviouslyEqualOne = false)
    {
        resourceUse.Invoke(Value, owner);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Prisoner : MonoBehaviour
    {

        PawnInstance instance;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            GameManager.Instance.RegisterPrisoner(instance);
        }

        public void ProcessFeeding()
        {
            Behaviour.Inventory inv = GetComponent<Behaviour.Inventory>();
            HungerHandler hungerHandler = GetComponent<Behaviour.HungerHandler>();
            int i = 0;
            while (hungerHandler.CurrentHunger < hungerHandler.Data.MaxHunger && i < inv.Items.Length)
            {

                if (inv.Items[i] == null || inv.Items[i].Quantity <= 0)
                {
                    if (inv.Items[i] != null)
                    {
                        InventoryManager.RemoveItem(inv.Items, inv.Items[i]);
                    }
                    i++;
                }
                else
                {
                    // Je suis un prisonnier
                    inv.Items[i].Item.UseItem(inv.Items[i], this.GetComponent<PawnInstance>());
                    inv.Items[i].Quantity--;
                }
            }

            bool isEmpty = true;
            for (i = 0; i < inv.Items.Length; i++)
            {
                if (inv.Items[i] != null)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (!isEmpty)
            {
                // Bug ? 
                ItemManager.AddItemOnTheGround(TileManager.Instance.PrisonerTile, transform, inv.Items);
                for (int j = 0; j < inv.Items.Length; j++)
                {
                    InventoryManager.RemoveItem(inv.Items, inv.Items[j]);
                }

            }

            inv.UpdateInventories();  
        }
    }
}
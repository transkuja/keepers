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
            Inventory inv = GetComponent<Inventory>();
            int i = 0;

            //if (inv.Items[i] == null || inv.Items[i].Quantity <= 0)
            //{
            //    if (inv.Items[i] != null)
            //    {
            //        InventoryManager.RemoveItem(inv.Items, inv.Items[i]);
            //    }
            //}
            //else
            {
                // Je suis un prisonnier
                inv.Items[i].Item.UseItem(inv.Items[i], GetComponent<PawnInstance>());
                inv.Items[i].Quantity--;
            }

            inv.UpdateInventories();  
        }
    }
}
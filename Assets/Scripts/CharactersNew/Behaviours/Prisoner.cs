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

            if (inv.Items[0] == null || inv.Items[0].Quantity <= 0)
            {
                if (inv.Items[0] != null)
                {
                    InventoryManager.RemoveItem(inv.Items, inv.Items[i]);
                }
            }
            else
            {
                inv.Items[i].Item.UseItem(inv.Items[i], GetComponent<PawnInstance>());
                inv.Items[i].Quantity--;
            }

            inv.UpdateInventories();  
        }
    }
}
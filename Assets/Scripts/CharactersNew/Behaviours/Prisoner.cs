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
            inv.Items[0].Item.UseItem(inv.Items[0], GetComponent<PawnInstance>());
            inv.Items[0].Quantity--;

            if (inv.Items[0] == null || inv.Items[0].Quantity <= 0)
            {
                if (inv.Items[0] != null)
                {
                    InventoryManager.RemoveItem(inv.Items, inv.Items[0]);
                }
            }

            inv.UpdateInventories();  
        }
    }
}
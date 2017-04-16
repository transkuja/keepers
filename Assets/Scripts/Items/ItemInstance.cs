using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInstance : MonoBehaviour, IHavestable
{
    [SerializeField]
    private ItemContainer itemContainer = null;

    public GlowObjectCmd GlowCmd;

    [SerializeField]
    int quantity = 1;

    [SerializeField]
    private bool isInScene = false;

    [SerializeField]
    private string idItem;

    void Start()
    {
        if (isInScene)
        {
            Init(idItem, quantity);
        }

    }


    public void Init(string _IdItem, int _iNb)
    {
        idItem = _IdItem;
        InteractionImplementer.Add(new Interaction(Harvest), 1, "Harvest", GameManager.Instance.SpriteUtils.spriteHarvest);
        itemContainer = new ItemContainer(GameManager.Instance.ItemDataBase.getItemById(_IdItem), quantity);

        if (itemContainer.Item.IngameVisual != null)
        {
            GameObject go = Instantiate(itemContainer.Item.IngameVisual, transform);
            if (go.transform.childCount > 0)
            {
                go.transform.localPosition = go.transform.GetChild(0).localPosition = Vector3.zero;
            } else
            {
                go.transform.localPosition = Vector3.zero;
            }
  
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }
        else
        {
            Debug.Log("Pas de Visuel Ingame pour l'item :\"" + itemContainer.Item.ItemName +"\"");
        }
    }

    public ItemContainer ItemContainer
    {
        get
        {
            return itemContainer;
        }

        set
        {
            itemContainer = value;
        }
    }

    [System.Obsolete("Use interactable component instead")]
    public InteractionImplementer InteractionImplementer
    {
        get
        {
            return GetComponent<Interactable>().Interactions;
        }

        set
        {
            GetComponent<Interactable>().Interactions = value;
        }
    }

    public void Harvest(int _i = 0)
    {
        int costAction = InteractionImplementer.Get("Harvest").costAction;
        if (GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>().ActionPoints >= costAction)
        {
            bool isNoLeftOver = InventoryManager.AddItemToInventory(GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Inventory>().Items, itemContainer);
            if (isNoLeftOver)
            {
                // TODO : bug here miscellenous interactions with feedback action UI
                Destroy(this);
                GlowController.UnregisterObject(GlowCmd);
                if (this.transform.childCount > 0)
                {
                    DestroyImmediate(this.transform.GetChild(1).gameObject);
                }

            }

            GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>().ActionPoints -= (short)costAction;
            GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Inventory>().UpdateInventories();
        }
        else
        {
            GameManager.Instance.Ui.ZeroActionTextAnimation(GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>());
        }
    }
    public void OnMouseOver()
    {
        GameManager.Instance.Ui.UiIconFeedBack.TriggerFeedback(itemContainer.Item.InventorySprite);
    }
    public void OnMouseExit()
    {
        GameManager.Instance.Ui.UiIconFeedBack.DisableFeedback();
    }
       

}
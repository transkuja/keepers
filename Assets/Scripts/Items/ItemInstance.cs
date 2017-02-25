using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour, IPickable {

    Item item;

    public delegate void Action();

    public List<Action> listActions = new List<Action>();
    public Action actionPick;

    [SerializeField]
    private string actionName = "";

    [SerializeField]
    private Sprite actionSprite;

    public List<ActionContainer> listActionContainers = new List<ActionContainer>();

    // Use this for initialization
    void Start () {

        listActions.Add(new Action(Pick));

        actionPick = new Action(Pick);

        listActionContainers.Add(new ActionContainer(new ActionQ(Pick), "Pick", null));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Pick()
    {
        Debug.Log("Picked");
        /*GameManager.Instance.listItemsInventory.Add(item);
        Destroy(gameObject);*/
    }
}

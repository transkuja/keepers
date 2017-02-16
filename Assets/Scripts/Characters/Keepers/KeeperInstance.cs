using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperInstance : MonoBehaviour {

    private Keeper associatedKeeper = null;
    private bool isSelected = false;

    [Header("Keeper Info")]
    [SerializeField]
    private new string name = "Perso1";

    [Header("Stats")]

    [Range(1, 100)]
    [SerializeField]
    private short hpValue = 100;

    [Range(1, 100)]
    [SerializeField]
    private short mpValue = 50;

    [Range(1, 10)]
    [SerializeField]
    private short strengthValue = 5;

    [Range(1, 10)]
    [SerializeField]
    private short defenseValue = 5;

    [Range(1, 10)]
    [SerializeField]
    private short intelligenceValue = 5;

    [Range(1, 10)]
    [SerializeField]
    private short spiritValue = 5;

    [Header("Status")]
    [Range(1, 100)]
    [SerializeField]
    private short hungerValue = 100;

    [Range(1, 100)]
    [SerializeField]
    private short mentalHealthValue = 100;

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }

        set
        {
            isSelected = value;
            goSelectionAura.SetActive(value);
        }
    }

    [SerializeField]
    private GameObject goSelectionAura;

    void Awake()
    {
        associatedKeeper = new Keeper();

        associatedKeeper.Name = name;

        associatedKeeper.Hp = hpValue;
        associatedKeeper.Mp = mpValue;
        associatedKeeper.Strength = strengthValue;
        associatedKeeper.Defense = defenseValue;
        associatedKeeper.Intelligence = intelligenceValue;
        associatedKeeper.Spirit = spiritValue;
        associatedKeeper.Hunger = hungerValue;
        associatedKeeper.MentalHealth = mentalHealthValue;

        KeeperManager.AddKeeper(associatedKeeper);
    }

    public Keeper getAssociatedPersonnage()
    {
        return associatedKeeper;
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        if (associatedKeeper != null)
            KeeperManager.RemoveKeeper(associatedKeeper);
    }

    public void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.tag == "TilePath")
        {
            // TODO Trigger UI Change Tile
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrisonerInstance : MonoBehaviour, IEscortable {

    [Header("Prisoner Info")]
    [SerializeField]
    private PrisonerOld prisoner = null;

    [SerializeField]
    private short currentMentalHealth;
    [SerializeField]
    private short currentHunger;
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int currentMp;

    [SerializeField]
    private int feedingSlotsCount = 2;

    public GameObject prisonerFeedingPanel;

    private bool isStarving = false;
    private bool isMentalHealthLow = false;
    private bool isAlive = true;

    private InteractionImplementer interactionImplementer;

    private GameObject keeperFollowed = null;

    // Aggro
    bool isTargetableByMonster = true;

    // Movement between tile
    bool isMovingBetweenTiles = false;
    float lerpMoveParam = 0.0f;
    Vector3 lerpStartPosition;
    Vector3 lerpEndPosition;
    Quaternion lerpStartRotation;
    Quaternion lerpEndRotation;

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            if (currentHp > prisoner.MaxHp)
            {
                currentHp = prisoner.MaxHp;
                isAlive = true;
            }
            else if (currentHp <= 0)
            {
                currentHp = 0;

                isAlive = false;
                Die();
            }
            else
            {
                isAlive = true;
            }

        }
    }

    public int CurrentMp
    {
        get { return currentMp; }
        set
        {
            currentMp = value;
            if (currentMp > prisoner.MaxMp)
            {
                currentMp = prisoner.MaxMp;
            }
            else if (currentMp < 0)
            {
                currentMp = 0;
            }

        }
    }

    public short CurrentHunger
    {
        get { return currentHunger; }
        set
        {
            currentHunger = value;
            if (currentHunger < 0)
            {
                currentHunger = 0;
                IsStarving = true;
            }
            else if (currentHunger > prisoner.MaxHunger)
            {
                currentHunger = prisoner.MaxHunger;
                IsStarving = false;
            }
            else
            {
                IsStarving = false;
            }

        }
    }

    public short CurrentMentalHealth
    {
        get { return currentMentalHealth; }
        set
        {
            currentMentalHealth = value;
            if (currentMentalHealth < 0)
            {
                currentMentalHealth = 0;
                IsMentalHealthLow = true;
            }
            else if (currentMentalHealth > prisoner.MaxMentalHealth)
            {
                currentMentalHealth = prisoner.MaxMentalHealth;
                IsMentalHealthLow = false;
            }
            else
            {
                IsMentalHealthLow = false;
            }
        }
    }


    public void Awake()
    {
        Init();
    }

    public void Die()
    {
        Debug.Log("Ashley is dead");
        GameManager.Instance.CheckGameState();
    }

    public PrisonerInstance(PrisonerInstance from)
    {
        prisoner = from.prisoner;
    }

    public void Init()
    {
        interactionImplementer = new InteractionImplementer();
        interactionImplementer.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.SpriteUtils.spriteEscort);
        interactionImplementer.Add(new Interaction(UnEscort), 0, "Unescort", GameManager.Instance.SpriteUtils.spriteUnescort, false);
        interactionImplementer.Add(new Interaction(InitFeeding), 1, "Feed", GameManager.Instance.SpriteUtils.spriteHarvest);
        GetComponent<Behaviour.Inventory>().Init(FeedingSlotsCount);
        currentHp = prisoner.MaxHp;
        currentHunger = prisoner.MaxHunger;
        currentMentalHealth = prisoner.MaxMentalHealth;
        currentMp = prisoner.MaxMp;
        isAlive = true;
    }

    public void StartBetweenTilesAnimation(Vector3 newPosition)
    {
        lerpMoveParam = 0.0f;
        lerpStartPosition = transform.position;
        lerpEndPosition = newPosition;
        Vector3 direction = newPosition - transform.position;
        lerpStartRotation = Quaternion.LookRotation(transform.forward);
        lerpEndRotation = Quaternion.LookRotation(direction);

        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("moveBetweenTiles");

        IsMovingBetweenTiles = true;        
    }

    private void Update()
    {
        if (IsMovingBetweenTiles)
        {
            lerpMoveParam += Time.deltaTime;
            if (lerpMoveParam >= 1.0f)
            {
                IsMovingBetweenTiles = false;
            }
            transform.position = Vector3.Lerp(lerpStartPosition, lerpEndPosition, Mathf.Clamp(lerpMoveParam, 0, 1));
            transform.rotation = Quaternion.Lerp(lerpStartRotation, lerpEndRotation, Mathf.Clamp(lerpMoveParam, 0, 1));
        }
    }

    #region Accessors
    public PrisonerOld Prisoner
    {
        get
        {
            return prisoner;
        }

        set
        {
            prisoner = value;
        }
    }

    public GameObject KeeperFollowed
    {
        get
        {
            return keeperFollowed;
        }

        set
        {
            keeperFollowed = value;
        }
    }
    public InteractionImplementer InteractionImplementer
    {
        get
        {
            return interactionImplementer;
        }

        set
        {
            interactionImplementer = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }

        set
        {
            isAlive = value;
        }
    }

    public bool IsTargetableByMonster
    {
        get
        {
            return isTargetableByMonster;
        }

        set
        {
            isTargetableByMonster = value;
        }
    }

    public bool IsMovingBetweenTiles
    {
        get
        {
            return isMovingBetweenTiles;
        }

        set
        {
            isMovingBetweenTiles = value;
            GetComponent<NavMeshAgent>().enabled = !value;
        }
    }

    public bool IsStarving
    {
        get
        {
            return isStarving;
        }

        set
        {
            isStarving = value;
        }
    }

    public bool IsMentalHealthLow
    {
        get
        {
            return isMentalHealthLow;
        }

        set
        {
            isMentalHealthLow = value;
        }
    }

    public int FeedingSlotsCount
    {
        get
        {
            return feedingSlotsCount;
        }
    }
    #endregion

    public void Escort(int _i = 0)
    {
        for(int i = 0; i < GameManager.Instance.AllKeepersListOld.Count; i++)
        {
            GameManager.Instance.AllKeepersListOld[i].Keeper.GoListCharacterFollowing.Clear();
            GameManager.Instance.AllKeepersListOld[i].isEscortAvailable = true;
        }
        // Ne va fonctionner que pour le prisonnier
        GameManager.Instance.ListOfSelectedKeepersOld[0].Keeper.GoListCharacterFollowing.Add(GameManager.Instance.GoTarget.GetComponentInParent<PrisonerInstance>().gameObject);
        GetComponent<NavMeshAgent>().stoppingDistance = 0.75f;
        GetComponent<NavMeshAgent>().avoidancePriority = 80;
        GameManager.Instance.ListOfSelectedKeepersOld[0].isEscortAvailable = false;

    }

    public void UnEscort(int _i = 0)
    {
        GameManager.Instance.ListOfSelectedKeepersOld[0].Keeper.GoListCharacterFollowing.Remove(this.gameObject);
        GameManager.Instance.ListOfSelectedKeepersOld[0].isEscortAvailable = true;
        GetComponent<NavMeshAgent>().avoidancePriority = 50;
    }

    public void InitFeeding(int _i = 0)
    {
        Behaviour.Inventory inv = gameObject.AddComponent<Behaviour.Inventory>();

        inv.Init(feedingSlotsCount);
        
        prisonerFeedingPanel.SetActive(true);
    }

    public void ProcessFeeding()
    {
        Behaviour.Inventory inv = GetComponent<Behaviour.Inventory>();
        int i = 0;
        while(currentHunger < prisoner.MaxHunger && i < inv.Items.Length)
        {
            
            if(inv.Items[i] == null || inv.Items[i].Quantity <= 0)
            {
                if (inv.Items[i] != null)
                {
                    InventoryManager.RemoveItem(inv.Items, inv.Items[i]);
                }
                //inv.Items[i] = null;

                i++;
            }
            else
            {
                inv.Items[i].Item.UseItem(inv.Items[i], this);
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
  
            ItemManager.AddItemOnTheGround(TileManager.Instance.PrisonerTile, transform, inv.Items);
            for (int j = 0; j < inv.Items.Length; j++){
                InventoryManager.RemoveItem(inv.Items, inv.Items[j]);
            }

        }

        prisonerFeedingPanel.SetActive(false);
        /*for(int j = 0; j < inv.Items.Length; j++)
        {
            inv.Items[j] = null;
        }*/

        GameManager.Instance.Ui.UpdatePrisonerFeedingPanel(gameObject);
    }
}

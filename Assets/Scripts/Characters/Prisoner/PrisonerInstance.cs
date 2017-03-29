using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrisonerInstance : MonoBehaviour, IEscortable {

    [Header("Prisoner Info")]
    [SerializeField]
    private Prisoner prisoner = null;

    [SerializeField]
    private short currentMentalHealth;
    [SerializeField]
    private short currentHunger;
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int currentMp;
    
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
    public Prisoner Prisoner
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
    #endregion

    public void Escort(int _i = 0)
    {
        for(int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            GameManager.Instance.AllKeepersList[i].Keeper.GoListCharacterFollowing.Clear();
            GameManager.Instance.AllKeepersList[i].isEscortAvailable = true;
        }
        // Ne va fonctionner que pour le prisonnier
        GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Add(GameManager.Instance.GoTarget.GetComponentInParent<PrisonerInstance>().gameObject);
        GetComponent<NavMeshAgent>().stoppingDistance = 0.75f;
        GetComponent<NavMeshAgent>().avoidancePriority = 80;
        GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable = false;

    }

    public void UnEscort(int _i = 0)
    {
        GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Remove(this.gameObject);
        GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable = true;
        GetComponent<NavMeshAgent>().avoidancePriority = 50;
    }
}

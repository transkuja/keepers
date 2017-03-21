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
    private short currentHunger = 0;
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int currentMp;
    
    private bool isStarving = false;
    private bool isMentalHealthLow = false;
    private bool isAlive = true;

    private InteractionImplementer interactionImplementer;

    private GameObject keeperFollowed = null;

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
            else if (currentHp < 0)
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
            if (currentHunger > prisoner.MaxHunger)
            {
                currentHunger = prisoner.MaxHunger;
                isStarving = true;
            }
            else if (currentHunger < 0)
            {
                currentHunger = 0;
                isStarving = false;
            }
            else
            {
                isStarving = false;
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
                isMentalHealthLow = true;
            }
            else if (currentMentalHealth > prisoner.MaxMentalHealth)
            {
                currentMentalHealth = prisoner.MaxMentalHealth;
                isMentalHealthLow = false;
            }
            else
            {
                isMentalHealthLow = false;
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
        interactionImplementer.Add(new Interaction(Escort), "Escort", GameManager.Instance.Ui.spriteEscort);
        interactionImplementer.Add(new Interaction(UnEscort), "Unescort", GameManager.Instance.Ui.spriteUnescort, false);
        currentHp = prisoner.MaxHp;
        currentHunger = 0;
        currentMentalHealth = prisoner.MaxMentalHealth;
        currentMp = prisoner.MaxMp;
        isAlive = true;
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
    #endregion

    public void Escort(int _i = 0)
    {
        for(int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            GameManager.Instance.AllKeepersList[i].Keeper.GoListCharacterFollowing.Clear();
            GameManager.Instance.AllKeepersList[i].isEscortAvailable = true;
        }

        GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Add(GameManager.Instance.GoTarget);
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

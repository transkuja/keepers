using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrisonerInstance : MonoBehaviour, IEscortable {

    [Header("Prisoner Info")]
    [SerializeField]
    private Prisoner prisoner = null;

    private InteractionImplementer interactionImplementer;

    private GameObject keeperFollowed = null;

    public void Awake()
    {
        Init();
    }

    public PrisonerInstance(PrisonerInstance from)
    {
        prisoner = from.prisoner;
    }

    public void Init()
    {
        interactionImplementer = new InteractionImplementer();
        interactionImplementer.Add(new Interaction(Escort), "Escort", null);
        interactionImplementer.Add(new Interaction(UnEscort), "Unescort", null, false);
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
    #endregion

    public void Escort()
    {
        for(int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            GameManager.Instance.AllKeepersList[i].Keeper.GoListCharacterFollowing.Clear();
            GameManager.Instance.AllKeepersList[i].isEscortAvailable = true;
        }

        GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Add(GameManager.Instance.GoTarget);
        this.GetComponent<NavMeshAgent>().stoppingDistance = 0.75f;
        GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable = false;
    }

    public void UnEscort()
    {
        GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Remove(this.gameObject);
        GameManager.Instance.ListOfSelectedKeepers[0].isEscortAvailable = true;
    }
}

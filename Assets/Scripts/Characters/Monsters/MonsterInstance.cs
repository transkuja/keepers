using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterInstance : MonoBehaviour {
    [Header("Monster Info")]
    [SerializeField]
    private Monster monster = null;
    public ParticleSystem deathParticles = null;
    [SerializeField]
    private int currentHp;

    [SerializeField]
    private int currentMp;

    private NavMeshAgent agent;
    private float moveTimer = 0.0f;
    private bool isAnimInitialized = false;

    // Battle variables
    bool hasRecentlyBattled = false;
    Vector3 originPosition;
    Quaternion originRotation;

    public Monster Monster
    {
        get
        {
            return monster;
        }

        set
        {
            monster = value;
        }
    }
    
    public void Awake()
    {
        currentHp = monster.MaxHp;
        currentMp = monster.MaxMp;
        agent = GetComponent<NavMeshAgent>();
        originPosition = transform.position;
        originRotation = transform.rotation;

    }

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            if (currentHp > monster.MaxHp)
            {
                currentHp = monster.MaxHp;
            }
            else if (currentHp < 0)
            {
                currentHp = 0;
            }
        }
    }

    public int CurrentMp
    {
        get { return currentMp; }
        set
        {
            currentMp = value;
            if (currentMp > monster.MaxMp)
            {
                currentMp = monster.MaxMp;
            }
            else if (currentMp < 0)
            {
                currentMp = 0;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    void Update () {
        NavMeshMovement();
    }

    public void RestAfterBattle()
    {
        foreach (BoxCollider bc in GetComponentsInChildren<BoxCollider>())
            bc.enabled = false;

        hasRecentlyBattled = true;
        Invoke("ReactivateTrigger", 3.0f);
    }

    private void ReactivateTrigger()
    {
        foreach (BoxCollider bc in GetComponentsInChildren<BoxCollider>())
            bc.enabled = true;
        hasRecentlyBattled = false;
    }

    public List<ItemContainer> ComputeLoot()
    {
        List<ItemContainer> tmpList = new List<ItemContainer>();
        Item it = null;
        foreach (string _IdItem in Monster.PossibleDrops)
        {
            it = GameManager.Instance.Database.getItemById(_IdItem);
            if (Random.Range(0, 10) > it.Rarity)
            {
                tmpList.Add(new ItemContainer(it, 1));
            }
        }

        return tmpList;
    }

    public void NavMeshMovement()
    {
        if (!hasRecentlyBattled)
        {
            moveTimer += Time.deltaTime;

            if (moveTimer >= 3.0f)
            {
                moveTimer = 0.0f;

                agent.enabled = false;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, isAnimInitialized ? transform.localEulerAngles.y + 180 : transform.localEulerAngles.y, transform.localEulerAngles.z);
                agent.enabled = true;
                agent.SetDestination(transform.position + transform.forward * 1.2f);

                isAnimInitialized = true;
            }
        }
        else
        {
            agent.SetDestination(originPosition);

            if (agent.remainingDistance < 0.0000001f)
            {
                agent.Stop();
                transform.rotation = originRotation;
                agent.Resume();
                isAnimInitialized = false;
                moveTimer = 0.0f;
            }
        }
    }
}

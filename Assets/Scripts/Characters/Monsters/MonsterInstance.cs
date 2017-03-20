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
    private float direction = 1.0f;


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

    private void OnTriggerExit(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        Invoke("ReactivateTrigger", 1.0f);
    }

    void Update () {
        NavMeshMovement();
    }

    private void ReactivateTrigger()
    {
        GetComponent<BoxCollider>().enabled = true;
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
        moveTimer += Time.deltaTime;

        if (moveTimer >= 2.0f)
        {
            moveTimer = 0.0f;
            // Fix this @ Anthony
            agent.Stop();
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 180, transform.localEulerAngles.z);
            agent.Resume();
            //
            agent.SetDestination(transform.position + transform.forward * direction);
            direction *= -1;
        }
    }
}

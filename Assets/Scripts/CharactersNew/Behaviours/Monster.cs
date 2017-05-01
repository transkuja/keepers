using UnityEngine;
using UnityEngine.AI;

namespace Behaviour
{
    public class Monster : MonoBehaviour
    {
        PawnInstance instance;
        [SerializeField]
        string monsterTypeID;

        private NavMeshAgent agent;
        private float moveTimer = 0.0f;
        private bool isAnimInitialized = false;

        // Battle variables
        Vector3 originPosition;
        Quaternion originRotation;
        [SerializeField]
        int baseAttack;
        [SerializeField]
        int baseDefense;
        [SerializeField]
        int bonusAttack;
        [SerializeField]
        int bonusDefense;
        // TODO: fix this behaviour
        bool hasRecentlyBattled = false;
        GameObject pointerFeedback;

        public PawnInstance getPawnInstance
        {
            get
            {
                return instance;
            }
        }

        public bool HasRecentlyBattled
        {
            get
            {
                return hasRecentlyBattled;
            }

            set
            {
                hasRecentlyBattled = value;
            }
        }

        public int BaseAttack
        {
            get
            {
                return baseAttack;
            }

            set
            {
                baseAttack = value;
            }
        }

        public int BaseDefense
        {
            get
            {
                return baseDefense;
            }

            set
            {
                baseDefense = value;
            }
        }

        public int BonusAttack
        {
            get
            {
                return bonusAttack;
            }

            set
            {
                bonusAttack = value;
            }
        }

        public int BonusDefense
        {
            get
            {
                return bonusDefense;
            }

            set
            {
                bonusDefense = value;
            }
        }

        public int EffectiveAttack
        {
            get
            {
                return baseAttack + bonusAttack;
            }
        }

        public int EffectiveDefense
        {
            get
            {
                return baseDefense + bonusDefense;
            }
        }

        public string MonsterTypeID
        {
            get
            {
                return monsterTypeID;
            }
        }

        public GameObject PointerFeedback
        {
            get
            {
                if (pointerFeedback == null)
                {
                    pointerFeedback = Instantiate(GameManager.Instance.PrefabUtils.selectionPointer, transform);
                    pointerFeedback.transform.localPosition = Vector3.zero + GameManager.Instance.PrefabUtils.selectionPointer.transform.localPosition;
                }
                return pointerFeedback;
            }
        }

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            originPosition = transform.position;
            originRotation = transform.rotation;

            GetComponent<Interactable>().Interactions.Add(new Interaction(Attack), 0, "Attack", GameManager.Instance.SpriteUtils.spriteAttack);

            GameManager.Instance.RegisterMonsterPosition(instance);
        }

        // TODO should be export in a patrol specific component
        void Update()
        {
            if (GameManager.Instance.CurrentState == GameState.Normal)
            {
                if (agent != null && agent.enabled)
                    NavMeshMovement();
            }
        }

        private void Attack(int _i = 0)
        {
            if (GameManager.Instance.CurrentState == GameState.Normal)
            {
                //NavMeshAgent agent = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<NavMeshAgent>();
                //if (agent != null && agent.isActiveAndEnabled)
                //    agent.SetDestination(transform.position);

                if (GetComponentInParent<Fighter>() != null && GetComponentInParent<Fighter>().IsTargetableByMonster == true)
                {
                    if (BattleHandler.IsABattleAlreadyInProcess())
                        return;

                    Tile tile = GetComponentInParent<PawnInstance>().CurrentTile;

                    BattleHandler.StartBattleProcess(tile);

                    GameManager.Instance.UpdateCameraPosition(GetComponentInParent<PawnInstance>());
                }
            }
        }

        public void NavMeshMovement()
        {
            if (!HasRecentlyBattled)
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
                    GetComponent<Fighter>().HasRecentlyBattled = false;
                }
            }
        }
    }
}
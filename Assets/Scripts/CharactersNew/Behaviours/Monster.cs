using UnityEngine;
using UnityEngine.AI;

namespace Behaviour
{
    public class Monster : MonoBehaviour
    {
        PawnInstance instance;

        private NavMeshAgent agent;
        private float moveTimer = 0.0f;
        private bool isAnimInitialized = false;

        // Battle variables
        Vector3 originPosition;
        Quaternion originRotation;
        int baseAttack;
        int baseDefense;
        int bonusAttack;
        int bonusDefense;
        // TODO: fix this behaviour
        bool hasRecentlyBattled = false;

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

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            originPosition = transform.position;
            originRotation = transform.rotation;

            GameManager.Instance.RegisterMonsterPosition(instance);
        }

        // TODO should be export in a patrol specific component
        void Update()
        {
            NavMeshMovement();
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
                }
            }
        }

    }
}
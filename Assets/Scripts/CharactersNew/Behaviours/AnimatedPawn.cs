using UnityEngine;
using UnityEngine.AI;

namespace Behaviour
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimatedPawn : MonoBehaviour
    {
        PawnInstance instance;
        NavMeshAgent agent;

        // Movement between tile
        // Prevents action poping when arriving on a tile
        Direction arrivingTrigger = Direction.None;
        bool isMovingBetweenTiles = false;
        bool isMovingToBattlePosition = false;
        float lerpMoveParam = 0.0f;
        Vector3 lerpStartPosition;
        Vector3 lerpEndPosition;
        Quaternion lerpStartRotation;
        Quaternion lerpEndRotation;

        Animator anim;
        Vector3 v3AgentDirectionTemp;

        // Rotations
        float fLerpRotation = 0.666f;
        Quaternion quatTargetRotation;
        Quaternion quatPreviousRotation;
        bool bIsRotating = false;
        [SerializeField]
        float fRotateSpeed = 1.0f;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            arrivingTrigger = Direction.None;
            fRotateSpeed = 5.0f;
        }

        void Update()
        {
            if (GetComponent<Keeper>() != null)
            {
                Keeper keeper = GetComponent<Keeper>();
                GameObject goDestinationTemp = gameObject;
                for (int i = 0; i < GetComponent<Keeper>().GoListCharacterFollowing.Count; i++)
                {
                    if (!keeper.GoListCharacterFollowing[i].GetComponent<AnimatedPawn>().IsMovingBetweenTiles)
                    {
                        keeper.GoListCharacterFollowing[i].GetComponentInParent<NavMeshAgent>().destination = goDestinationTemp.transform.position;
                        goDestinationTemp = keeper.GoListCharacterFollowing[i];
                    }
                }

                if (bIsRotating)
                {
                    Rotate();
                }
            }

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

            if (isMovingToBattlePosition)
            {
                lerpMoveParam += Time.deltaTime;
                if (lerpMoveParam >= 1.0f)
                {
                    IsMovingToBattlePosition = false;
                }
                transform.position = Vector3.Lerp(lerpStartPosition, lerpEndPosition, Mathf.Clamp(lerpMoveParam, 0, 1));
                transform.rotation = Quaternion.Lerp(lerpStartRotation, lerpEndRotation, Mathf.Clamp(lerpMoveParam, 0, 1));
            }

            if (anim.isActiveAndEnabled == true && agent.isActiveAndEnabled == true)
            {
                anim.SetFloat("velocity", agent.velocity.magnitude);
            }
        }


        public void StartBetweenTilesAnimation(Vector3 newPosition)
        {
            agent.enabled = false;

            lerpMoveParam = 0.0f;
            lerpStartPosition = transform.position;
            lerpEndPosition = newPosition;
            Vector3 direction = newPosition - transform.position;
            lerpStartRotation = Quaternion.LookRotation(transform.forward);
            lerpEndRotation = Quaternion.LookRotation(direction);

            anim.SetTrigger("moveBetweenTiles");

            IsMovingBetweenTiles = true;
        }

        // TODO: merge with the above function
        public void StartMoveToBattlePositionAnimation(Vector3 newPosition, Quaternion newRotation)
        {
            lerpMoveParam = 0.0f;
            lerpStartPosition = transform.position;
            lerpEndPosition = GetComponent<PawnInstance>().CurrentTile.transform.position + newPosition;
            lerpStartRotation = Quaternion.LookRotation(transform.forward);
            lerpEndRotation = newRotation;

            anim.SetTrigger("moveToBattlePosition");

            isMovingToBattlePosition = true;
        }
        
        public void TriggerRotation(Vector3 v3Direction)
        {
            if (agent.enabled == false)
            {
                Debug.Log("Agent is not active!");
                return;
            }
            agent.angularSpeed = 0.0f;

            quatPreviousRotation = transform.rotation;

            Vector3 v3PosTemp = transform.position;
            v3PosTemp.y = 0;
            v3Direction.y = 0.0f;

            quatTargetRotation.SetLookRotation(v3Direction - v3PosTemp);

            bIsRotating = true;

            agent.Stop();

            v3AgentDirectionTemp = v3Direction;

            fLerpRotation = 0.0f;
        }

        void Rotate()
        {
            if (fLerpRotation >= 1.0f)
            {
                transform.rotation = quatTargetRotation;
                bIsRotating = false;
                agent.Resume();
                fLerpRotation = 0.0f;

                agent.destination = v3AgentDirectionTemp;
                agent.angularSpeed = 100.0f;
            }
            else
            {
                fLerpRotation += fRotateSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(quatPreviousRotation, quatTargetRotation, fLerpRotation);
            }
        }
        

        #region Accessors
        public bool IsMovingBetweenTiles
        {
            get
            {
                return isMovingBetweenTiles;
            }

            set
            {
                isMovingBetweenTiles = value;
                agent.enabled = !isMovingBetweenTiles;
            }
        }

        public Direction ArrivingTrigger
        {
            get
            {
                return arrivingTrigger;
            }

            set
            {
                arrivingTrigger = value;
            }
        }

        public Animator Anim
        {
            get
            {
                return anim;
            }

            set
            {
                anim = value;
            }
        }

        public bool IsMovingToBattlePosition
        {
            get
            {
                return isMovingToBattlePosition;
            }

            set
            {
                isMovingToBattlePosition = value;
                anim.SetFloat("velocity", 0.0f);
            }
        }
        #endregion
    }
}
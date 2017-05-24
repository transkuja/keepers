using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
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
        bool cmdMove = false;
        bool cmdExplore = false;
        int whereMove = 0;
        [SerializeField]
        float fRotateSpeed = 1.0f;
        Vector3 beforeBattlePosition;
        Quaternion beforeBattleRotation;
        // Reset agent destination to avoid pawn strange behaviours
        float timerResetAgentDestination;
        float timerResetAgentDestinationDefault = 2.0f;
        bool doesAgentNeedReset = false;
        bool isCameraUpdated = false;
        bool wasAgentActiveBeforeBattle = true;

        float timerNeFaitRienPendantDixATrenteSecondes;

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
            timerResetAgentDestinationDefault = 2.0f;
            timerNeFaitRienPendantDixATrenteSecondes = Random.Range(10, 30);
        }
        void Update()
        {
            if (GameManager.Instance != null && (GameManager.Instance.CurrentState == GameState.Normal || GameManager.Instance.CurrentState == GameState.InTuto))
            {
                if (GetComponent<Keeper>() != null)
                {
                    Keeper keeper = GetComponent<Keeper>();
                    GameObject goDestinationTemp = gameObject;
                    for (int i = 0; i < GetComponent<Keeper>().GoListCharacterFollowing.Count; i++)
                    {
                        if (!keeper.GoListCharacterFollowing[i].GetComponent<AnimatedPawn>().IsMovingBetweenTiles)
                        {
                            if (keeper.GoListCharacterFollowing[i].GetComponentInParent<NavMeshAgent>() != null && keeper.GoListCharacterFollowing[i].GetComponentInParent<NavMeshAgent>().isActiveAndEnabled)
                                keeper.GoListCharacterFollowing[i].GetComponentInParent<NavMeshAgent>().destination = goDestinationTemp.transform.position;
                            goDestinationTemp = keeper.GoListCharacterFollowing[i];
                        }
                    }
                    if (bIsRotating)
                    {
                        Rotate();
                    }
                }
            }

            if (IsMovingBetweenTiles)
            {
                timerNeFaitRienPendantDixATrenteSecondes = Random.Range(10, 30);
                lerpMoveParam += Time.deltaTime;
                if (lerpMoveParam >= 1.0f)
                {
                    IsMovingBetweenTiles = false;
                }
                if (lerpMoveParam >= 0.25f && !isCameraUpdated)
                {
                    if (GameManager.Instance.CameraManagerReference.IsFollowingKeeper)
                        GameManager.Instance.UpdateCameraPosition(instance);
                    isCameraUpdated = true;
                }
                transform.position = Vector3.Lerp(lerpStartPosition, lerpEndPosition, Mathf.Clamp(lerpMoveParam, 0, 1));
                transform.rotation = Quaternion.Lerp(lerpStartRotation, lerpEndRotation, Mathf.Clamp(lerpMoveParam, 0, 1));
            }

            if (isMovingToBattlePosition)
            {
                timerNeFaitRienPendantDixATrenteSecondes = Random.Range(10, 30);
                lerpMoveParam += Time.deltaTime;
                if (lerpMoveParam >= 1.0f)
                {
                    IsMovingToBattlePosition = false;
                }
                transform.position = Vector3.Lerp(lerpStartPosition, lerpEndPosition, Mathf.Clamp(lerpMoveParam, 0, 1));
                transform.rotation = Quaternion.Lerp(lerpStartRotation, lerpEndRotation, Mathf.Clamp(lerpMoveParam, 0, 1));
            }

            if (anim != null && agent != null && anim.isActiveAndEnabled == true && agent.isActiveAndEnabled == true)
            {
                if( GetComponent<ItemInstance>() == null)
                {
                    anim.SetFloat("velocity", agent.velocity.magnitude);
                }

            }
    
            // ne fait rien pendant 10 secondes
            if(timerNeFaitRienPendantDixATrenteSecondes <= 0)
            {
                if (GetComponent<ItemInstance>() == null)
                {
                    anim.SetTrigger("idle");
                }
                timerNeFaitRienPendantDixATrenteSecondes = 10.0f;
     
            } else if (GameManager.Instance.CurrentState == GameState.Normal)
            {
                timerNeFaitRienPendantDixATrenteSecondes -= Time.deltaTime;
            }

            if (doesAgentNeedReset)
            {
                if (timerResetAgentDestination > 0.0f)
                    timerResetAgentDestination -= Time.deltaTime;
                else
                {
                    if (agent != null && agent.isActiveAndEnabled)
                        agent.ResetPath();
                    doesAgentNeedReset = false;
                }
            }
        }
        public void ResetPositionInBattle()
        {
            lerpMoveParam = 0.0f;
            isMovingToBattlePosition = true;
            lerpStartPosition = transform.position;
            lerpStartRotation = transform.rotation;
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
            beforeBattlePosition = lerpStartPosition;
            beforeBattleRotation = lerpStartRotation;
            anim.SetTrigger("moveToBattlePosition");
            isMovingToBattlePosition = true;
        }
        public void StartMoveFromBattlePositionAnimation()
        {
            lerpMoveParam = 0.0f;
            lerpStartPosition = transform.position;
            lerpEndPosition = beforeBattlePosition;
            lerpStartRotation = Quaternion.LookRotation(transform.forward);
            lerpEndRotation = beforeBattleRotation;
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

            // Reactivate aggro
            Tile currentKeeperTile = GameManager.Instance.GetFirstSelectedKeeper().CurrentTile;
            foreach (PawnInstance pi in GameManager.Instance.GetKeepersOnTile(currentKeeperTile))
                pi.GetComponent<Fighter>().IsTargetableByMonster = true;
            if (GameManager.Instance.PrisonerInstance.CurrentTile == currentKeeperTile)
            {
                if (GameManager.Instance.PrisonerInstance.GetComponent<Fighter>() != null)
                    GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().IsTargetableByMonster = true;
                else
                    Debug.LogWarning("Missing Fighter component on Prisoner.");
            }

            timerResetAgentDestination = timerResetAgentDestinationDefault;
            doesAgentNeedReset = true;
            agent.angularSpeed = 0.0f;
            quatPreviousRotation = transform.rotation;
            Vector3 v3PosTemp = transform.position;
            v3PosTemp.y = 0;
            v3Direction.y = 0.0f;
            quatTargetRotation.SetLookRotation(v3Direction - v3PosTemp);
            if (Quaternion.Angle(quatPreviousRotation, quatTargetRotation) > 10.0f)
            {
                bIsRotating = true;
                agent.Stop();
                fLerpRotation = 0.0f;
            }
            else
            {
                bIsRotating = true;
                agent.Resume();
            }
            v3AgentDirectionTemp = v3Direction;
            timerNeFaitRienPendantDixATrenteSecondes = Random.Range(10, 30);
        }
        void Rotate()
        {
            if (agent.enabled == false)
            {
                Debug.Log("Agent is not active!");
                bIsRotating = false;
                return;
            }
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
                isCameraUpdated = !value;
                // Update Ashley feeding panel
                if (GameManager.Instance.PrisonerInstance != null && GetComponent<Keeper>() != null)
                {
                    GameObject ashleyInventoryPanel = GameManager.Instance.PrisonerInstance.GetComponent<Inventory>().SelectedInventoryPanel;
                    if (!isMovingBetweenTiles && GameManager.Instance.PrisonerInstance.CurrentTile == GetComponent<PawnInstance>().CurrentTile)
                    {
                        ashleyInventoryPanel.transform.SetParent(GetComponent<Inventory>().SelectedInventoryPanel.transform, false);
                        GetComponent<Inventory>().SelectedInventoryPanel.GetComponent<GridLayoutGroup>().constraintCount = GetComponent<Inventory>().Data.NbSlot + 1;
                        ashleyInventoryPanel.SetActive(true);
                        // Show the feed slot after tuto has already warn the player
                        if (TutoManager.s_instance.waitForFeedSlotAppearance && TutoManager.s_instance.PlayingSequence == null)
                            TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqAshleyLowHunger>());
                    }
                    else
                    {
                        ashleyInventoryPanel.SetActive(false);
                        ashleyInventoryPanel.transform.SetParent(GetComponent<Inventory>().SelectedInventoryPanel.transform.parent, false);
                        GetComponent<Inventory>().SelectedInventoryPanel.GetComponent<GridLayoutGroup>().constraintCount = GetComponent<Inventory>().Data.NbSlot;
                    }
                }

                if (agent != null && agent.isActiveAndEnabled)
                {
                    agent.SetDestination(transform.position);
                    timerResetAgentDestination = timerResetAgentDestinationDefault/10.0f;
                    doesAgentNeedReset = true;
                }
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
                if (isMovingToBattlePosition == false && GameManager.Instance.CurrentState != GameState.InBattle)
                {
                    if (agent != null)
                        agent.enabled = wasAgentActiveBeforeBattle;
                }
            }
        }
        public bool CmdMove
        {
            get
            {
                return cmdMove;
            }
            set
            {
                cmdMove = value;
            }
        }
        public bool CmdExplore
        {
            get
            {
                return cmdExplore;
            }
            set
            {
                cmdExplore = value;
            }
        }
        public int WhereMove
        {
            get
            {
                return whereMove;
            }
            set
            {
                whereMove = value;
            }
        }
        public bool WasAgentActiveBeforeBattle
        {
            get
            {
                return wasAgentActiveBeforeBattle;
            }
            set
            {
                wasAgentActiveBeforeBattle = value;
            }
        }
        public NavMeshAgent Agent
        {
            get
            {
                return agent;
            }
            set
            {
                agent = value;
            }
        }
        #endregion
    }
}
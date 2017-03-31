﻿using UnityEngine;
using UnityEngine.AI;

namespace Behaviour
{
    public class AnimatedPawn : MonoBehaviour
    {
        PawnInstance instance;
        NavMeshAgent agent;

        // Movement between tile
        // Prevents action poping when arriving on a tile
        Direction arrivingTrigger = Direction.None;
        bool isMovingBetweenTiles = false;
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

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            agent = GetComponent<NavMeshAgent>();
            arrivingTrigger = Direction.None;
            anim = GetComponentInChildren<Animator>();
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
            }

            if (bIsRotating)
            {
                Rotate();
            }

            anim.SetFloat("velocity", agent.velocity.magnitude);
        }


        public void StartBetweenTilesAnimation(Vector3 newPosition)
        {
            lerpMoveParam = 0.0f;
            lerpStartPosition = transform.position;
            lerpEndPosition = newPosition;
            Vector3 direction = newPosition - transform.position;
            lerpStartRotation = Quaternion.LookRotation(transform.forward);
            lerpEndRotation = Quaternion.LookRotation(direction);

            anim.SetTrigger("moveBetweenTiles");

            IsMovingBetweenTiles = true;
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
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class AnimatedPawn : MonoBehaviour
    {
        PawnInstance instance;


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
        }

        void Update()
        {

        }

        
        public void StartBetweenTilesAnimation(Vector3 newPosition)
        {
            //lerpMoveParam = 0.0f;
            //lerpStartPosition = transform.position;
            //lerpEndPosition = newPosition;
            //Vector3 direction = newPosition - transform.position;
            //lerpStartRotation = Quaternion.LookRotation(transform.forward);
            //lerpEndRotation = Quaternion.LookRotation(direction);

            //anim.SetTrigger("moveBetweenTiles");

            //IsMovingBetweenTiles = true;
        }
        /*
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
        */
    }
}
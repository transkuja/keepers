using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class PathBlocker : MonoBehaviour
    {
        [SerializeField]
        Direction[] pathsBlocked;
        BoxCollider[] pathBlockedColliders;
        Tile effectiveTile;

        void Start()
        {
            effectiveTile = GetComponentInParent<Tile>();

            // Check if is blocking paths and deactivate concerned colliders
            if (pathsBlocked != null)
            {
                pathBlockedColliders = new BoxCollider[2 * pathsBlocked.Length];

                for (int i = 0; i < pathsBlocked.Length; i++)
                {
                    pathBlockedColliders[2 * i] = effectiveTile.GetTileTriggerFromDirection(pathsBlocked[i]);
                    pathBlockedColliders[2 * i].enabled = false;
                    pathBlockedColliders[2 * i + 1] = effectiveTile.Neighbors[(int)pathsBlocked[i]].GetTileTriggerFromDirection(Utils.GetOppositeDirection(pathsBlocked[i]));
                    pathBlockedColliders[2 * i + 1].enabled = false;
                }
            }
        }

        private void OnDestroy()
        {
            // Reactivate paths blocked when the blocker is destroyed
            if (pathsBlocked != null)
            {
                for (int i = 0; i < pathBlockedColliders.Length; i++)
                {
                    pathBlockedColliders[i].enabled = true;
                }
            }
        }
    }
}
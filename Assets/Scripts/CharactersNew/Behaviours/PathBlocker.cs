using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class PathBlocker : MonoBehaviour
    {
        [SerializeField]
        Direction[] pathsBlocked;
        GameObject[] pathBlockedPortals;
        Tile effectiveTile;

        void Start()
        {
            effectiveTile = GetComponentInParent<Tile>();

            // Check if is blocking paths and deactivate concerned colliders
            if (pathsBlocked != null)
            {
                pathBlockedPortals = new GameObject[2 * pathsBlocked.Length];

                for (int i = 0; i < pathsBlocked.Length; i++)
                {
                    pathBlockedPortals[2 * i] = effectiveTile.GetTileTriggerFromDirection(pathsBlocked[i]);
                    pathBlockedPortals[2 * i].SetActive(false);
                    pathBlockedPortals[2 * i + 1] = effectiveTile.Neighbors[(int)pathsBlocked[i]].GetTileTriggerFromDirection(Utils.GetOppositeDirection(pathsBlocked[i]));
                    pathBlockedPortals[2 * i + 1].SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            // Reactivate paths blocked when the blocker is destroyed
            if (pathsBlocked != null)
            {
                for (int i = 0; i < pathBlockedPortals.Length; i++)
                {
                    pathBlockedPortals[i].SetActive(true);
                }
            }
        }
    }
}
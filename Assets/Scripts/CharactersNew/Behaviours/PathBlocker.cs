using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class PathBlocker : MonoBehaviour
    {
        [SerializeField]
        Direction[] pathsBlocked;
        TilePassage[] pathBlockedPortals;
        Tile effectiveTile;

        void Start()
        {
            effectiveTile = GetComponentInParent<Tile>();

            // Check if is blocking paths and deactivate concerned colliders
            if (pathsBlocked != null)
            {
                pathBlockedPortals = new TilePassage[2 * pathsBlocked.Length];

                for (int i = 0; i < pathsBlocked.Length; i++)
                {
                    pathBlockedPortals[2 * i] = effectiveTile.GetPassage(pathsBlocked[i]);
                    pathBlockedPortals[2 * i].Status = PassageStatus.Disabled;
                    pathBlockedPortals[2 * i + 1] = effectiveTile.Neighbors[(int)pathsBlocked[i]].GetPassage(Utils.GetOppositeDirection(pathsBlocked[i]));
                    pathBlockedPortals[2 * i + 1].Status = PassageStatus.Disabled;
                }
            }
        }

        private void OnDisable()
        {
            // Reactivate paths blocked when the blocker is destroyed
            if (pathsBlocked != null && pathBlockedPortals != null)
            {
                for (int i = 0; i < pathBlockedPortals.Length; i++)
                {
                    pathBlockedPortals[i].Status = PassageStatus.Off;
                }
            }
        }
    }
}
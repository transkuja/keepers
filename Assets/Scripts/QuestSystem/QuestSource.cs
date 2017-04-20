using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace QuestSystem
{
    [Serializable]
    public class QuestSource
    {
        public string ID;
        public Transform Transform;
        public Tile Tile;
        public bool needsToBeSpawned;
    }

}

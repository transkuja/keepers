using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Monster : MonoBehaviour
    {
        PawnInstance instance;

        public PawnInstance getPawnInstance
        {
            get
            {
                return instance;
            }
        }

        void Start()
        {
            instance = GetComponent<PawnInstance>();
        }

    }
}
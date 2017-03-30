using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class HungerHandler : MonoBehaviour
    {
        PawnInstance instance;

        [SerializeField]
        int maxHunger;
        int currentHunger;
        bool isStarving = false;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            currentHunger = maxHunger;
        }

        public int CurrentHunger
        {
            get { return currentHunger; }
            set
            {
                currentHunger = value;
                if (currentHunger < 0)
                {
                    currentHunger = 0;
                    isStarving = true;
                }
                else if (currentHunger > maxHunger)
                {
                    currentHunger = maxHunger;
                    isStarving = false;
                }
                else
                {
                    isStarving = false;
                }

            }
        }

        public int MaxHunger
        {
            get
            {
                return maxHunger;
            }

            set
            {
                maxHunger = value;
            }
        }

    }
}
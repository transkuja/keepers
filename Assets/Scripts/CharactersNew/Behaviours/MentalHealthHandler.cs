using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class MentalHealthHandler : MonoBehaviour
    {

        PawnInstance instance;

        [SerializeField]
        int maxMentalHealth;
        int currentMentalHealth;
        bool isDepressed = false;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            currentMentalHealth = maxMentalHealth;
        }

        public int CurrentMentalHealth
        {
            get { return currentMentalHealth; }
            set
            {
                currentMentalHealth = value;
                if (currentMentalHealth < 0)
                {
                    currentMentalHealth = 0;
                    isDepressed = true;
                }
                else if (currentMentalHealth > maxMentalHealth)
                {
                    currentMentalHealth = maxMentalHealth;
                    isDepressed = false;
                }
                else
                {
                    isDepressed = false;
                }

            }
        }

        public int MaxMentalHealth
        {
            get
            {
                return maxMentalHealth;
            }

            set
            {
                maxMentalHealth = value;
            }
        }
    }
}
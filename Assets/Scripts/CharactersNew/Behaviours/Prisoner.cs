using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Prisoner : MonoBehaviour
    {

        PawnInstance instance;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
        }


    }
}
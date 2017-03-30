using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Fighter : MonoBehaviour
    {
        PawnInstance instance;
        bool isTargetableByMonster = true;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
        }


        public List<SkillBattle> BattleSkills
        {
            get
            {
                return battleSkills;
            }

            set
            {
                battleSkills = value;
            }
        }

        public bool IsTargetableByMonster
        {
            get
            {
                return isTargetableByMonster;
            }

            set
            {
                isTargetableByMonster = value;
            }
        }

        // TODO add battle associated fields
        [SerializeField]
        private List<SkillBattle> battleSkills;

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Fighter : MonoBehaviour
    {
        PawnInstance instance;
        bool isAMonster;

        // Non monster variables
        bool isTargetableByMonster = true;

        // Monster variables
        bool hasRecentlyBattled = false;

        // TODO add battle associated fields
        [SerializeField]
        private List<SkillBattle> battleSkills;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            if (GetComponent<Monster>() != null) IsAMonster = true;
            else IsAMonster = false;
        }

        #region Accessors
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

        public bool HasRecentlyBattled
        {
            get
            {
                return hasRecentlyBattled;
            }

            set
            {
                hasRecentlyBattled = value;
            }
        }

        public bool IsAMonster
        {
            get
            {
                return isAMonster;
            }

            set
            {
                isAMonster = value;
            }
        }
        #endregion


        #region Monster functions
        public void RestAfterBattle()
        {
            foreach (BoxCollider bc in GetComponentsInChildren<BoxCollider>())
                bc.enabled = false;

            HasRecentlyBattled = true;

            // TODO fix this
            GetComponent<Monster>().HasRecentlyBattled = true;

            Invoke("ReactivateTrigger", 3.0f);
        }

        private void ReactivateTrigger()
        {
            foreach (BoxCollider bc in GetComponentsInChildren<BoxCollider>())
                bc.enabled = true;
            HasRecentlyBattled = false;

            // TODO fix this
            GetComponent<Monster>().HasRecentlyBattled = false;

        }
        #endregion
    }
}

/*
 * Contains definition of battle skills 
 */
[System.Serializable]
public class SkillBattle
{
    private int damage;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}

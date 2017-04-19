using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Fighter : MonoBehaviour
    {
        PawnInstance instance;
        // TODO: externalize this in Monster
        bool isAMonster;

        // Non monster variables
        bool isTargetableByMonster = true;

        // TODO: externalize this in Monster
        // Monster variables
        bool hasRecentlyBattled = false;

        // Battle stats
        [SerializeField]
        private List<SkillBattle> battleSkills;
        [SerializeField]
        int nbrOfDice;
        [SerializeField]
        Die[] dice;

        // Instance variables
        int physicalSymbolStored = 0;
        int magicalSymbolStored = 0;
        int defensiveSymbolStored = 0;
        int supportSymbolStored = 0;

        bool hasPlayedThisTurn = false;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
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

        // TODO: externalize this in Monster
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

        // TODO: externalize this in Monster
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

        public Die[] Dice
        {
            get
            {
                return dice;
            }

            set
            {
                dice = value;
            }
        }

        public int PhysicalSymbolStored
        {
            get
            {
                return physicalSymbolStored;
            }

            set
            {
                physicalSymbolStored = value;
            }
        }

        public int MagicalSymbolStored
        {
            get
            {
                return magicalSymbolStored;
            }

            set
            {
                magicalSymbolStored = value;
            }
        }

        public int DefensiveSymbolStored
        {
            get
            {
                return defensiveSymbolStored;
            }

            set
            {
                defensiveSymbolStored = value;
            }
        }

        public int SupportSymbolStored
        {
            get
            {
                return supportSymbolStored;
            }

            set
            {
                supportSymbolStored = value;
            }
        }

        public bool HasPlayedThisTurn
        {
            get
            {
                return hasPlayedThisTurn;
            }

            set
            {
                hasPlayedThisTurn = value;
            }
        }

        #endregion

        // TODO: externalize this in Monster
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
    private string description;
    private Dictionary<FaceType, int> cost = new Dictionary<FaceType, int>();

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public Dictionary<FaceType, int> Cost
    {
        get
        {
            return cost;
        }

        set
        {
            cost = value;
        }
    }
}

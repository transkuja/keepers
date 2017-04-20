using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Fighter : MonoBehaviour
    {
        PawnInstance instance;
        private InteractionImplementer battleInteractions;
        private Transform interactionsPosition;

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
        [SerializeField]
        int physicalSymbolStored = 0;
        [SerializeField]
        int magicalSymbolStored = 0;
        [SerializeField]
        int defensiveSymbolStored = 0;
        [SerializeField]
        int supportSymbolStored = 0;

        bool hasPlayedThisTurn = false;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
            battleInteractions = new InteractionImplementer();
            foreach (Transform child in transform)
            {
                if (child.CompareTag("FeedbackTransform"))
                {
                    interactionsPosition = child;
                    break;
                }
            }
        }

        void Start()
        {
            if (GetComponent<Monster>() != null) IsAMonster = true;
            else IsAMonster = false;

            battleInteractions.Add(new Interaction(Attack), 0, "Attack", GameManager.Instance.SpriteUtils.spriteMoral);
            battleInteractions.Add(new Interaction(Guard), 0, "Guard", GameManager.Instance.SpriteUtils.spriteMoral);
            battleInteractions.Add(new Interaction(OpenSkillPanel), 0, "OpenSkillPanel", GameManager.Instance.SpriteUtils.spriteMoral);

        }

        public void ResetValuesAfterBattle()
        {
            physicalSymbolStored = 0;
            magicalSymbolStored = 0;
            defensiveSymbolStored = 0;
            supportSymbolStored = 0;
            hasPlayedThisTurn = false;
        }

        #region Interactions
        public void Attack(int _i = 0)
        {
            Debug.Log("attack");
            HasPlayedThisTurn = true;
        }

        public void Guard(int _i = 0)
        {
            Debug.Log("guard");
            HasPlayedThisTurn = true;
        }

        public void OpenSkillPanel(int _i = 0)
        {
            Debug.Log("openskillpanel");
        }
        #endregion

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

        public Transform InteractionsPosition
        {
            get
            {
                return interactionsPosition;
            }

            set
            {
                interactionsPosition = value;
            }
        }

        public InteractionImplementer BattleInteractions
        {
            get
            {
                return battleInteractions;
            }

            set
            {
                battleInteractions = value;
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

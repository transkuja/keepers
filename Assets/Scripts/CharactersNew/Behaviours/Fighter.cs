using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Fighter : MonoBehaviour
    {
        // Balance variables
        private int effectiveAttackValue = 5;
        private int effectiveDefenseValue = 5;

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

        bool hasClickedOnAttack = false;

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
            HasClickedOnAttack = true;
        }

        public void AttackProcess(Fighter _attackTarget)
        {
            Debug.Log("attackProcess lunched");
            Debug.Log(BattleHandler.LastThrowResult.ContainsKey(GetComponent<PawnInstance>()));
            Debug.Log(BattleHandler.LastThrowResult[GetComponent<PawnInstance>()]);
            foreach(PawnInstance pi in BattleHandler.LastThrowResult.Keys)
            {
                Debug.Log(pi.Data.PawnName);
            }
            Face[] lastThrowFaces = BattleHandler.LastThrowResult[GetComponent<PawnInstance>()];
            int attackDamage = 0;
            for (int i = 0; i < lastThrowFaces.Length; i++)
            {
                // Apply attack calculation
                if (lastThrowFaces[i].Type == FaceType.Physical)
                {
                    attackDamage += (effectiveAttackValue * lastThrowFaces[i].Value);
                }
                else
                {
                    // All non-physical faces count for 1 damage only
                    attackDamage += 1;
                }
            }
            if (_attackTarget.GetComponent<Keeper>() != null || _attackTarget.GetComponent<Escortable>() != null)
            {
                int effectiveDamage = (int)((float)attackDamage / (defensiveSymbolStored + 1));
                _attackTarget.GetComponent<Mortal>().CurrentHp -= effectiveDamage;
            }
            else if (_attackTarget.GetComponent<Monster>() != null)
            {
                // max defense => 10% dmg taken
                // 0 defense => 100% dmg taken
                int effectiveDamage = Mathf.Max((int)(attackDamage/(Mathf.Sqrt(_attackTarget.GetComponent<Monster>().EffectiveDefense + 1))), (int)(attackDamage/10.0f));
                _attackTarget.GetComponent<Mortal>().CurrentHp -= effectiveDamage;
            }

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

        public bool HasClickedOnAttack
        {
            get
            {
                return hasClickedOnAttack;
            }

            set
            {
                hasClickedOnAttack = value;
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

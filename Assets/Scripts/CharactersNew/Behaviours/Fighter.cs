using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Behaviour
{
    public class Fighter : MonoBehaviour
    {
        // Balance variables
        private int effectiveAttackValue = 5;
        private int effectiveDefenseValue = 5;
        private int stockMaxValue = 20;

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

        int baseAttack = 0;
        int baseDefense = 0;
        int temporaryDefense = 0;

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

        Face[] lastThrowResult;
        List<GameObject> lastThrowDiceInstance;

        // Pending variables
        bool isWaitingForDmgFeedback = false;
        int pendingDamage = 0;
        bool isWaitingForSkillPanelToClose = false;
        float showSkillPanelTimer = 2.2f;
        float showFeedbackDmgTimer = 1.7f;

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

            battleInteractions.Add(new Interaction(Attack), 0, "Attack", GameManager.Instance.SpriteUtils.spriteAttack);
            battleInteractions.Add(new Interaction(Guard), 0, "Guard", GameManager.Instance.SpriteUtils.spriteGuard);
            battleInteractions.Add(new Interaction(OpenSkillPanel), 0, "OpenSkillPanel", GameManager.Instance.SpriteUtils.spriteUseSkill);

            if (dice != null)
            {
                for (int i = 0; i < dice.Length; i++)
                {
                    int minDefenseForThisDice = 7;
                    int minAttackForThisDice = 7;
                    for (int j = 0; j < 6; j++)
                    {
                        if (dice[i].Faces[j].Type == FaceType.Physical)
                        {
                            if (dice[i].Faces[j].Value < minAttackForThisDice)
                                minAttackForThisDice = dice[i].Faces[j].Value;
                        }
                        if (dice[i].Faces[j].Type == FaceType.Defensive)
                        {
                            if (dice[i].Faces[j].Value < minDefenseForThisDice)
                                minDefenseForThisDice = dice[i].Faces[j].Value;
                        }
                    }
                    if (minAttackForThisDice == 7) minAttackForThisDice = 0;
                    if (minDefenseForThisDice == 7) minDefenseForThisDice = 0;
                    baseAttack += minAttackForThisDice;
                    baseDefense += minDefenseForThisDice;
                }
            }

            showSkillPanelTimer = 2.2f;
            showFeedbackDmgTimer = 1.7f;
        }

        private void Update()
        {
            if (isWaitingForSkillPanelToClose)
            {
                if (showSkillPanelTimer < 0.0f)
                {
                    GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName.SetActive(false);
                    showSkillPanelTimer = 2.2f;
                    showFeedbackDmgTimer = 1.7f;
                    isWaitingForSkillPanelToClose = false;
                    BattleHandler.IsWaitingForSkillEnd = false;
                }
                else
                {
                    showSkillPanelTimer -= Time.deltaTime;
                }

                if (isWaitingForDmgFeedback)
                {
                    if (showFeedbackDmgTimer < 0.0f)
                    {
                        GetComponent<PawnInstance>().AddFeedBackToQueue(-pendingDamage);
                        GetComponent<Mortal>().CurrentHp -= pendingDamage;
                        isWaitingForDmgFeedback = false;
                    }
                    else
                    {
                        showFeedbackDmgTimer -= Time.deltaTime;
                    }
                }
            }
        }

        void OnDestroy()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
                BattleHandler.IsWaitingForSkillEnd = false;
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

            int attackDamage = baseAttack * effectiveAttackValue;
            for (int i = 0; i < lastThrowResult.Length; i++)
            {
                // Apply attack calculation
                if (lastThrowResult[i].Type == FaceType.Physical)
                {
                    attackDamage += (effectiveAttackValue * lastThrowResult[i].Value);
                }

            }
            if (_attackTarget.GetComponent<Keeper>() != null || _attackTarget.GetComponent<Escortable>() != null)
            {
                int effectiveDamage = (int)((attackDamage * (baseDefense + temporaryDefense))/100.0f);
                _attackTarget.GetComponent<Mortal>().CurrentHp -= effectiveDamage;
            }
            else if (_attackTarget.GetComponent<Monster>() != null)
            {
                // max defense => 10% dmg taken
                // 0 defense => 100% dmg taken
                
                int effectiveDamage = ComputeEffectiveDamage(_attackTarget, attackDamage);
                _attackTarget.GetComponent<Mortal>().CurrentHp -= effectiveDamage;
                _attackTarget.GetComponent<PawnInstance>().AddFeedBackToQueue(-effectiveDamage);
            }
            hasClickedOnAttack = false;
            HasPlayedThisTurn = true;
        }

        // Compute effective damage when hitting a monster
        private int ComputeEffectiveDamage(Fighter _target, int _attackDmg)
        {
            return _attackDmg;
            //return Mathf.Max((int)(_attackDmg / (Mathf.Sqrt(_target.GetComponent<Monster>().EffectiveDefense + 1))), (int)(_attackDmg / 10.0f));
        }

        public void Guard(int _i = 0)
        {
            Debug.Log("guard");
            for (int i = 0; i < lastThrowResult.Length; i++)
            {
                // Apply attack calculation
                if (lastThrowResult[i].Type == FaceType.Defensive)
                {
                    temporaryDefense += lastThrowResult[i].Value;
                }

            }
            GetComponent<PawnInstance>().AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteDefenseSymbol, temporaryDefense);
            BattleHandler.ActivateFeedbackSelection(true, false);
            HasPlayedThisTurn = true;
        }

        public void OpenSkillPanel(int _i = 0)
        {
            Debug.Log("openskillpanel");
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(GetComponent<PawnInstance>()).gameObject.SetActive(true);
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
                if (GetComponent<Monster>() != null)
                    GetComponent<Monster>().HasRecentlyBattled = value;
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
                if (physicalSymbolStored > stockMaxValue)
                    physicalSymbolStored = stockMaxValue;
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAttributesStocks(this);
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
                if (magicalSymbolStored > stockMaxValue)
                    magicalSymbolStored = stockMaxValue;
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAttributesStocks(this);
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
                if (defensiveSymbolStored > stockMaxValue)
                    defensiveSymbolStored = stockMaxValue;
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAttributesStocks(this);
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
                if (supportSymbolStored > stockMaxValue)
                    supportSymbolStored = stockMaxValue;
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAttributesStocks(this);
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
                if (hasPlayedThisTurn == true)
                {
                    GameManager.Instance.ClearListKeeperSelected();
                    BattleHandler.CheckTurnStatus();
                }
                else
                {
                    temporaryDefense = 0;
                }
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
                if (hasClickedOnAttack == true)
                {
                    GameManager.Instance.Ui.mouseFollower.SetActive(true);
                    GameManager.Instance.Ui.mouseFollower.GetComponent<MouseFollower>().ExpectedTarget(TargetType.Foe);
                    BattleHandler.ActivateFeedbackSelection(false, true);
                }
            }
        }

        public Face[] LastThrowResult
        {
            get
            {
                return lastThrowResult;
            }

            set
            {
                lastThrowResult = value;
            }
        }

        public List<GameObject> LastThrowDiceInstance
        {
            get
            {
                return lastThrowDiceInstance;
            }

            set
            {
                lastThrowDiceInstance = value;
            }
        }

        public bool IsWaitingForDmgFeedback
        {
            get
            {
                return isWaitingForDmgFeedback;
            }

            set
            {
                isWaitingForDmgFeedback = value;
            }
        }

        public bool IsWaitingForSkillPanelToClose
        {
            get
            {
                return isWaitingForSkillPanelToClose;
            }

            set
            {
                isWaitingForSkillPanelToClose = value;
            }
        }

        public int PendingDamage
        {
            get
            {
                return pendingDamage;
            }

            set
            {
                if (GetComponent<Keeper>() != null || GetComponent<Escortable>() != null)
                    pendingDamage = Mathf.Max(value - baseDefense - temporaryDefense, 0);
                else
                    pendingDamage = ComputeEffectiveDamage(this, value);
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

public enum TargetType { Friend, Foe}

/*
 * Contains definition of battle skills 
 */
[System.Serializable]
public class SkillBattle
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private string skillName;
    [SerializeField]
    private string description;
    [SerializeField]
    private List<Face> cost = new List<Face>();
    [SerializeField]
    TargetType targetType;

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

    public List<Face> Cost
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

    public TargetType TargetType
    {
        get
        {
            return targetType;
        }

        set
        {
            targetType = value;
        }
    }

    public string SkillName
    {
        get
        {
            return skillName;
        }

        set
        {
            skillName = value;
        }
    }

    public bool CanUseSkill(Behaviour.Fighter _user)
    {
        foreach (Face f in cost)
        {
            if (f.Type == FaceType.Physical && _user.PhysicalSymbolStored < f.Value)
                return false;
            if (f.Type == FaceType.Magical && _user.MagicalSymbolStored < f.Value)
                return false;

            if (f.Type == FaceType.Defensive && _user.DefensiveSymbolStored < f.Value)
                return false;
            if (f.Type == FaceType.Support && _user.SupportSymbolStored < f.Value)
                return false;
        }
        return true;
    }

    public void UseSkill(Behaviour.Fighter _user, PawnInstance _target)
    {
        foreach (Face f in cost)
        {
            if (f.Type == FaceType.Physical)
                _user.PhysicalSymbolStored -= f.Value;
            if (f.Type == FaceType.Magical)
                _user.MagicalSymbolStored -= f.Value;

            if (f.Type == FaceType.Defensive)
                _user.DefensiveSymbolStored -= f.Value;
            if (f.Type == FaceType.Support)
                _user.SupportSymbolStored -= f.Value;
        }

        GameObject skillNameUI = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName;
        skillNameUI.transform.GetComponentInChildren<Text>().text = skillName;
        skillNameUI.SetActive(true);
        _target.GetComponent<Behaviour.Fighter>().IsWaitingForDmgFeedback = true;
        _target.GetComponent<Behaviour.Fighter>().IsWaitingForSkillPanelToClose = true;
        _target.GetComponent<Behaviour.Fighter>().PendingDamage = damage;

        BattleHandler.IsWaitingForSkillEnd = true;
    }
}

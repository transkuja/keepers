using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Behaviour;

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

        // Instance variables
        [SerializeField]
        int physicalSymbolStored = 0;
        [SerializeField]
        int magicalSymbolStored = 0;
        [SerializeField]
        int defensiveSymbolStored = 0;

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
            battleInteractions.Add(new Interaction(OpenSkillPanel), 0, "OpenSkillPanel", GameManager.Instance.SpriteUtils.spriteUseSkill);

            showSkillPanelTimer = 2.2f;
            showFeedbackDmgTimer = 1.7f;

            foreach (SkillBattle sb in battleSkills)
            {
                if (sb.SkillUser == null)
                    sb.SkillUser = this;
            }
        }

        private void Update()
        {
            if (isWaitingForSkillPanelToClose)
            {
                if (showSkillPanelTimer < 0.0f)
                {
                    EndSkillProcess();
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
            if (BattleHandler.IsABattleAlreadyInProcess() && BattleHandler.IsWaitingForSkillEnd)
            {
                EndSkillProcess();
            }
        }

        private void EndSkillProcess()
        {
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName.SetActive(false);
            showSkillPanelTimer = 2.2f;
            showFeedbackDmgTimer = 1.7f;
            isWaitingForSkillPanelToClose = false;
            BattleHandler.IsWaitingForSkillEnd = false;
            if (GetComponent<Mortal>().CurrentHp <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void ResetValuesAfterBattle()
        {
            physicalSymbolStored = 0;
            magicalSymbolStored = 0;
            defensiveSymbolStored = 0;
            hasPlayedThisTurn = false;
        }

        public void UpdateSkillButton(bool _hasAUsableSkill)
        {
            if (_hasAUsableSkill)
            {
                // TODO: change to proper sprite
                battleInteractions.SwapSprite("OpenSkillPanel", GameManager.Instance.SpriteUtils.spriteUseSkill);
            }
            else
            {
                battleInteractions.SwapSprite("OpenSkillPanel", GameManager.Instance.SpriteUtils.spriteUseSkill);
            }
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

            int attackDamage = 0;
            for (int i = 0; i < lastThrowResult.Length; i++)
            {
                // Apply attack calculation
                if (lastThrowResult[i].Type == FaceType.Physical)
                {
                    attackDamage += (effectiveAttackValue * lastThrowResult[i].Value);
                }
                else
                {
                    attackDamage += 1;
                }

            }
            if (_attackTarget.GetComponent<Keeper>() != null || _attackTarget.GetComponent<Escortable>() != null)
            {
                int effectiveDamage = (int)((attackDamage / (float)effectiveAttackValue));
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

            if (_attackTarget.GetComponent<Mortal>().CurrentHp <= 0)
                _attackTarget.gameObject.SetActive(false);

            hasClickedOnAttack = false;
            HasPlayedThisTurn = true;
        }

        // Compute effective damage when hitting a monster
        private int ComputeEffectiveDamage(Fighter _target, int _attackDmg)
        {
            return _attackDmg;
            //return Mathf.Max((int)(_attackDmg / (Mathf.Sqrt(_target.GetComponent<Monster>().EffectiveDefense + 1))), (int)(_attackDmg / 10.0f));
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
                    pendingDamage = value;
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
public class SkillBattle {

    [SerializeField]
    private Fighter skillUser;

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

    private SkillBattle depressedVersion;

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

    public Fighter SkillUser
    {
        get
        {
            return skillUser;
        }

        set
        {
            skillUser = value;
        }
    }

    public SkillBattle()
    {

    }

    public SkillBattle(SkillBattle _origin)
    {
        if (_origin == null)
        {
            new SkillBattle();
        }
        else
        {
            skillUser = _origin.skillUser;
            damage = _origin.damage;

            skillName = _origin.skillName;
            description = _origin.description;
            cost = _origin.cost;
            targetType = _origin.targetType;

            depressedVersion = new SkillBattle(_origin.depressedVersion);
        }
    }

    public bool CanUseSkill()
    {
        foreach (Face f in cost)
        {
            if (f.Type == FaceType.Physical && skillUser.PhysicalSymbolStored < f.Value)
                return false;
            if (f.Type == FaceType.Magical && skillUser.MagicalSymbolStored < f.Value)
                return false;

            if (f.Type == FaceType.Defensive && skillUser.DefensiveSymbolStored < f.Value)
                return false;
        }
        return true;
    }

    public void UseSkill(PawnInstance _target)
    {
        if (depressedVersion != null && skillUser.GetComponent<MentalHealthHandler>() != null && skillUser.GetComponent<MentalHealthHandler>().IsDepressed)
        {
            depressedVersion.UseSkill(_target);
        }
        else
        {
            foreach (Face f in cost)
            {
                if (f.Type == FaceType.Physical)
                    skillUser.PhysicalSymbolStored -= f.Value;
                if (f.Type == FaceType.Magical)
                    skillUser.MagicalSymbolStored -= f.Value;

                if (f.Type == FaceType.Defensive)
                    skillUser.DefensiveSymbolStored -= f.Value;
            }

            GameObject skillNameUI = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName;
            skillNameUI.transform.GetComponentInChildren<Text>().text = skillName;
            skillNameUI.SetActive(true);
            _target.GetComponent<Fighter>().IsWaitingForDmgFeedback = true;
            _target.GetComponent<Fighter>().IsWaitingForSkillPanelToClose = true;
            _target.GetComponent<Fighter>().PendingDamage = damage;

            BattleHandler.IsWaitingForSkillEnd = true;
        }
    }
}

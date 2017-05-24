using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Behaviour;
using System;

namespace Behaviour
{
    public class Fighter : MonoBehaviour
    {
        // Balance variables
        //private int effectiveAttackValue = 3;

        // Warning, UI is not think for values above 9, so ask before changing this setting
        public static int StockMaxValue = 9;

        //PawnInstance instance;
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

        // Fix, is there a way to set directly a sub skillbattle?
        [SerializeField]
        private List<SkillBattle> depressedSkills;
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

        Face[] lastThrowResult;
        List<GameObject> lastThrowDiceInstance;

        // Pending variables
        bool isWaitingForDmgFeedback = false;
        int pendingDamage = 0;
        bool isWaitingForSkillPanelToClose = false;
        float showSkillPanelTimer = 0.5f;
        float showFeedbackTimer = -1.0f;

        SkillDecisionAlgo skillDecisionAlgo;
        List<BattleBoeuf> effectiveBoeufs = new List<BattleBoeuf>();

        int consecutiveShots = 0;
        bool hasPlayedARapidSkill = false;
        bool isPendingDamageRealDamage = false;

        void Awake()
        {
            //instance = GetComponent<PawnInstance>();
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

            showSkillPanelTimer = 0.5f;

            if (!IsAMonster && GetComponent<Prisoner>() == null)
            {
                SkillBattle defaultAtk = new SkillBattle("default");
                battleSkills.Insert(0, defaultAtk);
                depressedSkills.Insert(0, defaultAtk);
            }

            foreach (SkillBattle sb in battleSkills)
            {
                if (sb.SkillUser == null)
                    sb.SkillUser = this;
            }

            if (GetComponent<LuckBased>() != null || GetComponent<MentalHealthHandler>() == null)
            {
                foreach (SkillBattle sb in battleSkills)
                    depressedSkills.Add(new SkillBattle(sb));
            }
            else
            {
                foreach (SkillBattle sb in depressedSkills)
                {
                    if (sb.SkillUser == null)
                        sb.SkillUser = this;
                }
            }

            MonstersBattleSkillsSelection mbss = new MonstersBattleSkillsSelection();
            skillDecisionAlgo = mbss.GetDecisionAlgorithm(GetComponent<PawnInstance>().Data.PawnId);
        }

        private void Update()
        {
            if (isWaitingForSkillPanelToClose)
            {
                if (isWaitingForDmgFeedback)
                {
                    if (showFeedbackTimer == -1.0f)
                    {
                        showFeedbackTimer = BattleHandler.CurrentSkillAnimDuration;
                    }
                    if (showFeedbackTimer < 0.0f)
                    {
                        if (pendingDamage != 0 || isPendingDamageRealDamage)
                        {
                            GetComponent<PawnInstance>().AddFeedBackToQueue(-pendingDamage);
                            GetComponent<Mortal>().CurrentHp -= pendingDamage;
                        }
                        isWaitingForDmgFeedback = false;
                        if (pendingDamage > 0)
                            GetComponent<AnimatedPawn>().Anim.SetTrigger("getHit");
                    }
                    else
                    {
                        showFeedbackTimer -= Time.deltaTime;
                    }
                    
                }
                else
                {
                    if (showSkillPanelTimer < 0.0f)
                    {
                        if (GameManager.Instance.CurrentState != GameState.InTuto
                            || (GameManager.Instance.CurrentState == GameState.InTuto && (GetComponent<Keeper>() != null || GetComponent<Escortable>() != null)))
                        {
                            EndSkillProcess();
                        }
                    }
                    else
                    {
                        showSkillPanelTimer -= Time.deltaTime;
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

        public void EndSkillProcess()
        {
            showSkillPanelTimer = 0.5f;
            showFeedbackTimer = -1.0f;
            isWaitingForSkillPanelToClose = false;
            //if (BattleHandler.PendingSkill != null)
            //{
                //GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName.SetActive(false);
                
                BattleHandler.IsWaitingForSkillEnd = false;

            //}
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
            consecutiveShots = 0;
            hasPlayedThisTurn = false;
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
                if (physicalSymbolStored > StockMaxValue)
                    physicalSymbolStored = StockMaxValue;
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
                if (magicalSymbolStored > StockMaxValue)
                    magicalSymbolStored = StockMaxValue;
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
                if (defensiveSymbolStored > StockMaxValue)
                    defensiveSymbolStored = StockMaxValue;
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
                if (hasPlayedARapidSkill)
                {
                    hasPlayedThisTurn = false;
                    hasPlayedARapidSkill = false;
                    GameManager.Instance.ClearListKeeperSelected();
                    BattleHandler.CheckTurnStatus();
                }
                else if (hasPlayedThisTurn == true)
                {
                    consecutiveShots = 0;
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
                pendingDamage = ComputeEffectiveDamage(this, value);
            }
        }

        private int ComputeEffectiveDamage(Fighter _fighter, int value)
        {
            if (value > 0)
            {
                foreach (BattleBoeuf boeuf in _fighter.effectiveBoeufs)
                {
                    if (boeuf.BoeufType == BoeufType.Defense)
                        value -= boeuf.EffectValue;
                }
                isPendingDamageRealDamage = true;
                return Mathf.Max(0, value);
            }
            else
            {
                isPendingDamageRealDamage = false;
                return Mathf.Min(0, value);
            }
        }

        public SkillDecisionAlgo SkillDecisionAlgo
        {
            get
            {
                return skillDecisionAlgo;
            }

            set
            {
                skillDecisionAlgo = value;
            }
        }

        public List<BattleBoeuf> EffectiveBoeufs
        {
            get
            {
                return effectiveBoeufs;
            }

            set
            {
                effectiveBoeufs = value;
            }
        }

        public List<SkillBattle> DepressedSkills
        {
            get
            {
                return depressedSkills;
            }

            set
            {
                depressedSkills = value;
            }
        }

        public int ConsecutiveShots
        {
            get
            {
                return consecutiveShots;
            }

            set
            {
                consecutiveShots = value;
            }
        }

        public bool HasPlayedARapidSkill
        {
            get
            {
                return hasPlayedARapidSkill;
            }

            set
            {
                hasPlayedARapidSkill = value;
            }
        }

        #endregion

        public void UseSkill(PawnInstance _target)
        {
            SkillBattle sb = skillDecisionAlgo.Invoke(this);
            if (sb.TargetType == TargetType.FriendSingle) sb.UseSkill(_target);
            else sb.UseSkill();
        }

        public void UpdateActiveBoeufs()
        {
            for (int i = 0; i < effectiveBoeufs.Count; i++)
            {
                effectiveBoeufs[i].Duration--;
                if (effectiveBoeufs[i].Duration == 0)
                {
                    effectiveBoeufs.Remove(effectiveBoeufs[i]);
                }
            }
            BuffFeedback bf = GetComponentInChildren<BuffFeedback>();
            if (bf != null)
                bf.UpdateCurrentBoeufsList(effectiveBoeufs);
        }

        public void AddBoeuf(BattleBoeuf _newBoeuf)
        {
            effectiveBoeufs.Add(_newBoeuf);
            BuffFeedback bf = GetComponentInChildren<BuffFeedback>();
            if (bf != null)
                bf.UpdateCurrentBoeufsList(effectiveBoeufs);
        }

        public void RemoveBoeuf(BattleBoeuf _oldBoeuf)
        {
            effectiveBoeufs.Remove(_oldBoeuf);
            BuffFeedback bf = GetComponentInChildren<BuffFeedback>();
            if (bf != null)
                bf.UpdateCurrentBoeufsList(effectiveBoeufs);
        }

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
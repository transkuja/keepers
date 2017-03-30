using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Behaviour
{
    public class Keeper : MonoBehaviour
    {
        PawnInstance instance;

        // UI
        public GameObject shorcutUI;
        public GameObject selectedPanelUI;
        public GameObject selectedStatPanelUI;
        public GameObject selectedActionPointsUI;

        // Interactions variables
        public int minMoralBuff = -10;
        public int maxMoralBuff = 20;

        // Actions
        [Header("Actions")]

        private int actionPoints;
        [SerializeField]
        private int maxActionPoints = 3;


        [SerializeField]
        private GameObject feedbackSelection;
        private bool isSelected = false;
        // Used only in menu. Handles selection in main menu.
        [SerializeField]
        private bool isSelectedInMenu = false;
        NavMeshAgent agent;

        private List<GameObject> goListCharacterFollowing = new List<GameObject>();

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            if (instance.Data.Behaviours[(int)BehavioursEnum.CanSpeak])
                instance.Interactions.Add(new Interaction(MoralBuff), 1, "Moral", GameManager.Instance.SpriteUtils.spriteMoral);

            agent = GetComponent<NavMeshAgent>();

            shorcutUI = CreateShortcutKeeperUI();

            // Need Equipement and inventory data
            ActionPoints = MaxActionPoints;
        }

        public bool IsTheLastKeeperOnTheTile()
        {
            bool isTheLastOnTile = true;
            foreach (PawnInstance pi in GameManager.Instance.AllKeepersList)
            {
                if (pi.GetComponent<Mortal>().IsAlive)
                {
                    if (pi != instance && pi.CurrentTile == instance.CurrentTile)
                    {
                        isTheLastOnTile = false;
                        break;
                    }
                }
            }
            return isTheLastOnTile;
        }

        #region Interactions

        public void MoralBuff(int _i = 0)
        {
            if (GameManager.Instance.ListOfSelectedKeepersOld.Count > 0)
            {
                int costAction = instance.Interactions.Get("Moral").costAction;
                if (GameManager.Instance.ListOfSelectedKeepersOld[0].ActionPoints >= costAction)
                {
                    GameManager.Instance.ListOfSelectedKeepersOld[0].ActionPoints -= (short)costAction;
                    short amountMoralBuff = (short)Random.Range(minMoralBuff, maxMoralBuff);
                    GameManager.Instance.GoTarget.GetComponentInParent<KeeperInstance>().CurrentMentalHealth += amountMoralBuff;
                    GameManager.Instance.Ui.UpdateShortcutPanel();
                    GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
                    GameManager.Instance.Ui.MoralBuffActionTextAnimation(amountMoralBuff);
                }
                else
                {
                    GameManager.Instance.Ui.ZeroActionTextAnimation();
                }
            }
        }
        #endregion

        #region UI

        private enum PanelShortcutChildren { Image, HpGauge, HungerGauge, MentalHealthGauge, ActionPoints };
        private enum PanelSelectedKeeperStatChildren { Image, ButtonCycleLeft, ButtonCycleRight, ActionPoints, StatTrigger };
        public GameObject CreateShortcutKeeperUI()
        {

            Sprite associatedSprite = instance.Data.AssociatedSprite;
            GameObject goShortcutKeeperUi = Instantiate(GameManager.Instance.PrefabUtils.PrefabSelectedCharacterUI, GameManager.Instance.Ui.goShortcutKeepersPanel.transform);

            goShortcutKeeperUi.name = "Panel_Shortcut_" + instance.Data.PawnName;
            goShortcutKeeperUi.transform.GetChild((int)PanelShortcutChildren.Image).GetComponent<Image>().sprite = associatedSprite;
            goShortcutKeeperUi.transform.localScale = Vector3.one;
            goShortcutKeeperUi.GetComponent<Button>().onClick.AddListener(() => GoToKeeper());

            return goShortcutKeeperUi;
        }

        public void ShowSelectdePanelUI(bool isShow)
        {
            selectedPanelUI.SetActive(isShow);
        }

        public void GoToKeeper()
        {
            IsSelected = true;
        }

        public void UpdateActionPoint(int actionPoint)
        {
            selectedActionPointsUI.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = actionPoint.ToString();
            //selectedActionPointsUI.GetChild(0).gameObject.GetComponent<Image>().fillAmount = actionPoint / maxActionPoints; ;
        }
        #endregion

        #region Accessors

        public int ActionPoints
        {
            get
            {
                return actionPoints;
            }

            set
            {
                if (value < actionPoints) GameManager.Instance.Ui.DecreaseActionTextAnimation(actionPoints - value);
                actionPoints = value;
                UpdateActionPoint(actionPoints);
                if (actionPoints > MaxActionPoints)
                    actionPoints = MaxActionPoints;
                if (actionPoints < 0)
                    actionPoints = 0;
            }
        }

        public List<GameObject> GoListCharacterFollowing
        {
            get
            {
                return goListCharacterFollowing;
            }

            set
            {
                goListCharacterFollowing = value;
            }
        }

        public PawnInstance getPawnInstance
        {
            get
            {
                return instance;
            }
        }


        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
            }
        }

        public int MaxActionPoints
        {
            get
            {
                return maxActionPoints;
            }

            set
            {
                maxActionPoints = value;
            }
        }
        #endregion
    }
}
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI;


public enum PanelShortcutChildren { Image, ActionPoints, HpGauge, HungerGauge, MentalHealthGauge };
public enum PanelSelectedKeeperStatChildren { Image, ButtonCycleRight, ButtonCycleLeft, ActionPoints, StatTrigger };
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

        void Awake()
        {
            instance = GetComponent<PawnInstance>();

            CreateShortcutKeeperUI();
            CreateSelectedPanel();
        }

        void Start()
        {
    
            if (instance.Data.Behaviours[(int)BehavioursEnum.CanSpeak])
                instance.Interactions.Add(new Interaction(MoralBuff), 1, "Moral", GameManager.Instance.SpriteUtils.spriteMoral);

            agent = GetComponent<NavMeshAgent>();

            shorcutUI.name = "Shortcut" + instance.Data.PawnName;
            selectedPanelUI.name = "SelectedKeeper" + instance.Data.PawnName;
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
            int costAction = instance.Interactions.Get("Moral").costAction;
            if (ActionPoints >= costAction)
            {
                ActionPoints -= (short)costAction;
                short amountMoralBuff = (short)Random.Range(minMoralBuff, maxMoralBuff);
                GetComponent<MentalHealthHandler>().CurrentMentalHealth += amountMoralBuff;
                GameManager.Instance.Ui.MoralBuffActionTextAnimation(amountMoralBuff);
            }
            else
            {
                GameManager.Instance.Ui.ZeroActionTextAnimation();
                }
        }
        #endregion

        #region UI
        public void CreateShortcutKeeperUI()
        {
            Sprite associatedSprite = instance.Data.AssociatedSprite;
            shorcutUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabShorcutCharacter, GameManager.Instance.Ui.goShortcutKeepersPanel.transform);

            shorcutUI.name = "Panel_Shortcut_" + instance.Data.PawnName;
            shorcutUI.transform.GetChild((int)PanelShortcutChildren.Image).GetComponent<Image>().sprite = associatedSprite;
            shorcutUI.transform.localScale = Vector3.one;
            shorcutUI.GetComponent<Button>().onClick.AddListener(() => GoToKeeper());

        }

        public void CreateSelectedPanel()
        {
            Sprite associatedSprite = instance.Data.AssociatedSprite;
            selectedPanelUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabSelectedKeeper, GameManager.Instance.Ui.goSelectedKeeperPanel.transform);
            selectedPanelUI.transform.localScale = Vector3.one;
            selectedPanelUI.transform.localPosition = Vector3.zero;

            selectedStatPanelUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabSelectedStatsUIPanel, selectedPanelUI.transform);
            selectedStatPanelUI.transform.localScale = Vector3.one;
            selectedStatPanelUI.transform.localPosition = new Vector3(30, 70, 0);


            selectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.Image).GetComponent<Image>().sprite = associatedSprite;
            selectedActionPointsUI = selectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ActionPoints).gameObject;
            selectedActionPointsUI.transform.localScale = Vector3.one;

            selectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleLeft).GetComponent<Button>().onClick.AddListener(() => GoToPreviousKeeper());
            selectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleRight).GetComponent<Button>().onClick.AddListener(() => GoToNextKeeper());
        }

        public void ShowSelectdePanelUI(bool isShow)
        {
            selectedPanelUI.SetActive(isShow);
        }

        public void GoToKeeper()
        {
            GameManager.Instance.ClearListKeeperSelected();
            GameManager.Instance.ListOfSelectedKeepers.Add(instance);
            IsSelected = true;
        }

        // TODO etre plus malin que ça
        public void GoToPreviousKeeper()
        {
            GameManager.Instance.ClearListKeeperSelected();
            int indice = -1;
            for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
            {
                PawnInstance pi = GameManager.Instance.AllKeepersList[i];
                if (pi == instance)
                {
                    indice = i;
                }
            }
            if (indice != -1)
            {
                int indicePreviousKeeper = indice - 1;
                if (indicePreviousKeeper < 0)
                {
                    indicePreviousKeeper = GameManager.Instance.AllKeepersList.Count -1;
                }

                PawnInstance nextKeeper = GameManager.Instance.AllKeepersList[indicePreviousKeeper];
                GameManager.Instance.ListOfSelectedKeepers.Add(nextKeeper);
                nextKeeper.GetComponent<Keeper>().IsSelected = true;
            }
        }

        // TODO etre plus malin que ça
        public void GoToNextKeeper()
        {
            GameManager.Instance.ClearListKeeperSelected();
            int indice = -1;
            for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
            {
                PawnInstance pi = GameManager.Instance.AllKeepersList[i];
                if ( pi == instance)
                {
                    indice = i;
                }
            }

            if( indice != -1)
            {
                int indiceNextKeeper = indice + 1;
                if (indiceNextKeeper > GameManager.Instance.AllKeepersList.Count -1)
                {
                    indiceNextKeeper = 0;
                }

                PawnInstance nextKeeper = GameManager.Instance.AllKeepersList[indiceNextKeeper];
                GameManager.Instance.ListOfSelectedKeepers.Add(nextKeeper);
                nextKeeper.GetComponent<Keeper>().IsSelected = true;
            }
        }

        public void UpdateActionPoint(int actionPoint)
        {
            selectedActionPointsUI.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = actionPoint.ToString();
            selectedActionPointsUI.gameObject.GetComponentInChildren<Image>().fillAmount = actionPoint / maxActionPoints; ;
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

                feedbackSelection.SetActive(value);
                if (agent != null)
                    agent.avoidancePriority = value == true ? 80 : 50;

                if (isSelected == true)
                {
                    GameManager.Instance.CameraManager.UpdateCameraPosition(instance);
                }
                ShowSelectdePanelUI(isSelected);
                GameManager.Instance.Ui.HideInventoryPanels();
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

        public bool IsSelectedInMenu
        {
            get
            {
                return isSelectedInMenu;
            }

            set
            {
                isSelectedInMenu = value;
            }
        }
        #endregion
    }
}
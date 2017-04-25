using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

namespace Behaviour
{
    public class Keeper : MonoBehaviour
    {
        PawnInstance instance;

        [System.Serializable]
        public class KeeperData : ComponentData
        {
            [SerializeField]
            int minMoralBuff;
            [SerializeField]
            int maxMoralBuff;
            [SerializeField]    
            int maxActionPoint;

            public int MinMoralBuff
            {
                get
                {
                    return minMoralBuff;
                }

                set
                {
                    minMoralBuff = value;
                }
            }

            public int MaxMoralBuff
            {
                get
                {
                    return maxMoralBuff;
                }

                set
                {
                    maxMoralBuff = value;
                }
            }

            public int MaxActionPoint
            {
                get
                {
                    return maxActionPoint;
                }

                set
                {
                    maxActionPoint = value;
                }
            }

            public KeeperData(int _minMoralBuff = -10, int _maxMoralBuff = 20, int _maxActionPoint = 3)
            {
                minMoralBuff = _minMoralBuff;
                maxMoralBuff = _maxMoralBuff;
                maxActionPoint = _maxActionPoint;
            }

        }

        // Actions
        [Header("Actions")]
        private int actionPoints;

        public KeeperData Data;

        [SerializeField]
        private GameObject feedbackSelection;
        private bool isSelected = false;


        // TODO remove
        [SerializeField]
        [System.Obsolete("remove this with old menu")]
        private bool isSelectedInMenu = false;
        NavMeshAgent agent;

        private List<GameObject> goListCharacterFollowing = new List<GameObject>();

        private ItemContainer[] equipements;

        // UI
        private GameObject shorcutUI;
        private GameObject selectedPanelUI;
        private GameObject selectedStatPanelUI;
        private GameObject selectedActionPointsUI;
        private GameObject selectedEquipementUI;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
            equipements = new ItemContainer[3];
        }

        void Start()
        {
            if (instance.Data.Behaviours[(int)BehavioursEnum.CanSpeak])
                instance.Interactions.Add(new Interaction(MoralBuff), 1, "Moral", GameManager.Instance.SpriteUtils.spriteMoral);

            agent = GetComponent<NavMeshAgent>();

            actionPoints = MaxActionPoints;
        }

        void OnDestroy()
        {
            Destroy(shorcutUI);
            Destroy(selectedPanelUI);
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
            Keeper from = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>();
            if (from.ActionPoints >= costAction)
            {
                from.ActionPoints -= (short)costAction;
                short amountMoralBuff = (short)Random.Range(Data.MinMoralBuff, Data.MaxMoralBuff);
                GetComponent<MentalHealthHandler>().CurrentMentalHealth += amountMoralBuff;
                //GameManager.Instance.Ui.MoralBuffActionTextAnimation(amountMoralBuff);
                GetComponent<PawnInstance>().AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteMoralBuff, amountMoralBuff);
            }
            else
            {
                GameManager.Instance.Ui.ZeroActionTextAnimation(from);
            }
        }
        #endregion

        #region UI
        public void InitUI()
        {
            CreateShortcutKeeperUI();
            CreateSelectedPanel();

            UpdateActionPoint(MaxActionPoints);
        }

        public void CreateShortcutKeeperUI()
        {
            Sprite associatedSprite = instance.Data.AssociatedSpriteForShortcut;
            if (associatedSprite == null) associatedSprite = GameManager.Instance.PrefabUIUtils.tmpShortcutForSwag; // ashley est crée à partir du json est il n'y a pas dans les donnée le lien sur le shortcut panel -> a corriger @Rémi
            ShorcutUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabShorcutCharacter, GameManager.Instance.Ui.goShortcutKeepersPanel.transform);

            ShorcutUI.name = "Panel_Shortcut_" + instance.Data.PawnName;
            ShorcutUI.transform.GetComponent<Image>().sprite = associatedSprite;
            ShorcutUI.transform.localScale = Vector3.one;
            ShorcutUI.GetComponent<Button>().onClick.AddListener(() => GoToKeeper());
        }

        public void CreateSelectedPanel()
        {
            Sprite associatedSprite = instance.Data.AssociatedSprite;
            SelectedPanelUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabSelectedKeeper, GameManager.Instance.PrefabUIUtils.PrefabSelectedKeeper.transform.position, GameManager.Instance.PrefabUIUtils.PrefabSelectedKeeper.transform.rotation);
            SelectedPanelUI.transform.SetParent(GameManager.Instance.Ui.goSelectedKeeperPanel.transform, false);
            SelectedPanelUI.transform.localScale = Vector3.one;

            SelectedStatPanelUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabSelectedStatsUIPanel, GameManager.Instance.PrefabUIUtils.PrefabSelectedStatsUIPanel.transform.position, GameManager.Instance.PrefabUIUtils.PrefabSelectedStatsUIPanel.transform.rotation);
            SelectedStatPanelUI.transform.SetParent(SelectedPanelUI.transform, false);
            SelectedStatPanelUI.transform.localScale = Vector3.one;
            SelectedStatPanelUI.name = "Stats";

            SelectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.Image).GetComponent<Image>().sprite = associatedSprite;
            SelectedActionPointsUI = SelectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ActionPoints).gameObject;
            SelectedActionPointsUI.transform.localScale = Vector3.one;
            SelectedActionPointsUI.name = "Action";

            SelectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleLeft).GetComponent<Button>().onClick.AddListener(() => GoToKeeper(-1));
            SelectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleRight).GetComponent<Button>().onClick.AddListener(() => GoToKeeper(+1));

            selectedEquipementUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabSelectedEquipementUIPanel, GameManager.Instance.PrefabUIUtils.PrefabSelectedEquipementUIPanel.transform.position, GameManager.Instance.PrefabUIUtils.PrefabSelectedEquipementUIPanel.transform.rotation);
            selectedEquipementUI.transform.GetComponent<InventoryOwner>().Owner = gameObject;
            selectedEquipementUI.transform.SetParent(SelectedPanelUI.transform, false);
            selectedEquipementUI.transform.localScale = Vector3.one;
            selectedEquipementUI.name = "Equipment";

            SelectedPanelUI.name = "Keeper_" + instance.Data.PawnName;
        }

        public void UpdateEquipement()
        {
            for (int i = 0; i < equipements.Length; i++)
            {
                GameObject currentSlot = SelectedEquipementUI.transform.GetChild(i).gameObject;
                if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
                {
                    Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
                }
            }

            for (int i = 0; i < equipements.Length; i++)
            {
                GameObject currentSlot = SelectedEquipementUI.transform.GetChild(i).gameObject;
                if (equipements[i] != null && equipements[i].Item != null && equipements[i].Item.Id != null)
                {
                    GameObject go = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabItemUI);
                    go.transform.SetParent(currentSlot.transform);
                    go.GetComponent<ItemInstance>().ItemContainer = equipements[i];
                    go.name = equipements[i].ToString();

                    go.GetComponent<Image>().sprite = equipements[i].Item.InventorySprite;
                    go.transform.localScale = Vector3.one;

                    go.transform.position = currentSlot.transform.position;
                    go.transform.SetAsFirstSibling();


                    go.transform.GetComponentInChildren<Text>().text = "";
                }
            }
        }

        public void ShowSelectedPanelUI(bool isShow)
        {
            SelectedPanelUI.SetActive(isShow);
        }

        public void GoToKeeper()
        {
            GameManager.Instance.ClearListKeeperSelected();
            GameManager.Instance.ListOfSelectedKeepers.Add(instance);
            IsSelected = true;
        }

        public void GoToKeeper(int direction)
        {
            GameManager.Instance.ClearListKeeperSelected();
            int currentKeeperSelectedIndex = GameManager.Instance.AllKeepersList.FindIndex(x => x == instance);

            PawnInstance nextKeeper = null;
            int nbIterations = 1;
            while (nextKeeper == null && nbIterations <= GameManager.Instance.AllKeepersList.Count)
            {
                if ((currentKeeperSelectedIndex + direction * nbIterations) % GameManager.Instance.AllKeepersList.Count < 0)
                {
                    nextKeeper = GameManager.Instance.AllKeepersList[GameManager.Instance.AllKeepersList.Count - 1];
                }
                else
                {
                    nextKeeper = GameManager.Instance.AllKeepersList[(currentKeeperSelectedIndex + direction * nbIterations) % GameManager.Instance.AllKeepersList.Count];
                }

                if (!nextKeeper.GetComponent<Behaviour.Mortal>().IsAlive)
                {
                    nextKeeper = null;
                }
                nbIterations++;
            }

            GameManager.Instance.ListOfSelectedKeepers.Add(nextKeeper);
            nextKeeper.GetComponent<Keeper>().IsSelected = true;

        }

        public void UpdateActionPoint(int actionPoint)
        {
            SelectedActionPointsUI.transform.SetAsLastSibling();
            SelectedActionPointsUI.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = actionPoint.ToString();

            ShorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).GetComponent<Text>().text = actionPoint.ToString();
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
                if (value < actionPoints) GameManager.Instance.Ui.DecreaseActionTextAnimation(this, actionPoints - value);
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

                if (GameManager.Instance.CurrentState == GameState.Normal || GameManager.Instance.CurrentState == GameState.InTuto)
                {
                    feedbackSelection.SetActive(value);

                    if (agent != null)
                        agent.avoidancePriority = value == true ? 80 : 50;

                    if (isSelected == true)
                    {
                        GameManager.Instance.UpdateCameraPosition(instance);
                    }
                    ShowSelectedPanelUI(isSelected);
                    GameManager.Instance.Ui.HideInventoryPanels();
                }
                else if (GameManager.Instance.CurrentState == GameState.InBattle)
                {
                    feedbackSelection.SetActive(false);
                    BattleHandler.DeactivateFeedbackSelection(true, false);
                }

                GameManager.Instance.Ui.ClearActionPanel();
            }

        }

        public int MaxActionPoints
        {
            get
            {
                return Data.MaxActionPoint;
            }
            set
            {
                Data.MaxActionPoint = value;
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

        public GameObject ShorcutUI
        {
            get
            {
                return shorcutUI;
            }

            set
            {
                shorcutUI = value;
            }
        }

        public GameObject SelectedPanelUI
        {
            get
            {
                return selectedPanelUI;
            }

            set
            {
                selectedPanelUI = value;
            }
        }

        public GameObject SelectedActionPointsUI
        {
            get
            {
                return selectedActionPointsUI;
            }

            set
            {
                selectedActionPointsUI = value;
            }
        }

        public GameObject SelectedStatPanelUI
        {
            get
            {
                return selectedStatPanelUI;
            }

            set
            {
                selectedStatPanelUI = value;
            }
        }

        public ItemContainer[] Equipements
        {
            get
            {
                return equipements;
            }

            set
            {
                equipements = value;
            }
        }

        public GameObject SelectedEquipementUI
        {
            get
            {
                return selectedEquipementUI;
            }

            set
            {
                selectedEquipementUI = value;
            }
        }

        public GameObject FeedbackSelection
        {
            get
            {
                return feedbackSelection;
            }
        }
        #endregion
    }
}

public enum PanelShortcutChildren { Image, ActionPoints, HpGauge, HungerGauge, MentalHealthGauge };
public enum PanelSelectedKeeperStatChildren { Image, ButtonCycleRight, ButtonCycleLeft, ActionPoints, StatTrigger };
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Behaviour
{
    public class Keeper : MonoBehaviour
    {
        PawnInstance instance;

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

        [SerializeField]
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
            // J'ai un probleme j'ai besoin que la creation de l'ui de ce mec soit faite avant celle des autres composants
            // Peut que sa sera corrigé avec le truc de Quentin du coup on pourra le déplacer dans le start
            instance = GetComponent<PawnInstance>();

            // Equipement
            equipements = new ItemContainer[3];

            CreateShortcutKeeperUI();
            CreateSelectedPanel();
        }

        void Start()
        {
    
            if (instance.Data.Behaviours[(int)BehavioursEnum.CanSpeak])
                instance.Interactions.Add(new Interaction(MoralBuff), 1, "Moral", GameManager.Instance.SpriteUtils.spriteMoral);

            agent = GetComponent<NavMeshAgent>();

            ShorcutUI.name = "Shortcut" + instance.Data.PawnName;
            SelectedPanelUI.name = "SelectedKeeper" + instance.Data.PawnName;

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
            ShorcutUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabShorcutCharacter, GameManager.Instance.Ui.goShortcutKeepersPanel.transform);

            ShorcutUI.name = "Panel_Shortcut_" + instance.Data.PawnName;
            ShorcutUI.transform.GetChild((int)PanelShortcutChildren.Image).GetComponent<Image>().sprite = associatedSprite;
            ShorcutUI.transform.localScale = Vector3.one;
            ShorcutUI.GetComponent<Button>().onClick.AddListener(() => GoToKeeper());

        }

        public void CreateSelectedPanel()
        {
            Sprite associatedSprite = instance.Data.AssociatedSprite;
            SelectedPanelUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabSelectedKeeper, GameManager.Instance.Ui.goSelectedKeeperPanel.transform);
            SelectedPanelUI.transform.localScale = Vector3.one;
            //SelectedPanelUI.transform.localPosition = Vector3.zero;

            SelectedStatPanelUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabSelectedStatsUIPanel, SelectedPanelUI.transform);
            SelectedStatPanelUI.transform.localScale = Vector3.one;
            //SelectedStatPanelUI.transform.localPosition = new Vector3(30, 70, 0);


            SelectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.Image).GetComponent<Image>().sprite = associatedSprite;
            SelectedActionPointsUI = SelectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ActionPoints).gameObject;
            SelectedActionPointsUI.transform.localScale = Vector3.one;


            SelectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleLeft).GetComponent<Button>().onClick.AddListener(() => GoToKeeper(-1));
            SelectedStatPanelUI.transform.GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleRight).GetComponent<Button>().onClick.AddListener(() => GoToKeeper(+1));


            selectedEquipementUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabSelectedEquipementUIPanel, SelectedPanelUI.transform);
            selectedEquipementUI.transform.GetComponent<InventoryOwner>().Owner = gameObject;
            selectedEquipementUI.transform.localPosition = Vector3.zero;
            selectedEquipementUI.transform.localScale = Vector3.one;
        }

        public void UpdateEquipement()
        {

            for (int i = 0; i < equipements.Length; i++)
            {
                GameObject currentSlot = SelectedEquipementUI.transform.GetChild(1).GetChild(i).gameObject;
                if (currentSlot.GetComponentInChildren<ItemInstance>() != null)
                {
                    Destroy(currentSlot.GetComponentInChildren<ItemInstance>().gameObject);
                }
            }

            for (int i = 0; i < equipements.Length; i++)
            {
                GameObject currentSlot = SelectedEquipementUI.transform.GetChild(1).GetChild(i).gameObject;
                if (equipements[i] != null && equipements[i].Item != null && equipements[i].Item.Id != null)
                {
                    GameObject go = Instantiate(GameManager.Instance.PrefabUtils.PrefabItemUI);
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

        public void ShowSelectdePanelUI(bool isShow)
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
            SelectedActionPointsUI.transform.GetChild(0).gameObject.GetComponentInChildren<Text>().text = actionPoint.ToString();
            SelectedActionPointsUI.gameObject.GetComponentInChildren<Image>().fillAmount = actionPoint / maxActionPoints; ;
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
                GameManager.Instance.Ui.ClearActionPanel();
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
        #endregion
    }
}

public enum PanelShortcutChildren { Image, ActionPoints, HpGauge, HungerGauge, MentalHealthGauge };
public enum PanelSelectedKeeperStatChildren { Image, ButtonCycleRight, ButtonCycleLeft, ActionPoints, StatTrigger };
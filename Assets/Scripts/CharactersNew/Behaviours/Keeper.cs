using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

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
        [SerializeField]
        private short maxActionPoints = 3;
        [SerializeField]
        private short actionPoints;

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
            actionPoints = maxActionPoints;
        }

        #region Interactions

        public void MoralBuff(int _i = 0)
        {
            if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
            {
                int costAction = instance.Interactions.Get("Moral").costAction;
                if (GameManager.Instance.ListOfSelectedKeepers[0].ActionPoints >= costAction)
                {
                    GameManager.Instance.ListOfSelectedKeepers[0].ActionPoints -= (short)costAction;
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

        #region Accessors

        public short ActionPoints
        {
            get
            {
                return actionPoints;
            }

            set
            {
                actionPoints = value;
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

        public short MaxActionPoints
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
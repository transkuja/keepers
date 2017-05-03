using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Behaviour
{
    public class Escortable : MonoBehaviour
    {
        PawnInstance instance;

        // UI
        private GameObject shorcutUI;

        private bool isEscorted = false;
        public Keeper escort;

        // Feed
        private int feedingSlotsCount = 2;

        // @ Seb: j'ai mis ça là mais à mon avis il va falloir faire qq chose pour récupérer le panel, vois avec Rémi
        public GameObject prisonerFeedingPanel;
        private GameObject myIconForShortcut;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            if (GetComponent<HungerHandler>() != null && GetComponent<MentalHealthHandler>() != null)
                instance.Interactions.Add(new Interaction(InitFeeding), 1, "Feed", GameManager.Instance.SpriteUtils.spriteHarvest);

            if (isEscorted)
                instance.Interactions.Add(new Interaction(UnEscort), 0, "Unescort", GameManager.Instance.SpriteUtils.spriteUnescort);
            else
                instance.Interactions.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.SpriteUtils.spriteEscort);
        }

        void OnDestroy()
        {
            Destroy(shorcutUI);
        }

        #region Interactions
        public void Escort(int _i = 0)
        {
            if (escort != null)
                escort.GoListCharacterFollowing.Remove(gameObject);

            escort = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Keeper>();
            escort.GetComponent<Keeper>().GoListCharacterFollowing.Add(gameObject);

            ActivateIconNearEscort();

            IsEscorted = true;
        }

        public void ActivateIconNearEscort()
        {
            if (myIconForShortcut == null)
            {
                myIconForShortcut = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI);
                myIconForShortcut.GetComponent<RectTransform>().sizeDelta = new Vector3(50.0f, 50.0f, 0.0f);
                myIconForShortcut.GetComponent<Image>().sprite = GetComponent<PawnInstance>().Data.AssociatedSpriteForShortcut;
            }

            myIconForShortcut.gameObject.SetActive(true);

            myIconForShortcut.transform.SetParent(escort.ShorcutUI.transform, false);
            myIconForShortcut.transform.localPosition = new Vector3(-50.0f, -70.0f, 0.0f);
            myIconForShortcut.transform.localScale = Vector3.one;
        }

        public void UnEscort(int _i = 0)
        {
            escort.GoListCharacterFollowing.Remove(gameObject);
            myIconForShortcut.gameObject.SetActive(false);
            escort = null;
            IsEscorted = false;
            ActivateEscortAction();
        }

        public void InitFeeding(int _i = 0)
        {
            Inventory inv = gameObject.AddComponent<Inventory>();

            inv.Data.NbSlot = feedingSlotsCount;
            inv.InitUI();

            prisonerFeedingPanel.SetActive(true);
        }

        #endregion

        #region UI
        public void InitUI()
        {
            CreateShortcutEscortUI();
        }

        public void CreateShortcutEscortUI()
        {
            Sprite associatedSprite = instance.Data.AssociatedSprite;
            ShorcutUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabShorcutCharacter, GameManager.Instance.Ui.goShortcutKeepersPanel.transform);
            ShorcutUI.transform.localScale = Vector3.one;
            ShorcutUI.transform.localPosition = Vector3.zero;
            ShorcutUI.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;

            // ? ? 
            ShorcutUI.transform.SetAsFirstSibling();
            // TMP Destroy action points irrelevent
            for (int i =0; i < ShorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).childCount; i++)
            {
                Destroy(ShorcutUI.transform.GetChild((int)PanelShortcutChildren.ActionPoints).transform.GetChild(i).gameObject);
            }

            // TMP

            ShorcutUI.GetComponent<Button>().onClick.AddListener(() => GoToEscorted());
        }

        #endregion

        public void GoToEscorted()
        {
            GameManager.Instance.UpdateCameraPosition(instance);
        }

        public bool IsEscorted
        {
            get
            {
                return isEscorted;
            }

            set
            {
                isEscorted = value;          
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

        public int FeedingSlotsCount
        {
            get
            {
                return feedingSlotsCount;
            }

            set
            {
                feedingSlotsCount = value;
            }
        }

        public void UpdateEscortableInteractions()
        {
            if (isEscorted)
            {
                if (GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Keeper>() == escort)
                {
                    ActivateUnescortAction();
                }
                else
                {
                    ActivateEscortAction();
                }
            }
        }

        private void ActivateEscortAction()
        {
            if (instance.Interactions.Get("Escort") == null)
            {
                instance.Interactions.Add(new Interaction(Escort), 0, "Escort", GameManager.Instance.SpriteUtils.spriteEscort);
                instance.Interactions.Remove("Unescort");
            }
        }

        private void ActivateUnescortAction()
        {
            if (instance.Interactions.Get("Unescort") == null)
            {
                instance.Interactions.Add(new Interaction(UnEscort), 0, "Unescort", GameManager.Instance.SpriteUtils.spriteUnescort, false);
                instance.Interactions.Remove("Escort");
            }
        }


    }
}
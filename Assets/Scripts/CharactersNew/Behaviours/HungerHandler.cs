using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour
{
    public class HungerHandler : MonoBehaviour
    {
        [System.Serializable]
        public class HungerHandlerData : ComponentData
        {
            [SerializeField]
            int maxHunger;

            public HungerHandlerData(int _maxHunger = 0)
            {
                maxHunger = _maxHunger;
            }

            public int MaxHunger
            {
                get
                {
                    return maxHunger;
                }

                set
                {
                    maxHunger = value;
                }
            }
        }

        PawnInstance instance;

        [SerializeField]
        HungerHandlerData data;
        //int maxHunger;  // OLD WITHOUT DATA
        int currentHunger;
        bool isStarving = false;

        // UI
        private GameObject selectedHungerUI;
        private GameObject shortcutHungerUI;


        private Color green;
        private Color yellow;
        private Color red;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
            green = new Color32(0x00, 0xFF, 0x6B, 0x92);
            red = new Color32(0xFF, 0x00, 0x00, 0x92);
            yellow = new Color32(0xD1, 0xFF, 0x00, 0x92);
        }

        void Start()
        { 
            currentHunger = data.MaxHunger;
        }

        #region UI
        public void InitUI()
        {
            CreateShortcutHungerPanel();
            ShortcutHungerUI.name = "Hunger";

            if (instance.GetComponent<Escortable>() != null)
            {
                ShortcutHungerUI.name = "Hunger_test";
                ShortcutHungerUI.transform.SetParent(instance.GetComponent<Escortable>().ShorcutUI.transform);
                ShortcutHungerUI.transform.localScale = Vector3.one;
                ShortcutHungerUI.transform.localPosition = Vector3.zero;
                ShortcutHungerUI.transform.SetAsFirstSibling();
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                CreateSelectedHungerPanel();
                SelectedHungerUI.name = "Hunger";
                SelectedHungerUI.transform.SetParent(instance.GetComponent<Keeper>().SelectedStatPanelUI.transform, false);
                SelectedHungerUI.transform.localScale = Vector3.one;
                SelectedHungerUI.transform.SetAsFirstSibling();

                ShortcutHungerUI.transform.SetParent(instance.GetComponent<Keeper>().ShorcutUI.transform);
                ShortcutHungerUI.transform.localScale = Vector3.one;
                ShortcutHungerUI.transform.localPosition = Vector3.zero;
                ShortcutHungerUI.transform.SetAsFirstSibling();
            }
            UpdateHungerPanel(Data.MaxHunger);
        }

        public void CreateSelectedHungerPanel()
        {
            SelectedHungerUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabHungerUI);
        }

        public void CreateShortcutHungerPanel()
        {
            ShortcutHungerUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabShortcutHungerUI);
        }

        public void UpdateHungerPanel(int hunger)
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                if (hunger < (Data.MaxHunger / 3.0f))
                    ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                else if (hunger < (2 * Data.MaxHunger / 3.0f))
                    ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                else
                    ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)data.MaxHunger;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                if (hunger < (Data.MaxHunger / 3.0f))
                {
                    ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                    SelectedHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                }
                else if (hunger < (2 * Data.MaxHunger / 3.0f))
                {
                    ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                    SelectedHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                }
                else
                {
                    ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                    SelectedHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                }
                SelectedHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)data.MaxHunger;
                ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)data.MaxHunger;
            }
        }
        #endregion

        public int CurrentHunger
        {
            get { return currentHunger; }
            set
            {
                int diffHunger = value - currentHunger;
                instance.AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteHunger, diffHunger);
                currentHunger = value;
                if (currentHunger < 0)
                {
                    currentHunger = 0;
                    IsStarving = true;
                }
                else if (currentHunger > data.MaxHunger)
                {
                    currentHunger = data.MaxHunger;
                    IsStarving = false;
                }
                else
                {
                    IsStarving = false;
                }
                UpdateHungerPanel(currentHunger);
                if (GetComponent<Prisoner>() != null)
                    GetComponent<Inventory>().UpdateInventories();

                if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto && TutoManager.s_instance.PlayingSequence == null)
                {
                    if (GameManager.Instance.CurrentState != GameState.InBattle)
                    {
                        if (GetComponent<Escortable>() != null && TutoManager.s_instance.GetComponent<SeqAshleyLowHunger>().AlreadyPlayed == false)
                        {
                            if (currentHunger < data.MaxHunger / 2.0f && diffHunger < 0)
                                TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqAshleyLowHunger>());
                        }

                        if (GetComponent<Keeper>() != null && TutoManager.s_instance.GetComponent<SeqLowHunger>().AlreadyPlayed == false)
                        {
                            if (currentHunger < data.MaxHunger / 3.0f && diffHunger < 0)
                                TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqLowHunger>());
                        }
                    }
                }
            }
        }

        public HungerHandlerData Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public bool IsStarving
        {
            get
            {
                return isStarving;
            }

            set
            {
                isStarving = value;
            }
        }

        public GameObject SelectedHungerUI
        {
            get
            {
                return selectedHungerUI;
            }

            set
            {
                selectedHungerUI = value;
            }
        }

        public GameObject ShortcutHungerUI
        {
            get
            {
                return shortcutHungerUI;
            }

            set
            {
                shortcutHungerUI = value;
            }
        }
    }
}


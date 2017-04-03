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

        void Start()
        {
            instance = GetComponent<PawnInstance>();

            InitUI();
   
            CurrentHunger = data.MaxHunger;
        }

        #region UI
        public void InitUI()
        {
            CreateShortcutHungerPanel();
            ShortcutHungerUI.name = "Hunger";

            if (instance.GetComponent<Escortable>() != null)
            {
                ShortcutHungerUI.transform.SetParent(instance.GetComponent<Escortable>().ShorcutUI.transform);
                ShortcutHungerUI.transform.localScale = Vector3.one;
                ShortcutHungerUI.transform.localPosition = Vector3.zero;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                CreateSelectedHungerPanel();
                SelectedHungerUI.name = "Hunger";
                SelectedHungerUI.transform.SetParent(instance.GetComponent<Keeper>().SelectedStatPanelUI.transform);
                SelectedHungerUI.transform.localScale = Vector3.one;
                SelectedHungerUI.transform.localPosition = new Vector3(100, 125, 0);

                ShortcutHungerUI.transform.SetParent(instance.GetComponent<Keeper>().ShorcutUI.transform);
                ShortcutHungerUI.transform.localScale = Vector3.one;
                ShortcutHungerUI.transform.localPosition = Vector3.zero;
            }
        }

        public void CreateSelectedHungerPanel()
        {
            SelectedHungerUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabHungerUI);
        }

        public void CreateShortcutHungerPanel()
        {
            ShortcutHungerUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabHungerUI);
        }

        public void UpdateHungerPanel(int hunger)
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)data.MaxHunger;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
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


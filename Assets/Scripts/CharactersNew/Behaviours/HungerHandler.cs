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
        public GameObject selectedHungerUI;
        public GameObject shortcutHungerUI;

        void Start()
        {
            instance = GetComponent<PawnInstance>();

            CreateShortcutHungerPanel();
            shortcutHungerUI.name = "Hunger";

            if (instance.GetComponent<Escortable>() != null)
            {
                shortcutHungerUI.transform.SetParent(instance.GetComponent<Escortable>().shorcutUI.transform);
                shortcutHungerUI.transform.localScale = Vector3.one;
                shortcutHungerUI.transform.localPosition = Vector3.zero;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                CreateSelectedHungerPanel();
                selectedHungerUI.name = "Hunger";
                selectedHungerUI.transform.SetParent(instance.GetComponent<Keeper>().selectedStatPanelUI.transform);
                selectedHungerUI.transform.localScale = Vector3.one;
                selectedHungerUI.transform.localPosition = new Vector3(100, 125, 0);

                shortcutHungerUI.transform.SetParent(instance.GetComponent<Keeper>().shorcutUI.transform);
                shortcutHungerUI.transform.localScale = Vector3.one;
                shortcutHungerUI.transform.localPosition = Vector3.zero;
            }

            CurrentHunger = data.MaxHunger;
        }

        #region UI
        public void CreateSelectedHungerPanel()
        {
            selectedHungerUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabHungerUI);
        }

        public void CreateShortcutHungerPanel()
        {
            shortcutHungerUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabHungerUI);
        }

        public void UpdateHungerPanel(int hunger)
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                shortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)data.MaxHunger;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                selectedHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)data.MaxHunger;
                shortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)data.MaxHunger;
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
    }
}


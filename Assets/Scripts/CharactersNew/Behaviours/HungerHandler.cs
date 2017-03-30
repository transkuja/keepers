using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour
{
    public class HungerHandler : MonoBehaviour
    {
        PawnInstance instance;

        [SerializeField]
        int maxHunger;
        int currentHunger;
        bool isStarving = false;

        // UI
        public GameObject selectedHungerUI;
        public GameObject shortcutHungerUI;


        void Awake()
        {
            instance = GetComponent<PawnInstance>();

            CreateShortcutHungerPanel();
            shortcutHungerUI.name = "Hunger";
            if (instance.GetComponent<Keeper>() != null)
            {
                CreateSelectedHungerPanel();
                selectedHungerUI.name = "Hunger";
            }

        }


        void Start()
        {
            instance = GetComponent<PawnInstance>();

            if (instance.GetComponent<Escortable>() != null)
            {
                //selectedHPUI.transform.SetParent(instance.GetComponent<Escortable>().selectedStatPanelUI.transform);
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                selectedHungerUI.transform.SetParent(instance.GetComponent<Keeper>().selectedStatPanelUI.transform);
                selectedHungerUI.transform.localScale = Vector3.one;
                selectedHungerUI.transform.localPosition = new Vector3(100, 125, 0);

                shortcutHungerUI.transform.SetParent(instance.GetComponent<Keeper>().shorcutUI.transform);
                shortcutHungerUI.transform.localScale = Vector3.one;
                shortcutHungerUI.transform.localPosition = Vector3.zero;
            }

            CurrentHunger = maxHunger;
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
                shortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)maxHunger;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                selectedHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)maxHunger;
                shortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)maxHunger;
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
                    isStarving = true;
                }
                else if (currentHunger > maxHunger)
                {
                    currentHunger = maxHunger;
                    isStarving = false;
                }
                else
                {
                    isStarving = false;
                }
                UpdateHungerPanel(currentHunger);
            }
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
}
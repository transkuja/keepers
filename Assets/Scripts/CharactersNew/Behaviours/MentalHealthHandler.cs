using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour
{
    public class MentalHealthHandler : MonoBehaviour
    {
        [System.Serializable]
        public class MentalHealthHandlerData : ComponentData
        {
            [SerializeField]
            int maxMentalHealth;

            public MentalHealthHandlerData(int _maxMentalHealth = 0)
            {
                maxMentalHealth = _maxMentalHealth;
            }

            public int MaxMentalHealth
            {
                get
                {
                    return maxMentalHealth;
                }

                set
                {
                    maxMentalHealth = value;
                }
            }
        } 
    
        PawnInstance instance;

        [SerializeField]
        MentalHealthHandlerData data;

        int currentMentalHealth;
        bool isLowMentalHealthBuffApplied = false;
        bool isDepressed = false;

        // UI
        private GameObject selectedMentalHealthUI;
        private GameObject shortcutMentalHealthUI;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            currentMentalHealth = data.MaxMentalHealth;
        }

        #region UI
        public void InitUI()
        {
            CreateShortcutMentalHealthPanel();
            ShortcutMentalHealthUI.name = "MentalHealth";


            if (instance.GetComponent<Escortable>() != null)
            {
                //selectedHPUI.transform.SetParent(instance.GetComponent<Escortable>().selectedStatPanelUI.transform);
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                CreateSelectedMentalHealthPanel();
                selectedMentalHealthUI.name = "MentalHealth";
                selectedMentalHealthUI.transform.SetParent(instance.GetComponent<Keeper>().SelectedStatPanelUI.transform);
                selectedMentalHealthUI.transform.localScale = Vector3.one;
                selectedMentalHealthUI.transform.localPosition = new Vector3(230, 100, 0);

                ShortcutMentalHealthUI.transform.SetParent(instance.GetComponent<Keeper>().ShorcutUI.transform);
                ShortcutMentalHealthUI.transform.localScale = Vector3.one;
                ShortcutMentalHealthUI.transform.localPosition = Vector3.zero;
            }

            UpdateMentalHealthPanel(Data.MaxMentalHealth);
        }

        public void CreateSelectedMentalHealthPanel()
        {
            selectedMentalHealthUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabMentalHealthUI);
        }

        public void CreateShortcutMentalHealthPanel()
        {
            ShortcutMentalHealthUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabMentalHealthUI);
        }

        public void UpdateMentalHealthPanel(int mentalHealth)
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)mentalHealth / (float)data.MaxMentalHealth;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                selectedMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)mentalHealth / (float)data.MaxMentalHealth;
                ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)mentalHealth / (float)data.MaxMentalHealth;
            }
        }
        #endregion

        #region Accessors
        #endregion
        public int CurrentMentalHealth
        {
            get { return currentMentalHealth; }
            set
            {
                currentMentalHealth = value;
                if (currentMentalHealth < 0)
                {
                    currentMentalHealth = 0;
                    isDepressed = true;
                }
                else if (currentMentalHealth > data.MaxMentalHealth)
                {
                    currentMentalHealth = data.MaxMentalHealth;
                    isDepressed = false;
                }
                else
                {
                    isDepressed = false;
                }
                UpdateMentalHealthPanel(currentMentalHealth);
            }
        }

        public MentalHealthHandlerData Data
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

        public bool IsLowMentalHealthBuffApplied
        {
            get
            {
                return isLowMentalHealthBuffApplied;
            }

            set
            {
                isLowMentalHealthBuffApplied = value;
            }
        }

        public bool IsDepressed
        {
            get
            {
                return isDepressed;
            }
        }

        public GameObject ShortcutMentalHealthUI
        {
            get
            {
                return shortcutMentalHealthUI;
            }

            set
            {
                shortcutMentalHealthUI = value;
            }
        }
    }
}
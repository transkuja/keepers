using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour
{
    public class MentalHealthHandler : MonoBehaviour
    {
        public class MentalHealthHandlerData : ComponentData
        {
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
        //int maxMentalHealth;  OLD WITHOUT DATA
        int currentMentalHealth;
        bool isLowMentalHealthBuffApplied = false;

        // UI
        public GameObject selectedMentalHealthUI;
        public GameObject shortcutMentalHealthUI;

        bool isDepressed = false;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();

            CreateShortcutMentalHealthPanel();
            shortcutMentalHealthUI.name = "MentalHealth";
            if (instance.GetComponent<Keeper>() != null)
            {
                CreateSelectedMentalHealthPanel();
                selectedMentalHealthUI.name = "MentalHealth";
            }

        }

        void Start()
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                //selectedHPUI.transform.SetParent(instance.GetComponent<Escortable>().selectedStatPanelUI.transform);
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                selectedMentalHealthUI.transform.SetParent(instance.GetComponent<Keeper>().selectedStatPanelUI.transform);
                selectedMentalHealthUI.transform.localScale = Vector3.one;
                selectedMentalHealthUI.transform.localPosition = new Vector3(230, 100, 0);

                shortcutMentalHealthUI.transform.SetParent(instance.GetComponent<Keeper>().shorcutUI.transform);
                shortcutMentalHealthUI.transform.localScale = Vector3.one;
                shortcutMentalHealthUI.transform.localPosition = Vector3.zero;
            }

            CurrentMentalHealth = data.MaxMentalHealth;
        }

        #region UI
        public void CreateSelectedMentalHealthPanel()
        {
            selectedMentalHealthUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabMentalHealthUI);
        }

        public void CreateShortcutMentalHealthPanel()
        {
            shortcutMentalHealthUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabMentalHealthUI);
        }

        public void UpdateMentalHealthPanel(int mentalHealth)
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                shortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)mentalHealth / (float)data.MaxMentalHealth;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                selectedMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)mentalHealth / (float)data.MaxMentalHealth;
                shortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)mentalHealth / (float)data.MaxMentalHealth;
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
    }
}
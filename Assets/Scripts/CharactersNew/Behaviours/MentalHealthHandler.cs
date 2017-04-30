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


        private Color green;
        private Color red;
        private Color yellow;

        // UI
        private GameObject selectedMentalHealthUI;
        private GameObject shortcutMentalHealthUI;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
            green = new Color32(0x00, 0xFF, 0x6B, 0x92);
            red = new Color32(0xFF, 0x00, 0x00, 0x92);
            yellow = new Color32(0xD1, 0xFF, 0x00, 0x92);
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
                selectedMentalHealthUI.transform.SetParent(instance.GetComponent<Keeper>().SelectedStatPanelUI.transform, false);
                selectedMentalHealthUI.transform.localScale = Vector3.one;


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
            ShortcutMentalHealthUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabShortcutMentalHealthUI);
        }

        public void UpdateMentalHealthPanel(int mentalHealth)
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                if (mentalHealth < (Data.MaxMentalHealth / 3.0f))
                    ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                else if (mentalHealth < (2 * Data.MaxMentalHealth / 3.0f))
                {
                    ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                }
                else
                    ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)mentalHealth / (float)data.MaxMentalHealth;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                if (mentalHealth < (Data.MaxMentalHealth / 3.0f))
                {
                    ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                    selectedMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                    selectedMentalHealthUI.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteMentalDown;
                }
                else if (mentalHealth < (2*Data.MaxMentalHealth / 3.0f))
                {
                    ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                    selectedMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                    selectedMentalHealthUI.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteMentalNormal;
                }

                else
                {
                    ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                    selectedMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                    selectedMentalHealthUI.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteMentalUp;
                }
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
                int feedbackAmount = value - currentMentalHealth;
                if (feedbackAmount < 0)
                    instance.AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteMoralBuff, value - currentMentalHealth);
                else
                    instance.AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteMoralDebuff, value - currentMentalHealth);

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

                if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto)
                {
                    if (TutoManager.s_instance.PlayingSequence == null)
                    {
                        if (TutoManager.s_instance.GetComponent<SeqMoraleExplained>().AlreadyPlayed == false)
                        {
                            TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqMoraleExplained>());
                        }
                        else if (TutoManager.s_instance.GetComponent<SeqLowMorale>().AlreadyPlayed == false)
                        {
                            if (currentMentalHealth < data.MaxMentalHealth / 4.0f)
                                TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqLowMorale>());
                        }
                    }                
                }
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
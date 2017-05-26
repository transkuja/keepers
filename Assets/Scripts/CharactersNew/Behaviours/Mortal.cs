
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour
{
    public class Mortal : MonoBehaviour
    {
        [System.Serializable]
        public class MortalData : ComponentData
        {
            [SerializeField]
            int maxHp;

            public MortalData(int _maxHp = 0)
            {
                maxHp = _maxHp;
            }

            public int MaxHp
            {
                get
                {
                    return maxHp;
                }

                set
                {
                    maxHp = value;
                }
            }
        }

        PawnInstance instance;

        [SerializeField]
        private MortalData data;

        [SerializeField]
        int currentHp;
        bool isAlive;

        private Color green;
        private Color red;
        private Color yellow;

        [SerializeField]
        private ParticleSystem deathParticles;

        // UI
        private GameObject selectedHPUI;
        private GameObject shortcutHPUI;

        void Awake()
        {

            instance = GetComponent<PawnInstance>();
            green = new Color32(0x00, 0xFF, 0x6B, 0x92);
            red = new Color32(0xFF, 0x00, 0x00, 0x92);
            yellow = new Color32(0xD1, 0xFF, 0x00, 0x92);
        }

        void Start()
        {
            currentHp = data.MaxHp;
            isAlive = true;
            if (GetComponent<Monster>() == null)
                GetComponent<Interactable>().Interactions.Add(new Interaction(Heal), 1, "Heal", GameManager.Instance.SpriteUtils.spriteHeal);
        }

        public void Die()
        {
            if (GetComponent<Keeper>() != null || GetComponent<Monster>() != null)
            {
                Keeper keeper = GetComponent<Keeper>();
                //Monster monster = GetComponent<Monster>();

                Debug.Log("Blaeuurgh... *dead*");
                PawnInstance pawnInstance = GetComponent<PawnInstance>();

                if(GameManager.Instance.Ui.tooltipAction!= null)
                {
                    // TMP Remi
                    GameManager.Instance.Ui.GoActionPanelQ.transform.parent.SetParent(GameManager.Instance.Ui.transform);
                    GameManager.Instance.Ui.tooltipAction.SetActive(false);
                }


                // Remove reference from tiles
                if (keeper != null)
                {
                    if (TileManager.Instance.KeepersOnTile.ContainsKey(keeper.GetComponent<PawnInstance>().CurrentTile))
                    {
                        foreach (PawnInstance k in TileManager.Instance.KeepersOnTile[keeper.GetComponent<PawnInstance>().CurrentTile])
                        {
                            if(k.GetComponent<MentalHealthHandler>() != null)
                                k.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 30;
                        }
                    }

                    TileManager.Instance.RemoveKilledKeeper(pawnInstance);
                    GameManager.Instance.ClearListKeeperSelected();

                    keeper.IsSelected = false;
                    // Drop items
                    if (!GetComponent<Inventory>().IsEmpty())
                    {
                        ItemManager.AddItemOnTheGround(pawnInstance.CurrentTile, transform, GetComponent<Inventory>().Items);
                    }
                }
                else
                    TileManager.Instance.RemoveDefeatedMonster(pawnInstance);

                // Death operations
                // TODO @Rémi, il me faut de quoi mettre a jour le shortcut panel pour afficher l'icone de mort

                GlowController.UnregisterObject(GetComponent<GlowObjectCmd>());
                GetComponent<AnimatedPawn>().Anim.SetTrigger("triggerDeath");

                // Try to fix glow bug
                Destroy(GetComponent<GlowObjectCmd>());

                if (keeper != null)
                {
                    keeper.ShowSelectedPanelUI(false);
                    if (EventManager.OnKeeperDie != null)
                        EventManager.OnKeeperDie(GetComponent<Keeper>());

                    // Deactivate pawn
                    DeactivatePawn();
                }
                else
                {
                    if (GameManager.Instance.CurrentState != GameState.InBattle)
                        Destroy(gameObject, 0.1f);

                }

     
              
            }
            else if (GetComponent<Prisoner>() != null)
            {
                Debug.Log("Ashley is dead");
            }
            GameManager.Instance.CheckGameState();
        }

        private void DeactivatePawn()
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
                c.enabled = false;
            enabled = false;

            // Deactivate gameobject after a few seconds
            StartCoroutine(DeactivateGameObject());
        }

        IEnumerator DeactivateGameObject()
        {
            yield return new WaitForSeconds(5.0f);
            gameObject.SetActive(false);
            foreach (Collider c in GetComponentsInChildren<Collider>())
                c.enabled = true;
            enabled = true;
        }

        #region UI
        public void InitUI()
        {
            CreateShortcutHPPanel();
            ShortcutHPUI.name = "Mortal";

            if (instance.GetComponent<Escortable>() != null)
            {
                ShortcutHPUI.transform.SetParent(instance.GetComponent<Escortable>().ShorcutUI.transform);
                ShortcutHPUI.transform.localScale = Vector3.one;
                ShortcutHPUI.transform.localPosition = Vector3.zero;
                ShortcutHPUI.transform.SetAsFirstSibling();
            }
            else if (instance.GetComponent<Keeper>() != null)
            {

                CreateSelectedHPPanel();
                SelectedHPUI.name = "Mortal";
                SelectedHPUI.transform.SetParent(instance.GetComponent<Keeper>().SelectedStatPanelUI.transform, false);
                SelectedHPUI.transform.localScale = Vector3.one;
                SelectedHPUI.transform.SetAsFirstSibling();
                //SelectedHPUI.transform.localPosition = Vector3.zero;

                ShortcutHPUI.transform.SetParent(instance.GetComponent<Keeper>().ShorcutUI.transform);
                ShortcutHPUI.transform.localScale = Vector3.one;
                ShortcutHPUI.transform.localPosition = Vector3.zero;
                ShortcutHPUI.transform.SetAsFirstSibling();
            }

            UpdateHPPanel(MaxHp);
        }

        public void CreateSelectedHPPanel()
        {
            SelectedHPUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabHPUI);
        }

        public void CreateShortcutHPPanel()
        {
            ShortcutHPUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabShortcutHPUI);
        }

        public void UpdateHPPanel(int currentHp)
        {
            if (instance.GetComponent<Escortable>() != null)
            {     
                if (currentHp <= 0)
                {
                    GetComponent<Escortable>().ShorcutUI.transform.GetChild(4).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteDeath;
                    GetComponent<HungerHandler>().ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                    GetComponent<Escortable>().ShorcutUI.GetComponent<Button>().interactable = false;

                    GetComponent<Escortable>().ShorcutUI.GetComponent<Image>().raycastTarget = false;

                    GetComponent<Escortable>().ShorcutUI.transform.GetChild(2).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                    GetComponent<Escortable>().ShorcutUI.transform.GetChild(4).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                    ShortcutHPUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                    if (GetComponent<HungerHandler>() != null)
                        GetComponent<HungerHandler>().ShortcutHungerUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                }
                else if (currentHp < (Data.MaxHp / 3.0f))
                    ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                else if (currentHp < (2 * Data.MaxHp / 3.0f))
                    ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                else
                    ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentHp / (float)Data.MaxHp;


            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                if (currentHp <= 0)
                {
                    if (GetComponent<MentalHealthHandler>() != null)
                    {
                        GetComponent<Keeper>().ShorcutUI.transform.GetChild(5).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteDeath;
                        GetComponent<Keeper>().ShorcutUI.transform.GetChild(3).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                        GetComponent<Keeper>().ShorcutUI.transform.GetChild(5).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                    }
                    else
                    {
                        GetComponent<Keeper>().ShorcutUI.transform.GetChild(4).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteDeath;
                        GetComponent<Keeper>().ShorcutUI.transform.GetChild(2).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                        GetComponent<Keeper>().ShorcutUI.transform.GetChild(4).GetComponent<Image>().CrossFadeAlpha(0.3f, 0, true);
                    }
                    GetComponent<Keeper>().UpdateActionPoint(0);
                    if (GetComponent<MentalHealthHandler>() != null)
                        GetComponent<MentalHealthHandler>().ShortcutMentalHealthUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                    GetComponent<HungerHandler>().ShortcutHungerUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                    GetComponent<Keeper>().ShorcutUI.GetComponent<Button>().interactable = false;

                    GetComponent<Keeper>().ShorcutUI.GetComponent<Image>().raycastTarget = false;

                    ShortcutHPUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                    if (GetComponent<HungerHandler>() != null)
                        GetComponent<HungerHandler>().ShortcutHungerUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                    if (GetComponent<MentalHealthHandler>() != null)
                        GetComponent<MentalHealthHandler>().ShortcutMentalHealthUI.GetComponentInChildren<Image>().CrossFadeAlpha(0.3f, 0, true);
                }
                else if (currentHp < (Data.MaxHp / 3.0f))
                {
                    ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                    SelectedHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = red;
                }
                else if (currentHp < (2 * Data.MaxHp / 3.0f))
                {
                    ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                    SelectedHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = yellow;
                }
                else
                {
                    ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                    SelectedHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = green;
                }
                SelectedHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentHp / (float)Data.MaxHp;
                ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentHp / (float)Data.MaxHp;
            }
        }
        #endregion

        public void Heal(int _i = 0)
        {
            int costAction = GetComponent<Interactable>().Interactions.Get("Heal").costAction;
            Keeper from = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Keeper>();
            if (from.ActionPoints >= costAction)
            {
                from.ActionPoints -= costAction;
                CurrentHp += 10;
            }
            else
            {
                GameManager.Instance.Ui.ZeroActionTextAnimation(from);
            }
        }

        #region Accessors
        public int MaxHp
        {
            get
            {
                return Data.MaxHp;
            }
        }

        public int CurrentHp
        {
            get { return currentHp; }
            set
            {
                currentHp = value;
                if (currentHp > Data.MaxHp)
                {
                    currentHp = Data.MaxHp;
                    IsAlive = true;
                }
                else if (currentHp <= 0)
                {
                    currentHp = 0;

                    IsAlive = false;
                    Die();
                    UpdateHPPanel(currentHp);
                }
                else
                {
                    IsAlive = true;
                }
                if (IsAlive)
                {
                    UpdateHPPanel(currentHp);
                }

                if (GameManager.Instance.CurrentState == GameState.InBattle || 
                    (GameManager.Instance.CurrentState == GameState.InTuto && TutoManager.s_instance.StateBeforeTutoStarts == GameState.InBattle))
                {
                    if (GetComponent<Keeper>() != null || GetComponent<Escortable>())
                    {
                        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateCharacterLifeBar(this);
                    }
                    else if (GetComponent<Monster>())
                    {
                        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateLifeBar(this);
                    }
                }
            }
        }

        public bool IsAlive
        {
            get
            {
                return isAlive;
            }

            set
            {
                isAlive = value;
            }
        }

        public MortalData Data
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

        public ParticleSystem DeathParticles
        {
            get
            {
                return deathParticles;
            }

            set
            {
                deathParticles = value;
            }
        }

        public GameObject SelectedHPUI
        {
            get
            {
                return selectedHPUI;
            }

            set
            {
                selectedHPUI = value;
            }
        }

        public GameObject ShortcutHPUI
        {
            get
            {
                return shortcutHPUI;
            }

            set
            {
                shortcutHPUI = value;
            }
        }

        #endregion
    }
}

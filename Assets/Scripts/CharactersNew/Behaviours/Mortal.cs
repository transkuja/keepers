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

        int currentHp;
        bool isAlive;

        [SerializeField]
        private ParticleSystem deathParticles;

        // UI
        private GameObject selectedHPUI;
        private GameObject shortcutHPUI;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();
        }

        void Start()
        {
            currentHp = data.MaxHp;
            isAlive = true;
        }

        public void Die()
        {
            if (GetComponent<Keeper>() != null)
            {
                Debug.Log("Blaeuurgh... *dead*");
                PawnInstance pawnInstance = GetComponent<PawnInstance>();

                // Drop items
                ItemManager.AddItemOnTheGround(pawnInstance.CurrentTile, transform, GetComponent<Inventory>().Items);

                // Remove reference from tiles
                TileManager.Instance.RemoveKilledKeeper(pawnInstance);

                // Death operations
                // TODO @Rémi, il me faut de quoi mettre a jour le shortcut panel pour afficher l'icone de mort
                //GameManager.Instance.ShortcutPanel_NeedUpdate = true;

                GlowController.UnregisterObject(GetComponent<GlowObjectCmd>());
                GetComponent<AnimatedPawn>().Anim.SetTrigger("triggerDeath");

                // Try to fix glow bug
                Destroy(GetComponent<GlowObjectCmd>());

                GetComponent<Keeper>().ShowSelectedPanelUI(false);
                GameManager.Instance.CheckGameState();
                if(EventManager.OnKeeperDie != null)
                    EventManager.OnKeeperDie(GetComponent<Keeper>());
                // Deactivate pawn
                DeactivatePawn();
            }
            else
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
            }
            else if (instance.GetComponent<Keeper>() != null)
            {

                CreateSelectedHPPanel();
                SelectedHPUI.name = "Mortal";
                SelectedHPUI.transform.SetParent(instance.GetComponent<Keeper>().SelectedStatPanelUI.transform);
                SelectedHPUI.transform.localScale = Vector3.one;
                SelectedHPUI.transform.localPosition = new Vector3(200, 200, 0);

                ShortcutHPUI.transform.SetParent(instance.GetComponent<Keeper>().ShorcutUI.transform);
                ShortcutHPUI.transform.localScale = Vector3.one;
                ShortcutHPUI.transform.localPosition = Vector3.zero;
            }

            UpdateHPPanel(MaxHp);
        }

        public void CreateSelectedHPPanel()
        {
            SelectedHPUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabHPUI);
        }

        public void CreateShortcutHPPanel()
        {
            ShortcutHPUI = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabHPUI);
        }

        public void UpdateHPPanel(int currentHp)
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentHp / (float)Data.MaxHp;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                SelectedHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentHp / (float)Data.MaxHp;
                ShortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)currentHp / (float)Data.MaxHp;
            }
        }
        #endregion


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
                }
                else
                {
                    IsAlive = true;
                    UpdateHPPanel(currentHp);
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

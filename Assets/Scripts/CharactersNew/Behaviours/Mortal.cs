using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour
{
    public class Mortal : MonoBehaviour
    {
        PawnInstance instance;

        [SerializeField]
        int maxHp;
        int currentHp;
        bool isAlive;

        [SerializeField]
        private Sprite deadSprite;


        // UI
        public GameObject selectedHPUI;
        public GameObject shortcutHPUI;

        void Awake()
        {
            instance = GetComponent<PawnInstance>();


            CreateShortcutHPPanel();
            shortcutHPUI.name = "Mortal";

            if (instance.GetComponent<Keeper>() != null)
            {
                CreateSelectedHPPanel();
                selectedHPUI.name = "Mortal";
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
                selectedHPUI.transform.SetParent(instance.GetComponent<Keeper>().selectedStatPanelUI.transform);
                selectedHPUI.transform.localScale = Vector3.one;
                selectedHPUI.transform.localPosition = new Vector3(200, 200, 0);

                shortcutHPUI.transform.SetParent(instance.GetComponent<Keeper>().shorcutUI.transform);
                shortcutHPUI.transform.localScale = Vector3.one;
                shortcutHPUI.transform.localPosition = Vector3.zero;
            }

            CurrentHp = maxHp;
            isAlive = true;
        }

        public void Die()
        {
            if (GetComponent<Keeper>() != null)
            {
                // TODO refacto TileManager needed
                //Debug.Log("Blaeuurgh... *dead*");
                //Tile currentTile = TileManager.Instance.GetTileFromKeeperOld[this];

                //// Drop items
                //ItemManager.AddItemOnTheGround(currentTile, transform, GetComponent<Behaviour.Inventory>().Items);

                //// Remove reference from tiles
                //TileManager.Instance.RemoveKilledKeeperOld(this);

                //// Death operations
                //GameManager.Instance.ShortcutPanel_NeedUpdate = true;

                //GlowController.UnregisterObject(GetComponent<GlowObjectCmd>());
                //anim.SetTrigger("triggerDeath");

                //// Try to fix glow bug
                //Destroy(GetComponent<GlowObjectCmd>());

                //GameManager.Instance.Ui.HideSelectedKeeperPanel();
                //GameManager.Instance.CheckGameState();

                //// Deactivate pawn
                //DeactivatePawn();
            }
            else if (GetComponent<Monster>() != null)
            {

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
        public void CreateSelectedHPPanel()
        {
            selectedHPUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabHPUI);
        }

        public void CreateShortcutHPPanel()
        {
            shortcutHPUI = Instantiate(GameManager.Instance.PrefabUtils.PrefabHPUI);
        }

        public void UpdateMentalHealthPanel(int hunger)
        {
            if (instance.GetComponent<Escortable>() != null)
            {
                shortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)maxHp;
            }
            else if (instance.GetComponent<Keeper>() != null)
            {
                selectedHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)maxHp;
                shortcutHPUI.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)hunger / (float)maxHp;
            }

        }
        #endregion


        #region Accessors
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

        public int CurrentHp
        {
            get { return currentHp; }
            set
            {
                currentHp = value;
                if (currentHp > maxHp)
                {
                    currentHp = maxHp;
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
                    UpdateMentalHealthPanel(currentHp);
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

        public Sprite DeadSprite
        {
            get
            {
                return deadSprite;
            }

            set
            {
                deadSprite = value;
            }
        }

        #endregion
    }
}

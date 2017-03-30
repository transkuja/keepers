using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Mortal : MonoBehaviour
    {
        PawnInstance instance;

        int maxHp;
        int currentHp;
        bool isAlive;

        void Start()
        {
            instance = GetComponent<PawnInstance>();
            currentHp = maxHp;
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

        #endregion
    }
}

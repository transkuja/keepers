using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Temp
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour {

    // Update is called once per frame
    void Update () {
        if (!CinematiqueManager.Instance.isPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    // On Click on a personnage
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("KeeperInstance"))
                    {
                        KeeperInstance k = hit.transform.gameObject.GetComponent<KeeperInstance>();
                        if (k != null)
                        {
                            if (GameManager.Instance.AllKeepersList.Contains(k))
                            {
                                k.IsSelectedInMenu = false;
                                GameManager.Instance.AllKeepersList.Remove(k);
                            }
                            else
                            {
                                k.IsSelectedInMenu = true;
                                GameManager.Instance.AllKeepersList.Add(k);
                            }
                            GameManager.Instance.CharacterPanelMenuNeedUpdate = true;
                            
                        }
                                                 
                    }
                }
            }

            // Deselect all
            if (Input.GetKeyDown(KeyCode.A))
            {
                foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
                {
                    ki.IsSelectedInMenu = false;
                }
                GameManager.Instance.CharacterPanelMenuNeedUpdate = true;
            }
        }

        // Skip Cinematique
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.AllKeepersList.Count == 0)
            {
                KeeperInstance[] keeperInstances = FindObjectsOfType<KeeperInstance>();
                for (int i = 0; i < keeperInstances.Length; i++)
                {
                    GameManager.Instance.AllKeepersList.Add(keeperInstances[i]);
                }
            }

            StartGame();
        }

    }

    public void StartGame()
    {
        GameManager.Instance.InitializeInGameKeepers();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Fade(AudioManager.Instance.themeMusic);
        }
        SceneManager.LoadScene(1);

    }
}

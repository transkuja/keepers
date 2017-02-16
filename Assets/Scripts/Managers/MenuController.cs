using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Temp
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

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
                        Keepers.Selectable s = hit.transform.gameObject.GetComponent<Keepers.Selectable>();
                        if (s != null)
                        {
                            if (GameManager.Instance.AllKeepersList.Contains(s))
                            {
                                s.Selected = false;
                                GameManager.Instance.AllKeepersList.Remove(s);
                                GameManager.Instance.CharacterPanelMenuNeedUpdate = true;
                            }
                            else
                            {
                                s.Selected = true;
                                GameManager.Instance.AllKeepersList.Add(s);
                                GameManager.Instance.CharacterPanelMenuNeedUpdate = true;
                            }
                        }
                    }
                }
            }

            // Deselect all
            if (Input.GetKeyDown(KeyCode.A))
            {
                foreach (Keepers.Selectable s in GameManager.Instance.AllKeepersList)
                {
                    s.Selected = false;
                }
                GameManager.Instance.AllKeepersList.Clear();
                GameManager.Instance.CharacterPanelMenuNeedUpdate = true;
            }
        }

        // Skip Cinematique
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(AudioManager.Instance!= null)
            {
                AudioManager.Instance.Fade(AudioManager.Instance.themeMusic);
            }
            SceneManager.LoadScene(1);

        }

    }
}

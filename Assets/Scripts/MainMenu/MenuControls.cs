using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Temp
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour {
    public int levelSelected = -1;
    public Image cardLevelSelectedImg;

    // TODO handle this better
    [SerializeField]
    Sprite[] levelImg;

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
                    else if (hit.transform.GetComponent<CardLevel>() != null)
                    {
                        if (levelSelected == hit.transform.GetComponent<CardLevel>().levelIndex)
                        {
                            levelSelected = -1;
                            cardLevelSelectedImg.enabled = false;
                        }
                        else
                        {
                            levelSelected = hit.transform.GetComponent<CardLevel>().levelIndex;
                            cardLevelSelectedImg.sprite = levelImg[levelSelected-1];
                            cardLevelSelectedImg.enabled = true;
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
                GameManager.Instance.AllKeepersList.Clear();
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

            // Prevents doing shit with load scene
            if (levelSelected == -1)
                levelSelected = 1;

            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    public void StartGame()
    {
        GameManager.Instance.InitializeInGameKeepers();
        if (AudioManager.Instance != null)
        {
            AudioClip toPlay;
            switch(levelSelected)
            {
                case 1:
                    toPlay = AudioManager.Instance.Scene1Clip;
                    break;
                case 2:
                    toPlay = AudioManager.Instance.Scene2Clip;
                    break;
                default:
                    toPlay = toPlay = AudioManager.Instance.menuMusic;
                    break;
            }
            AudioManager.Instance.Fade(toPlay);
        }
        SceneManager.LoadScene(levelSelected);

    }
}

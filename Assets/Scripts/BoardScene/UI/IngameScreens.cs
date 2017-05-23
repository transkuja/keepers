using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to identify game screens in scene.
/// </summary>
public class IngameScreens : MonoBehaviour {
    private static IngameScreens instance = null;
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    public void Start()
    {
        instance = this;
        GameManager.Instance.RegisterGameScreens(this);

        if (!SceneManager.GetActiveScene().name.Contains("Menu"))
        {
            switch (GameManager.Instance.QuestManager.CurrentQuestDeck.LevelId)
            {
                case "tuto":
                    transform.GetChild(0).GetChild((int)IngameScreensEnum.WinScreen).GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.level1;
                    break;
                case "level1":
                    transform.GetChild(0).GetChild((int)IngameScreensEnum.WinScreen).GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.level2;
                    break;
                case "level4":
                    transform.GetChild(0).GetChild((int)IngameScreensEnum.WinScreen).GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.level3;
                    break;
            }
        }
    }

    public static IngameScreens Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseEscapeMenu();
        }
    }

    public void OpenCloseEscapeMenu()
    {
        if (GameManager.Instance.CurrentState == GameState.Normal)
        {
            if (GameManager.Instance.Ui != null && GameManager.Instance.Ui.gameObject.activeSelf)
            {
                pauseMenu.transform.SetParent(GameManager.Instance.Ui.transform.GetChild(0));
                pauseMenu.transform.SetAsLastSibling();
                optionsMenu.transform.SetParent(GameManager.Instance.Ui.transform.GetChild(0));
                optionsMenu.transform.SetAsLastSibling();
            }
            pauseMenu.SetActive(true);
            GameManager.Instance.CurrentState = GameState.InPause;
        }
        else if (GameManager.Instance.CurrentState == GameState.InPause)
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                if (GameManager.Instance.Ui != null && GameManager.Instance.Ui.gameObject.activeSelf)
                {
                    pauseMenu.transform.SetParent(transform.GetChild(0));
                    pauseMenu.transform.SetSiblingIndex((int)IngameScreensEnum.EscapeMenu);
                    optionsMenu.transform.SetParent(transform.GetChild(0));
                    optionsMenu.transform.SetSiblingIndex((int)IngameScreensEnum.OptionsMenu);
                }
                transform.GetChild(0).GetChild((int)IngameScreensEnum.BattleResultScreens).gameObject.SetActive(false);
                GameManager.Instance.CurrentState = GameState.Normal;
            }
        }
        else if (GameManager.Instance.CurrentState == GameState.InTuto)
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                if (GameManager.Instance.Ui != null && GameManager.Instance.Ui.gameObject.activeSelf)
                {
                    pauseMenu.transform.SetParent(transform.GetChild(0));
                    pauseMenu.transform.SetSiblingIndex((int)IngameScreensEnum.EscapeMenu);
                    optionsMenu.transform.SetParent(transform.GetChild(0));
                    optionsMenu.transform.SetSiblingIndex((int)IngameScreensEnum.OptionsMenu);
                }
            }
            else
            {
                if (GameManager.Instance.Ui != null && GameManager.Instance.Ui.gameObject.activeSelf)
                {
                    pauseMenu.transform.SetParent(GameManager.Instance.Ui.transform.GetChild(0));
                    pauseMenu.transform.SetAsLastSibling();
                    optionsMenu.transform.SetParent(GameManager.Instance.Ui.transform.GetChild(0));
                    optionsMenu.transform.SetAsLastSibling();
                }
                pauseMenu.SetActive(true);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        pauseMenu.SetActive(false);
        if (GameManager.Instance.Ui != null && GameManager.Instance.Ui.gameObject.activeSelf)
        {
            pauseMenu.transform.SetParent(transform.GetChild(0));
            pauseMenu.transform.SetSiblingIndex((int)IngameScreensEnum.EscapeMenu);
            optionsMenu.transform.SetParent(transform.GetChild(0));
            optionsMenu.transform.SetSiblingIndex((int)IngameScreensEnum.OptionsMenu);
        }

        if (GameManager.Instance.CurrentState == GameState.InTuto && TutoManager.s_instance.PlayingSequence != null)
        {
            TutoManager.s_instance.Reset();
        }
        GameManager.Instance.CurrentState = GameState.Normal;
        AudioManager.Instance.Fade(AudioManager.Instance.menuMusic);
        GameManager.Instance.Ui.GoActionPanelQ.transform.parent.SetParent(GameManager.Instance.Ui.transform);
        SceneManager.LoadScene(0);
    }
}

public enum IngameScreensEnum
{
    BattleResultScreens,
    SelectBattleCharactersScreen,
    WinScreen,
    LoseScreen,
    EscapeMenu,
    OptionsMenu,
    MainQuestPanel,
    QuestReminderButton
}

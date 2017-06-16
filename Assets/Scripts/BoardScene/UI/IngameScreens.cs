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
    public GameObject theMenu;

    public void Start()
    {
        instance = this;
        GameManager.Instance.RegisterGameScreens(this);

        if (!SceneManager.GetActiveScene().name.Contains("Menu"))
        {
            switch (GameManager.Instance.QuestManager.CurrentQuestDeck.LevelId)
            {
                case "tuto":
                    transform.GetChild(0).GetChild((int)IngameScreensEnum.WinScreen).GetComponentInChildren<Image>().sprite = Translater.EndLevelWinningScreen(0);
                    break;
                case "level1":
                    transform.GetChild(0).GetChild((int)IngameScreensEnum.WinScreen).GetComponentInChildren<Image>().sprite = Translater.EndLevelWinningScreen(1);
                    break;
                case "level4":
                    transform.GetChild(0).GetChild((int)IngameScreensEnum.WinScreen).GetComponentInChildren<Image>().sprite = Translater.EndLevelWinningScreen(4);
                    break;
            }

            transform.GetChild(0).GetChild((int)IngameScreensEnum.LanguageSelection).gameObject.SetActive(false);

        }
        else
        {
            if (GameManager.Instance.menuReload)
            {
                transform.GetChild(0).GetChild((int)IngameScreensEnum.LanguageSelection).gameObject.SetActive(false);
                if (theMenu != null)
                    theMenu.SetActive(true);
                GameManager.Instance.DontReplayDuckAnimation();
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

    public void SetLanguage(string selectedLanguage)
    {
        Translater.CurrentLanguage = (LanguageEnum)System.Enum.Parse(typeof(LanguageEnum), selectedLanguage);
        transform.GetChild(0).GetChild((int)IngameScreensEnum.LanguageSelection).gameObject.SetActive(false);

        Camera.main.GetComponent<CameraMenu>().ResetPosition = true;
        transform.GetChild(0).GetChild((int)IngameScreensEnum.LoseScreen).GetComponentInChildren<Image>().sprite = Translater.GameOverScreen();

        transform.GetChild(0).GetChild((int)IngameScreensEnum.GameSelection).GetChild(0).GetComponent<Text>().text = Translater.GameSelection("new");
        transform.GetChild(0).GetChild((int)IngameScreensEnum.GameSelection).GetChild(1).GetComponent<Text>().text = Translater.GameSelection("");
        transform.GetChild(0).GetChild((int)IngameScreensEnum.GameSelection).gameObject.SetActive(true);
    }

    public void ChooseGame(string selectedOption)
    {
        if (selectedOption == "new")
            GameManager.Instance.ResetGameData();

        transform.GetChild(0).GetChild((int)IngameScreensEnum.GameSelection).gameObject.SetActive(false);
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
    QuestReminderButton,
    LanguageSelection,
    GameSelection
}

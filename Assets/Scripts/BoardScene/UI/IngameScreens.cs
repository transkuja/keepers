using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


/// <summary>
/// Used to identify game screens in scene.
/// </summary>
public class IngameScreens : MonoBehaviour {
    private static IngameScreens instance = null;

    public void Awake()
    {
        instance = this;
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
        if (Time.timeScale == 1.0f)
        {
            transform.GetChild(0).GetChild((int)IngameScreensEnum.EscapeMenu).gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            transform.GetChild(0).GetChild((int)IngameScreensEnum.EscapeMenu).gameObject.SetActive(false);
            transform.GetChild(0).GetChild((int)IngameScreensEnum.BattleResultScreens).gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        AudioManager.Instance.Fade(AudioManager.Instance.themeMusic);
        GameManager.Instance.ResetInstance();
        SceneManager.LoadScene(0);
    }
}

public enum IngameScreensEnum
{
    BattleResultScreens,
    SelectBattleCharactersScreen,
    WinScreen,
    LoseScreen,
    EscapeMenu
}

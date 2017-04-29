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

    public void Start()
    {
        instance = this;
        GameManager.Instance.RegisterGameScreens(this);
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
        if (GameManager.Instance.CurrentState == GameState.Normal || GameManager.Instance.CurrentState == GameState.InTuto)
        {
            transform.GetChild(0).GetChild((int)IngameScreensEnum.EscapeMenu).gameObject.SetActive(true);
            GameManager.Instance.CurrentState = GameState.InPause;
        }
        else if (GameManager.Instance.CurrentState == GameState.InPause)
        {
            if (transform.GetChild(0).GetChild((int)IngameScreensEnum.EscapeMenu).gameObject.activeInHierarchy)
            {
                transform.GetChild(0).GetChild((int)IngameScreensEnum.EscapeMenu).gameObject.SetActive(false);
                transform.GetChild(0).GetChild((int)IngameScreensEnum.BattleResultScreens).gameObject.SetActive(false);
                if (TutoManager.s_instance != null) // TMP
                {
                    if (TutoManager.s_instance.enableTuto == true)
                    {
                        GameManager.Instance.CurrentState = GameState.InTuto;
                    }
                } else
                {
                    GameManager.Instance.CurrentState = GameState.Normal;
                }

            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        GameManager.Instance.CurrentState = GameState.Normal;
        AudioManager.Instance.Fade(AudioManager.Instance.menuMusic);
        GameManager.Instance.ResetInstance();
        SceneManager.LoadScene(0);
    }


    public void GoToTuto()
    {
        GameManager.Instance.CurrentState = GameState.Normal;
        GameManager.Instance.ResetInstance();
        SceneManager.LoadScene(3);
    }
}

public enum IngameScreensEnum
{
    BattleResultScreens,
    SelectBattleCharactersScreen,
    WinScreen,
    LoseScreen,
    EscapeMenu,
    MainQuestPanel
}

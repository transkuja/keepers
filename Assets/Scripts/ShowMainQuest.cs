using UnityEngine;

public class ShowMainQuest : MonoBehaviour {

    private float timeToResetCamera = 2.5f;
    private float timeToLaunchTuto = 1.5f;
    bool isTimerActive = false;
    bool isFirstStepFinished = false;

    private void OnEnable()
    {
        GameManager.Instance.CurrentState = GameState.InPause;
        timeToResetCamera = 2.5f;
        timeToLaunchTuto = 1.5f;
    }

    private void OnDisable()
    {
        GameManager.Instance.CurrentState = GameState.Normal;
    }

    public void ShowLevelArrival()
    {
        GameManager.Instance.UpdateCameraPosition(TileManager.Instance.EndTile);
        isTimerActive = true;
    }

    public void Update()
    {
        if (isTimerActive)
        {
            timeToResetCamera -= Time.deltaTime;
            if (timeToResetCamera <= 0.0f)
            {
                isTimerActive = false;
                GameManager.Instance.UpdateCameraPosition(TileManager.Instance.BeginTile);
                isFirstStepFinished = true;
            }
        }
        
        if (isFirstStepFinished)
        {
            timeToLaunchTuto -= Time.deltaTime;
            if (timeToLaunchTuto <= 0.0f)
            {
                gameObject.SetActive(false);
                if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto && TutoManager.s_instance.GetComponent<SeqFirstMove>().AlreadyPlayed == false)
                {
                    TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqFirstMove>());
                }
            }
        }
    }
}

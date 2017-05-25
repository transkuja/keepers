using UnityEngine;
using UnityEngine.UI;

public class ShowMainQuest : MonoBehaviour {

    private float timeToResetCamera = 2.5f;
    private float timeToLaunchTuto = 1.5f;
    bool isTimerActive = false;
    bool isFirstStepFinished = false;
    public GameObject btnReviewMainQuest;
    bool wasAlreadyInPause = false;

    private void TranslateTexts()
    {
        // QuestInfos
        // Objectives
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Translater.MainQuestText(MainQuestTexts.Objective);
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = Translater.MainQuestText(MainQuestTexts.ObjectiveInfo);
        transform.GetChild(0).GetChild(3).GetComponentInChildren<Text>().text = Translater.MainQuestText(MainQuestTexts.Other);
        // QuestTitle
        transform.GetChild(1).GetComponentInChildren<Text>().text = Translater.MainQuestText(MainQuestTexts.Title);
    }

    private void OnEnable()
    {
        TranslateTexts();
        if (GameManager.Instance.CurrentState == GameState.InPause)
            wasAlreadyInPause = true;
        else
            wasAlreadyInPause = false;

        GameManager.Instance.CurrentState = GameState.InPause;
        timeToResetCamera = 2.5f;
        timeToLaunchTuto = 1.5f;
    }

    private void OnDisable()
    {
        if (wasAlreadyInPause)
            GameManager.Instance.CurrentState = GameState.InPause;
        else
            GameManager.Instance.CurrentState = GameState.Normal;
    }

    public void ShowLevelArrival()
    {
        GameManager.Instance.UpdateCameraPosition(TileManager.Instance.EndTile);
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        isTimerActive = true;
    }

    public void showMainQuestObjectif()
    {
        if (GameManager.Instance.CurrentState == GameState.InTuto)
            return;
        if( gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
            isFirstStepFinished = true;
        } else
        {
            this.gameObject.SetActive(true);
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
            isFirstStepFinished = false;
        }

    }

    public void Update()
    {
        if (!isTimerActive && Input.GetKeyDown(KeyCode.Space))
        {
            ShowLevelArrival();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isTimerActive)
            {
                isTimerActive = false;
                isFirstStepFinished = true;
            }
            else
            {
                isTimerActive = false;
                isFirstStepFinished = false;

                gameObject.SetActive(false);
                if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto)
                {
                    if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqfirstmove"] == false)
                        TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqFirstMove>());
                    else if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqmulticharacters"] == false)
                        TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqMultiCharacters>());
                }
            }

            GameManager.Instance.UpdateCameraPosition(TileManager.Instance.BeginTile);

            if (btnReviewMainQuest.activeSelf == false)
            {
                btnReviewMainQuest.SetActive(true);
                if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqfirstmove"] == true)
                    GameManager.Instance.Ui.TurnButton.transform.parent.gameObject.SetActive(true);

            }
        }

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
                if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto)
                {
                    if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqfirstmove"] == false)
                        TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqFirstMove>());
                    else if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqmulticharacters"] == false)
                        TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqMultiCharacters>());
                }

                if (btnReviewMainQuest.activeSelf == false)
                {
                    btnReviewMainQuest.SetActive(true);
                    if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqfirstmove"] == true)
                        GameManager.Instance.Ui.TurnButton.transform.parent.gameObject.SetActive(true);

                }
            }
        }
    }
}

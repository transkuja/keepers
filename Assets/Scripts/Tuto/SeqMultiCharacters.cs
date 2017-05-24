using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class SeqMultiCharacters : Sequence
{
    public GameObject selectedKeepersPanel;
    public GameObject shortcutButton;
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;

    public class ShowCurrentCharacterAvatar : Step
    {
        string str;
        GameObject feedback;
        public ShowCurrentCharacterAvatar(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            SeqMultiCharacters seqMultiCharacters = TutoManager.s_instance.GetComponent<SeqMultiCharacters>();
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = seqMultiCharacters.selectedKeepersPanel.transform.GetChild(0).GetChild(1).GetChild(4).position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 60.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            
            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    public class InteractWithAnotherCharacter : Step
    {
        string str;
        List<GameObject> feedbacksMouse;
        public InteractWithAnotherCharacter(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            feedbacksMouse = new List<GameObject>();
            for (int i = 1; i < GameManager.Instance.AllKeepersList.Count; i++)
            {
                if (GameManager.Instance.AllKeepersList[i] != GameManager.Instance.GetFirstSelectedKeeper())
                {
                    if (GameManager.Instance.AllKeepersList[i].gameObject.GetComponent<RightMouseClickExpected>() == null)
                    {
                        GameManager.Instance.AllKeepersList[i].gameObject.AddComponent<RightMouseClickExpected>();
                        GameManager.Instance.AllKeepersList[i].gameObject.GetComponent<RightMouseClickExpected>().TargetExpected = "Keeper";
                    }
                    GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(true);
                    GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>().enabled = true;

                    GameObject curKeeperFeedbackMouse = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, GameManager.Instance.Ui.transform.GetChild(0));
                    curKeeperFeedbackMouse.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.AllKeepersList[i].GetComponent<Interactable>().Feedback.position) + Vector3.up * (50 * (Screen.height / 1080.0f));
                    curKeeperFeedbackMouse.transform.localScale = Vector3.one;
                    curKeeperFeedbackMouse.AddComponent<ShowClickIsExpected>();
                    curKeeperFeedbackMouse.GetComponent<ShowClickIsExpected>().IsLeftClick = false;
                    feedbacksMouse.Add(curKeeperFeedbackMouse);
                }
   
            }

            if (GameManager.Instance.PrisonerInstance.gameObject.GetComponent<RightMouseClickExpected>() == null)
            {
                GameManager.Instance.PrisonerInstance.gameObject.AddComponent<RightMouseClickExpected>();
                GameManager.Instance.PrisonerInstance.gameObject.GetComponent<RightMouseClickExpected>().TargetExpected = "Keeper";
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            for (int i = 1; i < GameManager.Instance.AllKeepersList.Count; i++)
            {
                if (GameManager.Instance.AllKeepersList[i].gameObject.GetComponent<RightMouseClickExpected>() != null)
                    Destroy(GameManager.Instance.AllKeepersList[i].gameObject.GetComponent<RightMouseClickExpected>());
                GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(false);
            }
            if (GameManager.Instance.PrisonerInstance.gameObject.GetComponent<RightMouseClickExpected>() != null)
            {
                Destroy(GameManager.Instance.PrisonerInstance.gameObject.GetComponent<RightMouseClickExpected>());
            }

            foreach (GameObject go in feedbacksMouse)
                Destroy(go);
            feedbacksMouse.Clear();
            alreadyPlayed = false;
        }
    }

    public class ShortcutPanelTeaching : Step
    {
        string str;
        GameObject feedback;
        public ShortcutPanelTeaching(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            SeqMultiCharacters seqMultiCharacters = TutoManager.s_instance.GetComponent<SeqMultiCharacters>();
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = seqMultiCharacters.shortcutButton.transform.position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 70.0f;
                feedback.GetComponent<FlecheQuiBouge>().magnitude = 100.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -120);
            }

            if (seqMultiCharacters.shortcutButton.gameObject.GetComponent<MouseClickExpected>() == null)
                seqMultiCharacters.shortcutButton.gameObject.AddComponent<MouseClickExpected>();

            seqMultiCharacters.shortcutButton.transform.parent.gameObject.SetActive(true);
            Button[] shortcutButt = TutoManager.s_instance.shortcutButton.GetComponentsInChildren<Button>();

            foreach (Button b in shortcutButt)
            {
                b.interactable = true;
            }


            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
        }

        public override void Reverse()
        {
            SeqMultiCharacters seqMultiCharacters = TutoManager.s_instance.GetComponent<SeqMultiCharacters>();
            if (seqMultiCharacters.shortcutButton.gameObject.GetComponent<MouseClickExpected>() != null)
                Destroy(seqMultiCharacters.shortcutButton.gameObject.GetComponent<MouseClickExpected>());
            Destroy(feedback);
            foreach (Button b in GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>())
            {
                b.interactable = false;
            }
            alreadyPlayed = false;
        }
    }

    public class FreezeInteractionsForTuto : Step
    {
        string str;
        GameObject feedback;
        public FreezeInteractionsForTuto(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
            isReachableByClickOnPrevious = false;
        }

        public void Message_fct()
        {
            foreach (Button b in GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>())
            {
                ColorBlock cb = b.colors;
                cb.disabledColor = Color.white;
                b.colors = cb;
                b.interactable = false;
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            GameManager.Instance.Ui.ClearActionPanel();
            alreadyPlayed = false;
        }
    }

    public override void Init()
    {
        base.Init();
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f) + GameManager.Instance.ActiveTile.transform.position);

        GameManager.Instance.AllKeepersList[0].GetComponent<Behaviour.Keeper>().IsSelected = true;
        GameManager.Instance.AddKeeperToSelectedList(GameManager.Instance.AllKeepersList[0]);

        Button leftBtn = GameManager.Instance.AllKeepersList[0].GetComponent<Behaviour.Keeper>().BtnLeft.GetComponent<Button>();
        leftBtn.interactable = false;
        Button rightBtn = GameManager.Instance.AllKeepersList[0].GetComponent<Behaviour.Keeper>().BtnRight.GetComponent<Button>();
        rightBtn.interactable = false;

        ColorBlock cb = leftBtn.colors;
        cb.disabledColor = Color.white;
        leftBtn.colors = cb;

        cb = rightBtn.colors;
        cb.disabledColor = Color.white;
        rightBtn.colors = cb;

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqMultiCharacters", 0)));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqMultiCharacters", 1)));
        Etapes.Add(new ShowCurrentCharacterAvatar(Translater.TutoText("SeqMultiCharacters", 2)));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqMultiCharacters", 3)));
        Etapes.Add(new InteractWithAnotherCharacter(Translater.TutoText("SeqMultiCharacters", 4)));
        Etapes.Add(new FreezeInteractionsForTuto(Translater.TutoText("SeqMultiCharacters", 5)));
        Etapes.Add(new ShortcutPanelTeaching(Translater.TutoText("SeqMultiCharacters", 6)));
    }

    public override void End()
    {
        base.End();
        GameManager.Instance.AllKeepersList[0].GetComponent<Behaviour.Keeper>().BtnLeft.GetComponent<Button>().interactable = true;
        GameManager.Instance.AllKeepersList[0].GetComponent<Behaviour.Keeper>().BtnRight.GetComponent<Button>().interactable = true;
        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
            GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(false);

        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.GetComponent<SeqMultiCharacters>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

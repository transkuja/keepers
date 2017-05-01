using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeqAshleyEscort : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;
    
    public class DeactivateButtons : Step
    {
        string str;
        public DeactivateButtons(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            foreach(Button b in GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>())
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
            alreadyPlayed = false;
        }
    }

    public class ShowEscortAction : Step
    {
        string str;
        GameObject feedback;
        public ShowEscortAction(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = Camera.main.WorldToScreenPoint(GameManager.Instance.Ui.GoActionPanelQ.transform.GetChild(1).position);
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 70.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -45);
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

    public class ShowFeedAction : Step
    {
        string str;
        GameObject feedback;
        public ShowFeedAction(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
            isReachableByClickOnPrevious = false;
        }

        public void Message_fct()
        {
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = Camera.main.WorldToScreenPoint(GameManager.Instance.Ui.GoActionPanelQ.transform.GetChild(0).position);
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 70.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -45);
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

    public class ReactivateButtons : Step
    {
        string str;
        public ReactivateButtons(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            foreach (Button b in GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>())
            {
                b.interactable = true;
            }

            GameManager.Instance.Ui.ClearActionPanel();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            alreadyPlayed = false;
        }
    }

    public override void Init()
    {
        base.Init();
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f) + GameManager.Instance.ActiveTile.transform.position);

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        Etapes.Add(new DeactivateButtons("You have to escort Ashley safely to destination."));
        Etapes.Add(new ShowEscortAction("You can use the Escort action to change the character Ashley is following,"));
        Etapes.Add(new ShowFeedAction("or the Feed action to feed her."));
        Etapes.Add(new ReactivateButtons("Take good care of her!"));
    }

    public override void End()
    {
        base.End();
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.PlayingSequence = null;
        TutoManager.s_instance.GetComponent<SeqAshleyEscort>().AlreadyPlayed = true;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Behaviour;

public class SeqFirstMove : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;

    [Header("Hidden in Tuto")]
    public GameObject selectedKeepersPanel;
    public GameObject shortcutBtn;
    public GameObject endTurnBtn;

    public class ExploreActionPointsExplanation : Step
    {
        string str;
        GameObject feedbackPointer;
        public ExploreActionPointsExplanation(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().transform.GetChild(0).gameObject.GetComponent<ThrowDiceButtonFeedback>() == null)
                GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().transform.GetChild(0).gameObject.AddComponent<ThrowDiceButtonFeedback>();

            GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().interactable = false;
            if (feedbackPointer == null)
            {
                feedbackPointer = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedbackPointer.GetComponent<FlecheQuiBouge>().PointToPoint = Camera.main.WorldToScreenPoint(GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().transform.GetChild(0).position);
                feedbackPointer.GetComponent<FlecheQuiBouge>().distanceOffset = 30.0f;

                feedbackPointer.transform.localEulerAngles = new Vector3(0, 0, -80);
            }

            Button endTurnButt = TutoManager.s_instance.endTurnButton.GetComponentInChildren<Button>();
            endTurnButt.interactable = true;

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            if (GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().transform.GetChild(0).gameObject.GetComponent<ThrowDiceButtonFeedback>() != null)
                Destroy(GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().transform.GetChild(0).gameObject.GetComponent<ThrowDiceButtonFeedback>());
            GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().interactable = true;
            Destroy(feedbackPointer);
            alreadyPlayed = false;
        }
    }

    public class RightClickOnBridgeValidated : Step
    {
        string str;
        public RightClickOnBridgeValidated(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().interactable = false;

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().interactable = true;
            alreadyPlayed = false;
        }
    }

    public class ExploreStep : Step
    {
        string str;
        public ExploreStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.GetComponent<ThrowDiceButtonFeedback>() == null)
                GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.AddComponent<ThrowDiceButtonFeedback>();
            if (GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.GetComponent<MouseClickExpected>() == null)
                GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.AddComponent<MouseClickExpected>();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
        }
    
        public override void Reverse()
        {
            if (GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.GetComponent<ThrowDiceButtonFeedback>() != null)
                Destroy(GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.GetComponent<ThrowDiceButtonFeedback>());
            if (GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.GetComponent<MouseClickExpected>() != null)
                Destroy(GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.GetComponent<MouseClickExpected>());

            alreadyPlayed = false;
        }
    }

    public class SelectCharacterStep : Step
    {
        string str;
        public SelectCharacterStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            // show text
            if (GameManager.Instance.AllKeepersList[0].gameObject.GetComponent<MouseClickedOnIngameElt>() == null)
                GameManager.Instance.AllKeepersList[0].gameObject.AddComponent<MouseClickedOnIngameElt>();

            GameManager.Instance.AllKeepersList[0].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(true);
            GameManager.Instance.AllKeepersList[0].GetComponent<GlowObjectCmd>().enabled = true;

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            Destroy(GameManager.Instance.AllKeepersList[0].gameObject.GetComponent<MouseClickedOnIngameElt>());
            GameManager.Instance.AllKeepersList[0].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(false);
            alreadyPlayed = false;
        }
    }

    public class MovePawnOnTileStep : Step
    {
        string str;
        public MovePawnOnTileStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            // show text
            if (GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.GetComponent<RightMouseClickExpected>() == null)
                GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.AddComponent<RightMouseClickExpected>();
            GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.GetComponent<RightMouseClickExpected>().TargetExpected = "Tile";

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            Destroy(GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.GetComponent<RightMouseClickExpected>());
            alreadyPlayed = false;
        }
    }

    public class MovePawnToAnotherTileExplanation : Step
    {
        string str;
        public MovePawnToAnotherTileExplanation(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            // show text
            GameObject portal = GameManager.Instance.AllKeepersList[0].CurrentTile.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
            if (portal.GetComponent<RightMouseClickExpected>() == null)
                portal.AddComponent<RightMouseClickExpected>();
            portal.GetComponent<RightMouseClickExpected>().TargetExpected = "Portal";

            if (portal.GetComponent<GlowObjectCmd>() == null)
                portal.AddComponent<GlowObjectCmd>();
            portal.transform.parent.gameObject.SetActive(true);

            portal.GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(true);
            portal.GetComponent<GlowObjectCmd>().enabled = true;

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            GameObject portal = GameManager.Instance.AllKeepersList[0].CurrentTile.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
            portal.GetComponent<GlowObjectCmd>().UpdateColor(false);
            if (portal.GetComponent<RightMouseClickExpected>() != null)
                Destroy(portal.GetComponent<RightMouseClickExpected>());
            if (portal.GetComponent<GlowObjectCmd>() != null)
                Destroy(portal.GetComponent<GlowObjectCmd>());

            alreadyPlayed = false;
        }
    }

    public class ActionPointsExplanationStep : Step
    {
        string str;
        GameObject feedbackPointer;
        public ActionPointsExplanationStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            // Feedback sur les points d'action
            // show text
            GameObject actionPoints = GameManager.Instance.AllKeepersList[0].GetComponent<Keeper>().SelectedActionPointsUI;

            if (feedbackPointer == null)
            {
                feedbackPointer = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedbackPointer.SetActive(false);
                feedbackPointer.GetComponent<FlecheQuiBouge>().PointToPoint = actionPoints.transform.GetChild(2).position;
                feedbackPointer.GetComponent<FlecheQuiBouge>().distanceOffset = 30.0f;

                feedbackPointer.transform.localEulerAngles = new Vector3(0, 0, -80);
                feedbackPointer.SetActive(true);
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            // Desactive le feedback sur les points d'action
            Destroy(feedbackPointer);
            alreadyPlayed = false;
        }
    }

    public class ReactivateAscFeedbackStep : Step
    {
        string str;
        public ReactivateAscFeedbackStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            TutoManager.s_instance.EcrireMessage(str);

            // Reactivate feedback above heads
            if (!GameManager.Instance.AllKeepersList[0].transform.GetChild(2).GetChild(0).gameObject.activeSelf)
                GameManager.Instance.AllKeepersList[0].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            if (!GameManager.Instance.PrisonerInstance.transform.GetChild(2).GetChild(0).gameObject.activeSelf)
                GameManager.Instance.PrisonerInstance.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);

            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            alreadyPlayed = false;
        }
    }

    public class GiveACookieStep : Step
    {
        string str;
        bool hasCookieBeenOffered = false;
        public GiveACookieStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
            hasCookieBeenOffered = false;
        }

        public void Message_fct()
        {
            TutoManager.s_instance.EcrireMessage(str);

            if (!hasCookieBeenOffered)
            {
                ItemContainer cookie = new ItemContainer(GameManager.Instance.ItemDataBase.getItemById("thecookie"), 1);
                InventoryManager.AddItemToInventory(GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().Items, cookie);
                GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().UpdateInventories();
            }
            GameManager.Instance.AllKeepersList[0].AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteCookie, 1);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            hasCookieBeenOffered = true;
            alreadyPlayed = false;
        }
    }

    public class FirstEndTurnStep : Step
    {
        string str;
        GameObject feedback;
        public FirstEndTurnStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            GameObject endTurnButton = ((SeqFirstMove)TutoManager.s_instance.PlayingSequence).endTurnBtn;
            endTurnButton.SetActive(true);

            if (endTurnButton.GetComponentInChildren<Button>().gameObject.GetComponent<MouseClickExpected>() == null)
                endTurnButton.GetComponentInChildren<Button>().gameObject.AddComponent<MouseClickExpected>();
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0)); // Fix: reference to end turn button may need to be stocked somewhere
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = endTurnButton.transform.GetChild(1).position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 75.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, 20);
            }

            TutoManager.s_instance.EcrireMessage(str);

            // Reactivate feedback above heads
            GameManager.Instance.AllKeepersList[0].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            GameManager.Instance.PrisonerInstance.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);

            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
        }

        public override void Reverse()
        {
            Destroy(((SeqFirstMove)TutoManager.s_instance.PlayingSequence).endTurnBtn.GetComponentInChildren<Button>().gameObject.GetComponent<MouseClickExpected>());
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    public class AddHungerStep : Step
    {
        string str;
        GameObject feedback;
        public AddHungerStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            GameObject hungerPanel = GameManager.Instance.AllKeepersList[0].GetComponent<HungerHandler>().SelectedHungerUI;
            hungerPanel.SetActive(true);

            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0)); // Fix: reference to end turn button may need to be stocked somewhere
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = hungerPanel.transform.position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 150.0f;

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

    public class UseAnObjectStep : Step
    {
        string str;
        public UseAnObjectStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().SelectedInventoryPanel.gameObject.SetActive(true); // Inventory

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
        }

        public override void Reverse()
        {
            alreadyPlayed = false;
        }
    }

    public override void Init()
    {
        base.Init();
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f));
        //pointer = SpawnPointer();
        // hide

        //pointer2 = SpawnPointer2();
        Etapes = new List<Step>();
        // First
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        // Content
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Hi, I'm here to teach you how to play"));
        Etapes.Add(new SelectCharacterStep("First select the girl in armor by clicking on her."));
        Etapes.Add(new MovePawnOnTileStep("To interact with the world, you have to use the right click. Click on the ground to move the girl."));
        Etapes.Add(new MovePawnToAnotherTileExplanation("You can also interact with everything glowing in the world. Try a right click on this portal.")); // => click expected on bridge
        Etapes.Add(new RightClickOnBridgeValidated("Good,"));
        Etapes.Add(new ExploreActionPointsExplanation("you can see the cost of the action here."));
        Etapes.Add(new ExploreStep("Now click on the Explore button to explore the next area. And get a cookie."));

        Etapes.Add(new ReactivateAscFeedbackStep("Well done you genius,"));
        Etapes.Add(new GiveACookieStep("here's your cookie!"));
        Etapes.Add(new ActionPointsExplanationStep("This action cost you 3 action points. Always keep an eye on them.")); // ==> feedback sur les points d'action
        Etapes.Add(new FirstEndTurnStep("You can restore your action points by clicking on the end turn button."));

        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Ending your turn ends the day,")); // Activate hunger panel + feedback pointer
        Etapes.Add(new AddHungerStep("but your characters get hungry in the morning, so be careful!"));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Remember the cookie?"));
        Etapes.Add(new UseAnObjectStep("Eat it by double clicking on it to restore your selected character's hunger.")); // Activate inventory and add a cookie in to be used

        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Great! You should be able to finish this level now. Good luck!"));
    }

    public override void End()
    {
        base.End();
        selectedKeepersPanel.SetActive(true);
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.GetComponent<SeqFirstMove>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

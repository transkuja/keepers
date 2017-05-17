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
                feedbackPointer.GetComponent<FlecheQuiBouge>().distanceOffset = 80.0f;

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
        GameObject feedbackMouse;
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

            if (feedbackMouse == null)
            {
                feedbackMouse = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, GameManager.Instance.Ui.transform.GetChild(0));
                feedbackMouse.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.AllKeepersList[0].GetComponent<Interactable>().Feedback.position) + Vector3.up * (50 * (Screen.height/1080.0f));
                feedbackMouse.transform.localScale = Vector3.one;
                feedbackMouse.AddComponent<ShowClickIsExpected>();
                feedbackMouse.GetComponent<ShowClickIsExpected>().IsLeftClick = true;
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            Destroy(GameManager.Instance.AllKeepersList[0].gameObject.GetComponent<MouseClickedOnIngameElt>());
            Destroy(feedbackMouse);
            GameManager.Instance.AllKeepersList[0].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(false);
            alreadyPlayed = false;
        }
    }

    public class MovePawnOnTileStep : Step
    {
        string str;
        GameObject feedbackMouse;
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


            if (feedbackMouse == null)
            {
                feedbackMouse = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, GameManager.Instance.Ui.transform.GetChild(0));
                feedbackMouse.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.AllKeepersList[0].CurrentTile.transform.position) + Vector3.left * (150 * (Screen.width / 1920.0f)) + Vector3.up * (50 * (Screen.height / 1080.0f));
                feedbackMouse.transform.localScale = Vector3.one;
                feedbackMouse.AddComponent<ShowClickIsExpected>();
                feedbackMouse.GetComponent<ShowClickIsExpected>().IsLeftClick = false;
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            Destroy(GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.GetComponent<RightMouseClickExpected>());
            Destroy(feedbackMouse);
            alreadyPlayed = false;
        }
    }

    public class MovePawnToAnotherTileExplanation : Step
    {
        string str;
        GameObject feedbackMouse;
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

            if (feedbackMouse == null)
            {
                feedbackMouse = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, GameManager.Instance.Ui.transform.GetChild(0));
                feedbackMouse.transform.position = Camera.main.WorldToScreenPoint(portal.transform.position) + Vector3.up * (150 * (Screen.height / 1080.0f));
                feedbackMouse.transform.localScale = Vector3.one;
                feedbackMouse.AddComponent<ShowClickIsExpected>();
                feedbackMouse.GetComponent<ShowClickIsExpected>().IsLeftClick = false;
            }

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

            Destroy(feedbackMouse);

            alreadyPlayed = false;
        }
    }

    public class ActionPointsExplanationStep : Step
    {
        string str;
        GameObject feedbackPointer;
        GameObject feedbackCircle;
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
                feedbackPointer.GetComponent<FlecheQuiBouge>().PointToPoint = actionPoints.transform.GetChild(1).position;
                feedbackPointer.GetComponent<FlecheQuiBouge>().distanceOffset = 30.0f;

                feedbackPointer.transform.localEulerAngles = new Vector3(0, 0, -80);
                feedbackPointer.SetActive(true);
            }


            if (feedbackCircle == null)
            {
                feedbackCircle = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, GameManager.Instance.Ui.transform.GetChild(0));
                feedbackCircle.transform.position = (actionPoints.transform.GetChild(0).position + actionPoints.transform.GetChild(1).position + actionPoints.transform.GetChild(2).position) / 3.0f;
                feedbackCircle.transform.localScale = Vector3.one * 2.0f;
                feedbackCircle.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteTutoCircleFeedback;
                feedbackCircle.AddComponent<ThrowDiceButtonFeedback>();
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            // Desactive le feedback sur les points d'action
            Destroy(feedbackPointer);
            Destroy(feedbackCircle);
            alreadyPlayed = false;
        }
    }

    public class ReactivateAscFeedbackStep : Step
    {
        string str;
        GameObject feedback;
        GameObject feedbackCircle;
        public UseAnObjectStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().SelectedInventoryPanel.gameObject.SetActive(true); // Inventory

            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().SelectedInventoryPanel.transform.position + Vector3.up * (100 * (Screen.height/1080.0f));
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 200.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, 90);
            }

            if (feedbackCircle == null)
            {
                feedbackCircle = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI);
                feedbackCircle.transform.SetParent(GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().SelectedInventoryPanel.transform.GetChild(0).GetChild(0));
                feedbackCircle.transform.localPosition = Vector3.zero;
                feedbackCircle.transform.localScale = Vector3.one * 2.0f;
                feedbackCircle.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteTutoCircleFeedback;
                feedbackCircle.AddComponent<ThrowDiceButtonFeedback>();
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
            TutoManager.EnablePreviousButton(false);
        }

        public override void Reverse()
        {
            Destroy(feedbackCircle);
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    public override void Init()
    {
        base.Init();
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f));

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        // Content
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Hi, I'm here to teach you how to play"));
        Etapes.Add(new SelectCharacterStep("First select the girl in armor by clicking on her."));
        Etapes.Add(new MovePawnOnTileStep("To interact with the world, you have to use the right click. Click on the ground to move the girl."));
        //Etapes.Add(new MovePawnToAnotherTileExplanation("You can also interact with everything glowing in the world. Try a right click on this portal.")); // => click expected on bridge
        Etapes.Add(new MovePawnToAnotherTileExplanation("You can also interact with everything glowing in the world. Try right clicking on this portal.")); // => click expected on bridge
        Etapes.Add(new RightClickOnBridgeValidated("Good,"));
        Etapes.Add(new ExploreActionPointsExplanation("you can see the cost of the action here."));
        //Etapes.Add(new ExploreStep("Now click on the Explore button to explore the next area. And get a cookie."));
        Etapes.Add(new ExploreStep("Now click on the Explore button to explore the next area and get a cookie."));

        Etapes.Add(new ReactivateAscFeedbackStep("Well done you genius,"));
        Etapes.Add(new GiveACookieStep("here's your cookie!"));
        //Etapes.Add(new ActionPointsExplanationStep("This action cost you 3 action points. Always keep an eye on them.")); // ==> feedback sur les points d'action
        Etapes.Add(new ActionPointsExplanationStep("This action costed you 3 action points. Always keep an eye on them.")); // ==> feedback sur les points d'action
        //Etapes.Add(new FirstEndTurnStep("You can restore your action points by clicking on the end turn button."));
        Etapes.Add(new FirstEndTurnStep("Ending the turn will restore all your characters's action points"));
        
        //Etapes.Add(new TutoManager.Message(pawnMrResetti, "Ending your turn ends the day,")); // Activate hunger panel + feedback pointer
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Each turn represents a day-night cycle.")); // Activate hunger panel + feedback pointer
        //Etapes.Add(new AddHungerStep("but your characters get hungry in the morning, so be careful!"));
        Etapes.Add(new AddHungerStep("Your characters are getting hungrier every day, so be careful!"));
        //Etapes.Add(new TutoManager.Message(pawnMrResetti, "Remember the cookie?"));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Remember that cookie I gave you ? (I am such a nice person)"));
        //Etapes.Add(new UseAnObjectStep("Eat it by double clicking on it to restore your selected character's hunger.")); // Activate inventory and add a cookie in to be used
        Etapes.Add(new UseAnObjectStep("Eat it by double clicking on it to satisfy your selected character's hunger.")); // Activate inventory and add a cookie in to be used

        //Etapes.Add(new TutoManager.Message(pawnMrResetti, "Great! You should be able to finish this level now. Good luck!"));
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

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            Destroy(GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.GetComponent<RightMouseClickExpected>());
            Destroy(feedbackMouse);
            alreadyPlayed = false;
        }
    }

    public class MovePawnToAnotherTileExplanation : Step
    {
        string str;
        GameObject feedbackMouse;
        public MovePawnToAnotherTileExplanation(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            // show text
            GameObject portal = GameManager.Instance.AllKeepersList[0].CurrentTile.transform.GetChild(0).GetChild(1).GetChild((int)Direction.North_East).gameObject;
            if (portal.GetComponent<RightMouseClickExpected>() == null)
                portal.AddComponent<RightMouseClickExpected>();
            portal.GetComponent<RightMouseClickExpected>().TargetExpected = "Portal";

            if (portal.GetComponent<GlowObjectCmd>() == null)
                portal.AddComponent<GlowObjectCmd>();
            portal.transform.parent.gameObject.SetActive(true);

            portal.GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(true);
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            GameObject portal = GameManager.Instance.AllKeepersList[0].CurrentTile.transform.GetChild(0).GetChild(1).GetChild((int)Direction.North_East).gameObject;
            portal.GetComponent<GlowObjectCmd>().UpdateColor(false);
            if (portal.GetComponent<RightMouseClickExpected>() != null)
                Destroy(portal.GetComponent<RightMouseClickExpected>());
            if (portal.GetComponent<GlowObjectCmd>() != null)
                Destroy(portal.GetComponent<GlowObjectCmd>());

            Destroy(feedbackMouse);

            alreadyPlayed = false;
        }
    }

    public class ActionPointsExplanationStep : Step
    {
        string str;
        GameObject feedback;
        GameObject feedbackCircle;
        public UseAnObjectStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().SelectedInventoryPanel.gameObject.SetActive(true); // Inventory

            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().SelectedInventoryPanel.transform.position + Vector3.up * (100 * (Screen.height/1080.0f));
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 250.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, 90);
            }

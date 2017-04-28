using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Behaviour;

public class SeqFirstMove : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;

    [Header("Hidden in Tuto")]
    public GameObject selectedKeepersPanel;
    public GameObject shortcutBtn;
    public GameObject endTurnBtn;


    //public GameObject SpawnPointer2()
    //{
    //    GameObject pointer = GameObject.Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, endTurnBtn.transform, false);

    //    pointer.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteTutoHighlightCercle;
    //    pointer.transform.SetParent(endTurnBtn.transform.parent);
    //    pointer.transform.SetAsFirstSibling();
    //    pointer.AddComponent<UIPointerOpacityTingling>();
    //    pointer.SetActive(false);
    //    return pointer;
    //}

    //public class Activation : Etape
    //{
    //    GameObject goActivable;
    //    bool active;
    //    Sprite sprite;
    //    public Activation(GameObject _goActivable, Sprite _sprite, bool _active)
    //    {
    //        goActivable = _goActivable;
    //        active = _active;
    //        sprite = _sprite;
    //        step = Activation_fct(0.0f);
    //    }
    //    public IEnumerator Activation_fct(float delayTime)
    //    {
    //        yield return new WaitForSeconds(delayTime);
    //        goActivable.SetActive(active);
    //        goActivable.GetComponent<Image>().sprite = sprite;
    //        TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
    //        alreadyPlayed = true;
    //    }
    //}
    public class ExploreActionPointsExplanation : Step
    {
        string str;
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

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            if (GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().transform.GetChild(0).gameObject.GetComponent<ThrowDiceButtonFeedback>() != null)
                Destroy(GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().transform.GetChild(0).gameObject.GetComponent<ThrowDiceButtonFeedback>());

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

            //TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
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

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            Destroy(GameManager.Instance.AllKeepersList[0].gameObject.GetComponent<MouseClickedOnIngameElt>());
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
            if (GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.GetComponent<RightMouseClickExpected>() == null)
                GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.AddComponent<RightMouseClickExpected>();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            Destroy(GameManager.Instance.AllKeepersList[0].CurrentTile.gameObject.GetComponent<RightMouseClickExpected>());
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
            GameObject actionPoints = ((SeqFirstMove)TutoManager.s_instance.PlayingSequence).selectedKeepersPanel.transform.GetChild(0).GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.ActionPoints).gameObject;

            if (feedbackPointer == null)
            {
                feedbackPointer = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedbackPointer.GetComponent<FlecheQuiBouge>().PointToPoint = actionPoints.transform.GetChild(2).position;
                feedbackPointer.GetComponent<FlecheQuiBouge>().distanceOffset = 30.0f;

                feedbackPointer.transform.localEulerAngles = new Vector3(0, 0, -80);
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
            GameObject hungerPanel = ((SeqFirstMove)TutoManager.s_instance.PlayingSequence).selectedKeepersPanel.transform.GetChild(0).GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.Hunger).gameObject;
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
        GameObject feedback;
        public UseAnObjectStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            // Add a cookie in inventory with highlight sprite
            // show inventory
            // highlight the cookie
            // Insantiate ItemUI, sprite cookie, 
            ItemContainer cookie = new ItemContainer(GameManager.Instance.ItemDataBase.getItemById("thecookie"), 1);
            // Build the cookie item
            InventoryManager.AddItemToInventory(GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().Items, cookie);
            GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().UpdateInventories();
            GameManager.Instance.AllKeepersList[0].GetComponent<Inventory>().SelectedInventoryPanel.gameObject.SetActive(true); // Inventory

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    //public class LootAt : Etape
    //{
    //    GameObject mrresetti;
    //    GameObject pointer;
    //    GameObject gobtn;
    //    AnimationClip turnLeft;
    //    string str;

    //    public LootAt(GameObject _mrresetti, GameObject _pointer, GameObject _btnToHightlight, AnimationClip _turnLeft)
    //    {
    //        mrresetti = _mrresetti;
    //        gobtn = _btnToHightlight;
    //        turnLeft = _turnLeft;
    //        pointer = _pointer;
    //        pointer.transform.localScale = Vector3.one;
    //        pointer.transform.position = gobtn.transform.position;
    //        RectTransform rt = _btnToHightlight.GetComponent(typeof(RectTransform)) as RectTransform;
    //        float newX = _btnToHightlight.GetComponent<RectTransform>().sizeDelta.x + 70;
    //        float newY = _btnToHightlight.GetComponent<RectTransform>().sizeDelta.y + 70;
    //        pointer.GetComponent<RectTransform>().sizeDelta = new Vector2(newX, newY);
    //        pointer.SetActive(false);
    //        step = LootAt_fct(0.5f);
    //        str = "Regardez la bas, essayez de cliquer dessus.";
    //    }
    //    public IEnumerator LootAt_fct(float delayTime)
    //    {
    //        yield return new WaitForSeconds(delayTime);

    //        pointer.SetActive(true);
    //        mrresetti.GetComponentInChildren<Animator>().SetTrigger("turnLeft");
    //        yield return new WaitForSeconds(turnLeft.length);
    //        yield return TutoManager.s_instance.EcrireMessage(str);
    //        gobtn.gameObject.AddComponent<MouseClickExpected>();
    //        TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;

    //        // Clean
    //        yield return new WaitForSeconds(0.5f);
    //        //pointer.SetActive(false);
    //        //gobtn.transform.localScale = Vector3.one;
    //        //Destroy(pointer);
    //        alreadyPlayed = true;
    //    }
    //}

    //public class UnLookAt : Etape
    //{
    //    GameObject mrresetti;
    //    GameObject pointer;
    //    AnimationClip jump;
    //    GameObject btnToUnHightlight;
    //    string str;

    //    public UnLookAt(GameObject _mrresetti, GameObject _pointer, GameObject _btnToUnHightlight, AnimationClip _jumpAnimationClip)
    //    {
    //        mrresetti = _mrresetti;
    //        jump = _jumpAnimationClip;
    //        pointer = _pointer;
    //        btnToUnHightlight = _btnToUnHightlight;
    //        step = UnLookAtAt_fct(0.5f);
    //    }
    //    public IEnumerator UnLookAtAt_fct(float delayTime)
    //    {
    //        yield return new WaitForSeconds(delayTime);
    //        pointer.SetActive(false);
    //        mrresetti.GetComponentInChildren<Animator>().SetTrigger("jumpArround");
    //        yield return new WaitForSeconds(jump.length);
    //        TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
    //        btnToUnHightlight.GetComponent<MouseClickExpected>().enabled = false;

    //        // Clean
    //        //pointer.SetActive(false);
    //        //gobtn.transform.localScale = Vector3.one;
    //        //Destroy(pointer);
    //        alreadyPlayed = true;
    //    }
    //}


    //public class DeplacerCamera : Etape
    //{
    //    GameObject mrresetti;
    //    Tile where;
    //    public DeplacerCamera(GameObject _mrresetti, Tile _where)
    //    {
    //        mrresetti = _mrresetti;
    //        where = _where;
    //        step = DeplacerCamera_fct(0.5f);
    //    }

    //    public IEnumerator DeplacerCamera_fct(float delayTime)
    //    {
    //        yield return new WaitForSeconds(delayTime);
    //        GameManager.Instance.CameraManagerReference.UpdateCameraPosition(where);
    //        yield return new WaitForSeconds(delayTime);
    //        TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForInput;
    //        alreadyPlayed = true;
    //    }
    //}


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
        Etapes.Add(new SelectCharacterStep("First select the girl by clicking on her."));
        Etapes.Add(new MovePawnOnTileStep("To interact with the world, you have to use the right click. Try to move the girl."));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "You can also interact with everything glowing in the world, like this bridge over here.")); // => click expected on bridge
        Etapes.Add(new ExploreActionPointsExplanation("Good, you can see the cost of the action here."));
        Etapes.Add(new ExploreStep("Now click on the Explore button to explore the next area. And get a cookie."));

        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Well done you genius, here's your cookie!"));
        Etapes.Add(new ActionPointsExplanationStep("This action cost you 3 action points. Always keep an eye on them.")); // ==> feedback sur les points d'action
        Etapes.Add(new FirstEndTurnStep("You can restore your action points by clicking on the end turn button."));

        Etapes.Add(new AddHungerStep("Ending your turn starves your characters, so be careful!")); // Activate hunger panel + feedback pointer
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Remember the cookie?"));
        Etapes.Add(new UseAnObjectStep("Eat it by double clicking on it to restore your selected character's hunger.")); // Activate inventory and add a cookie in to be used

        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Great! You should be able to finish this level now. Good luck!"));
    }

    public override void End()
    {
        //if ( pointer != null ) pointer.SetActive(false);
        //pointer2.SetActive(false);


        // Reactivate all UI
        selectedKeepersPanel.SetActive(true);
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.PlayingSequence = null;
        //endTurnBtn.SetActive(true);
        //shortcutBtn.SetActive(true);

        //Destroy(pointer);

        //Destroy(pointer2);
        //endTurnBtn.gameObject.GetComponent<MouseClickExpected>().enabled = false;
        //shortcutBtn.transform.localScale = Vector3.one;
        //endTurnBtn.transform.localScale = Vector3.one;
        //this.Etapes[Etapes.Count - 1].step.Invoke();
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class SeqTutoCombat : Sequence
{
    public GameObject uiBattle;
    public GameObject rollDiceButton;
    public GameObject charactersStock;
    public GameObject skillPanel;
    public List<SkillBattle> previousCharacterSkills;

    public class RollDiceButtonExplain : Step
    {
        string str;
        public RollDiceButtonExplain(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
            Button rdButton = seqTutoCombat.rollDiceButton.GetComponentInChildren<Button>();
            rdButton.gameObject.AddComponent<MouseClickExpected>();
            rdButton.GetComponent<ThrowDiceButtonFeedback>().enabled = true;
            rdButton.interactable = true;

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
        }

        public override void Reverse()
        {
            alreadyPlayed = false;
        }
    }

    public class ShowCharactersStocks : Step
    {
        string str;
        GameObject feedback;
        public ShowCharactersStocks(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = seqTutoCombat.charactersStock.transform.position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 20.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -90);
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

    public class PawnSelection : Step
    {
        string str;
        public PawnSelection(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (BattleHandler.CurrentBattleKeepers[0].gameObject.GetComponent<MouseClickedOnIngameElt>() == null)
                BattleHandler.CurrentBattleKeepers[0].gameObject.AddComponent<MouseClickedOnIngameElt>();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            if (BattleHandler.CurrentBattleKeepers[0].gameObject.GetComponent<MouseClickedOnIngameElt>() != null)
                Destroy(BattleHandler.CurrentBattleKeepers[0].gameObject.GetComponent<MouseClickedOnIngameElt>());

            alreadyPlayed = false;
        }
    }

    public class StandardAtkExplain : Step
    {
        string str;
        GameObject feedback;
        public StandardAtkExplain(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
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

            if (feedback == null)
            {
                SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = Camera.main.WorldToScreenPoint(GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().transform.position);
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 80.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, 75);
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

    public class StandardAtkShowDice : Step
    {
        string str;
        GameObject feedback;
        public StandardAtkShowDice(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (feedback == null)
            {
                SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = Camera.main.WorldToScreenPoint(TileManager.Instance.DicePositionsOnTile.GetChild(0).transform.position);
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 80.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, 45);
            }
            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            Button skillButton = GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>()[1];
            skillButton.interactable = true;
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    public class SkillButtonExplain : Step
    {
        string str;
        GameObject feedback;
        public SkillButtonExplain(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            Button skillButton = GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>()[1];

            if (feedback == null)
            {
                SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = Camera.main.WorldToScreenPoint(skillButton.transform.position);
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 80.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -60);
            }

            skillButton.gameObject.AddComponent<MouseClickExpected>();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
        }

        public override void Reverse()
        {
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }
 
    public class SkillCostExplain : Step
    {
        string str;
        GameObject feedback;
        public SkillCostExplain(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();

            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = seqTutoCombat.skillPanel.transform.GetChild(0).GetChild(2).position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 40.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -60);
            }

            seqTutoCombat.skillPanel.GetComponentInChildren<Button>().interactable = false;

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    public class SkillSelectionStep : Step
    {
        string str;
        public SkillSelectionStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
            Button skillButton = seqTutoCombat.skillPanel.GetComponentInChildren<Button>();
            skillButton.interactable = true;

            if (skillButton.gameObject.GetComponent<MouseClickExpected>() == null)
                skillButton.gameObject.AddComponent<MouseClickExpected>();

            Behaviour.Monster monster = BattleHandler.CurrentBattleKeepers[0].CurrentTile.GetComponentInChildren<Behaviour.Monster>();
            if (monster.gameObject.GetComponent<MouseClickedOnIngameElt>() == null)
                monster.gameObject.AddComponent<MouseClickedOnIngameElt>();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForSkillUse;
        }

        public override void Reverse()
        {
            SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
            Button skillButton = seqTutoCombat.skillPanel.GetComponentInChildren<Button>();
            if (skillButton.gameObject.GetComponent<MouseClickExpected>() != null)
                Destroy(skillButton.gameObject.GetComponent<MouseClickExpected>());

            Behaviour.Monster monster = BattleHandler.CurrentBattleKeepers[0].CurrentTile.GetComponentInChildren<Behaviour.Monster>();
            if (monster.gameObject.GetComponent<MouseClickedOnIngameElt>() != null)
                Destroy(monster.gameObject.GetComponent<MouseClickedOnIngameElt>());

            alreadyPlayed = false;
        }
    }

    public class MonstersTurnStep : Step
    {
        string str;
        public MonstersTurnStep(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            // Give back the character his skills
            BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills.Clear();
            BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills = TutoManager.s_instance.GetComponent<SeqTutoCombat>().previousCharacterSkills;
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().TutoReloadSkillPanel(BattleHandler.CurrentBattleKeepers[0]);


            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForExternalEvent;
            TutoManager.EnablePreviousButton(false);
            TutoManager.EnableNextButton(true);
        }

        public override void Reverse()
        {
            BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().EndSkillProcess();
            TutoManager.s_instance.TutoPanelInstance.SetActive(false);
            
            alreadyPlayed = false;
        }
    }

    public override void Init()
    {
        base.Init();

        SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
        foreach (Button b in seqTutoCombat.uiBattle.GetComponentsInChildren<Button>())
            b.interactable = false;

        seqTutoCombat.rollDiceButton.GetComponentInChildren<ThrowDiceButtonFeedback>().enabled = false;

        previousCharacterSkills = BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills;
        BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills.Clear();
        SkillBattle tutoSkill = new SkillBattle();
        tutoSkill.Damage = 20;
        tutoSkill.Description = "Comes with great responsability.";
        tutoSkill.SkillName = "Great Power";
        tutoSkill.TargetType = TargetType.Foe;
        tutoSkill.SkillUser = BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>();
        tutoSkill.Cost = new List<Face>();
        tutoSkill.Cost.Add(new Face(FaceType.Physical, 0));
        tutoSkill.Cost.Add(new Face(FaceType.Magical, 0));
        tutoSkill.Cost.Add(new Face(FaceType.Defensive, 0));
        BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills.Add(tutoSkill);
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().TutoReloadSkillPanel(BattleHandler.CurrentBattleKeepers[0]);

        //pointer2 = SpawnPointer2();
        Etapes = new List<Step>();

        // Content
        Etapes.Add(new TutoManager.Message(null, "You have to roll dice to defeat monsters in this world"));
        Etapes.Add(new RollDiceButtonExplain("Click on the Dice button here to roll the dice of your characters."));
        Etapes.Add(new ShowCharactersStocks("Good. The value of each dice is added to the character's stocks."));
        Etapes.Add(new ShowCharactersStocks("These stocks allow you to perform skills when you have enough of each required symbol."));
        Etapes.Add(new PawnSelection("Select a pawn by clicking on it to perform an action."));
        Etapes.Add(new StandardAtkExplain("This button allows you to perform a standard attack based on your current roll,"));
        Etapes.Add(new StandardAtkShowDice("the more swords you have on the dice, the more damage you'll do."));
        Etapes.Add(new SkillButtonExplain("This button allows you to use a skill. Try it!"));
        Etapes.Add(new SkillCostExplain("This is the skill cost, you need at least all the required symbols to use it."));
        Etapes.Add(new SkillSelectionStep("Now select a skill and click on the monster to unleash your power!"));
        Etapes.Add(new TutoManager.Message(null, "Great!"));
        Etapes.Add(new MonstersTurnStep("When all your characters have played, it's the monsters turn.")); // ==> monsters play their turn when clicking on next arrow
        Etapes.Add(new TutoManager.Message(null, "Now use what you learned to finish the battle, good luck!")); // ==> show this step when turn reset, no previous arrow
    }

    public override void End()
    {
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.PlayingSequence = null;
        BattleHandler.PendingSkill = null;
        TutoManager.s_instance.GetComponent<SeqTutoCombat>().AlreadyPlayed = true;
    }
}

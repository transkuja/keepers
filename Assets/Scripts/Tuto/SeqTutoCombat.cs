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
            if (rdButton.gameObject.GetComponent<MouseClickExpected>() == null)
                rdButton.gameObject.AddComponent<MouseClickExpected>();
            rdButton.GetComponent<ThrowDiceButtonFeedback>().enabled = true;
            rdButton.interactable = true;

            rdButton.transform.parent.SetParent(GameManager.Instance.Ui.transform.GetChild(0));
            
            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
            TutoManager.EnablePreviousButton(false);
        }

        public override void Reverse()
        {
            SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
            Button rdButton = seqTutoCombat.rollDiceButton.GetComponentInChildren<Button>();
            if (rdButton.gameObject.GetComponent<MouseClickExpected>() != null)
                Destroy(rdButton.gameObject.GetComponent<MouseClickExpected>());

            rdButton.transform.parent.SetParent(GameManager.Instance.GetBattleUI.transform);
            rdButton.transform.parent.SetSiblingIndex(1);
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
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = seqTutoCombat.charactersStock.transform.GetChild(2).GetChild(0).transform.position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 10.0f;

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
        GameObject feedbackMouse;
        Button[] characterPanelButtons;
        public PawnSelection(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (BattleHandler.CurrentBattleKeepers[0].gameObject.GetComponent<MouseClickedOnIngameElt>() == null)
                BattleHandler.CurrentBattleKeepers[0].gameObject.AddComponent<MouseClickedOnIngameElt>();

            BattleHandler.CurrentBattleKeepers[0].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(true);
            BattleHandler.CurrentBattleKeepers[0].GetComponent<GlowObjectCmd>().enabled = true;

            characterPanelButtons = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().CharactersPanel.transform.GetChild(0).GetComponentsInChildren<Button>();
            for (int i = 0; i < characterPanelButtons.Length; i++)
            {
                characterPanelButtons[i].interactable = true;
                characterPanelButtons[i].transform.gameObject.AddComponent<MouseClickExpected>();
            }

            if (feedbackMouse == null)
            {
                feedbackMouse = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, GameManager.Instance.Ui.transform.GetChild(0));
                feedbackMouse.transform.position = Camera.main.WorldToScreenPoint(BattleHandler.CurrentBattleKeepers[0].GetComponent<Interactable>().Feedback.position) + Vector3.up * (50 * (Screen.height / 1080.0f)) + Vector3.right * (50 * (Screen.width / 1920.0f));
                feedbackMouse.transform.localScale = Vector3.one;
                feedbackMouse.AddComponent<ShowClickIsExpected>();
                feedbackMouse.GetComponent<ShowClickIsExpected>().IsLeftClick = true;
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickInGame;
        }

        public override void Reverse()
        {
            if (BattleHandler.CurrentBattleKeepers[0].gameObject.GetComponent<MouseClickedOnIngameElt>() != null)
                Destroy(BattleHandler.CurrentBattleKeepers[0].gameObject.GetComponent<MouseClickedOnIngameElt>());
            Destroy(feedbackMouse);
            GameManager.Instance.AllKeepersList[0].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(false);
            for (int i = 0; i < characterPanelButtons.Length; i++)
            {
                characterPanelButtons[i].interactable = false;
                Destroy(characterPanelButtons[i].transform.gameObject.GetComponent<MouseClickExpected>());
            }

            alreadyPlayed = false;
        }
    }

    public class StandardAtkExplain : Step
    {
        string str;
        public StandardAtkExplain(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
            foreach (Button b in seqTutoCombat.skillPanel.GetComponentsInChildren<Button>())
                b.interactable = false;

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
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
                //SeqTutoCombat seqTutoCombat = TutoManager.s_instance.GetComponent<SeqTutoCombat>();
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = Camera.main.WorldToScreenPoint(skillButton.transform.position);
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 100.0f;

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

            foreach (Button b in seqTutoCombat.skillPanel.GetComponentsInChildren<Button>())
                b.interactable = false;

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
        public GameObject feedback;
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

            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = seqTutoCombat.skillPanel.transform.GetChild(0).GetChild(0).position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 40.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -60);
            }

            if (skillButton.gameObject.GetComponent<MouseClickExpected>() == null)
                skillButton.gameObject.AddComponent<MouseClickExpected>();

            Behaviour.Monster monster = BattleHandler.CurrentBattleKeepers[0].CurrentTile.GetComponentInChildren<Behaviour.Monster>();
            if (monster.gameObject.GetComponent<MouseClickedOnIngameElt>() == null)
                monster.gameObject.AddComponent<MouseClickedOnIngameElt>();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForSkillUse;
            TutoManager.EnablePreviousButton(false);
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

            foreach (Button b in seqTutoCombat.skillPanel.GetComponentsInChildren<Button>())
                b.interactable = true;

            Destroy(feedback);
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
            BattleHandler.CurrentBattleKeepers[0].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(false);
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
            if (BattleHandler.CurrentBattleKeepers.Length == 1 && !BattleHandler.isPrisonerOnTile)
            {
                foreach (PawnInstance pi in BattleHandler.CurrentBattleMonsters)
                {
                    if (pi.GetComponent<Behaviour.Fighter>().PendingDamage != 0)
                        pi.GetComponent<Behaviour.Fighter>().EndSkillProcess();
                }
            }
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

        previousCharacterSkills = new List<SkillBattle>();
        for (int i = 0; i < BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills.Count; i++)
            previousCharacterSkills.Add(new SkillBattle(BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills[i]));
        BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills.Clear();
        SkillBattle tutoSkill = new SkillBattle();
        tutoSkill.Damage = 20;
        tutoSkill.Description = "Comes with great responsability.";
        tutoSkill.SkillName = "Great Power";
        tutoSkill.TargetType = TargetType.FoeSingle;
        tutoSkill.SkillUser = BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>();
        tutoSkill.Cost = new List<Face>();
        tutoSkill.Cost.Add(new Face(FaceType.Physical, 0));
        tutoSkill.Cost.Add(new Face(FaceType.Magical, 0));
        tutoSkill.Cost.Add(new Face(FaceType.Defensive, 0));
        BattleHandler.CurrentBattleKeepers[0].GetComponent<Behaviour.Fighter>().BattleSkills.Add(tutoSkill);
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().TutoReloadSkillPanel(BattleHandler.CurrentBattleKeepers[0]);

        Etapes = new List<Step>();

        // Content
        Etapes.Add(new TutoManager.Message(null, Translater.TutoText("SeqTutoCombat", 0)));
        Etapes.Add(new RollDiceButtonExplain(Translater.TutoText("SeqTutoCombat", 1)));
        Etapes.Add(new ShowCharactersStocks(Translater.TutoText("SeqTutoCombat", 2)));
        Etapes.Add(new ShowCharactersStocks(Translater.TutoText("SeqTutoCombat", 3)));
        Etapes.Add(new PawnSelection(Translater.TutoText("SeqTutoCombat", 4)));
        Etapes.Add(new StandardAtkExplain(Translater.TutoText("SeqTutoCombat", 5)));
        Etapes.Add(new SkillCostExplain(Translater.TutoText("SeqTutoCombat", 6)));
        Etapes.Add(new SkillSelectionStep(Translater.TutoText("SeqTutoCombat", 7)));

        Etapes.Add(new TutoManager.Message(null, Translater.TutoText("SeqTutoCombat", 8)));
        if (BattleHandler.CurrentBattleKeepers.Length > 1 || BattleHandler.isPrisonerOnTile)
        {
            Etapes.Add(new TutoManager.Message(null, Translater.TutoText("SeqTutoCombat", 9)));
            Etapes.Add(new MonstersTurnStep(Translater.TutoText("SeqTutoCombat", 10)));
        }
        else
        {
            Etapes.Add(new MonstersTurnStep(Translater.TutoText("SeqTutoCombat", 11)));
        }

        Etapes.Add(new TutoManager.Message(null, Translater.TutoText("SeqTutoCombat", 12)));
    }

    public override void End()
    {
        base.End();
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        BattleHandler.CurrentBattleKeepers[0].GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(false);
        if (position >= Etapes.Count)
        {
            Debug.Log("here");
            BattleHandler.ResetBattleHandlerForTuto();
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().ChangeState(UIBattleState.WaitForDiceThrow);
        }

        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            GameManager.Instance.AllKeepersList[i].GetComponent<Behaviour.Mortal>().SelectedHPUI.SetActive(true);
        }

        TutoManager.s_instance.GetComponent<SeqTutoCombat>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

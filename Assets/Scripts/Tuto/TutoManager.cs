﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Behaviour;

//public enum SequenceState { Idle, ReadyForNext, WaitingForInput, WaitingForClickUI, End };

public enum SequenceState { Idle, ReadyForNext, WaitingForClickUI, WaitingForClickInGame, WaitingForSkillUse, WaitingForExternalEvent };
public delegate void StepFunction();

public abstract class Step
{
    public bool alreadyPlayed = false;
    public StepFunction stepFunction;
    public bool isReachableByClickOnPrevious = true;

    public virtual void Reverse()
    {

    }
}

public class TutoManager : MonoBehaviour {

    public static TutoManager s_instance = null;
    public bool enableTuto = false;
    private Sequence playingSequence;
    private List<Sequence> sequences;

    private static bool mouseClicked;
    private static bool secondMouseClicked;
    private static bool externalEventReached;

    public GameObject tutoPanel;
    public GameObject uiPointer;

    private GameObject tutoPanelInstance;

    private GameState stateBeforeTutoStarts;

    [Header("UI blocked during all sequences")]
    public GameObject endTurnButton;
    public GameObject shortcutButton;

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        mouseClicked = false;
        secondMouseClicked = false;
        externalEventReached = false;
        if (s_instance.enableTuto && s_instance.GetComponent<SeqFirstMove>().AlreadyPlayed == false)
        {
            InitTutoScene();
        }
        else if (s_instance.enableTuto && s_instance.GetComponent<SeqFirstMove>().AlreadyPlayed == true && s_instance.GetComponent<SeqMultiCharacters>().AlreadyPlayed == false)
        {
            InitSecondLevel();
        }
    }

    void InitTutoScene()
    {
        SeqFirstMove seqIntro = s_instance.GetComponent<SeqFirstMove>();
        seqIntro.selectedKeepersPanel.SetActive(true);
        seqIntro.endTurnBtn.SetActive(false);
        seqIntro.shortcutBtn.SetActive(false);

        Transform selectedKeepersFirstCharUI = seqIntro.selectedKeepersPanel.transform.GetChild(0);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleLeft).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleRight).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.Mortal).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.Hunger).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.MentalHealth).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(1).gameObject.SetActive(false); // Equipements
        selectedKeepersFirstCharUI.GetChild(2).gameObject.SetActive(false); // Inventory

        GameManager.Instance.AllKeepersList[0].GetComponent<Keeper>().GoListCharacterFollowing.Add(GameManager.Instance.PrisonerInstance.gameObject);
        // No mental health for first sequence
        Destroy(GameManager.Instance.AllKeepersList[0].GetComponent<MentalHealthHandler>());
        // No hunger for Ashley on first sequence
        Destroy(GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>());

        // Deactivate feedback above head at start, reactivate at the end turn button step
        GameManager.Instance.AllKeepersList[0].transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        GameManager.Instance.PrisonerInstance.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);

        GameManager.Instance.PrisonerInstance.GetComponent<Escortable>().enabled = false;
        GameManager.Instance.PrisonerInstance.GetComponent<Interactable>().Interactions = new InteractionImplementer();
        Destroy(GameManager.Instance.PrisonerInstance.GetComponent<GlowObjectCmd>());
    }

    void InitSecondLevel()
    {
        SeqFirstMove seqIntro = s_instance.GetComponent<SeqFirstMove>();
        seqIntro.shortcutBtn.SetActive(false);

        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            Transform selectedKeepersFirstCharUI = seqIntro.selectedKeepersPanel.transform.GetChild(i);
            selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.Mortal).gameObject.SetActive(false);
            selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.MentalHealth).gameObject.SetActive(false);
        }

        GameManager.Instance.AllKeepersList[0].GetComponent<Keeper>().GoListCharacterFollowing.Add(GameManager.Instance.PrisonerInstance.gameObject);
        Escortable escortComponent = GameManager.Instance.PrisonerInstance.GetComponent<Escortable>();
        escortComponent.escort = GameManager.Instance.AllKeepersList[0].GetComponent<Keeper>();
        escortComponent.IsEscorted = true;
        escortComponent.ActivateIconNearEscort();
        
    }

    void Update()
    {
        if (playingSequence != null)
        {
            switch (playingSequence.CurrentState)
            {
                case SequenceState.Idle:
                    break;
                case SequenceState.ReadyForNext:
                    s_instance.PlayNextStep();
                    break;
                case SequenceState.WaitingForClickInGame:
                    if (mouseClicked)
                    {
                        mouseClicked = false;
                        s_instance.PlayNextStep();
                    }
                    break;
                case SequenceState.WaitingForClickUI:
                    if (mouseClicked)
                    {
                        mouseClicked = false;
                        s_instance.PlayNextStep();
                    }
                    break;
                case SequenceState.WaitingForSkillUse:
                    if (mouseClicked && secondMouseClicked)
                    {
                        mouseClicked = false;
                        secondMouseClicked = false;
                        s_instance.PlayNextStep();
                    }
                    break;
                case SequenceState.WaitingForExternalEvent:
                    if (externalEventReached)
                    {
                        externalEventReached = false;
                        s_instance.PlayNextStep();
                    }
                    break;

            }
        }
    }

    public Sequence PlayingSequence
    {
        get
        {
            return playingSequence;
        }

        set
        {
            playingSequence = value;
            if (playingSequence != null)
            {
                stateBeforeTutoStarts = GameManager.Instance.CurrentState;
                GameManager.Instance.CurrentState = GameState.InTuto;          
            } else
            {
                GameManager.Instance.CurrentState = stateBeforeTutoStarts;
                //TutoManager.s_instance.StopAllCoroutines();
            }
        }
    }

    public static bool MouseClicked
    {
        get
        {
            return mouseClicked;
        }

        set
        {
            mouseClicked = value;
        }
    }

    public GameObject TutoPanelInstance
    {
        get
        {
            return tutoPanelInstance;
        }

        set
        {
            tutoPanelInstance = value;
        }
    }

    public GameState StateBeforeTutoStarts
    {
        get
        {
            return stateBeforeTutoStarts;
        }

        set
        {
            stateBeforeTutoStarts = value;
        }
    }

    public static bool SecondMouseClicked
    {
        get
        {
            return secondMouseClicked;
        }

        set
        {
            secondMouseClicked = value;
        }
    }

    public GameObject TutoPanelInstance1
    {
        get
        {
            return tutoPanelInstance;
        }

        set
        {
            tutoPanelInstance = value;
        }
    }

    public void playSequence(Sequence seq)
    {
        s_instance.PlayingSequence = seq;
        s_instance.playingSequence.Init();
        s_instance.playingSequence.MoveNext();
        s_instance.playingSequence.Play();
    }
    
    public void EcrireMessage(string str)
    {
        if (tutoPanelInstance == null)
        {
            tutoPanelInstance = Instantiate(tutoPanel, GameManager.Instance.Ui.transform.GetChild(0), false);
            tutoPanelInstance.transform.localScale = Vector3.one;
        }

        tutoPanelInstance.SetActive(false);
        //yield return new WaitForSeconds(0.5f);
        tutoPanelInstance.GetComponentInChildren<Text>().text = str;
        tutoPanelInstance.SetActive(true);
        return;
    }

    public class Message : Step
    {
        GameObject mrresetti;
        string str;
        public Message(GameObject _mrresetti, string _str)
        {
            mrresetti = _mrresetti;
            stepFunction += Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            s_instance.EcrireMessage(str);

            EnableNextButton();
            if (s_instance.PlayingSequence.isPreviousStepReachable())
            {
                EnablePreviousButton();
            }
            else
            {
                EnablePreviousButton(false);
            }
            s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
            return;
        }
    }

    public static void EnablePreviousButton(bool _enable = true)
    {
        if (s_instance.tutoPanelInstance != null)
        {
            s_instance.tutoPanelInstance.transform.GetChild(2).gameObject.SetActive(_enable);
            if (_enable)
            {
                s_instance.tutoPanelInstance.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                s_instance.tutoPanelInstance.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => s_instance.PlayPreviousStep());
            }
        }
    }

    public static void EnableNextButton(bool _enable = true)
    {
        s_instance.tutoPanelInstance.transform.GetChild(3).gameObject.SetActive(_enable);
        if (_enable)
        {
            s_instance.tutoPanelInstance.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
            s_instance.tutoPanelInstance.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => s_instance.PlayNextStep());
        }
    }

    public void PlayNextStep()
    {
        if(s_instance.playingSequence != null)
        {
            if (s_instance.playingSequence.MoveNext())
            {
                if (s_instance.playingSequence.CurrentState != SequenceState.WaitingForExternalEvent)
                    s_instance.playingSequence.Play();
            }
            // End sequence
            else
            {
                s_instance.playingSequence.End();
            }
        }
    }

    public void PlayPreviousStep()
    {
        if (s_instance.playingSequence != null)
        {
            if (s_instance.playingSequence.MovePrevious())
            {
                s_instance.playingSequence.Play();
            }
        }
    }

    #region MmeResetti
    public class Spawn : Step
    {
        GameObject mrresetti;
        AnimationClip jump;

        public Spawn(GameObject _mrresetti, AnimationClip _jump)
        {
            mrresetti = _mrresetti;
            jump = _jump;
            stepFunction += Spawn_fct;
            isReachableByClickOnPrevious = false;
        }
        public void Spawn_fct()
        {
            s_instance.playAppearenceFeedback(mrresetti.GetComponent<PawnInstance>());
            //yield return new WaitForSeconds(delayTime);
            mrresetti.SetActive(true);
            mrresetti.GetComponentInChildren<Animator>().SetTrigger("jumpArround");

            // TODO ? 
            //yield return new WaitForSeconds(jump.length);
            s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
            alreadyPlayed = true;
            return;
        }
    }
 
    public static void UnSpawn(GameObject pawnMrResetti)
    {
        s_instance.playAppearenceFeedback(pawnMrResetti.GetComponent<PawnInstance>());
        Destroy(pawnMrResetti, 0.2f);
    }

    public void playAppearenceFeedback(PawnInstance pi)
    {
        if (pi.GetComponent<Behaviour.Mortal>().DeathParticles != null)
        {
            ParticleSystem ps = GameObject.Instantiate(pi.GetComponent<Behaviour.Mortal>().DeathParticles, pi.transform.parent);
            ps.transform.position = pi.transform.position;
            ps.Play();
            GameObject.Destroy(ps.gameObject, ps.main.duration);
        }
    }

    public GameObject SpawnMmeResetti(Vector3 where)
    {
        GameObject pawnMrResetti = GameManager.Instance.PawnDataBase.CreatePawn("mrresetti", where, Quaternion.identity, null);
        pawnMrResetti.SetActive(false);

        return pawnMrResetti;
    }
    #endregion

}

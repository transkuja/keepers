using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum SequenceState { Idle, ReadyForNext, WaitingForInput, WaitingForClickUI, End };

public abstract class Etape
{
    public bool alreadyPlayed = false;
    public IEnumerator step;

    public void Skip()
    {
        //Tuto.s_instance.StopCoroutine(step);
        TutoManager.s_instance.StopAllCoroutines();
        TutoManager.s_instance.PlayingSequence.Current.overstep();
    }

    public abstract void overstep();

}

public class TutoManager : MonoBehaviour {

    public static TutoManager s_instance = null;
    public bool enableTuto = false;
    private Sequence playingSequence;
    private List<Sequence> sequences;

    private static bool mouseCLicked;
    internal bool desactivateCamera;
    internal bool desactivateControls;

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        if (s_instance.enableTuto)
        {
            playSequence(GetComponent<SeqIntro>());
            desactivateCamera = true;
            desactivateControls = true;
        }
    }

    void Update()
    {
        if (playingSequence != null)
        {
            if (playingSequence.CurrentState == SequenceState.WaitingForClickUI)
            {
                if (MouseCLicked == true)
                {
                    MouseCLicked = false;
                    playingSequence.CurrentState = SequenceState.Idle;
                    playingSequence.MoveNext();
                    playingSequence.Play();
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                switch (playingSequence.CurrentState)
                {
                    case SequenceState.Idle:
                        playingSequence.Current.Skip();
                        break;
                    case SequenceState.WaitingForInput:
                        playingSequence.MoveNext();
                        if (playingSequence.isLastSequence())
                        {
                            playingSequence.CurrentState = SequenceState.End;
                        }
                        else
                        {
                            playingSequence.Play();
                            playingSequence.CurrentState = SequenceState.Idle;
                        }
                        break;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                playingSequence.Reset();
            }
            else if (playingSequence.CurrentState == SequenceState.End)
            {
                playingSequence.End();
                playingSequence.AlreadyPlayed = true;
                playingSequence = null;
            }
            else if (playingSequence.CurrentState == SequenceState.ReadyForNext)
            {
                playingSequence.MoveNext();
                if (playingSequence.isLastSequence())
                {
                    playingSequence.CurrentState = SequenceState.End;
                }
                else
                {
                    playingSequence.Play();
                    playingSequence.CurrentState = SequenceState.Idle;
                }
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
        }
    }

    public static bool MouseCLicked
    {
        get
        {
            return mouseCLicked;
        }

        set
        {
            mouseCLicked = value;
        }
    }

    public void playSequence(Sequence seq)
    {
        s_instance.playingSequence = seq;
        s_instance.playingSequence.Init();
        s_instance.playingSequence.Play();
    }

    public void Init()
    {

        s_instance.enableTuto = true;
    }



    public static IEnumerator EcrireMessage(Transform feedback, string str, float delayTime)
    {
        feedback.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        feedback.GetComponentInChildren<Text>().text = string.Empty;
        feedback.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        // Fête du sleep
        for (int i = 0; i < str.Length; i++)
        {
            feedback.GetComponentInChildren<Text>().text += str[i];
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(delayTime);
    }
}

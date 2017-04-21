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
//public abstract class Sequence : MonoBehaviour
//{
//    public bool alreadyPlayed = false;
//    public SequenceState currentState;
//    public List<Etape> _etapes;

//    // Enumerators are positioned before the first element
//    // until the first MoveNext() call.
//    int position = -1;

//    public bool MoveNext()
//    {
//        position++;
//        return (position < _etapes.Count);
//    }

//    public void Reset()
//    {
//        TutoManager.s_instance.StopAllCoroutines();
//        foreach ( Etape etape in _etapes)
//        {
//            if (!etape.alreadyPlayed) // Juste pour gagner un peu de temps sur certains traitement
//                etape.overstep(); // Potentiellement on pourait distingué les etapes avec des types entre elles pour ne pas faire certain traitement
//        }
//        position = -1;
//    }

//    public Etape Current
//    {
//        get
//        {
//            try
//            {
//                return _etapes[position];
//            }
//            catch (IndexOutOfRangeException)
//            {
//                throw new InvalidOperationException();
//            }
//        }
//    }

//    public void Play()
//    {
//        TutoManager.s_instance.StartCoroutine(this.Current.step);
//    }

//    public abstract void Init();
//    public abstract void End();

//    public bool isLastSequence()
//    {
//        return (position == _etapes.Count - 1);
//    }
//}

public class TutoManager : MonoBehaviour {

    public static TutoManager s_instance = null;
    public bool enableTuto = false;
    private Sequence playingSequence;
    private List<Sequence> sequences;

    private static bool mouseCLicked;

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

    void Start () {
        s_instance = this;
        if (enableTuto)
        {
            playSequence(GetComponent<SeqIntro>());
        }
    }

	void Update () {
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
            } else if (playingSequence.CurrentState == SequenceState.ReadyForNext)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sequence : MonoBehaviour
{
    private bool alreadyPlayed = false;
    private SequenceState currentState;
    private List<Step> etapes;

    public int position = -1;

    public bool MoveNext()
    {
        if (position >= 0)
        {
            CurrentStep.Reverse();
            CurrentStep.alreadyPlayed = true;
        }
        position++;
        return (position < etapes.Count);
    }

    public bool MovePrevious()
    {
        CurrentStep.Reverse();
        position--;
        return (position > 0);
    }

    public void Reset()
    {
        position = -1;
    }

    public Step CurrentStep
    {
        get
        {
            return etapes[position];
        }
        set
        {
            etapes[position] = value;
        }
    }

    public bool AlreadyPlayed
    {
        get
        {
            return alreadyPlayed;
        }

        set
        {
            alreadyPlayed = value;
        }
    }

    public SequenceState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
            if (value == SequenceState.WaitingForClickUI || value == SequenceState.WaitingForClickInGame)
            {
                TutoManager.s_instance.PlayingSequence.CurrentStep.isReachableByClickOnPrevious = false;
                TutoManager.EnableNextButton(false);
            }
            else if (value == SequenceState.Idle)
            {
                TutoManager.EnableNextButton();
            }

            if (TutoManager.s_instance.PlayingSequence.isPreviousStepReachable())
            {
                TutoManager.EnablePreviousButton();
            }
            else
            {
                TutoManager.EnablePreviousButton(false);
            }
        }
    }

    public List<Step> Etapes
    {
        get
        {
            return etapes;
        }

        set
        {
            etapes = value;
        }
    }

    public void Play()
    {
        CurrentStep.stepFunction.Invoke();
    }

    public virtual void Init()
    {
        currentState = SequenceState.Idle;
    }
    public abstract void End();

    public bool isLastSequence()
    {
        return (position == etapes.Count - 1);
    }
    public bool isLastMessage()
    {
        int nbrMsg = 0;
        foreach (Step e in etapes)
        {
            if (e.GetType() == typeof(TutoManager.Message))
                nbrMsg++;

        }
        
        return (position == nbrMsg);
    }
    public bool isPreviousStepReachable()
    {
        if (position > 0)
        {
            return etapes[position - 1].isReachableByClickOnPrevious;
        }

        // pas de message
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sequence : MonoBehaviour
{
    private bool alreadyPlayed = false;
    private SequenceState currentState;
    private List<Etape> etapes;

    public int position = -1;

    public bool MoveNext()
    {
        position++;
        return (position < etapes.Count);
    }

    public bool MovePrevious()
    {
        position--;

        return (position > 0);
    }

    public void Reset()
    {
        TutoManager.s_instance.StopAllCoroutines();
        position = -1;
    }

    public Etape Current
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
        }
    }

    public List<Etape> Etapes
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
        TutoManager.s_instance.StartCoroutine(this.Current.step);
    }

    public abstract void Init();
    public abstract void End();

    public bool isLastSequence()
    {
        return (position == etapes.Count - 1);
    }
    public bool isLastMessage()
    {
        int nbrMsg = 0;
        foreach (Etape e in etapes)
        {
            if (e.GetType() == typeof(TutoManager.Message))
                nbrMsg++;

        }
        
        return (position == nbrMsg);
    }
    public bool isFirstMessage()
    {
        int indiceFirstMessage = 0;
        for (int indiceMsg = 0; indiceMsg < etapes.Count; indiceMsg++)
        {
            Etape e = etapes[indiceMsg];
            if (e.GetType() == typeof(TutoManager.Message))
            {
                indiceFirstMessage = indiceMsg;
                break;
            }
    

        }

        if (indiceFirstMessage > 0)
        {
            return (position == indiceFirstMessage);
        }
        // pas de message
        return false;
    }
}

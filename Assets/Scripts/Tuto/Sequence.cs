using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sequence : MonoBehaviour
{
    private bool alreadyPlayed = false;
    private SequenceState currentState;
    private List<Etape> etapes;

    int position = -1;

    public bool MoveNext()
    {
        position++;
        return (position < etapes.Count);
    }

    public void Reset()
    {
        TutoManager.s_instance.StopAllCoroutines();
        foreach (Etape etape in etapes)
        {
            if (!etape.alreadyPlayed) // Juste pour gagner un peu de temps sur certains traitement
                etape.overstep(); // Potentiellement on pourait distingué les etapes avec des types entre elles pour ne pas faire certain traitement
        }
        position = -1;
    }

    public Etape Current
    {
        get
        {
           return etapes[position];
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Sequence : MonoBehaviour
{
    [SerializeField]
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
            if (value == SequenceState.WaitingForClickUI || value == SequenceState.WaitingForClickInGame 
                || value == SequenceState.WaitingForSkillUse || value == SequenceState.WaitingForExternalEvent)
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
        Button[] shortcutButt = TutoManager.s_instance.shortcutButton.GetComponentsInChildren<Button>();
        ColorBlock cb;

        foreach (Button b in shortcutButt)
        {
            cb = b.colors;
            cb.disabledColor = Color.white;
            b.colors = cb;
            b.interactable = false;
        }

        Button endTurnButt = TutoManager.s_instance.endTurnButton.GetComponentInChildren<Button>();

        cb = endTurnButt.colors;
        cb.disabledColor = Color.white;
        endTurnButt.colors = cb;
        endTurnButt.interactable = false;

        foreach (Button b in GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>())
        {
            cb = b.colors;
            cb.disabledColor = Color.white;
            b.colors = cb;
            b.interactable = false;
        }

    }

    public virtual void End()
    {
        Button[] shortcutButt = TutoManager.s_instance.shortcutButton.GetComponentsInChildren<Button>();

        foreach (Button b in shortcutButt)
        {
            b.interactable = true;
        }

        Button endTurnButt = TutoManager.s_instance.endTurnButton.GetComponentInChildren<Button>();
        endTurnButt.interactable = true;

        GameManager.Instance.Ui.ClearActionPanel();

        GameManager.Instance.PersistenceLoader.SetSequenceUnlocked(TutoManager.s_instance.PlayingSequence.GetType().ToString().ToLower(), true);
        Debug.Log(GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences[TutoManager.s_instance.PlayingSequence.GetType().ToString().ToLower()]);
        GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences[TutoManager.s_instance.PlayingSequence.GetType().ToString().ToLower()] = true;
        Debug.Log(GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences[TutoManager.s_instance.PlayingSequence.GetType().ToString().ToLower()]);
        if (TutoManager.s_instance.PlayingSequence.GetType() == typeof(SeqFirstMove))
        {
            GameManager.Instance.PersistenceLoader.SetPawnUnlocked("grekhan", true);
            GameManager.Instance.PersistenceLoader.SetPawnUnlocked("lupus", true);
            GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns["grekhan"] = true;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns["lupus"] = true;

            GameManager.Instance.PersistenceLoader.SetLevelUnlocked("4", true);
            GameManager.Instance.PersistenceLoader.SetLevelUnlocked("2", true);
            GameManager.Instance.PersistenceLoader.SetLevelUnlocked("1", false);
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels["4"] = true;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels["2"] = true;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels["1"] = false;

            GameManager.Instance.PersistenceLoader.SetEventUnlocked("1", true);
            GameManager.Instance.PersistenceLoader.SetEventUnlocked("2", true);
            GameManager.Instance.PersistenceLoader.SetEventUnlocked("3", true);
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceEvents["1"] = true;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceEvents["2"] = true;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceEvents["3"] = true;

            GameManager.Instance.PersistenceLoader.SetDeckUnlocked("deck_04", true);
            GameManager.Instance.PersistenceLoader.SetDeckUnlocked("deck_01", false);
            GameManager.Instance.PersistenceLoader.SetDeckUnlocked("deck_02", true);
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceDecks["deck_04"] = true;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceDecks["deck_01"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceDecks["deck_02"] = true;
        }
    }

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

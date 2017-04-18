using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour {

    private static TutoManager s_instance = null;

    public bool enableTuto = false;
    private SequenceTuto playingSequence;

    private List<SequenceTuto> sequences;

    public SequenceTuto PlayingSequence
    {
        get
        {
            return playingSequence;
        }

        set
        {
            playingSequence = value;
            GameManager.Instance.CurrentState = playingSequence != null ? (GameState.InPause) : GameState.Normal;
        }
    }

    public abstract class SequenceTuto
    {
        private bool alreadyPlayed = false;

        public abstract void Sequence();
        public abstract void EndStateSequence();
    }

    public void Update()
    {
        if (playingSequence != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                playingSequence.EndStateSequence();
            }
        }

    }



    // Use this for initialization
    void Start () {
        s_instance = this;
        sequences = new List<SequenceTuto>();
        if (enableTuto)
        {
            sequences.Add(new SequenceIntro());
            playSequence(sequences[0]);
        }
    }

    public class SequenceIntro : SequenceTuto
    {
        public override void Sequence()
        {
             
        }
        public override void EndStateSequence()
        {
            s_instance.PlayingSequence = null;
        }
    }

  
    public void playSequence(SequenceTuto st)
    {
        s_instance.PlayingSequence = st;
        st.Sequence();
    }
}

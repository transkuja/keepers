using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour {

    private static TutoManager s_instance = null;

    public bool enableTuto = false;
    private SequenceTuto playingSequence;

    private List<SequenceTuto> sequences;


    public IEnumerator MyFunction(GameObject go, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        go.SetActive(true);
        // Now do your thing here
    }


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
            GameObject go = GameManager.Instance.PawnDataBase.CreatePawn("ashley", new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity, null);
            go.SetActive(false);
            if (go.GetComponent<Behaviour.Mortal>().DeathParticles != null)
            {
                ParticleSystem ps = Instantiate(go.GetComponent<Behaviour.Mortal>().DeathParticles, go.transform.parent);
                ps.transform.position = go.transform.position;
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration);
            }
            s_instance.StartCoroutine(s_instance.MyFunction(go, 2.0f));

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

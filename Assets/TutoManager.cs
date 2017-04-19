using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour {

    private static TutoManager s_instance = null;

    public bool enableTuto = false;
    private List<SequenceTuto> sequences;

    public enum SequenceState { Idle, WaitingForInput, End };

    private SequenceTuto playingSequence;
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

    public void Update()
    {
        if (playingSequence != null)
        {
            if ((Input.GetKeyDown(KeyCode.Space)|| Input.GetMouseButtonDown(0)) && playingSequence.currentState == SequenceState.WaitingForInput)
            {
                playingSequence.currentState = SequenceState.Idle;
                s_instance.playNextSequence(playingSequence);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                playingSequence.EndStateSequence();
            }           
            else if ( playingSequence.currentState == SequenceState.End)
            {
                PlayingSequence = null;
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

    public void playSequence(SequenceTuto st)
    {
        s_instance.PlayingSequence = st;
        st.Sequence();
    }

    public void playNextSequence(SequenceTuto st)
    {
        if(st.currentState != SequenceState.End)
        StartCoroutine(st.listSequences[st.nextSeq]);
    }

    #region Sequences
    public abstract class SequenceTuto
    {
        public bool alreadyPlayed = false;
        public SequenceState currentState;
        public List<IEnumerator> listSequences;
        public int nextSeq;

        public abstract void Sequence();
        public abstract void EndStateSequence();
    }

    public class SequenceIntro : SequenceTuto
    {
        public GameObject goMrResetti;

        public override void Sequence()
        {
            currentState = SequenceState.Idle;
            goMrResetti = SpawnMrResetti();
            listSequences = new List<IEnumerator>();
            listSequences.Add(Spawn(goMrResetti, 0.5f));
            listSequences.Add(Intro(goMrResetti, 0.5f));
            listSequences.Add(EndSeq(0.5f));

            nextSeq = 0;
            s_instance.playNextSequence(this);
        }

        public GameObject SpawnMrResetti()
        {
            goMrResetti = GameManager.Instance.PawnDataBase.CreatePawn("mrresetti", new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity, null);
            goMrResetti.SetActive(false);
            if (goMrResetti.GetComponent<Behaviour.Mortal>().DeathParticles != null)
            {
                ParticleSystem ps = Instantiate(goMrResetti.GetComponent<Behaviour.Mortal>().DeathParticles, goMrResetti.transform.parent);
                ps.transform.position = goMrResetti.transform.position;
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration);
            }
            return goMrResetti;
        }

        public IEnumerator Spawn(GameObject mrresetti, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            mrresetti.SetActive(true);
            mrresetti.GetComponentInChildren<Animator>().SetTrigger("jumpArround");


            // TODO ? 
            yield return new WaitForSeconds(mrresetti.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);

            string str = "Salut c'est moi Mr Resetti. Bienvenue dans le tutoriel de \"Keepers\" !";
            yield return EcrireMessage(mrresetti.GetComponent<Interactable>().Feedback, str);
            currentState = SequenceState.WaitingForInput;
            nextSeq = 1;
        }
        public IEnumerator Intro(GameObject mrresetti, float delayTime)
        {
            string str2 = "Le jour se lève sur la planète France, encore !";
            yield return EcrireMessage(mrresetti.GetComponent<Interactable>().Feedback, str2);
            currentState = SequenceState.WaitingForInput;
            nextSeq = 2;
        }

        public IEnumerator EndSeq(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            if (goMrResetti != null) goMrResetti.SetActive(false);
            currentState = SequenceState.End;
        }


        public override void EndStateSequence()
        {
            s_instance.PlayingSequence = null;
            if (goMrResetti != null) goMrResetti.SetActive(false);
            alreadyPlayed = true;
        }
    }
    #endregion

    public static IEnumerator EcrireMessage(Transform feedback,string str)
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
        yield return new WaitForSeconds(0.5f);
       
    }
}

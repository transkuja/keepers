using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Behaviour;

public enum SequenceState { Idle, ReadyForNext, WaitingForInput, WaitingForClickUI, End };

public abstract class Etape
{
    public bool alreadyPlayed = false;
    public IEnumerator step;
}

public class TutoManager : MonoBehaviour {

    public static TutoManager s_instance = null;
    public bool enableTuto = false;
    private Sequence playingSequence;
    private List<Sequence> sequences;

    private static bool mouseCLicked;
    public GameObject tutoPanel;

    private GameObject tutoPanelInstance;

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        if (s_instance.enableTuto && s_instance.GetComponent<SeqIntro>().AlreadyPlayed == false)
        {
            InitTutoScene();
        }
    }

    void InitTutoScene()
    {
        SeqIntro seqIntro = s_instance.GetComponent<SeqIntro>();
        seqIntro.selectedKeeper.SetActive(false);
        seqIntro.endTurnBtn.SetActive(false);
        seqIntro.shortcutBtn.SetActive(false);

        Transform selectedKeepersFirstCharUI = seqIntro.selectedKeeper.transform.GetChild(0);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleLeft).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.ButtonCycleRight).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.Mortal).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.Hunger).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(0).GetChild((int)PanelSelectedKeeperStatChildren.MentalHealth).gameObject.SetActive(false);
        selectedKeepersFirstCharUI.GetChild(1).gameObject.SetActive(false); // Equipements
        selectedKeepersFirstCharUI.GetChild(2).gameObject.SetActive(false); // Inventory

        GameManager.Instance.AllKeepersList[0].GetComponent<Keeper>().GoListCharacterFollowing.Add(GameManager.Instance.PrisonerInstance.gameObject);
        GameManager.Instance.PrisonerInstance.GetComponent<Escortable>().enabled = false;
        GameManager.Instance.PrisonerInstance.GetComponent<Interactable>().Interactions = new InteractionImplementer();
        Destroy(GameManager.Instance.PrisonerInstance.GetComponent<GlowObjectCmd>());
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
                    s_instance.playNextSeq();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (playingSequence.CurrentState)
                {
                    case SequenceState.Idle:
                        break;
                    case SequenceState.WaitingForInput:
             
                        if (playingSequence.isLastSequence())
                        {
                            playingSequence.MoveNext();
                            playingSequence.CurrentState = SequenceState.End;
                        }
                        else
                        {
                            s_instance.playNextSeq();
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
                if (tutoPanelInstance != null && tutoPanelInstance.activeSelf == true)
                    Destroy(tutoPanelInstance);
                Debug.Log("prout");
                playingSequence.AlreadyPlayed = true;
                PlayingSequence = null;
            }
            else if (playingSequence.CurrentState == SequenceState.ReadyForNext)
            {
                if (playingSequence.isLastSequence())
                {
                    playingSequence.MoveNext();
                    playingSequence.CurrentState = SequenceState.End;
                }
                else
                {
                    s_instance.playNextSeq();
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
            if (playingSequence != null)
            {
                GameManager.Instance.CurrentState = GameState.InTuto;
            } else
            {
                GameManager.Instance.CurrentState = GameState.Normal;
                TutoManager.s_instance.StopAllCoroutines();
            }
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

    public void playSequence(Sequence seq)
    {
        s_instance.PlayingSequence = seq;
        s_instance.playingSequence.Init();
        s_instance.playingSequence.Play();
    }

    public void Init()
    {
        s_instance.enableTuto = true;
    }

    public IEnumerator EcrireMessage(string str)
    {
        if (tutoPanelInstance == null)
        {
            tutoPanelInstance = Instantiate(tutoPanel, GameManager.Instance.Ui.transform.GetChild(0), false);
            tutoPanelInstance.transform.localScale = Vector3.one;
        }

        tutoPanelInstance.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        tutoPanelInstance.GetComponentInChildren<Text>().text = str;
        tutoPanelInstance.SetActive(true);
        yield return null;
    }

    public class Message : Etape
    {
        GameObject mrresetti;
        string str;
        public Message(GameObject _mrresetti, string _str)
        {
            mrresetti = _mrresetti;
            step = Message_fct(0.5f);
            str = _str;
        }

        public Message(Message _origin)
        {
            mrresetti = _origin.mrresetti;
            str = _origin.str;
            step = Message_fct(0.5f);
        }
        public IEnumerator Message_fct(float delayTime)
        {
            yield return TutoManager.s_instance.EcrireMessage(str);
            s_instance.tutoPanelInstance.transform.GetChild(3).gameObject.SetActive(true);
            s_instance.tutoPanelInstance.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
            s_instance.tutoPanelInstance.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => TutoManager.s_instance.playNextSeq());
  
            if (!s_instance.PlayingSequence.isFirstMessage())
            {
                s_instance.tutoPanelInstance.transform.GetChild(2).gameObject.SetActive(true);
                s_instance.tutoPanelInstance.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                s_instance.tutoPanelInstance.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => TutoManager.s_instance.playPreviousSeq());
            }
            else
            {
                s_instance.tutoPanelInstance.transform.GetChild(2).gameObject.SetActive(false);
            }
            s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForInput;
            alreadyPlayed = true;
            yield return null;
        }
    }

    public void playNextSeq()
    {
        if(s_instance.playingSequence != null)
        {
            if (s_instance.playingSequence.MoveNext())
            {
                if (!s_instance.playingSequence.isLastSequence())
                {
                    if (s_instance.playingSequence.Current.alreadyPlayed)
                    {
                        s_instance.playingSequence.Current = new Message((Message)s_instance.playingSequence.Current);
                    }
                    s_instance.playingSequence.Play();
                }
                else
                {
                    s_instance.playingSequence.CurrentState = SequenceState.End;
                    s_instance.playingSequence.Play();
                }

            }
        }
    }

    public void playPreviousSeq()
    {
        if (s_instance.playingSequence != null)
        {
            if (s_instance.playingSequence.MovePrevious())
            {
                s_instance.playingSequence.Current = new Message((Message)s_instance.playingSequence.Current);
                s_instance.playingSequence.Play();
            }
        }
    }

    #region MmeResetti
    public class Spawn : Etape
    {
        GameObject mrresetti;
        AnimationClip jump;

        public Spawn(GameObject _mrresetti, AnimationClip _jump)
        {
            mrresetti = _mrresetti;
            jump = _jump;
            step = Spawn_fct(0.5f);
        }
        public IEnumerator Spawn_fct(float delayTime)
        {
            s_instance.playAppearenceFeedback(mrresetti.GetComponent<PawnInstance>());
            yield return new WaitForSeconds(delayTime);
            mrresetti.SetActive(true);
            mrresetti.GetComponentInChildren<Animator>().SetTrigger("jumpArround");

            // TODO ? 
            yield return new WaitForSeconds(jump.length);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
            alreadyPlayed = true;
            yield return null;
        }
    }
 
    public class UnSpawn : Etape
    {
        GameObject mrresetti;
        public UnSpawn(GameObject _mrresetti)
        {
            mrresetti = _mrresetti;
            step = UnSpawn_fct(0.5f);
        }

        public IEnumerator UnSpawn_fct(float delayTime)
        {
            s_instance.playAppearenceFeedback(mrresetti.GetComponent<PawnInstance>());
            yield return new WaitForSeconds(delayTime);
            if (mrresetti != null) mrresetti.SetActive(false);
            Destroy(mrresetti);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
            alreadyPlayed = true;
            yield return null;
        }
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

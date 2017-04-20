using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SeqIntro : Sequence {
    private GameObject pawnMrResetti;
    private GameObject pointer;
    public AnimationClip jumpAnimationClip;
    public AnimationClip turnLeftAnimationClip;
    public Transform canvas;
    public Transform gobtn;


    public void playAppearenceFeedback()
    {
        if (pawnMrResetti.GetComponent<Behaviour.Mortal>().DeathParticles != null)
        {
            ParticleSystem ps = GameObject.Instantiate(pawnMrResetti.GetComponent<Behaviour.Mortal>().DeathParticles, pawnMrResetti.transform.parent);
            ps.transform.position = pawnMrResetti.transform.position;
            ps.Play();
            GameObject.Destroy(ps.gameObject, ps.main.duration);
        }
    }

    public GameObject SpawnMmeResetti()
    {
        pawnMrResetti = GameManager.Instance.PawnDataBase.CreatePawn("mrresetti", new Vector3(0.0f, 0.0f, -1.0f), Quaternion.identity, null);
        pawnMrResetti.SetActive(false);
        playAppearenceFeedback();
        return pawnMrResetti;
    }

    public GameObject SpawnPointer()
    {
        pointer = GameObject.Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, canvas, false);
        pointer.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteMove;
        pointer.SetActive(false);
        return pointer;
    }

    public void DissapearMrResetti()
    {
        playAppearenceFeedback();
    }

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
            yield return new WaitForSeconds(delayTime);
            mrresetti.SetActive(true);
            mrresetti.GetComponentInChildren<Animator>().SetTrigger("jumpArround");

            // TODO ? 
            yield return new WaitForSeconds(jump.length);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
            alreadyPlayed = true;
        }
        public override void overstep()
        {
            mrresetti.SetActive(true);

            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
            alreadyPlayed = true;
        }
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
        public IEnumerator Message_fct(float delayTime)
        {
            yield return TutoManager.EcrireMessage(mrresetti.GetComponent<Interactable>().Feedback, str, delayTime);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForInput;
            alreadyPlayed = true;
        }
        public override void overstep()
        {
            mrresetti.GetComponent<Interactable>().Feedback.GetComponentInChildren<Text>().text = str;
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForInput;
            alreadyPlayed = true;
        }
    }

    public class LootAt : Etape
    {
        GameObject mrresetti;
        GameObject pointer;
        Transform gobtn;
        AnimationClip turnLeft;
        string str;

        public LootAt(GameObject _mrresetti, GameObject _pointer, Transform canvas, Transform _btnToHightlight, AnimationClip _turnLeft)
        {
            mrresetti = _mrresetti;
            gobtn = _btnToHightlight;
            turnLeft = _turnLeft;
            pointer = _pointer;
            pointer.transform.localScale = Vector3.one;
            pointer.transform.position = gobtn.position;
            pointer.SetActive(false);
            step = LootAt_fct(0.5f);
            str = "Regarde la bas !";
        }
        public IEnumerator LootAt_fct(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            mrresetti.GetComponentInChildren<Animator>().SetTrigger("turnLeft");
            yield return new WaitForSeconds(turnLeft.length);
            pointer.SetActive(true);
            yield return new WaitForSeconds(delayTime);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForInput;
            // Clean

            //Destroy(pointer);
            alreadyPlayed = true;
        }
        public override void overstep()
        {
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForInput;

            // Clean
            //Destroy(pointer);
            pointer.SetActive(false);
            alreadyPlayed = true;
        }
    }


    public class UnSpawn : Etape
    {
        GameObject mrresetti;
        public UnSpawn(GameObject _mrresetti)
        {
            mrresetti = _mrresetti;
            step = UnSpawn_fct(0.3f);
        }

        public IEnumerator UnSpawn_fct(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            if (mrresetti != null) mrresetti.SetActive(false);
            //Destroy(mrresetti);
            alreadyPlayed = true;
        }

        public override void overstep()
        {
            if (mrresetti != null) mrresetti.SetActive(false);
            //Destroy(mrresetti);
            alreadyPlayed = true;
        }
    }

  
    public override void Init()
    {
        CurrentState = SequenceState.Idle;
        pawnMrResetti = SpawnMmeResetti();
        pointer = SpawnPointer();
        Etapes = new List<Etape>();
        // First
        Etapes.Add(new Spawn(pawnMrResetti, jumpAnimationClip));

        // Content
        Etapes.Add(new Message(pawnMrResetti, "Salut c'est moi Mme Resetti. Bienvenue dans le tutoriel de \"Keepers\" !"));
        Etapes.Add(new Message(pawnMrResetti, "C'est votre première fois ici ?!"));
        Etapes.Add(new Message(pawnMrResetti, "Je vais vous apprendre les règles de bases pour jouer."));
        Etapes.Add(new LootAt(pawnMrResetti, pointer, canvas, gobtn, turnLeftAnimationClip));
        Etapes.Add(new Message(pawnMrResetti, "Regarder la bas!"));


        // Last
        Etapes.Add(new UnSpawn(pawnMrResetti));

        MoveNext();
    }

    public override void End()
    {
        DissapearMrResetti();
        pointer.SetActive(false);
        TutoManager.s_instance.StartCoroutine(this.Etapes[Etapes.Count - 1].step);
    }
}

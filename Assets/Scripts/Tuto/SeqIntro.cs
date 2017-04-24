using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SeqIntro : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;

    // Look at 1
    private GameObject pointer;
    public AnimationClip turnLeftAnimationClip;
    public GameObject shortcutBtn;

    // Look at 2
    private GameObject pointer2;
    public GameObject endTurnBtn;


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
        pointer = GameObject.Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, shortcutBtn.transform, false);

        pointer.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteTutoHighlightCercle;
        pointer.transform.SetParent(shortcutBtn.transform.parent);
        pointer.transform.SetAsFirstSibling();
        pointer.AddComponent<UIPointerOpacityTingling>();
        pointer.SetActive(false);
        return pointer;
    }

    public GameObject SpawnPointer2()
    {
        GameObject pointer = GameObject.Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, endTurnBtn.transform, false);

        pointer.GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteTutoHighlightCercle;
        pointer.transform.SetParent(endTurnBtn.transform.parent);
        pointer.transform.SetAsFirstSibling();
        pointer.AddComponent<UIPointerOpacityTingling>();
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
        GameObject gobtn;
        AnimationClip turnLeft;
        string str;

        public LootAt(GameObject _mrresetti, GameObject _pointer, GameObject _btnToHightlight, AnimationClip _turnLeft)
        {
            mrresetti = _mrresetti;
            gobtn = _btnToHightlight;
            turnLeft = _turnLeft;
            pointer = _pointer;
            pointer.transform.localScale = Vector3.one;
            pointer.transform.position = gobtn.transform.position;
            RectTransform rt = _btnToHightlight.GetComponent(typeof(RectTransform)) as RectTransform;
            float newX = _btnToHightlight.GetComponent<RectTransform>().sizeDelta.x + 70;
            float newY = _btnToHightlight.GetComponent<RectTransform>().sizeDelta.y + 70;
            pointer.GetComponent<RectTransform>().sizeDelta = new Vector2(newX, newY);
            pointer.SetActive(false);
            step = LootAt_fct(0.5f);
            str = "Regardez la bas, essayez de cliquer dessus.";
        }
        public IEnumerator LootAt_fct(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
  
            pointer.SetActive(true);
            mrresetti.GetComponentInChildren<Animator>().SetTrigger("turnLeft");
            yield return new WaitForSeconds(turnLeft.length);
            yield return TutoManager.EcrireMessage(mrresetti.GetComponent<Interactable>().Feedback, str, 0.0f);
            gobtn.gameObject.AddComponent<MouseClickExpected>();
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
            
            // Clean
            yield return new WaitForSeconds(0.5f);
            //pointer.SetActive(false);
            //gobtn.transform.localScale = Vector3.one;
            //Destroy(pointer);
            alreadyPlayed = true;
        }
        public override void overstep()
        {
            pointer.SetActive(true);
 
            //mrresetti.GetComponentInChildren<Animator>().SetTrigger("turnLeft");
            mrresetti.GetComponent<Interactable>().Feedback.GetComponentInChildren<Text>().text = str;
            gobtn.gameObject.AddComponent<MouseClickExpected>();
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;

            // Clean
            //Destroy(pointer);
            //gobtn.transform.localScale = Vector3.one;
            //pointer.SetActive(false);
            alreadyPlayed = true;
        }
    }

    public class UnLookAt : Etape
    {
        GameObject mrresetti;
        GameObject pointer;
        AnimationClip jump;
        GameObject btnToUnHightlight;
        string str;

        public UnLookAt(GameObject _mrresetti, GameObject _pointer, GameObject _btnToUnHightlight, AnimationClip _jumpAnimationClip)
        {
            mrresetti = _mrresetti;
            jump = _jumpAnimationClip;
            pointer = _pointer;
            btnToUnHightlight = _btnToUnHightlight;
            step = UnLookAtAt_fct(0.5f);
        }
        public IEnumerator UnLookAtAt_fct(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            pointer.SetActive(false);
            mrresetti.GetComponentInChildren<Animator>().SetTrigger("jumpArround");
            yield return new WaitForSeconds(jump.length);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
            btnToUnHightlight.GetComponent<MouseClickExpected>().enabled = false;

            // Clean
            //pointer.SetActive(false);
            //gobtn.transform.localScale = Vector3.one;
            //Destroy(pointer);
            alreadyPlayed = true;
        }
        public override void overstep()
        {
            pointer.SetActive(false);

            //mrresetti.GetComponentInChildren<Animator>().SetTrigger("jumpArround");
            btnToUnHightlight.gameObject.GetComponent<MouseClickExpected>().enabled = false;
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;

            // Clean
            //Destroy(pointer);
            //gobtn.transform.localScale = Vector3.one;
            //pointer.SetActive(false);
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
        //pointer2 = SpawnPointer2();
        Etapes = new List<Etape>();
        // First
        Etapes.Add(new Spawn(pawnMrResetti, jumpAnimationClip));

        // Content
        Etapes.Add(new Message(pawnMrResetti, "Salut c'est moi Mme Resetti. Bienvenue dans le tutoriel de \"Keepers\" !"));
        Etapes.Add(new Message(pawnMrResetti, "C'est votre première fois ici ?!"));
        Etapes.Add(new Message(pawnMrResetti, "Je vais vous apprendre les règles de bases pour jouer."));
        Etapes.Add(new LootAt(pawnMrResetti, pointer, shortcutBtn, turnLeftAnimationClip));
        Etapes.Add(new UnLookAt(pawnMrResetti, pointer, shortcutBtn, jumpAnimationClip));
        Etapes.Add(new Message(pawnMrResetti, "Ceci est le bouton de gestion des \"Keepers\". Vous pouvez voir les stats."));
        //Etapes.Add(new LootAt(pawnMrResetti, pointer2, endTurnBtn, turnLeftAnimationClip));
        //Etapes.Add(new UnLookAt(pawnMrResetti, pointer2, endTurnBtn, jumpAnimationClip));
        //Etapes.Add(new Message(pawnMrResetti, "Ceci est le boutton de fin de tour. Il permet de passer les jours pour récupérer des points d'action."));
        //Etapes.Add(new Message(pawnMrResetti, "Attention vos \"Keepers\" perdront du moral et de la faim à chaque tour."));

        // Last
        Etapes.Add(new UnSpawn(pawnMrResetti));

        MoveNext();
    }

    public override void End()
    {
        DissapearMrResetti();
        pointer.SetActive(false);
        //pointer2.SetActive(false);
        Destroy(pointer);
        //Destroy(pointer2);
        shortcutBtn.gameObject.GetComponent<MouseClickExpected>().enabled = false;
        //endTurnBtn.gameObject.GetComponent<MouseClickExpected>().enabled = false;
        shortcutBtn.transform.localScale = Vector3.one;
        //endTurnBtn.transform.localScale = Vector3.one;
        TutoManager.s_instance.StartCoroutine(this.Etapes[Etapes.Count - 1].step);
    }
}

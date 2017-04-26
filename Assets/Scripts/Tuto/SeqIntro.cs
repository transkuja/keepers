using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Behaviour;

public class SeqIntro : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;

    // Object to hide at start;

    // Look at 1
    private GameObject pointer;
    public AnimationClip turnLeftAnimationClip;

    // Look at 2
    private GameObject pointer2;

    [Header("Hidden in Tuto")]
    public GameObject selectedKeeper;
    public GameObject shortcutBtn;
    public GameObject endTurnBtn;

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





    //public class Activation : Etape
    //{
    //    GameObject goActivable;
    //    bool active;
    //    Sprite sprite;
    //    public Activation(GameObject _goActivable, Sprite _sprite, bool _active)
    //    {
    //        goActivable = _goActivable;
    //        active = _active;
    //        sprite = _sprite;
    //        step = Activation_fct(0.0f);
    //    }
    //    public IEnumerator Activation_fct(float delayTime)
    //    {
    //        yield return new WaitForSeconds(delayTime);
    //        goActivable.SetActive(active);
    //        goActivable.GetComponent<Image>().sprite = sprite;
    //        TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
    //        alreadyPlayed = true;
    //    }
    //}



    public class MessagePlusOverlay : Etape
    {
        GameObject mrresetti;
        string str;
        public MessagePlusOverlay(GameObject _mrresetti, string _str)
        {
            mrresetti = _mrresetti;
            step = Message_fct(0.5f);
            str = _str;
        }

        public MessagePlusOverlay(MessagePlusOverlay _origin)
        {
            mrresetti = _origin.mrresetti;
            str = _origin.str;
            step = Message_fct(0.5f);
        }
        public IEnumerator Message_fct(float delayTime)
        {
            GameManager.Instance.Ui.GoActionPanelQ.GetComponentInChildren<Button>().gameObject.AddComponent<ThrowDiceButtonFeedback>();

            yield return TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.TutoPanelInstance.transform.GetChild(3).gameObject.SetActive(true);
            TutoManager.s_instance.TutoPanelInstance.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
            TutoManager.s_instance.TutoPanelInstance.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => TutoManager.s_instance.playNextSeq());

            if (!TutoManager.s_instance.PlayingSequence.isFirstMessage())
            {
                TutoManager.s_instance.TutoPanelInstance.transform.GetChild(2).gameObject.SetActive(true);
                TutoManager.s_instance.TutoPanelInstance.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                TutoManager.s_instance.TutoPanelInstance.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => TutoManager.s_instance.playPreviousSeq());
            }
            else
            {
                TutoManager.s_instance.TutoPanelInstance.transform.GetChild(2).gameObject.SetActive(false);
            }
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;
            alreadyPlayed = true;
            yield return null;
        }
    }
  

    //public class LootAt : Etape
    //{
    //    GameObject mrresetti;
    //    GameObject pointer;
    //    GameObject gobtn;
    //    AnimationClip turnLeft;
    //    string str;

    //    public LootAt(GameObject _mrresetti, GameObject _pointer, GameObject _btnToHightlight, AnimationClip _turnLeft)
    //    {
    //        mrresetti = _mrresetti;
    //        gobtn = _btnToHightlight;
    //        turnLeft = _turnLeft;
    //        pointer = _pointer;
    //        pointer.transform.localScale = Vector3.one;
    //        pointer.transform.position = gobtn.transform.position;
    //        RectTransform rt = _btnToHightlight.GetComponent(typeof(RectTransform)) as RectTransform;
    //        float newX = _btnToHightlight.GetComponent<RectTransform>().sizeDelta.x + 70;
    //        float newY = _btnToHightlight.GetComponent<RectTransform>().sizeDelta.y + 70;
    //        pointer.GetComponent<RectTransform>().sizeDelta = new Vector2(newX, newY);
    //        pointer.SetActive(false);
    //        step = LootAt_fct(0.5f);
    //        str = "Regardez la bas, essayez de cliquer dessus.";
    //    }
    //    public IEnumerator LootAt_fct(float delayTime)
    //    {
    //        yield return new WaitForSeconds(delayTime);

    //        pointer.SetActive(true);
    //        mrresetti.GetComponentInChildren<Animator>().SetTrigger("turnLeft");
    //        yield return new WaitForSeconds(turnLeft.length);
    //        yield return TutoManager.s_instance.EcrireMessage(str);
    //        gobtn.gameObject.AddComponent<MouseClickExpected>();
    //        TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForClickUI;

    //        // Clean
    //        yield return new WaitForSeconds(0.5f);
    //        //pointer.SetActive(false);
    //        //gobtn.transform.localScale = Vector3.one;
    //        //Destroy(pointer);
    //        alreadyPlayed = true;
    //    }
    //}

    //public class UnLookAt : Etape
    //{
    //    GameObject mrresetti;
    //    GameObject pointer;
    //    AnimationClip jump;
    //    GameObject btnToUnHightlight;
    //    string str;

    //    public UnLookAt(GameObject _mrresetti, GameObject _pointer, GameObject _btnToUnHightlight, AnimationClip _jumpAnimationClip)
    //    {
    //        mrresetti = _mrresetti;
    //        jump = _jumpAnimationClip;
    //        pointer = _pointer;
    //        btnToUnHightlight = _btnToUnHightlight;
    //        step = UnLookAtAt_fct(0.5f);
    //    }
    //    public IEnumerator UnLookAtAt_fct(float delayTime)
    //    {
    //        yield return new WaitForSeconds(delayTime);
    //        pointer.SetActive(false);
    //        mrresetti.GetComponentInChildren<Animator>().SetTrigger("jumpArround");
    //        yield return new WaitForSeconds(jump.length);
    //        TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.ReadyForNext;
    //        btnToUnHightlight.GetComponent<MouseClickExpected>().enabled = false;

    //        // Clean
    //        //pointer.SetActive(false);
    //        //gobtn.transform.localScale = Vector3.one;
    //        //Destroy(pointer);
    //        alreadyPlayed = true;
    //    }
    //}


    //public class DeplacerCamera : Etape
    //{
    //    GameObject mrresetti;
    //    Tile where;
    //    public DeplacerCamera(GameObject _mrresetti, Tile _where)
    //    {
    //        mrresetti = _mrresetti;
    //        where = _where;
    //        step = DeplacerCamera_fct(0.5f);
    //    }

    //    public IEnumerator DeplacerCamera_fct(float delayTime)
    //    {
    //        yield return new WaitForSeconds(delayTime);
    //        GameManager.Instance.CameraManagerReference.UpdateCameraPosition(where);
    //        yield return new WaitForSeconds(delayTime);
    //        TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.WaitingForInput;
    //        alreadyPlayed = true;
    //    }
    //}


    public override void Init()
    {
        CurrentState = SequenceState.Idle;
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f));
        //pointer = SpawnPointer();
        // hide

        // Griser tous les actionq
        //(Button btn in GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Interactable>().Feedback.transform.GetChild(0).GetComponentsInChildren<Button>())
        //foreach (Button btn in GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>())
        //{
        //    btn.interactable = false;
        //}

        //pointer2 = SpawnPointer2();
        Etapes = new List<Etape>();
        // First
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        // Content
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Here's johnny !"));
        Etapes.Add(new MessagePlusOverlay(pawnMrResetti, "Click on the action button to explore the neighbouring area.")); // ==> feedback sur le bouton
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Well done you genius, here's your cookie!"));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "aaaa")); // ==> feedback sur les points d'action
        //Etapes.Add(new Activation(pawnMrResetti.GetComponent<Interactable>().Feedback.GetChild(0).GetChild(1).gameObject, GameManager.Instance.SpriteUtils.spriteMouseMiddleClicked, true));
        //Etapes.Add(new Message(pawnMrResetti, "Allez y visitez un peu."));
        //Etapes.Add(new Activation(pawnMrResetti.GetComponent<Interactable>().Feedback.GetChild(0).GetChild(1).gameObject, GameManager.Instance.SpriteUtils.spriteMouseMiddleClicked, false));
        //Etapes.Add(new Message(pawnMrResetti, "Le but du jeu est d'accompagner \"Ashley\" en vie a l'arrivée."));
        //Etapes.Add(new Message(pawnMrResetti, "Je suis gentille, je vous montre où elle se trouve."));
        //Etapes.Add(new DeplacerCamera(pawnMrResetti, TileManager.Instance.EndTile));
        //Etapes.Add(new DeplacerCamera(pawnMrResetti, TileManager.Instance.BeginTile));
        //Etapes.Add(new Message(pawnMrResetti, "Mais pour ça il va falloir selectionner votre \"Keepers\"."));
        //Etapes.Add(new Activation(pawnMrResetti.GetComponent<Interactable>().Feedback.GetChild(0).GetChild(1).gameObject, GameManager.Instance.SpriteUtils.spriteMouseLeftClicked, true));
        //Etapes.Add(new Activation(pawnMrResetti.GetComponent<Interactable>().Feedback.GetChild(0).GetChild(1).gameObject, GameManager.Instance.SpriteUtils.spriteMouseLeftClicked, false));
        //Etapes.Add(new Message(pawnMrResetti, "Bonne chance."));
        // Last
        Etapes.Add(new TutoManager.UnSpawn(pawnMrResetti));

        MoveNext();
    }

    public override void End()
    {
        //if ( pointer != null ) pointer.SetActive(false);
        //pointer2.SetActive(false);


        // Reactivate all UI
        selectedKeeper.SetActive(true);
        //endTurnBtn.SetActive(true);
        //shortcutBtn.SetActive(true);

        //Destroy(pointer);

        //Destroy(pointer2);
        //endTurnBtn.gameObject.GetComponent<MouseClickExpected>().enabled = false;
        //shortcutBtn.transform.localScale = Vector3.one;
        //endTurnBtn.transform.localScale = Vector3.one;
        TutoManager.s_instance.StartCoroutine(this.Etapes[Etapes.Count - 1].step);
    }
}

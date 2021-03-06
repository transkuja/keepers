﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class seqTeamCrocket : Sequence {
    private GameObject teamCrocketJames;
    private GameObject teamCrocketJessie;
    public AnimationClip jumpAnimationClip;

    public static void UnSpawn(GameObject teamCrocket)
    {
        TutoManager.s_instance.playAppearenceFeedback(teamCrocket.GetComponent<PawnInstance>());
        Destroy(teamCrocket, 0.2f);
    }


    public GameObject SpawnTeamCrocketJessie(Vector3 where)
    {
        GameObject jessie = GameManager.Instance.PawnDataBase.CreatePawn("swag", where, Quaternion.identity, null);
        jessie.SetActive(false);

        return jessie;
    }

    public GameObject SpawnTeamCrocketJames(Vector3 where)
    {
        GameObject james = GameManager.Instance.PawnDataBase.CreatePawn("grekhan", where, Quaternion.identity, null);
        james.SetActive(false);

        return james;
    }

    public static void EcrireMessage(Sprite whoSprite, string str)
    {
        if (TutoManager.s_instance.TutoPanelInstance == null)
        {
            TutoManager.s_instance.TutoPanelInstance = Instantiate(TutoManager.s_instance.tutoPanel, GameManager.Instance.Ui.transform.GetChild(0), false);
            TutoManager.s_instance.TutoPanelInstance.transform.localScale = Vector3.one;
        }



        TutoManager.s_instance.TutoPanelInstance.SetActive(false);
        //yield return new WaitForSeconds(0.5f);
        TutoManager.s_instance.TutoPanelInstance.GetComponentInChildren<Text>().text = str;
        TutoManager.s_instance.TutoPanelInstance.transform.GetChild(0).GetComponent<Image>().sprite = whoSprite;
        TutoManager.s_instance.TutoPanelInstance.SetActive(true);
        return;
    }

    public class Message : Step
    {
        GameObject speaker;
        string str;
        public Message(GameObject _speaker, string _str)
        {
            speaker = _speaker;
            stepFunction += Message_fct;
            str = _str;
 
        }

        public void Message_fct()
        {
            EcrireMessage(speaker.GetComponent<PawnInstance>().Data.AssociatedSprite, str);

            TutoManager.EnableNextButton();
            if (TutoManager.s_instance.PlayingSequence.isPreviousStepReachable())
            {
                TutoManager.EnablePreviousButton();
            }
            else
            {
                TutoManager.EnablePreviousButton(false);
            }
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
            return;
        }
    }

    public class SeDeplacer : Step
    {
        GameObject speaker;
        string str;
        public SeDeplacer(GameObject _speaker, string _str)
        {
            speaker = _speaker;
            stepFunction += SeDeplacer_fct;
            isReachableByClickOnPrevious = false;
            str = _str;
        }

        public void SeDeplacer_fct()
        {
            speaker.GetComponent<Behaviour.AnimatedPawn>();

            TutoManager.EnableNextButton();
            if (TutoManager.s_instance.PlayingSequence.isPreviousStepReachable())
            {
                TutoManager.EnablePreviousButton();
            }
            else
            {
                TutoManager.EnablePreviousButton(false);
            }
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
            return;
        }
    }

    public override void Init()
    {
        base.Init();

        GameManager.Instance.UpdateCameraPosition(GameManager.Instance.PrisonerInstance.CurrentTile);

        GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Escortable>().IsKidnappid = true;
        teamCrocketJames = SpawnTeamCrocketJames(new Vector3(1.0f, 0f, 0f) + GameManager.Instance.ActiveTile.transform.position);
        Etapes.Add(new Message(teamCrocketJames, "Grekhan"));
        teamCrocketJessie = SpawnTeamCrocketJessie(new Vector3(1.0f, 0f, 0f) + GameManager.Instance.ActiveTile.transform.position);
        Etapes.Add(new Message(teamCrocketJessie, "Swag"));

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(teamCrocketJames, jumpAnimationClip));
        Etapes.Add(new TutoManager.Spawn(teamCrocketJessie, jumpAnimationClip));


        // Content
        Etapes.Add(new Message(teamCrocketJames, Translater.TutoText("seqTeamCrocket", 0)));
        Etapes.Add(new Message(teamCrocketJessie, Translater.TutoText("seqTeamCrocket", 1)));
        Etapes.Add(new Message(teamCrocketJames, Translater.TutoText("seqTeamCrocket", 2)));
    }


    public override void End()
    {
        base.End();
        //if (teamCrocketJames != null)
        //    TutoManager.UnSpawn(teamCrocketJames);
        //if (teamCrocketJessie != null)
        //    TutoManager.UnSpawn(teamCrocketJessie);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.GetComponent<seqTeamCrocket>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

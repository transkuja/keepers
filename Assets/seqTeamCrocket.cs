using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class seqTeamCrocket : Sequence {
    private GameObject teamCrocket;
    public AnimationClip jumpAnimationClip;

    public static void UnSpawn(GameObject teamCrocket)
    {
        TutoManager.s_instance.playAppearenceFeedback(teamCrocket.GetComponent<PawnInstance>());
        Destroy(teamCrocket, 0.2f);
    }

    public GameObject SpawnTeamCrocket(Vector3 where)
    {
        GameObject pawnMrResetti = GameManager.Instance.PawnDataBase.CreatePawn("grekhan", where, Quaternion.identity, null);
        pawnMrResetti.SetActive(false);

        return pawnMrResetti;
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

    public override void Init()
    {
        base.Init();
        teamCrocket = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f));

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(teamCrocket, jumpAnimationClip));

        // Content
        Etapes.Add(new Message(teamCrocket, "Team Crocket, blast off at the speed of light!"));
    }


    public override void End()
    {
        base.End();
        if (teamCrocket != null)
            TutoManager.UnSpawn(teamCrocket);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.GetComponent<SeqFirstMove>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

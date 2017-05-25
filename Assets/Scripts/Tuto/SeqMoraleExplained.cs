using System.Collections.Generic;

using UnityEngine;



public class SeqMoraleExplained : Sequence {

    private GameObject pawnMrResetti;

    public AnimationClip jumpAnimationClip;

    public bool isLaunchedDuringASnowEvent = false;



    public override void Init()

    {

        base.Init();

        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f) + GameManager.Instance.ActiveTile.transform.position);



        Etapes = new List<Step>();

        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));



        Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqMoraleExplained", 0)));

        Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqMoraleExplained", 1)));



        if (isLaunchedDuringASnowEvent)

        {

            Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqMoraleExplained", 2)));

            Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqMoraleExplained", 3)));

        }



        Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqMoraleExplained", 4)));

    }



    public override void End()

    {

        base.End();

        if (pawnMrResetti != null)

            TutoManager.UnSpawn(pawnMrResetti);

        if (TutoManager.s_instance.TutoPanelInstance != null)

            Destroy(TutoManager.s_instance.TutoPanelInstance);



        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)

        {

            if(GameManager.Instance.AllKeepersList[i].GetComponent<Behaviour.MentalHealthHandler>() != null)

                GameManager.Instance.AllKeepersList[i].GetComponent<Behaviour.MentalHealthHandler>().SelectedMentalHealthUI.SetActive(true);

        }


        if (isLaunchedDuringASnowEvent)
        {
            foreach (PawnInstance pi in TileManager.Instance.KeepersOnTile[GameManager.Instance.ActiveTile])
            {
                if (pi.GetComponent<Behaviour.MentalHealthHandler>() != null)
                    pi.GetComponent<Behaviour.MentalHealthHandler>().CurrentMentalHealth -= 5;
            }
        }

        EventManager.HandleWeather();
        TutoManager.s_instance.GetComponent<SeqMoraleExplained>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }

}


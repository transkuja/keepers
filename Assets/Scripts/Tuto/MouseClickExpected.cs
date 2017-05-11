using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickExpected : MonoBehaviour, IPointerClickHandler {

    void Start()
    {
        TutoManager.MouseClicked = false;
    }

    private void Update()
    {
        if (TutoManager.s_instance.PlayingSequence != null 
            && TutoManager.s_instance.PlayingSequence.CurrentStep.GetType() == typeof(SeqMultiCharacters.ShortcutPanelTeaching))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                TutoManager.MouseClicked = true;
                GameManager.Instance.Ui.ToggleShortcutPanel();
            }
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            TutoManager.MouseClicked = true;
            if (TutoManager.s_instance.PlayingSequence.GetType() == typeof(SeqTutoCombat) && TutoManager.s_instance.PlayingSequence.CurrentStep.GetType() == typeof(SeqTutoCombat.SkillSelectionStep))
                Destroy(((SeqTutoCombat.SkillSelectionStep)TutoManager.s_instance.PlayingSequence.CurrentStep).feedback);
        }

    }
}

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
                TutoManager.MouseClicked = true;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            TutoManager.MouseClicked = true;
        }

    }
}

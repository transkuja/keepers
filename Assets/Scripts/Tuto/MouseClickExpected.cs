using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickExpected : MonoBehaviour, IPointerClickHandler {

    void Start()
    {
        TutoManager.MouseCLicked = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            TutoManager.MouseCLicked = true;
        }

    }
}

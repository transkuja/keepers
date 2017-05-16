using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLevel : MonoBehaviour {
    public int levelIndex;

    private bool isSelected;

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }

        set
        {
            isSelected = value;
        }
    }
}

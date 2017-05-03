using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOptionsReference : MonoBehaviour {

    [SerializeField]
    Toggle followingCharacterToggleButton;

    public Toggle FollowingCharacterToggleButton
    {
        get
        {
            return followingCharacterToggleButton;
        }

        set
        {
            followingCharacterToggleButton = value;
        }
    }
}

public enum PanelOptionsChildren { Header, DefaultBody, ControlsBody }
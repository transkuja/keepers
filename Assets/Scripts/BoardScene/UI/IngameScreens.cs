using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to identify game screens in scene.
/// </summary>
public class IngameScreens : MonoBehaviour {
    private static IngameScreens instance = null;

}

public enum IngameScreensEnum
{
    BattleResultScreens,
    SelectBattleCharactersScreen,
    WinScreen,
    LoseScreen
}
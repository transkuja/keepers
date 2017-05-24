using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageEnum { EN, FR }
public enum TooltipTextEnum { Hunger, Mood, Life }
public enum SkillDescriptionDetailsEnum { Target, HealValue, Damage, Effect}
public class Translater
{

    public static LanguageEnum CurrentLanguage = LanguageEnum.EN;

    public static string TutoText(string _sequence, int _index)
    {
        switch (_sequence)
        {
            case "SeqTutoCombat":
                switch (_index)
                {
                    case 0:
                        return "You have to roll dice to defeat monsters in this world";
                    case 1:
                        return "Click on the Dice button here to roll all your characters' dice.";
                    case 2:
                        return "Very well. Each die's value is added to the characters' stocks.";
                    case 3:
                        return "When these stocks' values are high enough, you will be able to perform skills.";
                    case 4:
                        return "To perform an action, select a pawn by clicking on it.";
                    case 5:
                        return "Here is your character's skill list";
                    case 6:
                        return "Each skill has a cost so you will need the required symbols' values to use one.";
                    case 7:
                        return "Now click on the skill \"Great Power\", then on the monster to unleash your power!";
                    case 8:
                        return "Great!";
                    case 9:
                        return "Once all of your characters have played, it's the monsters' turn";
                    case 10:
                        return "Use what you learned to finish the battle, good luck!";
                    default:
                        return "";
                }
            case "SeqMultiCharacters":
                switch (_index)
                {
                    case 0:
                        return "Now you have more characters.";
                    case 1:
                        return "Each one of them has its own action points, inventory and specificities.";
                    case 2:
                        return "You can see the character currently selected here.";
                    case 3:
                        return "You can interact with other characters by right clicking on them when you have a character selected.";
                    case 4:
                        return "Try interacting with another pawn!";
                    case 5:
                        return "Good,";
                    case 6:
                        return "you can also click here, or press A, to have a reminder of all your characters at once.";
                    default:
                        return "";
                }
            case "seqTeamCrocket":
                switch (_index)
                {
                    case 0:
                        return "Team Crocket, blast off at the speed of light!";
                    case 1:
                        return "We found you Waouf !";
                    case 2:
                        return "Let's bring him back home.";
                    default:
                        return "";
                }
            default:
                return "";
        }
        return "";
    }

    public static string PanelsText(string _level, int _index)
    {
        return "";
    }

    public static string QuestsText(string _level, int _index)
    {
        return "";
    }

    public static string PnjText(string _pnjName, int _index)
    {
        return "";
    }

    public static string EndTurnButtonText()
    {
        if (CurrentLanguage == LanguageEnum.FR)
            return "Jour";
        if (CurrentLanguage == LanguageEnum.EN)
            return "Day";
        return "";
    }

    public static string InteractionName(string _englishInteractionName)
    {
        return _englishInteractionName;
    }

    public static string TooltipText(TooltipTextEnum _expected)
    {
        return "";
    }

    public static string ItemDescription(string _fromJson)
    {

        return _fromJson;
    }

    public static string SkillName(string _userName, int _index)
    {
        return "";
    }

    public static string SkillDescription(string _userName, int _index)
    {
        return "";
    }

    public static string SkillDescriptionDetails(SkillDescriptionDetailsEnum _expectedField, SkillBattle _skillData)
    {
        return "";
    }

}

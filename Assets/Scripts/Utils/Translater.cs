using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageEnum { EN, FR }
public enum TooltipTextEnum { Hunger, Mood, Life }
public enum SkillDescriptionDetailsEnum { Target, HealValue, Damage, Effect}
public enum CharacterRace { Human, Dog, Cat, Duck, Hippopotamus}
public enum MainQuestTexts { Title, Objective, ObjectiveInfo, Other }
public enum QuestTexts { ObjectiveTitle, ObjectiveDesc, Title, Desc, Dialog, EndDialog, Hint }
public class Translater
{
    public static LanguageEnum CurrentLanguage = LanguageEnum.EN;

    public static string TutoText(string _sequence, int _index)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            return "";
        }
        else
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
                            return "Now play with all your remaining characters.";
                        case 10:
                            return "Once it's done, it's the monsters' turn";
                        case 11:
                            return "Once all of your characters have played, it's the monsters' turn";
                        case 12:
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
                case "SeqMoraleExplained":
                    switch (_index)
                    {
                        case 0:
                            return "Your characters' mood can be altered by certain events.";
                        case 1:
                            return "Arriving at an unwelcoming area, for example, can lower your characters' mood.";
                        case 2:
                            return "As well as staying at a snowy place.";
                        case 3:
                            return "Like... right now.";
                        case 4:
                            return "Characters can cheer each other up by using the Talk action.";
                        default:
                            return "";
                    }
                case "SeqLowMorale":
                    switch (_index)
                    {
                        case 0:
                            return "Be careful, one of your characters' mood is very low.";
                        case 1:
                            return "When a character's mood reaches 0, his efficiency is reduced!";
                        default:
                            return "";
                    }
                case "SeqLowHunger":
                    switch (_index)
                    {
                        case 0:
                            return "Darn, I told you to watch the hunger gauge!";
                        case 1:
                            return "When this gauge reaches 0, the character is starving, losing each turn a lot of health!";
                        case 2:
                            return "Feed him quick before he dies ...";
                        default:
                            return "";
                    }
                case "SeqAshleyLowHunger":
                    switch (_index)
                    {
                        case 0:
                            return "Be careful, Ashley is starving!";
                        case 1:
                            return "Drop some food in the Feed slot here to feed Ashley";
                        case 2:
                            return "Drop some food in the Feed slot here to feed her";
                        case 3:
                            return "Select a character in her area to feed her";
                        default:
                            return "";
                    }
                case "SeqAshleyEscort":
                    switch (_index)
                    {
                        case 0:
                            return "You have to escort Ashley safely to destination.";
                        case 1:
                            return "To change which character Ashley is following, you can right click on her to use the Escort action.";
                        case 2:
                            return "Take good care of her!";
                        default:
                            return "";
                    }
                case "SeqFirstMove":
                    switch (_index)
                    {
                        case 0:
                            return "Hi, I'm here to teach you how to play";
                        case 1:
                            return "First select the glowing pawn by clicking on it.";
                        case 2:
                            return "To interact with the world, you have to use the right click. Click on the ground to move the girl.";
                        case 3:
                            return "You can also interact with everything glowing in the world. Try right clicking on this portal.";
                        case 4:
                            return "Good,";
                        case 5:
                            return "you can see the cost of the action here.";
                        case 6:
                            return "Now click on the Explore button to explore the next area and get a cookie.";
                        case 7:
                            return "Well done you genius,";
                        case 8:
                            return "here's your cookie!";
                        case 9:
                            return "This action cost you 3 action points. Always keep an eye on them.";
                        case 10:
                            return "Ending the turn will restore all your characters' action points";
                        case 11:
                            return "Ending your turn ends the day,";
                        case 12:
                            return "but your characters get hungry in the morning, so be careful!";
                        case 13:
                            return "Remember that cookie I gave you ?";
                        case 14:
                            return "Eat it by double clicking on it to satisfy your selected character's appetite.";
                        case 15:
                            return "Great! You should now be able to finish this level. Good luck!";
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
        }
    }

    public static string PanelsText(string _level, int _index, CharacterRace _race = CharacterRace.Human)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            switch (_race)
            {
                case CharacterRace.Dog:
                    return "Ouaf Aou Ahouahou Wouf Wuf wuf Wouaf Wouaf Wuah Whaf Whouaf";
                case CharacterRace.Cat:
                    return "Miaou miaou miaou, Miaou Miaou";
                default:
                    return "";
            }
        }
        else
        {
            switch (_race)
            {
                case CharacterRace.Dog:
                    return "Arf arf Woof Woof woof wow ruff Ruff Woof";
                case CharacterRace.Cat:
                    return "Meow Meow Mew Mew Miaow";
                default:
                    switch (_level)
                    {
                        case "tuto":
                            return "Camera controls: ZQSD keys, the arrows or middle click.";
                        case "level2":
                            return "North: Village\nSouth-East: Danger";
                        case "level4":
                            return "Worthless alone but two uncovers a new path";
                        default:
                            return "";
                    }
            }
        }
    }

    public static string MainQuestText(MainQuestTexts _index)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            switch (_index)
            {
                case MainQuestTexts.Title:
                    return "THE LAST UNICORN SEAL";
                case MainQuestTexts.Objective:
                    return "Objectifs: ";
                case MainQuestTexts.ObjectiveInfo:
                    return "Amener Ashley EN VIE à La Fin.";
                case MainQuestTexts.Other:
                    return "Protect me!";
            }
        }
        else
        {
            switch(_index)
            {
                case MainQuestTexts.Title:
                    return "THE LAST UNICORN SEAL";
                case MainQuestTexts.Objective:
                    return "Objectives: ";
                case MainQuestTexts.ObjectiveInfo:
                    return "Bring Ashley ALIVE to The End.";
                case MainQuestTexts.Other:
                    return "Protect me!";
            }
        }
        return "";
    }

    public static string SideQuestsText(string _level, int _questIndex, QuestTexts _qt, CharacterRace _race = CharacterRace.Human)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            switch (_level)
            {
                case "level2":
                    switch (_questIndex)
                    {
                        case 0:
                            return "";
                        default:
                            return "";
                    }
                case "level4":
                    switch (_race)
                    {
                        case CharacterRace.Dog:
                            return "Coin? Coin! Coin coin coin.\n-Ouaf? Wouf waf wooof!\n-Coin !\n*Le chien et la maman canard se mettent à danser joyeusement*";
                        case CharacterRace.Cat:
                            return "Coin? Coin! Coin coin coin.\n-Miaou? Miaou miaou miaou!\n-Coin !\n*Les chats et la maman canard se mettent à danser joyeusement*";
                    }
                    switch (_questIndex)
                    {
                        case 0:
                            return "";
                        default:
                            return "";
                    }
                default:
                    return "";
            }
        }
        else
        {
            switch (_level)
            {
                case "level2":
                    switch (_questIndex)
                    {
                        case 0:
                            switch (_qt)
                            {
                                case QuestTexts.ObjectiveTitle:
                                    return "Kill Rabbit Jacob the SCARY MONSTER";
                                case QuestTexts.ObjectiveDesc:
                                    return "Eliminate the scary spooky monster menacing the village. That guy said it should be near this hidden passage...";
                                case QuestTexts.Title:
                                    return "Scary spooky monster must die";
                                case QuestTexts.Desc:
                                    return "A Frightful monster is menacing the village. Find it and persuade it not to come close again.";
                                case QuestTexts.Dialog:
                                    return "Hey you! Kill that big thing over there! *points at a hidden passage between two bushes*";
                                case QuestTexts.EndDialog:
                                    return "Great job! You know what, I have been wishing to get on an adventure for some time now... I'm coming with you!";
                                case QuestTexts.Hint:
                                    return "Go to the north, but be careful, this thing is one powerful being.";
                                default:
                                    return "";
                            }
                        default:
                            return "";
                    }
                case "level4":
                    switch (_race)
                    {
                        case CharacterRace.Dog:
                            return "Quack? Quack! Quack quack quack.\n-Woof? Ruff woof wooof!\n-Quack !\n*The dog and the mommy duck start to dance happily*";
                        case CharacterRace.Cat:
                            return "Quack? Quack! Quack quack quack.\n-Meow? meow mew meow!\n-Quack !\n*Both cats and the mommy duck start to dance happily*";
                        default:
                            switch (_questIndex)
                            {
                                case 0:
                                    switch (_qt)
                                    {
                                        case QuestTexts.ObjectiveTitle:
                                            return "Bring Mommy duck her 3 little ducklings";
                                        case QuestTexts.ObjectiveDesc:
                                            return "Mommy duck lost her ducklings, try to find them! Where are they? Well you know that ducks love water right...";
                                        case QuestTexts.Title:
                                            return "The duckling apocalypse";
                                        case QuestTexts.Desc:
                                            return "Mommy duck is blocking the way, and she won't move until she retrieves her ducklings.";
                                        case QuestTexts.Dialog:
                                            return "\"Quack? Quack! Quack quack quack.\" " + '\n' + " This Mommy duck looks very nervous. She seems to have lost her ducklings! Apparently she won't move until they come back... Will you help her?";
                                        case QuestTexts.EndDialog:
                                            return "Quack !*Jumps in happiness * "+'\n'+"The mommy duck seems very thankful, she greets her ducklings and move out of the way.Great job, you're a great person!";
                                        case QuestTexts.Hint:
                                            return "\"Quack quack...\"" + '\n' + "She seems very worried about her 3 babies... She's looking at the water, maybe she's giving you a hint?";
                                        default:
                                            return "";
                                    }
                                default:
                                    return "";
                            }
                    }
                default:
                    return "";
            }
        }
    }

    public static string PnjText(string _pnjName, int _index, CharacterRace _race = CharacterRace.Human)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            return "";
        }
        else
        {
            switch (_race)
            {
                case CharacterRace.Dog:
                    return "Arf arf Woof Woof woof wow ruff Ruff Woof";
                case CharacterRace.Cat:
                    return "Meow Meow Mew Mew Miaow";
                default:
                    switch (_pnjName)
                    {
                        case "PNJ_03":
                            switch (_race)
                            {
                                case CharacterRace.Dog:
                                    return "What a nice dog! You're so fluffy! *The mysterious woman pats the dog's head gently*";
                                case CharacterRace.Cat:
                                    switch (_index)
                                    {
                                        case 0:
                                            return "What a lovely cat !";
                                        case 1:
                                            return "This cat freaks me out.";
                                        default:
                                            return "";
                                    }
                                default:
                                    switch (_index)
                                    {
                                        case 0:
                                            return "Do you want to hear the story of this desert?";
                                        case 1:
                                            return "I hope you have some time ahead of you.";
                                        case 2:
                                            return "In my younger days, I used to be an adventurer just like you!";
                                        case 3:
                                            return "I came across a giant dinosaur and I won single handed.";
                                        case 4:
                                            return "You wonder how I did it?";
                                        case 5:
                                            return "Well, the thing is, T-rex have short arms. And I had a big sword.";
                                        case 6:
                                            return "Excalipoor was its name. I hid behind a huge rock and I stroke at the right moment!";
                                        case 7:
                                            return "What do you mean I cheated? I'm just too clever for you that can't find the solution to the desert enigma.";
                                        case 8:
                                            return "Try activate the two crystals at the same time and we'll see who is the smarter one!";
                                        default:
                                            return "";
                                    }
                            }
                        default:
                            return "";
                    }
            }
        }
    }

    public static string PnjName(string _pnjName)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            if (_pnjName == "PNJ_03")
                return "Femme Mystérieuse";
        }
        else
        {
            if (_pnjName == "PNJ_03")
                return "Mysterious Woman";
        }

        return "???";        
    }

    public static string EndTurnButtonText()
    {
        if (CurrentLanguage == LanguageEnum.FR)
            return "Jour";
        else
            return "Day";
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

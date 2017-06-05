using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public enum LanguageEnum { EN, FR }
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
        if (CurrentLanguage == LanguageEnum.FR)
        {
            if (_englishInteractionName == "Talk")
                return "Parler";
            if (_englishInteractionName == "Escort")
                return "Escorter";
            if (_englishInteractionName == "Release")
                return "Laisser ici";
            if (_englishInteractionName == "Harvest")
                return "Récolter";
            if (_englishInteractionName == "Fish")
                return "Pêcher";
            if (_englishInteractionName == "Trade")
                return "Echanger";
            if (_englishInteractionName == "Quest")
                return "Quête";
            if (_englishInteractionName == "Heal")
                return "Soigner";
            if (_englishInteractionName == "Attack")
                return "Attaquer";
            return _englishInteractionName;
        }
        else
            return _englishInteractionName;
    }

    public static string TooltipText(TypeOfJauge _expected)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            switch (_expected)
            {
                case TypeOfJauge.Hunger:
                    return "Faim: ";
                case TypeOfJauge.Mental:
                    return "Humeur: ";
                case TypeOfJauge.Health:
                    return "PV: ";
                case TypeOfJauge.Action:
                    return "PA restants: ";
                default:
                    return "";
            }
        }
        else
        {
            switch (_expected)
            {
                case TypeOfJauge.Hunger:
                    return "Hunger: ";
                case TypeOfJauge.Mental:
                    return "Mood: ";
                case TypeOfJauge.Health:
                    return "HP: ";
                case TypeOfJauge.Action:
                    return "AP left: ";
                default:
                    return "";
            }
        }
    }

    public static string ItemDescription(string _fromJson)
    {
        string regexPatternRestoresHunger = "^Restores ([0-9]+) hunger$";
        Regex rgx = new Regex(regexPatternRestoresHunger);
        Match match = rgx.Match(_fromJson);
        if (match.Success)
        {
            if (CurrentLanguage == LanguageEnum.FR)
                return "Rend " + match.Groups[1].Value + " de faim";
        }
        else
        {
            // TODO: fix this if more items are created
            if (CurrentLanguage == LanguageEnum.FR)
                return "Empêche de mourir de faim";
        }
        return _fromJson;

    }

    //public static string LuckBasedSkillName(string _pawnId, int _index)
    //{
    //    if (CurrentLanguage == LanguageEnum.FR)
    //        return "";
    //    else
    //    {
    //        switch (_pawnId)
    //        {
    //            case "lucky":
    //                if (_index == 1)
    //                    return "Paws";
    //                if (_index == 2)
    //                    return "Cat Ritual";
    //                if (_index == 0)
    //                    return "Purr";
    //                return "Attack";
    //        }
    //    }
    //}

    public static string SkillName(string _pawnId, int _index, bool _depressed = false)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 1)
                        return "Pattounes";
                    if (_index == 2)
                        return "Rituel Félin";
                    if (_index == 3)
                        return "Ron-Ron";
                    return "Attaquer";
                case "emo":
                    if (_index == 1)
                        return "Tir Rapide";
                    if (_index == 2)
                        return "Flèches de Feu";
                    return "Attaquer";
                case "swag":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Guérison Mineur";
                        if (_index == 2)
                            return "Soin Nuageux";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Guérison";
                        if (_index == 2)
                            return "Soin Arc-en-Ciel";
                        if (_index == 3)
                            return "Pouvoir de l'Amour";
                    }
                    return "Attaquer";
                case "nana":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Berserk";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Trancher";
                        if (_index == 2)
                            return "Mollasson!";
                        if (_index == 3)
                            return "Brise Armure";
                    }
                    return "Attaquer";
                case "lupus":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Crocs Précipités";
                        if (_index == 2)
                            return "Cendres";
                        if (_index == 3)
                            return "Mauvais Sort";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Crocs";
                        if (_index == 2)
                            return "Brasier";
                        if (_index == 3)
                            return "Ultima";
                    }
                    return "Attaquer";
                case "ashley":
                    if (_index == 1)
                        return "Trempette";
                    if (_index == 2)
                        return "Rayon Arc-en-Ciel";
                    if (_index == 3)
                        return "Fierté de Licorne";
                    return "Attaquer";
                case "grekhan":
                    if (_index == 1)
                        return (_depressed) ? "Hache Ecrasée" : "Hache Ecrasante";
                    if (_index == 2)
                        return "Gain de Puissance";
                    if (_index == 3)
                        return "Par ici!";
                    return "Attack";
                case "wolf":
                    if (_index == 0)
                        return "Crocs";
                    if (_index == 1)
                        return "Coup de Boule";
                    break;
                case "snowwolf":
                    if (_index == 0)
                        return "Crocs Enneigés";
                    if (_index == 1)
                        return "Coup Violent";
                    break;
                case "bird":
                    if (_index == 0)
                        return "Piqué";
                    if (_index == 1)
                        return "Ailes Furieuses";
                    break;
                case "ducky":
                    if (_index == 0)
                        return "Coin";
                    if (_index == 1)
                        return "Coin Coin";
                    if (_index == 2)
                        return "Coin!";
                    if (_index == 3)
                        return "Coin?";
                    break;
                case "rabbit_jacob_01":
                    if (_index == 0)
                        return "Morsure";
                    if (_index == 1)
                        return "Regard de la Mort";
                    break;
                case "snake":
                    if (_index == 0)
                        return "Crocs";
                    if (_index == 1)
                        return "Jet d'Acide";
                    if (_index == 2)
                        return "Etreinte";
                    break;
                case "bunny":
                    if (_index == 0)
                        return "Poing Carotte";
                    if (_index == 1)
                        return "Terrier";
                    break;
                case "snowbunny":
                    if (_index == 0)
                        return "Poing Carotte";
                    if (_index == 1)
                        return "Terrier";
                    break;
                case "duckprey":
                    if (_index == 0)
                        return "Coin";
                    break;
            }
            Debug.LogWarning("Missing FR translation for " + _pawnId + " or index missing : " + _index + ", depressed? " + _depressed);
        }
        else
        {
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 1)
                        return "Paws";
                    if (_index == 2)
                        return "Cat Ritual";
                    if (_index == 3)
                        return "Purr";
                    return "Attack";
                case "emo":
                    if (_index == 1)
                        return "Rapid Shot";
                    if (_index == 2)
                        return "Fire Arrows";
                    return "Attack";
                case "swag":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Low Cure";
                        if (_index == 2)
                            return "Cloudy Heal";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Cure";
                        if (_index == 2)
                            return "Rainbow Heal";
                        if (_index == 3)
                            return "Power of Love";
                    }
                    return "Attack";
                case "nana":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Berserk";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Cut";
                        if (_index == 2)
                            return "You're weak!";
                        if (_index == 3)
                            return "Crush Armor";
                    }
                    return "Attack";
                case "lupus":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Rushed Fangs";
                        if (_index == 2)
                            return "Ashes";
                        if (_index == 3)
                            return "Epic Fail";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Fangs";
                        if (_index == 2)
                            return "Fire";
                        if (_index == 3)
                            return "Ultima";
                    }
                    return "Attack";
                case "ashley":
                    if (_index == 1)
                        return "Splash";
                    if (_index == 2)
                        return "Rainbow Beam";
                    if (_index == 3)
                        return "Unicorn Pride";
                    return "Attack";
                case "grekhan":
                    if (_index == 1)
                        return (_depressed) ? "Crushed Axe" : "Axe Crush";
                    if (_index == 2)
                        return "Power Up";
                    if (_index == 3)
                        return "Over Here!";
                    return "Attack";
                case "wolf":
                    if (_index == 0)
                        return "Fangs";
                    if (_index == 1)
                        return "Headbutt";
                    break;
                case "snowwolf":
                    if (_index == 0)
                        return "White Fangs";
                    if (_index == 1)
                        return "Dash";
                    break;
                case "bird":
                    if (_index == 0)
                        return "Nosedive";
                    if (_index == 1)
                        return "Wings of Fury";
                    break;
                case "ducky":
                    if (_index == 0)
                        return "Quack";
                    if (_index == 1)
                        return "Quack Quack";
                    if (_index == 2)
                        return "Quack!";
                    if (_index == 3)
                        return "Quack?";
                    break;
                case "rabbit_jacob_01":
                    if (_index == 0)
                        return "Bite";
                    if (_index == 1)
                        return "Death Stare";
                    break;
                case "snake":
                    if (_index == 0)
                        return "Fangs";
                    if (_index == 1)
                        return "Acid Spit";
                    if (_index == 2)
                        return "Restraint";
                    break;
                case "bunny":
                    if (_index == 0)
                        return "Carrot Punch";
                    if (_index == 1)
                        return "Burrow";
                    break;
                case "snowbunny":
                    if (_index == 0)
                        return "Carrot Punch";
                    if (_index == 1)
                        return "Burrow";
                    break;
                case "duckprey":
                    if (_index == 0)
                        return "Quack";
                    break;
            }
            Debug.LogWarning("Missing EN translation for " + _pawnId + " or index missing : " + _index + ", depressed? " + _depressed);
        }
        return "";
    }

    public static string SkillDescription(string _pawnId, int _index, bool _depressed = false)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 1)
                        return "Pattounes";
                    if (_index == 2)
                        return "Rituel Félin";
                    if (_index == 3)
                        return "Ron-Ron";
                    return "Attaquer";
                case "emo":
                    if (_index == 1)
                        return "Tir Rapide";
                    if (_index == 2)
                        return "Flèches de Feu";
                    return "Attaquer";
                case "swag":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Guérison Mineur";
                        if (_index == 2)
                            return "Soin Nuageux";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Guérison";
                        if (_index == 2)
                            return "Soin Arc-en-Ciel";
                        if (_index == 3)
                            return "Pouvoir de l'Amour";
                    }
                    return "Attaquer";
                case "nana":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Berserk";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Trancher";
                        if (_index == 2)
                            return "Mollasson!";
                        if (_index == 3)
                            return "Brise Armure";
                    }
                    return "Attaquer";
                case "lupus":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Crocs Précipités";
                        if (_index == 2)
                            return "Cendres";
                        if (_index == 3)
                            return "Mauvais Sort";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Crocs";
                        if (_index == 2)
                            return "Brasier";
                        if (_index == 3)
                            return "Ultima";
                    }
                    return "Attaquer";
                case "ashley":
                    if (_index == 1)
                        return "Trempette";
                    if (_index == 2)
                        return "Rayon Arc-en-Ciel";
                    if (_index == 3)
                        return "Fierté de Licorne";
                    return "Attaquer";
                case "grekhan":
                    if (_index == 1)
                        return (_depressed) ? "Hache Ecrasée" : "Hache Ecrasante";
                    if (_index == 2)
                        return "Gain de Puissance";
                    if (_index == 3)
                        return "Par ici!";
                    return "Attack";
                case "wolf":
                    if (_index == 0)
                        return "Crocs";
                    if (_index == 1)
                        return "Coup de Boule";
                    break;
                case "snowwolf":
                    if (_index == 0)
                        return "Crocs Enneigés";
                    if (_index == 1)
                        return "Coup Violent";
                    break;
                case "bird":
                    if (_index == 0)
                        return "Piqué";
                    if (_index == 1)
                        return "Ailes Furieuses";
                    break;
                case "ducky":
                    if (_index == 0)
                        return "Coin";
                    if (_index == 1)
                        return "Coin Coin";
                    if (_index == 2)
                        return "Coin!";
                    if (_index == 3)
                        return "Coin?";
                    break;
                case "rabbit_jacob_01":
                    if (_index == 0)
                        return "Morsure";
                    if (_index == 1)
                        return "Regard de la Mort";
                    break;
                case "snake":
                    if (_index == 0)
                        return "Crocs";
                    if (_index == 1)
                        return "Jet d'Acide";
                    if (_index == 2)
                        return "Etreinte";
                    break;
                case "bunny":
                    if (_index == 0)
                        return "Poing Carotte";
                    if (_index == 1)
                        return "Terrier";
                    break;
                case "snowbunny":
                    if (_index == 0)
                        return "Poing Carotte";
                    if (_index == 1)
                        return "Terrier";
                    break;
                case "duckprey":
                    if (_index == 0)
                        return "Coin";
                    break;
            }
            Debug.LogWarning("Missing FR translation for " + _pawnId + " or index missing : " + _index + ", depressed? " + _depressed);
        }
        else
        {
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 1)
                        return "Paws";
                    if (_index == 2)
                        return "Cat Ritual";
                    if (_index == 3)
                        return "Purr";
                    return "Attack";
                case "emo":
                    if (_index == 1)
                        return "Rapid Shot";
                    if (_index == 2)
                        return "Fire Arrows";
                    return "Attack";
                case "swag":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Low Cure";
                        if (_index == 2)
                            return "Cloudy Heal";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Cure";
                        if (_index == 2)
                            return "Rainbow Heal";
                        if (_index == 3)
                            return "Power of Love";
                    }
                    return "Attack";
                case "nana":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Berserk";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Cut";
                        if (_index == 2)
                            return "You're weak!";
                        if (_index == 3)
                            return "Crush Armor";
                    }
                    return "Attack";
                case "lupus":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Rushed Fangs";
                        if (_index == 2)
                            return "Ashes";
                        if (_index == 3)
                            return "Epic Fail";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Fangs";
                        if (_index == 2)
                            return "Fire";
                        if (_index == 3)
                            return "Ultima";
                    }
                    return "Attack";
                case "ashley":
                    if (_index == 1)
                        return "Splash";
                    if (_index == 2)
                        return "Rainbow Beam";
                    if (_index == 3)
                        return "Unicorn Pride";
                    return "Attack";
                case "grekhan":
                    if (_index == 1)
                        return (_depressed) ? "Crushed Axe" : "Axe Crush";
                    if (_index == 2)
                        return "Power Up";
                    if (_index == 3)
                        return "Over Here!";
                    return "Attack";
                case "wolf":
                    if (_index == 0)
                        return "Fangs";
                    if (_index == 1)
                        return "Headbutt";
                    break;
                case "snowwolf":
                    if (_index == 0)
                        return "White Fangs";
                    if (_index == 1)
                        return "Dash";
                    break;
                case "bird":
                    if (_index == 0)
                        return "Nosedive";
                    if (_index == 1)
                        return "Wings of Fury";
                    break;
                case "ducky":
                    if (_index == 0)
                        return "Quack";
                    if (_index == 1)
                        return "Quack Quack";
                    if (_index == 2)
                        return "Quack!";
                    if (_index == 3)
                        return "Quack?";
                    break;
                case "rabbit_jacob_01":
                    if (_index == 0)
                        return "Bite";
                    if (_index == 1)
                        return "Death Stare";
                    break;
                case "snake":
                    if (_index == 0)
                        return "Fangs";
                    if (_index == 1)
                        return "Acid Spit";
                    if (_index == 2)
                        return "Restraint";
                    break;
                case "bunny":
                    if (_index == 0)
                        return "Carrot Punch";
                    if (_index == 1)
                        return "Burrow";
                    break;
                case "snowbunny":
                    if (_index == 0)
                        return "Carrot Punch";
                    if (_index == 1)
                        return "Burrow";
                    break;
                case "duckprey":
                    if (_index == 0)
                        return "Quack";
                    break;
            }
            Debug.LogWarning("Missing EN translation for " + _pawnId + " or index missing : " + _index + ", depressed? " + _depressed);
        }
        return "";
    }

    public static string SkillDescriptionDetails(SkillDescriptionDetailsEnum _expectedField, SkillBattle _skillData)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            switch (_expectedField)
            {
                case SkillDescriptionDetailsEnum.Target:
                    if (_skillData.TargetType == TargetType.FoeAll)
                        return "\nTarget: All enemies";
                    else if (_skillData.TargetType == TargetType.FoeSingle)
                        return "\nTarget: One enemy";
                    else if (_skillData.TargetType == TargetType.FriendSingle)
                        return "\nTarget: One ally";
                    else if (_skillData.TargetType == TargetType.FriendAll)
                        return "\nTarget: All allies";
                    else
                        return "\nTarget: Self";
                case SkillDescriptionDetailsEnum.Damage:
                    if (_skillData.CharacterSkillIndex == 0)
                    {
                        Behaviour.Fighter skillUser = _skillData.SkillUser;
                        int attackTotal = 0;
                        foreach (Face face in skillUser.LastThrowResult)
                        {
                            if (face.Type == FaceType.Physical)
                                attackTotal += face.Value * _skillData.effectiveAttackValue;
                            else
                                attackTotal += 3;
                        }
                        return "\n\nDamage based on orange dots on current throw\nDamage this turn: " + attackTotal;
                    }
                    else
                        return "\n\nDamage: " + _skillData.Damage;
                case SkillDescriptionDetailsEnum.Effect:
                    string effect = "";
                    effect += "\nEffect" + ((_skillData.Boeufs.Length > 1) ? "s" : "") + ": ";
                    if (_skillData.SkillName.Contains("Rapid"))
                        effect += "The more you shoot in a turn, the more magical power you gain.";
                    else
                        for (int i = 0; i < _skillData.Boeufs.Length; i++)
                        {
                            BattleBoeuf curBoeuf = _skillData.Boeufs[i];
                            if (curBoeuf.BoeufType == BoeufType.Damage)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                                effect += "damage dealt by " + Mathf.Abs(curBoeuf.EffectValue);
                            }
                            else if (curBoeuf.BoeufType == BoeufType.Defense)
                            {
                                effect += (curBoeuf.EffectValue > 0) ? "Reduce " : "Increase ";
                                effect += "damage taken by " + Mathf.Abs(curBoeuf.EffectValue);
                            }
                            else if (curBoeuf.BoeufType == BoeufType.Aggro)
                            {
                                effect += "Becomes attacks' target.";
                            }
                            else if (curBoeuf.BoeufType == BoeufType.CostReduction)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                                effect += "chances of being targeted by " + Mathf.Abs(curBoeuf.EffectValue) + " percent";
                            }
                            else if (curBoeuf.BoeufType == BoeufType.IncreaseStocks)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                                for (int j = 0; j < curBoeuf.SymbolsAffected.Length; j++)
                                {
                                    if (curBoeuf.SymbolsAffected[j] == FaceType.Physical)
                                        effect += "physical ";
                                    else if (curBoeuf.SymbolsAffected[j] == FaceType.Defensive)
                                        effect += "defensive ";
                                    else
                                        effect += "magic ";
                                }
                                effect += "gauges by " + Mathf.Abs(curBoeuf.EffectValue);
                            }
                            effect += " for " + (curBoeuf.Duration - 1) + " turns.\n";
                        }
                    return effect;
                case SkillDescriptionDetailsEnum.HealValue:
                    return "\n\nHeal value: " + _skillData.Damage;
            }
        }
        else
        {
            switch (_expectedField)
            {
                case SkillDescriptionDetailsEnum.Target:
                    if (_skillData.TargetType == TargetType.FoeAll)
                        return "\nTarget: All enemies";
                    else if (_skillData.TargetType == TargetType.FoeSingle)
                        return "\nTarget: One enemy";
                    else if (_skillData.TargetType == TargetType.FriendSingle)
                        return "\nTarget: One ally";
                    else if (_skillData.TargetType == TargetType.FriendAll)
                        return "\nTarget: All allies";
                    else
                        return "\nTarget: Self";
                case SkillDescriptionDetailsEnum.Damage:
                    if (_skillData.CharacterSkillIndex == 0)
                    {
                        Behaviour.Fighter skillUser = _skillData.SkillUser;
                        int attackTotal = 0;
                        foreach (Face face in skillUser.LastThrowResult)
                        {
                            if (face.Type == FaceType.Physical)
                                attackTotal += face.Value * _skillData.effectiveAttackValue;
                            else
                                attackTotal += 3;
                        }
                        return "\n\nDamage based on orange dots on current throw\nDamage this turn: " + attackTotal;
                    }
                    else
                        return "\n\nDamage: " + _skillData.Damage;
                case SkillDescriptionDetailsEnum.Effect:
                    string effect = "";
                    effect += "\nEffect" + ((_skillData.Boeufs.Length > 1) ? "s" : "") + ": ";
                    if (_skillData.SkillName.Contains("Rapid"))
                        effect += "The more you shoot in a turn, the more magical power you gain.";
                    else
                        for (int i = 0; i < _skillData.Boeufs.Length; i++)
                        {
                            BattleBoeuf curBoeuf = _skillData.Boeufs[i];
                            if (curBoeuf.BoeufType == BoeufType.Damage)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                                effect += "damage dealt by " + Mathf.Abs(curBoeuf.EffectValue);
                            }
                            else if (curBoeuf.BoeufType == BoeufType.Defense)
                            {
                                effect += (curBoeuf.EffectValue > 0) ? "Reduce " : "Increase ";
                                effect += "damage taken by " + Mathf.Abs(curBoeuf.EffectValue);
                            }
                            else if (curBoeuf.BoeufType == BoeufType.Aggro)
                            {
                                effect += "Becomes attacks' target";
                            }
                            else if (curBoeuf.BoeufType == BoeufType.CostReduction)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                                effect += "chances of being targeted by " + Mathf.Abs(curBoeuf.EffectValue) + " percent";
                            }
                            else if (curBoeuf.BoeufType == BoeufType.IncreaseStocks)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                                for (int j = 0; j < curBoeuf.SymbolsAffected.Length; j++)
                                {
                                    if (curBoeuf.SymbolsAffected[j] == FaceType.Physical)
                                        effect += "physical ";
                                    else if (curBoeuf.SymbolsAffected[j] == FaceType.Defensive)
                                        effect += "defensive ";
                                    else
                                        effect += "magic ";
                                }
                                effect += "gauges by " + Mathf.Abs(curBoeuf.EffectValue);
                            }
                            effect += " for " + (curBoeuf.Duration - 1) + " turns.\n";
                        }
                    return effect;
                case SkillDescriptionDetailsEnum.HealValue:
                    return "\n\nHeal value: " + _skillData.Damage;
            }
        }
        return "";
    }

    public static string BattleCharacterSelection()
    {
        if (CurrentLanguage == LanguageEnum.FR) return "Choisis tes combattants";
        else return "Select your characters";
    }

    public static Sprite WinningScreen(bool _won)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            if (_won) return GameManager.Instance.SpriteUtils.VictoryFR;
            return GameManager.Instance.SpriteUtils.DefeatFR;
        }
        else
        {
            if (_won) return GameManager.Instance.SpriteUtils.VictoryEN;
            return GameManager.Instance.SpriteUtils.DefeatEN;
        }
    }
}

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
            switch (_sequence)
            {
                case "SeqTutoCombat":
                    switch (_index)
                    {
                        case 0:
                            return "Il faut lancer des dés pour vaincre les monstres de ce monde";
                        case 1:
                            return "Clique sur le bouton \"Lancer les dés\" ici pour jeter les dés de tes personnages.";
                        case 2:
                            return "Très bien. La valeur de chaque dé est ajoutée aux réserves de leurs propriétaires.";
                        case 3:
                            return "Quand ces réserves sont suffisamment remplies, tu pourras utiliser des compétences.";
                        case 4:
                            return "Pour utiliser une compétence, choisis un pion en cliquant dessus.";
                        case 5:
                            return "Voilà la liste des compétences de ton personnage";
                        case 6:
                            return "Chaque compétence a un coût, il faut donc assez de chaque symbole en réserve pour en utiliser un.";
                        case 7:
                            return "Clique maintenant sur la compétence \"Grand Pouvoir\", puis sur un monstre pour déchaîner ta puissance!";
                        case 8:
                            return "Super!";
                        case 9:
                            return "Maintenant joue avec tes autres pions.";
                        case 10:
                            return "Une fois terminé, ce sera aux monstres de jouer";
                        case 11:
                            return "Une fois que tous tes pions ont joué, c'est au tour des monstres";
                        case 12:
                            return "Utilise ce que tu viens d'apprendre pour finir ce combat, bonne chance!";
                        default:
                            return "";
                    }
                case "SeqMultiCharacters":
                    switch (_index)
                    {
                        case 0:
                            return "Tu as désormais plus de personnages.";
                        case 1:
                            return "Chacun d'eux a ses propres points d'action, son inventaire et ses spécificités.";
                        case 2:
                            return "Tu peux voir le personnage actuellement sélectionné ici.";
                        case 3:
                            return "Tu peux interagir avec d'autres pions en faisant un clic droit sur eux lorsqu'un personnage est selectionné.";
                        case 4:
                            return "Essaie d'interagir avec un autre pion!";
                        case 5:
                            return "Bien,";
                        case 6:
                            return "tu peux à tout moment cliquer ici, ou appuyer sur A, pour voir l'état de tous tes personnages d'un simple coup d'oeil.";
                        default:
                            return "";
                    }
                case "SeqMoraleExplained":
                    switch (_index)
                    {
                        case 0:
                            return "L'humeur de tes pions peut être altérée par certains évènements.";
                        case 1:
                            return "Arriver dans une zone inhospitalière, par exemple, peut diminuer l'humeur de tes personnages.";
                        case 2:
                            return "Ou bien rester dans une zone enneigée.";
                        case 3:
                            return "Comme... maintenant.";
                        case 4:
                            return "Les pions peuvent se remonter le moral mutuellement en utilisant l'action Parler.";
                        default:
                            return "";
                    }
                case "SeqLowMorale":
                    switch (_index)
                    {
                        case 0:
                            return "Attention, l'humeur d'un de tes pions est très basse.";
                        case 1:
                            return "Si son humeur atteint 0, son efficacité sera réduite!";
                        default:
                            return "";
                    }
                case "SeqLowHunger":
                    switch (_index)
                    {
                        case 0:
                            return "Saperlipopette, je t'avais dit de surveiller la jauge de faim!";
                        case 1:
                            return "Quand cette jauge atteint 0, le pion devient affamé et perd beaucoup de vie à chaque tour!";
                        case 2:
                            return "Nourris le vite avant qu'il ne meure ...";
                        default:
                            return "";
                    }
                case "SeqAshleyLowHunger":
                    switch (_index)
                    {
                        case 0:
                            return "Attention, Ashley est affamée!";
                        case 1:
                            return "Dépose de la nourriture dans la case Nourrir ici pour nourrir Ashley";
                        case 2:
                            return "Dépose de la nourriture dans la case Nourrir ici pour la nourrir";
                        case 3:
                            return "Sélectionne un de tes pions dans sa zone pour la nourrir";
                        default:
                            return "";
                    }
                case "SeqAshleyEscort":
                    switch (_index)
                    {
                        case 0:
                            return "Tu dois escorter Ashley saine et sauve à destination.";
                        case 1:
                            return "Pour changer le pion qu'Ashley suit, tu peux faire un clic droit sur elle et utiliser l'action Escorter.";
                        case 2:
                            return "Prends bien soin d'elle!";
                        default:
                            return "";
                    }
                case "SeqFirstMove":
                    switch (_index)
                    {
                        case 0:
                            return "Bonjour, je suis là pour t'apprendre à jouer";
                        case 1:
                            return "Commence par sélectionner le pion clignotant en cliquant dessus.";
                        case 2:
                            return "Pour interagir avec le monde, tu dois utiliser le clic droit. Clique sur le sol pour bouger ton pion.";
                        case 3:
                            return "Tu peux aussi interagir avec tout ce qui brille dans ce monde. Essaie de faire un clic droit sur ce portail au sol.";
                        case 4:
                            return "Bien,";
                        case 5:
                            return "tu peux voir le coût de l'action ici.";
                        case 6:
                            return "Clique maintenant sur le bouton Explorer afin d'explorer la zone adjacente et gagner un cookie.";
                        case 7:
                            return "Bien joué petit(e) génie,";
                        case 8:
                            return "voilà ton cookie!";
                        case 9:
                            return "Cette action t'a coûtée 3 points d'action. Garde toujours un oeil dessus.";
                        case 10:
                            return "En terminant ton tour tu peux regagner les points d'action de tous tes pions";
                        case 11:
                            return "Terminer un tour termine la journée,";
                        case 12:
                            return "mais tes pions ont faim le matin au réveil, donc fais attention!";
                        case 13:
                            return "Tu te rappelles ce cookie que je t'ai donné ?";
                        case 14:
                            return "Mange le en double cliquant dessus afin de satisfaire l'appétit de ton pion.";
                        case 15:
                            return "Super! Tu devrais maintenant être capable de finir ce niveau. Bonne chance!";
                        default:
                            return "";
                    }
                case "seqTeamCrocket":
                    switch (_index)
                    {
                        case 0:
                            return "Team Crocket, plus rapide que la lumière!";
                        case 1:
                            return "On t'as retrouvé Waouf !";
                        case 2:
                            return "Ramenons-le à la maison.";
                        default:
                            return "";
                    }
                default:
                    return "";
            }
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
                            return "To interact with the world, you have to use the right click. Click on the ground to move your pawn.";
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
                    switch (_level)
                    {
                        case "tuto":
                            return "Contrôles caméra: ZQSD, les flèches ou le clic central.";
                        case "level2":
                            return "Nord: Village\nSud-Est: Danger";
                        case "level4":
                            return "Inutile seul mais à deux découvrent un nouveau chemin";
                        default:
                            return "";
                    }
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
                    return "LE DERNIER NARVAL";
                case MainQuestTexts.Objective:
                    return "Objectifs: ";
                case MainQuestTexts.ObjectiveInfo:
                    return "Amener Ashley EN VIE à La Fin.";
                case MainQuestTexts.Other:
                    return "Protégez-moi!";
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
                            switch (_qt)
                            {
                                case QuestTexts.ObjectiveTitle:
                                    return "Tuer Rabbit Jacob l'ABOMINABLE MONSTRE";
                                case QuestTexts.ObjectiveDesc:
                                    return "Eliminer l'effrayant monstre qui menace le village. Cet étrange personnage a dit qu'il devrait y avoir un passage secret pas loin...";
                                case QuestTexts.Title:
                                    return "L'effrayant monstre doit mourir";
                                case QuestTexts.Desc:
                                    return "Un horrible monstre menace le village. Trouvez le et persuadez le de ne plus embêter les villageois.";
                                case QuestTexts.Dialog:
                                    return "Vous là! Tuez ce gros machin là-haut! *montre du doigt un passage secret entre deux buissons*";
                                case QuestTexts.EndDialog:
                                    return "Bravo! Vous savez quoi, cela fait un moment que je désire partir à l'aventure... Je viens avec vous!";
                                case QuestTexts.Hint:
                                    return "Allez vers le nord mais soyez prudents, cette chose est relativement puissante.";
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
                            return "Coin? Coin! Coin coin coin.\n-Ouaf? Wouf waf wooof!\n-Coin !\n*Le chien et la maman canard se mettent à danser joyeusement*";
                        case CharacterRace.Cat:
                            return "Coin? Coin! Coin coin coin.\n-Miaou? Miaou miaou miaou!\n-Coin !\n*Les chats et la maman canard se mettent à danser joyeusement*";
                    }
                    switch (_questIndex)
                    {
                        case 0:
                            switch (_qt)
                            {
                                case QuestTexts.ObjectiveTitle:
                                    return "Ramener ses 3 canetons à la maman canard";
                                case QuestTexts.ObjectiveDesc:
                                    return "Cette maman canard a perdu ses canetons, essayez de les retrouver! Ils se trouvent sûrement près de l'eau ...";
                                case QuestTexts.Title:
                                    return "L'apocalypse des canetons";
                                case QuestTexts.Desc:
                                    return "Une maman canard bloque le chemin, et elle ne bougera pas tant qu'elle n'aura pas retrouvé ses petits.";
                                case QuestTexts.Dialog:
                                    return "\"Coin? Coin! Coin coin coin.\" " + '\n' + " Cette maman canard à l'air très anxieuse. On dirait qu'elle a perdu ses canetons! Apparemment elle ne bougera pas tant qu'ils ne seront pas revenus... Allez vous l'aider?";
                                case QuestTexts.EndDialog:
                                    return "Coin! *Saute de joie* " + '\n' + "La maman canard semble très reconnaissante, elle s'en va avec ses petits vers d'autres horizons. Bien joué, vous êtes quelqu'un de bien!";
                                case QuestTexts.Hint:
                                    return "\"Coin coin...\"" + '\n' + "Elle semble très inquiète au sujet de ses 3 petits... Elle regarde l'eau, peut-être vous donne-t-elle un indice?";
                                default:
                                    return "";
                            }
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
            switch (_pnjName)
            {
                case "PNJ_03":
                    switch (_race)
                    {
                        case CharacterRace.Dog:
                            return "Quel gentil chien! Tu es tout doux! *La femme mystérieuse caresse la tête de Lupus gentiment*";
                        case CharacterRace.Cat:
                            switch (_index)
                            {
                                case 0:
                                    return "Quel adorable chat!";
                                case 1:
                                    return "Ce chat est effrayant.";
                                default:
                                    return "";
                            }
                        default:
                            switch (_index)
                            {
                                case 0:
                                    return "Voulez-vous connaître l'histoire de ce désert?";
                                case 1:
                                    return "J'espère que vous avez un peu de temps.";
                                case 2:
                                    return "Dans ma jeunesse, j'étais une aventurière comme vous!";
                                case 3:
                                    return "Une fois, j'ai défait un dinosaure gigantesque d'une seule main.";
                                case 4:
                                    return "Vous vous demandez comment cela est possible?";
                                case 5:
                                    return "Et bien c'est très simple. Les T-rex ont des petits bras. Et j'avais une grosse épée!";
                                case 6:
                                    return "Elle s'appelait Excalipur. Je me suis caché derrière un énorme rocher et j'ai frappé au moment opportun!";
                                case 7:
                                    return "Comment ça j'ai triché? Je suis juste trop intelligente pour vous qui n'êtes même pas capable de trouver la solution à l'énigme de ce désert.";
                                case 8:
                                    return "Essayez d'activer les deux cristaux en même temps et on verra bien qui est le plus malin!";
                                default:
                                    return "";
                            }
                    }
                default:
                    return "";
            }
        }
        else
        {
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
                                    return "In my youth, I used to be an adventurer just like you!";
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

    public static string LuckBasedSkillName(string _pawnId, int _index)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 0)
                        return "Pattounes Mignonnes";
                    if (_index == 1)
                        return "Pattounes Mortelles";
                    if (_index == 2)
                        return "Pattounes";
                    if (_index == 3)
                        return "Pas d'Humeur";
                    if (_index == 4)
                        return "Ronron Parfait";
                    if (_index == 5)
                        return "Ronron";
                    if (_index == 6)
                        return "Calins pour Tous";
                    if (_index == 7)
                        return "Suprématie Féline";
                    if (_index == 8)
                        return "Châtiment de la Litière";
                    break;
            }
            Debug.LogWarning("Missing FR translation for " + _pawnId + " or index missing : " + _index);
            return "";
        }
        else
        {
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 0)
                        return "Cute Paws";
                    if (_index == 1)
                        return "Deadly Paws";
                    if (_index == 2)
                        return "Paws";
                    if (_index == 3)
                        return "Not In The Mood";
                    if (_index == 4)
                        return "Purrfect Purr";
                    if (_index == 5)
                        return "Purr";
                    if (_index == 6)
                        return "Hug Them All";
                    if (_index == 7)
                        return "Cat Supremacy";
                    if (_index == 8)
                        return "Litter Punishment";
                    break;
            }
            Debug.LogWarning("Missing EN translation for " + _pawnId + " or index missing : " + _index);
            return "";
        }
    }

    public static string SkillName(string _pawnId, int _index, bool _depressed = false)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            if (_index == -1)
                return "Grand Pouvoir";
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 1)
                        return "Pattounes";
                    if (_index == 2)
                        return "Rituel Félin";
                    if (_index == 3)
                        return "Ronron";
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
                            return "Guérison Mineure";
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
                    if (_index == 0)
                        return "Trempette";
                    if (_index == 1)
                        return "Rayon Arc-en-Ciel";
                    if (_index == 2)
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
            if (_index == -1)
                return "Great Power";
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
                    if (_index == 0)
                        return "Splash";
                    if (_index == 1)
                        return "Rainbow Beam";
                    if (_index == 2)
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
            if (_index == -1)
                return "Implique de grandes responsabilités.";
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 1)
                        return "Qui sait de quoi sont capables ces pattes?";
                    if (_index == 2)
                        return "Miaou ?";
                    if (_index == 3)
                        return "Ils semblent contents.";
                    return "";
                case "emo":
                    if (_index == 1)
                        return "Tire plus vite que son ombre. Peut rejouer après cette attaque.";
                    if (_index == 2)
                        return "Tire des flèches de feu.";
                    return "";
                case "swag":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Soigne un allié";
                        if (_index == 2)
                            return "Soigne tous les alliés";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Soigne un allié";
                        if (_index == 2)
                            return "Soigne tous les alliés";
                        if (_index == 3)
                            return "Une grande force émerge du pouvoir de l'amour.";
                    }
                    return "";
                case "nana":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Conséquence de sa mauvaise humeur";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Taille l'ennemi en pièces d'un autre pays.";
                        if (_index == 2)
                            return "Réduit les dégâts d'un ennemi";
                        if (_index == 3)
                            return "Réduit l'armure d'un ennemi";
                    }
                    return "";
                case "lupus":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Mord la cible";
                        if (_index == 2)
                            return "Lance des cendres partout";
                        if (_index == 3)
                            return "Impossible de se concentrer";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Mord la cible";
                        if (_index == 2)
                            return "Fais un barbecue";
                        if (_index == 3)
                            return "Meilleur sort au monde";
                    }
                    return "";
                case "ashley":
                    if (_index == 1)
                        return "Fait peut-être quelque chose.";
                    if (_index == 2)
                        return "Dégâts aléatoires sur tout le monde";
                    if (_index == 3)
                        return "Je suis une licorne, ouiiiiii!";
                    return "";
                case "grekhan":
                    if (_index == 1)
                        return (_depressed) ? "Ecrase faiblement son ennemi" : "Ecrase son ennemi";
                    return "";
            }
            Debug.LogWarning("Missing FR translation for " + _pawnId + " or index missing : " + _index + ", depressed? " + _depressed);
        }
        else
        {
            if (_index == -1)
                return "Comes with great responsability.";
            switch (_pawnId)
            {
                case "lucky":
                    if (_index == 1)
                        return "Who knows what these paws can do?";
                    if (_index == 2)
                        return "Mew ?";
                    if (_index == 3)
                        return "They seems happy.";
                    return "";
                case "emo":
                    if (_index == 1)
                        return "Shoots faster than his shadow. Can do another action after.";
                    if (_index == 2)
                        return "Fire fire arrows.";
                    return "";
                case "swag":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Heals an ally";
                        if (_index == 2)
                            return "Heals all allies";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Heals an ally";
                        if (_index == 2)
                            return "Heals all allies";
                        if (_index == 3)
                            return "Strength comes from power of love";
                    }
                    return "";
                case "nana":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Consequence of bad mood";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Cut the enemy to pieces";
                        if (_index == 2)
                            return "Reduce an enemy's damage";
                        if (_index == 3)
                            return "Reduce an enemy's armor";
                    }
                    return "";
                case "lupus":
                    if (_depressed)
                    {
                        if (_index == 1)
                            return "Bite the target";
                        if (_index == 2)
                            return "Throw ashes everywhere";
                        if (_index == 3)
                            return "Can't focus";
                    }
                    else
                    {
                        if (_index == 1)
                            return "Bite the target";
                        if (_index == 2)
                            return "Makes a barbecue";
                        if (_index == 3)
                            return "Best spell ever";
                    }
                    return "";
                case "ashley":
                    if (_index == 1)
                        return "Maybe it does something.";
                    if (_index == 2)
                        return "Random damage to all";
                    if (_index == 3)
                        return "I'm an unicorn, yaaaaay!";
                    return "";
                case "grekhan":
                    if (_index == 1)
                        return (_depressed) ? "Crush the ennemy weakly" : "Crush the enemy";
                    return "";
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
                        return "\nCible: Tous les ennemis";
                    else if (_skillData.TargetType == TargetType.FoeSingle)
                        return "\nCible: Un ennemi";
                    else if (_skillData.TargetType == TargetType.FriendSingle)
                        return "\nCible: Un allié";
                    else if (_skillData.TargetType == TargetType.FriendAll)
                        return "\nCible: Tous les alliés";
                    else
                        return "\nCible: Soi";
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
                        return "\n\nDégâts basés sur les points oranges des dés de ce tour\nDégâts ce tour: " + attackTotal;
                    }
                    else
                        return "\n\nDégâts: " + _skillData.Damage;
                case SkillDescriptionDetailsEnum.Effect:
                    string effect = "";
                    effect += "\nEffet" + ((_skillData.Boeufs.Length > 1) ? "s" : "") + ": ";
                    if (_skillData.SkillName.Contains("Rapid"))
                        effect += "Plus ce sort est utilisé dans un tour, plus la jauge de magie se remplit.";
                    else
                        for (int i = 0; i < _skillData.Boeufs.Length; i++)
                        {
                            BattleBoeuf curBoeuf = _skillData.Boeufs[i];
                            if (curBoeuf.BoeufType == BoeufType.Damage)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Réduit " : "Augmente ";
                                effect += "les dégâts infligés de " + Mathf.Abs(curBoeuf.EffectValue);
                            }
                            else if (curBoeuf.BoeufType == BoeufType.Defense)
                            {
                                effect += (curBoeuf.EffectValue > 0) ? "Réduit " : "Augmente ";
                                effect += "les dégâts subis de " + Mathf.Abs(curBoeuf.EffectValue);
                            }
                            else if (curBoeuf.BoeufType == BoeufType.Aggro)
                            {
                                effect += "Devient la cible des attaques.";
                            }
                            else if (curBoeuf.BoeufType == BoeufType.CostReduction)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Réduit " : "Augmente ";
                                effect += "les chances d'être ciblé de " + Mathf.Abs(curBoeuf.EffectValue) + " pourcent";
                            }
                            else if (curBoeuf.BoeufType == BoeufType.IncreaseStocks)
                            {
                                effect += (curBoeuf.EffectValue < 0) ? "Réduit " : "Augmente ";
                                effect += "la jauge ";

                                int nbrOfJaugeAffected = 0;
                                for (int j = 0; j < curBoeuf.SymbolsAffected.Length; j++)
                                {
                                    if (curBoeuf.SymbolsAffected[j] == FaceType.Physical)
                                    {
                                        effect += "physique ";
                                        nbrOfJaugeAffected++;
                                    }
                                    else if (curBoeuf.SymbolsAffected[j] == FaceType.Defensive)
                                    {
                                        effect += "defensive ";
                                    nbrOfJaugeAffected++;
                                    }
                                    else
                                    {
                                        effect += "magique ";
                                        nbrOfJaugeAffected++;
                                    }
                                }
                                effect += "de " + Mathf.Abs(curBoeuf.EffectValue);

                            }
                            effect += " pendant " + (curBoeuf.Duration - 1) + " tours.\n";
                        }
                    return effect;
                case SkillDescriptionDetailsEnum.HealValue:
                    return "\n\nSoigne: " + _skillData.Damage + "PV";
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

    public static string GameSelection(string gameSelected)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            if (gameSelected == "new") return "Nouvelle Partie"; else return "Continuer";
        }
        else
        {
            if (gameSelected == "new") return "New Game"; else return "Continue";
        }
    }

    public static Sprite BattleWinningScreen(bool _won)
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

    public static Sprite EndLevelWinningScreen(int _levelIndex)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            if (_levelIndex == 0) return GameManager.Instance.SpriteUtils.level1FR;
            if (_levelIndex == 1) return GameManager.Instance.SpriteUtils.level2FR;
            return GameManager.Instance.SpriteUtils.level3FR;
        }
        else
        {
            if (_levelIndex == 0) return GameManager.Instance.SpriteUtils.level1;
            if (_levelIndex == 1) return GameManager.Instance.SpriteUtils.level2;
            return GameManager.Instance.SpriteUtils.level3;
        }
    }

    public static Sprite GameOverScreen()
    {
        if (CurrentLanguage == LanguageEnum.FR)
            return GameManager.Instance.SpriteUtils.loseScreenFR;
        else
            return GameManager.Instance.SpriteUtils.loseScreen;
    }

    public static Texture2D CreditsTexture()
    {
        if (CurrentLanguage == LanguageEnum.FR)
            return GameManager.Instance.Texture2DUtils.creditsFR;
        else
            return GameManager.Instance.Texture2DUtils.creditsEN;
    }

    public static string LevelName(string _levelName)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            if (_levelName == "Tutorial") return "Tutoriel";
            if (_levelName == "The Journey") return "Le Périple";
            if (_levelName == "Mother") return "Mère";
            return "???";
        }
        else
            return _levelName;
    }

    public static Sprite LevelDifficulty(int _diff)
    {
        if (CurrentLanguage == LanguageEnum.FR)
        {
            if (_diff == 1) return GameManager.Instance.SpriteUtils.spriteEasyFR;
            if (_diff == 2) return GameManager.Instance.SpriteUtils.spriteMediumFR;
            if (_diff == 3) return GameManager.Instance.SpriteUtils.spriteHardFR;
            return GameManager.Instance.SpriteUtils.spriteTrivialFR;
        }
        else
        {
            if (_diff == 1) return GameManager.Instance.SpriteUtils.spriteEasy;
            if (_diff == 2) return GameManager.Instance.SpriteUtils.spriteMedium;
            if (_diff == 3) return GameManager.Instance.SpriteUtils.spriteHard;
            return GameManager.Instance.SpriteUtils.spriteTrivial;

        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowAssociatedDice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    PawnInstance pawn;
    GameObject feedbackAboveDice;
    int attackTotal = 0;

    private void Start()
    {
        pawn = GetComponentInParent<PawnInstance>();

        feedbackAboveDice = Instantiate(GameManager.Instance.PrefabUIUtils.diceFeedbackOnPointerEnter, GameManager.Instance.Ui.transform.GetChild(0));
        feedbackAboveDice.SetActive(false);

        attackTotal = 0;
        foreach (Face face in pawn.GetComponent<Behaviour.Fighter>().LastThrowResult)
        {
            if (face.Type == FaceType.Physical)
                attackTotal += face.Value;
        }
    }

    // TODO: may be useless, test @Anthony
    private void OnDisable()
    {
        if (name == "Skills")
        {
            Transform characterPanel = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetCharacterPanelIndex(pawn);
            foreach (Image im in characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetComponentsInChildren<Image>())
            {
                im.color = Color.white;
            }
            foreach (Text txt in characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetComponentsInChildren<Text>())
            {
                txt.color = Color.white;
            }
        }
        else
        {
            feedbackAboveDice.SetActive(false);
            foreach (GameObject die in pawn.GetComponent<Behaviour.Fighter>().LastThrowDiceInstance)
            {
                die.GetComponent<GlowObjectCmd>().UpdateColor(false);
                die.GetComponent<GlowObjectCmd>().enabled = true;
            }
        }
        feedbackAboveDice.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (name == "Skills")
        {
            Transform characterPanel = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetCharacterPanelIndex(pawn);
            foreach (Image im in characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetComponentsInChildren<Image>())
            {
                im.color = Color.yellow;
            }
            foreach (Text txt in characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetComponentsInChildren<Text>())
            {
                txt.color = Color.yellow;
            }
        }
        else
        {
            if (name == "Attack")
            {
                feedbackAboveDice.GetComponentInChildren<Text>().text = attackTotal.ToString();
                feedbackAboveDice.GetComponentInChildren<Text>().color = Color.red;
                feedbackAboveDice.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteAttackSymbol;
                feedbackAboveDice.GetComponentInChildren<Image>().color = Color.red;
                // TODO: @Anthony, pretty very ugly butt it works
                feedbackAboveDice.transform.position = 
                    Camera.main.WorldToScreenPoint(TileManager.Instance.DicePositionsOnTile
                    .GetChild(GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetCharacterPanelIndex(pawn).parent.GetSiblingIndex()).localPosition
                    + GameManager.Instance.CameraManagerReference.ActiveTile.transform.position);
                feedbackAboveDice.SetActive(true);
            }
            foreach (GameObject die in pawn.GetComponent<Behaviour.Fighter>().LastThrowDiceInstance)
            {
                die.GetComponent<GlowObjectCmd>().UpdateColor(true);
                die.GetComponent<GlowObjectCmd>().enabled = true;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (name == "Skills")
        {
            Transform characterPanel = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetCharacterPanelIndex(pawn);
            foreach (Image im in characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetComponentsInChildren<Image>())
            {
                im.color = Color.white;
            }
            foreach (Text txt in characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetComponentsInChildren<Text>())
            {
                txt.color = Color.white;
            }
        }
        else
        {
            feedbackAboveDice.SetActive(false);
            foreach (GameObject die in pawn.GetComponent<Behaviour.Fighter>().LastThrowDiceInstance)
            {
                die.GetComponent<GlowObjectCmd>().UpdateColor(false);
                die.GetComponent<GlowObjectCmd>().enabled = true;
            }
        }

    }

    void OnDestroy()
    {
        Destroy(feedbackAboveDice);
    }
}

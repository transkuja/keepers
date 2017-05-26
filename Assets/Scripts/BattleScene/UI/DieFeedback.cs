using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieFeedback : MonoBehaviour {

    PawnInstance owner;
    Face faceInfo;

    GameObject feedback;
    bool isRising = false;
    bool isMovingToPanel = false;
    float timer = 0.0f;
    Vector3 lerpStartPosition;
    Vector3 lerpEndPosition;
    float lerpParam = 0.0f;

    private Color greenFonce;
    private Color greenClaire;

    private Color orangeClaire;
    private Color orangeFonce;

    private Color bleuClaire;
    private Color bleuFonce;

    private Color violetClaire;
    private Color violetFonce;

    public void Start()
    {
        greenClaire = new Color32(0x1c, 0xc7, 0x56, 0xFF);
        greenFonce = new Color32(0x03, 0x60, 0x41, 0xFF);


        orangeClaire = new Color32(0x1c, 0xc7, 0x56, 0xFF);
        orangeFonce = new Color32(0x03, 0x60, 0x41, 0xFF);

        bleuClaire = new Color32(0x1c, 0xc7, 0x56, 0xFF);
        bleuFonce = new Color32(0x03, 0x60, 0x41, 0xFF);
        violetClaire = new Color32(0x1c, 0xc7, 0x56, 0xFF);
        violetFonce = new Color32(0x03, 0x60, 0x41, 0xFF);
    }


    public void PopFeedback(Face _faceInfo, PawnInstance _owner)
    {
        if (feedback == null)
            feedback = Instantiate(GameManager.Instance.PrefabUIUtils.diceFeedback, GameManager.Instance.Ui.transform.GetChild(0));

        owner = _owner;
        faceInfo = _faceInfo;
        feedback.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        feedback.transform.localScale = Vector3.one;
        feedback.GetComponentInChildren<Text>().text = "+ " + faceInfo.Value;
        feedback.GetComponentInChildren<Text>().color = greenClaire;
        feedback.GetComponentInChildren<Text>().GetComponent<Outline>().effectColor = greenFonce;
        feedback.GetComponentInChildren<Text>().GetComponent<Outline>().effectDistance = Vector2.zero;

        Transform attributesPanel = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetCharacterPanelIndex(owner).GetChild((int)CharactersPanelChildren.Attributes);
        if (faceInfo.Type == FaceType.Physical)
        {
            feedback.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteAttackSymbol;
            feedback.GetComponentInChildren<Image>().color = new Color(1, 0.5f, 0, 1);
            feedback.GetComponentInChildren<Image>().GetComponent<Outline>().effectColor = new Color(1, 0.5f, 0, 1);
            lerpEndPosition = attributesPanel.GetChild((int)AttributesChildren.Attack).position;
        }
        else if (faceInfo.Type == FaceType.Defensive)
        {
            feedback.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteDefenseSymbol;
            feedback.GetComponentInChildren<Image>().color = new Color(0, 0.5f, 1.0f, 1);
            feedback.GetComponentInChildren<Image>().GetComponent<Outline>().effectColor = new Color(0, 0.5f, 1.0f, 1);
            lerpEndPosition = attributesPanel.GetChild((int)AttributesChildren.Defense).position;
        }
        else if (faceInfo.Type == FaceType.Magical)
        {
            feedback.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteMagicSymbol;
            feedback.GetComponentInChildren<Image>().color = new Color(0.6f, 0, 0.6f, 1);
            feedback.GetComponentInChildren<Image>().GetComponent<Outline>().effectColor = new Color(0.6f, 0, 0.6f, 1);
            lerpEndPosition = attributesPanel.GetChild((int)AttributesChildren.Magic).position;
        }

        isRising = true;
    }

    private void Update()
    {
        if (feedback != null)
        {
            if (isRising)
            {
                if (timer < 0.5f)
                {
                    timer += Time.deltaTime;
                    feedback.transform.localPosition += (Time.deltaTime * 200) * Vector3.up;
                }
                else
                {
                    timer = 0.0f;
                    isRising = false;
                    isMovingToPanel = true;
                    lerpStartPosition = feedback.transform.position;
                    lerpParam = 0.0f;
                }
            }
            else
            {
                if (isMovingToPanel)
                {
                    feedback.transform.position = Vector3.Lerp(lerpStartPosition, lerpEndPosition, lerpParam*2);
                    lerpParam += Time.deltaTime;
                    if (lerpParam >= 0.5f)
                    {
                        isMovingToPanel = false;
                        Destroy(feedback);
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        Destroy(feedback);
    }

    private void OnDestroy()
    {
        Destroy(feedback);
    }
}

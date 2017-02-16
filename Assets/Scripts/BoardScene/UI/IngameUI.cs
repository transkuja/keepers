using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class IngameUI : MonoBehaviour
{
    // CharacterPanel
    [Header("Character Panel")]
    public GameObject CharacterPanel;

    // Turn Panel
    [Header("Turn Panel")]
    public GameObject TurnPanel;
    public GameObject TurnButton;
    public float buttonRotationSpeed = 1.0f;

    public bool isTurnEnding = false;

    public void Awake()
    {
        UpdateCharacterPanelUI();
    }

    public void Update()
    {
        if (GameManager.Instance != null )
        {
            if (GameManager.Instance.CharacterPanelIngameNeedUpdate)
            {
                UpdateCharacterPanelUI();
            }
        }
    }

    void UpdateCharacterPanelUI()
    {
        if (GameManager.Instance == null){ return; }
        if (CharacterPanel == null) { return; }

        // Clear
        if (CharacterPanel.GetComponentsInChildren<Image>().Length > 0)
        {
            foreach (Image characterImage in CharacterPanel.GetComponentsInChildren<Image>())
            {
                Destroy(characterImage.gameObject);
            }
        }

        // On selection
        float margeX = 60.0f;
        float margeY = 70.0f;
        float offsetY = 20.0f;
  
        int nbCaracters = GameManager.Instance.AllKeepersList.Count;
        for (int i = 0; i < nbCaracters; i++)
        {
            Keepers.Selectable currentSelectedCharacter = GameManager.Instance.AllKeepersList[i];
            if ( currentSelectedCharacter.selected)
            {
                if ( currentSelectedCharacter.associatedSprite != null)
                {
   
                    float value = margeY + (offsetY * (i)) + ((currentSelectedCharacter.associatedSprite.GetComponent<Image>().rectTransform.rect.height) * (i));
                    GameObject characterImage = Instantiate(currentSelectedCharacter.associatedSprite, CharacterPanel.transform);
                    characterImage.transform.localPosition = new Vector3(value, -margeX, 0.0f);
                    characterImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                }
            }

        }

        GameManager.Instance.CharacterPanelIngameNeedUpdate = false;
    }

    public void EndTurn()
    {
        AnimateButtonOnClick();
    }
    
    
    private void AnimateButtonOnClick()
    {
        // Activation de l'animation au moment du click
        Animator anim_button = TurnButton.GetComponent<Animator>();
        if (!isTurnEnding)
        {
            anim_button.speed = buttonRotationSpeed;
            anim_button.enabled = true;
            isTurnEnding = false;
        }

    }  
}

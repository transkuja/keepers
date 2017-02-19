using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject CaracterPanel;
    public GameObject baseCharacterImage;

    public void ChangeLanguage(string language)
    {
        LanguageSO.LoadDatabase(language);
    }

    public void Update()
    {
        if (GameManager.Instance.CharacterPanelMenuNeedUpdate)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        // Clear
        if (CaracterPanel.GetComponentsInChildren<Image>().Length > 0)
        {
            foreach (Image characterImage in CaracterPanel.GetComponentsInChildren<Image>())
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
            KeeperInstance currentSelectedCharacter = GameManager.Instance.AllKeepersList[i];

            if (currentSelectedCharacter.IsSelectedInMenu)
            {
                Sprite associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
                if (associatedSprite != null)
                {
                    GameObject CharacterImage = Instantiate(baseCharacterImage, CaracterPanel.transform);
                    CharacterImage.name = currentSelectedCharacter.Keeper.CharacterName;
                    CharacterImage.GetComponent<Image>().sprite = associatedSprite;


                    float value = margeY + (offsetY * (i)) + ((CharacterImage.GetComponent<Image>().rectTransform.rect.height) * (i));
                    CharacterImage.transform.localPosition = new Vector3(margeX, -value, 0.0f);
                    CharacterImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }

        }

        GameManager.Instance.CharacterPanelMenuNeedUpdate = false;
    }
}

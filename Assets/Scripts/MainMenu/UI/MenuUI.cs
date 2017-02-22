using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject CharacterPanel;
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
        for (int i = 0; i < CharacterPanel.transform.childCount; i++)
        {
            Destroy(CharacterPanel.transform.GetChild(i).gameObject);
        }

        // On selection 
        int nbCaracters = GameManager.Instance.AllKeepersList.Count;
        for (int i = 0; i < nbCaracters; i++)
        {
            KeeperInstance currentSelectedCharacter = GameManager.Instance.AllKeepersList[i];

            if (currentSelectedCharacter.IsSelectedInMenu)
            {
                Sprite associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
                if (associatedSprite != null)
                {
                    GameObject CharacterImage = Instantiate(baseCharacterImage, CharacterPanel.transform);
                    CharacterImage.name = currentSelectedCharacter.Keeper.CharacterName + ".Panel";
                    CharacterImage.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
                }
            }

        }

        GameManager.Instance.CharacterPanelMenuNeedUpdate = false;
    }
}

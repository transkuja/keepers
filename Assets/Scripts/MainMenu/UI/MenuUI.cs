using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject CaracterPanel;

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
                GameObject associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
                if (associatedSprite != null)
                {
   
                    float value = margeY + (offsetY * (i)) + ((associatedSprite.GetComponent<Image>().rectTransform.rect.height) * (i));
                    GameObject characterImage = Instantiate(associatedSprite, CaracterPanel.transform);
                    characterImage.transform.localPosition = new Vector3(margeX, -value, 0.0f);
                    characterImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                }
            }

        }

        GameManager.Instance.CharacterPanelMenuNeedUpdate = false;
    }
}

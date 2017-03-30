using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject CharacterPanel;
    public GameObject baseCharacterImage;

    public Image startButtonImg;

    void Start()
    {
        startButtonImg.enabled = false;
    }

    public void ChangeLanguage(string language)
    {
        LanguageSO.LoadDatabase(language);
    }

    public void Update()
    {
        UpdateStartButton();
    }

    public void UpdateUI()
    {
        // Clear
        for (int i = 0; i < CharacterPanel.transform.childCount; i++)
        {
            Destroy(CharacterPanel.transform.GetChild(i).gameObject);
        }

        // On selection 
        int nbCharacters = GameManager.Instance.AllKeepersListOld.Count;
        for (int i = 0; i < nbCharacters; i++)
        {
            KeeperInstance currentSelectedCharacter = GameManager.Instance.AllKeepersListOld[i];

            if (currentSelectedCharacter.IsSelectedInMenu)
            {
                Sprite associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
                if (associatedSprite != null)
                {
                    GameObject CharacterImage = Instantiate(baseCharacterImage, CharacterPanel.transform);
                    CharacterImage.name = currentSelectedCharacter.Keeper.CharacterName + ".Panel";
                    CharacterImage.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
                    CharacterImage.transform.localScale = Vector3.one;

                    //Strengh
                    CharacterImage.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Text>().text = currentSelectedCharacter.Keeper.BaseStrength.ToString();
                    //Defense
                    CharacterImage.transform.GetChild(0).GetChild(0).GetChild(1).GetComponentInChildren<Text>().text = currentSelectedCharacter.Keeper.BaseDefense.ToString();
                    //Spririt
                    CharacterImage.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text = currentSelectedCharacter.Keeper.BaseSpirit.ToString();
                    //Intelligence
                    CharacterImage.transform.GetChild(0).GetChild(0).GetChild(3).GetComponentInChildren<Text>().text = currentSelectedCharacter.Keeper.BaseIntelligence.ToString();
                }

            }

        }
    }

    public void UpdateStartButton()
    {
        if (GameManager.Instance.AllKeepersListOld.Count == 0 || GetComponent<MenuControls>().levelSelected == -1)
        {
            startButtonImg.enabled = false;
        }
        else
        {
            startButtonImg.enabled = true;
        }

    }

}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDescriptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject descriptionPanel;
    string skillDescription;

    public string SkillDescription
    {
        get
        {
            return skillDescription;
        }

        set
        {
            skillDescription = value;
        }
    }

    private void Start()
    {
        if (descriptionPanel == null)
        {
            descriptionPanel = Instantiate(GameManager.Instance.PrefabUIUtils.skillDescriptionPanel, GameManager.Instance.Ui.transform.GetChild(0));
        }
        descriptionPanel.SetActive(false);
        descriptionPanel.GetComponentInChildren<Text>().text = skillDescription;
        descriptionPanel.transform.localPosition = new Vector3(0, 200, 0);
    }

    private void OnEnable()
    {
        if (descriptionPanel == null)
        {
            descriptionPanel = Instantiate(GameManager.Instance.PrefabUIUtils.skillDescriptionPanel, GameManager.Instance.Ui.transform.GetChild(0));
        }
        descriptionPanel.SetActive(false);
        descriptionPanel.GetComponentInChildren<Text>().text = skillDescription;
        descriptionPanel.transform.localPosition = new Vector3(0, 200, 0);
    }

    private void OnDisable()
    {
        Destroy(descriptionPanel);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }
}

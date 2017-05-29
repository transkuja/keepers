using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDescriptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject descriptionPanel;
    SkillBattle skillData;

    public SkillBattle SkillData
    {
        get
        {
            return skillData;
        }

        set
        {
            skillData = value;
        }
    }

    private void Start()
    {
        RefreshDescription();
    }

    private void OnEnable()
    {
        RefreshDescription();
    }

    public void RefreshDescription()
    {
        if (descriptionPanel == null)
        {
            descriptionPanel = Instantiate(GameManager.Instance.PrefabUIUtils.skillDescriptionPanel, GameManager.Instance.Ui.transform.GetChild(0));
        }
        descriptionPanel.SetActive(false);
        descriptionPanel.GetComponentInChildren<Text>().text = skillData.Description;

        descriptionPanel.GetComponentInChildren<Text>().text += Translater.SkillDescriptionDetails(SkillDescriptionDetailsEnum.Target, skillData);
       
        if (skillData.IsMeantToHeal)
            descriptionPanel.GetComponentInChildren<Text>().text += Translater.SkillDescriptionDetails(SkillDescriptionDetailsEnum.HealValue, skillData);
        else if (skillData.Damage > 0)
            descriptionPanel.GetComponentInChildren<Text>().text += Translater.SkillDescriptionDetails(SkillDescriptionDetailsEnum.Damage, skillData);

        if (skillData.Boeufs != null && skillData.Boeufs.Length > 0)
            descriptionPanel.GetComponentInChildren<Text>().text += Translater.SkillDescriptionDetails(SkillDescriptionDetailsEnum.Effect, skillData);

        descriptionPanel.transform.localPosition = GameManager.Instance.PrefabUIUtils.skillDescriptionPanel.transform.localPosition;
        descriptionPanel.transform.localScale = Vector3.one;
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

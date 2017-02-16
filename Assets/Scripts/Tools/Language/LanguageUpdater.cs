using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class LanguageUpdater : MonoBehaviour {

	[SerializeField] private string key;

    void Awake()
    {
        LanguageSO.UpdateEvent.AddListener(UpdateL10n);
    }

    void OnDestroy()
    {
        LanguageSO.UpdateEvent.RemoveListener(UpdateL10n);
    }

    void UpdateL10n()
    {
        if (GetComponent<Text>() != null)
            GetComponent<Text>().text = LanguageSO.GetText(key);
        else if (GetComponent<Button>() != null)
            GetComponentInChildren<Text>().text = LanguageSO.GetText(key);
    }

}

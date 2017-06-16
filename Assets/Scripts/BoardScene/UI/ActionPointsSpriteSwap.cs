using UnityEngine;
using UnityEngine.UI;


public class ActionPointsSpriteSwap : MonoBehaviour {

	void Start () {
        if (Translater.CurrentLanguage == LanguageEnum.FR)
            GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteTokenActionFR;
	}

}

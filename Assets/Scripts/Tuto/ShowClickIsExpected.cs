using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowClickIsExpected : MonoBehaviour {

    bool isLeftClick = false;
    bool valueReceived = false;
    float timerSwapImg = 0.25f;
    Image img;

    public bool IsLeftClick
    {
        get
        {
            return isLeftClick;
        }

        set
        {
            isLeftClick = value;
            valueReceived = true;
        }
    }

    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = GameManager.Instance.SpriteUtils.spriteMouse;
    }

    void Update () {
		if (valueReceived)
        {
            timerSwapImg -= Time.unscaledDeltaTime;
            if (timerSwapImg < 0.0f)
            {
                if (img.sprite == GameManager.Instance.SpriteUtils.spriteMouse)
                {
                    if (isLeftClick)
                        img.sprite = GameManager.Instance.SpriteUtils.spriteMouseLeftClicked;
                    else
                        img.sprite = GameManager.Instance.SpriteUtils.spriteMouseRightClicked;
                }
                else
                    img.sprite = GameManager.Instance.SpriteUtils.spriteMouse;

                timerSwapImg = 0.25f;
            }
        }
	}
}

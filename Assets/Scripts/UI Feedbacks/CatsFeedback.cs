using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatsFeedback : MonoBehaviour {
	
    public void SendSprite(Sprite _sprite)
    {
        GetComponent<Image>().sprite = _sprite;
        GetComponent<Image>().enabled = true;
        gameObject.AddComponent<ThrowDiceButtonFeedback>();
        GetComponent<ThrowDiceButtonFeedback>().speed = 10.0f;
        Destroy(gameObject, 2.5f);
    }

}

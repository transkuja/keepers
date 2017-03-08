using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingAshley : MonoBehaviour {
    Image img;
	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();
        img.fillAmount = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        
        float h, s, v;
        Color.RGBToHSV(img.color, out h, out s, out v);
        if(v > 0)
        {
            v -= 0.012f;
            img.color = Color.HSVToRGB(h, s, v);
        }
        else
        {
            img.fillAmount -= 0.005f;
        }

	}
}

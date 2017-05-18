using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPointerOpacityTingling : MonoBehaviour {

    private Image pointer;
    private Color colorPointer;
    private float alpha;
    private float timer = 0.0f;
    private float speed = 1.0f;
    private GameObject btn;

    public float Timer
    {
        get
        {
            return timer;
        }

        set
        {
            timer = value;
        }
    }

    // Use this for initialization
    void Start () {
        pointer = GetComponent<Image>();
     
        colorPointer = pointer.color;
        alpha = colorPointer.a;
        timer = .0f;
        speed = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > 3.0f)
        {
            if (pointer.enabled == false)
            {
                pointer.enabled = true;
            }

            colorPointer = pointer.color;
            float variableQ = Mathf.Cos(timer * speed);
            colorPointer.a = alpha * (variableQ + 0.95f);
            //btn.transform.localScale = new Vector3(1 + variableQ/10, 1 + variableQ/10, 0);
            pointer.color = colorPointer;

            if (timer >= 4 * Mathf.PI)
            {
                timer -= 4 * Mathf.PI - 3.1f;
            }
        } else
        {
            if (pointer.enabled == true)
            {
                pointer.enabled = false;
            }
        }

    }
}

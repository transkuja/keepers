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

	// Use this for initialization
	void Start () {
        pointer = GetComponent<Image>();
        btn = pointer.transform.parent.GetChild(1).gameObject;
        colorPointer = pointer.color;
        alpha = colorPointer.a;
        timer = .0f;
        speed = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        colorPointer = pointer.color;
        float variableQ = Mathf.Cos(timer * speed);
        colorPointer.a = alpha * (variableQ + 0.95f);
        btn.transform.localScale = new Vector3(1 + variableQ/10, 1 + variableQ/10, 0);
        pointer.color = colorPointer;

        if (timer >= 2*Mathf.PI)
        {
            timer -= 2 * Mathf.PI;
        }
    }
}

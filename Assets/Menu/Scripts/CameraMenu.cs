using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour {

    [SerializeField] MenuManager menuManager;

    [SerializeField]
    List<Credits> credits;

    private bool resetPosition;
    float lerpParam;
    Vector3 lerpStartPosition;
    Vector3 originPos;

    public bool ResetPosition
    {
        get
        {
            return resetPosition;
        }

        set
        {
            lerpStartPosition = transform.position;
            foreach (Credits c in credits)
                c.gameObject.SetActive(true);

            resetPosition = value;
        }
    }

    void Start()
    {
        originPos = transform.position;
        transform.position += new Vector3(1.5f, 1.0f, 1.0f);
    }

    void Update()
    {
        if (resetPosition)
        {
            lerpParam += Time.deltaTime;
            if (lerpParam >= 1.0f)
            {
                resetPosition = false;
            }
            transform.position = Vector3.Lerp(lerpStartPosition, originPos, Mathf.Clamp(lerpParam, 0, 1));
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ThrowDiceButtonFeedback : MonoBehaviour {

    private float timer = 0.0f;
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float magnitude = 10.0f;
    [SerializeField]
    private Image highlightFeedback;

    private void OnEnable()
    {
        if (highlightFeedback != null)
            highlightFeedback.enabled = true;
        timer = 0.0f;
    }
    private void OnDisable()
    {
        if (highlightFeedback != null)
            highlightFeedback.enabled = false;
        transform.localScale = Vector3.one;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float variableQ = Mathf.Cos(timer * speed);
        transform.localScale = new Vector3(1 + variableQ / magnitude, 1 + variableQ / magnitude, 0);

        if (timer >= 2 * Mathf.PI)
        {
            timer -= 2 * Mathf.PI;
        }
    }
}

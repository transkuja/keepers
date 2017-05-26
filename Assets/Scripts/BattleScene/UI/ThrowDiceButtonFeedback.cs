using UnityEngine;
using UnityEngine.UI;

public class ThrowDiceButtonFeedback : MonoBehaviour {

    private float timer = 0.0f;
    [SerializeField]
    public float speed = 5.0f;
    [SerializeField]
    private float magnitude = 10.0f;
    [SerializeField]
    private Image highlightFeedback;

    Vector3 standardScale;

    private void OnEnable()
    {
        standardScale = transform.localScale;
        if (highlightFeedback != null && GameManager.Instance.CurrentState == GameState.InTuto && TutoManager.s_instance.GetComponent<SeqTutoCombat>().AlreadyPlayed == false)
            highlightFeedback.enabled = true;
        timer = 0.0f;
    }
    private void OnDisable()
    {
        if (highlightFeedback != null)
            highlightFeedback.enabled = false;
        transform.localScale = standardScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float variableQ = Mathf.Cos(timer * speed);
        transform.localScale = new Vector3(standardScale.x + variableQ / magnitude, standardScale.x + variableQ / magnitude, 0);

        if (timer >= 2 * Mathf.PI)
        {
            timer -= 2 * Mathf.PI;
        }
    }
}

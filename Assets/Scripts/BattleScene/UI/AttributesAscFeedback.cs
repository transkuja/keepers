using UnityEngine;
using UnityEngine.UI;

public class AttributesAscFeedback : MonoBehaviour {

    float timer;
    bool isReady = false;

	void Start () {
        timer = 0.7f;
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (isReady)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                transform.position += (Time.deltaTime * 100 * Vector3.up);
            }
            else
            {
                isReady = false;
                Destroy(gameObject, 0.5f);
            }
        }
    }

    public void FeedbackValue(int value)
    {
        GetComponent<Text>().text = value.ToString();
        isReady = true;
    }
}

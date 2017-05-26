using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour {

    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    float animationSpeed;

    public float GetAnimationTime()
    {
        return sprites.Length * animationSpeed;
    }

    public void Play()
    {
        if(gameObject.activeSelf)
            StartCoroutine(AnimPlay());
    }

    public IEnumerator AnimPlay()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            GetComponent<Image>().sprite = sprites[i];
            yield return new WaitForSeconds(animationSpeed);
        }
        Destroy(gameObject);
    }
}

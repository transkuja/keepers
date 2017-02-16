using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventDestroy : MonoBehaviour {

    public GameObject mainCamera;

    // Update is called once per frame
    public void Destroy() {
        mainCamera.transform.localPosition = gameObject.transform.localPosition;
        mainCamera.transform.localRotation = gameObject.transform.localRotation;
        mainCamera.SetActive(true);

        CinematiqueManager.Instance.isPlaying = false;
        Destroy(gameObject);
	}
}

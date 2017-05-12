using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowManagerMenu : MonoBehaviour
{
    public void OnEnable()
    {
        GlowController.UnregisterObject(GetComponent<GlowObjectCmd>());
    }

    public void OnDisable()
    {
        GlowController.UnregisterObject(GetComponent<GlowObjectCmd>());

    }
}

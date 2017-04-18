using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeClimat { None, Snow }

public class Climat : MonoBehaviour {

    [SerializeField]
    private TypeClimat typeClimat = TypeClimat.None;

    [SerializeField]
    private GameObject refSnowParticleSystem;

    void Start()
    {

    }

    public TypeClimat TypeClimat
    {
        get
        {
            return typeClimat;
        }

        set
        {
            if (typeClimat != value)
            {
                typeClimat = value;
                ResetAllParticleSystem();
                switch (typeClimat)
                {
                    case TypeClimat.None:
                        break;
                    case TypeClimat.Snow:
                        Debug.Log("test");
                        refSnowParticleSystem.SetActive(true);
                        break;
                }
            }
        }
    }

    public void ResetAllParticleSystem()
    {
        refSnowParticleSystem.SetActive(false);
    }
}

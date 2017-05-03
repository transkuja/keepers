using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeClimat { None, Snow, Butterfly }

public class Climat : MonoBehaviour {

    [SerializeField]
    private TypeClimat typeClimat = TypeClimat.None;

    [SerializeField]
    private GameObject refSnowParticleSystem;

    [SerializeField]
    private GameObject refButterflyParticleSystem;

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
                        if(refSnowParticleSystem!= null)
                            refSnowParticleSystem.SetActive(true);
                        break;
                    case TypeClimat.Butterfly:
                        if (refButterflyParticleSystem != null)
                            refButterflyParticleSystem.SetActive(true);
                        break;
                }
            }
        }
    }

    public void ResetAllParticleSystem()
    {
        if (refSnowParticleSystem != null)
            refSnowParticleSystem.SetActive(false);
        if (refButterflyParticleSystem != null)
            refButterflyParticleSystem.SetActive(false);
    }
}

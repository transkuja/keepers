using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationHandler : MonoBehaviour {

    public enum State
    {
        toEnd = 1,
        idle = 0,
        toDebut = -1,
    }

    public class Translation
    {
        public class KeyPose
        {
            public Transform transformReference;
            private Vector3 position;
            public Quaternion rotation;

            public KeyPose(Vector3 _pos, Quaternion _rot, Transform _trRef)
            {
                transformReference = _trRef;
                Position = _pos;
                rotation = _rot;
            }
            public Vector3 Position
            {
                get
                {
                    if(transformReference != null)
                    {
                        return position + transformReference.position;
                    }
                    else
                    {
                        return position;
                    }
                }

                set
                {
                    position = value;
                }
            }
        }

        public string name;
        public float speed;
        public List<KeyPose> listKeyPoses;

        public int iCurrentKeyPose;

        public Translation(string _name, float _speed)
        {
            listKeyPoses = new List<KeyPose>();
            name = _name;
            speed = _speed;
            iCurrentKeyPose = 0;
        }

        public void AddKeyPose(Vector3 _pos, Quaternion _rot, Transform _trRef = null)
        {
                listKeyPoses.Add(new KeyPose(_pos, _rot, _trRef));
        }
    }

    private Translation.KeyPose keyPosToFollow;     // La keypose sur laquelle il faut se recaler;
    private float fLerp;                            // La variable de qui est utiliser pour le lerp;
    private float fSpeed;                           // The calculated speed for constant mouvement between keyposes by considering the distance between them

    public List<Translation> listTranslation;       // Toutes les translations contenues que le handler peut gérer
    public Translation currentTranslation;          // La Translation actuelle
    public Translation.KeyPose startPosOverride;    // Si l'on veut forcer une position de départ
    public Transform trHandled;                     // Le Transform concerné par les translations
    [HideInInspector] public State state;           // L'état actuel de la translation courante
    public bool bFollow = false;                    // Mode pour que le trHandled se recale en permanance sur un transform de reference
    Vector3 vFrom;
    Vector3 vTo;

    private void LateUpdate() {
		if(state != State.idle)
        {
            UpdateTranslation();
        }else if (bFollow && keyPosToFollow != null)
        {
            UpdateFollow();
        }
	}

    private void UpdateTranslation()
    {
        if (bFollow)
        {
            ComputeTranslationSector();
        }

        fLerp += Time.unscaledDeltaTime * fSpeed;

        trHandled.position = Vector3.Lerp(vFrom, vTo, fLerp);

        Quaternion qFrom = (startPosOverride != null) ? startPosOverride.rotation : currentTranslation.listKeyPoses[currentTranslation.iCurrentKeyPose].rotation;
        Quaternion qTo = currentTranslation.listKeyPoses[currentTranslation.iCurrentKeyPose + (int)state].rotation;
        trHandled.rotation = Quaternion.Lerp(qFrom, qTo, fLerp);

        if(fLerp >= 1)
        {
            if(startPosOverride != null)
            {
                startPosOverride = null;
            }

            currentTranslation.iCurrentKeyPose += (int)state;

            if ((currentTranslation.iCurrentKeyPose >= currentTranslation.listKeyPoses.Count - 1) || (currentTranslation.iCurrentKeyPose <= 0))    // Si la Translation est terminée
            {
                state = State.idle;
            }
            else    // Si la translation n'est pas terminée
            {
                fLerp = 0;
                ComputeTranslationSector();
            }
        }
    }

    private void UpdateFollow()
    {
        trHandled.position = keyPosToFollow.Position;
        trHandled.rotation = keyPosToFollow.rotation;
    }

    private void ComputeTranslationSector()
    {
        vFrom = (startPosOverride != null) ? startPosOverride.Position : currentTranslation.listKeyPoses[currentTranslation.iCurrentKeyPose].Position;

        keyPosToFollow = currentTranslation.listKeyPoses[currentTranslation.iCurrentKeyPose + (int)state];

        vTo = currentTranslation.listKeyPoses[currentTranslation.iCurrentKeyPose + (int)state].Position;

        fSpeed = currentTranslation.speed / Vector3.Magnitude(vFrom - vTo);
    }

    public void Init()
    {
        listTranslation = new List<Translation>();
        currentTranslation = null;
        startPosOverride = null;
        state = State.idle;
        fLerp = 0;
    }

    public void Invert()
    {
        switch (state)
        {
            case State.toEnd:
                state = State.toDebut;
                currentTranslation.iCurrentKeyPose += 1;
                fLerp = 1 - fLerp;
                ComputeTranslationSector();
                break;
            case State.idle:
                Debug.Log("You tried to invert the translation while no translation was playing");
                break;
            case State.toDebut:
                state = State.toEnd;
                currentTranslation.iCurrentKeyPose -= 1;
                fLerp = 1 - fLerp;
                ComputeTranslationSector();
                break;
            default:
                break;
        }
    }

    public void SetCurrent(string _name)
    {
        currentTranslation = GetTranslationByName(_name);
    }

    public void SetCurrent(Translation _translation)
    {
        currentTranslation = _translation;
    }

    public void PlayCurrent(State _mode)
    {
        state = _mode;

        switch (_mode)
        {
            case State.toEnd:
                currentTranslation.iCurrentKeyPose = 0;
                fLerp = 0;
                ComputeTranslationSector();
                break;
            case State.idle:

                break;
            case State.toDebut:
                currentTranslation.iCurrentKeyPose = currentTranslation.listKeyPoses.Count - 1;
                fLerp = 0;
                ComputeTranslationSector();
                break;
            default:
                break;
        }
    }

    public void Play(string _name, State _mode, bool bFromCurPos)
    {
        if (bFromCurPos)
        {
            startPosOverride = new Translation.KeyPose(trHandled.position, trHandled.rotation, null);
        }

        SetCurrent(_name);
        PlayCurrent(_mode);
    }

    public Translation GetTranslationByName(string _name)
    {
        Translation trans = (listTranslation.Find(t => t.name == _name));

        if (trans == null)
        {
            Debug.Log("The translation \"" + _name + "\" doesn't exists");
        }

        return trans;
    }

    public Translation BrowseTranslations(bool bPrevious = false)
    {
        int i = listTranslation.FindIndex(t => t == currentTranslation);

        if (bPrevious)
        {
            return listTranslation[(i + 1) % listTranslation.Count];
        }
        else
        {
            return listTranslation[(i - 1 + listTranslation.Count) % listTranslation.Count];
        }

    }
}

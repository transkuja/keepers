using UnityEngine;

[System.Serializable]
public class Die {
    [SerializeField]
    int nbrOfFaces;
    [SerializeField]
    Face[] faces;

    public int NbrOfFaces
    {
        get
        {
            return nbrOfFaces;
        }

        set
        {
            nbrOfFaces = value;
        }
    }

    public Face[] Faces
    {
        get
        {
            return faces;
        }

        set
        {
            faces = value;
        }
    }


    public Die()
    {

    }

    public Die(int _nbrOfFaces, Face[] _faces)
    {
        nbrOfFaces = _nbrOfFaces;
        faces = _faces;
    }
}

[System.Serializable]
public class Face
{
    [SerializeField]
    FaceType type;
    [SerializeField]
    int value;

    public Face()
    {

    }

    public Face(FaceType _type, int _value)
    {
        type = _type;
        value = _value;
    }

    public FaceType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public int Value
    {
        get
        {
            return value;
        }

        set
        {
            this.value = value;
        }
    }
}

[System.Serializable]
public enum FaceType
{
    Physical,
    Magical,
    Defensive,
    Support
}
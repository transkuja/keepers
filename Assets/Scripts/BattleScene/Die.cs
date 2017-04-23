using UnityEngine;

public enum DieFaceChildren { Edge, Back, Down, Front, Left, Right, Up }

[System.Serializable]
public class Die {
    [SerializeField]
    Face[] faces;

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

    public Die(Face[] _faces)
    {
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
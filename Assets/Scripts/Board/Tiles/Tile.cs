using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


public enum TileType
{
    None,
    Mountain,
    Plain,
    Snow
}

public enum Direction
{
    North,
    North_East,
    South_East,
    South,
    South_West,
    North_West,
    None
}

public class Tile : MonoBehaviour{
    [SerializeField]
    private TileType type;

    private static bool showLinks;

    /* Neighbors:
        Indexes -> Direction
        0 -> North
        1 -> North East
        2 -> South East
        3 -> South
        4 -> South West
        5 -> North West
    */
    private const int NEIGHBORSCOUNT = 6;
    [SerializeField]
    private Tile[] neighbors = new Tile[NEIGHBORSCOUNT];

    private List<Monster> monstersOnTile = new List<Monster>();

    //Accessors
    public TileType Type
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

    public Tile[] Neighbors
    {
        get
        {
            return neighbors;
        }

        set
        {
            neighbors = value;
        }
    }


    public Tile North
    {
        get
        {
            return Neighbors[(int)Direction.North];
        }

        set
        {
            Neighbors[(int)Direction.North] = value;
        }
    }

    public Tile North_East
    {
        get
        {
            return Neighbors[(int)Direction.North_East];
        }

        set
        {
            Neighbors[(int)Direction.North_East] = value;
        }
    }

    public Tile South_East
    {
        get
        {
            return Neighbors[(int)Direction.South_East];
        }

        set
        {
            Neighbors[(int)Direction.South_East] = value;
        }
    }

    public Tile South
    {
        get
        {
            return Neighbors[(int)Direction.South];
        }

        set
        {
            Neighbors[(int)Direction.South] = value;
        }
    }

    public Tile South_West
    {
        get
        {
            return Neighbors[(int)Direction.South_West];
        }

        set
        {
            Neighbors[(int)Direction.South_West] = value;
        }
    }

    public Tile North_West
    {
        get
        {
            return Neighbors[(int)Direction.North_West];
        }

        set
        {
            Neighbors[(int)Direction.North_West] = value;
        }
    }

    public static bool ShowLinks
    {
        get
        {
            return showLinks;
        }

        set
        {
            showLinks = value;
        }
    }

    public List<Monster> MonstersOnTile
    {
        get
        {
            return monstersOnTile;
        }

        set
        {
            monstersOnTile = value;
        }
    }

    // Debug functions
    void OnDrawGizmos()
    {
        if(showLinks)
        {
            Gizmos.color = Color.red;
            if (Neighbors[(int)Direction.North] != null)
                Gizmos.DrawLine(transform.position + Vector3.up * 1.1f, Neighbors[(int)Direction.North].transform.position + Vector3.up * 1.1f);
            if (Neighbors[(int)Direction.North_East] != null)
                Gizmos.DrawLine(transform.position + Vector3.up * 1.1f, Neighbors[(int)Direction.North_East].transform.position + Vector3.up * 1.1f);
            Gizmos.color = Color.blue;
            if (Neighbors[(int)Direction.South_East] != null)
                Gizmos.DrawLine(transform.position + Vector3.up, Neighbors[(int)Direction.South_East].transform.position + Vector3.up);
            if (Neighbors[(int)Direction.South] != null)
                Gizmos.DrawLine(transform.position + Vector3.up, Neighbors[(int)Direction.South].transform.position + Vector3.up);
            if (Neighbors[(int)Direction.South_West] != null)
                Gizmos.DrawLine(transform.position + Vector3.up, Neighbors[(int)Direction.South_West].transform.position + Vector3.up);
            Gizmos.color = Color.red;
            if (Neighbors[(int)Direction.North_West] != null)
                Gizmos.DrawLine(transform.position + Vector3.up * 1.1f, Neighbors[(int)Direction.North_West].transform.position + Vector3.up * 1.1f);
        }
    }
}

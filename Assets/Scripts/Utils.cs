using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

	public static Direction GetOppositeDirection(Direction from)
    {
        switch (from)
        {
            case Direction.North:
                return Direction.South;
            case Direction.North_East:
                return Direction.South_West;
            case Direction.North_West:
                return Direction.South_East;
            case Direction.South:
                return Direction.North;
            case Direction.South_East:
                return Direction.North_West;
            case Direction.South_West:
                return Direction.North_East;
            default:
                return from;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PerfectMazeProject.DirectionExtensions
{
    public enum Direction
    {
        North, South, East, West
    }

    public class Directions
    {
        public static Direction Opposite(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return Direction.South;
                case Direction.South: return Direction.North;
                case Direction.East: return Direction.West;
                case Direction.West: return Direction.East;
                default: throw new ArgumentOutOfRangeException(direction.ToString());
            }
        }
    }
}


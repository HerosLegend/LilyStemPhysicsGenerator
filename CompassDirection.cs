using System;
using System.Linq;
using UnityEngine;

namespace BlindkatStudios.Movement {
    public static class CompassDirection {

            public enum Direction {
            right = 0,
            upRight,
            up,
            leftUp,
            left,
            leftDown,
            down,
            rightDown,

        }
        
        // List of 8 directional vectors
        public static readonly Vector2[] DirectionVectors = new[] {
            
                // Right
                new Vector2(1, 0),

                // Up Right
                new Vector2(1, 1),

                // Up
                new Vector2(0, 1),

                // Left up
                new Vector2(-1, 1),

                // Left
                new Vector2(-1, 0),

                // Left down
                new Vector2(-1, -1),

                // Down
                new Vector2(0, -1),

                // Right down
                new Vector2(1, -1)
            };
        
        // Read only array of compass directions
        public static readonly Direction[] AllDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray();

        /// <summary>
        /// Returns a compass direction based on a float passed in
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Direction GetCompassDirection(float value) {

            // Normalise the angle between 0 and 359
            value %= 360;

            // Adjust for negative angles
            if (value < 0) value += 360;

            // Create an offset, so we have wiggle room for each side of the value, example -10, 10 will result in right direction.
            var offset = 45 / 2;

            // Divide the angle by 45 (8 enum values)
            int index = Mathf.FloorToInt((value + offset) / 45f);

            // Check out of bounds
            if (index < 0 || index >= AllDirections.Length) {

                // Return first object in array
                return AllDirections[0];
            }

            // Return the indexed facing direction
            return AllDirections[index];

        }
    }
}

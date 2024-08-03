using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

namespace DefineExtension
{
    public static class CharacterExtension
    {
        public static Vector2Int GetTileDirection(this Character.Direction _direction)
        {
            switch (_direction)
            {
                case Character.Direction.Up:
                    return new Vector2Int(0, 1);
                case Character.Direction.Down:
                    return new Vector2Int(0, -1);
                case Character.Direction.Left:
                    return new Vector2Int(-1, 0);
                case Character.Direction.Right:
                    return new Vector2Int(1, 0);
            }

            return Vector2Int.zero;
        }
    }
}

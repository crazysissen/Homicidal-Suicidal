using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace HomicidalSuicidal
{
    public enum Direction { Up, Down, Left, Right }

    public static class ExtensionMethods
    {
        public static Vector2 VelocityNullifier(this Vector2 vector) => new Vector2(vector.X != 0 ? 1 : 0, vector.Y != 0 ? 1 : 0);
    }
}

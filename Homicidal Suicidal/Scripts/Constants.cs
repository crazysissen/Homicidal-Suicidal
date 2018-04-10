using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna;

namespace HomicidalSuicidal
{
    static class Constants
    {
        public const float screenWidthWorldUnits = 10;
        public static Point CoordinateOnScreen(WorldObject worldObject, Vector2 camera)
        {
            Vector2 calculatedPosition = (worldObject.Position - camera) / screenWidthWorldUnits;
            return calculatedPosition.ToPoint();
        }

        public const float gravity = 9.81f;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    public static class World
    {
        static Vector2 PlayerPosition => Game1.Player.CenterPosition;

        static Texture2D ground, wall, platform;

        static readonly Point   groundSize = new Point(80, 80),
                                wallSize = new Point(900, 900),
                                platformSize = new Point(120, 30);

        public static void Initialize()
        {

        }

        public static void Update()
        {

        }
    }
}

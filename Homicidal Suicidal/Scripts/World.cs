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
        static readonly Point groundSize = new Point(80, 80),
                        wallSize = new Point(900, 900),
                        platformSize = new Point(120, 30);

        static Vector2 PlayerPosition => Game1.Player.CenterPosition;

        static Texture2D ground, wall, platform;

        static float nextZoneLoad;
        static int nextZoneIndex, currentSeed;

        static WorldZone[] currentZones;

        public static void Initialize(int initialSeed)
        {
            Random tempRandom = new Random(initialSeed);
            currentSeed = tempRandom.Next(0, (int)(0.5f * int.MaxValue));

            LoadZone(0);
            LoadZone(1);

            nextZoneLoad = WorldZone.groundNumber * groundSize.X; 
        }

        static void LoadZone(int index)
        {


            nextZoneLoad += WorldZone.groundNumber * groundSize.X;
            ++nextZoneIndex;
        }

        static void MoveZoneArray()
        {
            currentZones[0].DestroyZone();
            currentZones[0] = currentZones[1];
            currentZones[1] = currentZones[2];
        }

        public static void Update()
        {

        }
    }
}

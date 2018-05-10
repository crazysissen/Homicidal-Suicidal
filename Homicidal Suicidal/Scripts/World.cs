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

        static int nextZoneIndex, currentSeed;

        static WorldZone[] currentZones;

        public static void Initialize(int initialSeed)
        {
            currentZones = new WorldZone[3];
            Random tempRandom = new Random(initialSeed);
            currentSeed = tempRandom.Next(0, (int)(0.5f * int.MaxValue));

            currentZones[1] = LoadZone(0);
            currentZones[2] = LoadZone(1);

            nextZoneIndex = 2;
        }

        static WorldZone LoadZone(int zoneIndex) => new WorldZone(++currentSeed, zoneIndex * WorldZone.groundNumber * WorldZone.groundSize.X);

        static void Proceed()
        {
            if (currentZones[0] != null)
                currentZones[0].DestroyZone();

            currentZones[0] = currentZones[1];
            currentZones[1] = currentZones[2];
            currentZones[2] = LoadZone(nextZoneIndex);

            ++nextZoneIndex;
        }

        public static void Update(float deltaTime)
        {
            if (Player.MainPlayer.Position.X > WorldZone.groundNumber * WorldZone.groundSize.X * (nextZoneIndex - 1))
            {
                Proceed();
            }
        }
    }
}

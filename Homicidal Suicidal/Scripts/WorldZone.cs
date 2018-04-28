using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    [Serializable]
    class WorldZone
    {
        const int groundNumber = 16,
                  minPlatforms = 1,
                  maxPlatforms = 3,
                  minEnemies = 1,
                  maxEnemies = 2;

        readonly Point groundSize = new Point(80, 80);

        int worldIndex;

        public Vector2[] GroundPositions { get; private set; }
        public Vector2[] PlatformPositions { get; private set; }
        public object[] Enemies { get; private set; }

        public void Generate(int seed)
        {
            Random r = new Random(seed);

            for (int i = 0; i < groundNumber; ++i)
            {
                //StaticObject newGround = new StaticObject()
            }

            int enemyCount = r.Next(minEnemies, maxEnemies + 1);
            for (int i = 0; i < enemyCount; ++i)
            {
                Vector2 position = new Vector2(((float)i + 1) / ((float)enemyCount + 1)/*TODO*/);
            }
        }

        public void Initialize()
        {

        }

        int NewIndex()
        {
            ++worldIndex;
            return worldIndex;
        }
    }
}

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
        public const int
            groundNumber = 16,
            minPlatforms = 1,
            maxPlatforms = 3,
            minEnemies = 1,
            maxEnemies = 2,
            minGroundHeight = -40,
            maxGroundHeight = 40,
            maxEnemyOffset = 30,
            minPlatformHeight = 180,
            maxPlatformHeight = 540,
            screenBottomTechnical = 1080;

        readonly Point 
            groundSize = new Point(240, 160),
            platformSize = new Point(100, 20);

        int worldIndex;

        public StaticObject[] GroundTiles { get; private set; }
        public StaticObject[] Platforms { get; private set; }
        public WorldObject[] Enemies { get; private set; }

        public WorldZone(int seed, int xDislocation) => Generate(seed, xDislocation);

        public void Generate(int seed, int xDislocation)
        {
            Random r = new Random(seed);

            List<Vector2> possibleEnemyPositions = new List<Vector2>();

            GroundTiles = new StaticObject[groundNumber];
            Platforms = new StaticObject[r.Next(minPlatforms, maxPlatforms + 1)];
            Enemies = new WorldObject[r.Next(minEnemies, maxEnemies + 1)];
                
            for (int i = 0; i < GroundTiles.Length; ++i)
            {
                GroundTiles[i] = new StaticObject("GroundTile[" + NewIndex() + "]", new Rectangle(groundSize.X * i + xDislocation, r.Next(minGroundHeight, maxGroundHeight), groundSize.X, groundSize.Y), Game1.AllSprites["Floor"]);
                possibleEnemyPositions.Add(GroundTiles[i].CenterPosition + new Vector2(r.Next(-maxEnemyOffset, maxEnemyOffset + 1), -GroundTiles[i].Offset.Y * 0.5f));
            }

            for (int i = 0; i < Platforms.Length; ++i)
            {
                Platforms[i] = new StaticObject("Platform[" + NewIndex() + "]", new Rectangle(r.Next(groundSize.X, groundSize.X * (groundNumber - 1) + 1), -r.Next(minPlatformHeight, maxPlatformHeight + 1), platformSize.X, platformSize.Y), /*TODO*/ Game1.AllSprites["Square"]);
                possibleEnemyPositions.Add(Platforms[i].CenterPosition + new Vector2(r.Next(-maxEnemyOffset, maxEnemyOffset + 1), -Platforms[i].Offset.Y * 0.5f));
            }

            int enemyCount = r.Next(minEnemies, maxEnemies + 1);
            for (int i = 0; i < enemyCount; ++i)
            {
                int positionIndex = r.Next(0, possibleEnemyPositions.Count);
                int enemyType = r.Next(0, 1); // TODO
                Vector2 offset;

                if (enemyType == 0)
                {
                    // Default template for DoctorEnemy
                    Enemies[i] = new DoctorEnemy("Doctor[" + NewIndex() + "]", true, Game1.AllSprites["Doctor_Attack"], Color.White, new Point(240, 145), possibleEnemyPositions[positionIndex] - new Vector2(120, 145), 3, 100, 50, 9999, 20);
                    //Enemies[i].CenterPosition = possibleEnemyPositions[positionIndex] - Enemies[i].Offset;
                }

                possibleEnemyPositions.RemoveAt(positionIndex);
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

        public void DestroyZone()
        {
            foreach (StaticObject current in GroundTiles)
                current.DestroyObject();

            foreach (StaticObject current in Platforms)
                current.DestroyObject();

            foreach (WorldObject current in Enemies)
                current.DestroyObject();
        }
    }
}

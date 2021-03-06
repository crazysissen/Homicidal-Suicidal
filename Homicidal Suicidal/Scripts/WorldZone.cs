﻿using System;
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
            minPlatforms = 6,
            maxPlatforms = 12,
            minEnemies = 5,
            maxEnemies = 8,
            minGroundHeight = -40,
            maxGroundHeight = 40,
            maxEnemyOffset = 30,
            minPlatformHeight = 180,
            maxPlatformHeight = 540,
            screenBottomTechnical = 1080,
            wallCount = 3;

        public static readonly Point 
            groundSize = new Point(240, 160),
            platformSize = new Point(200, 20);

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

            int wallWidth = (groundSize.X * groundNumber) / wallCount;
            for (int i = 0; i < wallCount; ++i)
            {
                RenderedObject wall = new RenderedObject("Wall[" + NewIndex() + "]", new Rectangle(xDislocation + i * wallWidth, -minGroundHeight - wallWidth, wallWidth, wallWidth), Game1.AllSprites["Wall"], Color.DarkGray, 1);
            }

            for (int i = 0; i < GroundTiles.Length; ++i)
            {
                GroundTiles[i] = new StaticObject("GroundTile[" + NewIndex() + "]", new Rectangle(groundSize.X * i + xDislocation, r.Next(minGroundHeight, maxGroundHeight), groundSize.X, groundSize.Y), Game1.AllSprites["Floor"], 0.5f);
                GroundTiles[i].Tags.Add("Ground");
                possibleEnemyPositions.Add(GroundTiles[i].CenterPosition + new Vector2(r.Next(-maxEnemyOffset, maxEnemyOffset + 1), -GroundTiles[i].Offset.Y * 0.5f));
            }

            for (int i = 0; i < Platforms.Length; ++i)
            {
                Platforms[i] = new StaticObject("Platform[" + NewIndex() + "]", new Rectangle(r.Next(groundSize.X, groundSize.X * (groundNumber - 1) + 1) + xDislocation, -r.Next(minPlatformHeight, maxPlatformHeight + 1), platformSize.X, platformSize.Y), Game1.AllSprites["Platform"], 0.5f);
                Platforms[i].Tags.Add("Ground");
                possibleEnemyPositions.Add(Platforms[i].CenterPosition + new Vector2(r.Next(-maxEnemyOffset, maxEnemyOffset + 1), -Platforms[i].Offset.Y * 0.5f));
            }

            int enemyCount = r.Next(minEnemies, maxEnemies + 1);
            for (int i = 0; i < Enemies.Length; ++i)
            {
                int positionIndex = r.Next(0, possibleEnemyPositions.Count);
                int enemyType = r.Next(0, 3);

                if (enemyType == 0)
                {
                    // Default template for DoctorEnemy
                    Enemies[i] = new DoctorEnemy("Doctor[" + NewIndex() + "]", true, Game1.AllSprites["Doctor_Attack"], Color.White, new Point(240, 145), possibleEnemyPositions[positionIndex] - new Vector2(120, 145), 6, 100, 15, 600, 0.4f);
                    Enemies[i].Tags.Add("Enemy");
                }

                if (enemyType == 1)
                {
                    Enemies[i] = new NurseEnemy("Nurse[" + NewIndex() + "]", true, Color.White, new Point(240, 180), possibleEnemyPositions[positionIndex] - new Vector2(120, 180), 0.1f, 250, 0.4f);
                    Enemies[i].Tags.Add("Enemy");
                }

                if (enemyType == 2)
                {
                    Enemies[i] = new SurgeonEnemy("Surgeon[" + NewIndex() + "]", true, Game1.AllSprites["Surgeon_Idle"], Color.White, new Point(140, 135), possibleEnemyPositions[positionIndex] - new Vector2(70, 135), 4, 100, 5, 600, 0.4f);
                    Enemies[i].Tags.Add("Enemy");
                }

                possibleEnemyPositions.RemoveAt(positionIndex);
            }
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
                if (current != null)
                    current.DestroyObject();
        }
    }
}

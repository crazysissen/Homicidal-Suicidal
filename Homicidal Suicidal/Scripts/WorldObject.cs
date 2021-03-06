﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace HomicidalSuicidal
{
    public abstract class WorldObject
    {
        public static Dictionary<string, WorldObject> WorldObjects { get; private set; }

        public List<string> Tags { get; private set; }

        List<WorldObject> touching;

        static int worldObjectIndex;

        string name;
        public string Name
        {
            get => name;

            set
            {
                if (WorldObjects.ContainsKey(value))
                {
                    return;
                }

                WorldObjects.Remove(name);
                WorldObjects.Add(value, this);

                name = value;
            }
        }

        public Point Size { get; set; } 
        public Vector2 Position { get; set; }
        public Vector2 Offset => new Vector2((float)Size.X * 0.5f, (float)Size.Y * 0.5f);
        public Vector2 CenterPosition { get => Position + Offset; set => Position = value - Offset; }
        public Rectangle Rect => new Rectangle(Position.ToPoint(), Size);

        public float hitboxRightInset = 1, hitboxLeftInset = 1, hitboxUpInset = 1, hitboxDownInset = 1;

        public virtual PhysicsObject PhysObject => null;

        public virtual IRenderable Renderable => null;

        protected abstract object Component { get; }
        
        public T GetComponent<T>(out bool successful)
        {
            successful = (Component is T) ? true : false;
            return (Component is T) ? (T)Component : default(T);
        }

        public static WorldObject Get(string name) => WorldObjects[name];

        public WorldObject(string name)
        {
            Tags = new List<string>();

            if (WorldObjects == null)
                WorldObjects = new Dictionary<string, WorldObject>();

            CheckNameAvaliability(name, out string tempName);

            WorldObjects.Add(tempName, this);
            name = tempName;
        }

        public WorldObject(string name, out string trueName)
        {
            Tags = new List<string>();

            if (WorldObjects == null)
                WorldObjects = new Dictionary<string, WorldObject>();

            if (!CheckNameAvaliability(name, out string tempName))
            {
                Console.WriteLine("WorldObject name already taken, generating automatic name.");
            }

            trueName = tempName;
            WorldObjects.Add(tempName, this);
            name = tempName;
        }

        public WorldObject(string name, Rectangle rectangle)
        {
            Tags = new List<string>();

            if (WorldObjects == null)
                WorldObjects = new Dictionary<string, WorldObject>();

            if (!CheckNameAvaliability(name, out string tempName))
            {
                Console.WriteLine("WorldObject name already taken, generating automatic name.");
            }

            Position = rectangle.Location.ToVector2();
            Size = rectangle.Size;

            WorldObjects.Add(tempName, this);
            name = tempName;
        }

        public WorldObject(string name, out string trueName, Rectangle rectangle)
        {
            Tags = new List<string>();

            if (WorldObjects == null)
                WorldObjects = new Dictionary<string, WorldObject>();

            if (!CheckNameAvaliability(name, out string tempName))
            {
                Console.WriteLine("WorldObject name already taken, generating automatic name.");
            }

            Position = rectangle.Location.ToVector2();
            Size = rectangle.Size;

            trueName = tempName;
            WorldObjects.Add(tempName, this);
            name = tempName;
        }

        public static void InitializeClass() => WorldObjects = new Dictionary<string, WorldObject>();

        public List<string> IntersectTags()
        {
            List<string> returnList = new List<string>();

            foreach (KeyValuePair<string, WorldObject> subject in WorldObjects)
            {
                if ((subject.Value.CenterPosition - CenterPosition).Length() < Constants.collisionIgnoreDistance)
                    if (Intersects(subject.Value))
                        returnList.AddRange(subject.Value.Tags);
            }

            return returnList;
        }

        public static void UpdateAllPhysics(GameTime gameTime)
        {
            for (int i = 0; i < WorldObjects.Count; ++i)
            {
                if (WorldObjects.ElementAt(i).Value.PhysObject != null)
                    WorldObjects.ElementAt(i).Value.PhysObject.UpdateMovement(gameTime, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public static void UpdateAllDerived(GameTime gameTime)
        {
            for (int i = 0; i < WorldObjects.Count; ++i)
            {
                WorldObjects.ElementAt<KeyValuePair<string, WorldObject>>(i).Value.Update(gameTime, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public static void UpdateAllCollision()
        {
            bool[,] calculated = new bool[WorldObjects.Count, WorldObjects.Count];
            List<WorldObject[]> toCollide = new List<WorldObject[]>();

            int i = 0, j = 0;

            foreach (KeyValuePair<string, WorldObject> pair in WorldObjects)
            {
                if (pair.Value.PhysObject != null)
                {
                    foreach (KeyValuePair<string, WorldObject> subject in WorldObjects)
                    {
                        if (subject.Value.PhysObject != null)
                            if ((subject.Value.CenterPosition - pair.Value.CenterPosition).Length() < Constants.collisionIgnoreDistance)
                                if (!calculated[i, j] && !calculated[j, i] && j != i && !pair.Value.PhysObject.Ignores(subject.Value) && !subject.Value.PhysObject.Ignores(pair.Value))
                                    if (pair.Value.Intersects(subject.Value))
                                        toCollide.Add(new WorldObject[] { pair.Value, subject.Value });

                        calculated[i, j] = true;
                        calculated[j, i] = true;

                        ++j;
                    }
                }

                j = 0;
                ++i;
            }

            for (int k = 0; k < toCollide.Count; ++k)
            {
                toCollide[k][0].PhysObject.Collide(toCollide[k][1].PhysObject);
            }
        }

        public bool Intersects(WorldObject worldObject)
        {
            float   top = CenterPosition.Y - hitboxUpInset * (float)Size.Y / 2,
                    right = CenterPosition.X + hitboxRightInset * (float)Size.X / 2,
                    bottom = CenterPosition.Y + hitboxDownInset * (float)Size.Y / 2,
                    left = CenterPosition.X - hitboxLeftInset * (float)Size.X / 2,

                    thatTop = worldObject.CenterPosition.Y - worldObject.hitboxUpInset * (float)worldObject.Size.Y / 2,
                    thatRight = worldObject.CenterPosition.X + worldObject.hitboxRightInset * (float)worldObject.Size.X / 2,
                    thatBottom = worldObject.CenterPosition.Y + worldObject.hitboxDownInset * (float)worldObject.Size.Y / 2,
                    thatLeft = worldObject.CenterPosition.X - worldObject.hitboxLeftInset * (float)worldObject.Size.X / 2;

            return  ((top >= thatTop && top <= thatBottom) || (bottom <= thatBottom && bottom >= thatTop)) && 
                    ((left >= thatLeft && left <= thatRight) || (right <= thatRight && right >= thatLeft)) ||
                    ((thatTop >= top && thatTop <= bottom) || (thatBottom <= bottom && thatBottom >= top)) && 
                    ((thatLeft >= left && thatLeft <= right) || (thatRight <= right && thatRight >= left));
        } 

        public Vector2 ClosestOffset(WorldObject worldObject)
        {
            float[] distances = new float[]
            {
                (CenterPosition.X + hitboxRightInset * (Size.X * 0.5f)) - (worldObject.CenterPosition.X - worldObject.hitboxRightInset * (worldObject.Size.X * 0.5f)), // Left
                (worldObject.CenterPosition.X + worldObject.hitboxLeftInset * worldObject.Size.X * 0.5f) - (CenterPosition.X - hitboxLeftInset * Size.X * 0.5f), // Right
                (CenterPosition.Y + hitboxDownInset * (Size.Y * 0.5f)) - (worldObject.CenterPosition.Y - worldObject.hitboxDownInset * (worldObject.Size.Y * 0.5f)), // Up
                (worldObject.CenterPosition.Y + worldObject.hitboxUpInset * worldObject.Size.Y * 0.5f) - (CenterPosition.Y - hitboxUpInset * Size.Y * 0.5f)  // Down
            };

            Vector2[] directions = new Vector2[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1) };

            int index = 0;
            for (int i = 1; i < distances.Length; ++i)
            {
                if (distances[i] < distances[index])
                {
                    index = i;
                }
            }

            return directions[index] * distances[index];
        }

        /// <summary>
        /// Is this name avaliable? Returns true if so and false otherwise. 
        /// Outs endName representing the input if it is avaliable or a randomly generated one otherwise.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <param name="endName">The name after check.</param>
        /// <returns></returns>
        public bool CheckNameAvaliability(string name, out string endName)
        {
            if (WorldObjects.ContainsKey(name))
            {
                endName = "WorldObject[" + worldObjectIndex + "]";
                ++worldObjectIndex;
                return false;
            }

            endName = name;
            return true;
        }

        /// <summary>
        /// Is this name avaliable? Returns true if so and false otherwise. 
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns></returns>
        public bool NameAvaliability(string name) => !WorldObjects.ContainsKey(name);

        /// <summary>
        /// Called every frame.
        /// </summary>
        /// <param name="gameTime">Game timing values.</param>
        /// <param name="deltaTime">Shorthand for <code>(float)gameTime.ElapsedGameTime.TotalSeconds</code></param>
        protected virtual void Update(GameTime gameTime, float deltaTime)
        {
            return;
        }

        public void DestroyObject()
        {
            for (int i = 0; i < WorldObjects.Count; ++i)
            {
                if (WorldObjects.ElementAt(i).Value.Equals(this))
                {
                    WorldObjects.Remove(WorldObjects.ElementAt(i).Key);
                }
            }
        }

        public static void DestroyAllObjects()
        {
            List<WorldObject> tempList = new List<WorldObject>();
            foreach (KeyValuePair<string, WorldObject> pair in WorldObjects)
            {
                tempList.Add(pair.Value);
            }

            for (int i = tempList.Count - 1; i >= 0; --i)
            {
                tempList[i].DestroyObject();
            }
        }
    }
}

using System;
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
                    Console.WriteLine("Tried to change name to one that already exists, aborting.");
                    return;
                }

                WorldObjects.Remove(name);
                WorldObjects.Add(value, this);

                name = value;
            }
        }

        public Point Size { get; set; } 
        public Vector2 Position { get; set; }
        public Vector2 Offset => new Vector2((float)Size.X / 2, (float)Size.Y / 2);
        public Vector2 CenterPosition => Position + Offset; 
        public Rectangle Rect => new Rectangle(Position.ToPoint(), Size); 

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

            if (!CheckNameAvaliability(name, out string tempName))
            {
                Console.WriteLine("WorldObject name already taken, generating automatic name.");
                
            }

            WorldObjects.Add(tempName, this);
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
        }

        public static void InitializeClass() => WorldObjects = new Dictionary<string, WorldObject>();

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

            for (int i = 0; i < WorldObjects.Count; ++i)
            {
                if (WorldObjects.ElementAt<KeyValuePair<string, WorldObject>>(i).Value.PhysObject != null)
                {
                    for (int j = 0; j < WorldObjects.Count; ++j)
                    {
                        if (WorldObjects.ElementAt<KeyValuePair<string, WorldObject>>(j).Value.PhysObject != null)
                        {
                            KeyValuePair<string, WorldObject> pair = WorldObjects.ElementAt<KeyValuePair<string, WorldObject>>(i);
                            KeyValuePair<string, WorldObject> subject = WorldObjects.ElementAt<KeyValuePair<string, WorldObject>>(j);

                            if (!calculated[i, j] && subject.Value.PhysObject != null && pair.Value != subject.Value)
                            {
                                if (pair.Value.Intersects(subject.Value))
                                {
                                    pair.Value.PhysObject.Collide(subject.Value.PhysObject);

                                }
                            }

                            calculated[i, j] = true;
                            calculated[j, i] = true;
                        }
                    }
                }
            }
        }

        public bool Intersects(WorldObject worldObject)
        {
            float   top = CenterPosition.Y - (float)Size.Y / 2,
                    right = CenterPosition.X + (float)Size.X / 2,
                    bottom = CenterPosition.Y + (float)Size.Y / 2,
                    left = CenterPosition.X - (float)Size.X / 2,

                    thatTop = worldObject.CenterPosition.Y - (float)worldObject.Size.Y / 2,
                    thatRight = worldObject.CenterPosition.X + (float)worldObject.Size.X / 2,
                    thatBottom = worldObject.CenterPosition.Y + (float)worldObject.Size.Y / 2,
                    thatLeft = worldObject.CenterPosition.X - (float)worldObject.Size.X / 2;

            return  ((top >= thatTop && top <= thatBottom) || (bottom <= thatBottom && bottom >= thatTop)) && 
                    ((left >= thatLeft && left <= thatRight) || (right <= thatRight && right >= thatLeft)) ||
                    ((thatTop >= top && thatTop <= bottom) || (thatBottom <= bottom && thatBottom >= top)) && 
                    ((thatLeft >= left && thatLeft <= right) || (thatRight <= right && thatRight >= left));
        } 

        public Vector2 ClosestOffset(WorldObject worldObject)
        {
            float[] distances = new float[]
            {
                (CenterPosition.X + (Size.X * 0.5f)) - (worldObject.CenterPosition.X - (worldObject.Size.X * 0.5f)), // Left
                (worldObject.CenterPosition.X + worldObject.Size.X * 0.5f) - (CenterPosition.X - Size.X * 0.5f), // Right
                (CenterPosition.Y + (Size.Y * 0.5f)) - (worldObject.CenterPosition.Y - (worldObject.Size.Y * 0.5f)), // Up
                (worldObject.CenterPosition.Y + worldObject.Size.Y * 0.5f) - (CenterPosition.Y - Size.Y * 0.5f)  // Down
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
    }
}

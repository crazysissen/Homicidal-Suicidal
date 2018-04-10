using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace HomicidalSuicidal
{
    abstract class WorldObject
    {
        public static Dictionary<string, WorldObject> WorldObjects { get; private set; }

        public List<string> Tags { get; private set; }

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
        public Rectangle Rect { get => new Rectangle(Position.ToPoint(), Size); }

        public virtual PhysicsObject PhysObject { get => null; }

        public virtual IRenderable Renderable
        {
            get
            {
                Console.WriteLine("Tried to get nonexistent IRenderable. Object: " + Name);
                return null;
            }
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

        public static void UpdateAllDerived(GameTime gameTime)
        {
            foreach(KeyValuePair<string, WorldObject> pair in WorldObjects)
            {
                pair.Value.Update(gameTime, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        
        //public bool Intersects(WorldObject worldObject)
        //{
        //    if (Position )
        //}

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

        public static void UpdateAll(GameTime gameTime)
        {
            foreach (KeyValuePair<string, WorldObject> pair in WorldObjects)
            {
                pair.Value.Update(gameTime, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}

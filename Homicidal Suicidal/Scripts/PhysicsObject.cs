using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    abstract class PhysicsObject : WorldObject
    {
        public Vector2 Velocity { get; protected set; }
        public float GravityMultiplier { get; protected set; }
        public bool Kinematic { get; protected set; }
        public string[] CollisionTags { get; protected set; }
        public override PhysicsObject PhysObject => this;

        public virtual void OnCollision(string[] tags) { }

        public void UpdateMovement(GameTime gameTime, float deltaTime)
        {
            Velocity += new Vector2(0, 1) * Constants.gravity * deltaTime;
            Position += Velocity;
        }

        public PhysicsObject(Vector2 initialVelocity, float gravityMultiplier, string name) : base(name)
        {
            GravityMultiplier = gravityMultiplier;
            Velocity = initialVelocity;
        }

        public PhysicsObject(Vector2 initialVelocity, float gravityMultiplier, string name, out string trueName) : base(name, out trueName)
        {
            GravityMultiplier = gravityMultiplier;
            Velocity = initialVelocity;
        }

        public PhysicsObject(Vector2 initialVelocity, float gravityMultiplier, string name, Rectangle rectangle) : base(name, rectangle)
        {
            GravityMultiplier = gravityMultiplier;
            Velocity = initialVelocity;
        }

        public PhysicsObject(Vector2 initialVelocity, float gravityMultiplier, string name, out string trueName, Rectangle rectangle) : base(name, out trueName, rectangle)
        {
            GravityMultiplier = gravityMultiplier;
            Velocity = initialVelocity;
        }
    }
}

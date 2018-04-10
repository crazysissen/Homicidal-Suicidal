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
        public Vector2 Velocity { get; set; }
        public bool Static { get; private set; }

        public void UpdatePhysics(GameTime gameTime, float deltaTime)
        {
            Velocity += new Vector2(0, 1) * Constants.gravity;
            Position += Velocity;
        }

        public PhysicsObject(Vector2 initialVelocity, bool isStatic, string name) : base(name)
        {
            Velocity = initialVelocity;
            Static = isStatic;
        }

        public PhysicsObject(Vector2 initialVelocity, bool isStatic, string name, out string trueName) : base(name, out trueName)
        {
            Velocity = initialVelocity;
            Static = isStatic;
        }

        public PhysicsObject(Vector2 initialVelocity, bool isStatic, string name, Rectangle rectangle) : base(name, rectangle)
        {
            Velocity = initialVelocity;
            Static = isStatic;
        }

        public PhysicsObject(Vector2 initialVelocity, bool isStatic, string name, out string trueName, Rectangle rectangle) : base(name, out trueName, rectangle)
        {
            Velocity = initialVelocity;
            Static = isStatic;
        }
    }
}

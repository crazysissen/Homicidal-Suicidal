using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HomicidalSuicidal
{
    public class Player : PhysicsObject, IRenderable
    {
        protected override object Component => this;

        #region Renderable Implementation

        // Make sure to inherit from either WorldObject or PhysicsObject and from the IRenderable interface.
        // Example: [ class MyClass : WorldObject, IRenderable ] 

        public override IRenderable Renderable => this;

        Rectangle IRenderable.Rect { get => Rect; }

        Texture2D IRenderable.Sprite { get => sprite; }
        Texture2D sprite;

        float IRenderable.Rotation { get => rotation; }
        float rotation;

        float IRenderable.Layer { get => layer; }
        float layer;

        Color IRenderable.SpriteColor { get => color; }
        Color color;

        // Make sure to set the sprite and color variables in the constructor.
        // Example: [ public MyClass(string name, Texture2D texture, Color spriteColor) : base(name) { sprite = texture; color = spriteColor; } ]
        // The ": base()" refers to the base class WorldObject that in and of itself takes at least one string in the constructor.

        #endregion

        public static Player MainPlayer { get; private set; }

        const float speed = 5,
                    jumpPower = 7;

        public float Health { get; set; }

        public Player(string name, Rectangle rectangle, Texture2D texture) : base(Vector2.Zero, 1, name, rectangle)
        {
            //if (player != null && player != this)
            MainPlayer = this;

            sprite = texture;
            Position = rectangle.Location.ToVector2();
            color = Color.White;
            Size = rectangle.Size;
            Health = 100;
            Kinematic = true;
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            // Die logic
            if (Health <= 0)
            {
                Console.WriteLine("Should Die");
            }

            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 velocity = (keyboardState.IsKeyDown(Keys.D)) ? new Vector2(speed, 0) : Vector2.Zero;
            velocity += (keyboardState.IsKeyDown(Keys.A)) ? new Vector2(-speed, 0) : Vector2.Zero;

            Position += velocity;

            if (Methods.KeyDown(Keys.Space))
            {
                Position -= new Vector2(0, 1);
                Velocity += new Vector2(0, -jumpPower);
            }

            // Console.WriteLine(Intersects(StaticObject.worldObjectThing));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HomicidalSuicidal
{
    class TestPlayer : PhysicsObject, IRenderable
    {
        public override IRenderable Renderable => this;

        Rectangle IRenderable.Rect { get => Rect; }

        Texture2D IRenderable.Sprite { get => sprite; }
        Texture2D sprite;

        float IRenderable.Rotation { get => 0; }

        float IRenderable.Layer { get => 1; }

        Color IRenderable.SpriteColor { get => color; }
        Color color;

        bool spaceDown;

        public TestPlayer(string name, Rectangle rectangle, Texture2D texture) : base(Vector2.Zero, 1, name, rectangle)
        {
            sprite = texture;
            Position = rectangle.Location.ToVector2();
            color = Color.Red;
            Size = rectangle.Size;
            Kinematic = true;
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 velocity = (keyboardState.IsKeyDown(Keys.W)) ? new Vector2(0, -2) : Vector2.Zero;
            velocity += (keyboardState.IsKeyDown(Keys.D)) ? new Vector2(2, 0) : Vector2.Zero;
            velocity += (keyboardState.IsKeyDown(Keys.S)) ? new Vector2(0, 2) : Vector2.Zero;
            velocity += (keyboardState.IsKeyDown(Keys.A)) ? new Vector2(-2, 0) : Vector2.Zero;

            Position += velocity;

            if (keyboardState.IsKeyDown(Keys.Space) && !spaceDown)
            {
                Position -= new Vector2(0, 1);
                Velocity += new Vector2(0, -5);
            }

            spaceDown = keyboardState.IsKeyDown(Keys.Space);

            Console.WriteLine(Intersects(TestObject.worldObjectThing));
        }
    }
}

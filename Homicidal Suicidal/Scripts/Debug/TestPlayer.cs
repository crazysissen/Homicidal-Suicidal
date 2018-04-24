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
        protected override object Component => this;

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
            
        }
    }
}

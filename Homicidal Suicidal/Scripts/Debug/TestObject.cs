using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    class TestObject : PhysicsObject, IRenderable
    {
        protected override object Component => this;

        public static WorldObject worldObjectThing;

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

        public TestObject(string name, Rectangle rectangle, Texture2D texture) : base(Vector2.Zero, 0, name, rectangle)
        {
            color = Color.Black;
            worldObjectThing = this;
            sprite = texture;
            Position = rectangle.Location.ToVector2();
            Size = rectangle.Size;
        }

    }
}

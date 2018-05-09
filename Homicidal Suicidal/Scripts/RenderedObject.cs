using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    class RenderedObject : WorldObject, IRenderable
    {
        protected override object Component => this;

        public static WorldObject worldObjectThing;

        public override IRenderable Renderable => this;

        WorldObject IRenderable.Object { get => this; }

        Texture2D IRenderable.Sprite { get => sprite; }
        Texture2D sprite;

        float IRenderable.Rotation { get => rotation; }
        float rotation;

        float IRenderable.Layer { get => layer; }
        float layer;

        Color IRenderable.SpriteColor { get => color; }
        Color color;

        public RenderedObject(string name, Rectangle rectangle, Texture2D texture, Color color, float layer = 0.1f) : base(name, rectangle)
        {
            sprite = texture;
            this.layer = layer;
            this.color = color;
        }
    }
}

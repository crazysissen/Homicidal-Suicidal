using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomicidalSuicidal
{
    class NurseAura : WorldObject, IRenderable
    {
        protected override object Component => this;

        #region Renderable Implementation

        // Make sure to inherit from either WorldObject or PhysicsObject and from the IRenderable interface.
        // Example: [ class MyClass : WorldObject, IRenderable ] 

        public override IRenderable Renderable => this;

        WorldObject IRenderable.Object { get => this; }

        Texture2D sprite;
        Texture2D IRenderable.Sprite { get => sprite; }

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

        float rotationPerSec;

        public NurseAura(Vector2 centerPos, Texture2D auraSprite, Rectangle auraRectangle, string auraName, int radius, float auraRotation, float auraLayer) : base(auraName, auraRectangle)
        {
            CenterPosition = centerPos;
            Size = new Point(2 * radius, 2 * radius);
            Name = auraName;
            rotationPerSec = auraRotation;
            sprite = auraSprite;
            layer = auraLayer;
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            rotation += rotationPerSec * deltaTime;
        }

        public void DestroyAura()
        {
            DestroyObject();
        }
    }
}

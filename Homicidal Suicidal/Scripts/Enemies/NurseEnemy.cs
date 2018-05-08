using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna;

namespace HomicidalSuicidal
{
    class NurseEnemy : WorldObject, IRenderable
    {
        protected override object Component => this;

        #region Renderable Implementation

        // Make sure to inherit from either WorldObject or PhysicsObject and from the IRenderable interface.
        // Example: [ class MyClass : WorldObject, IRenderable ] 

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

        // Make sure to set the sprite and color variables in the constructor.
        // Example: [ public MyClass(string name, Texture2D texture, Color spriteColor) : base(name) { sprite = texture; color = spriteColor; } ]
        // The ": base()" refers to the base class WorldObject that in and of itself takes at least one string in the constructor.

        #endregion

        float health, healing;

        public NurseEnemy(string nurseName, Texture2D nurseTexture, Color nurseColor, Point nurseSize, Vector2 nurseStartPos, float nurseHealth, float nurseHealing, float nurseLayer) : base(nurseName)
        {
            Name = nurseName;
            Size = nurseSize;
            layer = nurseLayer;
            sprite = nurseTexture;
            color = nurseColor;
            Position = nurseStartPos;
            healing = nurseHealing;
            health = nurseHealth;
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {

        }
    }
}

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
    class SurgeonEnemy : WorldObject, IRenderable
    {
        #region Renderable Implementation

        // Make sure to inherit from either WorldObject or PhysicsObject and from the IRenderable interface.
        // Example: [ class MyClass : WorldObject, IRenderable ] 

        public override IRenderable Renderable => this;

        Rectangle IRenderable.Rect { get => Rect; }

        Texture2D IRenderable.Sprite { get => _sprite; }
        Texture2D _sprite;

        Color IRenderable.SpriteColor { get => _color; }
        Color _color;

        // Make sure to set the sprite and color variables in the constructor.
        // Example: [ public MyClass(string name, Texture2D texture, Color spriteColor) : base(name) { sprite = texture; color = spriteColor; } ]
        // The ": base()" refers to the base class WorldObject that in and of itself takes at least one string in the constructor.

        #endregion

        Vector2 startPos;
        float health, healing;

        public SurgeonEnemy(string surgeonName, Texture2D surgeonTexture, Color surgeonColor, Rectangle surgeonRectangle, Vector2 surgeonStartPos, float surgeonHealth, float surgeonHealing) : base(surgeonName, surgeonRectangle)
        {
            Name = surgeonName;
            _sprite = surgeonTexture;
            _color = surgeonColor;
            startPos = surgeonStartPos;
            healing = surgeonHealing;
            health = surgeonHealth;
        }
    }
}

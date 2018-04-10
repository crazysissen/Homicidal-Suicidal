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

        public NurseEnemy(string doctorName, Texture2D doctorTexture, Color doctorColor, Rectangle doctorRectangle, Vector2 doctorStartPos, float doctorHealth, float doctorHealing) : base(doctorName, doctorRectangle)
        {
            Name = doctorName;
            _sprite = doctorTexture;
            _color = doctorColor;
            startPos = doctorStartPos;
            healing = doctorHealing;
            health = doctorHealth;
        }
    }
}

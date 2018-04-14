using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    interface IRenderable
    {
        Rectangle Rect { get; }

        Texture2D Sprite { get; }

        Color SpriteColor { get; }

        float Rotation { get; }

        float Layer { get; }
    }
}

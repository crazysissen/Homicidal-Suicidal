using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    public interface IRenderable
    {
        WorldObject Object { get; }

        Texture2D Sprite { get; }

        Color SpriteColor { get; }

        float Rotation { get; }

        float Layer { get; }
    }
}

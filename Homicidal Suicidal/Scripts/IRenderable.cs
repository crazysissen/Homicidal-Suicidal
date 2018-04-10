using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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

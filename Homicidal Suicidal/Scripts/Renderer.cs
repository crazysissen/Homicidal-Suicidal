using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    static class Renderer
    {
        public static List<IRenderable> Renderables
        {
            get
            {
                List<IRenderable> returnList = new List<IRenderable>();

                foreach (KeyValuePair<string, WorldObject> pair in WorldObject.WorldObjects)
                {
                    if (pair.Value.Renderable != null)
                    {
                        returnList.Add(pair.Value.Renderable);
                    }
                }

                return returnList;
            }
        }

        public static void RenderAll(SpriteBatch spriteBatch, Point camera)
        {
            foreach (IRenderable renderable in Renderables)
            {
                Render(renderable, Game1.MainSpriteBatch, camera);
            }
        }

        public static void RenderAll(SpriteBatch spriteBatch)
            => RenderAll(spriteBatch, Point.Zero);

        public static void Render(IRenderable renderable, SpriteBatch spriteBatch, Point camera)
            => spriteBatch.Draw(renderable.Sprite, new Rectangle(renderable.Rect.Location, renderable.Rect.Size), null, renderable.SpriteColor, renderable.Rotation, camera, SpriteEffects.None, );

        public static void RenderThis(this IRenderable renderable, SpriteBatch spriteBatch, Point camera)
            => Render(renderable, spriteBatch, camera);
    }
}

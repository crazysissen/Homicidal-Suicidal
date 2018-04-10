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

        public static void Render(IRenderable renderable) 
            => Render(renderable, Game1.MainSpriteBatch);

        public static void Render(IRenderable renderable, SpriteBatch spriteBatch) 
            => spriteBatch.Draw(renderable.Sprite, renderable.Rect, renderable.SpriteColor);

        public static void Render(IRenderable renderable, Point camera)
            => Render(renderable, Game1.MainSpriteBatch, camera);

        public static void Render(IRenderable renderable, SpriteBatch spriteBatch, Point camera)
            => spriteBatch.Draw(renderable.Sprite, new Rectangle(renderable.Rect.Location - camera, renderable.Rect.Size), renderable.SpriteColor, );

        public static void RenderThis(this IRenderable renderable) 
            => Render(renderable, Game1.MainSpriteBatch);

        public static void RenderThis(this IRenderable renderable, SpriteBatch spriteBatch) 
            => Render(renderable, spriteBatch);

        public static void RenderThis(this IRenderable renderable, Point camera)
            => Render(renderable, Game1.MainSpriteBatch, camera);

        public static void RenderThis(this IRenderable renderable, SpriteBatch spriteBatch, Point camera)
            => Render(renderable, spriteBatch, camera);
    }
}

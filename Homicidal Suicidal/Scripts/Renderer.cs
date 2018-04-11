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
            List<IRenderable> renderables = Renderables;
            foreach (IRenderable renderable in renderables)
            {
                Console.WriteLine(renderable.Rect + "" + renderable.Sprite);
                Render(renderable, Game1.MainSpriteBatch, camera);
            }
        }

        public static void RenderAll(SpriteBatch spriteBatch)
            => RenderAll(spriteBatch, Point.Zero);

        public static void Render(IRenderable renderable, SpriteBatch spriteBatch, Point camera)
            => spriteBatch.Draw(renderable.Sprite, renderable.Rect.Location.ToVector2(), null, renderable.SpriteColor, renderable.Rotation, Vector2.Zero, renderable.Rect.Size.ToVector2() / new Vector2(renderable.Sprite.Width, renderable.Sprite.Height), SpriteEffects.None, 0);

        public static void RenderThis(this IRenderable renderable, SpriteBatch spriteBatch, Point camera)
            => Render(renderable, spriteBatch, camera);
    }
}

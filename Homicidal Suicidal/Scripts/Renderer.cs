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
        public static Vector2 CameraScreenPosition => camera + offset - new Vector2(Game1.Graphics.PreferredBackBufferWidth * 0.5f, Game1.Graphics.PreferredBackBufferWidth * 0.5f);

        public static Vector2 camera;
        public static Vector2 offset = new Vector2(500, 0);

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

        public static void RenderAll(SpriteBatch spriteBatch)
        {
            List<IRenderable> renderables = Renderables;
            foreach (IRenderable renderable in renderables)
            {
                Render(renderable, Game1.MainSpriteBatch, CameraScreenPosition);
            }
        }

        //public static void RenderAll(SpriteBatch spriteBatch)
        //    => RenderAll(spriteBatch, Point.Zero);

        public static void Render(IRenderable renderable, SpriteBatch spriteBatch, Vector2 camera)
            => spriteBatch.Draw(renderable.Sprite, renderable.Object.Position /* + renderable.Object.Offset*/ - camera, null, renderable.SpriteColor, renderable.Rotation, Vector2.Zero /*renderable.Object.Offset*/, renderable.Object.Size.ToVector2() / new Vector2(renderable.Sprite.Width, renderable.Sprite.Height), SpriteEffects.None, renderable.Layer * 0.00001f);

        public static void RenderThis(this IRenderable renderable, SpriteBatch spriteBatch, Vector2 camera)
            => Render(renderable, spriteBatch, camera);
    }
}

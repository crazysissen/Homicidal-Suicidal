using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HomicidalSuicidal
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch;

        public static SpriteBatch MainSpriteBatch { get => spriteBatch; }

        static Dictionary<string, Texture2D> allSprites;
        string[] _loadTags = new string[] { "Square" };

        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            player = new Player("Hellothere", allSprites["Square"], Color.White, new Rectangle(0, 0, 20, 20));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            allSprites = new Dictionary<string, Texture2D>();

            foreach (string name in _loadTags)
                if (Content.Load<Texture2D>(name) != null)
                    allSprites.Add(name, Content.Load<Texture2D>(name));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // In order: All movement, all updates, all collision

            foreach (KeyValuePair<string, WorldObject> pair in WorldObject.WorldObjects)
            {
                pair.Value.PhysObject.UpdateMovement(gameTime, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            WorldObject.UpdateAll(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            Renderer.RenderAll(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

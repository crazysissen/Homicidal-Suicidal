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
        string[] loadTags = new string[] { "Square" };

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

            foreach (string name in loadTags)
                if (Content.Load<Texture2D>(name) != null)
                    allSprites.Add(name, Content.Load<Texture2D>(name));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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

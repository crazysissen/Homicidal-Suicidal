using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace HomicidalSuicidal
{
    public class Game1 : Game
    {
        // These variables will never be more than one, static is appropriate
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Player player;
        public static SpriteBatch MainSpriteBatch { get => spriteBatch; }
        public static Dictionary<string, Texture2D> allSprites;

        string[] _loadTags = new string[] 
        {
            "Square", "Floor",
            "Doctor_Attack", "Doctor_Dead", "Doctor_Dying", "Doctor_Idle",
            "Healing_Aura", "Nurse_Dying", "Nurse_Dead", "Nurse_Healing"
        };
        Song inGameSong;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1919;
            graphics.PreferredBackBufferHeight = 1079;

            
        }

        protected override void Initialize()
        {
            base.Initialize();

            WorldObject.InitializeClass();

            //player = new Player("Hellothere", allSprites["Square"], Color.White, new Rectangle(0, 0, 20, 20));
            new StaticObject("Test", new Rectangle(0, 400, 1500, 100), allSprites["Square"]);
            new StaticObject("Test", new Rectangle(0, 400, 1500, 100), allSprites["Square"]);
            new Player("Test", new Rectangle(0, 0, 40, 40), allSprites["Square"]);
            new DoctorEnemy("Doctor1", allSprites["Floor"], Color.White, new Point(50, 50), new Vector2(300, 0), 100, 50, 9999, 9999);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(inGameSong);
        }

        protected override void LoadContent()
        {
            inGameSong = Content.Load<Song>("Suicidal Dash");

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

            WorldObject.UpdateAllPhysics(gameTime);
            WorldObject.UpdateAllDerived(gameTime);
            WorldObject.UpdateAllCollision();

            Renderer.camera = new Vector2(Player.MainPlayer.CenterPosition.X, 500);

            Miscellanious.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Renderer.RenderAll(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

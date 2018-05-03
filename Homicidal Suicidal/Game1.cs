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
        public static SpriteBatch MainSpriteBatch => spriteBatch;
        public static Dictionary<string, Texture2D> AllSprites { get; set; }
        public static Player Player { get; set; }
        public static GraphicsDeviceManager Graphics { get; set; }

        public enum GameState { MainMenu, InGame, Pause, Win, Lose }
        public Stack<GameState> CurrentState { get; private set; }

        GUI gui = new GUI();
        SpriteFont menuFont, defaultFont;

        static SpriteBatch spriteBatch;
        Song inGameSong;

        string[] _loadTags = new string[] 
        {
            "Square", "Floor",
            "Doctor_Attack", "Doctor_Dead", "Doctor_Dying", "Doctor_Idle",
            "Healing_Aura", "Nurse_Dying", "Nurse_Dead", "Nurse_Healing"
        };

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Graphics.PreferredBackBufferWidth = 1919;
            Graphics.PreferredBackBufferHeight = 1079;
        }

        protected override void Initialize()
        {
            base.Initialize();

            WorldObject.InitializeClass();

            //player = new Player("Hellothere", allSprites["Square"], Color.White, new Rectangle(0, 0, 20, 20));
            new StaticObject("Test", new Rectangle(0, 400, 1500, 100), AllSprites["Square"]);
            new StaticObject("Test", new Rectangle(0, 400, 1500, 100), AllSprites["Square"]);
            new Player("Test", new Rectangle(0, 0, 40, 40), AllSprites["Square"]);
            new DoctorEnemy("Doctor1", true, AllSprites["Doctor_Attack"], Color.White, new Point(240, 145), new Vector2(300, 0), 3, 100, 50, 9999, 20);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(inGameSong);

            CurrentState = new Stack<GameState>();
            CurrentState.Push(GameState.MainMenu);
        }

        protected override void LoadContent()
        {
            inGameSong = Content.Load<Song>("Suicidal Dash");

            defaultFont = Content.Load<SpriteFont>("Fonts/DefaultFont");
            menuFont = Content.Load<SpriteFont>("Fonts/MenuFont");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            AllSprites = new Dictionary<string, Texture2D>();

            foreach (string name in _loadTags)
                if (Content.Load<Texture2D>(name) != null)
                    AllSprites.Add(name, Content.Load<Texture2D>(name));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            gui.New();

            // Declaring bodies here is unnecessary, it makes things easier to see
            switch (CurrentState.Peek())
            {
                case GameState.MainMenu:
                    {
                        MainMenuUpdate();
                        break;
                    }

                case GameState.InGame:
                    {
                        InGameUpdate(gameTime);
                        break;
                    }

                case GameState.Pause:
                    {
                        PauseUpdate();
                        break;
                    }

                case GameState.Win:
                    {
                        break;
                    }
            }

            base.Update(gameTime);

            Methods.UpdateMethods();
        }

        void MainMenuUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Point screenSize = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            gui.Add(
                new GUI.Label("Homicidal Suicidal", 80, new Vector2(100, 100), menuFont, Color.White),
                new GUI.Label("Start", 80, new Vector2(100, 480), defaultFont, Color.LightGray),
                new GUI.Label("Quit", 80, new Vector2(100, 600), defaultFont, Color.LightGray),
                new GUI.Button(new Rectangle(100, 480, 500, 80), StartButton),
                new GUI.Button(new Rectangle(100, 600, 500, 80), ExitButton)
                );
        }

        void PauseUpdate()
        {
            Point screenSize = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            gui.Add(
                new GUI.Texture(new Rectangle(-1, -1, screenSize.X + 2, screenSize.Y + 2), new Color(0, 0, 0, 0.7f)),
                new GUI.Label("Paused", 80, new Vector2(100, 100), menuFont, Color.White),
                new GUI.Label("Resume", 80, new Vector2(100, 480), defaultFont, Color.LightGray),
                new GUI.Label("Quit", 80, new Vector2(100, 600), defaultFont, Color.LightGray),
                new GUI.Button(new Rectangle(100, 480, 500, 80), ResumeButton),
                new GUI.Button(new Rectangle(100, 600, 500, 80), ExitButton)
                );
        }

        void ResumeButton()
        {
            CurrentState.Pop();
        }

        void StartButton()
        {
            CurrentState.Push(GameState.InGame);
        }

        void ExitButton()
        {
            Exit();
        }

        void InGameUpdate(GameTime gameTime)
        {
            // In order: All movement, all updates, all collision
            if (Methods.KeyDown(Keys.Escape))
                CurrentState.Push(GameState.Pause);

            WorldObject.UpdateAllPhysics(gameTime);
            WorldObject.UpdateAllDerived(gameTime);
            WorldObject.UpdateAllCollision();

            Renderer.camera = new Vector2(Player.MainPlayer.CenterPosition.X, 500);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (CurrentState.Peek() == GameState.InGame || CurrentState.Peek() == GameState.Pause)
                Renderer.RenderAll(spriteBatch);

            gui.Draw(spriteBatch);
            //spriteBatch.DrawString(defaultFont, "Hello", new Vector2(100, 100), Color.White, 0, Vector2.One, 12, SpriteEffects.None);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Vector2 NormalizeThis(Vector2 vector)
        {
            Vector2 newVector = vector;
            newVector.Normalize();
            return newVector;
        }
    }
}

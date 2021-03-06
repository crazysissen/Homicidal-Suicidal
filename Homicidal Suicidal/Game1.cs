﻿using System;
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
        public static Stack<GameState> CurrentState { get; private set; }

        public enum GameState { MainMenu, InGame, Pause, Win, Lose, Introduction }

        GUI gui = new GUI();
        SpriteFont menuFont, defaultFont;

        static SpriteBatch spriteBatch;
        Song inGameSong;

        string[] _loadTags = new string[] 
        {
            "Square", "Floor", "Syringe", "Button", "Wall", "Scalpel", "Platform", "Bullet",
            "Ambulance_1", "Ambulance_2",
            "Doctor_Attack", "Doctor_Dead", "Doctor_Dying", "Doctor_Idle",
            "Healing_Aura", "Nurse_Dying", "Nurse_Dead", "Nurse_Healing",
            "Surgeon_Attack", "Surgeon_Dead", "Surgeon_Dying", "Surgeon_Idle"
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
            WorldObject.InitializeClass();
            new Player("Test", new Rectangle(1000, -540, 200, 200));

            base.Initialize();

            World.Initialize((new Random()).Next(0, 10000));

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(inGameSong);

            CurrentState = new Stack<GameState>();
            CurrentState.Push(GameState.MainMenu);
            Ambulance ambulance = new Ambulance(new Rectangle(0, -470, 489, 600)); 
        }

        protected override void LoadContent()
        {
            inGameSong = Content.Load<Song>("Suicidal Dash");

            defaultFont = Content.Load<SpriteFont>("Fonts/DefaultFont");
            menuFont = Content.Load<SpriteFont>("Fonts/MenuFont");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            AllSprites = new Dictionary<string, Texture2D>();

            Player.MainPlayer.ImportTextures(Content);

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

            Animator.UpdateAll((float)gameTime.ElapsedGameTime.TotalSeconds);

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
                        WinUpdate();
                        break;
                    }

                case GameState.Lose:
                    {
                        LoseUpdate();
                        break;
                    }

                case GameState.Introduction:
                    {
                        IntroductionUpdate();
                        break;
                    }
            }

            base.Update(gameTime);

            Methods.UpdateMethods();
        }

        void IntroductionUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                CurrentState.Push(GameState.InGame);

            Point screenSize = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            gui.Add(
                new GUI.Texture(new Rectangle(-1, -1, screenSize.X + 2, screenSize.Y + 2), new Color(0, 0, 0, 0.7f)),
                new GUI.Label("I want to die.", 100, new Vector2(100, 100), menuFont, Color.White),
                new GUI.Label("They won't let me die.", 100, new Vector2(100, 300), menuFont, Color.White),
                new GUI.Label("They must die.", 100, new Vector2(100, 500), menuFont, Color.White),
                new GUI.Label("Press Space to continue", 60, new Vector2(900, 990), defaultFont, Color.White)
                );
        }

        void LoseUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                CurrentState.Push(GameState.InGame);

            Point screenSize = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            gui.Add(
                new GUI.Texture(new Rectangle(-1, -1, screenSize.X + 2, screenSize.Y + 2), new Color(0, 0, 0, 0.7f)),
                new GUI.Label("The latest health control claims that", 60, new Vector2(100, 100), menuFont, Color.White),
                new GUI.Label("you are healthy and ready to go home.", 60, new Vector2(100, 200), menuFont, Color.White),
                new GUI.Label("You've failed your mission...", 60, new Vector2(100, 350), menuFont, Color.White),
                new GUI.Button(new Rectangle(100, 660, 340, 120), AllSprites["Button"], Color.White, Color.LightGray, Color.Gray, PlayAgainButton),
                new GUI.Button(new Rectangle(100, 830, 340, 120), AllSprites["Button"], Color.White, Color.LightGray, Color.Gray, ExitButton),
                new GUI.Label("Retry", 70, new Vector2(158, 672), defaultFont, Color.Black),
                new GUI.Label("Quit", 80, new Vector2(158, 840), defaultFont, Color.Black)
                );
        }

        void WinUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                CurrentState.Push(GameState.InGame);

            Point screenSize = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            gui.Add(
                new GUI.Texture(new Rectangle(-1, -1, screenSize.X + 2, screenSize.Y + 2), new Color(0, 0, 0, 0.7f)),
                new GUI.Label("You Died! Congrats!", 140, new Vector2(100, 100), menuFont, Color.White),
                new GUI.Button(new Rectangle(100, 660, 340, 120), AllSprites["Button"], Color.White, Color.LightGray, Color.Gray, PlayAgainButton),
                new GUI.Button(new Rectangle(100, 830, 340, 120), AllSprites["Button"], Color.White, Color.LightGray, Color.Gray, ExitButton),
                new GUI.Label("Play Again", 46, new Vector2(120, 687), defaultFont, Color.Black),
                new GUI.Label("Quit", 80, new Vector2(158, 840), defaultFont, Color.Black)
                );
        }

        void MainMenuUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                CurrentState.Push(GameState.Introduction);

            Point screenSize = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            gui.Add(
                new GUI.Label("Homicidal Suicidal", 140, new Vector2(100, 100), menuFont, Color.White),
                new GUI.Button(new Rectangle(100, 660, 340, 120), AllSprites["Button"], Color.White, Color.LightGray, Color.Gray, StartButton),
                new GUI.Button(new Rectangle(100, 830, 340, 120), AllSprites["Button"], Color.White, Color.LightGray, Color.Gray, ExitButton),
                new GUI.Label("Start", 80, new Vector2(140, 670), defaultFont, Color.Black),
                new GUI.Label("Quit", 80, new Vector2(140, 840), defaultFont, Color.Black)
                );
        }

        void PauseUpdate()
        {
            if (Methods.KeyDown(Keys.Escape))
                CurrentState.Pop();

            Point screenSize = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            gui.Add(
                new GUI.Texture(new Rectangle(-1, -1, screenSize.X + 2, screenSize.Y + 2), new Color(0, 0, 0, 0.7f)),
                new GUI.Label("Paused", 80, new Vector2(100, 100), menuFont, Color.White),
                new GUI.Label("Resume", 80, new Vector2(100, 470), defaultFont, Color.LightGray),
                new GUI.Label("Quit", 80, new Vector2(100, 590), defaultFont, Color.LightGray),
                new GUI.Button(new Rectangle(100, 480, 500, 80), ResumeButton),
                new GUI.Button(new Rectangle(100, 600, 500, 80), ExitButton)
                );
        }

        void PlayAgainButton()
        {
            WorldObject.DestroyAllObjects();
            Initialize();
            CurrentState.Push(GameState.InGame);
        }

        void ResumeButton() => CurrentState.Pop();

        void StartButton() => CurrentState.Push(GameState.Introduction);

        void ExitButton() => Exit();

        void InGameUpdate(GameTime gameTime)
        {
            World.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            // In order: All movement, all updates, all collision
            if (Methods.KeyDown(Keys.Escape))
                CurrentState.Push(GameState.Pause);

            WorldObject.UpdateAllPhysics(gameTime);
            WorldObject.UpdateAllCollision();
            WorldObject.UpdateAllDerived(gameTime);

            gui.Add(
                new GUI.Texture(new Rectangle(90, 970, 220, 70), Color.SlateGray),
                new GUI.Texture(new Rectangle(100, 980, (int)(Player.MainPlayer.Health / Player.maxHealth * 200), 50), Color.Lime)
                );

            Renderer.camera = new Vector2(Player.MainPlayer.CenterPosition.X, 0);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (CurrentState.Peek() == GameState.InGame || CurrentState.Peek() == GameState.Pause)
                Renderer.RenderAll(spriteBatch);

            gui.Draw(spriteBatch);

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

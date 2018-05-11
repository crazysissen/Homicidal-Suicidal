using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HomicidalSuicidal
{
    public class Player : PhysicsObject, IRenderable
    {
        protected override object Component => this;

        #region Renderable Implementation

        // Make sure to inherit from either WorldObject or PhysicsObject and from the IRenderable interface.
        // Example: [ class MyClass : WorldObject, IRenderable ] 

        public override IRenderable Renderable => this;

        WorldObject IRenderable.Object { get => this; }

        Texture2D IRenderable.Sprite { get => animator.GetTexture(); }
        Animator animator; 

        float IRenderable.Rotation { get => rotation; }
        float rotation;

        float IRenderable.Layer { get => layer; }
        float layer;

        Color IRenderable.SpriteColor { get => color; }
        Color color;

        // Make sure to set the sprite and color variables in the constructor.
        // Example: [ public MyClass(string name, Texture2D texture, Color spriteColor) : base(name) { sprite = texture; color = spriteColor; } ]
        // The ": base()" refers to the base class WorldObject that in and of itself takes at least one string in the constructor.

        #endregion

        public static Player MainPlayer { get; private set; }

        public const float 
            maxHealth = 100,
            speed = 5,
            jumpPower = 7,
            attackSpeed = 0.3f,
            bulletSpeed = 20,
            deathRate = 0.02f,
            animationStateTime = 0.08f,
            shootAnimationDuration = 0.1f;

        bool LeftMousePressed => Mouse.GetState().LeftButton == ButtonState.Pressed;

        Vector2 mousePos;

        float attackTimer, shootAnimationCountdown;

        public float Health { get; set; }

        bool grounded;

        bool airJumped;

        string[]
            runningTexturesImport = new string[] { "Player_Run_1&7", "Player_Run_2", "Player_Run_3", "Player_Run_4", "Player_Run_5", "Player_Run_6", "Player_Run_1&7", "Player_Run_8", "Player_Run_9", "Player_Run_10", "Player_Run_11", "Player_Run_12" },
            shootingTexturesImport = new string[] { "Player_Run_1&7_Shoot", "Player_Run_2_Shoot", "Player_Run_3_Shoot", "Player_Run_4_Shoot", "Player_Run_5_Shoot", "Player_Run_6_Shoot", "Player_Run_1&7_Shoot", "Player_Run_8_Shoot", "Player_Run_9_Shoot", "Player_Run_10_Shoot", "Player_Run_11_Shoot", "Player_Run_12_Shoot" },
            jumpingTexturesImport = new string[] { "Player_Jump_1", "Player_Jump_2" },
            jumpingShootingTexturesImport = new string[] { "Player_Jump_1_Shoot", "Player_Jump_2_Shoot" },
            dyingTexturesImport = new string[] { "Player_Dying_1", "Player_Dying_2", "Player_Dying_3" },
            livingTexturesImport = new string[] { "Player_Lives_1", "Player_Lives_2", "Player_Lives_3" };

        Texture2D[] rTextures, rsTextures, jTextures, jsTextures, dTextures, lTextures;
        Texture2D idle, shoot;

        public Player(string name, Rectangle rectangle) : base(Vector2.Zero, 1, name, rectangle)
        {
            //if (player != null && player != this)
            MainPlayer = this;
            attackTimer = attackSpeed;

            Position = rectangle.Location.ToVector2();
            color = Color.White;
            Size = rectangle.Size;
            Health = maxHealth * 0.5f;
            Kinematic = true;

            Tags.Add("Player");

            hitboxUpInset = 0.1f;
            hitboxRightInset = 0.25f;
            hitboxLeftInset = 0.25f;
        }

        public void ImportTextures(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            rTextures = ImportList(runningTexturesImport, content);
            rsTextures = ImportList(shootingTexturesImport, content);
            jTextures = ImportList(jumpingTexturesImport, content);
            jsTextures = ImportList(jumpingShootingTexturesImport, content);
            dTextures = ImportList(dyingTexturesImport, content);
            lTextures = ImportList(livingTexturesImport, content);

            idle = content.Load<Texture2D>("Player_Idle");
            shoot = content.Load<Texture2D>("Player_Shoot");

            animator = new Animator(
                new Animation(1, -1, false, new Animation.AnimationState(idle, 0)), // Idle
                new Animation(0.1f, 0, false, new Animation.AnimationState(shoot, 0)), // Shoot idle
                new Animation(animationStateTime * rTextures.Length, CreateAnimationStateList(rTextures, animationStateTime), - 1, false), // Run
                new Animation(animationStateTime * rsTextures.Length, CreateAnimationStateList(rsTextures, animationStateTime), -1, false), // Run shoot
                new Animation(100, CreateAnimationStateList(jTextures, 0.3f)), // Jump
                new Animation(100, CreateAnimationStateList(jsTextures, 0.3f)), // Jump shoot
                new Animation(100, CreateAnimationStateList(dTextures, 0.1f)), // Dying
                new Animation(100, CreateAnimationStateList(lTextures, 0.1f)) // Living
                );
        }

        Texture2D[] ImportList(string[] importStrings, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Texture2D[] returnArray = new Texture2D[importStrings.Length];
            for (int i = 0; i < returnArray.Length; ++i)
            {
                returnArray[i] = content.Load<Texture2D>(importStrings[i]);
            }
            return returnArray;
        }

        Animation.AnimationState[] CreateAnimationStateList(Texture2D[] textures, float stepTime)
        {
            Animation.AnimationState[] returnList = new Animation.AnimationState[textures.Length];
            for (int i = 0; i < textures.Length; ++i)
            {
                returnList[i] = new Animation.AnimationState(textures[i], stepTime * i);
            }
            return returnList;
        }

        public void Heal(float amount)
        {
            Health += amount;
            if (Health > maxHealth)
                Health = maxHealth;
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            // Live logic
            if (Health >= Player.maxHealth)
            {
                Game1.CurrentState.Push(Game1.GameState.Lose);
            }
            else if (Health <= 0)
            {
                Game1.CurrentState.Push(Game1.GameState.Win);
            }

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            mousePos = new Vector2(mouseState.X, mouseState.Y) + Renderer.CameraScreenPosition;

            Attack(mousePos, deltaTime);
            Dying(deathRate, deltaTime);

            Velocity = (keyboardState.IsKeyDown(Keys.D)) ? new Vector2(speed, Velocity.Y) : new Vector2(0, Velocity.Y);
            Velocity += (keyboardState.IsKeyDown(Keys.A)) ? new Vector2(-speed, 0) : Vector2.Zero;

            if (Methods.KeyDown(Keys.Space) && airJumped == false)
            {
                Velocity = new Vector2(Velocity.X, -jumpPower);
                airJumped = true;
            }

            Animate(deltaTime);

            grounded = false;
        }

        void Animate(float deltaTime)
        {
            if (shootAnimationCountdown > 0)
                shootAnimationCountdown -= deltaTime;

            if (Velocity.X == 0 && grounded)
            {
                if (animator.CurrentState != 0 && shootAnimationCountdown <= 0)
                    animator.SetState(0);

                if (animator.CurrentState != 1 && shootAnimationCountdown > 0)
                    animator.SetState(1);

                return;
            }

            if (Velocity.X > 0 && grounded)
            {
                if (animator.CurrentState != 2 && shootAnimationCountdown <= 0)
                    animator.SetState(2, animator.CurrentState == 3 ? animator.CurrentTime : 0);

                if (animator.CurrentTime != 3 && shootAnimationCountdown > 0)
                    animator.SetState(3, animator.CurrentState == 2 ? animator.CurrentTime : 0);

                return;
            }

            if ((animator.CurrentState != 4 && animator.CurrentState != 5) || (animator.CurrentState == 5 && shootAnimationCountdown <= 0))
                animator.SetState(4);

            if (animator.CurrentState == 4 && shootAnimationCountdown > 0)
                animator.SetState(5);
        }

        public override void OnCollision(PhysicsObject physicsObject)
        {
            Console.WriteLine("Touch Ground");

            if (physicsObject.Tags.Contains("Ground"))
            {
                airJumped = false;
                grounded = true;
            }
        }

        void Attack(Vector2 mousePos, float deltaTime)
        {
            attackTimer -= deltaTime;

            if (LeftMousePressed && attackTimer <= 0)
            {
                Bullet bullet = new Bullet("Player Bullet", "Player Bullet", Bullet.Owner.Player, Position + new Vector2(130, 160), Game1.NormalizeThis(mousePos - CenterPosition) * bulletSpeed, Game1.AllSprites["Bullet"], Color.White, new Point(12, 38), 15, (float)Math.Atan2((mousePos - CenterPosition).Y, (mousePos - CenterPosition).X) + (float)Math.PI / 2, 0, "Enemy");
                bullet.Tags.Add("Bullet");

                Console.WriteLine("Bullet fired");
                attackTimer = attackSpeed;

                shootAnimationCountdown = shootAnimationDuration;
            }
        }

        void Dying(float maxHealthMultiplier, float deltaTime)
        {
            if (Health > 0)
                Health -= maxHealthMultiplier * maxHealth * deltaTime;
        }
    }
}

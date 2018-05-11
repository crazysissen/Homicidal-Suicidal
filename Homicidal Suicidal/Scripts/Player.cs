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

        Texture2D IRenderable.Sprite { get => sprite; }
        Texture2D sprite;

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

        //Vector2 apparentOffset = new Vector2(, );
        //public Vector2 ApparentCenter => (Position + apparentOffset);

        public const float maxHealth = 100,
                           speed = 5,
                           jumpPower = 7,
                           attackSpeed = 0.3f,
                           bulletSpeed = 20,
                           deathRate = 0.02f;

        bool LeftMousePressed => Mouse.GetState().LeftButton == ButtonState.Pressed;

        Vector2 mousePos;

        float attackTimer;

        public float Health { get; set; }

        bool airJumped;

        public Player(string name, Rectangle rectangle, Texture2D texture) : base(Vector2.Zero, 1, name, rectangle)
        {
            //if (player != null && player != this)
            MainPlayer = this;
            attackTimer = attackSpeed;

            sprite = texture;
            Position = rectangle.Location.ToVector2();
            color = Color.White;
            Size = rectangle.Size;
            Health = maxHealth * 0.5f;
            Kinematic = true;

            Tags.Add("Player");
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

            Vector2 velocity = (keyboardState.IsKeyDown(Keys.D)) ? new Vector2(speed, 0) : Vector2.Zero;
            velocity += (keyboardState.IsKeyDown(Keys.A)) ? new Vector2(-speed, 0) : Vector2.Zero;

            Position += velocity;

            if (Methods.KeyDown(Keys.Space) && airJumped == false)
            {
                Velocity = new Vector2(Velocity.X, -jumpPower);
                airJumped = true;
            }
        }

        public override void OnCollision(PhysicsObject physicsObject)
        {
            Console.WriteLine("Touch Ground");

            if (physicsObject.Tags.Contains("Ground"))
            {
                airJumped = false;
            }
        }

        void Attack(Vector2 mousePos, float deltaTime)
        {
            attackTimer -= deltaTime;

            if (LeftMousePressed && attackTimer <= 0)
            {
                Bullet bullet = new Bullet("Player Bullet", "Player Bullet", Bullet.Owner.Player, CenterPosition, Game1.NormalizeThis(mousePos - CenterPosition) * bulletSpeed, Game1.AllSprites["Square"], Color.White, new Point(10, 10), 15, 0, 0, "Enemy");
                bullet.Tags.Add("Bullet");

                Console.WriteLine("Bullet fired");
                attackTimer = attackSpeed;
            }
        }

        void Dying(float maxHealthMultiplier, float deltaTime)
        {
            if (Health > 0)
                Health -= maxHealthMultiplier * maxHealth * deltaTime;
        }
    }
}

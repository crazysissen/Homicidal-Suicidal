using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna;

namespace HomicidalSuicidal
{
    public enum States { Idle, Attack, Dying }

    class DoctorEnemy : WorldObject, IRenderable
    {
        protected override object Component => this;

        object ThisScript => this;

        #region Renderable Implementation

        // Make sure to inherit from either WorldObject or PhysicsObject and from the IRenderable interface.
        // Example: [ class MyClass : WorldObject, IRenderable ] 

        public override IRenderable Renderable => this;

        Rectangle IRenderable.Rect { get => Rect; }

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

        protected States states;

        public bool hostile;

        public float syringeSpeed;

        public float health, healing, attackSpeed = 1, attackTimer, attackRange;
        public float DistanceToPlayer => (Player.MainPlayer.CenterPosition - CenterPosition).Length();

        public Vector2 DirectionToPlayer => Game1.NormalizeThis(Player.MainPlayer.CenterPosition - CenterPosition); 

        public DoctorEnemy(string doctorName, bool doctorHostile, Texture2D doctorTexture, Color doctorColor, Point doctorSize, Vector2 doctorStartPos, float doctorSyringeSpeed, float doctorHealth, float doctorHealing, float doctorRange, float doctorLayer) : base(doctorName)
        {
            Name = doctorName;
            Size = doctorSize;
            layer = doctorLayer;
            sprite = doctorTexture;
            color = doctorColor;
            Position = doctorStartPos;
            healing = doctorHealing;
            attackRange = doctorRange;
            health = doctorHealth;
            hostile = doctorHostile;
            syringeSpeed = doctorSyringeSpeed;
            
            attackTimer = attackSpeed;

            Tags.Add("Enemy");
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            StateCheck();

            if (attackTimer > 0)
                attackTimer -= deltaTime;

            if (states == States.Attack && attackTimer <= 0)
            {
                ThrowNeedle();
                attackTimer = attackSpeed;
            }
        }

        void StateCheck()
        {
            if (health <= 0)
            {
                states = States.Dying;
            }
            else if (DistanceToPlayer <= attackRange && hostile)
            {
                states = States.Attack;
            }
            else if (DistanceToPlayer > attackRange && health > 0)
            {
                states = States.Idle;
            }
        }

        void ThrowNeedle()
        {
            Console.WriteLine("Bulletspawn");
            // Temp bullet creation
            Bullet bullet = new Bullet("Syringe", "Syringe", Bullet.Owner.Enemy, CenterPosition, DirectionToPlayer * syringeSpeed, Game1.allSprites["Square"], Color.White, new Point(10, 10), healing, 0, 9999, "Player");
        }
    }
}
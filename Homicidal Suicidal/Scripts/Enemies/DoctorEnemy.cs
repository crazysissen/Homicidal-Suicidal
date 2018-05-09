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
    class DoctorEnemy : PhysicsObject, IRenderable
    {
        protected override object Component => this;

        #region Renderable Implementation

        // Make sure to inherit from either WorldObject or PhysicsObject and from the IRenderable interface.
        // Example: [ class MyClass : WorldObject, IRenderable ] 

        public override IRenderable Renderable => this;

        WorldObject IRenderable.Object { get => this; }

        Texture2D IRenderable.Sprite { get => animator.GetTexture(); }
        //Texture2D sprite;

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

        Animator animator;

        protected States states;

        public bool hostile;
        public float health, healing, attackSpeed = 1, attackTimer, attackRange, syringeSpeed;

        readonly Vector2 apparentOffset = new Vector2(12, 12);
        public Vector2 ApparentCenter => (Position + apparentOffset);
        public float DistanceToPlayer => (Player.MainPlayer.CenterPosition - ApparentCenter).Length();
        public Vector2 DirectionToPlayer => Game1.NormalizeThis(Player.MainPlayer.CenterPosition - (Position + new Vector2(1, 0))); 

        public DoctorEnemy(string doctorName, bool doctorHostile, Texture2D doctorTexture, Color doctorColor, Point doctorSize, Vector2 doctorStartPos, float doctorSyringeSpeed, float doctorHealth, float doctorHealing, float doctorRange, float doctorLayer) : base(Vector2.Zero, 0, doctorName)
        {
            Name = doctorName;
            Size = doctorSize;
            layer = doctorLayer;
            //sprite = doctorTexture;
            color = doctorColor;
            Position = doctorStartPos;
            healing = doctorHealing;
            attackRange = doctorRange;
            health = doctorHealth;
            hostile = doctorHostile;
            syringeSpeed = doctorSyringeSpeed;
            Kinematic = true;
            attackTimer = attackSpeed;

            animator = new Animator(
                new Animation(1, -1, new Animation.AnimationState(Game1.AllSprites["Doctor_Idle"], 0)),     // State 0: Idle
                new Animation(0.1f , 0, new Animation.AnimationState(Game1.AllSprites["Doctor_Attack"], 0)),// State 1: Attack
                new Animation(100, -1, new Animation.AnimationState(Game1.AllSprites["Doctor_Dying"], 0), new Animation.AnimationState(Game1.AllSprites["Doctor_Dead"], 0.8f)) // State 2: Death
                );

            Tags.Add("Enemy");
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            StateCheck();

            if (attackTimer > 0)
                attackTimer -= deltaTime;

            if (states == States.Attack && attackTimer <= 0)
            {
                ThrowSyringe();
                attackTimer = attackSpeed;
            }

            if (states == States.Dying && animator.CurrentState != 2)
            {
                animator.SetState(2);
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
            else if (DistanceToPlayer > attackRange && health > 0 || !hostile)
            {
                states = States.Idle;
            }
        }

        void ThrowSyringe()
        {
            // Temp bullet creation
            Bullet bullet = new Bullet("Syringe", "Syringe", Bullet.Owner.Enemy, Position + new Vector2(1, 0), DirectionToPlayer * syringeSpeed, Game1.AllSprites["Syringe"], Color.White, new Point(10, 38), healing, 0, 1, "Player");
            animator.SetState(1);
        }
    }
}
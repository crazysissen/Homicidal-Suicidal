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
    class SurgeonEnemy : PhysicsObject, IRenderable
    {
        protected override object Component => this;

        protected States states;

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

        // Angles for the 2nd and 3rd bullet
        readonly float tempBulletAngle = (float)Math.Atan2(1.5f, 2);
        readonly float tempBulletAngle2 = -(float)Math.Atan2(1.5f, 2);

        float sin, cos, sin2, cos2;
        float x, y, x2, y2;

        Vector2 bulletStartPos = new Vector2(7, 12);

        Vector2 apparentOffset = new Vector2(15, 14);
        public Vector2 ApparentCenter => (Position + apparentOffset);
        float DistanceToPlayer => (Player.MainPlayer.CenterPosition - ApparentCenter).Length();
        Vector2 DirectionToPlayer => Game1.NormalizeThis(Player.MainPlayer.CenterPosition - ApparentCenter);

        float healing, scalpelSpeed, range, attackSpeed = 1, attackTimer;
        public float Health { get; set; }

        public bool hostile;

        public SurgeonEnemy(string surgeonName, bool surgeonHostile, Texture2D surgeonTexture, Color surgeonColor, Point surgeonSize, Vector2 surgeonStartPos, float surgeonscalpelSpeed, float surgeonHealth, float surgeonHealing, float surgeonRange, float surgeonLayer) : base(Vector2.Zero, 0, surgeonName)
        {
            Name = surgeonName;
            hostile = surgeonHostile;
            Size = surgeonSize;
            layer = surgeonLayer;
            color = surgeonColor;
            Position = surgeonStartPos;
            healing = surgeonHealing;
            Health = surgeonHealth;
            scalpelSpeed = surgeonscalpelSpeed;
            range = surgeonRange;
            attackTimer = attackSpeed;

            Kinematic = true;
            Tags.Add("Enemy");

            cos = (float)Math.Cos(tempBulletAngle);
            sin = (float)Math.Sin(tempBulletAngle);
            cos2 = (float)Math.Cos(tempBulletAngle2);
            sin2 = (float)Math.Sin(tempBulletAngle2);

            animator = new Animator(
                new Animation(1, -1, false, new Animation.AnimationState(Game1.AllSprites["Surgeon_Idle"], 0)),     // State 0: Idle
                new Animation(0.1f, 0, false, new Animation.AnimationState(Game1.AllSprites["Surgeon_Attack"], 0)),// State 1: Attack
                new Animation(100, -1, false, new Animation.AnimationState(Game1.AllSprites["Surgeon_Dying"], 0), new Animation.AnimationState(Game1.AllSprites["Surgeon_Dead"], 0.8f)) // State 2: Death
                );
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            StateCheck();

            if (attackTimer > 0)
                attackTimer -= deltaTime;

            if (states == States.Attack && attackTimer <= 0)
            {
                ThrowScalpels();
                attackTimer = attackSpeed;
            }
        }

        void StateCheck()
        {
            // Die logic
            if (Health <= 0)
            {
                states = States.Dying;
                
                if (animator.CurrentState != 2)
                {
                    Tags.Remove("Enemy");
                    Tags.Add("Ground");
                    animator.SetState(2);
                }
            }
            else if (DistanceToPlayer <= range && hostile)
            {
                states = States.Attack;
            }
            else if (DistanceToPlayer > range && Health > 0 || !hostile)
            {
                states = States.Idle;
            }
        }

        void ThrowScalpels()
        {
            animator.SetState(1);

            Bullet surgeonBullet = new Bullet("Scalpel", "Scalpel", Bullet.Owner.Enemy, Position + bulletStartPos, DirectionToPlayer * scalpelSpeed, Game1.AllSprites["Scalpel"], Color.White, new Point(5, 20), healing, 0, 0.1f, "Player");
            surgeonBullet.Tags.Add("Bullet");

            x = DirectionToPlayer.X * cos - DirectionToPlayer.Y * sin;
            y = DirectionToPlayer.X * sin + DirectionToPlayer.Y * cos;
            x2 = DirectionToPlayer.X * cos2 - DirectionToPlayer.Y * sin2;
            y2 = DirectionToPlayer.X * sin2 + DirectionToPlayer.Y * cos2;

            Vector2 bullet1Vector = new Vector2(x, y);
            Vector2 bullet2Vector = new Vector2(x2, y2);

            surgeonBullet = new Bullet("Scalpel1", "Scalpel", Bullet.Owner.Enemy, Position + bulletStartPos, bullet1Vector * scalpelSpeed, Game1.AllSprites["Scalpel"], Color.White, new Point(5, 20), healing, 0, 0.1f, "Player");
            surgeonBullet.Tags.Add("Bullet");
            surgeonBullet = new Bullet("Scalpel2", "Scalpel", Bullet.Owner.Enemy, Position + bulletStartPos, bullet2Vector * scalpelSpeed, Game1.AllSprites["Scalpel"], Color.White, new Point(5, 20), healing, 0, 0.1f, "Player");
            surgeonBullet.Tags.Add("Bullet");
        }
    }
}

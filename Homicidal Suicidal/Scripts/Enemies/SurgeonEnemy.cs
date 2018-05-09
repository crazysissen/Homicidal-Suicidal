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

        // Temporary Angles for the 2nd and 3rd bullet
        readonly float tempBulletAngle = (float)Math.Atan2(1, 2);
        readonly float tempBulletAngle2 = -(float)Math.Atan2(1, 2);

        float sin, cos, sin2, cos2;
        float x, y, x2, y2;

        Vector2 bulletStartPos = new Vector2(7, 12);

        Vector2 apparentOffset = new Vector2(15, 14);
        public Vector2 ApparentCenter => (Position + apparentOffset);
        float DistanceToPlayer => (Player.MainPlayer.CenterPosition - ApparentCenter).Length();
        Vector2 DirectionToPlayer => Game1.NormalizeThis(Player.MainPlayer.CenterPosition - ApparentCenter);
                
        float health, healing, scalpelSpeed, range, attackSpeed = 1, attackTimer, attackRange;

        public bool hostile;

        public SurgeonEnemy(string surgeonName, bool surgeonHostile, Texture2D surgeonTexture, Color surgeonColor, Point surgeonSize, Vector2 surgeonStartPos, float surgeonscalpelSpeed, float surgeonHealth, float surgeonHealing, float surgeonRange, float surgeonLayer) : base(Vector2.Zero, 0, surgeonName)
        {
            Name = surgeonName;
            hostile = surgeonHostile;
            Size = surgeonSize;
            layer = surgeonLayer;
            sprite = surgeonTexture;
            color = surgeonColor;
            Position = surgeonStartPos;
            healing = surgeonHealing;
            health = surgeonHealth;
            scalpelSpeed = surgeonscalpelSpeed;
            range = surgeonRange;
            attackTimer = attackSpeed;

            Kinematic = true;
            Tags.Add("Enemy");

            cos = (float)Math.Cos(tempBulletAngle);
            sin = (float)Math.Sin(tempBulletAngle);
            cos2 = (float)Math.Cos(tempBulletAngle2);
            sin2 = (float)Math.Sin(tempBulletAngle2);
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
            if (health <= 0)
            {
                states = States.Dying;
                //DestroyObject();
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

        void ThrowScalpels()
        {
            Bullet surgeonBullet = new Bullet("scalpel", "scalpel", Bullet.Owner.Enemy, Position + bulletStartPos, DirectionToPlayer * scalpelSpeed, Game1.AllSprites["Scalpel"], Color.White, new Point(5, 20), healing, 0, 1, "Player");

            x = DirectionToPlayer.X * cos - DirectionToPlayer.Y * sin;
            y = DirectionToPlayer.X * sin + DirectionToPlayer.Y * cos;
            x2 = DirectionToPlayer.X * cos2 - DirectionToPlayer.Y * sin2;
            y2 = DirectionToPlayer.X * sin2 + DirectionToPlayer.Y * cos2;

            Vector2 bullet1Vector = new Vector2(x, y);
            Vector2 bullet2Vector = new Vector2(x2, y2);

            Bullet surgeonBullet1 = new Bullet("scalpel1", "scalpel", Bullet.Owner.Enemy, Position + bulletStartPos, bullet1Vector * scalpelSpeed, Game1.AllSprites["Scalpel"], Color.White, new Point(5, 20), healing, 0, 1, "Player");
            Bullet surgeonBullet2 = new Bullet("scalpel2", "scalpel", Bullet.Owner.Enemy, Position + bulletStartPos, bullet2Vector * scalpelSpeed, Game1.AllSprites["Scalpel"], Color.White, new Point(5, 20), healing, 0, 1, "Player");
        }
    }
}

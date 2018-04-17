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

        float syringeHealing, syringeSpeed;

        float health, healing, attackSpeed = 1, attackTimer, attackRange;
        float DistanceToPlayer => (Player.MainPlayer.CenterPosition - CenterPosition).Length();

        public DoctorEnemy(string doctorName, Texture2D doctorTexture, Color doctorColor, Point doctorSize, Vector2 doctorStartPos, float doctorHealth, float doctorHealing, float doctorRange, float doctorLayer) : base(doctorName)
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

            attackTimer = attackSpeed;

            Tags.Add("Enemy");
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            base.Update(gameTime, deltaTime);

            StateCheck();

            if (attackTimer > 0)
                attackTimer -= deltaTime;

            if (states == States.Attack && attackTimer <= 0)
                ThrowNeedle();
        }

        void StateCheck()
        {
            // The doctor died this frame
            if (health <= 0)
            {
                states = States.Dying;
            }
            // If the player is within radius of the doctors attack range
            else if (DistanceToPlayer <= attackRange)
            {
                states = States.Attack;
            }
            // If he's not attacking or dying he idles
            else
            {
                states = States.Idle;
            }
        }

        // He attac
        void ThrowNeedle()
        {
            attackTimer = attackSpeed;
            Bullet bullet = new Bullet("Syringe", new Vector2(1, 0), Game1.allSprites["Square"], Color.White, new Rectangle(), syringeHealing, 0, 9999, "Player");
        }
    }
}
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

        Vector2 apparentOffset = new Vector2(15, 14);
        public Vector2 ApparentCenter => (Position + apparentOffset);
        float DistanceToPlayer => (Player.MainPlayer.CenterPosition - ApparentCenter).Length();
                
        float health, healing, syringeSpeed, range, attackSpeed = 1, attackTimer, attackRange;

        public bool hostile;

        public SurgeonEnemy(string surgeonName, bool surgeonHostile, Texture2D surgeonTexture, Color surgeonColor, Point surgeonSize, Vector2 surgeonStartPos, float surgeonSyringeSpeed, float surgeonHealth, float surgeonHealing, float surgeonRange, float surgeonLayer) : base(Vector2.Zero, 0, surgeonName)
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
            syringeSpeed = surgeonSyringeSpeed;
            range = surgeonRange;

            Kinematic = true;
            Tags.Add("Enemy");
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            StateCheck();
        }

        void StateCheck()
        {
            // Die logic
            if (health <= 0)
            {
                states = States.Dying;
                DestroyObject();
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

        void ThrowNeedles()
        {

        }
    }
}

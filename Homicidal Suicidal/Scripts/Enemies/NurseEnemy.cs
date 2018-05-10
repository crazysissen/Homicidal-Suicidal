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

    class NurseEnemy : PhysicsObject, IRenderable
    {
        protected States states;

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

        public const float maxHealth = 100;

        public bool hostile;

        Vector2 apparentOffset = new Vector2(15, 19);
        public Vector2 ApparentCenter => (Position + apparentOffset);
        public float health, auraRadius, healingMultiplier;
        public float DistanceToPlayer => (Player.MainPlayer.CenterPosition - ApparentCenter).Length();

        public NurseEnemy(string nurseName, bool nurseHostile, Texture2D nurseTexture, Color nurseColor, Point nurseSize, Vector2 nurseStartPos, float nurseMaxHealthAuraHealingMultiplier, float nurseAuraRadius, float nurseLayer) : base(Vector2.Zero, 0, nurseName)
        {
            Name = nurseName;
            Size = nurseSize;
            layer = nurseLayer;
            sprite = nurseTexture;
            color = nurseColor;
            Position = nurseStartPos;
            auraRadius = nurseAuraRadius;
            health = maxHealth;
            healingMultiplier = nurseMaxHealthAuraHealingMultiplier;
            hostile = nurseHostile;

            Kinematic = true;
            Tags.Add("Enemy");
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            StateCheck();

            if (DistanceToPlayer <= auraRadius && hostile)
            {
                Player.MainPlayer.Heal(Player.maxHealth * healingMultiplier * deltaTime);
            }
        }

        void StateCheck()
        {
            // Die logic
            if (health <= 0)
            {
                states = States.Dying;
                hostile = false;
                //DestroyObject();
            }
            else if (health > 0)
            {
                states = States.Attack;
            }
        }
    }
}

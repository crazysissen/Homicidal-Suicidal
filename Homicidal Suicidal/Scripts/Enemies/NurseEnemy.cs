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

        public const float maxHealth = 100;

        public bool hostile;

        NurseAura nurseAura;

        Vector2 apparentOffset = new Vector2(15, 19);
        public Vector2 ApparentCenter => (Position + apparentOffset);
        float healingMultiplier;
        int auraRadius;
        public float Health { get; set; }
        public float DistanceToPlayer => (Player.MainPlayer.CenterPosition - ApparentCenter).Length();

        public NurseEnemy(string nurseName, bool nurseHostile, Color nurseColor, Point nurseSize, Vector2 nurseStartPos, float nurseMaxHealthAuraHealingMultiplier, int nurseAuraRadius, float nurseLayer) : base(Vector2.Zero, 0, nurseName)
        {
            Name = nurseName;
            Size = nurseSize;
            layer = nurseLayer;
            color = nurseColor;
            Position = nurseStartPos;
            auraRadius = nurseAuraRadius;
            Health = maxHealth;
            healingMultiplier = nurseMaxHealthAuraHealingMultiplier;
            hostile = nurseHostile;

            Kinematic = true;
            Tags.Add("Enemy");

            nurseAura = new NurseAura(ApparentCenter + new Vector2(65, 20), Color.White, Game1.AllSprites["Healing_Aura"], new Rectangle(-auraRadius, -auraRadius, 2 * auraRadius, 2 * auraRadius), "Healing Aura", auraRadius, 1, 0.99f);

            animator = new Animator(
                new Animation(1, -1, new Animation.AnimationState(Game1.AllSprites["Nurse_Healing"], 0)),// State 1: Attack
                new Animation(100, -1, new Animation.AnimationState(Game1.AllSprites["Nurse_Dying"], 0), new Animation.AnimationState(Game1.AllSprites["Nurse_Dead"], 0.8f)) // State 2: Death
                );
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
            if (Health <= 0)
            {
                states = States.Dying;
                nurseAura.DestroyAura();
                hostile = false;
                //DestroyObject();

                if (animator.CurrentState != 1)
                {
                    Tags.Remove("Enemy");
                    Tags.Add("Ground");
                    animator.SetState(1);
                }
            }
            else if (Health > 0)
            {
                states = States.Attack;
            }
        }
    }
}

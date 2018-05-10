using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    class Ambulance : PhysicsObject, IRenderable
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

        Color IRenderable.SpriteColor { get => color; }
        Color color;

        // Make sure to set the sprite and color variables in the constructor.
        // Example: [ public MyClass(string name, Texture2D texture, Color spriteColor) : base(name) { sprite = texture; color = spriteColor; } ]
        // The ": base()" refers to the base class WorldObject that in and of itself takes at least one string in the constructor.

        #endregion

        public const float
            heal = 50,
            layer = 0,
            defaultSpeed = 2,
            acceleration = 2.2f,
            distancePerSpeedStep = 150;

        float Speed => defaultSpeed + acceleration * (0.2f * ((DistanceToPlayer / distancePerSpeedStep) * (DistanceToPlayer / distancePerSpeedStep))); // TODO
        float DistanceToPlayer => Player.MainPlayer.Position.X - Position.X - Offset.X * 2;

        public Ambulance(Rectangle rectangle) : base(Vector2.Zero, 0, "Ambulance", rectangle)
        {
            color = Color.White;

            IgnoreCollision = new string[] { "Ground", "Bullet", "Enemy" };

            animator = new Animator(new Animation(1, -1, 
                new Animation.AnimationState(Game1.AllSprites["Square"] /*TODO*/, 0), 
                new Animation.AnimationState(Game1.AllSprites["Square"] /*TODO*/, 0.5f)));
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            Velocity = new Vector2(Speed, 0);
        }

        public override void OnCollision(PhysicsObject physicsObject)
        {
            if (physicsObject.Tags.Contains("Player"))
            {
                Player player = physicsObject.GetComponent<Player>(out bool successful);
                player.Heal(Player.maxHealth);
            }
        }
    }
}

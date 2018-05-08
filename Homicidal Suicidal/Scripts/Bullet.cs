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
    class Bullet : PhysicsObject, IRenderable
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

        public enum Owner { Player, Enemy };
        Owner owner;

        public Vector2 DirectionToPlayer => Game1.NormalizeThis(Player.MainPlayer.CenterPosition - CenterPosition);

        string targetTag;

        float healing, balancedVariable = 4;

        bool alive;

        public Bullet(string bulletName, string bulletTag, Owner bulletOwner, Vector2 initialPosition, Vector2 initialVelocity, Texture2D bulletSprite, Color bulletColor, Point bulletSize, float bulletHealing, float bulletRotation, float bulletLayer, string bulletTargetTag) : base(initialVelocity, 0, bulletName)
        {
            Name = bulletName;
            layer = bulletLayer;
            sprite = bulletSprite;
            color = bulletColor;
            healing = bulletHealing;
            rotation = bulletRotation;
            targetTag = bulletTargetTag;
            Velocity = initialVelocity;
            Tags.Add(bulletTag);
            alive = true;
            owner = bulletOwner;

            Kinematic = true;
            Size = bulletSize;
            CenterPosition = initialPosition;
        }
        
        protected override void Update(GameTime gameTime, float deltaTime)
        {
            //double bulletAngle = Math.Atan2(Velocity.Y, Velocity.X);

            //// Top Right Quadrant
            //if (Velocity.X >= 0 && Velocity.Y >= 0)
            //{
            //    rotation = (float)((Math.PI * 0.5f) - (Math.Atan2(Velocity.Y, Velocity.X)));
            //}
            //// Bottom Right Quadrant
            //else if (Velocity.X >= 0 && Velocity.Y < 0)
            //{
            //    rotation = (float)(Math.PI - Math.Atan2(-Velocity.Y, Velocity.X));
            //}
            //// Bottom Left Quadrant
            //else if (Velocity.X < 0 && Velocity.Y < 0)
            //{
            //    rotation = (float)(Math.PI + Math.Atan2(-Velocity.Y, -Velocity.X));
            //}
            //// Top Left Quadrant
            //else if (Velocity.X < 0 && Velocity.Y >= 0)
            //{
            //    rotation = (float)((Math.PI * 1.5f) - (Math.Atan2(Velocity.Y, -Velocity.X)));
            //}

            if (Tags.Contains("Syringe"))
            {
                //rotation = (float)-newBulletAngle;

                // AIMLOCK ON PLAYER VERSION
                Velocity = Game1.NormalizeThis(Player.MainPlayer.CenterPosition - Position) * balancedVariable;

                // SWARM VERSION
                //Velocity += Game1.NormalizeThis(Player.MainPlayer.CenterPosition - CenterPosition) / 4;
            }
        }

        public override void OnCollision(PhysicsObject physicsObject)
        {
            bool[] successes = new bool[4];

            Player player = physicsObject.GetComponent<Player>(out successes[0]);
            DoctorEnemy doctor = physicsObject.GetComponent<DoctorEnemy>(out successes[1]);
            NurseEnemy nurse = physicsObject.GetComponent<NurseEnemy>(out successes[2]);
            SurgeonEnemy surgeon = physicsObject.GetComponent<SurgeonEnemy>(out successes[3]);

            if (successes[0] && targetTag == "Player") // Player
            {
                //Player.MainPlayer.Health -= healing;

                DestroyObject();
            }

            if (targetTag == "Enemy")
            {
                if (successes[1]) // Doctor
                {
                    doctor.health -= healing;

                    DestroyObject();
                }

                if (successes[2]) // Nurse
                {
                    // nurse.health -= damage;

                    DestroyObject();
                }

                if (successes[3]) // Surgeon
                {
                    // surgeon.health -= damage;

                    DestroyObject();
                }

                
            }
        }
    }
}

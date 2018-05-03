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

        public enum Owner { Player, Enemy };
        Owner owner;

        public Vector2 DirectionToPlayer => Game1.NormalizeThis(Player.MainPlayer.CenterPosition - CenterPosition);

        string targetTag;

        float damage, balancedVariable = 4;

        bool alive;

        public Bullet(string bulletName, string bulletTag, Owner bulletOwner, Vector2 initialPosition, Vector2 initialVelocity, Texture2D bulletSprite, Color bulletColor, Point bulletSize, float bulletDamage, float bulletRotation, float bulletLayer, string bulletTargetTag) : base(initialVelocity, 0, bulletName)
        {
            Name = bulletName;
            layer = bulletLayer;
            sprite = bulletSprite;
            color = bulletColor;
            damage = bulletDamage;
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
            float bulletDir = Velocity.Length();
            double bulletAngle = Math.Atan2(Velocity.Y, Velocity.X);
            double directionToPlayerAngle = Math.Atan2(DirectionToPlayer.Y, DirectionToPlayer.X);
            double newBulletAngle = bulletAngle - directionToPlayerAngle;

            if (Tags.Contains("Syringe"))
            {
                rotation = (float)-newBulletAngle;

                Velocity = Velocity += Game1.NormalizeThis((DirectionToPlayer/* - Velocity*/) / balancedVariable);
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
                Player.MainPlayer.Health += damage;

                DestroyObject();
            }

            if (targetTag == "Enemy")
            {
                if (successes[1]) // Doctor
                {
                    doctor.health -= damage;

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

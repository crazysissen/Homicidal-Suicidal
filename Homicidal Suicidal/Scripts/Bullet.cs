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

        string targetTag;

        float damage;

        bool alive;

        public Bullet(string bulletName, string bulletTag, Owner bulletOwner, Vector2 initialVelocity, Texture2D bulletSprite, Color bulletColor, Rectangle bulletRectangle, float bulletHealing, float bulletRotation, float bulletLayer, string bulletTargetTag) : base(initialVelocity, 0, bulletName, bulletRectangle)
        {
            Name = bulletName;
            layer = bulletLayer;
            sprite = bulletSprite;
            color = bulletColor;
            damage = bulletHealing;
            rotation = bulletRotation;
            targetTag = bulletTargetTag;
            Velocity = initialVelocity;
            Tags.Add(bulletTag);
            alive = true;
            owner = bulletOwner;
        }

        protected override void Update(GameTime gameTime, float deltaTime)
        {
            base.Update(gameTime, deltaTime);

        }

        public override void OnCollision(PhysicsObject physicsObject)
        {
            if (owner == Owner.Player)
            {
                Player player = physicsObject.GetComponent<Player>();

                return;
            }
            
            if (physicsObject.GetComponent<IEnemy>().ThisEnemyType == EnemyType.Doctor)
            {
                DoctorEnemy doctor = physicsObject.GetComponent<DoctorEnemy>();

                return;
            }

            if (physicsObject.GetComponent<IEnemy>().ThisEnemyType == EnemyType.Nurse)
            {
                NurseEnemy nurse = physicsObject.GetComponent<NurseEnemy>();

                return;
            }

            if (physicsObject.GetComponent<IEnemy>().ThisEnemyType == EnemyType.Surgeon)
            {
                SurgeonEnemy surgeon = physicsObject.GetComponent<SurgeonEnemy>();

                return;
            }
        }
    }
}

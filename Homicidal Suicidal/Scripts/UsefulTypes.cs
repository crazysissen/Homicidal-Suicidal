using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HomicidalSuicidal
{
    public enum EnemyType { Doctor, Nurse, Surgeon }

    public enum Direction { Up, Down, Left, Right }

    public static class ExtensionMethods
    {
        public static Vector2 VelocityNullifier(this Vector2 movement, Vector2 offset)
        {
            return new Vector2(
                ((movement.X > 0 && offset.X < 0) || (movement.X < 0 && offset.X > 0)) ? 0 : 1,
                ((movement.Y > 0 && offset.Y < 0) || (movement.Y < 0 && offset.Y > 0)) ? 0 : 1);
        }

        public static bool Ignores(this PhysicsObject physicsObject, WorldObject subject)
        {
            foreach (string tag in physicsObject.IgnoreCollision)
            {
                foreach (string compareTag in subject.Tags)
                {
                    if (tag == compareTag)
                        return true;
                }
            }

            return false;
        }
    }

    public static class Methods
    {
        static KeyboardState previous;
        static bool leftMousePressed, rightMousePressed;

        public static void UpdateMethods()
        {
            previous = Keyboard.GetState();

            MouseState state = Mouse.GetState();
            leftMousePressed = state.LeftButton == ButtonState.Pressed;
            rightMousePressed = state.RightButton == ButtonState.Pressed;
        }

        public static bool KeyDown(Keys key) => (!previous.IsKeyDown(key) && Keyboard.GetState().IsKeyDown(key));

        public static bool LeftMouseDown() => Mouse.GetState().LeftButton == ButtonState.Pressed && !leftMousePressed;

        public static bool RightMouseDown() => Mouse.GetState().RightButton == ButtonState.Pressed && !rightMousePressed;

    }
}

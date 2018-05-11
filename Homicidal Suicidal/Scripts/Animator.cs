using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomicidalSuicidal
{
    public class Animator
    {
        public static List<Animator> allAnimators = new List<Animator>();

        Animation[] animations;

        public int CurrentState { get; private set; }
        public float CurrentTime { get; set; }

        bool transferLive;
        float transferBackTime;

        public Animator(params Animation[] animations)
        {
            this.animations = animations;
            allAnimators.Add(this);
        }

        public void SetState(int state)
        {
            if (state < animations.Length && state >= 0)
            {
                CurrentState = state;
                CurrentTime = 0;
            }

            Console.WriteLine("State set");
        }

        public void SetState(int state, float time)
        {
            SetState(state);
            CurrentTime = time;
        }

        public void Update(float deltaTime)
        {
            CurrentTime += deltaTime;
            if (CurrentTime > animations[CurrentState].endTime)
            {
                CurrentTime -= animations[CurrentState].endTime;
                
                if (animations[CurrentState].autoTransfer >= 0 && animations[CurrentState].autoTransfer < animations.Length)
                {
                    if (!animations[CurrentState].keepCurrentTime)
                        CurrentTime = 0;

                    CurrentState = animations[CurrentState].autoTransfer;
                }
            }
        }

        public Texture2D GetTexture()
        {
            if (animations.Length > 0)
                for (int i = animations[CurrentState].animationStates.Length - 1; i >= 0; --i)
                    if (animations[CurrentState].animationStates[i].timestamp <= CurrentTime)
                        return animations[CurrentState].animationStates[i].texture;

            return Game1.AllSprites["Square"];
        }

        public void ChangeState(int state)
        {
            if (state >= allAnimators.Count || state < 0)
                return;

            CurrentState = state;
            CurrentTime = 0;
        }

        public static void UpdateAll(float deltaTime)
        {
            foreach (Animator animator in allAnimators)
            {
                animator.Update(deltaTime);
            }
        }
    }

    public class Animation
    {
        public AnimationState[] animationStates;
        public float endTime;
        public int autoTransfer;
        public bool keepCurrentTime;

        public Animation(float endTime, int autoTransfer = -1, bool keepCurrentTime = false, params AnimationState[] states) : this(endTime, states, autoTransfer, keepCurrentTime) { }

        public Animation(float endTime, AnimationState[] states, int autoTransfer = -1, bool keepCurrentTime = false)
        {
            animationStates = states;
            this.endTime = endTime;
            this.autoTransfer = autoTransfer;
            this.keepCurrentTime = keepCurrentTime;
        }

        public struct AnimationState
        {
            public Texture2D texture;
            public float timestamp;

            public AnimationState(Texture2D texture, float timestamp)
            {
                this.texture = texture;
                this.timestamp = timestamp;
            }
        }
    }
}

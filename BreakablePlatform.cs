using System.Threading.Channels;
using SFML.Graphics;
using SFML.System;

namespace Platformer
{
    public class BreakablePlatform : Entity
    {
        public override bool Solid => true;
        public bool IsBroken;
        public Clock BreakTimer;
        
        private Dictionary<AnimationState , IntRect> spriteRects;
        private AnimationState currentSprite;
        
        public BreakablePlatform() : base("tileset")
        {
            spriteRects = new Dictionary<AnimationState, IntRect>();
            spriteRects.Add(AnimationState.State1, new IntRect(6 * 18, 0, 18, 18));
            spriteRects.Add(AnimationState.State2, new IntRect(7 * 18, 0, 18, 18));
            
            sprite.TextureRect = spriteRects[AnimationState.State1];
            currentSprite = AnimationState.State1;
            sprite.Origin = new Vector2f(9f, 9f);

            IsBroken = false;
        }

        public override void Update(Scene scene, float deltaTime)
        {
            if (IsBroken)
            {
                if (currentSprite == AnimationState.State1 && BreakTimer.ElapsedTime.AsSeconds() > 0.2)
                {
                    currentSprite = AnimationState.State2;
                    sprite.TextureRect = spriteRects[currentSprite];
                }

                if (currentSprite == AnimationState.State2 && BreakTimer.ElapsedTime.AsSeconds() > 0.3)
                {
                    Dead = true;
                }
            }
        }
    }
}

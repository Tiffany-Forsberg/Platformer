using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public class Coin : Entity
    {
        private Dictionary<AnimationState , IntRect> spriteRects;
        private AnimationState currentSprite;
        private Clock spinTimer;
        
        public Coin() : base("tileset")
        {
            spriteRects = new Dictionary<AnimationState, IntRect>();
            spriteRects.Add(AnimationState.State1, new IntRect(200, 128, 13, 13));
            spriteRects.Add(AnimationState.State2, new IntRect(218, 128, 13, 13));

            sprite.TextureRect = spriteRects[AnimationState.State1];
            currentSprite = AnimationState.State1;
            sprite.Origin = new Vector2f(6.5f, 6.5f);

            spinTimer = new Clock();
        }

        public override void Update(Scene scene, float deltaTime)
        {
            if (spinTimer.ElapsedTime.AsSeconds() > 0.3f)
            {
                if (currentSprite == AnimationState.State1)
                {
                    currentSprite = AnimationState.State2;
                }
                else
                {
                    currentSprite = AnimationState.State1;
                }
                sprite.TextureRect = spriteRects[currentSprite];
                spinTimer.Restart();
            }
            
            if (scene.FindByType<Hero>(out Hero hero))
            {
                if (Collision.RectangleRectangle(Bounds, hero.Bounds, out _))
                {
                    Dead = true;
                    Scene.Coins += 1;
                }
            }
        }
    }
}

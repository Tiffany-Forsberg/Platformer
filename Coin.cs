using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public class Coin : Entity
    {
        private Dictionary<string , IntRect> spriteRects;
        private string currentSprite;
        private Clock spinTimer;
        
        public Coin() : base("tileset")
        {
            spriteRects = new Dictionary<string, IntRect>();
            spriteRects.Add("flat", new IntRect(200, 128, 13, 13));
            spriteRects.Add("tall", new IntRect(218, 128, 13, 13));

            sprite.TextureRect = spriteRects["flat"];
            currentSprite = "flat";
            sprite.Origin = new Vector2f(6.5f, 6.5f);

            spinTimer = new Clock();
        }

        public override void Update(Scene scene, float deltaTime)
        {
            if (spinTimer.ElapsedTime.AsSeconds() > 0.3f)
            {
                if (currentSprite == "flat")
                {
                    currentSprite = "tall";
                }
                else
                {
                    currentSprite = "flat";
                }
                sprite.TextureRect = spriteRects[currentSprite];
                spinTimer.Restart();
            }
            
            if (scene.FindByType<Hero>(out Hero hero))
            {
                if (Collision.RectangleRectangle(Bounds, hero.Bounds, out _))
                {
                    Dead = true;
                }
            }
        }
    }
}

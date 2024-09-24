using SFML.Graphics;
using SFML.System;

namespace Platformer
{
    public class Gui : Entity
    {
        private Dictionary<int, IntRect> numberRects;
        private Dictionary<string, IntRect> coinSpriteRects;
        private string coinCurrentSprite;
        
        public Gui() : base("tileset")
        {
            numberRects = new Dictionary<int, IntRect>();
            numberRects.Add(0, new IntRect(4, 146, 13, 13));
            numberRects.Add(1, new IntRect(22, 146, 13, 13));
            numberRects.Add(2, new IntRect(40, 146, 13, 13));
            numberRects.Add(3, new IntRect(58, 146, 13, 13));
            numberRects.Add(4, new IntRect(76, 146, 13, 13));
            numberRects.Add(5, new IntRect(94, 146, 13, 13));
            numberRects.Add(6, new IntRect(112, 146, 13, 13));
            numberRects.Add(7, new IntRect(130, 146, 13, 13));
            numberRects.Add(8, new IntRect(148, 146, 13, 13));
            numberRects.Add(9, new IntRect(166, 146, 13, 13));

            coinSpriteRects = new Dictionary<string, IntRect>();
            coinSpriteRects.Add("flat", new IntRect(200, 128, 13, 13));
            coinSpriteRects.Add("tall", new IntRect(218, 128, 13, 13));
            
            sprite.Origin = new Vector2f(6.5f, 6.5f);
        }

        public override void Render(RenderTarget target)
        {
            sprite.TextureRect = numberRects[Scene.Coins > 9 ? 9 : Scene.Coins];
            target.Draw(sprite);
        }
    }
}

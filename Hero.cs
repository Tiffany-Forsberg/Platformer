using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public class Hero : Entity
    {
        public const float WalkSpeed = 100.0f;
        public const float JumpForce = 250.0f;
        public const float GravityForce = 400.0f;

        private bool faceRight = false;
        
        public Hero() : base("characters")
        {
            sprite.TextureRect = new IntRect(0, 0, 24, 24);
            sprite.Origin = new Vector2f(12, 12);
        }

        public override void Update(Scene scene, float deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                scene.TryMove(this, new Vector2f(-WalkSpeed * deltaTime, 0));
                faceRight = false;
            }
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                scene.TryMove(this, new Vector2f(WalkSpeed * deltaTime, 0));
                faceRight = true;
            }
        }

        public override void Render(RenderTarget target)
        {
            sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1);
            base.Render(target);
        }
    }
}

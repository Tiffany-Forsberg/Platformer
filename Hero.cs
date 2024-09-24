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
        private float verticalSpeed;
        private bool isGrounded;
        private bool isUpPressed;
        private Dictionary<string, IntRect> spriteRects;
        private string currentSprite;
        private Clock animationTimer;
        
        public Hero() : base("characters")
        {
            spriteRects = new Dictionary<string, IntRect>();
            spriteRects.Add("stand", new IntRect(0, 0, 24, 24));
            spriteRects.Add("walk", new IntRect(24, 0, 24, 24));
            
            sprite.TextureRect = spriteRects["stand"];
            sprite.Origin = new Vector2f(12, 12);

            animationTimer = new Clock();
        }
        
        public override FloatRect Bounds {
            get {
                var bounds = base.Bounds;
                bounds.Left += 3;
                bounds.Width -= 6;
                bounds.Top += 3;
                bounds.Height -= 3;
                return bounds;
            }
        }

        public override void Update(Scene scene, float deltaTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                scene.TryMove(this, new Vector2f(WalkSpeed * deltaTime, 0));
                faceRight = true;
                HandleWalkAnimation();
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                scene.TryMove(this, new Vector2f(-WalkSpeed * deltaTime, 0));
                faceRight = false;
                HandleWalkAnimation();
            }
            else
            {
                currentSprite = "stand";
                sprite.TextureRect = spriteRects[currentSprite];
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                if (isGrounded && !isUpPressed)
                {
                    verticalSpeed = -JumpForce;
                    isUpPressed = true;
                }
            }
            else
            {
                isUpPressed = false;
            }

            verticalSpeed += GravityForce * deltaTime;
            if (verticalSpeed > 500.0f) verticalSpeed = 500.0f;

            isGrounded = false;
            Vector2f velocity = new Vector2f(0, verticalSpeed * deltaTime);
            if (scene.TryMove(this, velocity))
            {
                if (verticalSpeed > 0.0f)
                {
                    isGrounded = true;
                    verticalSpeed = 0.0f;
                }

                verticalSpeed = -0.5f * verticalSpeed;
            }
            
            if (sprite.Position.X > Program.ViewSize.X || sprite.Position.X < 0)
            {
                scene.Reload();
            }
            
            if (sprite.Position.Y > Program.ViewSize.Y || sprite.Position.Y < 0)
            {
                scene.Reload();
            }
        }

        private void HandleWalkAnimation()
        {
            if (animationTimer.ElapsedTime.AsSeconds() < 0.2f) return;
            
            if (currentSprite == "stand")
            {
                currentSprite = "walk";
            }
            else
            {
                currentSprite = "stand";
            }
            
            sprite.TextureRect = spriteRects[currentSprite];
            animationTimer.Restart();
        }

        public override void Render(RenderTarget target)
        {
            sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1);
            base.Render(target);
        }
    }
}

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
                }

                verticalSpeed = 0.0f;
            }
        }

        public override void Render(RenderTarget target)
        {
            sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1);
            base.Render(target);
        }
    }
}

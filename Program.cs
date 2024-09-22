using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer {
    class Program {
        static void Main(string[] args) {
            using (var window = new RenderWindow(
                new VideoMode(800, 600),
                "Platformer"
            )) {
                window.Closed += (o, e) => window.Close();

                // Initialize
                Clock clock = new Clock();
                Scene scene = new Scene();

                window.SetView(new View(new Vector2f(200, 150), new Vector2f(400,300)));

                // Spawn entities
                for (int i = 0; i < 10; i++)
                {
                    scene.Spawn(new Platform { Position = new Vector2f(18 + i * 18, 288) });
                }
                scene.Spawn(new Door {Position = new Vector2f(18, 267.5f)});
                scene.Spawn(new Key{Position = new Vector2f(60, 60)});
                scene.Spawn(new Background());

                while (window.IsOpen) {
                    window.DispatchEvents();
                    float deltaTime = clock.Restart().AsSeconds();
                    
                    // Updates
                    scene.UpdateAll(deltaTime);
                    window.Clear();
                    
                    // Drawing
                    scene.RenderAll(window);
                    window.Display();
                }
            }
        }
    }
}

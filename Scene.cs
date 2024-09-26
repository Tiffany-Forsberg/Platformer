using System.Text;
using SFML.Graphics;
using SFML.System;

namespace Platformer
{
    public class Scene
    {
        public static int Coins;
        
        private readonly Dictionary<string, Texture> textures;
        private readonly List<Entity> entities;
        private string currentScene;
        private string? nextScene;

        public Scene()
        {
            textures = new Dictionary<string, Texture>();
            entities = new List<Entity>();
            
            Coins = 0;
        }

        public bool TryMove(Entity entity, Vector2f movement)
        {
            entity.Position += movement;
            bool collided = false;

            for (int i = 0; i < entities.Count; i++)
            {
                Entity other = entities[i];
                if (!other.Solid) continue;
                if (other == entity) continue;

                FloatRect boundsA = entity.Bounds;
                FloatRect boundsB = other.Bounds;

                if (Collision.RectangleRectangle(boundsA, boundsB, out Collision.Hit hit))
                {
                    entity.Position += hit.Normal * hit.Overlap;
                    i = -1;
                    collided = true;

                    if (!other.Dead && other is BreakablePlatform platform)
                    {
                        if (
                            platform.Bounds.Left + 2 < entity.Bounds.Left + entity.Bounds.Width &&
                            platform.Bounds.Left + platform.Bounds.Width - 2 > entity.Bounds.Left
                        )
                        {
                            if (entity.Bounds.Top < platform.Bounds.Top); // Bugfix for issue where platform breaks when player jumps of it
                            else if (platform.Bounds.Top + platform.Bounds.Height >= entity.Bounds.Top)
                            {
                                platform.IsBroken = true;
                                platform.BreakTimer = new Clock();
                            }
                        }
                    }
                }
            }
            
            return collided;
        }

        public bool FindByType<T>(out T found) where T : Entity
        {
            foreach (var entity in entities)
            {
                if (!entity.Dead && entity is T typed)
                {
                    found = typed;
                    return true;
                }
            }
            
            found = default(T);
            return false;
        }

        public void Spawn(Entity entity)
        {
            entities.Add(entity);
            entity.Create(this);
        }

        public Texture LoadTexture(string name)
        {
            if (textures.TryGetValue(name, out Texture found))
            {
                return found;
            }

            string fileName = $"assets/{name}.png";
            Texture texture = new Texture(fileName);
            textures.Add(name, texture);
            return texture;
        }

        public void Reload()
        {
            nextScene = currentScene;
        }

        public void Load(string nextScene)
        {
            this.nextScene = nextScene;
        }

        private void HandleSceneChange()
        {
            if (nextScene == null) return;
            entities.Clear();
            Spawn(new Background());

            string file = $"assets/{nextScene}.txt";
            Console.WriteLine($"Loading scene '{file}'");

            foreach (var line in File.ReadLines(file, Encoding.UTF8))
            {
                string parsed = line.Trim();

                int commentAt = parsed.IndexOf('#');
                if (commentAt >= 0)
                {
                    parsed = parsed.Substring(0, commentAt);
                    parsed = parsed.Trim();
                }

                if (parsed.Length == 0) continue;

                string[] words = parsed.Split(" ");

                int x = int.Parse(words[1]);
                int y = int.Parse(words[2]);
                
                switch (words[0])
                {
                    case "w" :
                        Spawn(new Platform { Position = new Vector2f(x, y) });
                        break;
                    case "b" :
                        Spawn(new BreakablePlatform { Position = new Vector2f(x, y) });
                        break;
                    case "d" :
                        Spawn(new Door { Position = new Vector2f(x, y), NextRoom = words[3]});
                        break;
                    case "s" :
                        Spawn(new SecretLevelDoor { Position = new Vector2f(x, y), NextRoom = words[3] });
                        break;
                    case "k" :
                        Spawn(new Key { Position = new Vector2f(x, y) });
                        break;
                    case "c" :
                        Spawn(new Coin {Position = new Vector2f(x, y)});
                        break;
                    case "h":
                        Spawn(new Hero { Position = new Vector2f(x, y) });
                        break;
                }
            }

            // Coin counter
            Spawn(new Coin { Position = new Vector2f(345, 40) });
            Spawn(new Gui { Position = new Vector2f(360, 40) });
            
            currentScene = nextScene;
            nextScene = null;
        }

        public void UpdateAll(float deltaTime)
        {
            HandleSceneChange();
            
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Entity entity = entities[i];
                entity.Update(this, deltaTime);
            }

            for (int i = 0; i < entities.Count;)
            {
                Entity entity = entities[i];
                if (entity.Dead) entities.RemoveAt(i);
                else i++;
            }
        }

        public void RenderAll(RenderTarget target)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                entity.Render(target);
            }
        }
    }
}

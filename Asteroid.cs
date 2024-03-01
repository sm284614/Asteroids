using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class Asteroid : Entity //asteroids are just entities with a few extra attributes
    {
        private MaterialType _material;
        private RockSize _rockSize;
        private int _childrenSpawned;

        public Asteroid(GraphicsComponent sprite, PhysicsComponent physicalComponent, MaterialType material, RockSize rockSize, int pointsValue, int childrenSpawned) : base(sprite, physicalComponent, true)
        {
            _material = material;
            _rockSize = rockSize;
            _pointsValue = pointsValue;
            _childrenSpawned = childrenSpawned;
        }
        public static Asteroid CreateAsteroid(Vector2 position, float rotation, MaterialType material, RockSize size, int childrenSpawned)
        {
            //note the Static method here: anyone can ask this class to generate an Asteroid entity
            //instead of all the hard-coded values here, we should make a class for material and for size and store the multipliers there
            int r = Random.Shared.Next(3) + 1;
            string textureName = "asteroid_";
            int mass = 1;
            int pointsValue = 1;
            int health = 1;
            float speed = 1f;
            switch (material) //what's it made of?
            {

                case MaterialType.Ice:
                    textureName += "ice_";
                    pointsValue *= 1;
                    mass *= 2;
                    break;
                case MaterialType.Metal:
                    textureName += "metal_";
                    pointsValue *= 5;
                    mass *= 10;
                    break;
                case MaterialType.Rock:
                default:
                    textureName += "rock_";
                    pointsValue *= 2;
                    mass *= 5;
                    break;
            }
            switch (size)
            {
                case RockSize.Tiny:
                    textureName += "tiny_";
                    pointsValue *= 5;
                    mass *= 4;
                    speed = 3f + (float)Random.Shared.NextDouble() * 3f;
                    break;
                case RockSize.Small:
                    textureName += "small_";
                    pointsValue *= 10;
                    mass *= 8;
                    speed = 2f + (float)Random.Shared.NextDouble() * 2f;
                    break;
                case RockSize.Medium:
                    textureName += "medium_";
                    pointsValue *= 50;
                    mass *= 16;
                    speed = 1f + (float)Random.Shared.NextDouble() * 1f;
                    break;
                case RockSize.Large:
                default:
                    textureName += "large_";
                    pointsValue *= 100;
                    mass *= 32;
                    speed = 0.25f + (float)Random.Shared.NextDouble() * 0.5f;
                    break;
            }
            textureName += r.ToString();
            Texture2D texture = Game1.ContentLoader.Load<Texture2D>(@"graphics\" + textureName);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            GraphicsComponent sprite = new GraphicsComponent(texture, Color.White);
            float impactDamage = mass * speed;
            health = mass * 10;
            float rotationSpeed = (float)Random.Shared.NextDouble() * 0.04f - 0.02f;
            PhysicsComponent physical = new PhysicsComponent(position, rotation, mass, speed, rotationSpeed, impactDamage, health);
            if (size != RockSize.Large)
            {
                float radius = (texture.Width + texture.Height) * 0.25f;
                physical.MoveForwards(radius * 1.5f);
            }
            return new Asteroid(sprite, physical, material, size, pointsValue, childrenSpawned);
        }
        public List<Asteroid> SpawnChildAsteroids() //called when an asteroid dies
        {
            List<Asteroid> children = new List<Asteroid>();
            float randomRotation = Physics.RandomRotation();
            if (_rockSize != RockSize.Tiny) //tiny asteroids don't spawn child asteroids
            {
                RockSize childSize;
                for (int i = 0; i < _childrenSpawned; i++)
                {
                    Asteroid asteroid;
                    switch (_rockSize)
                    {
                        case RockSize.Large:
                            childSize = RockSize.Medium;
                            break;
                        case RockSize.Medium:
                            childSize = RockSize.Small;
                            break;
                        case RockSize.Small:
                        default:
                            childSize = RockSize.Tiny;
                            break;
                    }
                    float rotation = i * (float)(2 * Math.PI / _childrenSpawned) + randomRotation; //face in different directions away from each other
                    Vector2 position = _physicsComponent.Position; //get starting position from this parent object
                    asteroid = Asteroid.CreateAsteroid(position, rotation, _material, childSize, _childrenSpawned);
                    children.Add(asteroid);
                }
            }
            return children;
        }
        public override List<Entity> DoDeathAction()
        {
            List<Entity> children = new List<Entity>();
            children.AddRange(SpawnChildAsteroids());
            return children;
        }
        public enum MaterialType
        {
            Rock,
            Ice,
            Metal
        }
        public enum RockSize
        {
            None,
            Tiny,
            Small,
            Medium,
            Large,
        }
    }
}

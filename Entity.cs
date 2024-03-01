using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class Entity //a moving object on the screen
    {
        protected GraphicsComponent _graphicsComponent; //visible part to be drawn
        protected PhysicsComponent _physicsComponent; //physics part that we move
        protected bool _dead;
        protected bool _wrapsAroundScreen;
        protected int _pointsValue;
        public Entity(GraphicsComponent sprite, PhysicsComponent entityData, bool wrapsAroundScreen)
        {
            _graphicsComponent = sprite;
            _physicsComponent = entityData;
            _wrapsAroundScreen = wrapsAroundScreen;
            _physicsComponent.Radius = _graphicsComponent.Radius;
            _pointsValue = 0;            
        }
        public Rectangle BoundingBox { get => _graphicsComponent.BoundingBox; }
        public Vector2 Position { get => _physicsComponent.Position; }
        public Vector2 Velocity { get => _physicsComponent.Velocity; }
        public float Rotation { get => _physicsComponent.Rotation; }
        public float Mass { get => _physicsComponent.Mass; }
        public float Radius { get => _physicsComponent.Radius; }
        public float ImpactDamage { get => _physicsComponent.ImpactDamage; }
        public bool WrapsAroundScreen {  get => _wrapsAroundScreen; }
        public bool Dead { get => _dead; set => _dead = value; }
        public int PointsValue { get => _pointsValue; }
        public virtual void Update(GameTime gameTime) //overridable by inheriting classes
        {
            _physicsComponent.Update();
            _graphicsComponent.Update(_physicsComponent.Position, _physicsComponent.Rotation);
        } 
        public virtual void TakeDamage(float damage)
        {
            if (_physicsComponent.ApplyDamageAndCheckIfDestroyed(damage))
            {
                _dead = true;
            }
        }
        public virtual List<Entity> DoDeathAction() //overridable by inheriting classes
        {
            return new List<Entity>() { }; //when something dies, if can spawn things in its place (like asteroids, debris, corpses)
        } 
        public void SetVelocity(Vector2 v)
        {
            _physicsComponent.Velocity = v;
        }
        public void SetPosition(Vector2 position)
        {
            _physicsComponent.Position = position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_dead)
            {
                _graphicsComponent.Draw(spriteBatch);
            }
        }
        
    }

}

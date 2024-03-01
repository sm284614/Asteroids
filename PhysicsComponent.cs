using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class PhysicsComponent //the part of an entity related to its motion and physical presence
    {
        protected Vector2 _position;
        protected Vector2 _velocity;
        protected float _rotation;
        protected float _radius;
        protected float _speedCurrent;
        protected float _speedMaximum;
        protected float _healthCurrent;
        protected float _healthMaximum;
        protected float _mass;
        protected float _impactDamage;
        protected float _rotationSpeed;
        protected float _rotationSpeedMax;
        protected float _acceleration;
        protected float _rotationAcceleration;
        public PhysicsComponent(Vector2 position, float rotation, float mass, float speed, float rotationSpeed, float impactDamage, float health)
        {
            _position = position;
            _rotation = rotation;
            _mass = mass;
            _impactDamage = impactDamage;
            _speedCurrent = speed;
            _velocity = Vector2.Zero;
            _rotationSpeedMax = 0.1f; //these should be parameterised or loaded from a file
            _rotationAcceleration = 0.005f; //▲
            _acceleration = 0.1f; //▲
            _speedMaximum = 20; //▲
            _velocity.X += (float)Math.Sin(_rotation) * speed;
            _velocity.Y -= (float)Math.Cos(_rotation) * speed;
            _healthMaximum = health;
            _healthCurrent = health;
            _rotationSpeed = rotationSpeed;
        }
        public Vector2 Position { get => _position; set => _position = value; }
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }
        public float Rotation { get => _rotation; set => _rotation = value; }
        public float Radius { get => _radius; set => _radius = value; }
        public float Mass { get => _mass; }
        public float ImpactDamage { get => _impactDamage; }
        public float Health { get => _healthCurrent; }
        public bool ApplyDamageAndCheckIfDestroyed(float damage) //Entity has the "dead" bool and needs to know
        {
            _healthCurrent -= damage;
            return _healthCurrent <= 0;
        }
        public void Update() //basic update for anything on screen
        {
            _position += _velocity;
            _rotation += _rotationSpeed;
        }
        public void AccelerateForwards()
        {
            _velocity.X += (float)Math.Sin(_rotation) * _acceleration; //maths
            _velocity.Y -= (float)Math.Cos(_rotation) * _acceleration;
            _speedCurrent = (float)Math.Sqrt(_velocity.X * _velocity.X + _velocity.Y * _velocity.Y); //pythagoras to get speed from velocity
            if (_speedCurrent > _speedMaximum)
            {
                _velocity *= _speedMaximum / _speedCurrent; // enforce speed limit, should be done by stage
            }
        }
        public void MoveForwards(float amount) //used to directly move an object if we need to
        {
            _position.X += (float)Math.Sin(_rotation) * amount;
            _position.Y -= (float)Math.Cos(_rotation) * amount;
        }
        public void RotateClockwise()
        {
            if (_rotationSpeed < _rotationSpeedMax)
            {
                _rotationSpeed += _rotationAcceleration;
            }
        }
        public void RotateAntiClockwise()
        {
            if (_rotationSpeed < _rotationSpeedMax)
            {
                _rotationSpeed -= _rotationAcceleration;
            }
        }
        public void ReduceAngularMomentum()
        {
            _rotationSpeed *= 0.9f;
        } //used only for player
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class SpaceShip : Entity //inherits from entity, could be used for enemy spaceships
    {
        protected double _lastShotTime;
        protected double _shootingDelay;
        protected bool _shooting;
        public bool Shooting { get => _shooting; set => _shooting = value; }
        public SpaceShip(GraphicsComponent sprite, PhysicsComponent physical, bool wrapsAroundScreen) : base (sprite, physical, wrapsAroundScreen)
        {
            _shooting = false;
            _lastShotTime = -1;
        }
    }
}

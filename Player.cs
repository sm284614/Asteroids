using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public class Player : SpaceShip
    {
        private Vector2 _spawnPoint;
        private int _score;
        private Keys _accelerateKey;
        private Keys _rotateClockwiseKey;
        private Keys _rotateAnticlockwiseKey;
        private Keys _shootKey;
        public Player(GraphicsComponent sprite, PhysicsComponent physical) : base(sprite, physical, true)
        {
            _spawnPoint = new Vector2(_physicsComponent.Position.X, _physicsComponent.Position.Y);
            _accelerateKey = Keys.W;  //if we wanted more players, these should be passed as parameters to the constructor here
            _rotateAnticlockwiseKey = Keys.A;
            _rotateClockwiseKey = Keys.D;
            _shootKey = Keys.Space;
            _shootingDelay = 0.2;
        }
        public int Score { get => _score; set => _score = value; } //used in stage to display
        public float Health { get => _physicsComponent.Health; } //used in stage to display
        public override List<Entity> DoDeathAction()
        {
            return new List<Entity>(); //on death, don't spawn anything.
        }

        public Entity BlasterProjectile() //used in stage when stage sees player wants to shoot, return an projectile entity. pew pew!
        {
            GraphicsComponent sprite = new GraphicsComponent(Game1.ContentLoader.Load<Texture2D>(@"graphics\blaster"), Color.White);
            PhysicsComponent physical = new PhysicsComponent(_physicsComponent.Position, _physicsComponent.Rotation, 0.1f, 10, 0, 200, 1);
            physical.MoveForwards(_physicsComponent.Radius + 1); //move a little to avoid collision with player when spawned
            return new Entity(sprite, physical, false);
        }
        public override void Update(GameTime gameTime) //overrides Entity class update (polymorphic)
        {
            if (!_dead) //can't move if you're dead
            {
                if (Input.KeyDown(_rotateClockwiseKey))
                {
                    _physicsComponent.RotateClockwise();
                }
                if (Input.KeyDown(_rotateAnticlockwiseKey))
                {
                    _physicsComponent.RotateAntiClockwise();
                }
                if (Input.KeyDown(_shootKey))
                {
                    if (gameTime.TotalGameTime.TotalSeconds - _lastShotTime > _shootingDelay)
                    {
                        _shooting = true; //let stage know we want to shoot as we can't add entities to stage here
                        _lastShotTime = gameTime.TotalGameTime.TotalSeconds;
                    }
                }
                if (Input.KeyDown(_accelerateKey))
                {
                    _physicsComponent.AccelerateForwards(); //note that the player's physics component sets its own values
                }
                _physicsComponent.ReduceAngularMomentum();
                base.Update(gameTime); //call the base update for Entity to do usual move/rotate
            }            
        }

    }
}

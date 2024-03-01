using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class GraphicsComponent //the visual part of anything on the screen
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Color _colour;
        protected float _rotation;
        protected Vector2 _origin; //offset from texture top-left, used for rotation and scaling
        protected float _scale;
        protected SpriteEffects _facing; //can flip sprites left/right, top/bottom
        public GraphicsComponent(Texture2D texture, Color colour)
        {
            _texture = texture;
            _position = Vector2.Zero;
            _colour = colour;
            _rotation = 0;
            _scale = 1;
            _origin = new Vector2(texture.Width / 2, _texture.Height / 2);
            _facing = SpriteEffects.None;
        }
        public float Rotation { get => _rotation; }
        public float Radius { get => (_texture.Width + _texture.Height) * 0.25f * _scale; }
        public Rectangle BoundingBox { get => new Rectangle((int)_position.X - _texture.Width/2, (int)_position.Y - _texture.Height / 2, _texture.Width, _texture.Height); }
        public void Update(Vector2 position, float rotation) //these are sent from an Entity's physics component
        {
            _position = position;
            _rotation = rotation;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, _colour, _rotation, _origin, _scale, _facing, 1);
        }
    }
}

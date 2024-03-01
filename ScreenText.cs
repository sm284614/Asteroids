using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public class ScreenText //text we want to draw somewhere on the screen
    {
        private SpriteFont _font;
        private string _text;
        private Vector2 _position;
        private Color _colour;
        private bool _visible;
        public ScreenText(SpriteFont font, string startMessage, Vector2 messagePosition, Color colour, bool visible)
        {
            _font = font;
            _text = startMessage;
            _position = messagePosition;
            _colour = colour;
            _visible = visible;
        }
        public string Text { get => _text; set => _text = value; }
        public bool Visible { get => _visible; set => _visible = value; }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_visible)
            {
                spriteBatch.DrawString(_font, _text, _position, _colour);
            }
        }
    }
}

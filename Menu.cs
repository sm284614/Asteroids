using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Asteroids
{
    public class Menu
    {
        private Texture2D _backgroundTexture;
        private List<ScreenText> _screenText;
        private List<GraphicsComponent> _sprites; //not used here
        private Keys _gameStartKey;
        public Menu()
        {
            _gameStartKey = Keys.Space;
            _backgroundTexture = Game1.ContentLoader.Load<Texture2D>(@"graphics\title_screen"); //we could define all of this in a file if we wanted.
            _sprites = new List<GraphicsComponent>();
            _screenText = new List<ScreenText>();
            string startMessage = "Press " + _gameStartKey.ToString().ToUpper() + " to start";
            Vector2 stringSize = Game1.Font.MeasureString(startMessage); //find size of this text when drawn as a bitmap
            Vector2 messagePosition = new Vector2(Settings.Resolution.X * 0.5f, Settings.Resolution.Y * 0.75f) - stringSize / 2; //calculate mid-screen position
            _screenText.Add(new ScreenText(Game1.Font, startMessage, messagePosition, Color.White, true));
        }
        public void Update() //called in Game1.Update
        {
            if (Input.KeyPressedOnce(_gameStartKey)) 
            {
                Game1.GameState = Settings.GameState.Loading;
            }
            if (Input.KeyPressedOnce(Keys.Escape))
            {
                Game1.GameState = Settings.GameState.Exit;
            }
        }
        public void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            foreach (GraphicsComponent sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }
            foreach (ScreenText screenText in _screenText)
            {
                screenText.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}

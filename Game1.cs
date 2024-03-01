using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Asteroids
{
    public class Game1 : Game //inherits from base class in MonoGame framework
    {
        public static Settings.GameState GameState; //used to see what we're updating/drawing
        public static ContentManager ContentLoader; //used to load textures etc. when needed
        public static SpriteFont Font; //definition of font.
        public static GraphicsDeviceManager Graphics; //for graphics things        
        private ScreenText _errorMessage;
        private SpriteBatch spriteBatch; //draws bitmaps to screen
        private Stage current_stage; //our game level
        private Menu main_menu;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            _errorMessage = new ScreenText(Font, "", Vector2.Zero, Color.Red, false);
            Settings.Resolution = new Point(1280, 720); //set this in the static settings class
            Font = Content.Load<SpriteFont>("font"); //this needs to be loaded in contentmanager
            Graphics.PreferredBackBufferWidth = Settings.Resolution.X;
            Graphics.PreferredBackBufferHeight = Settings.Resolution.Y;
            IsMouseVisible = true;
            Graphics.ApplyChanges(); //set window size/options
            ContentLoader = Content;
            main_menu = new Menu(); //create the menu
            GameState = Settings.GameState.Menu; //set initial game state
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice); //instantiate
        }

        protected override void UnloadContent()
        {
            // not used (nothing to unload: we're not using that much memory and can fit all resources in RAM no problem)
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update(); //go check inputs (static class)
            switch (GameState)
            {
                case Settings.GameState.Menu: //starting state for game
                    {
                        main_menu.Update();
                        break;
                    }
                case Settings.GameState.Loading: //loading state set when space pressed on menu
                    {
                        try
                        {
                            current_stage = new Stage(gameTime); //load stage
                            GameState = Settings.GameState.Playing;
                        }
                        catch (System.Exception exception)
                        {
                            _errorMessage .Text= exception.Message + ":" + exception.InnerException.Message ;
                            _errorMessage.Visible = true;
                            GameState = Settings.GameState.Error;
                        }
                        break;
                    }
                case Settings.GameState.Playing: //playing state set when stage successfully loads
                    {
                        current_stage.Update(gameTime);
                        break;
                    }
                case Settings.GameState.Error: //set if stage fails to load
                    {
                        if (Input.KeyPressedOnce(Keys.Space))
                        {
                            _errorMessage.Visible = false;
                            GameState = Settings.GameState.Menu;
                        }
                        break;
                    }
                case Settings.GameState.Exit: //set if escape is pressed on menu
                    {
                        Exit();
                        break;
                    }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            switch (GameState)
            {
                case Settings.GameState.Menu:
                    {
                        main_menu.Draw(ref spriteBatch);
                        break;
                    }
                case Settings.GameState.Loading:
                    {
                        //no laoding screen image here, loading takes less than one frame
                        break;
                    }
                case Settings.GameState.Playing:
                    {
                        current_stage.Draw(spriteBatch);
                        break;
                    }
                case Settings.GameState.Error:
                    {
                        _errorMessage.Draw(spriteBatch);
                        GraphicsDevice.Clear(Color.Red);
                        break;
                    }
            }
            base.Draw(gameTime);
        }
    }
}

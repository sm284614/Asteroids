using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class Stage
    {
        private double _mostRecentAsteroidSpawnTime;
        private int _asteroidSpawnDelaySeconds;
        private int _asteroidsWaitingToSpawn;
        private Player _player;
        private ScreenText _scoreText;//we'd be better off with a list of these screentexts
        private ScreenText _healthText;
        private ScreenText _gameOverText;
        private Rectangle _screenArea; //bounds of the single-screen of this game
        private List<Entity> _entities; //all game objects go in this list
        private Texture2D _backgroundTexture;
        public Stage(GameTime gameTime)
        {
            _screenArea = new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y);
            _asteroidsWaitingToSpawn = 5;
            _mostRecentAsteroidSpawnTime = 0;
            _entities = new List<Entity>();
            _asteroidSpawnDelaySeconds = 10; //+1 asteroid every 10 seconds
            _backgroundTexture = Game1.ContentLoader.Load<Texture2D>(@"graphics\stars");
            _scoreText = new ScreenText(Game1.Font, "Score: 0000000", new Vector2(Settings.Resolution.X - Game1.Font.MeasureString("Score: 0000000").X - 20, 20), Color.White, true);
            _healthText = new ScreenText(Game1.Font, "Health: 0000", new Vector2(20, 20), Color.White, true);
            _gameOverText = new ScreenText(Game1.Font, "GAME OVER", new Vector2(Settings.Resolution.X / 2 - Game1.Font.MeasureString("GAME OVER").X / 2, Settings.Resolution.Y / 2 - Game1.Font.MeasureString("GAME OVER").Y / 2), Color.Red, false);
            SpawnPlayerShip();
            SpawnNewBigAsteroid(_asteroidsWaitingToSpawn, gameTime);
        }
        private void SpawnPlayerShip()
        {
            _player = new Player(new GraphicsComponent(Game1.ContentLoader.Load<Texture2D>(@"graphics\ship"), Color.White), new PhysicsComponent(_screenArea.Center.ToVector2(), 0, 200, 0, 0, 1, 500)); ;
            _entities.Add(_player);
        }
        public void Update(GameTime gameTime)
        {
            CheckAsteroidSpawnTime(gameTime);
            UpdateEntities(gameTime);
            UpdatePlayer(gameTime);
            UpdateInterface();
        }
        private void CheckAsteroidSpawnTime(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - _mostRecentAsteroidSpawnTime > _asteroidSpawnDelaySeconds)
            {
                _asteroidsWaitingToSpawn++;
            }
            if (_asteroidsWaitingToSpawn > 0)
            {
                SpawnNewBigAsteroid(_asteroidsWaitingToSpawn, gameTime);
            }
        }
        private void UpdateEntities(GameTime gameTime)
        {
            for (int i = _entities.Count - 1; i >= 0; i--) //check all entities, looping BACKWARDS as we might remove some.
            {
                _entities[i].Update(gameTime); //update (i.e. move)
                for (int j = i - 1; j >= 0; j--) //check all other entities (note the start at i-1, so we don't double-check anything)
                {
                    if (Physics.RadiusCollisionBetween(_entities[i], _entities[j])) //check collision
                    {
                        Physics.CollisionBounce(gameTime, _entities[i], _entities[j]); //boing!
                        _entities[i].Update(gameTime); //extra update for these two to bounce away from each other
                        _entities[j].Update(gameTime); //extra update for these two to bounce away from each other
                        if (_entities[i].GetType() != _entities[j].GetType()) //objects of the same type won't damage each other.
                        {
                            _entities[i].TakeDamage(_entities[j].ImpactDamage);
                            _entities[j].TakeDamage(_entities[i].ImpactDamage);
                            if (_entities[i].Dead || _entities[j].Dead) //if either entity is now dead, exit the for loop for this entity
                            {
                                break;
                            }
                        }
                    }
                }
                if (IsOffScreen(_entities[i]))
                {
                    if (_entities[i].WrapsAroundScreen)
                    {
                        WrapAroundScreen(_entities[i]);
                    }
                    else //projectiles die when moving off screen
                    {
                        _entities[i].Dead = true;

                    }
                }
                if (_entities[i].Dead) //if we try this when looping forwards, we'll likely get a crash
                {
                    RemoveExpiredEntity(_entities[i]);
                }
            }
        }
        private void UpdatePlayer(GameTime gameTime)
        {
            //we could be cleverer and move this into Entity and use more polymorphism
            if (_player != null && !_player.Dead)
            {
                if (_player.Shooting)
                {
                    _entities.Add(_player.BlasterProjectile());
                    _player.Shooting = false;
                }
            }
            else
            {
                _gameOverText.Visible = true;
            }
        }
        private void UpdateInterface()
        {
            if (Input.KeyPressedOnce(Keys.Escape))
            {
                Game1.GameState = Settings.GameState.Menu;
            }
            _scoreText.Text = "Score: " + _player.Score.ToString("D7");
            _healthText.Text = "Health: " + _player.Health.ToString("000");
        }
        private void SpawnNewBigAsteroid(int numberToSpawn, GameTime gameTime)
        {
            Vector2 edge_position;
            Asteroid asteroid;
            bool clear_position_to_spawn;
            for (int i = 0; i < numberToSpawn; i++) // how many?
            {
                clear_position_to_spawn = true;
                edge_position = RandomScreenEdgePosition(); //random edge position (means half the asteroid will just appear on screen)
                Array enumValues = Enum.GetValues(typeof(Asteroid.MaterialType)); //get all materials
                Asteroid.MaterialType randomMaterial = (Asteroid.MaterialType)enumValues.GetValue(Random.Shared.Next(enumValues.Length)); //get random enum;
                asteroid = Asteroid.CreateAsteroid(edge_position, Physics.RandomRotation(), randomMaterial, Asteroid.RockSize.Large, 4); //use static method in asteroid class to make a new one
                for (int j = 0; j < _entities.Count; j++) //check other entities
                {
                    if (Physics.RadiusCollisionBetween(_entities[j], asteroid)) //if this position would intersect any other object on screen
                    {
                        clear_position_to_spawn = false; //don't spawn it
                        break;
                    }
                }
                if (clear_position_to_spawn) //otherwise, do spawn it
                {
                    _entities.Add(asteroid);
                    _asteroidsWaitingToSpawn--;
                    _mostRecentAsteroidSpawnTime = gameTime.TotalGameTime.TotalSeconds;
                }
            }
        }
        private Vector2 RandomScreenEdgePosition()
        {
            float x, y;
            int n;
            n = Random.Shared.Next(4);
            if (n == 1) //top screen edge
            {
                x = Random.Shared.Next(_screenArea.Width);
                y = _screenArea.Top;
            }
            else if (n == 2) //right screen edge
            {
                x = _screenArea.Right;
                y = Random.Shared.Next(_screenArea.Height);
            }
            else if (n == 3) //bottom screen edge
            {
                x = Random.Shared.Next(_screenArea.Width);
                y = _screenArea.Bottom;
            }
            else //left screen edge
            {
                x = _screenArea.Left;
                y = Random.Shared.Next(_screenArea.Height);
            }
            return new Vector2(x, y);
        }
        private bool IsOffScreen(Entity entity)
        {
            return !entity.BoundingBox.Intersects(_screenArea); //intersects is a method of the Rectangle class, can also check if Point/Vector2 is in Rectangle
        }
        private void WrapAroundScreen(Entity entity)
        {
            Rectangle boundingBox = entity.BoundingBox;
            if (boundingBox.Right <= _screenArea.Left) //bounding box moved past left edge
            {
                entity.SetPosition(new Vector2(_screenArea.Right + boundingBox.Width / 2 - 1, entity.Position.Y));
            }
            if (boundingBox.Left >= _screenArea.Right) //bounding box moved past right edge
            {
                entity.SetPosition(new Vector2(_screenArea.Left - boundingBox.Width / 1 + 1, entity.Position.Y));
            }
            if (boundingBox.Bottom <= _screenArea.Top) //bounding box move past top edge
            {
                entity.SetPosition(new Vector2(entity.Position.X, _screenArea.Bottom + boundingBox.Height / 2 - 1));
            }
            if (boundingBox.Top >= _screenArea.Bottom) //bounding box move past bottom edge
            {
                entity.SetPosition(new Vector2(entity.Position.X, _screenArea.Top - boundingBox.Height / 2 + 1));
            }
        }
        private void RemoveExpiredEntity(Entity entity)
        {
            _player.Score += entity.PointsValue; //player gets point for anything destoryed
            _entities.AddRange(entity.DoDeathAction()); //this is messy: really, we should use delegates/actions in each class type
            _entities.Remove(entity); //note that the player still exists in the _player object if removed from the entity list
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            foreach (Entity entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
            _scoreText.Draw(spriteBatch);
            _healthText.Draw(spriteBatch);
            _gameOverText.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}

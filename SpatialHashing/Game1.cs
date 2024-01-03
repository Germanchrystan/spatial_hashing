using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StatialHashing;

namespace SpatialHashing
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<GameObject> bubbles = new List<GameObject>(Constants.BUBBLE_AMOUNT);
        private SpriteFont gameFont;

        private CollisionCheckerIteratorStrategy collisionCheckerIterator;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Constants.WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = Constants.WINDOW_HEIGHT;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameFont = Content.Load<SpriteFont>("galleryFont");

            Texture2D bubbleTexture = new Texture2D(GraphicsDevice, 1,1);
            bubbleTexture.SetData<Color>(new Color[] { Color.Black });
            
            collisionCheckerIterator = new SpatialHashingCollisionCheckerIterator(16);
            // collisionCheckerIterator = new NormalCollisionCheckerIterator();

            Random rnd = new Random();
            for(int i = 0; i < Constants.BUBBLE_AMOUNT; i++)
            {
                int randomX = rnd.Next(0, Constants.WINDOW_WIDTH - (int)Constants.RADIUS * 2);
                int randomY = rnd.Next(0, Constants.WINDOW_HEIGHT - (int)Constants.RADIUS * 2);
                int randomXDirection = rnd.Next(-1, 2);
                int randomYDirection = randomXDirection != 0 ? rnd.Next(-1, 2) : 1; 

                bubbles.Add(new Bubble(Constants.RADIUS, new Vector2(randomX, randomY), new Vector2(randomXDirection, randomYDirection), bubbleTexture));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            foreach(var bubble in bubbles)
            {
                bubble.Update(gameTime);
            }

            collisionCheckerIterator.UpdateCollisions((List<GameObject>)bubbles);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach(var bubble in bubbles)
            {
                bubble.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(gameFont, "Collision checks: " + collisionCheckerIterator.CollisionCheckCounter, new Vector2(3, 20), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
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

        private List<Bubble> bubbles = new List<Bubble>(Constants.BUBBLE_AMOUNT);
        private SpriteFont gameFont;
        private int collisionCheckingCounter = 0;
        private bool firstFrame = true;

        private SpatialHashingManager spatialHashingManager;

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
            
            spatialHashingManager = new SpatialHashingManager(16);

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

            // Register Bubbles
            foreach(Bubble bubble in bubbles)
            {
                spatialHashingManager.RegisterBubble(bubble);
            }
            
            // Collision checking
            for(int i = 0; i < Constants.BUBBLE_AMOUNT; i++)
            {
                if(firstFrame) collisionCheckingCounter++;
                // Normal collision check
                // for(int j = i+1; j < Constants.BUBBLE_AMOUNT; j++)
                // {
                //     if(firstFrame) collisionCheckingCounter++;
                //     CollisionChecker.CheckCollision(bubbles[i], bubbles[j]);
                // }
                
                // Spatial Hashing check
                List<Bubble> nearbyBubbles = spatialHashingManager.GetNearby(bubbles[i]);
                foreach(Bubble nearbyBubble in nearbyBubbles)
                {
                    if(firstFrame) collisionCheckingCounter++;
                    if (bubbles[i] != nearbyBubble) CollisionChecker.CheckCollision(bubbles[i], nearbyBubble);
                }
            }

            foreach(var bubble in bubbles)
            {
                bubble.Update(gameTime);
            }

            firstFrame = false;
            spatialHashingManager.ClearBuckets();
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
            _spriteBatch.DrawString(gameFont, "Collision checks: " + collisionCheckingCounter, new Vector2(3, 40), Color.White);
            // _spriteBatch.DrawString(gameFont, "")
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
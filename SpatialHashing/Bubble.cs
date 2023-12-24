using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpatialHashing
{
    public class Bubble
    {
        private float radius;
        private Vector2 position;
        private Vector2 direction;
        private float speed = 120f;
        private Texture2D texture;
        private Rectangle rect;


        public Bubble(float radius, Vector2 position, Vector2 direction, Texture2D texture)
        {
            this.radius = radius;
            this.position = position;
            this.direction = direction;
            this.texture = texture;
            this.rect = new Rectangle((int)position.X, (int)position.Y, (int)radius * 2, (int)radius * 2);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += direction.X * speed * dt;
            position.Y += direction.Y * speed * dt;
            
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
            rect.Width = (int)radius * 2;
            rect.Height = (int)radius * 2;

            if (position.X < 0 || position.X + (radius * 2) > Constants.WINDOW_WIDTH )
            {
                SwitchXPosition();
            }

            if(position.Y < 0 || position.Y + (radius * 2) > Constants.WINDOW_HEIGHT)
            {
                SwitchYPosition();
            }
        }

        public void SwitchXPosition()
        {
            direction.X *= -1;
        }
        public void SwitchYPosition()
        {
            direction.Y *= -1;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.Black);
        }
    }
}

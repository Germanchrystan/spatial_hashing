using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StatialHashing
{
  public interface GameObject
  {
    public Rectangle Rect { get { return Rect ;}}
    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch);
    public void OnCollision();
  }
}
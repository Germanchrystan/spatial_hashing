using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StatialHashing
{
  public interface GameObject
  {
    public void Update(GameTime gameTime);
    public void OnCollision();
    public Rectangle Rect { get { return Rect ;}}
  }
}
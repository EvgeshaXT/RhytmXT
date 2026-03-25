using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

public class Cursor
{
    Texture2D _texture;
    MouseState lastMouseState;
    Vector2 _position;
    Vector2 _origin;

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("cursor");
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
    }

    public void Update()
    {
        MouseState currentMouseState = Mouse.GetState();

        if (currentMouseState.X != lastMouseState.X ||
            currentMouseState.Y != lastMouseState.Y)
            _position = new Vector2(currentMouseState.X, currentMouseState.Y);

        lastMouseState = currentMouseState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _position, null, Color.White, 0, _origin, 1f, SpriteEffects.None, 0f);
    }
}
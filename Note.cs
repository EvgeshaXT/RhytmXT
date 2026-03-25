using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class Note
{
    Texture2D _texture;
    Vector2 _position, _origin;
    int _type, _hitSound;
    float _speed;
    bool _isActive;
    public int Timing { get; private set; }
    public float StartTime { get; private set; }

    public Note(int timing, int type, int hitSound, float speed, float hitPositionX, int screenWidth, Texture2D texture)
    {
        Timing = timing;
        _type = type;
        _hitSound = hitSound;
        _speed = speed;
        _texture = texture;

        _position = new Vector2(screenWidth + texture.Width / 2, 410);
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        _isActive = false;

        float traveilTimeInSeconds = (screenWidth - hitPositionX) / _speed;
        StartTime = timing - (traveilTimeInSeconds * 1000f);
    }

    public void Update(GameTime gameTime)
    {
        if (!_isActive) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _position.X -= _speed * deltaTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Color color;
        if (_hitSound == 8)
            color = Color.Blue;
        else color = Color.Red;

        spriteBatch.Draw(_texture, _position, null, color, 0, _origin, 1f, SpriteEffects.None, 0f);
    }

    public void Activate()
    {
        if (!_isActive) _isActive = true;
    }

    public bool IsOffScreen() => _position.X + _texture.Width < 0;
}
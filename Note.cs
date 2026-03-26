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
    public double Timing { get; private set; }

    public Note(double timing, int type, int hitSound, float speed, Vector2 position, Texture2D texture)
    {
        Timing = timing;
        _type = type;
        _hitSound = hitSound;
        _speed = speed;
        _texture = texture;
        _position = position;

        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        _isActive = false;
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

    // public bool IsOffScreen(float _hitPositionX) => _position.X + _texture.Width / 2 < _hitPositionX;
}
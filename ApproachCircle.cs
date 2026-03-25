using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class ApproachCircle
{
    Texture2D _texture;
    Vector2 _origin;

    Texture2D _circle;
    int _radius, _diameter;
    Vector2 _circlePosition;
    float _rectangleDim;

    public ApproachCircle(GraphicsDevice graphicsDevice, Vector2 circlePosition, float rectangleDim)
    {
        _radius = 48;
        _diameter = _radius * 2;

        _circlePosition = circlePosition;
        _circlePosition.X += 16;
        _circlePosition.Y += 16;

        _rectangleDim = rectangleDim;

        CreateCircle(graphicsDevice);
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("approachcircle");
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(_texture, position, null, Color.White, 0, _origin, 1f, SpriteEffects.None, 0f);
        spriteBatch.Draw(_circle, _circlePosition, null, Color.Gray * _rectangleDim * _rectangleDim, 0, _origin, 1f, SpriteEffects.None, 0);
    }

    void CreateCircle(GraphicsDevice graphicsDevice)
    {
        _circle = new Texture2D(graphicsDevice, _diameter, _diameter);
        Color[] data = new Color[_diameter * _diameter];
        
        for (int y = 0; y < _diameter; y++)
        {
            for (int x = 0; x < _diameter; x++)
            {
                int dx = x - _radius;
                int dy = y - _radius;

                float distance = (float)Math.Sqrt(dx*dx + dy*dy);
                int index = x + y*_diameter;

                if (distance <= _radius) data[index] = Color.White;
                else data[index] = Color.Transparent;
            }
        }

        _circle.SetData(data);
    }
}
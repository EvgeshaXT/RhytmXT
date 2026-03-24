using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace RhytmXT;

public class GameField
{
    Texture2D _background;
    Texture2D _rectangle;
    Song song;

    int _screenWidth;

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _background = content.Load<Texture2D>("GameField/image");

        _rectangle = new Texture2D(graphicsDevice, 1, 1);
        _rectangle.SetData([Color.White]);

        song = content.Load<Song>("GameField/audio");
        MediaPlayer.Play(song);

        _screenWidth = graphicsDevice.Viewport.Width;
    }

    public void Draw(SpriteBatch spriteBatch, float backgroundDim, float rectangleDim)
    {
        spriteBatch.Draw(_background, new Vector2(0, 0), Color.White * (1 - backgroundDim));


        spriteBatch.Draw(_rectangle, new Rectangle(0, 300, _screenWidth, 220), Color.Black * rectangleDim);
    }
}
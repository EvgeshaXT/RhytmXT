using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace RhytmXT;

public class GameField
{
    Texture2D _background, _rectangle, _tact;
    Song song;
    string _map;
    readonly int _screenWidth;

    Vector2 _tactPosition;
    float _tactSpeed, _bpm, _offset, _currentDelay;
    bool _tactMoving;

    public GameField(int screenWidth)
    {
        _map = "macaroom - akuma.xt";

        _screenWidth = screenWidth;

        _tactPosition = new Vector2(_screenWidth / 2, 300);

        _bpm = GetBPM();
        _tactSpeed = GetTactSpeed(_bpm);
        _offset = GetOffset();

        _currentDelay = _offset / 1000;
        _tactMoving = false;
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _background = GetBackground(content);
        _rectangle = GetRectangle(graphicsDevice);
        _tact = GetRectangle(graphicsDevice);

        song = GetSong(content);
        MediaPlayer.Play(song);
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!_tactMoving)
        {
            _currentDelay -= deltaTime;

            if (_currentDelay <= deltaTime)
            {
                _tactMoving = true;
            }
            return;
        }

        _tactPosition.X -= _tactSpeed * deltaTime;

        if (_tactPosition.X < 0) _tactPosition.X = _screenWidth;
    }

    public void Draw(SpriteBatch spriteBatch, float backgroundDim, float rectangleDim)
    {
        // Background
        spriteBatch.Draw(_background, new Vector2(0, 0), Color.White * (1 - backgroundDim));
        // Rectangle
        spriteBatch.Draw(_rectangle, new Rectangle(0, 300, _screenWidth, 220), Color.Black * rectangleDim);
        // Tact
        if (_tactMoving)
            spriteBatch.Draw(_tact, new Rectangle((int)_tactPosition.X, (int)_tactPosition.Y, 1, 220), Color.White);
    }

    float GetTactSpeed(float bpm)
    {
        float secondsPerTact = 60f / bpm * 4;
        return _screenWidth / secondsPerTact;
    }

    Song GetSong(ContentManager content)
    {
        string mapXt = Path.Combine(Directory.GetCurrentDirectory(), "Content", "GameField", _map);
        string[] lines = File.ReadAllLines(mapXt);
        string audioName = lines[3][15..].Split('.')[0];

        return content.Load<Song>($"GameField/{audioName}");
    }

    Texture2D GetBackground(ContentManager content)
    {
        string mapXt = Path.Combine(Directory.GetCurrentDirectory(), "Content", "GameField", _map);
        string[] lines = File.ReadAllLines(mapXt);
        string backgroundName = lines[4][20..].Split('.')[0];

        return content.Load<Texture2D>($"GameField/{backgroundName}");
    }

    Texture2D GetRectangle(GraphicsDevice graphicsDevice)
    {
        _rectangle = new Texture2D(graphicsDevice, 1, 1);
        _rectangle.SetData([Color.White]);

        return _rectangle;
    }

    float GetBPM()
    {
        string mapXt = Path.Combine(Directory.GetCurrentDirectory(), "Content", "GameField", _map);
        string[] lines = File.ReadAllLines(mapXt);
        string[] timings = lines[7].Split(',');

        float bpm = float.Parse(timings[1]);

        return bpm;
    }

    float GetOffset()
    {
        string mapXt = Path.Combine(Directory.GetCurrentDirectory(), "Content", "GameField", _map);
        string[] lines = File.ReadAllLines(mapXt);
        string[] timings = lines[7].Split(',');

        float offset = float.Parse(timings[0]);

        return offset;
    }
}
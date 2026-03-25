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
    float _tactSpeed, _bpm, _offset;
    bool _tactMoving, _songStarted;

    string _audioName, _backgroundName;

    public GameField(int screenWidth)
    {
        _map = "macaroom - akuma.xt";
        _screenWidth = screenWidth;

        // Load _audioName, _backgroundName, _bpm, _offset
        LoadMapData();

        _tactPosition = new Vector2(_screenWidth / 2, 300);
        _tactSpeed = GetTactSpeed(_bpm);

        _tactMoving = false;
        _songStarted = false;
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _background = content.Load<Texture2D>($"GameField/{_backgroundName}");
        _rectangle = GetRectangle(graphicsDevice);
        _tact = GetRectangle(graphicsDevice);

        song = content.Load<Song>($"GameField/{_audioName}");
    }

    public void Update(GameTime gameTime)
    {
        if (!_songStarted)
        {
            MediaPlayer.Play(song);
            _songStarted = true;
            return;
        }

        if (MediaPlayer.State != MediaState.Playing)
            return;

        double currentSongTime = MediaPlayer.PlayPosition.TotalMilliseconds;

        if (!_tactMoving)
        {
            if (currentSongTime >= _offset)
            {
                _tactMoving = true;
            }
            return;
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
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

    void LoadMapData()
    {
        string mapXt = Path.Combine(Directory.GetCurrentDirectory(), "Content", "GameField", _map);
        string[] lines = File.ReadAllLines(mapXt);

        _audioName = lines[3][15..].Split('.')[0];
        _backgroundName = lines[4][20..].Split('.')[0];

        string[] timings = lines[7].Split(',');
        _offset = float.Parse(timings[0]);
        _bpm = float.Parse(timings[1]);
    }

    Texture2D GetRectangle(GraphicsDevice graphicsDevice)
    {
        _rectangle = new Texture2D(graphicsDevice, 1, 1);
        _rectangle.SetData([Color.White]);

        return _rectangle;
    }
}
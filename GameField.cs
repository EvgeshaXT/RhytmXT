using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace RhytmXT;

public class GameField
{
    Texture2D _background, _rectangle;
    Song song;
    string _map;
    readonly int _screenWidth;

    Vector2 _tactPosition;
    float _tactStartPositionX, _tactSpeed, _bpm;
    int _offset;
    bool _tactMoving, _songStarted;

    string _audioName, _backgroundName;
    string _mapPath;

    ApproachCircle _approachCircle;
    Vector2 _approachCirclePosition;
    public float _rectangleDim;

    Notes _notes;

    public GameField(int screenWidth, float rectangleDim)
    {
        _map = "macaroom - akuma.xt";
        _screenWidth = screenWidth;
        _rectangleDim = rectangleDim;

        _tactStartPositionX = 350;
        _tactPosition = new Vector2(_tactStartPositionX, 300);

        _tactMoving = false;
        _songStarted = false;

        _approachCirclePosition = new Vector2(350, 410);
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
    {
        // Load _audioName, _backgroundName, _bpm, _offset
        LoadMapData();
        _tactSpeed = GetTactSpeed();

        _notes = new Notes(_screenWidth, _tactSpeed, _approachCirclePosition.X);

        _background = content.Load<Texture2D>($"GameField/{_backgroundName}");
        _rectangle = GetRectangle(graphicsDevice);

        song = content.Load<Song>($"GameField/{_audioName}");

        _approachCircle = new ApproachCircle(graphicsDevice, _approachCirclePosition, _rectangleDim);
        _approachCircle.LoadContent(content);

        _notes.LoadContent(content);
        _notes.LoadNotesFromFile(_mapPath);
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
                _tactPosition.X = _tactStartPositionX;
            }
            return;
        }

        _tactPosition.X = GetTactPosition(currentSongTime);

        _notes.Update(currentSongTime);
    }

    public void Draw(SpriteBatch spriteBatch, float backgroundDim)
    {
        // Background
        spriteBatch.Draw(_background, new Vector2(0, 0), Color.White * (1 - backgroundDim));
        // Rectangle
        spriteBatch.Draw(_rectangle, new Rectangle(0, 300, _screenWidth, 220), Color.Black * _rectangleDim);
        // Tact
        if (_tactMoving)
            spriteBatch.Draw(_rectangle, new Rectangle((int)_tactPosition.X, (int)_tactPosition.Y, 1, 220), Color.White);
        _approachCircle.Draw(spriteBatch, _approachCirclePosition);

        _notes.Draw(spriteBatch);
    }

    float GetTactSpeed()
    {
        float secondsPerTact = 60f / _bpm * 4;
        return _screenWidth / secondsPerTact;
    }

    float GetTactPosition(double currentSongTime)
    {
        if (!_tactMoving) return _tactPosition.X;

        double elapsedTime = (currentSongTime - _offset) / 1000;
        float distance = _tactSpeed * (float)Math.Max(0, elapsedTime);
        float positionX = _tactStartPositionX - distance;

        while (positionX < 0) positionX += _screenWidth;

        return positionX;
    }

    void LoadMapData()
    {
        _mapPath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "GameField", _map);
        string[] lines = File.ReadAllLines(_mapPath);

        foreach (string line in lines)
        {
            if (line.StartsWith("AudioFilename:")) _audioName = line[15..].Split('.')[0];
            else if (line.StartsWith("BackgroundFilename:")) _backgroundName = line[20..].Split('.')[0];

            else if (line.StartsWith("[Timings]"))
            {
                string timingsLine = lines[Array.IndexOf(lines, line) + 1];

                string[] timings = timingsLine.Split(',');
                _offset = int.Parse(timings[0]);
                _bpm = float.Parse(timings[1]);
            }
        }
    }

    Texture2D GetRectangle(GraphicsDevice graphicsDevice)
    {
        _rectangle = new Texture2D(graphicsDevice, 1, 1);
        _rectangle.SetData([Color.White]);

        return _rectangle;
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class Note
{
    Texture2D _texture;
    Texture2D _textureOverlay;
    Vector2 _position, _origin;
    int _type;
    Color _color;
    public int StartTime { get; set; }
    float _speed, _startPositionX, _scale;
    bool _isActive;

    public Note(Texture2D texture, Texture2D textureOverlay, Vector2 startPosition, Vector2 origin, int type, int hitsound, int startTime, float tactSpeed)
    {
        _texture = texture;
        _textureOverlay = textureOverlay;
        _position = startPosition;
        _startPositionX = startPosition.X;

        _origin = origin;
        
        _type = type;

        if (hitsound == 0) 
        {
            _color = Color.Red;
            _scale = 1f;
        }
        else if (hitsound == 4)
        {
            _color = Color.Red;
            _scale = 1.25f;
        }
        else if (hitsound == 8)
        {
            _color = Color.DodgerBlue;
            _scale = 1f;
        }
        else
        {
            _color = Color.DodgerBlue;
            _scale = 1.25f;
        }

        StartTime = startTime;
        _speed = tactSpeed;

        _isActive = false;
    }

    public void Update(double currentSongTime)
    {
        if (!_isActive) return;
        
        double elapsedTime = (currentSongTime - StartTime) / 1000f;
        float distance = _speed * (float)Math.Max(0, elapsedTime);
        _position.X = _startPositionX - distance;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_isActive) return;
        
        spriteBatch.Draw(_texture, _position, null, _color, 0, _origin, _scale, SpriteEffects.None, 0f);
        if (_textureOverlay != null)
            spriteBatch.Draw(_textureOverlay, _position, null, Color.White, 0, _origin, _scale, SpriteEffects.None, 0f);
    }

    public void Activate()
    {
        _isActive = true;
    }

    public float GetPosition() => _position.X;
}

public class Notes
{
    List<Note> _notes;
    Texture2D _texture;
    Texture2D _textureOverlay;
    Vector2 _startPosition, _origin;
    int _screenWidth, _timing, _type, _hitSound, _startTime, _nextNoteIndex, _localOffset;
    float _tactSpeed, _approachCirclePositionX;

    public Notes(int screenWidth, float tactSpeed, float approachCirclePositionX, int localOffset)
    {
        _notes = [];
        
        _screenWidth = screenWidth;
        _nextNoteIndex = 0;
        _localOffset = localOffset;

        _tactSpeed = tactSpeed;
        _approachCirclePositionX = approachCirclePositionX;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("taikohitcircle");
        try
        {
            _textureOverlay = content.Load<Texture2D>("taikohitcircleoverlay");
        }
        catch
        {
            _textureOverlay = null;
        }

        _startPosition = new Vector2(_screenWidth + _texture.Width / 2, 410);
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
    }

    public void LoadNotesFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        bool isNotesSection = false;

        foreach (string line in lines)
        {
            if (line.StartsWith("[Notes]"))
            {
                isNotesSection = true;
                continue;
            }

            if (isNotesSection)
            {
                if (string.IsNullOrWhiteSpace(line)) break;

                string[] parts = line.Split(',');

                _timing = int.Parse(parts[0]) + _localOffset;
                _startTime = SetStartTime();
                _type = int.Parse(parts[1]);
                _hitSound = int.Parse(parts[2]);
                
                Note note = new(_texture, _textureOverlay, _startPosition, _origin, _type, _hitSound, _startTime, _tactSpeed);
                _notes.Add(note);
            }
        }
    }

    public void Update(double currentSongTime)
    {
        while (_nextNoteIndex < _notes.Count && currentSongTime >= _notes[_nextNoteIndex].StartTime)
        {
            _notes[_nextNoteIndex].Activate();
            _nextNoteIndex++;
        }

        while (_notes.Count > 0 && _notes[0].GetPosition() + _texture.Width / 2 < 0)
        {
            _notes.RemoveAt(0);
            _nextNoteIndex--;
        }

        foreach (Note note in _notes)
        {
            note.Update(currentSongTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Note note in _notes)
        {
            note.Draw(spriteBatch);
        }
    }

    int SetStartTime()
    {
        float distance = _screenWidth + _texture.Width / 2 - _approachCirclePositionX;
        float time = distance / _tactSpeed * 1000f;

        return _timing - (int)Math.Round(time);
    }
}
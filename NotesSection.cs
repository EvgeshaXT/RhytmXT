using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class Note
{
    Texture2D _texture;
    Vector2 _position, _origin;
    int _type;
    Color _color;
    public int StartTime { get; set; }
    float _speed;
    bool _isActive;

    public Note(Texture2D texture, Vector2 startPosition, Vector2 origin, int type, int hitsound, int startTime, float tactSpeed)
    {
        _texture = texture;
        _position = startPosition;
        _origin = origin;
        
        _type = type;

        if (hitsound == 8) _color = Color.DodgerBlue;
        else _color = Color.Red;

        StartTime = startTime;
        _speed = tactSpeed;

        _isActive = false;
    }

    public void Update(float deltaTime)
    {
        if (!_isActive) return;
        
        _position.X -= _speed * deltaTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_isActive) return;
        
        spriteBatch.Draw(_texture, _position, null, _color, 0, _origin, 1f, SpriteEffects.None, 0f);
    }

    public void Activate()
    {
        _isActive = true;
    }
}

public class Notes
{
    List<Note> _notes;
    Texture2D _texture;
    Vector2 _startPosition, _origin;
    int _screenWidth, _timing, _type, _hitSound, _startTime, _nextNoteIndex;
    float _tactSpeed;

    public Notes(int screenWidth, float tactSpeed)
    {
        _notes = [];
        
        _screenWidth = screenWidth;
        _nextNoteIndex = 0;

        _tactSpeed = tactSpeed;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("taikohitcircle");
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

                _timing = int.Parse(parts[0]);
                _startTime = SetStartTime();
                _type = int.Parse(parts[1]);
                _hitSound = int.Parse(parts[2]);

                Note note = new(_texture, _startPosition, _origin, _type, _hitSound, _startTime, _tactSpeed);
                _notes.Add(note);
            }
        }
    }

    public void Update(float deltaTime, double currentSongTime)
    {
        while (_nextNoteIndex < _notes.Count && currentSongTime >= _notes[_nextNoteIndex].StartTime)
        {
            _notes[_nextNoteIndex].Activate();
            _nextNoteIndex++;
        }

        foreach (Note note in _notes)
        {
            note.Update(deltaTime);
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
        // Only for "macaroom - akuma.xt"!
        int startTime = _timing - 1570;
        return startTime;
    }
}
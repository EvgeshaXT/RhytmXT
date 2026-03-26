using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhytmXT;

public class Notes
{
    Texture2D _texture;
    List<Note> _notes;
    int _screenWidth;
    float _speed, _hitPositionX;
    double _notesOffset;
    int _nextNoteIndex;
    Vector2 _startPosition;

    public Notes(int screenWidth, float tactSpeed, float hitPositionX)
    {
        _screenWidth = screenWidth;
        _speed = tactSpeed;
        _hitPositionX = hitPositionX;

        _notes = [];
        _nextNoteIndex = 0;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("taikohitcircle");

        _startPosition = new Vector2(_screenWidth + _texture.Width / 2, 410);
        CalculateNotesOffset(_startPosition.X);
    }

    public void Update(GameTime gameTime, double currentSongTime)
    {
        while (_nextNoteIndex < _notes.Count && currentSongTime >= _notes[_nextNoteIndex].Timing)
        {
            _notes[_nextNoteIndex].Activate();
            _nextNoteIndex++;
        }

        foreach (Note note in _notes)
        {
            note.Update(gameTime);
        }

        // _notes.RemoveAll(n => n.IsOffScreen(_hitPositionX));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Note note in _notes)
        {
            note.Draw(spriteBatch);
        }
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

                double timing = int.Parse(parts[0]) - _notesOffset;
                int type = int.Parse(parts[1]);
                int hitSound = int.Parse(parts[2]);

                Note note = new(timing, type, hitSound, _speed, _startPosition, _texture);
                _notes.Add(note);
            }
        }
    }

    void CalculateNotesOffset(float noteStartPositionX)
    {
        _notesOffset = (noteStartPositionX - _hitPositionX) / _speed * 1000f;
    }
}
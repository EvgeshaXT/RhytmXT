using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RhytmXT;

public class Drum
{
    KeyboardState previousKeyboardState;
    KeyboardState keyboardState;
    HashSet<Keys> _processedPresses;

    SoundEffect _donEffect;
    SoundEffect _katEffect;

    Texture2D _innerTexture;
    Texture2D _outerTexture;

    int _keysCount;
    List<KeyBinding> _donKeys;
    List<KeyBinding> _katKeys;
    Dictionary<string, string> _currentKeyBinds;

    public Drum(Settings settings)
    {
        _processedPresses = [];

        _keysCount = settings.keysCount;
        _donKeys = [];
        _katKeys = [];

        _currentKeyBinds = settings.keyBinds[$"{_keysCount}keys"];
        LoadKeyBinds();
    }

    public void LoadContent(ContentManager content)
    {
        _donEffect = content.Load<SoundEffect>("taiko-normal-hitnormal");
        _katEffect = content.Load<SoundEffect>("taiko-normal-hitclap");

        _innerTexture = content.Load<Texture2D>("taiko-drum-inner");
        _outerTexture = content.Load<Texture2D>("taiko-drum-outer");

        LoadKeyBinds();
    }

    void LoadKeyBinds()
    {
        for (int i = 1; i <= _keysCount; i++)
        {
            string keyName = $"don{i}";
            
            string keyString = _currentKeyBinds[keyName];
            Keys key = (Keys)System.Enum.Parse(typeof(Keys), keyString);
            _donKeys.Add(new KeyBinding(keyName, key));
        }

        for (int i = 1; i <= _keysCount; i++)
        {
            string keyName = $"rim{i}";
            
            string keyString = _currentKeyBinds[keyName];
            Keys key = (Keys)System.Enum.Parse(typeof(Keys), keyString);
            _katKeys.Add(new KeyBinding(keyName, key));
        }
    }

    public void Update()
    {
        previousKeyboardState = keyboardState;
        keyboardState = Keyboard.GetState();

        foreach (var keyBind in _donKeys)
        {
            if (keyboardState.IsKeyDown(keyBind.Key) && !previousKeyboardState.IsKeyDown(keyBind.Key) && !_processedPresses.Contains(keyBind.Key))
            {
                _processedPresses.Add(keyBind.Key);
                _donEffect.Play();
            }

            else if (!keyboardState.IsKeyDown(keyBind.Key) && previousKeyboardState.IsKeyDown(keyBind.Key))
            {
                _processedPresses.Remove(keyBind.Key);
            }
        }

        foreach (var keyBind in _katKeys)
        {
            if (keyboardState.IsKeyDown(keyBind.Key) && !previousKeyboardState.IsKeyDown(keyBind.Key) && !_processedPresses.Contains(keyBind.Key))
            {
                _processedPresses.Add(keyBind.Key);
                _katEffect.Play();
            }

            else if (!keyboardState.IsKeyDown(keyBind.Key) && previousKeyboardState.IsKeyDown(keyBind.Key))
            {
                _processedPresses.Remove(keyBind.Key);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // TO Do
        spriteBatch.Draw(_innerTexture, new Vector2(200, 410), null, Color.White, 0,
                        new Vector2(_innerTexture.Width / 2, _innerTexture.Height / 2), 1f, SpriteEffects.None, 0f);
    }
}

public class KeyBinding
{
    public Keys Key { get; private set; }
    public string Number { get; private set; }
    public KeyBinding(string number, Keys keys)
    {
        Number = number;
        Key = keys;
    }
}
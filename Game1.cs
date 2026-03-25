using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

public class Game1 : Game
{
    GraphicsDeviceManager _graphics;
    SpriteBatch _spriteBatch;
    Cursor _cursor;
    GameField _gameField;
    Settings settings;
    int screenWidth, screenHeight;

    float backgroundDim, rectangleDim;

    SpriteFont _font;
    int _frameCount;
    double _elapsedTime, _fps;
    Vector2 textSize;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        IsFixedTimeStep = false;
        _graphics.SynchronizeWithVerticalRetrace = false;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _cursor = new Cursor();
        FullScreen(_graphics);

        screenWidth = GraphicsDevice.Viewport.Width;
        screenHeight = GraphicsDevice.Viewport.Height;

        settings = Settings.Load();
        backgroundDim = settings.backgroundDim;
        rectangleDim = settings.rectangleDim;

        _gameField = new GameField(screenWidth, rectangleDim);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _cursor.LoadContent(Content);
        _gameField.LoadContent(Content, GraphicsDevice);

        _font = Content.Load<SpriteFont>("DefaultFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _cursor.Update();
        _gameField.Update(gameTime);

        _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
        _frameCount++;

        if (_elapsedTime >= 1.0)
        {
            _fps = _frameCount;
            _frameCount = 0;
            _elapsedTime = 0;
        }

        textSize = _font.MeasureString($"{_fps} fps");

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _gameField.Draw(_spriteBatch, backgroundDim);
        _cursor.Draw(_spriteBatch);

        _spriteBatch.DrawString(_font, $"{_fps} fps",
                                new Vector2(screenWidth - textSize.X - 10, screenHeight - textSize.Y - 10),
                                Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    void FullScreen(GraphicsDeviceManager graphics)
    {
        var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

        graphics.PreferredBackBufferWidth = displayMode.Width;
        graphics.PreferredBackBufferHeight = displayMode.Height;

        graphics.IsFullScreen = true;

        graphics.ApplyChanges();
    }
}
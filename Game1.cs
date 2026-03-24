using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RhytmXT;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Cursor _cursor;
    GameField _gameField;
    Settings settings;

    float backgroundDim, rectangleDim;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        _cursor = new Cursor();
        _gameField = new GameField();
        FullScreen(_graphics);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _cursor.LoadContent(Content);
        _gameField.LoadContent(Content, GraphicsDevice);

        settings = Settings.Load();
        backgroundDim = settings.backgroundDim;
        rectangleDim = settings.rectangleDim;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _cursor.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _gameField.Draw(_spriteBatch, backgroundDim, rectangleDim);
        _cursor.Draw(_spriteBatch);

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
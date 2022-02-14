using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Implementation
{
    enum GameState
    {
        Menu,
        Controls,
        Arena,
        Shop
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont titleFont;
        private SpriteFont displayFont;
        GameState state = GameState.Menu;
        string userInput; //variable used to carry the user input (temporary)
        KeyboardState kbState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            titleFont = Content.Load<SpriteFont>("Title");
            displayFont = Content.Load<SpriteFont>("Display");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kbState = Keyboard.GetState();

            switch (state)
            {
                case GameState.Menu:
                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        state = GameState.Arena;
                    }
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        state = GameState.Controls;
                    }
                    break;
                case GameState.Controls:
                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        state = GameState.Menu;
                    }
                    break;
                case GameState.Arena:
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        state = GameState.Shop;
                    }
                    break;
                case GameState.Shop:
                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        state = GameState.Arena;
                    }
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        state = GameState.Menu;
                    }
                    break;
                default:
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gold);
            switch (state)
            {
                case GameState.Menu:
                    _spriteBatch.DrawString(titleFont, "STRIKE XII",
                        new Vector2(400, 400), Color.Black);
                    break;
                case GameState.Controls:
                    _spriteBatch.DrawString(displayFont, "Filler for Controls page",
                        new Vector2(400, 400), Color.Black);
                    break;
                case GameState.Arena:
                    _spriteBatch.DrawString(displayFont, "Filler for Arena",
                        new Vector2(400, 400), Color.Black);
                    _spriteBatch.DrawString(displayFont, "Press Space to go to the shop page (happens upon character death)",
                        new Vector2(500, 400), Color.Black);
                    break;
                case GameState.Shop:
                    _spriteBatch.DrawString(displayFont, "Filler for Shop page",
                        new Vector2(400, 400), Color.Black);
                    break;
                default:
                    break;
            }
            

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CC_gamePrototype
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Temp player assets
        private Texture2D playerSprites;
        private Player player;
        private Rectangle platformPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the player sprite sheet
            playerSprites = Content.Load<Texture2D>("playerSpriteSheet");

            // Initialize the player with the asset loaded in
            player = new Player(playerSprites, GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            // Temp platform location
            platformPosition = new Rectangle(
                    150,
                    450,
                    playerSprites.Width,
                    playerSprites.Height);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Temp player update call
            player.Update(gameTime, platformPosition);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Temp player draw call (should, in theory, be handled by the animation manager later down the line)
            player.Draw(_spriteBatch, playerSprites);

            // Temp platforms
            _spriteBatch.Draw(
                playerSprites, 
                platformPosition, 
                Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

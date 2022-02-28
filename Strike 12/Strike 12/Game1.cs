﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Strike_12
{
    /// <summary>
    /// enum for gamestates
    /// </summary>
    enum GameState
    {
        Menu,
        Controls,
        Arena,
        Shop
    }
    public class Game1 : Game
    {
        //fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont titleFont;
        private SpriteFont displayFont;

        // Temp player assets
        private Texture2D playerSprites;
        private Player player;
        private Rectangle platformPosition;

        //sets the default state as the menu
        GameState state = GameState.Menu;

        //keyboard States
        KeyboardState kbState;
        KeyboardState prevKbState = Keyboard.GetState();

        //variables
        double timer = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 992;
            _graphics.PreferredBackBufferWidth = 1216;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// define spritebatch and two fonts
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //different fonts
            titleFont = Content.Load<SpriteFont>("Title");
            displayFont = Content.Load<SpriteFont>("Display");

            // Load the player sprite sheet
            playerSprites = Content.Load<Texture2D>("playerSpriteSheet");

            // Temp platform location
            platformPosition = new Rectangle(
                    500,
                    912,
                    playerSprites.Width,
                    playerSprites.Height);

            // Initialize the player with the asset loaded in
            player = new Player
                (playerSprites, new Rectangle (
                    (GraphicsDevice.Viewport.Width / 2),(GraphicsDevice.Viewport.Height / 2),128,128),
                new Vector2(
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2));
        }

        /// <summary>
        /// Gets user input of key presses and depending on which state the game is in,
        /// different keys have different functions
        /// for now it is which state the game will switch to
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //gets keyboard state for each frame
            kbState = Keyboard.GetState();

            
            //switch statement for specific key presses in the different states states
            switch (state)
            {
                //if enter is pressed in menu, starts the game; if space is pressed opens the control screen
                case GameState.Menu:
                    if (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))
                    {
                        state = GameState.Arena;
                    }
                    if (kbState.IsKeyDown(Keys.Space) && prevKbState.IsKeyUp(Keys.Space))
                    {
                        state = GameState.Controls;
                    }
                    break;

                //while in the control screen, press enter to return to the menu
                case GameState.Controls:
                    if (kbState.IsKeyDown(Keys.Space) && prevKbState.IsKeyUp(Keys.Space))
                    {
                        state = GameState.Menu;
                    }
                    break;

                // when in the arena, "dies" when you press space, entering the shop
                case GameState.Arena:

                    // Temp player update call
                    player.Update(gameTime);

                    timer = timer + gameTime.ElapsedGameTime.TotalSeconds;
                    if (timer >= 4)
                    {
                        //win 
                    }
                    if (kbState.IsKeyDown(Keys.Space) && prevKbState.IsKeyUp(Keys.Space))
                    {
                        timer = 0;
                        state = GameState.Shop;
                    }

                    break;

                //if enter is pressed in the shop, returns to arena; if space is pressed brings up the menu
                case GameState.Shop:
                    if (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))
                    {
                        state = GameState.Arena;
                    }
                    if (kbState.IsKeyDown(Keys.Space) && prevKbState.IsKeyUp(Keys.Space))
                    {
                        state = GameState.Menu;
                    }
                    break;

                default:
                    break;
            }

            //uses kbState from this run and defines it as the prevKBState
            //before process repeats
            prevKbState = kbState;
            base.Update(gameTime);
        }

        /// <summary>
        /// draws different assests such as text for different states the game will be in 
        /// and the color of the background
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            //sets background to gold and opens the sprite batch
            GraphicsDevice.Clear(Color.Goldenrod);
            _spriteBatch.Begin();

            //depending on the states, displays the appropriate text 
            switch (state)
            {
                //text for menu screen
                case GameState.Menu:
                    _spriteBatch.DrawString(titleFont, "STRIKE XII",
                        new Vector2(200, 200), Color.Black);
                    _spriteBatch.DrawString(displayFont, "Press Enter to continue\nTo learn the controls, press Space",
                        new Vector2(100, 400), Color.Black);
                    break;

                //text for control screen
                case GameState.Controls:
                    _spriteBatch.DrawString(titleFont, "Filler for Controls page",
                        new Vector2(150, 200), Color.Black);
                    _spriteBatch.DrawString(displayFont, "Press Space to continue to return to the menu",
                        new Vector2(100, 400), Color.Black);
                    break;

                //text for arena screen
                case GameState.Arena:
                    _spriteBatch.DrawString(titleFont, "Filler for Arena",
                        new Vector2(150, 200), Color.Black);
                    _spriteBatch.DrawString(displayFont, "Press Space to go to the shop page (happens upon character death)",
                        new Vector2(100, 400), Color.Black);
                    _spriteBatch.DrawString(displayFont, $"\nTime Passed: {String.Format("{0:0.00}", timer)}",
                       new Vector2(10, 10), Color.Black);

                    // Temp player draw call (should, in theory, be handled by the animation manager later down the line)
                    player.Draw(_spriteBatch, playerSprites);

                    // Temp platforms
                    _spriteBatch.Draw(
                        playerSprites,
                        platformPosition,
                        Color.White);

                    break;

                //text for shop screen
                case GameState.Shop:
                    _spriteBatch.DrawString(titleFont, "Filler for Shop page",
                        new Vector2(150, 200), Color.Black);
                    _spriteBatch.DrawString(displayFont, "Press Enter to return to the arena\nPress Space to return to the menu",
                        new Vector2(100, 400), Color.Black);
                    break;

                default:
                    break;
            }

            //closes the spriteBatch before calling draw
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

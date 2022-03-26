using Microsoft.Xna.Framework;
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
        Shop,
        GameOver,
        GameWinner
    }
    public class Game1 : Game
    {
        //fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont titleFont;
        private SpriteFont displayFont;
        private int windowWidth = 1664;
        private int windowHeight = 960;
        Random rng = new Random();
        private Texture2D titleScreen;

        // Temp player assets
        private Texture2D playerSprites;
        private Player player;
        private int pStartX;
        private int pStartY;

        // Temp enemy assets
        private Texture2D enemySprites;
        private Enemy enemy;
        private int eStartX;
        private int eStartY;
        Rectangle eSize;
        private double waveLength = 10;
        private double waveDelta = 10;
        private EnemyManager eManager;


        // Level Assets
        private LevelEditor editor;
        private Texture2D tileSprites;
        private Tile tile;

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
            _graphics.PreferredBackBufferHeight = windowHeight;
            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.ApplyChanges();
            eManager = new EnemyManager(enemySprites, eSize, windowWidth, windowHeight);



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
            enemySprites = Content.Load<Texture2D>("enemySpriteSheet");

            //other assests
            tileSprites = Content.Load<Texture2D>("brick");
            titleScreen = Content.Load<Texture2D>("Logo (1)");

            pStartX = (GraphicsDevice.Viewport.Width / 2);
            pStartY = (GraphicsDevice.Viewport.Height / 2);

            eStartX = rng.Next(300,windowWidth-300);
            eStartY = rng.Next(300, windowHeight - 300);
            eSize = new Rectangle(eStartX, eStartY, 128, 128);

            // Initialize the player with the asset loaded in
            player = new Player
                (playerSprites, new Rectangle(pStartX, pStartY, 64, 128),
                    windowWidth, windowHeight,
                new Vector2(
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2));

            //eSize.X = rng.Next(300, windowWidth - 300);
            //eSize.Y = rng.Next(300, windowHeight - 300);
            eManager.Initialize();
            enemy = new Enemy(enemySprites, new Rectangle(rng.Next(64, windowWidth - 64), rng.Next(0, windowHeight - 64), 64, 64), windowWidth, windowHeight);
            eManager.SpawnEnemy(enemy);


            // -- LEVEL LOADING --
            editor = new LevelEditor();
            editor.Load(1, tileSprites, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

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
                //if enter is pressed in menu, starts the game
                //if space is pressed opens the control screen
                case GameState.Menu:

                    eManager.Count = 0;
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

                    eManager.FirstWave();
                    timer = timer + gameTime.ElapsedGameTime.TotalSeconds;

                    // Checking collisons
                    for (int i = 0; i < editor.LayoutRows; i++)
                    {
                        for (int j = 0; j < editor.LayoutColumns; j++)
                        {
                            if (editor[i, j] != null)
                            {
                                if (player.CheckCollision(editor[i, j].Type, player, editor[i, j]) 
                                    && (editor[i, j].Type == "ground" || editor[i, j].Type == "platform"))
                                {
                                    player.PlatformPosY = editor[i, j].Size.Y;
                                    player.PlatformPosX = editor[i, j].Size.X;
                                    player.IsGrounded = true;
                                }
                                if (player.CheckCollision(editor[i, j].Type, player, editor[i, j]) && editor[i, j].Type == "leftWall")
                                {
                                    player.WallPosX = editor[i, j].Size.X;
                                    if (player.Size.X-64>editor[i, j].Size.X)
                                    {
                                        player.LeftCollided = false;
                                    }
                                    else
                                    {
                                        player.LeftCollided = true;
                                    }
                                }
                                if (player.CheckCollision(editor[i, j].Type, player, editor[i, j]) && editor[i, j].Type == "rightWall")
                                {
                                    if (editor[i, j].Size.X > player.Size.X + 128)
                                    {
                                        player.RightCollided = false;
                                    }
                                    else
                                    {
                                        player.RightCollided = true;
                                    }
                                }
                            }
                        }
                    }

                    //if space is pressed, go to shop
                    if (kbState.IsKeyDown(Keys.Space) && prevKbState.IsKeyUp(Keys.Space))
                    {
                        timer = 0;
                        state = GameState.Shop;
                    }

                    //checks if player fell in a pit
                    if (player.Size.Y > windowHeight)
                    {
                        player.Health -= 1;
                    }

                        //collision for each enemy in the Enemy class
                        foreach (Enemy enemy in eManager.Enemies)
                    {
                        if (enemy.CheckCollision("enemy", enemy, player))
                        {

                            if(player.TakeDamage(gameTime))
                            {
                                player.Health -= 1;
                            }

                        }
                        else if (enemy.CheckCollision("top", enemy, player))
                        {
                        //has to make the player jump when they hit the top
                        }
                    }
                    //if the player has no more health, go to shop
                    if (player.Health <= 0)
                    {
                        state = GameState.Shop;
                    }

                    // Temp player and enemy update call
                    player.Update(gameTime);
                    foreach (Enemy enemy in eManager.Enemies)
                    {
                        enemy.Update(gameTime);
                    }

                    //if the count of the list is zero (empty), will automatically add one to it
                    if (eManager.Enemies.Count == 0)
                    {
                        enemy = new Enemy(enemySprites, new Rectangle(rng.Next(64, windowWidth - 64), rng.Next(0, windowHeight - 64), 64, 64), windowWidth, windowHeight);
                        eManager.SpawnEnemy(enemy);
                    }

                    //adds one to the count in the manager every frame
                    eManager.Count++;
                    //if the count divided by 60 if equal to or greater than the wave length, adds another to the list
                    if (eManager.Count/60 >= waveLength)
                    {
                        enemy = new Enemy(enemySprites, new Rectangle(rng.Next(64, windowWidth - 64), rng.Next(0, windowHeight - 64), 64, 64), windowWidth, windowHeight);
                        eManager.SpawnEnemy(enemy);
                        waveDelta /= 1.5;
                        waveLength += waveDelta;
                    }
                    break;

                // Game Winner: appears when timer is greater than 30
                case GameState.GameWinner:
                    player.Reset();
                    enemy.Reset();

                    timer = 0;

                    if (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))
                    {
                        state = GameState.Arena;
                    }
                    if (kbState.IsKeyDown(Keys.Space) && prevKbState.IsKeyUp(Keys.Space))
                    {
                        state = GameState.Shop;
                    }

                    break;

                // Game Over: appears when health is less than 1
                case GameState.GameOver:
                    player.Reset();
                    enemy.Reset();

                    timer = 0;

                    if (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))
                    {
                        state = GameState.Arena;
                    }
                    if (kbState.IsKeyDown(Keys.Space) && prevKbState.IsKeyUp(Keys.Space))
                    {
                        state = GameState.Shop;
                    }

                    break;

                //if enter is pressed in the shop, returns to arena; if space is pressed brings up the menu
                case GameState.Shop:

                    //resets data from Arena
                    eManager.Enemies.Clear();
                    waveLength = 10;
                    waveDelta = 10;
                    eManager.Count = 0;
                    timer = 0;
                    player.Reset();
                    foreach (Enemy enemy in eManager.Enemies)
                    {
                        enemy.Reset();
                    }

                    //key presses to change between gamestates
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
                    _spriteBatch.Draw(titleScreen, new Rectangle((windowWidth/2 - titleScreen.Width/2 - 250), (windowHeight/2 - titleScreen.Height/2 - 400), 1500, 750), Color.White);
                    _spriteBatch.DrawString(displayFont, "        Press Enter to continue\nTo learn the controls, press Space",
                        new Vector2(700, 1500), Color.Black);
                    break;

                //text for control screen
                case GameState.Controls:
                    _spriteBatch.DrawString(titleFont, "Press W to Jump\nPress A to Move Left\nPress D to Move Right",
                        new Vector2(150, 200), Color.Black);
                    _spriteBatch.DrawString(displayFont, "Press Space to continue to return to the menu",
                        new Vector2(100, 1800), Color.Black);
                    break;

                //text for arena screen
                case GameState.Arena:

                    // Draw the tiles
                    editor.Draw(_spriteBatch, tileSprites);

                    _spriteBatch.DrawString(displayFont, "Press Space to go to the shop page (happens upon character death)",
                        new Vector2(100, 400), Color.Black);
                    _spriteBatch.DrawString(displayFont, $"\nTime Passed: {String.Format("{0:0.00}", timer)}",
                       new Vector2(100, 150), Color.Black);
                    _spriteBatch.DrawString(displayFont, $"\nPlayer Health: {player.Health}",
                       new Vector2(100, 100), Color.Black);

                    // Temp player draw call (should, in theory, be handled by the animation manager later down the line)
                    player.Draw(_spriteBatch, playerSprites);
                    foreach (Enemy enemy in eManager.Enemies)
                    {
                        enemy.Draw(_spriteBatch, enemySprites);
                    }

                    break;

                //Text for game winner screen
                case GameState.GameWinner:
                    _spriteBatch.DrawString(titleFont, "Filler for Game Winner page",
                        new Vector2(150, 200), Color.Black);
                    _spriteBatch.DrawString(displayFont, "Press Enter to return to the arena\nPress Space to return to the shop",
                        new Vector2(100, 400), Color.Black);
                    break;

                // Text for game over state
                case GameState.GameOver:
                    _spriteBatch.DrawString(titleFont, "Filler for Game Over page",
                        new Vector2(150, 200), Color.Black);
                    _spriteBatch.DrawString(displayFont, "Press Enter to return to the arena\nPress Space to return to the shop",
                        new Vector2(100, 400), Color.Black);
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

        //need reset method to reset the following:
        //time, player and enemy position, player health



    }
}

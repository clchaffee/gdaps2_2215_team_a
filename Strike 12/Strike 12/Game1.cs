﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Strike_12
{
    /// <summary>
    /// enum for gamestates
    /// </summary>
    enum GameState
    {
        Menu,
        Controls,
        Start,
        Arena,
        Shop,
        GameOver,
        GameWinner
    }
    /// <summary>
    /// best unupgraded time: Colby with 81.43s
    /// </summary>
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
        private bool fading;
        private int fadeOpacity;
        private Texture2D black;

        public int Interval { get; set; } = 0;
        private bool easy = true;
        private bool medium = false;
        private bool hard = false;
        private bool impossible = false;
        private bool collidable = true;

        private int count;
        private bool spawnCap = true;
        private int waitTime;

        // player assets
        private Texture2D playerSprites;
        private Player player;
        private int pStartX;
        private int pStartY;

        private bool isCollidingUp;
        private bool isCollidingDown;
        private bool isCollidingRight;
        private bool isCollidingLeft;

        private bool playerInvincible = false;

        private float percent = 0f;
        private bool incrementing = false;

        // enemy assets
        private Texture2D enemySprites;
        private Texture2D enemyFollow;
        private Texture2D enemyBase;
        private Texture2D enemyBullet;
        private Texture2D enemyBounce;
        private Texture2D enemyBounceFlip;
        private Texture2D enemyLaser;

        // Enemy types for testing purposes
        private Enemy enemy;
        private BulletEnemy bEnemy;
        private BounceEnemy pEnemy;
        private FollowEnemy fEnemy;
        private LaserEnemy lEnemy;
        private int eStartX;
        private int eStartY;
        Rectangle eSize;
        private double waveLength = 10;
        private double waveDelta = 10;
        private EnemyManager eManager;
        private EnemyManager bManager;
        int wave = 1;
        private double dampener = .04;
        int interval = 0;
        float bounceRotate;

        //variables for the shop
        private Shop shop;
        private int points = 0;
        private List<Button> buttons = new List<Button>();
        private Texture2D buttonTexture;
        private Texture2D shopWall;
        private Texture2D shopFG;
        private Texture2D shopKeeper;
        private Texture2D sign;
        private Texture2D smallSign;
        private Texture2D shelf;
        private Texture2D controls;
        private Texture2D textBox;

        private string comment;

        //other buttons
        private Texture2D noseButton;
        private Texture2D startButton;
        private Texture2D optionButton;
        private Texture2D menuButton;
        private Texture2D buttonSelect;

        //items
        private Texture2D healthUpgrade;
        private Texture2D speedUpgrade;
        private Texture2D energyUpgrade;
        private Texture2D dashUpgrade;
        private Texture2D timeUpgrade;

        // Level Assets
        private LevelEditor editor;
        private Texture2D tileSprites;
        private Tile tile;
        private List<LevelEditor> levels;
        private int lvlNum;

        private Texture2D backBar;
        private Texture2D bar;
        private Texture2D healthSprite;
        private Texture2D energySprite;

        // Other Assets
        private Texture2D arenaBackground;
        private Texture2D titleBG;
        private Texture2D arenaBG;

        private Texture2D clockMinute1;
        private Texture2D clockMinute2;
        private Texture2D clockHour;
        private Texture2D clockTop;
        private Texture2D Shade;
        private Texture2D cat;

        //sets the default state as the menu
        GameState state = GameState.Menu;

        //keyboard & Mouse States
        KeyboardState kbState;
        KeyboardState prevKbState = Keyboard.GetState();
        private MouseState mouseState;
        private MouseState prevMouseState;

        //variables
        double timer = 0;
        double clockTimer = 0;
        int energyTimer = 0;
        int stoppedTimer = 0;

        // Animation Fields
        AnimationManager playerAnimation;
        AnimationManager clockHourAnimation;
        AnimationManager clockMin1Animation;
        AnimationManager clockMin2Animation;
        AnimationManager shopKeeperAnimation;
        AnimationManager catAnimation;
        AnimationManager shadeManager;
        Texture2D playerIdle;
        Texture2D playerWalk;
        Texture2D playerCrouch;
        Texture2D playerDash;
        Texture2D playerDashAlt;
        PlayerStates playerState;

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
            eManager = new EnemyManager(windowWidth, windowHeight);
            bManager = new EnemyManager(windowWidth, windowHeight);

            //initializing screen fading
            fading = false;
            fadeOpacity = 0;

            //initializes comment to null
            comment = null;

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

            // Load textures
            playerSprites = Content.Load<Texture2D>("playerSpriteSheet");
            enemySprites = Content.Load<Texture2D>("enemySpriteSheet");
            buttonTexture = Content.Load<Texture2D>("tempTile");

            //other assests
            tileSprites = Content.Load<Texture2D>("brick");
            titleScreen = Content.Load<Texture2D>("Logo (1)");
            titleBG = Content.Load<Texture2D>("NewTitle");
            arenaBG = Content.Load<Texture2D>("Arena");
            arenaBackground = Content.Load<Texture2D>("ArenaBG");

            backBar = Content.Load<Texture2D>("backbar");
            bar = Content.Load<Texture2D>("bar");
            healthSprite = Content.Load<Texture2D>("health-sprite");
            energySprite = Content.Load<Texture2D>("energy-sprite");
            controls = Content.Load<Texture2D>("controls");

            //Shop
            shopWall = Content.Load<Texture2D>("ShopWall");
            shopFG = Content.Load<Texture2D>("ShopFG");
            shopKeeper = Content.Load<Texture2D>("shopKeeperSheet");
            sign = Content.Load<Texture2D>("sign");
            smallSign = Content.Load<Texture2D>("sign-export");
            shelf = Content.Load<Texture2D>("shelf");
            textBox = Content.Load<Texture2D>("textbox");

            //fading asset
            black = Content.Load<Texture2D>("black");

            startButton = Content.Load<Texture2D>("Start");
            optionButton = Content.Load<Texture2D>("Options");
            menuButton = Content.Load<Texture2D>("Menu");
            buttonSelect = Content.Load<Texture2D>("buttonSelect");

            noseButton = Content.Load<Texture2D>("CatNose");

            healthUpgrade = Content.Load<Texture2D>("HealthBottle");
            speedUpgrade = Content.Load<Texture2D>("SpeedBottle");
            energyUpgrade = Content.Load<Texture2D>("EnergyBottle");
            dashUpgrade = Content.Load<Texture2D>("DashBoot");
            timeUpgrade = Content.Load<Texture2D>("WatchStop");

            pStartX = (GraphicsDevice.Viewport.Width / 2);
            pStartY = (GraphicsDevice.Viewport.Height - 192);

            eStartX = rng.Next(300, windowWidth - 300);
            eStartY = rng.Next(300, windowHeight - 300);
            eSize = new Rectangle(eStartX, eStartY, 128, 128);

            // Initialize the player with the asset loaded in
            player = new Player
                (playerSprites, new Rectangle(pStartX, pStartY, 64, 128),
                    windowWidth, windowHeight,
                new Vector2(
                64,
                GraphicsDevice.Viewport.Height - 128));

            // ENEMY STUFF
            eManager.Initialize();

            

            // -- LEVEL LOADING --
            levels = new List<LevelEditor>();
            for (int i = 0; i < 6; i++)
            {
                levels.Add(new LevelEditor());
                levels[i].Load(i + 1, tileSprites, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }
            lvlNum = 0;

            //makes a new shop and buttons for each of the purchases
            shop = new Shop(points);

            //other buttons at the start of the list for the start screen
            buttons.Add(new Button("start", startButton, new Rectangle(windowWidth / 2 - 256 / 2, windowHeight/2, 256, 124), 0));
            buttons.Add(new Button("options", optionButton, new Rectangle(windowWidth / 2 - 256 / 2, 600, 256, 124), 0));
            buttons.Add(new Button("menu", menuButton, new Rectangle(300, 800, 256, 124), 0));

            buttons.Add(new Button("Health",
                healthUpgrade,
                new Rectangle(1100, 150, healthUpgrade.Width, healthUpgrade.Height),
                30));

            buttons.Add(new Button("Speed",
                speedUpgrade,
                new Rectangle(1250, 150, speedUpgrade.Width, speedUpgrade.Height),
                50));

            buttons.Add(new Button("Energy",
                energyUpgrade,
                new Rectangle(1400, 150, energyUpgrade.Width, energyUpgrade.Height),
                100));

            buttons.Add(new Button("Dash",
                dashUpgrade,
                new Rectangle(1100, 390, dashUpgrade.Width, dashUpgrade.Height),
                150));

            buttons.Add(new Button("Time Stop",
                  timeUpgrade,
                  new Rectangle(1400, 390, timeUpgrade.Width, timeUpgrade.Height),
                  450));

            /*        NOT FOR SPRINT 3
            buttons.Add(new Button("heal", 
                buttonTexture, 
                new Rectangle(1250, 300, 100, 50),
                10));

            buttons.Add(new Button("slow", 
                buttonTexture, 
                new Rectangle(1400, 300, 100, 50), 
                10));*/

            buttons.Add(new Button("cat",
             noseButton,
             new Rectangle(197, 625, noseButton.Width / 4, noseButton.Height / 4), 0));

            // For animation
            playerAnimation = new AnimationManager();
            playerIdle = Content.Load<Texture2D>("playerIdle");
            playerWalk = Content.Load<Texture2D>("playerWalk");
            playerCrouch = Content.Load<Texture2D>("playerCrouch");
            playerDash = Content.Load<Texture2D>("Dash");
            playerDashAlt = Content.Load<Texture2D>("DashAlt");

            enemyBase = Content.Load<Texture2D>("Enemy");
            enemyBullet = Content.Load<Texture2D>("BulletEnemy");
            enemyFollow = Content.Load<Texture2D>("FollowEnemy");
            enemyBounce = Content.Load<Texture2D>("BounceEnemy");
            enemyBounceFlip = Content.Load<Texture2D>("BounceEnemyFlip");
            enemyLaser = Content.Load<Texture2D>("LaserEnemy");

            bounceRotate = 0;

            clockHourAnimation = new AnimationManager();
            clockMin1Animation = new AnimationManager();
            clockMin2Animation = new AnimationManager();
            shadeManager = new AnimationManager();
            clockHour = Content.Load<Texture2D>("HourHand");
            clockMinute1 = Content.Load<Texture2D>("MinuteHand1");
            clockMinute2 = Content.Load<Texture2D>("MinuteHand2");
            clockTop = Content.Load<Texture2D>("clockTop");
            shopKeeperAnimation = new AnimationManager();
            Shade = Content.Load<Texture2D>("atmosphere");
            cat = Content.Load<Texture2D>("cat");
            catAnimation = new AnimationManager();
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
                    if (buttons[0].IsPressed)
                    {
                        state = GameState.Start;
                    }

                    /* god mode here
                    if (kbState.IsKeyDown(Keys.OemTilde) && prevKbState.IsKeyUp(Keys.OemTilde))
                    {
                        collidable = false;
                    }
                    */

                    buttons[0].Update(gameTime);

                    playerAnimation.Update(gameTime, 3, .09);
                    break;

                case GameState.Start:

                    playerAnimation.Update(gameTime, 8, .09);
                    player.SizeX = player.SizeX + 10;
                    if (player.SizeX > _graphics.PreferredBackBufferWidth)
                    {
                        state = GameState.Arena;
                    }
                    //starts to fade
                    fading = true;
                    fadeOpacity = fadeOpacity + 3;
                    break;

                // when in the arena, "dies" when you press space, entering the shop
                case GameState.Arena:

                    shadeManager.Update(gameTime, 8, 0.02);

                    // ClockAnimation
                    clockHourAnimation.Update(gameTime, 12, 0.00055555556);
                    if (clockTimer <=15 && !player.TimeStopActive)
                    {
                        clockMin1Animation.Update(gameTime, 15, 0.0166666667);
                    }
                    else if (clockTimer <30 && !player.TimeStopActive)
                    {
                        clockMin2Animation.Update(gameTime, 15, 0.0166666667);
                    }
                    else if (!player.TimeStopActive)
                    {
                        clockTimer = 0;
                        clockMin1Animation.Reset();
                        clockMin2Animation.Reset();
                        clockMin1Animation.Update(gameTime, 15, 0.0166666667);
                    }

                    /*debug controls for Annalee while working on shop
                    if (kbState.IsKeyDown(Keys.Back) && prevKbState.IsKeyUp(Keys.Back))
                    {
                        state = GameState.GameOver;
                    }
                    */

                    //resets fading
                    fading = false;
                    fadeOpacity = 0;

                    //eManager.FirstWave();
                    if (!player.TimeStopActive)
                    {
                        timer = timer + gameTime.ElapsedGameTime.TotalSeconds;
                        clockTimer = clockTimer + gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    //updates levels every 2 minutes, would use % but game time is too fast to process that
                    if (timer >= 360)
                    {
                        state = GameState.GameWinner;
                    }
                    if (timer >= 300)
                    {
                        lvlNum = 5;
                    }
                    else if (timer >= 240)
                    {
                        lvlNum = 4;
                    }
                    else if (timer >= 180)
                    {
                        lvlNum = 3;
                    }
                    else if (timer >= 120)
                    {
                        lvlNum = 2;
                    }
                    else if (timer >= 60)
                    {
                        lvlNum = 1;
                    }

                    // Temp player and enemy update call
                    player.Update(gameTime);

                    playerState = player.State;

                    switch (playerState)
                    {
                        case PlayerStates.moveRight:
                        case PlayerStates.jumpRight:
                            playerAnimation.Update(gameTime, 8, .1);
                            break;
                        case PlayerStates.moveLeft:
                        case PlayerStates.jumpLeft:
                            playerAnimation.Update(gameTime, 8, .1);
                            break;
                        case PlayerStates.faceRight:
                            playerAnimation.Update(gameTime, 3, .09);
                            break;
                        case PlayerStates.faceLeft:
                            playerAnimation.Update(gameTime, 3, .09);
                            break;
                        case PlayerStates.crouchLeft:
                        case PlayerStates.crouchRight:
                            playerAnimation.Update(gameTime, 1, .09);
                            break;
                        case PlayerStates.airdash:
                            playerAnimation.Update(gameTime, 1, .09);
                            break;
                    }


                    // Increment the player's energy if it is currently under the maximum
                    if (player.CurrentEnergy < player.Energy && !player.TimeStopActive)
                    {
                        if (energyTimer > 60)
                        {
                            player.CurrentEnergy += 2;
                            energyTimer = 0;
                        }
                        else
                        {
                            energyTimer++;
                        }
                    }

                    if (player.CurrentEnergy > 60)
                    {
                        player.CurrentEnergy = 0;
                    }

                    isCollidingUp = false;
                    isCollidingDown = false;
                    isCollidingRight = false;
                    isCollidingLeft = false;

                    // Collision Detection
                    for (int i = 0; i < levels[lvlNum].LayoutRows; i++)
                    {
                        for (int j = 0; j < levels[lvlNum].LayoutColumns; j++)
                        {
                            if (levels[lvlNum][i, j] != null)
                            {
                                //// Check for top collisions
                                if (player.IsCollidingTop(player, levels[lvlNum][i, j]) &&
                                    (!isCollidingUp) && (levels[lvlNum][i, j].Type != "leftWall" && levels[lvlNum][i, j].Type != "rightWall"))
                                {
                                    while (player.Size.Bottom != levels[lvlNum][i, j].Size.Top)
                                    {
                                        player.SizeY -= 1;
                                    }

                                    player.VelocityY = 0;
                                    player.PositionY = player.SizeY;

                                    player.IsGrounded = true;
                                    isCollidingUp = true;
                                }

                                // Check for bottom collisions
                                if (player.IsCollidingBottom(player, levels[lvlNum][i, j]) &&
                                    (!isCollidingDown) && (levels[lvlNum][i, j].Type != "leftWall" && levels[lvlNum][i, j].Type != "rightWall"))
                                {
                                    while (player.Size.Top != levels[lvlNum][i, j].Size.Bottom)
                                    {
                                        player.SizeY++;
                                    }

                                    player.PositionY = player.SizeY;

                                    player.VelocityY = 0;
                                    player.CanJump = false;
                                    player.IsGrounded = false;
                                    isCollidingDown = true;
                                }

                                // Check for left collisions
                                if (player.IsCollidingLeft(player, levels[lvlNum][i, j], player.VelocityX) &&
                                    (!isCollidingLeft))
                                {
                                    player.LeftCollided = true;

                                    while (player.Size.Left != levels[lvlNum][i, j].Size.Right)
                                    {
                                        player.SizeX += 1;
                                    }

                                    player.PositionX = player.SizeX;

                                    isCollidingLeft = true;
                                }
                                else
                                {
                                    if (player.Size.Left > levels[lvlNum][i, j].Size.Right)
                                    {
                                        player.LeftCollided = false;
                                    }
                                }

                                // Check for right collisions
                                if (player.IsCollidingRight(player, levels[lvlNum][i, j], player.VelocityX) &&
                                    (!isCollidingRight))
                                {
                                    player.RightCollided = true;

                                    while (player.Size.Right != levels[lvlNum][i, j].Size.Left)
                                    {
                                        player.SizeX -= 1;
                                    }

                                    player.PositionX = player.SizeX;

                                    isCollidingRight = true;
                                }
                                else
                                {
                                    if (player.Size.Right < levels[lvlNum][i, j].Size.Left)
                                    {
                                        player.RightCollided = false;
                                    }
                                }
                            }
                        }
                    }

                    //checks if player fell in a pit
                    if (player.Size.Y > windowHeight)
                    {
                        //player.Health -= 1;
                        state = GameState.GameOver;
                    }

                    if (collidable == true)
                    {
                        if (!player.TimeStopActive)
                        {
                            foreach (Enemy enemy in eManager.Enemies)
                            {

                                if (enemy.IsCollidingBottom(enemy, player) ||
                                    enemy.IsCollidingLeft(enemy, player, player.VelocityX) ||
                                    enemy.IsCollidingRight(enemy, player, player.VelocityX))
                                {
                                    if (player.TakeDamage(gameTime))
                                    {
                                        player.Health -= 1;
                                        playerInvincible = true;
                                    }
                                }
                            }
                        }
                    }

                    // Update I-Frames Counter
                    if (player.ICounter > 0)
                    {
                        player.ICounter--;
                    }
                    else
                    {
                        playerInvincible = false;
                    }

                    if (!player.TimeStopActive)
                    {
                        foreach (Enemy enemy in eManager.Enemies)
                        {
                            if (enemy is FollowEnemy)
                            {
                                ((FollowEnemy)enemy).Update(gameTime, player);

                                enemy.AnimationUpdate(gameTime, 3, 0.4);
                            }
                            else if (enemy is LaserEnemy)
                            {
                                ((LaserEnemy)enemy).Update(gameTime, player.Size.Y);

                                enemy.AnimationUpdate(gameTime, 3, 0.6);
                            }
                            else if (enemy is BounceEnemy)
                            {
                                enemy.Update(gameTime);

                                enemy.AnimationUpdate(gameTime, 3, 0.04);
                            }
                            else
                            {
                                enemy.Update(gameTime);

                                if (enemy is BulletEnemy)
                                {
                                    enemy.AnimationUpdate(gameTime, 5, 0.6);
                                }
                                else if (enemy is Enemy)
                                {
                                    enemy.AnimationUpdate(gameTime, 4, .04);
                                }
                            }
                        }
                    }

                    //adds one to the count in the manager every frame
                    eManager.Count++;


                    if ((int)timer % 5 == 0 && !player.TimeStopActive)
                    {
                        if (spawnCap)
                        {
                            //waves
                            switch (eManager.WaveNum)
                            {
                                //wave 1 always spawns regular enemies
                                case 1:
                                    //eManager.SpawnFormula(.09);
                                    //eManager.Enemies.Clear();
                                    if (Interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(1, 100) > 50)
                                            {
                                                eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(1, 100) > 50)
                                            {
                                                eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        eManager.NumEnemies--;
                                        interval += 5;
                                        Interval++;
                                    }


                                    break;

                                case 2:
                                    //eManager.SpawnFormula(.09);
                                    //eManager.Enemies.Clear();
                                    if (Interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 80)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) > 39)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        eManager.NumEnemies--;
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;

                                //wave 3: 60% for normal, 40% for bullet
                                case 3:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 60)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) > 39)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        eManager.NumEnemies--;
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;

                                //wave 4: 40% normal, 40% projectile, 20% bounce
                                case 4:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 40)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 40)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 40)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 40)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }

                                    break;

                                //wave 5: 30% normal, 30% bounce, 40% bullet
                                case 5:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 30)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 30)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 30)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 30)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }


                                    dampener -= .001;
                                    break;

                                //Wave 6: 40% normal, 20% bounce, 30% bullet, 10% follow
                                case 6:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 40)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 30)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 20)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 40)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 30)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 20)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;
                                //wave 7: 20% norm 30% bounce 20% bullet 30% follow
                                case 7:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 20)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 20)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 30)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 20)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 20)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 30)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;

                                //10 norm  40 follow 40 bounce 10 bullet
                                case 8:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 10)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 10)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 40)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 10)
                                            {
                                                if (rng.Next(1, 100) > 50)
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(64, 384), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                                else
                                                {
                                                    eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(windowWidth - 128 - 384, windowWidth - 128), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                                }
                                            }
                                            else if (rng.Next(0, 100) < 10)
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 40)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;

                                //50 follow 50 bounce
                                case 9:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 50)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 50)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;

                                //60 follow 30 bounce 10 bullet
                                case 10:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 60)
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 30)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {

                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 60)
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 30)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            if(Interval == 3)
                                            {
                                                eManager.WaveProgress(new LaserEnemy(enemySprites, new Rectangle(0, 0, 64, 128), windowWidth, windowHeight, player.SizeY), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;

                                case 11:
                                    //70 follow 10 laser 20 bounce
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 10)
                                            {
                                                eManager.WaveProgress(new LaserEnemy(enemySprites, new Rectangle(0, 0, 64, 128), windowWidth, windowHeight, player.SizeY), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 20)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 10)
                                            {
                                                eManager.WaveProgress(new LaserEnemy(enemySprites, new Rectangle(0, 0, 64, 128), windowWidth, windowHeight, player.SizeY), Interval);
                                            }
                                            else if (rng.Next(0, 100) < 20)
                                            {
                                                eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;

                                //50 follow 50 laser
                                case 12:
                                    if (interval == 0)
                                    {
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        int tooMany = 0;
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 50)
                                            {                                                
                                                eManager.WaveProgress(new LaserEnemy(enemySprites, new Rectangle(0, 0, 64, 128), windowWidth, windowHeight, player.SizeY), Interval);
                                                tooMany++;
                                                if(tooMany >= 4)
                                                {
                                                    eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                                }
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    else
                                    {
                                        int tooMany = 0;
                                        int spawned = eManager.SpawnFormula(dampener, interval, Interval);
                                        for (int i = 0; i < spawned; i++)
                                        {
                                            if (rng.Next(0, 100) < 50)
                                            {
                                                eManager.WaveProgress(new LaserEnemy(enemySprites, new Rectangle(0, 0, 64, 128), windowWidth, windowHeight, player.SizeY), Interval);
                                                tooMany++;
                                                if (tooMany >= 4)
                                                {
                                                    eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                                }
                                            }
                                            else
                                            {
                                                eManager.WaveProgress(new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight), Interval);
                                            }
                                        }
                                        interval += 5;
                                        Interval++;
                                    }
                                    break;


                                default:
                                    break;
                            }


                            count++;
                            spawnCap = false;
                            waitTime = 59;
                            if (Interval == 7)
                            {
                                eManager.Start += 5;
                                eManager.End += 5;
                                Interval = 0;
                                eManager.Enemies.Clear();
                                eManager.NumEnemies = 0;
                                eManager.WaveNum++;
                                spawnCap = true;
                                dampener -= .001;
                                interval = (eManager.WaveNum - 1) * 5;
                            }
                        }
                        else
                        {
                            waitTime--;
                            if (waitTime == 0)
                            {
                                spawnCap = true;
                            }
                        }
                    }

                    //if the player has no more health, go to shop
                    if (player.Health <= 0)
                    {
                        state = GameState.GameOver;
                    }

                    break;

                // Game Winner: appears when timer is greater than 30
                case GameState.GameWinner:
                    //resets data from Arena
                    eManager.Enemies.Clear();

                    timer = timer + gameTime.ElapsedGameTime.TotalSeconds;

                    //starts to fade
                    fading = true;
                    if (fadeOpacity < 256)
                    {
                        fadeOpacity = fadeOpacity + 3;
                    }
                    
                    break;

                // Game Over: appears when health is less than 1
                case GameState.GameOver:
                    player.Reset();
                    player.Deaths++;

                    // Ensure that the player isn't invincible
                    playerInvincible = false;

                    foreach (Enemy enemy in eManager.Enemies)
                    {
                        enemy.Reset();
                    }
                    eManager.Start = 0;
                    eManager.End = 30;
                    interval = 0;
                    Interval = 0;
                    eManager.NumEnemies = 0;
                    eManager.Enemies.Clear();
                    dampener = .04;
                    eManager.WaveNum = 1;

                    //level reset
                    lvlNum = 0;
                    clockHourAnimation.Reset();
                    clockMin1Animation.Reset();
                    clockMin2Animation.Reset();

                    //resets data from Arena
                    eManager.Enemies.Clear();
                    waveLength = 10;
                    waveDelta = 10;
                    eManager.Count = 0;

                    if (timer > player.BestTime)
                    {
                        player.BestTime = (float)timer;
                    }

                    //you get 5 points per second spent alive in the arena
                    shop.Points += 5 * (int)timer;
                    timer = 0;
                    clockTimer = 0;

                    //sets a new comment
                    comment = shop.Comment(null);

                    fading = true;
                    fadeOpacity = fadeOpacity + 3;
                    if (fadeOpacity >= 256)
                    {
                        state = GameState.Shop;
                    }

                    break;

                //if enter is pressed in the shop, returns to arena; if space is pressed brings up the menu
                case GameState.Shop:
                    player.Health = shop.MaxHealth;

                    /*debug key for money
                    if (kbState.IsKeyDown(Keys.P) && prevKbState.IsKeyUp(Keys.P))
                    {
                        shop.Points += 1000;
                    }
                    */

                    shopKeeperAnimation.Update(gameTime, 14, .15);
                    catAnimation.Update(gameTime, 8, .12);

                    // for each button calls update method, checks if pressed and gives upgrade if you have enough points
                    foreach (Button button in buttons)
                    {
                        if (button.Type != "start" || button.Type != "options" || button.Type != "menu")
                        {
                            button.Update(gameTime);
                        }

                        //if the button has been prssed and the player has enough points to purchase the item
                        if (button.IsPressed && shop.Points >= button.Cost)
                        {
                            if (button.Type != "Dash" && button.Type != "Time Stop")
                            {
                                shop.Points -= button.Cost;
                                shop.Spendings += button.Cost;
                            }
                            else if (button.Type == "Dash" && !shop.AirDash)
                            {
                                shop.Points -= button.Cost;
                                shop.Spendings += button.Cost;
                            }
                            else if (button.Type == "Time Stop" && !shop.TimeSlow)
                            {
                                shop.Points -= button.Cost;
                                shop.Spendings += button.Cost;
                            }
                            //switch statement for each button to increase cost
                            switch (button.Type)
                            {
                                case "Health":
                                    button.Cost += 10;
                                    shop.MaxHealth += 1;
                                    player.MaxHealth += 1;
                                    break;

                                case "Speed":
                                    button.Cost += 20;
                                    player.BaseSpeed += 0.05f;
                                    break;

                                case "Energy":
                                    button.Cost += 50;
                                    player.Energy += 2f;
                                    break;

                                case "Dash":
                                    player.dashPurchased = true;
                                    shop.AirDash = true;
                                    break;

                                case "Time Stop":
                                    player.timeStopPurchased = true;
                                    shop.TimeSlow = true;
                                    break;
                            }
                        }
                    }

                    //key presses to change between gamestates
                    if (kbState.IsKeyDown(Keys.Enter) && prevKbState.IsKeyUp(Keys.Enter))
                    {
                        player.Reset();
                        state = GameState.Arena;
                    }
                    if (kbState.IsKeyDown(Keys.Q) && prevKbState.IsKeyUp(Keys.Q))
                    {
                        player.SizeX = GraphicsDevice.Viewport.Width / 2;
                        player.SizeY = GraphicsDevice.Viewport.Height - 196;
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
                case GameState.Start:
                    _spriteBatch.Draw(titleBG, new Rectangle(0, 0, titleBG.Width, titleBG.Height), Color.White);

                    switch (state)
                    {
                        case GameState.Menu:
                            playerAnimation.Draw(_spriteBatch, playerIdle, player.Size, SpriteEffects.None, 0f, 64, 1f);
                            break;

                        case GameState.Start:
                            playerAnimation.Draw(_spriteBatch, playerWalk, player.Size, SpriteEffects.None, 0f, 64, 1f);
                            break;
                    }

                    _spriteBatch.Draw(arenaBG, new Rectangle(0, 75, titleBG.Width, titleBG.Height), Color.White);

                    //draw buttons
                    buttons[0].Draw(_spriteBatch, displayFont, buttonSelect, smallSign);

                    //fade
                    if (fading)
                    {
                        _spriteBatch.Draw(black, new Vector2(0, 0), null, new Color(0, 0, 0, fadeOpacity), 0f, Vector2.Zero, new Vector2(windowWidth, windowHeight), SpriteEffects.None, 0);
                    }
                    break;

                //text for arena screen
                case GameState.Arena:
                case GameState.GameWinner:

                    _spriteBatch.Draw(arenaBackground, new Vector2(0, 0), Color.White);
                    clockHourAnimation.Draw(_spriteBatch, clockHour, new Rectangle(0, 0, windowWidth, windowHeight), SpriteEffects.None, 0f, windowWidth, 1f);
                    if (clockTimer <= 15 || clockTimer > 30)
                    {
                        clockMin1Animation.Draw(_spriteBatch, clockMinute1, new Rectangle(0, 0, windowWidth, windowHeight), SpriteEffects.None, 0f, windowWidth, 1f);
                    }
                    else if (clockTimer < 30)
                    {
                        clockMin2Animation.Draw(_spriteBatch, clockMinute2, new Rectangle(0, 0, windowWidth, windowHeight), SpriteEffects.None, 0f, windowWidth, 1f);
                    }
                    _spriteBatch.Draw(clockTop, new Rectangle(50, 50, windowWidth - 100, windowHeight - 100), Color.White);
                    // Draw the tiles
                    levels[lvlNum].Draw(_spriteBatch, tileSprites);

                    //health bar
                    _spriteBatch.Draw(backBar, new Rectangle(75, 10, 
                        player.Health * (backBar.Width / 2) / player.MaxHealth, backBar.Height / 2), Color.Red);
                    _spriteBatch.Draw(bar, new Rectangle(75, 10, bar.Width / 2, bar.Height / 2), Color.White);
                    _spriteBatch.Draw(healthSprite, new Vector2(0, 0), Color.White);

                    //energy bar
                    _spriteBatch.Draw(backBar, new Rectangle(windowWidth - (bar.Width / 2) - 70, 10, 
                        (int)(player.CurrentEnergy * (backBar.Width / 2) / player.Energy), backBar.Height / 2), Color.Green);
                    _spriteBatch.Draw(bar, new Rectangle(windowWidth - (bar.Width / 2) - 70, 10, bar.Width / 2, bar.Height / 2), Color.White);
                    _spriteBatch.Draw(energySprite, new Vector2(windowWidth - energySprite.Width, 0), Color.White);


                    if (timer <= 5)
                    {
                        _spriteBatch.Draw(controls, new Rectangle(64, windowHeight / 2 + 125, controls.Width * 3, controls.Height * 3), Color.White);
                    }
                    

                    //_spriteBatch.DrawString(displayFont, $"\nTime Passed: {String.Format("{0:0.00}", timer)}",
                    //    new Vector2(100, 150), Color.LightGray);

                    //_spriteBatch.DrawString(displayFont, $"\nWave: {eManager.WaveNum}",
                    //    new Vector2(100, 100), Color.White);
                    //_spriteBatch.DrawString(displayFont, $"\n# of enemies in wave: {eManager.Enemies.Count}",
                    //    new Vector2(100, 250), Color.LightGray);

                    // Update i-frame flash percent
                    if (playerInvincible)
                    {
                        if (!incrementing)
                        {
                            if (percent > 0.3)
                            {
                                percent -= 0.4f;
                            }
                            else
                            {
                                incrementing = true;
                            }
                        }
                        else
                        {
                            if (percent < 1)
                            {
                                percent += 0.4f;
                            }
                            else
                            {
                                incrementing = false;
                            }
                        }
                    }
                    else
                    {
                        percent = 1f;
                    }

                    // Player switch statement
                    switch (playerState)
                    {
                        case PlayerStates.moveRight:
                        case PlayerStates.jumpRight:
                            playerAnimation.Draw(_spriteBatch, playerWalk, player.Size, SpriteEffects.None, 0f, 64, percent);
                            break;
                        case PlayerStates.moveLeft:
                        case PlayerStates.jumpLeft:
                            playerAnimation.Draw(_spriteBatch, playerWalk, player.Size, SpriteEffects.FlipHorizontally, 0f, 64, percent);
                            break;
                        case PlayerStates.faceRight:
                            playerAnimation.Draw(_spriteBatch, playerIdle, player.Size, SpriteEffects.None, 0f, 64, percent);
                            break;
                        case PlayerStates.faceLeft:
                            playerAnimation.Draw(_spriteBatch, playerIdle, player.Size, SpriteEffects.FlipHorizontally, 0f, 64, percent);
                            break;
                        case PlayerStates.crouchLeft:
                            playerAnimation.Draw(_spriteBatch, playerCrouch, player.Size, SpriteEffects.FlipHorizontally, 0f, 64, percent);
                            break;
                        case PlayerStates.crouchRight:
                            playerAnimation.Draw(_spriteBatch, playerCrouch, player.Size, SpriteEffects.None, 0f, 64, percent);
                            break;
                        case PlayerStates.airdash:

                            SpriteEffects flipSprite = new SpriteEffects();
                            int rotation = 0;
                            Texture2D sprite = playerDash;

                            switch (player.MoveDirection)
                            {
                                case Keys.W:
                                    flipSprite = SpriteEffects.None;
                                    rotation = 0;
                                    sprite = playerDashAlt;
                                    break;

                                case Keys.S:
                                    flipSprite = SpriteEffects.FlipVertically;
                                    rotation = 0;
                                    sprite = playerDashAlt;
                                    break;

                                case Keys.A:
                                    flipSprite = SpriteEffects.FlipHorizontally;
                                    rotation = 0;
                                    sprite = playerDash;
                                    break;

                                case Keys.D:
                                    flipSprite = SpriteEffects.None;
                                    rotation = 0;
                                    sprite = playerDash;
                                    break;
                            }

                            playerAnimation.Draw(_spriteBatch, sprite, player.Size, flipSprite, rotation, 64, percent);

                            break;
                    }

                    foreach (Enemy enemy in eManager.Enemies)
                    {
                        if (enemy is BounceEnemy)
                        {
                            BounceEnemy tempEnemy = (BounceEnemy)enemy;
                            if (tempEnemy.PositionY == PositionY.PositiveY)
                            {
                                if (tempEnemy.PositionX == PositionX.PositiveX)
                                {
                                    enemy.Animation.Draw(_spriteBatch, enemyBounceFlip, enemy.Size, SpriteEffects.None, bounceRotate, 64, 1f);
                                }
                                else
                                {
                                    enemy.Animation.Draw(_spriteBatch, enemyBounceFlip, enemy.Size, SpriteEffects.FlipHorizontally, bounceRotate, 64, 1f);
                                }
                            }
                            else
                            {
                                if (tempEnemy.PositionX == PositionX.PositiveX)
                                {
                                    enemy.Animation.Draw(_spriteBatch, enemyBounce, enemy.Size, SpriteEffects.FlipHorizontally, bounceRotate, 64, 1f);
                                }
                                else
                                {
                                    enemy.Animation.Draw(_spriteBatch, enemyBounce, enemy.Size, SpriteEffects.None, bounceRotate, 64, 1f);
                                }
                            }
                        }
                        else if (enemy is BulletEnemy)
                        {
                            enemy.Animation.Draw(_spriteBatch, enemyBullet, enemy.Size, SpriteEffects.None, 0, 64, 1f);
                        }
                        else if (enemy is FollowEnemy)
                        {
                            enemy.Animation.Draw(_spriteBatch, enemyFollow, enemy.Size, SpriteEffects.None, 0, 64, 1f);
                        }
                        else if (enemy is LaserEnemy)
                        {
                            enemy.Animation.Draw(_spriteBatch, enemyLaser, enemy.Size, SpriteEffects.None, 0, enemy.Size.Width, 1f);
                        }
                        else if (enemy is Enemy)
                        {
                            EnemyStates enemyState = enemy.State;
                            switch (enemyState)
                            {
                                case (EnemyStates.moveRight):
                                    enemy.Animation.Draw(_spriteBatch, enemyBase, enemy.Size, SpriteEffects.FlipHorizontally, 0, 64, 1f);
                                    break;
                                case (EnemyStates.moveLeft):
                                    enemy.Animation.Draw(_spriteBatch, enemyBase, enemy.Size, SpriteEffects.None, 0, 64, 1f);
                                    break;
                            }
                        }
                    }

                    shadeManager.Draw(_spriteBatch, Shade, new Rectangle(0, 0, Shade.Width, Shade.Height), SpriteEffects.None, 0, windowWidth, 1f);

                    //if you win fades to black and sends message
                    if (state == GameState.GameWinner)
                    {
                        //fade
                        if (fading)
                        {
                            _spriteBatch.Draw(black, new Vector2(0, 0), null, new Color(0, 0, 0, fadeOpacity), 0f, Vector2.Zero, new Vector2(windowWidth, windowHeight), SpriteEffects.None, 0);
                        }

                        if (fadeOpacity >= 255)
                        {
                            _spriteBatch.DrawString(titleFont, "Surprisingly you're here, where no one else has been\n" +
                                "Thank you for taking the time to play this and congratuations\n" +
                                "with love from Copious Cats",
                                new Vector2(150, 200), Color.White);
                        }
                        if (timer >= 370)
                        {
                            _spriteBatch.DrawString(displayFont, "Press Escape to close the game, theres nothing more here, please leave, go touch grass or something.",
                            new Vector2(100, 400), Color.White);
                        }
                        
                    }

                    break;

                // Text for game over state
                case GameState.GameOver:
                    _spriteBatch.Draw(arenaBackground, new Vector2(0, 0), Color.White);
                    clockHourAnimation.Draw(_spriteBatch, clockHour, new Rectangle(0, 0, windowWidth, windowHeight), SpriteEffects.None, 0f, windowWidth, 1f);
                    if (clockTimer <= 15 || clockTimer > 30)
                    {
                        clockMin1Animation.Draw(_spriteBatch, clockMinute1, new Rectangle(0, 0, windowWidth, windowHeight), SpriteEffects.None, 0f, windowWidth, 1f);
                    }
                    else if (clockTimer < 30)
                    {
                        clockMin2Animation.Draw(_spriteBatch, clockMinute2, new Rectangle(0, 0, windowWidth, windowHeight), SpriteEffects.None, 0f, windowWidth, 1f);
                    }
                    _spriteBatch.Draw(clockTop, new Rectangle(50, 50, windowWidth - 100, windowHeight - 100), Color.White);
                    // Draw the tiles
                    levels[lvlNum].Draw(_spriteBatch, tileSprites);

                    //bars
                    _spriteBatch.Draw(bar, new Rectangle(75, 10, bar.Width / 2, bar.Height / 2), Color.White);
                    _spriteBatch.Draw(healthSprite, new Vector2(0, 0), Color.White);

                    _spriteBatch.Draw(bar, new Rectangle(windowWidth - (bar.Width / 2) - 70, 10, bar.Width / 2, bar.Height / 2), Color.White);
                    _spriteBatch.Draw(energySprite, new Vector2(windowWidth - energySprite.Width, 0), Color.White);

                    shadeManager.Draw(_spriteBatch, Shade, new Rectangle(0, 0, Shade.Width, Shade.Height), SpriteEffects.None, 0, windowWidth, 1f);

                    if (fading)
                    {
                        _spriteBatch.Draw(black, new Vector2(0, 0), null, new Color(0, 0, 0, fadeOpacity), 0f, Vector2.Zero, new Vector2(windowWidth, windowHeight), SpriteEffects.None, 0);
                    }

                    break;

                //text for shop screen
                case GameState.Shop:

                    _spriteBatch.Draw(shopWall, new Vector2(0, 0), Color.White);
                    //_spriteBatch.Draw(shopKeeper, new Vector2(450, 100), Color.White);
                    shopKeeperAnimation.Draw(_spriteBatch, shopKeeper, new Rectangle(450, 100, shopKeeper.Width / 14, shopKeeper.Height), SpriteEffects.None, 0f, shopKeeper.Width/14, 1f);
                    _spriteBatch.Draw(shopFG, new Vector2(0, 0), Color.White);
                    catAnimation.Draw(_spriteBatch, cat, new Rectangle(0, 0, windowWidth, windowHeight), SpriteEffects.None, 0f, windowWidth, 1f);
                    //draws stats
                    shop.Draw(_spriteBatch, displayFont);

                    _spriteBatch.DrawString(displayFont, String.Format("\nCoins: ${0}", shop.Points) +
                        $"\nHealth: {player.Health}" +
                        $"\n{String.Format("Speed: {0:0.00}", player.BaseSpeed)}" +
                        $"\nEnergy: {player.Energy}\n" +
                        $"\nDeaths: {player.Deaths}" +
                        $"\n{String.Format("Best Time: {0:0.00}", player.BestTime)}" +
                        $"\nLifetime Spendings: ${shop.Spendings}",
                       new Vector2(40, 100), Color.White);

                    _spriteBatch.Draw(textBox, new Rectangle(270, 175, textBox.Width + 100, textBox.Height + 20), Color.White);
                    _spriteBatch.DrawString(displayFont, comment, new Vector2(300, 190), Color.Black);

                    _spriteBatch.DrawString(displayFont, "Press Enter to return to the arena\nPress Q to quit to the menu",
                        new Vector2(40, 400), Color.White);

                    //sign and shelves
                    _spriteBatch.Draw(sign, new Vector2(1150, 642), Color.White);
                    _spriteBatch.Draw(shelf, new Vector2(1085, 265), Color.White);
                    _spriteBatch.Draw(shelf, new Vector2(1085, 505), Color.White);

                    
                    //draws each button
                    for (int i = 3; i < buttons.Count; i++)
                    {
                        buttons[i].Draw(_spriteBatch, displayFont, buttonSelect, smallSign);
                        //if the player doesnt have enough points to by the item
                        if (buttons[i].IsHighlight && shop.Points < buttons[i].Cost)
                        {
                            _spriteBatch.DrawString(displayFont, "Sorry hun, \nyou don't have enough \nto buy that.", new Vector2(300, 240), Color.Black);
                        }
                    }
                    break;
            }

            //closes the spriteBatch before calling draw
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}


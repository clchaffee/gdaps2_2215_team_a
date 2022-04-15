using Microsoft.Xna.Framework;
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
        private int Interval { get; set; } = 0;
        private bool easy = true;
        private bool medium = false;
        private bool hard = false;
        private bool impossible = false;
        private bool collidable = false;

        int count;
        bool spawnCap = true;
        int waitTime;

        // player assets
        private Texture2D playerSprites;
        private Player player;
        private int pStartX;
        private int pStartY;

        bool isCollidingUp;
        bool isCollidingDown;
        bool isCollidingRight;
        bool isCollidingLeft;

        // enemy assets
        private Texture2D enemySprites;

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

        //variables for the shop
        private Shop shop;
        private int points = 0;
        private List<Button> buttons = new List<Button>();
        private Texture2D buttonTexture;
        private Texture2D shopWall;
        private Texture2D shopFG;
        private Texture2D shopKeeper;
        private Texture2D noseButton;
        private string comment;

        //other buttons
        private Texture2D startButton;
        private Texture2D optionButton;
        private Texture2D menuButton;

        //items
        private Texture2D healthUpgrade;
        private Texture2D speedUpgrade;
        private Texture2D energyUpgrade;

        // Level Assets
        private LevelEditor editor;
        private Texture2D tileSprites;
        private Tile tile;
        private List<LevelEditor> levels;
        private int lvlNum;

        // Other Assets
        private Texture2D arenaBackground;
        private Texture2D titleBG;

        //sets the default state as the menu
        GameState state = GameState.Menu;

        //keyboard & Mouse States
        KeyboardState kbState;
        KeyboardState prevKbState = Keyboard.GetState();
        private MouseState mouseState;
        private MouseState prevMouseState;

        //variables
        double timer = 0;
        int energyTimer = 0;
        int stoppedTimer = 0;

        // Animation Fields
        AnimationManager playerAnimation;
        Texture2D playerIdle;
        Texture2D playerWalk;
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

            //other assests and buttons
            tileSprites = Content.Load<Texture2D>("brick");
            titleScreen = Content.Load<Texture2D>("Logo (1)");
            titleBG = Content.Load<Texture2D>("tempTS");
            arenaBackground = Content.Load<Texture2D>("ArenaBG");
            shopWall = Content.Load<Texture2D>("ShopWall");
            shopFG = Content.Load<Texture2D>("ShopFG");
            shopKeeper = Content.Load<Texture2D>("ShopKeeper");
            noseButton = Content.Load<Texture2D>("CatNose");

            startButton = Content.Load<Texture2D>("Start");
            optionButton = Content.Load<Texture2D>("Options");
            menuButton = Content.Load<Texture2D>("Menu");

            healthUpgrade = Content.Load<Texture2D>("HealthBottle");
            speedUpgrade = Content.Load<Texture2D>("SpeedBottle");
            energyUpgrade = Content.Load<Texture2D>("EnergyBottle");

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

            //eSize.X = rng.Next(300, windowWidth - 300);
            //eSize.Y = rng.Next(300, windowHeight - 300);

            // ENEMY STUFF
            eManager.Initialize();
            //enemy = new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(128, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight);
            //eManager.SpawnEnemy(enemy);
            //bEnemy = new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight);
            //eManager.SpawnEnemy(bEnemy);
            //pEnemy = new BounceEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight);
            //eManager.SpawnEnemy(pEnemy);
            //fEnemy = new FollowEnemy(enemySprites, new Rectangle(64, 64, 64, 64), windowWidth, windowHeight);
            //eManager.SpawnEnemy(bEnemy);
            //lEnemy = new LaserEnemy(buttonTexture, new Rectangle(0, 0, 64, 128), windowWidth, windowHeight, player.Size.Y);
            //eManager.SpawnEnemy(lEnemy);


            // -- LEVEL LOADING --
            levels = new List<LevelEditor>();
            for (int i = 0; i < 6; i++)
            {
                levels.Add(new LevelEditor());
                levels[i].Load(i+1, tileSprites, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }
            lvlNum = 0;

            //makes a new shop and buttons for each of the purchases
            shop = new Shop(points);

            //other buttons at the start of the list for the start screen
            buttons.Add(new Button("start", startButton, new Rectangle(windowWidth / 2 - 256 / 2, 450, 256, 124), 0));
            buttons.Add(new Button("options", optionButton, new Rectangle(windowWidth / 2 - 256 / 2, 600, 256, 124), 0));
            buttons.Add(new Button("menu", menuButton, new Rectangle(300, 800, 256, 124), 0));

            buttons.Add(new Button("health",
                healthUpgrade,
                new Rectangle(1100, 150, healthUpgrade.Width, healthUpgrade.Height),
                10));

            buttons.Add(new Button("speed",
                speedUpgrade,
                new Rectangle(1250, 150, speedUpgrade.Width, speedUpgrade.Height),
                10));

            buttons.Add(new Button("energy",
                energyUpgrade,
                new Rectangle(1400, 150, energyUpgrade.Width, energyUpgrade.Height),
                10));

            buttons.Add(new Button("dash",
                buttonTexture,
                new Rectangle(1100, 400, 100, 50),
                50));

            buttons.Add(new Button("timestop",
                  buttonTexture,
                  new Rectangle(1400, 400, 100, 50),
                  100));

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
                    if (buttons[1].IsPressed)
                    {
                        state = GameState.Controls;
                    }

                    buttons[0].Update(gameTime);
                    buttons[1].Update(gameTime);


                    playerAnimation.Update(gameTime, 3, .09);
                    break;

                //while in the control screen, press enter to return to the menu
                case GameState.Controls:
                    if (buttons[2].IsPressed)
                    {
                        state = GameState.Menu;
                    }
                    if (kbState.IsKeyDown(Keys.D1) && prevKbState.IsKeyUp(Keys.D1))
                    {
                        easy = true;
                        medium = false;
                        hard = false;
                        impossible = false;
                    }
                    if (kbState.IsKeyDown(Keys.D2) && prevKbState.IsKeyUp(Keys.D2))
                    {
                        easy = false;
                        medium = true;
                        hard = false;
                        impossible = false;
                    }
                    if (kbState.IsKeyDown(Keys.D3) && prevKbState.IsKeyUp(Keys.D3))
                    {
                        easy = false;
                        medium = false;
                        hard = true;
                        impossible = false;
                    }
                    if (kbState.IsKeyDown(Keys.D0) && prevKbState.IsKeyUp(Keys.D0))
                    {
                        easy = false;
                        medium = false;
                        hard = false;
                        impossible = true;
                    }

                    buttons[2].Update(gameTime);

                    break;

                //start animation
                case GameState.Start:

                    playerAnimation.Update(gameTime, 8, .09);
                    player.SizeX = player.SizeX + 10;
                    if (player.SizeX > _graphics.PreferredBackBufferWidth)
                    {
                        state = GameState.Arena;
                    }

                    break;

                // when in the arena, "dies" when you press space, entering the shop
                case GameState.Arena:

                    //eManager.FirstWave();
                    timer = timer + gameTime.ElapsedGameTime.TotalSeconds;

                    //updates levels every 2 minutes, would use % but game time is too fast to process that
                    // REMOVE /10 AT THE END
                    if (timer >= 600/10)
                    {
                        lvlNum = 5;
                    }
                    else if (timer >= 480/10)
                    {
                        lvlNum = 4;
                    }
                    else if (timer >= 360/10)
                    {
                        lvlNum = 3;
                    }
                    else if (timer >= 240/10)
                    {
                        lvlNum = 2;
                    }
                    else if (timer >= 120/10)
                    {
                        lvlNum = 1;
                    }

                    // Temp player and enemy update call
                    player.Update(gameTime);

                    playerState = player.State;

                    switch(playerState)
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
                    }


                    // Increment the player's energy if it is currently under the maximum
                    if (player.CurrentEnergy < player.Energy)
                    {
                        if (energyTimer > 60)
                        {
                            player.CurrentEnergy++;
                            energyTimer = 0;
                        }
                        else
                        {
                            energyTimer++;
                        }
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
                                // Check for top collisions
                                if (player.IsCollidingTop(player, editor[i, j]) &&
                                    (!isCollidingUp))
                                {
                                    while (player.Size.Bottom != editor[i, j].Size.Top)
                                    {
                                        player.SizeY -= 1;
                                    }

                                    player.VelocityY = 0;
                                    player.PositionY = player.SizeY;

                                    player.IsGrounded = true;
                                    isCollidingUp = true;
                                }
                                else
                                {
                                    //if (player.Size.Bottom > editor[i, j].Size.Top)
                                    //{
                                    //    isCollidingUp = false;
                                    //}
                                }

                                // Check for bottom collisions
                                if (player.IsCollidingBottom(player, editor[i, j]) &&
                                    (!isCollidingDown))
                                {
                                    while (player.Size.Top != editor[i, j].Size.Bottom)
                                    {
                                        player.SizeY++;
                                    }

                                    player.PositionY = player.SizeY;

                                    player.VelocityY = 0;
                                    player.CanJump = false;
                                    player.IsGrounded = false;
                                    isCollidingDown = true;
                                }
                                else
                                {
                                    
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
                                    if (player.Size.Left > editor[i, j].Size.Right)
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
                                    if (player.Size.Right < editor[i, j].Size.Left)
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

                    //TODO: Reimplement enemy collision 
                    //collision for each enemy in the Enemy class
                    if (collidable == true)
                    foreach (Enemy enemy in eManager.Enemies)
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
                                }

                            }
                            else if (enemy.IsCollidingTop(enemy, player))
                            {
                                //player.Jump();
                            }
                        }
                    }
                    else
                    {
                        
                    }

                    // Temp player and enemy update call
                    //bEnemy.Update(gameTime);
                    //lEnemy.Update(gameTime, player.Size.Y);
                    //pEnemy.Update(gameTime);
                    //fEnemy.Update(gameTime, player);

                    if (!player.TimeStopActive)
                    {
                        foreach (Enemy enemy in eManager.Enemies)
                        {
                            if (enemy is FollowEnemy)
                            {
                                ((FollowEnemy)enemy).Update(gameTime, player);
                            }
                            else if (enemy is LaserEnemy)
                            {
                                ((LaserEnemy)enemy).Update(gameTime, player.Size.Y);
                            }
                            else
                            {
                                enemy.Update(gameTime);
                            }
                        }
                    }

                    //if the count of the list is zero(empty), will automatically add one to it
                    //if (eManager.Enemies.Count == 0)
                    //{
                    //    enemy = new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(128, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight);
                    //    eManager.SpawnEnemy(enemy);
                    //}

                    //adds one to the count in the manager every frame
                    eManager.Count++;

                    // TODO: properly update the spawning method/algorithm

                    if ((int)timer % 5 == 0)
                    {

                        if (spawnCap)
                        {
                            switch (eManager.WaveNum)
                            {
                                //wave 1 always spawns regular enemies
                                case 1:
                                    eManager.SpawnFormula(.1);
                                    //eManager.Enemies.Clear();
                                    for (int i = 0; i < eManager.NumEnemies[Interval]; i++)
                                    {
                                        eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                    }

                                    break;
                                //    if (medium == true)
                                //    {
                                //        eManager.SpawnFormula(.1);
                                //        //eManager.limitation = .1;
                                //        for (int i = 0; i < eManager.numEnemies[Interval]; i++)
                                //        {
                                //            eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(128, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                //        }
                                //    }
                                //    if (hard == true)
                                //    {
                                //        eManager.SpawnFormula(.125);
                                //        //eManager.limitation = .3;
                                //        for (int i = 0; i < eManager.numEnemies[Interval]; i++)
                                //        {
                                //            eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(128, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                //        }
                                //    }
                                //    if (impossible == true)
                                //    {
                                //        eManager.SpawnFormula(.2);
                                //        for (int i = 0; i < eManager.numEnemies[Interval]; i++)
                                //        {
                                //            eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(128, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                //        }
                                //    }
                                //    break;

                                
                                //wave 2 has a 20 percent chance to spawn a bullet enemy
                                case 2:
                                    eManager.SpawnFormula(.1);
                                    //eManager.Enemies.Clear();
                                    for (int i = 0; i < eManager.NumEnemies[Interval]; i++)
                                    {
                                        if (rng.Next(0, 100) > 19)
                                        {
                                            eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                        }
                                        else
                                        {
                                            eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, (int)player.PositionX, (int)player.PositionY), Interval);
                                        }
                                    }

                                    break;

                                //wave 3: 60% for normal, 40% for bullet
                                case 3:
                                    eManager.SpawnFormula(.1);
                                    //eManager.Enemies.Clear();
                                    for (int i = 0; i < eManager.NumEnemies[Interval]; i++)
                                    {
                                        if (rng.Next(0, 100) > 39)
                                        {
                                            eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                        }
                                        else
                                        {
                                            eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                        }
                                    }

                                    break;

                                //wave 4: 40% normal, 40% projectile, 20% bounce
                                case 4:
                                    eManager.SpawnFormula(.1);
                                    //eManager.Enemies.Clear();
                                    for (int i = 0; i < eManager.NumEnemies[Interval]; i++)
                                    {
                                        if (rng.Next(0, 100) < 40)
                                        {
                                            eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(128, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                        }
                                        else if(rng.Next(0, 100) < 40)
                                        {
                                            eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                        }
                                        else
                                        {
                                            eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                        }
                                    }

                                    break;
                            
                                //wave 5: 30% normal, 30% bounce, 40% bullet
                                case 5:
                                    eManager.SpawnFormula(.1);
                                    //eManager.Enemies.Clear();
                                    for (int i = 0; i < eManager.NumEnemies[Interval]; i++)
                                    {
                                        if (rng.Next(0, 100) < 30)
                                        {
                                            eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                        }
                                        else if(rng.Next(0, 100) < 30)
                                        {
                                            eManager.WaveProgress(new BulletEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                        }
                                        else
                                        {
                                            eManager.WaveProgress(new BounceEnemy(enemySprites, new Rectangle(0, 0, 64, 64), windowWidth, windowHeight, player.SizeX, player.SizeY), Interval);
                                        }
                                    }

                                    break;

                                //Wave 6: 40% normal, 20% bounce, 30% bullet, 10% follow
                                case 6:
                                    eManager.SpawnFormula(.1);
                                    //eManager.Enemies.Clear();
                                    for (int i = 0; i < eManager.NumEnemies[Interval]; i++)
                                    {
                                        if (rng.Next(0, 100) < 40)
                                        {
                                            eManager.WaveProgress(new Enemy(enemySprites, new Rectangle(rng.Next(128, windowWidth - 64 - 64), rng.Next(player.SizeY - 192, windowHeight - 64 - 64), 64, 64), windowWidth, windowHeight), Interval);
                                        }
                                        else if(rng.Next(0 ,100) < 30)
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

                                    break;
                                     
                                default:
                                    break;
                            }


                            Interval++;

                            count++;
                            spawnCap = false;
                            waitTime = 59
                                ;
                            if (Interval == 7)
                            {
                                eManager.Start += 5;
                                eManager.End += 5;
                                Interval = 0;
                                eManager.Enemies.Clear();
                                eManager.NumEnemies.Clear();
                                eManager.WaveNum++;
                                spawnCap = true;
                            }
                            //else if (Interval == 7)
                            //{
                            //    eManager.Enemies.Clear();
                            //    Interval = 0;
                            //    wave++;
                            //}
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

                    // If time is stopped, increased the stopped time timer (aka, the stoppedTimer)
                    if (player.TimeStopActive)
                    {
                        if(stoppedTimer < 180)
                        {
                            stoppedTimer++;
                        }
                        else
                        {
                            player.TimeStopActive = false;
                            stoppedTimer = 0;
                        }
                    }

                    break;

                // Game Winner: appears when timer is greater than 30
                case GameState.GameWinner:
                    player.Reset();
                    bEnemy.Reset();
                    foreach (Enemy enemy in eManager.Enemies)
                    {
                        enemy.Reset();
                    }

                    //resets data from Arena
                    eManager.Enemies.Clear();
                    waveLength = 10;
                    waveDelta = 10;
                    eManager.Count = 0;

                    //level reset
                    lvlNum = 0;

                    //you get 5 points per second spent alive in the arena
                    shop.Points += 5 * (int)timer;
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
                    player.Deaths++;
                    foreach (Enemy enemy in eManager.Enemies)
                    {
                        enemy.Reset();
                    }
                    eManager.Start += 0;
                    eManager.End += 30;
                    Interval = 0;
                    eManager.Enemies.Clear();
                    eManager.WaveNum = 1;

                    //level reset
                    lvlNum = 0;

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

                    //sets a new comment
                    comment = shop.Comment(null);

                    Thread.Sleep(500);
                    state = GameState.Shop;

                    break;

                //if enter is pressed in the shop, returns to arena; if space is pressed brings up the menu
                case GameState.Shop:
                    player.Health = shop.MaxHealth;

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
                            if(button.Type != "dash" && button.Type !="timestop")
                            {
                                shop.Points -= button.Cost;
                                shop.Spendings += button.Cost;
                            }
                            else if(button.Type == "dash" && !shop.AirDash)
                            {
                                shop.Points -= button.Cost;
                                shop.Spendings += button.Cost;
                            }
                            else if(button.Type == "timestop" && !shop.TimeSlow)
                            {
                                shop.Points -= button.Cost;
                                shop.Spendings += button.Cost;
                            }
                            //switch statement for each button to increase cost
                            switch (button.Type)
                            {
                                case "health":
                                    button.Cost += 10;
                                    shop.MaxHealth += 1;
                                    player.MaxHealth += 1;
                                    break;

                                case "speed":
                                    button.Cost += 20;
                                    player.BaseSpeed += 0.05f;
                                    break;

                                case "energy":
                                    button.Cost += 10;
                                    player.Energy += 2f;
                                    break;

                                case "dash":
                                    player.dashPurchased = true;
                                    shop.AirDash = true;
                                    break;

                                case "timestop":
                                    player.timeStopPurchased = true;
                                    shop.TimeSlow = true;
                                    break;

                                case "heal":
                                    button.Cost += 10;
                                    break;

                                case "slow":
                                    button.Cost += 10;
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
                    _spriteBatch.Draw(titleBG, new Rectangle(0, 0, titleBG.Width * 4, titleBG.Height * 4), Color.White);
                    //_spriteBatch.Draw(titleScreen, new Rectangle((windowWidth/2 - titleScreen.Width/2 - 250), (windowHeight/2 - titleScreen.Height/2 - 200), 1500, 750), Color.White);

                    _spriteBatch.DrawString(displayFont, $"Frame: {playerAnimation.Frames}",
                        new Vector2(10, 10), Color.White);
                    _spriteBatch.DrawString(displayFont, $"Current Time: {playerAnimation.CurrentTime}",
                        new Vector2(10, 50), Color.White);

                    //player.Draw(_spriteBatch, playerSprites);
                    switch (state)
                    {
                        case GameState.Menu:
                            playerAnimation.Draw(_spriteBatch, playerIdle, player.Size, SpriteEffects.None);
                            break;

                        case GameState.Start:
                            playerAnimation.Draw(_spriteBatch, playerWalk, player.Size, SpriteEffects.None);
                            break;
                    }

                    //draw buttons
                    buttons[0].Draw(_spriteBatch, displayFont);
                    buttons[1].Draw(_spriteBatch, displayFont);

                    player.Draw(_spriteBatch, playerSprites);
                    break;

                // text for control screen
                case GameState.Controls:
                    _spriteBatch.DrawString(titleFont, "Press W to Jump\nPress A to Move Left\nPress D to Move Right" +
                        "\nPress Right Shift and a direction to airdash (WHEN UNLOCKED)" +
                        "\nPress Space to stop time for a short period (WHEN UNLOCKED)\n",
                        new Vector2(100, 50), Color.Black);

                    //changes color based on what difficulty was selected
                    if (easy)
                    {
                        _spriteBatch.DrawString(displayFont, "Press 1 for Easy Difficulty",
                            new Vector2(100, 500), Color.Green);
                    }
                    else
                    {
                        _spriteBatch.DrawString(displayFont, "Press 1 for Easy Difficulty",
                            new Vector2(100, 500), Color.Black);
                    }
                    if (medium)
                    {
                        _spriteBatch.DrawString(displayFont, "Press 2 for Medium Difficulty",
                            new Vector2(100, 525), Color.Green);
                    }
                    else
                    {
                        _spriteBatch.DrawString(displayFont, "Press 2 for Medium Difficulty",
                            new Vector2(100, 525), Color.Black);
                    }
                    if (hard)
                    {
                        _spriteBatch.DrawString(displayFont, "Press 3 for Hard Difficulty",
                            new Vector2(100, 550), Color.Green);
                    }
                    else
                    {
                        _spriteBatch.DrawString(displayFont, "Press 3 for Hard Difficulty",
                            new Vector2(100, 550), Color.Black);
                    }
                    if (impossible)
                    {
                        _spriteBatch.DrawString(displayFont, "Press 0 for IMPOSSIBLE Difficulty  (You WILL NOT survive.)",
                            new Vector2(100, 575), Color.Green);
                    }
                    else
                    {
                        _spriteBatch.DrawString(displayFont, "Press 0 for IMPOSSIBLE Difficulty  (You WILL NOT survive.)",
                            new Vector2(100, 575), Color.Black);
                    }

                    buttons[2].Draw(_spriteBatch, displayFont);

                    break;

                //text for arena screen
                case GameState.Arena:

                    _spriteBatch.Draw(arenaBackground, new Vector2(0,0), Color.White);
                    // Draw the tiles
                    levels[lvlNum].Draw(_spriteBatch, tileSprites);

                    _spriteBatch.DrawString(displayFont, $"\nTime Passed: {String.Format("{0:0.00}", timer)}",
                        new Vector2(100, 150), Color.LightGray);
                    _spriteBatch.DrawString(displayFont, $"\nPlayer Health: {player.Health}",
                       new Vector2(100, 50), Color.LightGray);
                    _spriteBatch.DrawString(displayFont, $"\nEnergy: {player.CurrentEnergy}",
                       new Vector2(100, 100), Color.LightGray);

                    _spriteBatch.DrawString(displayFont, $"\nWave: {eManager.WaveNum}",
                        new Vector2(100, 200), Color.Black);
                    _spriteBatch.DrawString(displayFont, $"\n# of enemies in wave: {eManager.Enemies.Count}",
                        new Vector2(100, 250), Color.LightGray);

                    // Temp player draw call (should, in theory, be handled by the animation manager later down the line)
                    //player.Draw(_spriteBatch, playerSprites);

                    switch (playerState)
                    {
                        case PlayerStates.moveRight:
                        case PlayerStates.jumpRight:
                            playerAnimation.Draw(_spriteBatch, playerWalk, player.Size, SpriteEffects.None);
                            break;
                        case PlayerStates.moveLeft:
                        case PlayerStates.jumpLeft:
                            playerAnimation.Draw(_spriteBatch, playerWalk, player.Size, SpriteEffects.FlipHorizontally);
                            break;
                        case PlayerStates.faceRight:
                            playerAnimation.Draw(_spriteBatch, playerIdle, player.Size, SpriteEffects.None);
                            break;
                        case PlayerStates.faceLeft:
                            playerAnimation.Draw(_spriteBatch, playerIdle, player.Size, SpriteEffects.FlipHorizontally);
                            break;
                    }

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
                    _spriteBatch.Draw(arenaBackground, new Vector2(0,0), Color.White);
                    // Draw the tiles
                    levels[lvlNum].Draw(_spriteBatch, tileSprites);

                    _spriteBatch.DrawString(displayFont, "Go to the shop page (happens upon character death)",
                        new Vector2(100, 400), Color.LightGray);
                    _spriteBatch.DrawString(displayFont, $"\nTime Passed: {String.Format("{0:0.00}", timer)}",
                       new Vector2(100, 150), Color.LightGray);
                    _spriteBatch.DrawString(displayFont, $"\nPlayer Health: {player.Health}",
                       new Vector2(100, 100), Color.LightGray);
                    break;

                //text for shop screen
                case GameState.Shop:

                    _spriteBatch.Draw(shopWall, new Vector2(0, 0), Color.White);
                    _spriteBatch.Draw(shopKeeper, new Vector2(450, 100), Color.White);
                    _spriteBatch.Draw(shopFG, new Vector2(0, 0), Color.White);
                    //draws stats
                    shop.Draw(_spriteBatch, displayFont);

                    _spriteBatch.DrawString(displayFont, $"\nKromer: {shop.Points} " +
                        $"\nHealth: {player.Health}" +
                        $"\n{String.Format("Speed: {0:0.00}", player.BaseSpeed)}" +
                        $"\nEnergy: {player.Energy}\n" +
                        $"\nDeaths: {player.Deaths}" +
                        $"\n{String.Format("Best Time: {0:0.00}", player.BestTime)}" +
                        $"\nSpendings: {shop.Spendings}",
                       new Vector2(40, 100), Color.White);

                    _spriteBatch.DrawString(displayFont, comment, new Vector2(450, 200), Color.LightGray);

                    _spriteBatch.DrawString(displayFont, "Press Enter to return to the arena\nPress Q to quit to the menu",
                        new Vector2(40, 400), Color.White);

                    //draws each button
                    for (int i = 3; i < buttons.Count;i++)
                    {
                            buttons[i].Draw(_spriteBatch, displayFont);
                            //if the player doesnt have enough points to by the item
                            if (buttons[i].IsHighlight && shop.Points < buttons[i].Cost)
                            {
                                _spriteBatch.DrawString(displayFont, "Sorry hun, you don't have enough to buy that.", new Vector2(600, 80), Color.White);
                            }
                    }
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


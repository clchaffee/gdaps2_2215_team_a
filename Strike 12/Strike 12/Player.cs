using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
/// <summary>
/// Author: Copious Cats 
/// Purpose: This is a class that will inherit from the GameObject 
/// class and will hold all info that pretains to the player
/// </summary>
namespace Strike_12
{
    // THIS IS THE CORRECT ONE

    /// <summary>
    /// This enum represents the various states that the player can be in 
    /// </summary>
    enum PlayerStates
    {
        moveLeft,
        moveRight,
        faceLeft,
        faceRight,
        jumpLeft,
        jumpRight,
        crouch,
        crouchLeft,
        crouchRight,
        airdash,
        airdashLeft,
        airdashRight
    }

    class Player : GameObject
    {
        // ----- | Fields | -----
        //parameters for size and position for the player not currently decided

        // ----- | Fields | -----
        //player states
        private PlayerStates playerState;
        private PlayerStates previousPlayerState;
        private PlayerStates dashDirection;

        //player information
        private Texture2D playerSprite;
        private float baseSpeed = 1f;
        private float moveSpeed = 8f;
        private int maxHealth = 10;
        private int health = 10;
        private float energy = 20f;
        private float maxEnergy = 20f;
        private int deaths = 0;
        private float bestTime = 0f;

        //other
        private int dashCounter = 20;
        private Keys moveDirection;
        private int iCounter = 0;
        private Rectangle platformPos;
        private bool timeStopActive = false;
        private int timeStopCooldown = 0;
        private int buffer = 20;
        private double lastJump = 0;

        //fields for gravity
        private float gravityMultiplier = 1f;
        protected Vector2 position;
        protected Vector2 velocity;
        protected bool isGrounded;

        // Other
        protected bool isCrouching;

        // Collision Fields
        protected bool collided;
        protected bool leftCollided;
        protected bool rightCollided;
        protected Rectangle wallPos;
        protected bool canJump = true;

        // Input Fields
        public KeyboardState kbState;
        private KeyboardState previousKBState;

        //shop related functions
        public bool dashPurchased = false;
        public bool timeStopPurchased = false;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public float BaseSpeed
        {
            get { return baseSpeed; }
            set { baseSpeed = value; }
        }

        public float Energy
        {
            get { return maxEnergy; }
            set { maxEnergy = value; }
        }
        public float CurrentEnergy
        {
            get { return energy; }
            set { energy = value; }
        }

        public int Deaths
        {
            get { return deaths; }
            set { deaths = value; }
        }

        public float BestTime
        {
            get { return bestTime; }
            set { bestTime = value; }
        }

        public int PlatformPosY
        {
            get { return platformPos.Y; }
            set { platformPos.Y = value; }
        }

        public int PlatformPosX
        {
            get { return platformPos.X; }
            set { platformPos.X = value; }
        }

        public int WallPosX
        {
            get { return wallPos.X; }
            set { wallPos.X = value; }
        }

        public bool IsGrounded
        {
            get { return isGrounded; }
            set { isGrounded = value; }
        }

        public bool LeftCollided
        {
            get { return leftCollided; }
            set { leftCollided = value; }
        }

        public bool RightCollided
        {
            get { return rightCollided; }
            set { rightCollided = value; }
        }

        public bool CanJump
        {
            get { return canJump; }
            set { canJump = value; }
        }

        public bool TimeStopActive
        {
            get { return timeStopActive; }
            set { timeStopActive = value; }
        }

        public PlayerStates State
        {
            get { return playerState; }
        }

        // ------------------------------------------

        public float VelocityX
        {
            get { return velocity.X; }
            set { velocity.X = value; }
        }

        public int VelocityY
        {
            get { return (int)velocity.Y; }
            set { velocity.Y = value; }
        }

        public float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        // ----- | Constructor | -----
        public Player(Texture2D texture, Rectangle size, int windowWidth, int windowHeight,
            Vector2 position) : base(texture, size, windowWidth, windowHeight)

        {
            this.playerSprite = texture;
            this.position = position;
            this.size = size;
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            isGrounded = false;
            isCrouching = false;
            playerState = PlayerStates.faceRight;
        }


        /// <summary>
        /// if the counter reaches 60, aka after 60 frames have passes (1 second), returns true,
        /// indicating the player is able to take damage again
        /// if returns true, the player is able to be damaged
        /// if returns false, the player cannot be hit
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool TakeDamage(GameTime gameTime)
        {
            iCounter++;
            if (iCounter == 10)
            {
                iCounter = 0;
                return true;
            }
            else
            {
                return false;
            }

        }

        // -- Methods Overriden from parent class -- 

        /// <summary>
        /// For testing purposes, I have been using this update method, as our collision detection testing currently
        /// relies on a value for the platform position which must be passed into it
        /// 
        /// In the future, this will be replaced with the actual update method, however, for the time being I request
        /// that this remain until everything is properly moved over
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            //sets current keyboard state
            kbState = Keyboard.GetState();

            // If the player is in the airdash state,
            if (playerState == PlayerStates.airdash)
            {
                // Air Dash Logic
                if (dashCounter > 0)
                {
                    switch (moveDirection)
                    {
                        case Keys.Space:
                            size.Y -= 20;
                            position.Y -= 20f;
                            break;

                        case Keys.A:
                            size.X -= 20;
                            position.X -= 20f;
                            break;

                        case Keys.S:
                            size.Y += 20;
                            position.Y += 20f;
                            break;

                        case Keys.D:
                            size.X += 20;
                            position.X += 20f;
                            break;

                        default:
                            break;
                    }

                    dashCounter--;
                }
                else
                {
                    // Reset the player's height and width
                    if (moveDirection == Keys.A || moveDirection == Keys.D)
                    {
                        size = new Rectangle(size.X, size.Y, size.Height, size.Width);
                    }

                    playerState = dashDirection;
                }
            }

            if (!isCrouching)
            {
                //if A is pressed, moves player left, changes player state depending on if jumping or not.
                if (kbState.IsKeyDown(Keys.A) && !leftCollided && playerState != PlayerStates.airdash)
                {
                    velocity.X = -(baseSpeed * moveSpeed);

                    if (!isGrounded && previousPlayerState != PlayerStates.moveLeft)
                    {
                        playerState = PlayerStates.jumpLeft;
                    }
                    else
                    {
                        playerState = PlayerStates.moveLeft;
                    }
                }
                //if D is pressed, moves player right, changes player state depending on if jumping or not.
                else if (kbState.IsKeyDown(Keys.D) && !rightCollided && playerState != PlayerStates.airdash)
                {
                    velocity.X = (baseSpeed * moveSpeed);

                    if (!isGrounded && previousPlayerState != PlayerStates.moveRight)
                    {
                        playerState = PlayerStates.jumpRight;
                    }
                    else
                    {

                        playerState = PlayerStates.moveRight;
                    }
                }
                //otherwise x velocity is zero (doesn't move left or right) and updates player state accordingly.
                else
                {
                    velocity.X = 0f;

                    if (isGrounded && (previousPlayerState == PlayerStates.moveRight ||
                        previousPlayerState == PlayerStates.jumpRight))
                    {
                        playerState = PlayerStates.faceRight;
                    }
                    else if (isGrounded && (previousPlayerState == PlayerStates.moveLeft ||
                        previousPlayerState == PlayerStates.jumpLeft))
                    {
                        playerState = PlayerStates.faceLeft;
                    }
                }
            }


            // If W is pressed, player jumps, with addition of velocity gravity, and updates player state accordingly
            if (previousKBState.IsKeyUp(Keys.Space) && kbState.IsKeyDown(Keys.Space) && !isCrouching)
            {
                lastJump = gameTime.TotalGameTime.TotalMinutes;

                //if (velocity.Y == 0)
                //{
                //    canJump = false;
                //}

                if (/*VelocityY == 0*/ isGrounded && (lastJump + buffer > gameTime.TotalGameTime.TotalMinutes) && canJump)
                {
                    canJump = false;
                    isGrounded = false;
                    position.Y -= 60f;
                    velocity.Y = -20f;

                    if (previousPlayerState == PlayerStates.faceRight || previousPlayerState == PlayerStates.jumpRight)
                    {
                        playerState = PlayerStates.jumpRight;
                    }
                    else if (previousPlayerState == PlayerStates.faceLeft || previousPlayerState == PlayerStates.jumpLeft)
                    {
                        playerState = PlayerStates.jumpLeft;
                    }
                }
                else
                {
                    canJump = true;
                }
            }

            // Crouching
            if (kbState.IsKeyDown(Keys.S) && !previousKBState.IsKeyDown(Keys.S) /*&& isGrounded*/)
            {
                size = new Rectangle(size.X, size.Y, size.Width, 64);
                position.Y += 64;

                switch (playerState)
                {
                    case PlayerStates.moveRight:
                    case PlayerStates.jumpRight:
                    case PlayerStates.faceRight:
                        playerState = PlayerStates.crouchRight;
                        break;

                    case PlayerStates.moveLeft:
                    case PlayerStates.jumpLeft:
                    case PlayerStates.faceLeft:
                        playerState = PlayerStates.crouchLeft;
                        break;
                }

                isCrouching = true;
            }
            else if (!kbState.IsKeyDown(Keys.S) && previousKBState.IsKeyDown(Keys.S))
            {
                size = new Rectangle(size.X, size.Y, size.Width, 128);

                if (playerState == PlayerStates.crouchRight)
                {
                    playerState = PlayerStates.faceRight;
                }
                else
                {
                    playerState = PlayerStates.faceLeft;
                }

                isCrouching = false;
            }

            if (isCrouching && isGrounded && VelocityX != 0)
            {
                if (VelocityX > 0)
                {
                    VelocityX--;
                    if (VelocityX < 1)
                    {
                        VelocityX = 0;
                    }
                }
                else
                {
                    VelocityX++;
                    if (VelocityX > -1)
                    {
                        VelocityX = 0;
                    }
                }
            }

            // Grounded Player Logics
            if (isGrounded)
            {
                //position.Y = this.SizeY - playerSprite.Height;
                gravityMultiplier = 1f;
                moveSpeed = baseSpeed * 10f;
                dashCounter = 20;
                velocity.Y = 0;
                canJump = true;
            }
            //if the player is no longer on the ground, applies gravity
            else if (!isGrounded && playerState != PlayerStates.airdash)
            {
                // While the gravity multipler is under a specified value, add to it
                if (gravityMultiplier < 5)
                {
                    gravityMultiplier += 1f;
                }

                // If the player is falling, lower their movespeed to allow for precise landing
                if (velocity.Y <= 10 && velocity.Y >= 0)
                {
                    moveSpeed += baseSpeed * 0.3f;
                }
                else if (velocity.Y > 0)
                {
                    moveSpeed = baseSpeed * 8.7f;
                }

                // Update the player's Y velocity according to the multiplier
                velocity.Y += 0.15f * gravityMultiplier;

                // Timestop Check
                if (kbState.IsKeyDown(Keys.Q) &&
                    timeStopPurchased &&
                    !timeStopActive &&
                    timeStopCooldown > 250)
                {
                    timeStopActive = true;
                    timeStopCooldown = 0;
                }
                else
                {
                    timeStopCooldown++;
                    if (timeStopCooldown > 250)
                    {
                        timeStopActive = false;
                    }
                }

                // Air Dash
                if (kbState.IsKeyDown(Keys.LeftShift) &&
                    dashCounter > 0 &&
                    energy - 5 >= 0 &&
                    playerState != PlayerStates.airdash &&
                    !isGrounded &&
                    dashPurchased)
                {
                    energy -= 5;
                    velocity.Y = 0;
                    velocity.X = 0;
                    size.X += 0;
                    size.Y += 0;
                    dashDirection = playerState;

                    if (kbState.IsKeyDown(Keys.W))
                    {
                        moveDirection = Keys.W;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        size = new Rectangle(size.X, size.Y, size.Height, size.Width);
                        moveDirection = Keys.A;
                    }
                    else if (kbState.IsKeyDown(Keys.S))
                    {
                        moveDirection = Keys.S;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        size = new Rectangle(size.X, size.Y, size.Height, size.Width);
                        moveDirection = Keys.D;
                    }

                    playerState = PlayerStates.airdash;
                }
            }

            // Okay so come back to this and check this out
            // checks for if the player is above a platform or ground, and resets grounded to false
            //IsGrounded = false;

            // Update the player's "size" position and float position using the player's velocity after
            // calculations (X - Direction)
            position.X += velocity.X;
            size.X = (int)position.X;

            // Update the player's Y position depending on whether or not the player is grounded
            if (!isGrounded)
            {
                position.Y += velocity.Y;
                size.Y = (int)position.Y;
            }
            else
            {
                size.Y += 0;
            }

            // Update the player's previous state, and previous keyboard state
            previousPlayerState = playerState;
            previousKBState = kbState;
        }

        //draw
        public override void Draw(SpriteBatch spriteBatch, Texture2D playerTexture)
        {
            // Player state switch
            switch (playerState)
            {
                // If the player is facing right
                case PlayerStates.faceRight:
                    spriteBatch.Draw(
                        playerTexture,
                        size,
                        new Rectangle(0 * 128, 0, 128, 128),
                        Color.White);
                    break;

                // If the player is facing left
                case PlayerStates.faceLeft:
                    spriteBatch.Draw(
                        playerTexture,
                        size,
                        new Rectangle(3 * 128, 0, 128, 128),
                        Color.White);
                    break;

                // If the player is moving left
                case PlayerStates.moveLeft:
                    spriteBatch.Draw(
                        playerTexture,
                        size,
                        new Rectangle(4 * 128, 0, 128, 128),
                        Color.White);
                    break;

                // If the player is moving right
                case PlayerStates.moveRight:
                    spriteBatch.Draw(
                         playerTexture,
                         size,
                         new Rectangle(1 * 128, 0, 128, 128),
                         Color.White);
                    break;

                // if the player is jumping left
                case PlayerStates.jumpLeft:
                    spriteBatch.Draw(
                         playerTexture,
                         size,
                         new Rectangle(5 * 128, 0, 128, 128),
                         Color.White);
                    break;

                //if the player is jumping right
                case PlayerStates.jumpRight:
                    spriteBatch.Draw(
                         playerTexture,
                         size,
                         new Rectangle(2 * 128, 0, 128, 128),
                         Color.White);
                    break;

                case PlayerStates.airdash:
                    spriteBatch.Draw(
                         playerTexture,
                         size,
                         new Rectangle(1 * 128, 0, 128, 128),
                         Color.White);
                    break;
            }
        }

        /* public virtual bool CheckTouchingLeft(GameObject collider, GameObject collided)
         {
             return (collider.Size.Bottom >  collided.Size.Top &&
                     collider.Size.Top < collided.Size.Bottom &&
                     collider.Size.Right + this.velocity.X > collided.Size.Left &&
                     collider.Size.Left < collided.Size.Right);
         }*/

        public virtual bool CheckTouchingTop(GameObject collider, GameObject collided)
        {
            return (collider.Size.Bottom + this.velocity.Y > collided.Size.Top &&
                    collider.Size.Top < collided.Size.Top &&
                    collider.Size.Right > collided.Size.Left &&
                    collider.Size.Left < collided.Size.Right);
        }

        public void Jump()
        {
            isGrounded = false;
            position.Y -= 60f;
            velocity.Y = -20f;

            if (previousPlayerState == PlayerStates.faceRight || previousPlayerState == PlayerStates.jumpRight)
            {
                playerState = PlayerStates.jumpRight;
            }
            else if (previousPlayerState == PlayerStates.faceLeft || previousPlayerState == PlayerStates.jumpLeft)
            {
                playerState = PlayerStates.jumpLeft;
            }
        }

        /// <summary>
        /// This method resets the player to its starting state for resetting the game
        /// </summary>
        public override void Reset()
        {
            position.X = 64;
            position.Y = windowHeight - 196;
            playerState = PlayerStates.faceRight;
            velocity.X = 0f;
            velocity.Y = 0f;
            dashCounter = 20;
            timeStopActive = false;
            energy = maxEnergy;
            Health = maxHealth;
        }
    }
}

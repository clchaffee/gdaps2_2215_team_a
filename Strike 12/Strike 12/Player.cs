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
        airdash
    }

    class Player : GameObject
    {
        // ----- | Fields | -----
        //parameters for size and position for the player not currently decided

        private PlayerStates playerState;
        private PlayerStates previousPlayerState;
        private PlayerStates dashDirection;
        private Texture2D playerSprite;
        private float moveSpeed = 10f;
        private int health = 10;
        private float gravityMultiplier = 1f;
        private int dashCounter = 20;
        private int iCounter = 0;

        //fields for gravity
        protected Vector2 position;
        protected Vector2 velocity;
        protected bool isGrounded;

        // Input Fields
        public KeyboardState kbState;
        private KeyboardState previousKBState;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public bool IsGrounded
        {
            get { return isGrounded; }
            set { isGrounded = value; }
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
            if(iCounter == 20)
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
                if (dashCounter > 0)
                {
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        size.Y -= 20;
                        position.Y -= 20f;
                        dashCounter--;
                        return;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        size.X -= 20;
                        position.X -= 20f;
                        dashCounter--;
                        return;
                    }
                    else if (kbState.IsKeyDown(Keys.S))
                    {
                        size.Y += 20;
                        position.Y += 20f;
                        dashCounter--;
                        return;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        size.X += 20;
                        position.X += 20f;
                        dashCounter--;
                        return;
                    }
                }
                else
                {
                    playerState = dashDirection;
                }
            }

            //if A is pressed, moves player left, changes player state depending on if jumping or not.
            if (kbState.IsKeyDown(Keys.A))
            {
                velocity.X = -moveSpeed;
                if (!isGrounded)
                {
                    playerState = PlayerStates.jumpLeft;
                }
                else
                {
                    playerState = PlayerStates.moveLeft;
                }
            }
            //if D is pressed, moves player right, changes player state depending on if jumping or not.
            else if (kbState.IsKeyDown(Keys.D))
            {
                velocity.X = moveSpeed;
                if (!isGrounded)
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

            //if W is pressed, player jumps, with addition of velocity gravity, and updates player state accordingly
            if (kbState.IsKeyDown(Keys.W) && isGrounded == true)
            {
                position.Y -= 60f;
                velocity.Y = -20f;

                isGrounded = false;

                if (previousPlayerState == PlayerStates.faceRight || previousPlayerState == PlayerStates.jumpRight)
                {
                    playerState = PlayerStates.jumpRight;
                }
                else if (previousPlayerState == PlayerStates.faceLeft || previousPlayerState == PlayerStates.jumpLeft)
                {
                    playerState = PlayerStates.jumpLeft;
                }
            }

            //if the player is no longer on the ground, applies gravity
            if (!isGrounded)
            {

                // While the gravity multipler is under a specified value, add to it
                if (gravityMultiplier < 5)
                {
                    gravityMultiplier += 1f;
                }

                // If the player is falling, lower their movespeed to allow for precise landing
                if (velocity.Y <= 10 && velocity.Y >= 0)
                {
                    moveSpeed += 0.3f;
                }
                else if (velocity.Y > 0)
                {
                    moveSpeed = 8.7f;
                }

                // Update the player's Y velocity according to the multiplier
                velocity.Y += 0.15f * gravityMultiplier;

                // Air Dash
                if (kbState.IsKeyDown(Keys.Enter) && !previousKBState.IsKeyDown(Keys.Enter) && dashCounter > 0)
                {
                    dashDirection = playerState;
                    playerState = PlayerStates.airdash;
                    velocity.Y = 0;
                    velocity.X = 0;
                    size.X += 0;
                    size.Y += 0;
                }
            }
            else if (isGrounded)
            {
                dashCounter = 20;
                velocity.Y = 0;
            }
                
            //updates position based on velocity and the size rectangle
            position += velocity;
            size.X = (int)position.X;
            size.Y = (int)position.Y;

            //sets previous player and kb states
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
                         new Rectangle(2 * 128, 0, 128, 128),
                         Color.White);
                    break;
            }
        }

        public override void Reset()
        {
            position.X = windowWidth / 2 - size.Width / 2;
            position.Y = windowHeight / 2 - size.Height / 2;
            Health = 10;
            velocity.X = 0f;
            velocity.Y = 0f;
        }
    }
}

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
        jumpRight
    }

    class Player : GameObject
    {
        // ----- | Fields | -----
        private KeyboardState kbState; // Currently, I wrote the player logic using currentKBState, but I didn't want
                                       // to delete this randomly in case anyone prefers this convention
        //parameters for size and position for the player not currently decided

        private PlayerStates playerState = PlayerStates.faceRight;
        private PlayerStates previousPlayerState;
        private Texture2D playerSprite;
        private float moveSpeed = 5f;
        public int windowWidth;
        public int windowHeight;

        //fields for gravity
        protected Vector2 position;
        protected Vector2 velocity;
        protected bool isGrounded;

        // Input Fields
        public KeyboardState currentKBState;
        private KeyboardState previousKBState;

        // Location Fields
        //private Rectangle platformPosition;

        // ----- | Constructor | -----
        public Player(Texture2D texture, Rectangle size, int windowWidth, int windowHeight, 
            Vector2 position) : base(texture, size, windowWidth, windowHeight)
            
        {
            // This platformPos is completely temporary, as it is required to test the collisions currently in place
            this.playerSprite = texture;
            this.position = position;
            this.size = size;
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;
            isGrounded = false;
        }

        // -- Methods Overriden from parent class

        /// <summary>
        /// For testing purposes, I have been using this update method, as our collision detection testing currently
        /// relies on a value for the platform position which must be passed into it
        /// 
        /// In the future, this will be replaced with the actual update method, however, for the time being I request
        /// that this remain until everything is properly moved over
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            currentKBState = Keyboard.GetState();

            if (currentKBState.IsKeyDown(Keys.A))
            {
                velocity.X = -moveSpeed;
                playerState = PlayerStates.moveLeft;
            }
            else if (currentKBState.IsKeyDown(Keys.D))
            {
                velocity.X = moveSpeed;
                playerState = PlayerStates.moveRight;
            }
            else
            {
                velocity.X = 0f;
                if (previousPlayerState == PlayerStates.moveRight) 
                {
                    playerState = PlayerStates.faceRight;
                }
                else if (previousPlayerState == PlayerStates.moveLeft)
                {
                    playerState = PlayerStates.faceLeft;
                }
            }

            if (currentKBState.IsKeyDown(Keys.W) && isGrounded == true)
            {
                position.Y -= 15f;
                velocity.Y = -5f;
                isGrounded = false;
            }

            if (!isGrounded)
            {
                float i = 1;

                velocity.Y += 0.15f * i;
            }

            if (isGrounded)
            {
                velocity.Y = 0f;
            }

            if (position.Y + playerSprite.Height >=700) //placeholder value for collision
            {
                isGrounded = true;
            }

            position += velocity;
            size.X = (int)position.X;
            size.Y = (int)position.Y;
            previousPlayerState = playerState;
        }
        


        public void Draw(SpriteBatch spriteBatch, Texture2D playerTexture)
        {
            /*spriteBatch.Draw(
                        playerTexture,
                        position,
                        new Rectangle(1 * 128, 128, 128, 128),
                        Color.White);*/

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

                case PlayerStates.jumpRight:
                    spriteBatch.Draw(
                         playerTexture,
                         size,
                         new Rectangle(2 * 128, 0, 128, 128),
                         Color.White);
                    break;
            }
        }
    }
}

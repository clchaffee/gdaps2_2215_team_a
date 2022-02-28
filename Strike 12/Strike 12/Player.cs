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
        jump
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
        private int moveSpeed = 12;

        // Gravity Fields
        protected Vector2 position;
        protected Vector2 velocity;
        protected bool isGrounded;

        // Input Fields
        private KeyboardState currentKBState;
        private KeyboardState previousKBState;

        // Location Fields
        private Rectangle playerSize;
        private Rectangle platformPosition;

        // ----- | Constructor | -----
        public Player(Texture2D texture, Rectangle size, Vector2 pos, int windowHeight, int windowWidth)
            : base(texture, size, windowHeight, windowWidth)
        {
            // This platformPos is completely temporary, as it is required to test the collisions currently in place
            position = pos;
            playerSprite = texture;
            playerSize = size;
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
            // This is entirely temporary, as the collision detection code currently relies on the position of the platform
            // drawn to the screen, which will be removed as testing concludes and the collision detection is handeled 
            // elsewhere
            Rectangle platformPos = platformPosition;

            // Gather the current state of the keyboard
            currentKBState = Keyboard.GetState();


            // Left Movement 
            if (currentKBState.IsKeyDown(Keys.D))
            {
                playerState = PlayerStates.moveRight;
            }
            else if (!currentKBState.IsKeyDown(Keys.D))
            {
                playerState = PlayerStates.faceRight;
            }

            // Right movement
            if (currentKBState.IsKeyDown(Keys.A))
            {
                playerState = PlayerStates.moveLeft;
            }
            else if (!currentKBState.IsKeyDown(Keys.A))
            {
                playerState = PlayerStates.faceLeft;
            }

            // If the player is no longer pressing A, switch back to the facing left state
            if (currentKBState.IsKeyDown(Keys.W))
            {
                playerState = PlayerStates.jump;
            }

            // Move the player based on the current velocity
            velocity.X += moveSpeed;

            /*if (jumpLength > 0)
            {
                position.Y -= (60 - 10 * jumpModifier);
            }
            else if (jumpLength < 0)
            {
                playerState = previousPlayerState;
            }

            if (currentKBState.IsKeyDown(Keys.A) && !CheckLeftCollision(platformPos))
            {
                // ...allow the player to move by the constant movespeed
                position.X -= moveSpeed + 5;
            }

            if (currentKBState.IsKeyDown(Keys.D) && !CheckRightCollision(platformPos))
            {
                // ...allow the player to move by the constant movespeed
                position.X += moveSpeed + 5;
            }

            jumpModifier += 1;
            jumpLength -= 1;

            // Temporary gravity calculations, this should be moved into wherever the collision detection is being
            // handeled
            // If the player is colliding with a platform in the downward direction (aka, moving into a platform)...
            if (CheckDownwardCollision(platformPos))
            {
                // ...while the bottom of the player is clipping inside of the top of the object, move the player
                // up by one until they are no longer clipping into the platform
                while (position.Bottom > platformPos.Top)
                {
                    position.Y += 1;
                }

                isGrounded = true;
            }
            // If the player is not colliding or will not be colliding with a platform
            else
            {
                // ... add to the player's current Y position by a constant value, gravity
                if (playerState != PlayerStates.jump)
                {
                    position.Y += gravity;
                    isGrounded = false;
                }
            }

            // Save the previous keyboard input
            previousKBState = Keyboard.GetState();*/
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D playerTexture)
        {
            // Player state switch
            switch (playerState)
            {
                // If the player is facing right
                case PlayerStates.faceRight:

                    // Draws the player texture facing right
                    spriteBatch.Draw(
                        playerTexture,
                        position,
                        new Rectangle(1 * 128, 128, 128, 128),
                        Color.White);

                    break;

                // If the player is facing left
                case PlayerStates.faceLeft:

                    spriteBatch.Draw(
                        playerTexture,
                        position,
                        new Rectangle(2 * 128, 128, 128, 128),
                        Color.White);

                    break;

                // If the player is moving left
                case PlayerStates.moveLeft:

                    spriteBatch.Draw(
                        playerTexture,
                        position,
                        new Rectangle(2 * 128, 128, 128, 128),
                        Color.White);

                    break;

                // If the player is moving right
                case PlayerStates.moveRight:

                    spriteBatch.Draw(
                         playerTexture,
                         position,
                         new Rectangle(1 * 128, 128, 128, 128),
                         Color.White);

                    break;

                case PlayerStates.jump:

                    spriteBatch.Draw(
                         playerTexture,
                         position,
                         new Rectangle(1 * 128, 128, 128, 128),
                         Color.White);

                    break;
            }
        }

        // Temp collision checking
        // In theory, this should be moved to the game object manager class in order to elminiate the need for
        /* the player to check each individual object that it could possibly collide with each frame
        public bool CheckDownwardCollision(Rectangle check)
        {
            return (position.Bottom + gravity > check.Top &&
                    position.Left < check.Right &&
                    position.Right > check.Left);
        }

        public bool CheckRightCollision(Rectangle check)
        {
            return (position.Right + moveSpeed > check.Left &&
                    position.Left < check.Right &&
                    position.Top < check.Bottom &&
                    position.Bottom > check.Top);
        }

        public bool CheckLeftCollision(Rectangle check)
        {
            return (position.Left - moveSpeed < check.Right &&
                    position.Left > check.Right &&
                    position.Top < check.Bottom &&
                    position.Bottom > check.Top);
        }*/


    }
}

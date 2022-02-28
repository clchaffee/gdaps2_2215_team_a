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
        private int gravity = 10;
        private int jumpSpeed = 50;
        private bool grounded;
        private int jumpLength = 5;
        private int jumpModifier = 0;

        // Input Fields
        private KeyboardState currentKBState;
        private KeyboardState previousKBState;

        // Location Fields
        private Rectangle playerPosition;
        private Rectangle platformPosition;


        // ----- | Constructor | -----
        public Player(Texture2D texture, Rectangle position, Rectangle platformPos, int windowHeight, int windowWidth)
            :base(texture, position, windowHeight, windowWidth)
        {
            // This platformPos is completely temporary, as it is required to test the collisions currently in place
            platformPosition = platformPos;
            playerSprite = texture;
            playerPosition = position; 
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

            // Player state switch
            switch (playerState)
            {
                // If the player is facing right
                case PlayerStates.faceRight:

                    // If the player is pressing D, switch the state to move right
                    if (currentKBState.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerStates.moveRight;
                    }

                    // If the player is pressing A, switch the state to face left
                    if (currentKBState.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerStates.faceLeft;
                    }

                    // If the player presses W, switch them to the jump state
                    if (currentKBState.IsKeyDown(Keys.W) && previousKBState.IsKeyUp(Keys.W) && grounded == true)
                    {
                        jumpLength = 5;
                        jumpModifier = 0;
                        previousPlayerState = playerState;
                        playerState = PlayerStates.jump;
                    }

                    break;

                // If the player is facing left
                case PlayerStates.faceLeft:

                    // If the player is pressing A, move the player to the left
                    if (currentKBState.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerStates.moveLeft;
                    }

                    // If the player presses D, move the player to face right
                    if (currentKBState.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerStates.faceRight;
                    }

                    // If the player presses W, switch them to the jump state
                    if (currentKBState.IsKeyDown(Keys.W) && previousKBState.IsKeyUp(Keys.W) && grounded == true)
                    {
                        jumpLength = 5;
                        jumpModifier = 0;
                        previousPlayerState = playerState;
                        playerState = PlayerStates.jump;
                    }

                    break;

                // If the player is moving left
                case PlayerStates.moveLeft:

                    // If the player presses A, and the player is not colliding with an object, and will not be
                    // colliding with an object...
                    if (currentKBState.IsKeyDown(Keys.A) && !CheckLeftCollision(platformPos))
                    {
                        // ...allow the player to move by the constant movespeed
                        position.X -= moveSpeed;
                    }
                    else
                    {
                        // If they stop moving, put them in the faceleft state
                        playerState = PlayerStates.faceLeft;
                    }

                    // If the player presses W, switch them to the jump state
                    if (currentKBState.IsKeyDown(Keys.W) && previousKBState.IsKeyUp(Keys.W) && grounded == true)
                    {
                        jumpLength = 5;
                        jumpModifier = 0;
                        previousPlayerState = playerState;
                        playerState = PlayerStates.jump;
                    }

                    break;

                // If the player is moving right
                case PlayerStates.moveRight:

                    // If the player presses D, and the player is not colliding with an object, and will not be
                    // colliding with an object...
                    if (currentKBState.IsKeyDown(Keys.D) && !CheckRightCollision(platformPos))
                    {
                        // ...allow the player to move by the constant movespeed
                        position.X += moveSpeed;
                    }
                    else
                    {
                        // If they stop moving, put them in the faceright state
                        playerState = PlayerStates.faceRight;
                    }

                    // If the player presses W, switch them to the jump state
                    if (currentKBState.IsKeyDown(Keys.W) && previousKBState.IsKeyUp(Keys.W) && grounded == true)
                    {
                        jumpLength = 5;
                        jumpModifier = 0;
                        previousPlayerState = playerState;
                        playerState = PlayerStates.jump;
                    }

                    break;

                // If the player is jumping
                case PlayerStates.jump:

                    /*Conrad's thoughts on jumping
                    if (grounded && (currentKBState.IsKeyUp(Keys.W) && previousKBState.IsKeyDown(Keys.W)))
                    {
                        position.Y -= jumpSpeed;
                        grounded = false;
                    }
                    else if (!grounded)
                    {
                        position.Y -= jumpSpeed;
                        jumpSpeed -= gravity;
                    }*/

                    if (jumpLength > 0)
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

                    break;
            }

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

                grounded = true;
            }
            // If the player is not colliding or will not be colliding with a platform
            else
            {
                // ... add to the player's current Y position by a constant value, gravity
                if (playerState != PlayerStates.jump)
                {
                    position.Y += gravity;
                    grounded = false;
                }
            }

            // Save the previous keyboard input
            previousKBState = Keyboard.GetState();
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
        // the player to check each individual object that it could possibly collide with each frame
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
        }


    }
}

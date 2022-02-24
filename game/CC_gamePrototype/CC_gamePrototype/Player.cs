using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CC_gamePrototype
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

    /// <summary>
    /// Currently, the player manages its own animations and logic
    /// While the logic will always remain exclusive to the player, the animation is likely temporary, as it should
    /// be handled by an animation manager later down the line
    /// </summary>
    class Player : GameObjectManager
    {
        // Fields
        private PlayerStates playerState = PlayerStates.faceRight;
        private Texture2D playerSprite;
        private int moveSpeed = 5;
        private int gravity = 5;
        private bool grounded;

        // Input Fields
        private KeyboardState currentKBState;
        private KeyboardState previousKBState;

        // Location Fields
        private Rectangle position;         

        // Properties

        // Constructor
        // Takes a texture to draw, an int representing the player's X position, and an int that represent's the
        // player's Y position
        public Player(Texture2D playerText, int playerX, int playerY)
        {
            playerSprite = playerText;
            position = new Rectangle(playerX, playerY, 128, 128);
        }

        // Methods
        public void Update(GameTime gameTime, Rectangle platformPos)
        {
            // Gather the current state of the keyboard
            currentKBState = Keyboard.GetState();

            // Player state switch
            switch (playerState)
            {
                // If the player is facing right
                case PlayerStates.faceRight:

                    // If the player is pressing D, switch the state to move right
                    if (currentKBState.IsKeyDown(Keys.D) && !CheckSidewaysCollision(platformPos))
                    {
                        playerState = PlayerStates.moveRight;
                    }

                    // If the player is pressing A, switch the state to face left
                    if (currentKBState.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerStates.faceLeft;
                    }

                    break;

                // If the player is facing left
                case PlayerStates.faceLeft:

                    // If the player is pressing A, move the player to the left
                    if (currentKBState.IsKeyDown(Keys.A) && !CheckSidewaysCollision(platformPos))
                    {
                        position.X -= moveSpeed;
                    }

                    // If the player presses D, move the player to face right
                    if (currentKBState.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerStates.faceRight;
                    }

                    break; 

                // If the player is moving left
                case PlayerStates.moveLeft:

                    if (currentKBState.IsKeyDown(Keys.A))
                    {
                        position.X -= moveSpeed;
                    }
                    else
                    {
                        playerState = PlayerStates.faceLeft;
                    }

                    break;

                // If the player is moving right
                case PlayerStates.moveRight:

                    if (currentKBState.IsKeyDown(Keys.D))
                    {
                        position.X += moveSpeed;
                    }
                    else
                    {
                        playerState = PlayerStates.faceRight;
                    }

                    break;
            }

            /*Temporary collision detection, implemented so that the player can jump successfully
            if (Rectangle.Intersect(position, platformPos) == Rectangle.Empty)
            {
                if (position.Bottom + gravity > platformPos.Top)
                {
                    while (position.Bottom >= platformPos.Top)
                    {
                        position.Y += 1;
                    }
                }
                else
                {
                    position.Y += gravity;
                }
            }
            else if (position.Bottom == platformPos.Top &&
                     Rectangle.Intersect(position, platformPos) != Rectangle.Empty)
            {
                position.Y += 0;
            }*/

            if (CheckDownwardCollision(platformPos))
            {
                while (position.Bottom > platformPos.Top)
                {
                    position.Y += 1;
                }
            }
            else
            {
                position.Y += gravity;
            }

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
            }
        }

        // Temp collision checking
        //In theory, this should be moved to the game object manager class in order to elminiate the need for
        public bool CheckDownwardCollision(Rectangle check)
        {
            return (position.Bottom + gravity > check.Top &&
                    position.Left < check.Right &&
                    position.Right > check.Left);
        }

        public bool CheckSidewaysCollision(Rectangle check)
        {
            return (position.Right + moveSpeed > check.Left &&
                    position.Left < check.Right &&
                    position.Top < check.Bottom &&
                    position.Bottom > check.Top)
                    ||
                   (position.Left - moveSpeed < check.Right &&
                    position.Left > check.Right &&
                    position.Top < check.Top &&
                    position.Bottom > check.Bottom);
        }

    }
}

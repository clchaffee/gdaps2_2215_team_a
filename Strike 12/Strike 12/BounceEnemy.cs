using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Strike_12
{
    class BounceEnemy : BulletEnemy
    {
        // ----- Fields -----
        private Texture2D enemySprite;
        private CornerState location;
        private int xSpeed;
        private int ySpeed;

        // ----- | Constructor | -----
        // Paramatarized Constructor
        public BounceEnemy(Texture2D texture, Rectangle size, int windowWidth, int windowHeight, int playerX, int playerY)
            : base(texture, size, windowWidth, windowHeight, playerX, playerY)
        {
            enemySprite = texture;
            // Chooses a random corner to for the block
            location = (CornerState)rng.Next(0, 4);
            xSpeed = rng.Next(2, 11);
            ySpeed = rng.Next(2, 11);

            // Cases for random corner
            switch (location)
            {
                case CornerState.TLeft:
                    this.size.X = size.Width;
                    this.size.Y = size.Height;
                    break;

                case CornerState.TRight:
                    this.size.X = windowWidth - 128;
                    this.size.Y = size.Height;
                    break;

                case CornerState.BLeft:
                    this.size.X = size.Width;
                    this.size.Y = windowHeight - 128;
                    break;

                case CornerState.BRight:
                    this.size.X = windowWidth - 128;
                    this.size.Y = windowHeight - 128;
                    break;
            }
        }

        // ----- | Method | -----

        // Update():
        public override void Update(GameTime gameTime)
        {
            switch (location)
            {
                case CornerState.TLeft:
                    size.X += xSpeed;
                    size.Y += ySpeed;
                    break;

                case CornerState.TRight:
                    size.X -= xSpeed;
                    size.Y += ySpeed;
                    break;

                case CornerState.BLeft:
                    size.X += xSpeed;
                    size.Y -= ySpeed;
                    break;

                case CornerState.BRight:
                    size.X -= xSpeed;
                    size.Y -= ySpeed;
                    break;
            }

            //changes direction when the border of the window is hit
            if (size.X > windowWidth - 128 || size.X < 64)
            {
                xSpeed = -1 * xSpeed;
            }
            if (size.Y > windowHeight - 128 || size.Y < 64)
            {
                ySpeed = -1 * ySpeed;
            }
        }

        // Draw()
        public override void Draw(SpriteBatch spriteBatch, Texture2D Texture)
        {
            switch (location)
            {
                case CornerState.TLeft:
                    spriteBatch.Draw(
                         enemySprite,
                         size,
                         new Rectangle(64 - windowWidth, 64 - windowHeight, 128, 128),
                         Color.White);
                    break;

                case CornerState.TRight:
                    spriteBatch.Draw(
                         enemySprite,
                         size,
                         new Rectangle(windowWidth, 64 - windowHeight, 128, 128),
                         Color.White);
                    break;

                case CornerState.BLeft:
                    spriteBatch.Draw(
                         enemySprite,
                         size,
                         new Rectangle(64 - windowWidth, windowHeight, 128, 128),
                         Color.White);
                    break;

                case CornerState.BRight:
                    spriteBatch.Draw(
                         enemySprite,
                         size,
                         new Rectangle(windowWidth, windowHeight, 128, 128),
                         Color.White);
                    break;
            }
        }

        // Reset():
        public override void Reset()
        {
            location = (CornerState)rng.Next(0, 4);
            xSpeed = rng.Next(2, 11);
            ySpeed = rng.Next(2, 11);

            switch (location)
            {
                case CornerState.TLeft:
                    this.size.X = size.Width;
                    this.size.Y = size.Height;
                    break;

                case CornerState.TRight:
                    this.size.X = windowWidth - 128;
                    this.size.Y = size.Height;
                    break;

                case CornerState.BLeft:
                    this.size.X = size.Width;
                    this.size.Y = windowHeight - 128;
                    break;

                case CornerState.BRight:
                    this.size.X = windowWidth - 128;
                    this.size.Y = windowHeight - 128;
                    break;
            }
        }
    }
}

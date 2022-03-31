using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 
/// </summary>
namespace Strike_12
{
    // Terminology, T = Top, B = Bottom
    enum CornerState
    {
        TLeft,
        TRight,
        BLeft,
        BRight
    }

    class BulletEnemy : Enemy
    {
        // ----- Fields -----
        private Texture2D enemySprite;
        private CornerState location;
        private int xSpeed;
        private int ySpeed;

        // ----- | Constructor | -----
        // Paramatarized Constructor
        public BulletEnemy(Texture2D texture, Rectangle size, int windowWidth, int windowHeight)
            : base(texture, size, windowWidth, windowHeight)
        {
            this.enemySprite = texture;
            this.size = size;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            // Chooses a random corner to for the block
            location = (CornerState)rng.Next(0, 4);
            xSpeed = rng.Next(2, 11);
            ySpeed = rng.Next(2, 11);

            // Cases for random corner
            switch (location)
            {
                case CornerState.TLeft:
                    this.size.X = 0 - size.Width;
                    this.size.Y = 0 - size.Height;
                    break;

                case CornerState.TRight:
                    this.size.X = windowWidth;
                    this.size.Y = 0- size.Height;
                    break;

                case CornerState.BLeft:
                    this.size.X = 0 - size.Width;
                    this.size.Y = windowHeight;
                    break;

                case CornerState.BRight:
                    this.size.X = windowWidth;
                    this.size.Y = windowHeight;
                    break;
            }

        }

        // ----- Methods -----

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
        }

        // Draw():
        public override void Draw(SpriteBatch spriteBatch, Texture2D Texture)
        {
            switch (location)
            {
                case CornerState.TLeft:
                    spriteBatch.Draw(
                         enemySprite,
                         size,
                         new Rectangle(0 - windowWidth, 0 - windowHeight, 128, 128),
                         Color.White);
                    break;

                case CornerState.TRight:
                    spriteBatch.Draw(
                         enemySprite,
                         size,
                         new Rectangle(windowWidth, 0 - windowHeight, 128, 128),
                         Color.White);
                    break;

                case CornerState.BLeft:
                    spriteBatch.Draw(
                         enemySprite,
                         size,
                         new Rectangle(0 - windowWidth, windowHeight, 128, 128),
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

        // Reset()
        public override void Reset()
        {
            location = (CornerState)rng.Next(0, 4);
            xSpeed = rng.Next(2, 11);
            ySpeed = rng.Next(2, 11);

            switch (location)
            {
                case CornerState.TLeft:
                    this.size.X = 0 - size.Width;
                    this.size.Y = 0 - size.Height;
                    break;

                case CornerState.TRight:
                    this.size.X = windowWidth;
                    this.size.Y = 0 - size.Height;
                    break;

                case CornerState.BLeft:
                    this.size.X = 0 - size.Width;
                    this.size.Y = windowHeight;
                    break;

                case CornerState.BRight:
                    this.size.X = windowWidth;
                    this.size.Y = windowHeight;
                    break;
            }
        }
    }
}

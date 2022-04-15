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
        public BulletEnemy(Texture2D texture, Rectangle size, int windowWidth, int windowHeight, int playerX, int playerY)
            : base(texture, size, windowWidth, windowHeight)
        {
            enemySprite = texture;
            // Chooses a random corner to for the block
            location = (CornerState)rng.Next(0, 4);
            //xSpeed = rng.Next(2, 11);
            //ySpeed = rng.Next(2, 11);
            // 0o0
            Vector2 enemyPos = new Vector2(size.X, size.Y);
            Vector2 playerPos = new Vector2(playerX, playerY);

            float xDistance = enemyPos.X - playerPos.X;
            float yDistance = enemyPos.Y - playerPos.Y;

            float length = (float)Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));

            float nomX = Math.Abs(xDistance / length);
            float nomY = Math.Abs(yDistance / length);
            switch (location)
            {
                case CornerState.TLeft:
                    this.size.X = 0 - size.Width;
                    this.size.Y = 0 - size.Height;

                        xSpeed = (int)(6 * nomX);

                        ySpeed = (int)(6 * nomY);
                    
                    break;

                case CornerState.TRight:
                    this.size.X = windowWidth;
                    this.size.Y = 0- size.Height;
                        
                    xSpeed = -(int)(6 * nomX);
                    
                    ySpeed = (int)(6 * nomY);
                    
                    break;

                case CornerState.BLeft:
                    this.size.X = 0 - size.Width;
                    this.size.Y = windowHeight;

                        xSpeed = (int)(6 * nomX);
                    
                    
                        ySpeed = -(int)(6 * nomY);

                    break;

                case CornerState.BRight:
                    this.size.X = windowWidth;
                    this.size.Y = windowHeight;

                        xSpeed = -(int)(6 * nomX);

                        ySpeed = -(int)(6 * nomX);
                    
                    break;
            }




            // Cases for random corner


        }

        // ----- Methods -----

        // Update():
        public override void Update(GameTime gameTime)
        {
            size.X += xSpeed;
            size.Y += ySpeed;
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

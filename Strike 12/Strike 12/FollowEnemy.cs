﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Strike_12
{
    class FollowEnemy : Enemy
    {
        // ----- Fields -----
        private Texture2D enemySprite;
        private CornerState location;
        private int xSpeed;
        private int ySpeed;

        // ----- | Constructor | -----
        // Paramatarized Constructor
        public FollowEnemy(Texture2D texture, Rectangle size, int windowWidth, int windowHeight)
            : base(texture, size, windowWidth, windowHeight)
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

        // ----- | Methods | -----

        // Update():
        public void Update(GameTime gameTime, Player playerVal)
        {
            Vector2 enemyPos = new Vector2(size.X, size.Y);
            Vector2 playerPos = new Vector2(playerVal.Size.X, playerVal.Size.Y);

            float xDistance = enemyPos.X - playerPos.X;
            float yDistance = enemyPos.Y - playerPos.Y;

            float length = (float)Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));

            float nomX = Math.Abs(xDistance / length);
            float nomY = Math.Abs(yDistance / length);

            if (enemyPos.X > playerPos.X)
            {
                size.X -= (int)(6 * nomX);
            }
            else
            {
                size.X += (int)(6 * nomX);
            }

            if (enemyPos.Y > playerPos.Y)
            {
                size.Y -= (int)(6 * nomY);
            }
            else
            {
                size.Y += (int)(6 * nomY);
            }


        }
    }
}

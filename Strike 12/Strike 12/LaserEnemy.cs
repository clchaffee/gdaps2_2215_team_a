using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strike_12
{
    class LaserEnemy : Enemy
    {
        private int playerPos;
        private const int speed = 3;
        private bool blast = false;
        private int baseSize;
        int cooldown = 0;



        public LaserEnemy(Texture2D texture, Rectangle size, int windowWidth, int windowHeight, int playerPos)
            : base(texture, size, windowWidth, windowHeight)
        {
            this.playerPos = playerPos;
            baseSize = size.Width;

            //spawns randomly on the y axis 
            size.Y = rng.Next(0, windowHeight + 1 - size.Height);
            //randomly decides if it spawns on the right or left
            if(rng.Next(1, 100) > 50)
            {
                size.X = 0;
            }
            else
            {
                size.X = windowWidth - size.Width;
            }
        }

        public void Update(GameTime gameTime, int playerPos)
        {
            //only if blast is active
            if (blast)
            {
                //waits for a quarter second before blasting for 3/4 seconds
                if(cooldown > 15 && cooldown < 60)
                {
                    //depending on the side it's on, moves extends the laser
                    if (size.X == 0)
                    {
                        size.Width = windowWidth;
                    }
                    else if (size.X == windowWidth - size.Width)
                    {
                        size.X = 0;
                        size.Width = windowHeight;
                    }
                }
                //increments cooldown
                cooldown++;
                //after a second, resets the laser
                if (cooldown == 60)
                {
                    Reset();
                }
            }

            else if (size.Y > playerPos - 20 && size.Y <  playerPos + 20)
            {
                blast = true;
            }
            else if (playerPos < size.Y)
            {
                size.Y -= speed;
            }
            else if (playerPos > size.Y)
            {
                size.Y += speed;
            }
        }

        //draws yay
        public override void Draw(SpriteBatch spriteBatch, Texture2D enemyTexture)
        {
            spriteBatch.Draw(enemyTexture, size, Color.White);
        }

        //resets everything 
        public override void Reset()
        {
            if (rng.Next(1, 100) > 50)
            {
                size.X = 0;
            }
            else
            {
                size.X = windowWidth - size.Width;
            }
            size.Y = rng.Next(0, windowHeight + 1 - size.Height);
            size.Width = 64;
            blast = false;
            cooldown = 0;

        }

    }
}

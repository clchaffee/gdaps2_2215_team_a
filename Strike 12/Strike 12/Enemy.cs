using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
/// <summary>
/// Author: Copious Cats 
/// Purpose: This is an abstract class that the different 
/// objects will inherit from
/// </summary>
namespace Strike_12
{
    //temp states of the player
    enum EnemyStates 
    { 
        moveLeft,
        moveRight
    }

    /// <summary>
    /// basic patrol style Enemy
    /// </summary>
    class Enemy : GameObject
    {
        // ----- | Fields | -----
        int speed = 3;  //temp speed variable, will be changed
        private EnemyStates enemyState = EnemyStates.moveRight;

        private Texture2D enemySprite;

        // ----- | Constructor | -----

        // Paramatarized Constructor
        public Enemy(Texture2D texture, Rectangle size, int windowWidth, int windowHeight)
            : base(texture, size, windowWidth, windowHeight)
        {
            this.enemySprite = texture;
            this.size = size;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        // ----- | Property | -----

        // ----- | Methods | -----
        // -- Methods Overriden from parent class
        /// <summary>
        /// changes direction od speed when bounces against the wall
        /// and updates sprite acordingly
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Bounce(windowWidth, windowHeight) == true)
            {
                speed *= -1;
            }
            
            //changes state based on which direction enemy is moving
            if (speed < 0)
            {
                enemyState = EnemyStates.moveLeft;
            }
            else
            {
                enemyState = EnemyStates.moveRight;
            }
            size.X += speed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch spriteBatch, Texture2D enemyTexture)
        {
            switch (enemyState)
            {
                case EnemyStates.moveLeft:
                    spriteBatch.Draw(
                        enemyTexture,
                        size,
                        new Rectangle(4 * 128, 0, 128, 128),
                        Color.White);
                    break;

                case EnemyStates.moveRight:
                    spriteBatch.Draw(
                         enemyTexture,
                         size,
                         new Rectangle(1 * 128, 0, 128, 128),
                         Color.White);
                    break;

                default:
                    break;
            }
        }
    }
}

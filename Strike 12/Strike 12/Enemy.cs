﻿using System;
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
    //temp states of the enemy
    enum EnemyStates 
    { 
        moveLeft,
        moveRight,
        airLeft,
        airRight
    }

    /// <summary>
    /// basic patrol style Enemy that moves left and right across the screen,
    /// changing direction when it bounces against the wall (game window)
    /// </summary>
    class Enemy : GameObject
    {
        // ----- | Fields | -----
        private EnemyStates enemyState;
        private Texture2D enemySprite;

        private int moveSpeed = 5; //temp speed variable, can be changed

        //fields for gravity
        protected Vector2 position;
        protected Vector2 velocity;
        protected bool isGrounded;
        protected bool hasGravity = true; // temp value, will be fully implimented later
        private float gravityMultiplier = 1f;

        // ----- | Constructor | -----
        // Paramatarized Constructor
        public Enemy(Texture2D texture, Rectangle size, int windowWidth, int windowHeight)
            : base(texture, size, windowWidth, windowHeight)
        {
            this.enemySprite = texture;
            this.size = size;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            enemyState = EnemyStates.moveRight;
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
            size.X += moveSpeed;

            //changes direction when the border of the window is hit
            if (size.X > windowWidth - 128 || size.X < 0)
            {
                moveSpeed = -1 * moveSpeed;
            }

            //flips sprite based on neg or pos speed
            if (moveSpeed < 0)
            {
                enemyState = EnemyStates.moveLeft;
            }
            else
            {
                enemyState = EnemyStates.moveRight;
            }
        }

        /// <summary>
        /// changes enemy sprite based on which direction it is going
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch spriteBatch, Texture2D enemyTexture)
        {
            //different appearance depending on game state
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

        public override void Reset()
        {
            size.X = 100;
            size.Y = 750;
            moveSpeed = 5;
        }
    }
}

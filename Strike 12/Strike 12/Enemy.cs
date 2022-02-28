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
    /// basic Enemy class
    /// changes states depending on which way 
    /// </summary>
    class Enemy : GameObject
    {
        // ----- | Fields | -----
        int speed = 3;  //temp speed variable, will be changed
        private EnemyStates enemyState = EnemyStates.moveRight;

        // ----- | Constructor | -----

        // Paramatarized Constructor
        public Enemy(Texture2D texture, Rectangle position, int windowWidth, int windowHeight)
            : base(texture, position, windowWidth, windowHeight)
        {
            this.size = position;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        // ----- | Property | -----

        // ----- | Methods | -----
        // -- Methods Overriden from parent class
        public override void Update(GameTime gameTime)
        {
            if (Bounce(windowWidth, windowHeight) == true)
            {
                speed *= -1;
            }
            
            if (speed < 0)
            {
                enemyState = EnemyStates.moveLeft;
            }
            else
            {
                enemyState = EnemyStates.moveRight;
            }
            position.X += speed;
        }

        public override void Draw(SpriteBatch sb)
        {
            switch (enemyState)
            {
                case EnemyStates.moveLeft:
                    break;
                case EnemyStates.moveRight:
                    break;
                default:
                    break;
            }
        }
    }
}

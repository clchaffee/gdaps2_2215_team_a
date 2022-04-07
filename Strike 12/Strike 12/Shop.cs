using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strike_12
{

    /// <summary>
    /// class to hold shop information, upgrades, abilities and stats data
    /// </summary>
    class Shop
    {
        //currency/points
        private int kromer = 0;
        private int spendings = 0;

        //upgrades
        private int maxHealth = 10;
        private float speed = 8f;
        private int maxEnergy = 0;
        
        //abilities
        private bool airDash = false;
        private bool heal = false;
        private bool timeSlow = false;

        /// <summary>
        /// get set property for points
        /// </summary>
        public int Points
        {
            get { return kromer; }
            set { kromer = value; }
        }

        /// <summary>
        /// get set property for lifetime spendings
        /// </summary>
        public int Spendings
        {
            get { return spendings; }
            set { spendings = value; }
        }

        /// <summary>
        /// get set property for max health
        /// </summary>
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        /// <summary>
        /// get set property for speed
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// get set property for energy
        /// </summary>
        public int Energy
        {
            get { return maxEnergy; }
            set { maxEnergy = value; }
        }

        /// <summary>
        /// get set property for air dash
        /// </summary>
        public bool AirDash
        {
            get { return airDash; }
            set { airDash = value; }
        }

        /// <summary>
        /// get set property for heal ability
        /// </summary>
        public bool Heal
        {
            get { return heal; }
            set { heal = value; }
        }

        /// <summary>
        /// get set property for time slow ability
        /// </summary>
        public bool TimeSlow
        {
            get { return timeSlow; }
            set { timeSlow = value; }
        }

        /// <summary>
        /// constructor starts with default points at 0
        /// </summary>
        /// <param name="points"></param>
        public Shop(int points)
        {
            kromer = points;
        }

        /// <summary>
        /// draw method to print all the player stats
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteFont"></param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.DrawString(spriteFont, $"Stats and Upgrades:",
                      new Vector2(75, 75), Color.White);
        }
    }
}

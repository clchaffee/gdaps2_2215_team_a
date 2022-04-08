﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strike_12
{
    /// <summary>
    /// states of buttons
    /// mainly for color indicators rn
    /// </summary>
    enum State 
    { 
        Pressed,
        Highlighted,
        NonHighlighted
    }

    class Button
    {
        //fields for position and image
        protected Texture2D texture;
        protected Rectangle size;

        //mouse and game states
        private MouseState mouseState;
        private MouseState prevMouseState;
        private State state = State.NonHighlighted;

        //variables
        private string type;
        private bool isPressed = false;
        private bool isHighlighted = false;
        private int cost;

        //get set properties for each button called/created
        //button upgrade
        public string Type
        {
            get { return type; }
        }

        //size of button
        public Rectangle Size
        {
            get { return size; }
        }

        //bool to check if the button is pressed
        public bool IsPressed
        {
            get { return isPressed; }
        }

        public bool IsHighlight
        {
            get { return isHighlighted; }
        }

        //cost of upgrades
        public int Cost
        {
            get { return cost; }
            set { this.cost = value; }
        }

        //constructor to take in each piece of data
        public Button(string type, Texture2D texture, Rectangle size, int cost)
        {
            this.type = type;
            this.texture = texture;
            this.size = size;
            this.cost = cost;
        }

        /// <summary>
        /// personal update method for buttons themselves
        /// uses mouse click input to change states of button
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            //if the mouse is in the bounds of the window
            if (mouseState.X > size.X && mouseState.Y > size.Y 
                && mouseState.X < (size.X + size.Width) && mouseState.Y < size.Y + size.Height)
            {
                isHighlighted = true;
                state = State.Highlighted;
                //if the button is still being presse, not yet released
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    state = State.Pressed;
                }
                //once clicked and then released
                else if (prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    isPressed = true;
                }
                else
                {
                    isPressed = false;
                }
            }
            //if the mouth is not in the bunds of the button
            else
            {
                isHighlighted = false;
                state = State.NonHighlighted;
            }
            //updated the mouse state
            prevMouseState = mouseState;
        }

        /// <summary>
        /// personal draw method for each button that prints out the button based on state
        /// and the printed (updated) cost of said button
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteFont"></param>
        /// <param name="texture"></param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //switch for states of the buttons
            //as of now, only changes colors
            switch (state)
            {
                case (State.Pressed):
                    if (type != "cat")
                    {
                        spriteBatch.Draw(texture, size, Color.Green);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, size, Color.White);
                        spriteBatch.DrawString(spriteFont, $"MEOW ^-3-^",
                                 new Vector2(size.X + 50, size.Y - 100), Color.LightGray);
                    }
                    break;

                case (State.Highlighted):
                    if (type != "cat")
                    {
                        spriteBatch.Draw(texture, size, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, size, Color.White);
                    }
                    break;

                case (State.NonHighlighted):
                    spriteBatch.Draw(texture, size, Color.White);
                    break;

                default:
                    break;
            }

            if (Cost != 0)
            {
                //prints cost
                spriteBatch.DrawString(spriteFont, $"{Type} \nCost: {cost}",
                          new Vector2(size.X, size.Y + size.Height), Color.LightGray);
            }

        }
    }
}

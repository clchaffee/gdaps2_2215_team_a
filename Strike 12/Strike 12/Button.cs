using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Strike_12
{
    /// <summary>
    /// Wrot by Professor Mesh:
    /// If the client wants to be notified when a button is clicked, it must
    /// implement a method matching this delegate and then tie that method to
    /// the button's "OnButtonClick" event.
    /// </summary>
    public delegate void OnButtonClickDelegate();

    /// <summary>
    /// states of buttons
    /// mainly for color indicators rn
    /// </summary
    enum State 
    { 
        Pressed,
        Highlighted,
        NonHighlighted
    }

    class Button
    {
        //fields
        //position and image
        protected Texture2D texture;
        protected Rectangle size;
        public Shop shop;

        //mouse and game states
        private MouseState mouseState;
        private MouseState prevMouseState;
        private State state = State.NonHighlighted;

        //variables
        private string type;
        private int cost;

        //event for left click
        public event OnButtonClickDelegate OnLeftButtonClick;

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

        //cost of upgrades
        public int Cost
        {
            get { return cost; }
            set { this.cost = value; }
        }

        //constructor to take in each piece of data of each button
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
            if (size.Contains(mouseState.Position))
            {
                state = State.Highlighted;
                //if the button is still being presse, not yet released
                if (prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    OnLeftButtonClick();
                    state = State.Pressed;
                }
       
            }
            //if the mouth is not in the bunds of the button
            else
            {
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
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D texture)
        {
            //switch for states of the buttons
            //as of now, only changes colors
            switch (state)
            {
                case (State.Pressed):
                    spriteBatch.Draw(texture, size, Color.Green);
                    break;

                case (State.Highlighted):
                    spriteBatch.Draw(texture, size, Color.Red);
                    break;

                case (State.NonHighlighted):
                    spriteBatch.Draw(texture, size, Color.White);
                    break;

                default:
                    break;
            }

            //prints cost
            spriteBatch.DrawString(spriteFont, $"Cost: {cost}",
                      new Vector2(size.X, size.Y + size.Height), Color.Black);
        }

        public void Purchased()
        {
            shop.Points -= Cost;
        }
    }
}

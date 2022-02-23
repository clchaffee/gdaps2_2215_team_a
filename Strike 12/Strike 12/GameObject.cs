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
    abstract class GameObject
    {
        // ----- | Fields | -----

        // ----- | Constructor | -----

        // Paramatarized Constructor

        // ----- | Property | -----

        // ----- | Methods | -----

        //fields for position and image
        public Texture2D texture;
        public Rectangle position = new Rectangle();

        //property
        protected Rectangle Position
        {
            get { return position; }
        }

        //constructor
        protected GameObject(Texture2D texture, Rectangle position)
        {
            this.texture = texture;
            this.position = position;
        }

        // Abstract Update(GameTime gameTime):
        public abstract void Update(GameTime gameTime);

        // Draw(spriteBatch sb):
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, Color.White);
        }

        //Animation manager moved here?


    }
}

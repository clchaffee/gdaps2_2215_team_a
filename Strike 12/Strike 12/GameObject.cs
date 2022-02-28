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

        //fields for position and image
        protected Texture2D texture;
        protected Rectangle size;

        //property
        protected Rectangle Size
        {
            get { return size; }
        }

        //constructor
        protected GameObject(Texture2D texture, Rectangle size)
        {
            this.texture = texture;
            this.size = size;
        }

        // Abstract Update(GameTime gameTime):
        public abstract void Update(GameTime gameTime);

        // Draw(spriteBatch sb):
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, size, Color.White);
        }

    }
}

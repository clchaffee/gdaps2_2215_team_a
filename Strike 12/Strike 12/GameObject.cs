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

        // Abstract Update(GameTime gameTime):
        public abstract void Update(GameTime gameTime);

        // Draw(spriteBatch sb):
        public void Draw(SpriteBatch sb)
        {
            // TODO: (GameObject) Logic Needed
        }
    }
}

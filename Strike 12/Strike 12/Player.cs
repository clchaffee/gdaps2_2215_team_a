using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
/// <summary>
/// Author: Copious Cats 
/// Purpose: This is a class that will inherit from the GameObject 
/// class and will hold all info that pretains to the player
/// </summary>
namespace Strike_12
{
    class Player : GameObject
    {
        // ----- | Fields | -----
        private KeyboardState kbState;
        //parameters for size and position for the player not currently decided


        // ----- | Constructor | -----
        public Player(Texture2D texture, Rectangle position)
            :base(texture, position)
        {

        }


        // Paramatarized Constructor

        // ----- | Property | -----

        // ----- | Methods | -----

        // -- Methods Overriden from parent class

        /// <summary>
        /// Update contains logic for moving the character left and right for now
        /// needs to be updated, probably by Colby, in order to simulate jumping
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.D))
            { position.X += 1; }
            if (kbState.IsKeyDown(Keys.A))
            { position.X -= 1; }

        }


    }
}

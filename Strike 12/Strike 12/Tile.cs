using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strike_12
{
    /// <summary>
    /// class to hold tile information
    /// </summary>
    class Tile : GameObject, ICollidable 
    {
        private Texture2D texture;
        private Rectangle size;
        private string tileType;

        public Rectangle Size
        {
            get { return this.size; }
        }

        public string Type
        {
            get { return tileType; }
        }

        //constructor
        public Tile(Texture2D texture, Rectangle size, int windowWidth, int windowHeight, string type)
            : base(texture, size, windowWidth, windowHeight)
        {
            this.texture = texture;
            this.size = size;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.tileType = type;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, size, Color.White);
        }
    }
}

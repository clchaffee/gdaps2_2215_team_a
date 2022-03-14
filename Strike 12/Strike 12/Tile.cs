using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strike_12
{
    class Tile : GameObject
    {
        private Texture2D texture;
        private Rectangle size;

        public Rectangle Size()
        {
            return this.size;
        }

        public Tile(Texture2D texture, Rectangle size, int windowWidth, int windowHeight)
            : base(texture, size, windowWidth, windowHeight)
        {
            this.texture = texture;
            this.size = size;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }


        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, new Rectangle(500, 905, 128, 128), Color.White);
        }
    }
}

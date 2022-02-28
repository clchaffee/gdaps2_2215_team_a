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
        public Texture2D texture;
        public Rectangle size;
        public int windowWidth;
        public int windowHeight;
        Vector2 grav = new Vector2(10, 10);

        //property
        protected Rectangle Size
        {
            get { return size; }
        }

        //constructor
        protected GameObject(Texture2D texture, Rectangle size, int windowWidth, int windowHeight)
        {
            this.texture = texture;
            this.size = size;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            //position = new Rectangle((int)grav.X, (int)grav.Y, 10, 10);
        }

        // Abstract Update(GameTime gameTime):
        public abstract void Update(GameTime gameTime);

        // Draw(spriteBatch sb):
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, size, Color.White);
        }

        //collision detection
        public bool CheckCollision(GameObject collider, GameObject collided)
        {
            return (collider.size.Intersects(collided.size));
        }

        //makes a game object bounce when hitting the wall or ground
        public bool Bounce(int windowWidth, int windowHeight)
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;

            //if hits bottom of screen, returns true
            if ((size.Y - size.Height) <= windowHeight)
            {
               return true;
            }
            //if hits top of screen, returns true
            if ((size.Y) >= windowHeight)
            {
                return true;
            }
            //if hits right of screen, returns true
            if ((size.X + size.Width) >= windowWidth)
            {
                return true;
            }
            //if hits left of screen, returns true
            if ((size.X) <= windowWidth)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //screen wraps the character 
        public bool ScreenWrap()
        {
            if (size.X + size.Width >= windowWidth)
            {
                return true;
            }
            else if (size.X <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            //return ((position.X + position.Width >= windowWidth) || (position.X <= 0));

        }

        






    }
}

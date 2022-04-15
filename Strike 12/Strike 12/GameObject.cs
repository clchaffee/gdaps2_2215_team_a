using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
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
        protected int windowWidth;
        protected int windowHeight;
        protected Random rng = new Random();

        //property
        public Rectangle Size
        {
            get { return size; }
            set { size = value; }
        }
        public int SizeY
        {
            get { return size.Y; }
            set { size.Y = value; }
        }
        public int SizeX
        {
            get { return size.X; }
            set { size.X = value; }
        }

        //constructor
        protected GameObject(Texture2D texture, Rectangle size, int windowWidth, int windowHeight)
        {
            this.texture = texture;
            this.size = size;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        // Abstract Update(GameTime gameTime):
        public abstract void Update(GameTime gameTime);

        // Draw(spriteBatch sb):
        protected virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, size, Color.White);
        }

        //collision detection checks type and returns true or false if there was collision
        // Check Top Collision
        public bool IsCollidingTop(GameObject collider, GameObject collidee)
        {
            //return (Rectangle.Intersect(collider.size, collidee.size) != null);
            return collider.size.Bottom > collidee.size.Top &&
                   collider.size.Top < collidee.size.Top &&
                   collider.size.Left < collidee.size.Right &&
                   collider.size.Right > collidee.size.Left;
        }

        // Check Bottom Collision
        public bool IsCollidingBottom(GameObject collider, GameObject collidee)
        {
            //return (Rectangle.Intersect(collider.size, collidee.size) != null);
            return collider.size.Top < collidee.size.Bottom &&
                   collider.size.Bottom > collidee.size.Bottom &&
                   collider.size.Left < collidee.size.Right &&
                   collider.size.Right > collidee.size.Left;
        }

        // Check Left Collision
        public bool IsCollidingLeft(GameObject collider, GameObject collidee, float velocity)
        {
            return //collider.size.Left + velocity <= collidee.size.Right &&
                   collider.size.Left < collidee.size.Right &&
                   collider.size.Right > collidee.size.Right &&
                   collider.size.Top < collidee.size.Bottom - 16 &&
                   collider.size.Bottom > collidee.size.Top + 16;
        }

        public bool IsCollidingRight(GameObject collider, GameObject collidee, float velocity)
        {
            return //collider.size.Right + velocity >= collidee.size.Left &&
                   collider.size.Right > collidee.size.Left &&
                   collider.size.Left < collidee.size.Left &&
                   collider.size.Top < collidee.size.Bottom - 16 &&
                   collider.size.Bottom > collidee.size.Top + 16;
        }

        //makes a game object bounce when hitting the wall or ground
        protected bool Bounce(int windowWidth, int windowHeight)
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;

            //if hits bottom of screen, returns true
            if ((size.Y - size.Height) < windowHeight)
            {
               return true;
            }
            //if hits top of screen, returns true
            else if ((size.Y) > windowHeight)
            {
                return true;
            }
            //if hits right of screen, returns true
            else if ((size.X + size.Width) > windowWidth)
            {
                return true;
            }
            //if hits left of screen, returns true
            else if ((size.X) < windowWidth)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //screen wraps the character 
        protected bool ScreenWrap()
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


        public abstract void Draw(SpriteBatch spriteBatch, Texture2D Texture);

        public virtual void Reset() { }

    }
}

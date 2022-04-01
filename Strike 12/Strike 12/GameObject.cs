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
        protected Random rng;

        //property
        public Rectangle Size
        {
            get { return size; }
            set { size = value; }
        }

        public int XPos
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
        public virtual bool CheckCollision(string type, GameObject collider, GameObject collided)
        {
            switch(type)
            {
                case "enemy":
                    return collider.Size.Intersects(collided.Size);
                    break;
                case "leftWall":
                case "rightWall":
                    return collider.Size.Intersects(collided.Size);
                    break;
                case "ground":
                    return collider.Size.Intersects(collided.Size);
                    break;
                case "platform":
                    return collider.Size.Intersects(collided.Size);
                    break;
                default:
                    return false;
                    break;
            }
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

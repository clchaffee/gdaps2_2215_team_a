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
        public Rectangle position;
        public int windowWidth;
        public int windowHeight;
        Vector2 grav = new Vector2(10, 10);

        //property
        protected Rectangle Position
        {
            get { return position; }
        }

        //constructor
        protected GameObject(Texture2D texture, Rectangle position, int windowWidth, int windowHeight)
        {
            this.texture = texture;
            this.position = position;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            //position = new Rectangle((int)grav.X, (int)grav.Y, 10, 10);
        }

        // Abstract Update(GameTime gameTime):
        public abstract void Update(GameTime gameTime);

        // Draw(spriteBatch sb):
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        //collision detection
        public bool CheckCollision(GameObject collider, GameObject collided)
        {
            return (collider.position.Intersects(collided.position));
        }

        //makes a game object bounce when hitting the wall or ground
        public bool Bounce(int windowWidth, int windowHeight)
        {
            this.windowHeight = windowHeight;
            this.windowWidth = windowWidth;

            //if hits bottom of screen, returns true
            if ((position.Y - position.Height) <= windowHeight)
            {
               return true;
            }
            //if hits top of screen, returns true
            if ((position.Y) >= windowHeight)
            {
                return true;
            }
            //if hits right of screen, returns true
            if ((position.X + position.Width) >= windowWidth)
            {
                return true;
            }
            //if hits left of screen, returns true
            if ((position.X) <= windowWidth)
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
            if (position.X + position.Width >= windowWidth)
            {
                return true;
            }
            else if (position.X <= 0)
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

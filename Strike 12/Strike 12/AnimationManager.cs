using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
/// <summary>
/// To deal with Static and dynamic animation
/// </summary>
namespace Strike_12
{
    class AnimationManager
    {
        // NOTE: SAM will be used for Static Animations and
        // DAM will be used for dynamic animations

        // ===== Fields =====

        // CONSTANTS FOR SAM
        const int ImageSize = 25;

        // FOR DAM
        double currentTime;
        const double TimesPerFrame = .1;
        double frames;

        public double Frames
        {
            get { return frames; }
        }
        public double CurrentTime
        {
            get { return currentTime; }
        }

        // ===== Constructor =====
        public AnimationManager()
        {
            currentTime = 0;
            frames = 0;
        }

        // ===== Methods =====

        // --- SAM Methods ---
        
        // CheckMouse():
        private Rectangle CheckMouse(Rectangle rect, MouseState mState)
        {
            if (rect.Contains(mState.Position))
            {
                return new Rectangle(rect.X, rect.Y, ImageSize * 1, ImageSize);
            }
            else
            {
                return new Rectangle(rect.X, rect.Y, ImageSize * 0, ImageSize);
            }
        }

        // Draw()
        public void Draw(SpriteBatch _spriteBatch, Texture2D texture, Rectangle rect, MouseState mState)
        {
            _spriteBatch.Draw(texture, CheckMouse(rect, mState), Color.White);
        }

        // --- DAM Methods

        // Update()
        public void Update(GameTime gameTime, int frameAmount, double frameRate)
        {
            currentTime = gameTime.TotalGameTime.TotalSeconds;

            currentTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime >= TimesPerFrame)
            {
                frames += frameRate;

                if (frames > frameAmount)
                {
                    frames = 0;
                }
            }

            currentTime -= TimesPerFrame;
        }

        // Draw()
        public void Draw(SpriteBatch _spriteBatch, Texture2D texture, Rectangle rect, SpriteEffects effect, float rotation)
        {
            _spriteBatch.Draw(
                texture,
                rect,
                new Rectangle(
                    0 + (64 * (int)Math.Floor(frames)),
                    0,
                    rect.Width,
                    rect.Height),
                Color.White,
                rotation,
                Vector2.Zero,
                effect,
                0f); 

        }

        public void DrawShopKeeper(SpriteBatch _spriteBatch, Texture2D texture, Rectangle rect, SpriteEffects effect, float rotation)
        {
            _spriteBatch.Draw(
                texture,
                rect,
                new Rectangle(
                    0 + (130 * (int)Math.Floor(frames)),
                    0,
                    rect.Width,
                    rect.Height),
                Color.White,
                rotation,
                Vector2.Zero,
                effect,
                0f);

        }
    }
}

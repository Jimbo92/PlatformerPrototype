using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Platformer_Prototype
{
    class Sprite
    {
        enum ESpriteType
        {
            Basic,
            Anim
        };
        private ESpriteType SpriteType = ESpriteType.Basic;

        public Texture2D Texture;
        public int Rows;
        public int Columns;
        public float CurrentFrame;
        public float TotalFrames;
        public Rectangle sourceRectangle;
        public Rectangle destinationRectangle;
        public int Width;
        public int Height;
        public Rectangle CollisionBox = Rectangle.Empty;
        public bool AnimFinnished = false;

        public Sprite(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            SpriteType = ESpriteType.Basic;
            Texture = getContent.Load<Texture2D>(getTexture);
            Width = getWidth;
            Height = getHeight;
        }
        public Sprite(ContentManager getContent, string getTexture, int getWidth, int getHeight, int getRows, int getColumns)
        {
            SpriteType = ESpriteType.Anim;
            Texture = getContent.Load<Texture2D>(getTexture);
            Width = getWidth;
            Height = getHeight;
            Rows = getRows;
            Columns = getColumns;
            CurrentFrame = 0;
            TotalFrames = Rows * Columns;
        }

        public void UpdateAnimation(float getDelay)
        {
            CurrentFrame += getDelay;
            if (CurrentFrame >= TotalFrames)
            {
                AnimFinnished = true;
                CurrentFrame = 0;
            }
        }

        public void Draw(SpriteBatch sB, Vector2 getPosition, float getRotation, SpriteEffects getEffects)
        {
            CollisionBox = new Rectangle((int)getPosition.X - Width / 2, (int)getPosition.Y - Width / 2, Width + 2, Height + 2);

            if (SpriteType == ESpriteType.Basic)
            {
                destinationRectangle = new Rectangle((int)getPosition.X, (int)getPosition.Y, Width, Height);
                sB.Draw(Texture,
                    destinationRectangle,
                    null,
                    Color.White,
                    getRotation,
                    new Vector2(destinationRectangle.Width / 2, destinationRectangle.Height / 2),
                    getEffects,
                    0);
            }
            if (SpriteType == ESpriteType.Anim)
            {
                int sourceWidth = Texture.Width / Columns;
                int sourceHeight = Texture.Height / Rows;

                int row = (int)((float)CurrentFrame / (float)Columns);
                int column = (int)CurrentFrame % Columns;

                sourceRectangle = new Rectangle(sourceWidth * column, sourceHeight * row, sourceWidth, sourceHeight);
                destinationRectangle = new Rectangle((int)getPosition.X, (int)getPosition.Y, Width, Height);

                sB.Draw(Texture,
                    destinationRectangle,
                    sourceRectangle,
                    Color.White,
                    getRotation,
                    new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2),
                    getEffects,
                    0);
            }
        }
        public void Draw(SpriteBatch sB, Vector2 getPosition, Vector2 getOrigin, float getRotation, SpriteEffects getEffects, Color getColour)
        {
            CollisionBox = new Rectangle((int)getPosition.X, (int)getPosition.Y, Width + 2, Height + 2);

            if (SpriteType == ESpriteType.Basic)
            {
                destinationRectangle = new Rectangle((int)getPosition.X, (int)getPosition.Y, Width, Height);
                sB.Draw(Texture,
                    destinationRectangle,
                    null,
                    getColour,
                    getRotation,
                    getOrigin,
                    getEffects,
                    0);
            }
            if (SpriteType == ESpriteType.Anim)
            {
                int sourceWidth = Texture.Width / Columns;
                int sourceHeight = Texture.Height / Rows;

                int row = (int)((float)CurrentFrame / (float)Columns);
                int column = (int)CurrentFrame % Columns;

                sourceRectangle = new Rectangle(sourceWidth * column, sourceHeight * row, sourceWidth, sourceHeight);
                destinationRectangle = new Rectangle((int)getPosition.X, (int)getPosition.Y, Width, Height);

                sB.Draw(Texture,
                    destinationRectangle,
                    sourceRectangle,
                    getColour,
                    getRotation,
                    getOrigin,
                    getEffects,
                    0);
            }
        }
    }
}

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
    class Item
    {
        public Sprite sprite;
        public Vector2 Position;
        public bool isDrawn = true;
        private int Width;
        private int Height;
        public bool isCollected = false;


        public Item(ContentManager getContent, string getTexture, int getWidth, int getHeight)
        {
            Width = getWidth;
            Height = getHeight;
            sprite = new Sprite(getContent, getTexture, getWidth, getHeight);
        }

        public void Draw(SpriteBatch sB, BaseEngine getBengine)
        {
            Position.X = getBengine.tileDraw.X;
            Position.Y = getBengine.tileDraw.Y;

            if (isDrawn)
            {
                sprite.Draw(sB, new Vector2(Position.X, Position.Y), Vector2.Zero, 0, SpriteEffects.None, Color.White);
            }
        }

    }
}

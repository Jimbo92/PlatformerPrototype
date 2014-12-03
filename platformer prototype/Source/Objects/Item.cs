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
        public enum EItemType
        {
            Crystal,
            Coin,
            Breakable
        };

        public EItemType ItemType;
        public Sprite sprite;
        public Vector2 Position;
        public bool isDrawn = true;
        private int Width;
        private int Height;

        private Texture2D GlowEff;
        private float GlowEff_Rot;
        private Color Colour = Color.White;


        public Item(ContentManager getContent, string getTexture, int getWidth, int getHeight, EItemType getType)
        {
            ItemType = getType;
            Width = getWidth;
            Height = getHeight;
            sprite = new Sprite(getContent, getTexture, getWidth, getHeight);
            GlowEff = getContent.Load<Texture2D>("objects/items/itemglow");
        }

        public void Update()
        {
            //Glow effect Anim
            GlowEff_Rot++;
            if (GlowEff_Rot > 360)
                GlowEff_Rot = 0;

            if (ItemType == EItemType.Coin)
                Colour = Color.LightGoldenrodYellow;
        }

        public void Draw(SpriteBatch sB, BaseEngine getBengine)
        {
            Position.X = getBengine.tileDraw.X;
            Position.Y = getBengine.tileDraw.Y;

            if (isDrawn)
            {
                if (ItemType != EItemType.Breakable)
                    sB.Draw(GlowEff, new Rectangle((int)Position.X + 16, (int)Position.Y + 16, 36, 36), null, Colour, MathHelper.ToRadians(GlowEff_Rot), new Vector2(39, 39), SpriteEffects.None, 0);
                sprite.Draw(sB, new Vector2(Position.X, Position.Y), Vector2.Zero, 0, SpriteEffects.None, Color.White);
            }
        }
    }
}

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
    class Lever
    {
        public Rectangle Rect;
        public Texture2D Tex;
        public bool isOn = false;
        public int id;
        private Color Colour;

        public void Update(BaseEngine getBengine)
        {
            if (getBengine.player.Bounds.Intersects(new Rectangle(Rect.X + (int)Camera.Position.X, Rect.Y + (int)Camera.Position.Y, 32, 32)))
            {
                if (Input.KeyboardPressed(Keys.Enter))
                    if (!isOn)
                        isOn = true;
                    else
                        isOn = false;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            if (id == 1)
                Colour = Color.LightBlue;
            if (id == 2)
                Colour = Color.LightGreen;
            if (id == 3)
                Colour = Color.Orange;
            if (id == 4)
                Colour = Color.LightYellow;

            if (isOn)
                Tex = Textures._OBJ_Switch_Tex[1];
            else
                Tex = Textures._OBJ_Switch_Tex[0];

            sB.Draw(Tex, new Rectangle(Rect.X + (int)Camera.Position.X, Rect.Y + (int)Camera.Position.Y, Rect.Width, Rect.Height), Colour);
        }

    }
}

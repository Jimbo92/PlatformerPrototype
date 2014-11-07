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
    class Door
    {
        public enum color
        {
            blue = 1,
            green = 2,
            red = 3,
            yellow = 4,
        }
        public color Shade;

       
        public Rectangle rect;
        public Texture2D texture;
        public bool open;
        public int id;

        public void Update(BaseEngine getBengine)
        {
            foreach (Lever l in getBengine.Switches)
            {
                if (l.id == id)
                {
                    
                    open = l.isOn;
                }
             
            }

        }

        public bool checkDoors(BaseEngine getBengine)
        {
           
            if (getBengine.player.Bounds.Intersects(new Rectangle(rect.X + (int)Camera.Position.X, rect.Y + (int)Camera.Position.Y, 32, 32)) && !open)
            {
     
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            if (!open)
            {      
                texture = Textures._OBJ_Lock_Tex[(int)Shade];
                sB.Draw(texture, new Rectangle(rect.X + (int)Camera.Position.X, rect.Y + (int)Camera.Position.Y, rect.Width, rect.Height), Color.White);
            }
        }
    }
}

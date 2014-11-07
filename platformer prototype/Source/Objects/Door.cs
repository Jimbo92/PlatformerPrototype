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
        public Rectangle rect;
        public Texture2D texture;
        public bool open;
        public int id;

        public void Update(BaseEngine getBengine)
        {
            foreach (Lever l in getBengine.Switches)
                if (l.id == id)
                    open = l.isOn;
        }

        public bool checkDoors(BaseEngine getBengine)
        {     
            if (getBengine.player.Bounds.Intersects(new Rectangle(rect.X + (int)Camera.Position.X, rect.Y + (int)Camera.Position.Y, 32, 32)) && !open)
                return true;
            else
                return false;
        }

        public void Draw(SpriteBatch sB)
        {
            if (id == 1)
                texture = Textures._OBJ_Lock_Tex[0];
            if (id == 2)
                texture = Textures._OBJ_Lock_Tex[1];
            if (id == 3)
                texture = Textures._OBJ_Lock_Tex[2];
            if (id == 4)
                texture = Textures._OBJ_Lock_Tex[3];

            if (!open)                 
                sB.Draw(texture, new Rectangle(rect.X + (int)Camera.Position.X, rect.Y + (int)Camera.Position.Y, rect.Width, rect.Height), Color.White);
        }
    }
}

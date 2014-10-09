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
    static class Textures
    {
        //Object Textures
        static public Texture2D _OBJ_Ladder_Tex;

        //Map Textures
        static public Texture2D _TILE_Dirt_Tex;
        static public Texture2D _TILE_Grass_Tex;

        //Debug Textures
        static public Texture2D _DBG_DebugPlain_Tex;


        static public void LoadContent(ContentManager getContent)
        {
            //Object Textures
            _OBJ_Ladder_Tex = getContent.Load<Texture2D>("Ladder");

            //Map Textures
            _TILE_Dirt_Tex = getContent.Load<Texture2D>("Dirt");
            _TILE_Grass_Tex = getContent.Load<Texture2D>("Grass");

            //Debug Textures
            _DBG_DebugPlain_Tex = getContent.Load<Texture2D>("level");
        }
    }
}

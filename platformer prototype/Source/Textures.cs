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
        //Grass
        static public Texture2D[] _TILE_Grass_Tex = new Texture2D[7];
        //Dirt
        static public Texture2D[] _TILE_Dirt_Tex = new Texture2D[7];

        //Effects
        static public Texture2D _TILE_Shade_Effect;

        //Debug Textures
        static public Texture2D _DBG_DebugPlain_Tex;
        static public Texture2D _DBG_Line_Tex;

        static public void LoadContent(ContentManager getContent)
        {
            //Object Textures
            _OBJ_Ladder_Tex = getContent.Load<Texture2D>("tiles/Ladder");
            //_OBJ_Grass_Tex = getContent.Load<Texture2D>("objects/grass4");

            //Map Textures
            //Grass & Dirt
            for (int i = 0; i < 7; i++)
            {
                _TILE_Grass_Tex[i] = getContent.Load<Texture2D>("tiles/grass" + i);
                _TILE_Dirt_Tex[i] = getContent.Load<Texture2D>("tiles/dirt" + i);
            }

            //Effects
            _TILE_Shade_Effect = getContent.Load<Texture2D>("tiles/shade");


            //Debug Textures
            _DBG_DebugPlain_Tex = getContent.Load<Texture2D>("debug/level");
            _DBG_Line_Tex = getContent.Load<Texture2D>("debug/lineTex");
        }

        static public void Update()
        {

        }


        //Draw Trigger Map Data & Base Texture Tiles
        public static void DrawTriggerMapData(SpriteBatch sB, BaseEngine Bengine, Game1 game1)
        {
            Rectangle tileDraw;

            for (int i = 0; i < Bengine.map.GetLength(1); i++)
                for (int j = Bengine.map.GetLength(0) - 1; j > -1; j--)
                {
                    tileDraw = new Rectangle((Bengine.tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (Bengine.tileSize * (Bengine.map.GetLength(0) - j)) + (int)Camera.Position.Y, Bengine.tileSize, Bengine.tileSize);

                    if (tileDraw.X > 0 - Bengine.tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                        if (tileDraw.Y > 0 - Bengine.tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                        {
                            //----------------------------------------------------//Triggers//----------------------------------------------------//

                            //if (Bengine.map[j, i] == 1)
                            //    sB.Draw(_DBG_DebugPlain_Tex, tileDraw, Color.White);
                            if (Bengine.map[j, i] == 2)
                                sB.Draw(_OBJ_Ladder_Tex, tileDraw, Color.White);

                            //----------------------------------------------------//Textures//----------------------------------------------------//


                            //Grass Tile Left
                            if (Bengine.ForeMapTextures[j, i] == 1)
                                sB.Draw(_TILE_Grass_Tex[0], tileDraw, Color.White);
                            //Grass Tile Mid
                            if (Bengine.ForeMapTextures[j, i] == 2)
                                sB.Draw(_TILE_Grass_Tex[1], tileDraw, Color.White);
                            //Grass Tile Right
                            if (Bengine.ForeMapTextures[j, i] == 3)
                                sB.Draw(_TILE_Grass_Tex[2], tileDraw, Color.White);
                            //Grass Tile Single
                            if (Bengine.ForeMapTextures[j, i] == 4)
                                sB.Draw(_TILE_Grass_Tex[3], tileDraw, Color.White);
                            //Grass Tile NoTop
                            if (Bengine.ForeMapTextures[j, i] == 5)
                                sB.Draw(_TILE_Grass_Tex[4], tileDraw, Color.White);
                            //Grass Tile Hill Left
                            if (Bengine.ForeMapTextures[j, i] == 6)
                                sB.Draw(_TILE_Grass_Tex[5], tileDraw, Color.White);
                            //Grass Tile Hill Right
                            if (Bengine.ForeMapTextures[j, i] == 7)
                                sB.Draw(_TILE_Grass_Tex[6], tileDraw, Color.White);

                            //Dirt Tile Left
                            if (Bengine.ForeMapTextures[j, i] == 8)
                                sB.Draw(_TILE_Dirt_Tex[0], tileDraw, Color.White);
                            //Dirt Tile Mid
                            if (Bengine.ForeMapTextures[j, i] == 9)
                                sB.Draw(_TILE_Dirt_Tex[1], tileDraw, Color.White);
                            //Dirt Tile Right
                            if (Bengine.ForeMapTextures[j, i] == 10)
                                sB.Draw(_TILE_Dirt_Tex[2], tileDraw, Color.White);
                            //Dirt Tile Single
                            if (Bengine.ForeMapTextures[j, i] == 11)
                                sB.Draw(_TILE_Dirt_Tex[3], tileDraw, Color.White);
                            //Dirt Tile NoTop
                            if (Bengine.ForeMapTextures[j, i] == 12)
                                sB.Draw(_TILE_Dirt_Tex[4], tileDraw, Color.White);
                            //Dirt Tile Hill Left
                            if (Bengine.ForeMapTextures[j, i] == 13)
                                sB.Draw(_TILE_Dirt_Tex[5], tileDraw, Color.White);
                            //Dirt Tile Hill Right
                            if (Bengine.ForeMapTextures[j, i] == 14)
                                sB.Draw(_TILE_Dirt_Tex[6], tileDraw, Color.White);



                            //----------------------------------------------------//Objects//----------------------------------------------------//

                            //Draw Torch Obj
                            //if (Bengine.ForeMapTextures[j, i] == 6)
                            //    Bengine.Torch.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                        }
                }
        }

        //Draw Background Map Textures
        public static void DrawBackgroundMapTextures(SpriteBatch sB, BaseEngine Bengine, Game1 game1)
        {
            Rectangle RectTextureTile;

            for (int i = 0; i < Bengine.BackMapTextures.GetLength(1); i++)
                for (int j = Bengine.BackMapTextures.GetLength(0) - 1; j > -1; j--)
                {
                    RectTextureTile = new Rectangle((Bengine.tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (Bengine.tileSize * (Bengine.BackMapTextures.GetLength(0) - j)) + (int)Camera.Position.Y, Bengine.tileSize, Bengine.tileSize);

                    if (RectTextureTile.X > 0 - Bengine.tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                        if (RectTextureTile.Y > 0 - Bengine.tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                        {
                            //----------------------------------------------------//Textures//----------------------------------------------------//

                            //Draw Grass
                            if (Bengine.BackMapTextures[j, i] == 1)
                                sB.Draw(Textures._TILE_Grass_Tex[4], RectTextureTile, Color.Gray);

                            //Draw Dirt
                            if (Bengine.BackMapTextures[j, i] == 2)
                                sB.Draw(Textures._TILE_Dirt_Tex[4], RectTextureTile, Color.Gray);

                        }
                }
        }

        //Draw Foreground Map Textures
        public static void DrawForegroundMapTextures(SpriteBatch sB, BaseEngine Bengine, Game1 game1)
        {
            Rectangle RectTextureTile;

            for (int i = 0; i < Bengine.ForeMapTextures.GetLength(1); i++)
                for (int j = Bengine.ForeMapTextures.GetLength(0) - 1; j > -1; j--)
                {
                    RectTextureTile = new Rectangle((Bengine.tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (Bengine.tileSize * (Bengine.ForeMapTextures.GetLength(0) - j)) + (int)Camera.Position.Y, Bengine.tileSize, Bengine.tileSize);

                    if (RectTextureTile.X > 0 - Bengine.tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                        if (RectTextureTile.Y > 0 - Bengine.tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                        {
                            //----------------------------------------------------//Textures//----------------------------------------------------//

                            //Draw Water Top Tile
                            if (Bengine.ForeMapTextures[j, i] == 15)
                                Bengine.WaterTop.Draw(sB, new Vector2(RectTextureTile.X, RectTextureTile.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                            
                            //Draw Water Base Tile
                            if (Bengine.ForeMapTextures[j, i] == 16)
                                Bengine.WaterBase.Draw(sB, new Vector2(RectTextureTile.X, RectTextureTile.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                            
                            
                            
                            ////----------------------------------------------------//Objects//----------------------------------------------------//
                            //
                            ////Draw Grass Obj
                            //if (Bengine.ForeMapTextures[j, i] == 5)
                            //    sB.Draw(Textures._OBJ_Grass_Tex, RectTextureTile, Color.White);


                            //----------------------------------------------------//Effects//----------------------------------------------------//

                            //Shade Tile Effect
                            if (Bengine.ForeMapTextures[j, i] == 17)
                                sB.Draw(_TILE_Shade_Effect, RectTextureTile, Color.White);

                        }
                }

        }
    }
}

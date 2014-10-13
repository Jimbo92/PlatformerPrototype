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
        public enum ETextureType
        {
            INGAME,
            EDITOR,
            SELECTOR,
        };

        static public ETextureType TextureType = ETextureType.EDITOR;

        //Object Textures
        static public Texture2D _OBJ_Ladder_Tex;

        //Map Textures
        //Grass
        static public Texture2D[] _TILE_Grass_Tex = new Texture2D[7];
        //Dirt
        static public Texture2D[] _TILE_Dirt_Tex = new Texture2D[7];

        //Effects
        static public Texture2D[] _TILE_Shade_Effect = new Texture2D[3];

        //Debug Textures
        static public Texture2D[] _DBG_Trigger_Tex = new Texture2D[3];
        static public Texture2D _DBG_Line_Tex;

        static private BaseEngine Bengine;
        static private int SelectorOffset = 40;
        static private Rectangle tileDraw = new Rectangle();

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

            //Effects & Editor
            for (int i = 0; i < 3; i++)
            {
                _TILE_Shade_Effect[i] = getContent.Load<Texture2D>("tiles/effects/shade" + i);
                _DBG_Trigger_Tex[i] = getContent.Load<Texture2D>("editor/trigger" + i);
            }


            //Debug Textures

            _DBG_Line_Tex = getContent.Load<Texture2D>("debug/lineTex");
        }

        static public void Update(BaseEngine getBengine)
        {
            Bengine = getBengine;
        }


        //Draw Trigger Map Data
        public static void DrawTriggerMapData(SpriteBatch sB, int[,] MapData, int tileSize, Vector2 Offset, Game1 game1)
        {
            for (int i = 0; i < MapData.GetLength(1); i++)
                for (int j = MapData.GetLength(0) - 1; j > -1; j--)
                {
                    switch (TextureType)
                    {
                        case ETextureType.INGAME: tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (MapData.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize); break;
                        case ETextureType.EDITOR: tileDraw = new Rectangle((int)Camera.Position.X + tileSize * i, (int)Camera.Position.Y + tileSize * j, tileSize, tileSize); break;
                        case ETextureType.SELECTOR: tileDraw = new Rectangle((int)Offset.X + SelectorOffset * i, (int)Offset.Y + SelectorOffset * j, tileSize, tileSize); break;
                    }
                    if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                        if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                        {
                            //----------------------------------------------------//Triggers//----------------------------------------------------//
                            if (Global_GameState.GameState == Global_GameState.EGameState.EDITOR)
                            {
                                //Trigger Full Block Texture
                                if (MapData[j, i] == 1)
                                    sB.Draw(_DBG_Trigger_Tex[0], tileDraw, Color.White);
                                //Trigger Half Block Left Texture
                                if (MapData[j, i] == 4)
                                    sB.Draw(_DBG_Trigger_Tex[1], tileDraw, Color.White);
                                //Trigger Half Block Right Texture
                                if (MapData[j, i] == 5)
                                    sB.Draw(_DBG_Trigger_Tex[1], tileDraw, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                                //Trigger Water Block Texture
                                if (MapData[j, i] == 3)
                                    sB.Draw(_DBG_Trigger_Tex[2], tileDraw, Color.White);
                            }

                            if (MapData[j, i] == 2)
                                sB.Draw(_OBJ_Ladder_Tex, tileDraw, Color.White);



                            //----------------------------------------------------//Objects//----------------------------------------------------//

                            //Draw Torch Obj
                            //if (Bengine.ForeMapTextures[j, i] == 6)
                            //    Bengine.Torch.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                        }
                }
        }

        //Draw Background Map Textures
        public static void DrawBackgroundMapTextures(SpriteBatch sB, int[,] MapData, int tileSize, Vector2 Offset, Game1 game1)
        {
            for (int i = 0; i < MapData.GetLength(1); i++)
                for (int j = MapData.GetLength(0) - 1; j > -1; j--)
                {
                    switch (TextureType)
                    {
                        case ETextureType.INGAME: tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (MapData.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize); break;
                        case ETextureType.EDITOR: tileDraw = new Rectangle((int)Camera.Position.X + tileSize * i, (int)Camera.Position.Y + tileSize * j, tileSize, tileSize); break;
                        case ETextureType.SELECTOR: tileDraw = new Rectangle((int)Offset.X + SelectorOffset * i, (int)Offset.Y + SelectorOffset * j, tileSize, tileSize); break;
                    }
                    if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                        if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                        {
                            //----------------------------------------------------//Textures Back//----------------------------------------------------//

                            //Draw Grass
                            if (MapData[j, i] == 15)
                                sB.Draw(Textures._TILE_Grass_Tex[4], tileDraw, Color.Gray);

                            //Draw Dirt
                            if (MapData[j, i] == 16)
                                sB.Draw(Textures._TILE_Dirt_Tex[4], tileDraw, Color.Gray);

                            //----------------------------------------------------//Textures ForeBack//----------------------------------------------------//

                            //Grass Tile Left
                            if (MapData[j, i] == 1)
                                sB.Draw(_TILE_Grass_Tex[0], tileDraw, Color.White);
                            //Grass Tile Mid
                            if (MapData[j, i] == 2)
                                sB.Draw(_TILE_Grass_Tex[1], tileDraw, Color.White);
                            //Grass Tile Right
                            if (MapData[j, i] == 3)
                                sB.Draw(_TILE_Grass_Tex[2], tileDraw, Color.White);
                            //Grass Tile Single
                            if (MapData[j, i] == 4)
                                sB.Draw(_TILE_Grass_Tex[3], tileDraw, Color.White);
                            //Grass Tile NoTop
                            if (MapData[j, i] == 5)
                                sB.Draw(_TILE_Grass_Tex[4], tileDraw, Color.White);
                            //Grass Tile Hill Left
                            if (MapData[j, i] == 6)
                                sB.Draw(_TILE_Grass_Tex[5], tileDraw, Color.White);
                            //Grass Tile Hill Right
                            if (MapData[j, i] == 7)
                                sB.Draw(_TILE_Grass_Tex[6], tileDraw, Color.White);

                            //Dirt Tile Left
                            if (MapData[j, i] == 8)
                                sB.Draw(_TILE_Dirt_Tex[0], tileDraw, Color.White);
                            //Dirt Tile Mid
                            if (MapData[j, i] == 9)
                                sB.Draw(_TILE_Dirt_Tex[1], tileDraw, Color.White);
                            //Dirt Tile Right
                            if (MapData[j, i] == 10)
                                sB.Draw(_TILE_Dirt_Tex[2], tileDraw, Color.White);
                            //Dirt Tile Single
                            if (MapData[j, i] == 11)
                                sB.Draw(_TILE_Dirt_Tex[3], tileDraw, Color.White);
                            //Dirt Tile NoTop
                            if (MapData[j, i] == 12)
                                sB.Draw(_TILE_Dirt_Tex[4], tileDraw, Color.White);
                            //Dirt Tile Hill Left
                            if (MapData[j, i] == 13)
                                sB.Draw(_TILE_Dirt_Tex[5], tileDraw, Color.White);
                            //Dirt Tile Hill Right
                            if (MapData[j, i] == 14)
                                sB.Draw(_TILE_Dirt_Tex[6], tileDraw, Color.White);

                        }
                }
        }

        //Draw Foreground Map Textures
        public static void DrawForegroundMapTextures(SpriteBatch sB, int[,] MapData, int tileSize, Vector2 Offset, Game1 game1)
        {         
            for (int i = 0; i < MapData.GetLength(1); i++)
                for (int j = MapData.GetLength(0) - 1; j > -1; j--)
                {
                    switch (TextureType)
                    {
                        case ETextureType.INGAME: tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (MapData.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize); break;
                        case ETextureType.EDITOR: tileDraw = new Rectangle((int)Camera.Position.X + tileSize * i, (int)Camera.Position.Y + tileSize * j, tileSize, tileSize); break;
                        case ETextureType.SELECTOR: tileDraw = new Rectangle((int)Offset.X + SelectorOffset * i, (int)Offset.Y + SelectorOffset * j, tileSize, tileSize); break;
                    }

                    if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                        if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                        {
                            //----------------------------------------------------//Textures//----------------------------------------------------//




                            ////----------------------------------------------------//Objects//----------------------------------------------------//
                            //
                            ////Draw Grass Obj
                            //if (Bengine.ForeMapTextures[j, i] == 5)
                            //    sB.Draw(Textures._OBJ_Grass_Tex, RectTextureTile, Color.White);

                        }
                }
        }

        //Draw Map Effects
        public static void DrawMapEffects(SpriteBatch sB, int[,] MapData, int tileSize, Vector2 Offset, Game1 game1)
        {
            for (int i = 0; i < MapData.GetLength(1); i++)
                for (int j = MapData.GetLength(0) - 1; j > -1; j--)
                {
                    switch (TextureType)
                    {
                        case ETextureType.INGAME: tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (MapData.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize); break;
                        case ETextureType.EDITOR: tileDraw = new Rectangle((int)Camera.Position.X + tileSize * i, (int)Camera.Position.Y + tileSize * j, tileSize, tileSize); break;
                        case ETextureType.SELECTOR: tileDraw = new Rectangle((int)Offset.X + SelectorOffset * i, (int)Offset.Y + SelectorOffset * j, tileSize, tileSize); break;
                    }

                    if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                        if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                        {
                            ////----------------------------------------------------//Objects//----------------------------------------------------//
                            //
                            ////Draw Grass Obj
                            //if (Bengine.ForeMapTextures[j, i] == 5)
                            //    sB.Draw(Textures._OBJ_Grass_Tex, RectTextureTile, Color.White);


                            //----------------------------------------------------//Effects//----------------------------------------------------//

                            //Shade Tile Effect Full
                            if (MapData[j, i] == 1)
                                sB.Draw(_TILE_Shade_Effect[0], tileDraw, Color.White);
                            //Shade Tile Effect Down Half Left
                            if (MapData[j, i] == 2)
                                sB.Draw(_TILE_Shade_Effect[1], tileDraw, Color.White);
                            //Shade Tile Effect Down Half Right
                            if (MapData[j, i] == 3)
                                sB.Draw(_TILE_Shade_Effect[1], tileDraw, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
                            //Shade Tile Effect Up Half Left
                            if (MapData[j, i] == 4)
                                sB.Draw(_TILE_Shade_Effect[2], tileDraw, Color.White);
                            //Shade Tile Effect Up Half Right
                            if (MapData[j, i] == 5)
                                sB.Draw(_TILE_Shade_Effect[2], tileDraw, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);

                        }
                }
        }
    }
}

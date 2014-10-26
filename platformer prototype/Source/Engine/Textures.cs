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
        static public Texture2D _ITEM_Crystal_Tex;
        static public Texture2D _ITEM_WoodBox_Tex;
        static public Texture2D[] _OBJ_Platforms_Tex = new Texture2D[2];

        //Map Textures
        //Grass
        static public Texture2D _TILE_GrassSS_Tex;
        //Dirt
        static public Texture2D _TILE_DirtSS_Tex;
        //Sand
        static public Texture2D _TILE_SandSS_Tex;
        //Snow
        static public Texture2D _TILE_SnowSS_Tex;
        //Castle
        static public Texture2D _TILE_CastleSS_Tex;
        //Metal
        static public Texture2D _TILE_MetalSS_Tex;


        //Effects
        static public Texture2D[] _TILE_Shade_Effect = new Texture2D[3];

        //Debug Textures
        static public Texture2D[] _DBG_Trigger_Tex = new Texture2D[8];
        static public Texture2D[] _DBG_ETrigger_Tex = new Texture2D[3];
        static public Texture2D[] _DBG_Lava_Tex = new Texture2D[2];
        static public Texture2D _DBG_WaterTop_Tex;
        static public Texture2D _DBG_WaterBot_Tex;
        static public Texture2D _DBG_Line_Tex;

        static private BaseEngine Bengine;
        static private int SelectorOffset = 40;
        static private Rectangle tileDraw = new Rectangle();



        static public void LoadContent(ContentManager getContent)
        {
            //Object Textures
            _OBJ_Ladder_Tex = getContent.Load<Texture2D>("tiles/Ladder");
            //_OBJ_Grass_Tex = getContent.Load<Texture2D>("objects/grass4");
            _ITEM_Crystal_Tex = getContent.Load<Texture2D>("objects/items/gemblue");
            _ITEM_WoodBox_Tex = getContent.Load<Texture2D>("objects/box");
            for (int i = 0; i < 2; i++)
            {
                _OBJ_Platforms_Tex[i] = getContent.Load<Texture2D>("editor/triggerplatform" + i);
            }



            //Map Textures
            //Grass
            _TILE_GrassSS_Tex = getContent.Load<Texture2D>("tiles/grass");
            //Dirt
            _TILE_DirtSS_Tex = getContent.Load<Texture2D>("tiles/dirt");
            //Sand
            _TILE_SandSS_Tex = getContent.Load<Texture2D>("tiles/sand");
            //Snow
            _TILE_SnowSS_Tex = getContent.Load<Texture2D>("tiles/snow");
            //Castle
            _TILE_CastleSS_Tex = getContent.Load<Texture2D>("tiles/castle");
            //Metal
            _TILE_MetalSS_Tex = getContent.Load<Texture2D>("tiles/metal");


            //Effects
            for (int i = 0; i < 3; i++)
            {
                _TILE_Shade_Effect[i] = getContent.Load<Texture2D>("tiles/effects/shade" + i);
            }


            //Debug Textures & Editor
            for (int i = 0; i < 8; i++)
            {
                _DBG_Trigger_Tex[i] = getContent.Load<Texture2D>("editor/trigger" + i);
            }
            for (int i = 0; i < 3; i++)
            {
                _DBG_ETrigger_Tex[i] = getContent.Load<Texture2D>("editor/etrigger" + i);
            }
            for (int i = 0; i < 2; i++)
            {
                _DBG_Lava_Tex[i] = getContent.Load<Texture2D>("editor/lava" + i);
            }
            _DBG_Line_Tex = getContent.Load<Texture2D>("debug/lineTex");
            _DBG_WaterTop_Tex = getContent.Load<Texture2D>("editor/watertopeditor");
            _DBG_WaterBot_Tex = getContent.Load<Texture2D>("editor/waterbottomeditor");
        }

        static public void Update(BaseEngine getBengine)
        {
            Bengine = getBengine;
        }

        static private void SpriteSheetDraw(SpriteBatch sB, Texture2D getTexture, Rectangle getRectangle, Color getColour, int getColumns, int getRows, int getFrame)
        {
            Rectangle sourceRectangle;

            int Columns = getColumns;
            int Rows = getRows;

            int sourceWidth = getTexture.Width / Columns;
            int sourceHeight = getTexture.Height / Rows;

            int row = getFrame / Columns;
            int column = getFrame % Columns;

            sourceRectangle = new Rectangle(sourceWidth * column, sourceHeight * row, sourceWidth, sourceHeight);

            sB.Draw(getTexture, getRectangle, sourceRectangle, getColour);
        }

        //Draw Trigger Map Data
        public static void DrawTriggerMapData(SpriteBatch sB, char[,] MapData, int tileSize, Vector2 Offset, Game1 game1)
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
                                if (MapData[j, i] == '☺')
                                    sB.Draw(_DBG_Trigger_Tex[0], tileDraw, Color.White);
                                //Trigger Side Block Left Texture
                                if (MapData[j, i] == '☻')
                                    sB.Draw(_DBG_Trigger_Tex[1], tileDraw, Color.White);
                                //Trigger Side Block Right Texture
                                if (MapData[j, i] == '♥')
                                    sB.Draw(_DBG_Trigger_Tex[1], tileDraw, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                                //Trigger Half Block Texture
                                if (MapData[j, i] == '♦')
                                    sB.Draw(_DBG_Trigger_Tex[3], tileDraw, Color.White);
                                //Trigger Water Block Texture
                                if (MapData[j, i] == '♣')
                                    sB.Draw(_DBG_Trigger_Tex[2], tileDraw, Color.White);
                                //Trigger Lava Block Texture
                                if (MapData[j, i] == '•')
                                    sB.Draw(_DBG_Trigger_Tex[4], tileDraw, Color.White);
                                //Crystal Item
                                if (MapData[j, i] == '◘')
                                    sB.Draw(_ITEM_Crystal_Tex, tileDraw, Color.White);
                                //Trigger Player Start
                                if (MapData[j, i] == '○')
                                    sB.Draw(_DBG_Trigger_Tex[5], tileDraw, Color.White);
                                //Trigger One way platforms
                                if (MapData[j, i] == '◙')
                                    sB.Draw(_DBG_Trigger_Tex[7], tileDraw, Color.White);
                                //Trigger Enemy Spawner
                                //if (MapData[j, i] == '♂')
                                //    sB.Draw(_DBG_Trigger_Tex[6], tileDraw, Color.White);
                                //Trigger Wood Box
                                if (MapData[j, i] == '♀')
                                    sB.Draw(_ITEM_WoodBox_Tex, tileDraw, Color.White);
                                //Trigger Platform Hor
                                if (MapData[j, i] == '♪')
                                    sB.Draw(_OBJ_Platforms_Tex[0], tileDraw, Color.White);
                                //Trigger Platform Ver
                                if (MapData[j, i] == '♫')
                                    sB.Draw(_OBJ_Platforms_Tex[1], tileDraw, Color.White);
                                //Trigger Enemy Spawn Crw
                                if (MapData[j, i] == '☼')
                                    sB.Draw(_DBG_ETrigger_Tex[0], tileDraw, Color.White);
                                //Trigger Enemy Spawn Wlk
                                if (MapData[j, i] == '►')
                                    sB.Draw(_DBG_ETrigger_Tex[1], tileDraw, Color.White);
                                //Trigger Enemy Spawn Fly
                                if (MapData[j, i] == '◄')
                                    sB.Draw(_DBG_ETrigger_Tex[2], tileDraw, Color.White);

                            }

                            //Trigger Ladder Block
                            if (MapData[j, i] == '♠')
                                sB.Draw(_OBJ_Ladder_Tex, tileDraw, Color.White);



                            //----------------------------------------------------//Objects//----------------------------------------------------//

                            //Draw Torch Obj
                            //if (Bengine.ForeMapTextures[j, i] == 6)
                            //    Bengine.Torch.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                        }
                }
        }

        //Draw Background Map Textures
        public static void DrawBackgroundMapTextures(SpriteBatch sB, char[,] MapData, int tileSize, Vector2 Offset, Game1 game1)
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

                            //----------------------------------------------------//Textures Infront//----------------------------------------------------//
                            
                            //------------//Grass ForeBack//---------//
                            //Grass Solid Up
                            if (MapData[j, i] == '☺') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 22);
                            //Grass Solid Left
                            if (MapData[j, i] == '☻') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 8);
                            //Grass Solid Right
                            if (MapData[j, i] == '♥') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 2);
                            //Grass Solid Down
                            if (MapData[j, i] == '♦') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 36);
                            //Grass Solid Corner Up Left
                            if (MapData[j, i] == '♣') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 32);
                            //Grass Solid Corner Up Right
                            if (MapData[j, i] == '♠') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 26);
                            //Grass Solid Corner Down Left
                            if (MapData[j, i] == '•') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 19);
                            //Grass Solid Corner Down Right
                            if (MapData[j, i] == '◘') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 13);
                            //-----//Solid Hill//----//
                            //Grass Solid Hill Up Right
                            if (MapData[j, i] == '○') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 5);
                            //Grass Solid Hill Up Left
                            if (MapData[j, i] == '◙') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 3);
                            //Grass Solid Hill Down Right
                            if (MapData[j, i] == '♂') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 1);
                            //Grass Solid Hill Down Left
                            if (MapData[j, i] == '♀') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 7);
                            //------//Grass No Top ForeBack//------//
                            //Grass Solid Mid NoTop
                            if (MapData[j, i] == '♪') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 30);
                            //------------//Grass Background//---------//
                            //Grass Solid Mid NoTop Behind
                            if (MapData[j, i] == '♫') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.Gray, 6, 7, 30);
                            //Grass Mid No Top Bottomless Behind
                            if (MapData[j, i] == '☼') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.Gray, 6, 7, 12);

                            //------------//Dirt Foreback//---------//
                            //Dirt Solid Up
                            if (MapData[j, i] == '►') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 22);
                            //Dirt Solid Left
                            if (MapData[j, i] == '◄') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 8);
                            //Dirt Solid Right
                            if (MapData[j, i] == '↕') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 2);
                            //Dirt Solid Down
                            if (MapData[j, i] == '‼') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 36);
                            //Dirt Solid Corner Up Left
                            if (MapData[j, i] == '¶') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 32);
                            //Dirt Solid Corner Up Right
                            if (MapData[j, i] == '§') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 26);
                            //Dirt Solid Corner Down Left
                            if (MapData[j, i] == '▬') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 19);
                            //Dirt Solid Corner Down Right
                            if (MapData[j, i] == '↨') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 13);
                            //-----//Solid Hill//----//
                            //Dirt Solid Hill Up Right
                            if (MapData[j, i] == '↑') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 5);
                            //Dirt Solid Hill Up Left
                            if (MapData[j, i] == '↓') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 3);
                            //Dirt Solid Hill Down Right
                            if (MapData[j, i] == '→') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 1);
                            //Dirt Solid Hill Down Left
                            if (MapData[j, i] == '←') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 7);
                            //------//Dirt No Top Foreback//------//
                            //Dirt Solid Mid NoTop
                            if (MapData[j, i] == '∟') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 30);                            
                            //------------//Dirt Background//---------//
                            //Dirt Solid Mid NoTop Behind
                            if (MapData[j, i] == '↔') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.Gray, 6, 7, 30);
                            //Dirt Mid No Top Bottomless Behind
                            if (MapData[j, i] == '▲') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.Gray, 6, 7, 12);

                            //------------//Sand Foreback//---------//
                            //Sand Solid Up
                            if (MapData[j, i] == '▼') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 16);
                            //Sand Solid Left
                            if (MapData[j, i] == '!') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 2);
                            //Sand Solid Right
                            if (MapData[j, i] == '"') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 37);
                            //Sand Solid Down
                            if (MapData[j, i] == '#') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 30);
                            //Sand Solid Corner Up Left
                            if (MapData[j, i] == '$') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 26);
                            //Sand Solid Corner Up Right
                            if (MapData[j, i] == '%') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 5);
                            //Sand Solid Corner Down Left
                            if (MapData[j, i] == '&') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 13);
                            //Sand Solid Corner Down Right
                            if (MapData[j, i] == '■') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 7);
                            //-----//Solid Hill//----//
                            //Sand Solid Hill Up Right
                            if (MapData[j, i] == '(') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 8);
                            //Sand Solid Hill Up Left
                            if (MapData[j, i] == ')') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 14);
                            //Sand Solid Hill Down Right
                            if (MapData[j, i] == '*') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 36);
                            //Sand Solid Hill Down Left
                            if (MapData[j, i] == '+') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 1);
                            //------//Sand No Top Foreback//------//
                            //Sand Solid Mid NoTop
                            if (MapData[j, i] == '-') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 19);
                            //------------//Sand Background//---------//
                            //Sand Solid Mid NoTop Behind
                            if (MapData[j, i] == '.') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.Gray, 6, 7, 19);
                            //Sand Mid No Top Bottomless Behind
                            if (MapData[j, i] == '/') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.Gray, 6, 7, 6);

                            //------------//Snow Foreback//---------//
                            //Snow Solid Up
                            if (MapData[j, i] == '0') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 20);
                            //Snow Solid Left
                            if (MapData[j, i] == '1') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 32);
                            //Snow Solid Right
                            if (MapData[j, i] == '2') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 26);
                            //Snow Solid Down
                            if (MapData[j, i] == '3') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 19);
                            //Snow Solid Corner Up Left
                            if (MapData[j, i] == '4') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 15);
                            //Snow Solid Corner Up Right
                            if (MapData[j, i] == '5') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 9);
                            //Snow Solid Corner Down Left
                            if (MapData[j, i] == '6') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 2);
                            //Snow Solid Corner Down Right
                            if (MapData[j, i] == '7') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 37);
                            //-----//Solid Hill//----//
                            //Snow Solid Hill Up Right
                            if (MapData[j, i] == '8') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 38);
                            //Snow Solid Hill Up Left
                            if (MapData[j, i] == '9') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 3);
                            //Snow Solid Hill Down Right
                            if (MapData[j, i] == ':') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 25);
                            //Snow Solid Hill Down Left
                            if (MapData[j, i] == ';') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 31);
                            //------//Snow No Top Foreback//------//
                            //Snow Solid Mid NoTop
                            if (MapData[j, i] == '<') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 13);
                            //------------//Snow Background//---------//
                            //Snow Solid Mid NoTop Behind
                            if (MapData[j, i] == '=') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.Gray, 6, 7, 13);
                            //Snow Mid No Top Bottomless Behind
                            if (MapData[j, i] == '>') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.Gray, 6, 7, 36);

                            //------------//Castle Foreback//---------//
                            //Castle Solid Up
                            if (MapData[j, i] == '?') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 22);
                            //Castle Solid Left
                            if (MapData[j, i] == '@') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 8);
                            //Castle Solid Right
                            if (MapData[j, i] == 'A') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 2);
                            //Castle Solid Down
                            if (MapData[j, i] == 'B') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 36);
                            //Castle Solid Corner Up Left
                            if (MapData[j, i] == 'C') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 32);
                            //Castle Solid Corner Up Right
                            if (MapData[j, i] == 'D') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 26);
                            //Castle Solid Corner Down Left
                            if (MapData[j, i] == 'E') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 19);
                            //Castle Solid Corner Down Right
                            if (MapData[j, i] == 'F') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 13);
                            //-----//Solid Hill//----//
                            //Castle Solid Hill Up Right
                            if (MapData[j, i] == 'G') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 5);
                            //Castle Solid Hill Up Left
                            if (MapData[j, i] == 'H') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 3);
                            //Castle Solid Hill Down Right
                            if (MapData[j, i] == 'I') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 1);
                            //Castle Solid Hill Down Left
                            if (MapData[j, i] == 'J') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 7);
                            //------//Castle No Top Foreback//------//
                            //Castle Solid Mid NoTop
                            if (MapData[j, i] == 'K') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 30);
                            //------------//Castle Background//---------//
                            //Castle Solid Mid NoTop Behind
                            if (MapData[j, i] == 'L') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.Gray, 6, 7, 30);
                            //Castle Mid No Top Bottomless Behind
                            if (MapData[j, i] == 'M') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.Gray, 6, 7, 12);

                            //------------//Metal Foreback//---------//
                            //Metal Solid Up
                            if (MapData[j, i] == 'N') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 3);
                            //------//Metal No Top Foreback//------//
                            //Metal Solid Mid NoTop
                            if (MapData[j, i] == 'O') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 15);
                            //Metal Solid Mid NoTop Alt 1
                            if (MapData[j, i] == 'P') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 21);
                            //Metal Solid Mid NoTop Alt 2
                            if (MapData[j, i] == 'Q') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 26);
                            //------------//Metal Background//---------//
                            //Metal Solid Mid NoTop Behind
                            if (MapData[j, i] == 'R') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.Gray, 5, 7, 6);


                            //Foliage                            
                            
                        }
                }
        }

        //Draw Foreground Map Textures
        public static void DrawForegroundMapTextures(SpriteBatch sB, char[,] MapData, int tileSize, Vector2 Offset, Game1 game1)
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

                            //------------//Grass Foreground//---------//
                            //Grass Left Cliff Style 1
                            if (MapData[j, i] == '☺') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 15);
                            //Grass Right Cliff Style 1
                            if (MapData[j, i] == '☻') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 21);
                            //Grass Left Cliff Style 2
                            if (MapData[j, i] == '♥') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 27);
                            //Grass Right Cliff Style 2
                            if (MapData[j, i] == '♦') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 33);
                            //Grass Left Hill Top
                            if (MapData[j, i] == '♣') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 39);
                            //Grass Right Hill Top
                            if (MapData[j, i] == '♠') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 4);
                            //Grass Half Single
                            if (MapData[j, i] == '•') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 6);
                            //Grass Half Mid Left
                            if (MapData[j, i] == '◘') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 0);
                            //Grass Half Mid
                            if (MapData[j, i] == '○') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 20);
                            //Grass Half Mid Right
                            if (MapData[j, i] == '◙') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 40);
                            //Grass Edge Left
                            if (MapData[j, i] == '♂') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 17);
                            //Grass Edge Right
                            if (MapData[j, i] == '♀') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 11);
                            //Grass Single
                            if (MapData[j, i] == '♪') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 34);
                            //Grass Mid Left
                            if (MapData[j, i] == '♫') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 28);
                            //Grass Mid Right
                            if (MapData[j, i] == '☼') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 16);
                            //Grass Mid Top
                            if (MapData[j, i] == '►') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 10);
                            //------------//Grass No Top Foreground//---------//
                            //Grass Single NoTop
                            if (MapData[j, i] == '◄') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 9);
                            //Grass Mid NoTop Bottomless
                            if (MapData[j, i] == '↕') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 12);
                            //Grass Half Up NoTop Left
                            if (MapData[j, i] == '‼') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 24);
                            //Grass Half Up NoTop Right
                            if (MapData[j, i] == '¶') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 18);
                            //Grass Half Down NoTop Left
                            if (MapData[j, i] == '§') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 31);
                            //Grass Half Down NoTop Right
                            if (MapData[j, i] == '▬') SpriteSheetDraw(sB, _TILE_GrassSS_Tex, tileDraw, Color.White, 6, 7, 37);

                            //------------//Dirt Foreground//---------//
                            //Dirt Left Cliff Style 1
                            if (MapData[j, i] == '↨') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 15);
                            //Dirt Right Cliff Style 1
                            if (MapData[j, i] == '↑') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 21);
                            //Dirt Left Cliff Style 2
                            if (MapData[j, i] == '↓') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 27);
                            //Dirt Right Cliff Style 2
                            if (MapData[j, i] == '→') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 33);
                            //Dirt Left Hill Top
                            if (MapData[j, i] == '←') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 39);
                            //Dirt Right Hill Top
                            if (MapData[j, i] == '∟') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 4);
                            //Dirt Half Single
                            if (MapData[j, i] == '↔') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 6);
                            //Dirt Half Mid Left
                            if (MapData[j, i] == '▲') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 0);
                            //Dirt Half Mid
                            if (MapData[j, i] == '▼') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 20);
                            //Dirt Half Mid Right
                            if (MapData[j, i] == '!') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 40);
                            //Dirt Edge Left
                            if (MapData[j, i] == '"') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 17);
                            //Dirt Edge Right
                            if (MapData[j, i] == '#') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 11);
                            //Dirt Single
                            if (MapData[j, i] == '$') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 34);
                            //Dirt Mid Left
                            if (MapData[j, i] == '%') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 28);
                            //Dirt Mid Right
                            if (MapData[j, i] == '&') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 16);
                            //Dirt Mid Top
                            if (MapData[j, i] == '■') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 10);
                            //------------//Dirt No Top Foreground//---------//
                            //Dirt Single NoTop
                            if (MapData[j, i] == '(') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 9);
                            //Dirt Mid NoTop Bottomless
                            if (MapData[j, i] == ')') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 12);
                            //Dirt Half Up NoTop Left
                            if (MapData[j, i] == '*') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 24);
                            //Dirt Half Up NoTop Right
                            if (MapData[j, i] == '+') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 18);
                            //Dirt Half Down NoTop Left
                            if (MapData[j, i] == '-') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 31);
                            //Dirt Half Down NoTop Right
                            if (MapData[j, i] == '.') SpriteSheetDraw(sB, _TILE_DirtSS_Tex, tileDraw, Color.White, 6, 7, 37);
                            
                            //------------//Sand Foreground//---------//
                            //Sand Left Cliff Style 1
                            if (MapData[j, i] == '0') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 9);
                            //Sand Right Cliff Style 1
                            if (MapData[j, i] == '1') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 15);
                            //Sand Left Cliff Style 2
                            if (MapData[j, i] == '2') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 21);
                            //Sand Right Cliff Style 2
                            if (MapData[j, i] == '3') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 27);
                            //Sand Left Hill Top
                            if (MapData[j, i] == '4') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 33);
                            //Sand Right Hill Top
                            if (MapData[j, i] == '5') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 39);
                            //Sand Half Single
                            if (MapData[j, i] == '6') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 0);
                            //Sand Half Mid Left
                            if (MapData[j, i] == '7') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 20);
                            //Sand Half Mid
                            if (MapData[j, i] == '8') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 40);
                            //Sand Half Mid Right
                            if (MapData[j, i] == '9') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 34);
                            //Sand Edge Left
                            if (MapData[j, i] == ':') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 17);
                            //Sand Edge Right
                            if (MapData[j, i] == ';') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 11);
                            //Sand Single
                            if (MapData[j, i] == '<') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 28);
                            //Sand Mid Left
                            if (MapData[j, i] == '=') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 22);
                            //Sand Mid Right
                            if (MapData[j, i] == '>') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 10);
                            //Sand Mid Top
                            if (MapData[j, i] == '?') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 4);
                            //------------//Sand No Top Foreground//---------//
                            //Sand Single NoTop
                            if (MapData[j, i] == '@') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 3);
                            //Sand Mid NoTop Bottomless
                            if (MapData[j, i] == 'A') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 6);
                            //Sand Half Down NoTop Left
                            if (MapData[j, i] == 'B') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 18);
                            //Sand Half Down NoTop Right
                            if (MapData[j, i] == 'C') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 12);
                            //Sand Half Up NoTop Left
                            if (MapData[j, i] == 'D') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 25);
                            //Sand Half Up NoTop Right
                            if (MapData[j, i] == 'E') SpriteSheetDraw(sB, _TILE_SandSS_Tex, tileDraw, Color.White, 6, 7, 31);

                            //------------//Snow Foreground//---------//
                            //Snow Left Cliff Style 1
                            if (MapData[j, i] == 'F') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 39);
                            //Snow Right Cliff Style 1
                            if (MapData[j, i] == 'G') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 4);
                            //Snow Left Cliff Style 2
                            if (MapData[j, i] == 'H') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 10);
                            //Snow Right Cliff Style 2
                            if (MapData[j, i] == 'I') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 16);
                            //Snow Left Hill Top
                            if (MapData[j, i] == 'J') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 28);
                            //Snow Right Hill Top
                            if (MapData[j, i] == 'K') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 22);
                            //Snow Half Single
                            if (MapData[j, i] == 'L') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 30);
                            //Snow Half Mid Left
                            if (MapData[j, i] == 'M') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 12);
                            //Snow Half Mid
                            if (MapData[j, i] == 'N') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 18);
                            //Snow Half Mid Right
                            if (MapData[j, i] == 'O') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 24);
                            //Snow Edge Left
                            if (MapData[j, i] == 'P') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 17);
                            //Snow Edge Right
                            if (MapData[j, i] == 'Q') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 11);
                            //Snow Single
                            if (MapData[j, i] == 'R') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 6);
                            //Snow Mid Left
                            if (MapData[j, i] == 'S') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 40);
                            //Snow Mid Right
                            if (MapData[j, i] == 'T') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 0);
                            //Snow Mid Top
                            if (MapData[j, i] == 'U') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 34);
                            //------------//Snow No Top Foreground//---------//
                            //Snow Single NoTop
                            if (MapData[j, i] == 'V') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 33);
                            //Snow Mid NoTop Bottomless
                            if (MapData[j, i] == 'W') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 36);
                            //Snow Half Down NoTop Left
                            if (MapData[j, i] == 'X') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 7);
                            //Snow Half Down NoTop Right
                            if (MapData[j, i] == 'Y') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 1);
                            //Snow Half Up NoTop Left
                            if (MapData[j, i] == 'Z') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 5);
                            //Snow Half Up NoTop Right
                            if (MapData[j, i] == '[') SpriteSheetDraw(sB, _TILE_SnowSS_Tex, tileDraw, Color.White, 6, 7, 14);

                            //------------//Castle Foreground//---------//
                            //Castle Left Cliff Style 1
                            if (MapData[j, i] == ']') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 15);
                            //Castle Right Cliff Style 1
                            if (MapData[j, i] == '^') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 21);
                            //Castle Left Cliff Style 2
                            if (MapData[j, i] == '_') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 27);
                            //Castle Right Cliff Style 2
                            if (MapData[j, i] == '`') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 33);
                            //Castle Left Hill Top
                            if (MapData[j, i] == 'a') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 39);
                            //Castle Right Hill Top
                            if (MapData[j, i] == 'b') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 4);
                            //Castle Half Single
                            if (MapData[j, i] == 'c') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 6);
                            //Castle Half Mid Left
                            if (MapData[j, i] == 'd') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 0);
                            //Castle Half Mid
                            if (MapData[j, i] == 'e') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 20);
                            //Castle Half Mid Right
                            if (MapData[j, i] == 'f') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 40);
                            //Castle Edge Left
                            if (MapData[j, i] == 'g') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 17);
                            //Castle Edge Right
                            if (MapData[j, i] == 'h') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 11);
                            //Castle Single
                            if (MapData[j, i] == 'i') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 34);
                            //Castle Mid Left
                            if (MapData[j, i] == 'j') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 28);
                            //Castle Mid Right
                            if (MapData[j, i] == 'k') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 16);
                            //Castle Mid Top
                            if (MapData[j, i] == 'l') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 10);
                            //------------//Castle No Top Foreground//---------//
                            //Castle Single NoTop
                            if (MapData[j, i] == 'm') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 9);
                            //Castle Mid NoTop Bottomless
                            if (MapData[j, i] == 'n') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 12);
                            //Castle Half Down NoTop Left
                            if (MapData[j, i] == 'o') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 24);
                            //Castle Half Down NoTop Right
                            if (MapData[j, i] == 'p') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 18);
                            //Castle Half Up NoTop Left
                            if (MapData[j, i] == 'q') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 31);
                            //Castle Half Up NoTop Right
                            if (MapData[j, i] == 'r') SpriteSheetDraw(sB, _TILE_CastleSS_Tex, tileDraw, Color.White, 6, 7, 37);

                            //------------//Metal Foreground//---------//
                            //Metal Left Cliff Style 1
                            if (MapData[j, i] == 's') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 32);
                            //Metal Right Cliff Style 1
                            if (MapData[j, i] == 't') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 2);
                            //Metal Left Cliff Style 2
                            if (MapData[j, i] == 'u') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 7);
                            //Metal Right Cliff Style 2
                            if (MapData[j, i] == 'v') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 13);
                            //Metal Left Hill Top
                            if (MapData[j, i] == 'w') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 23);
                            //Metal Right Hill Top
                            if (MapData[j, i] == 'x') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 18);
                            //Metal Half Single
                            if (MapData[j, i] == 'y') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 20);
                            //Metal Half Mid Left
                            if (MapData[j, i] == 'z') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 12);
                            //Metal Half Mid
                            if (MapData[j, i] == '{') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 16);
                            //Metal Half Mid Right
                            if (MapData[j, i] == '|') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 10);
                            //Metal Single
                            if (MapData[j, i] == '}') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 27);
                            //Metal Mid Left
                            if (MapData[j, i] == '~') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 33);
                            //Metal Mid Right
                            if (MapData[j, i] == '⌂') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 8);
                            //Metal Mid Top
                            if (MapData[j, i] == 'Ç') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 28);
                            //------------//Metal No Top Foreground//---------//
                            //Metal Single NoTop
                            if (MapData[j, i] == 'ü') SpriteSheetDraw(sB, _TILE_MetalSS_Tex, tileDraw, Color.White, 5, 7, 14);

                            ////----------------------------------------------------//Objects//----------------------------------------------------//
                            //
                            ////Draw Grass Obj
                            //if (Bengine.ForeMapTextures[j, i] == 5)
                            //    sB.Draw(Textures._OBJ_Grass_Tex, RectTextureTile, Color.White);

                        }
                }
        }

        //Draw Map Effects
        public static void DrawMapEffects(SpriteBatch sB, char[,] MapData, int tileSize, Vector2 Offset, Game1 game1)
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
                            if (MapData[j, i] == '☺')
                                sB.Draw(_TILE_Shade_Effect[0], tileDraw, Color.White);
                            //Shade Tile Effect Down Half Left
                            if (MapData[j, i] == '☻')
                                sB.Draw(_TILE_Shade_Effect[1], tileDraw, Color.White);
                            //Shade Tile Effect Down Half Right
                            if (MapData[j, i] == '♥')
                                sB.Draw(_TILE_Shade_Effect[1], tileDraw, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
                            //Shade Tile Effect Up Half Left
                            if (MapData[j, i] == '♦')
                                sB.Draw(_TILE_Shade_Effect[2], tileDraw, Color.White);
                            //Shade Tile Effect Up Half Right
                            if (MapData[j, i] == '♣')
                                sB.Draw(_TILE_Shade_Effect[2], tileDraw, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
                            //Water Tile Top And Bottom for Editor
                            if (Global_GameState.GameState == Global_GameState.EGameState.EDITOR)
                            {
                                if (MapData[j, i] == '♠')
                                    sB.Draw(_DBG_WaterTop_Tex, tileDraw, Color.White);
                                if (MapData[j, i] == '•')
                                    sB.Draw(_DBG_WaterBot_Tex, tileDraw, Color.White);
                                if (MapData[j, i] == '◘')
                                    sB.Draw(_DBG_Lava_Tex[0], tileDraw, Color.White);
                                if (MapData[j, i] == '○')
                                    sB.Draw(_DBG_Lava_Tex[1], tileDraw, Color.White);
                            }
                        }
                }
        }
    }
}

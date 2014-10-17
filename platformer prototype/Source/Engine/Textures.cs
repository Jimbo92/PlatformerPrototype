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
        static public Texture2D _TILE_GrassSS_Tex;
        //Dirt
        static public Texture2D _TILE_DirtSS_Tex;

        //Effects
        static public Texture2D[] _TILE_Shade_Effect = new Texture2D[3];

        //Debug Textures
        static public Texture2D[] _DBG_Trigger_Tex = new Texture2D[4];
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

            //Map Textures
            //Grass
            _TILE_GrassSS_Tex = getContent.Load<Texture2D>("tiles/grass");
            //Dirt
            _TILE_DirtSS_Tex = getContent.Load<Texture2D>("tiles/dirt");

            //Effects
            for (int i = 0; i < 3; i++)
            {
                _TILE_Shade_Effect[i] = getContent.Load<Texture2D>("tiles/effects/shade" + i);
            }


            //Debug Textures & Editor
            for (int i = 0; i < 4; i++)
            {
                _DBG_Trigger_Tex[i] = getContent.Load<Texture2D>("editor/trigger" + i);
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
                            }
                        }
                }
        }
    }
}

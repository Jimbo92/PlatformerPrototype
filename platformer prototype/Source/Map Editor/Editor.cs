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
using System.IO;

namespace Platformer_Prototype
{
    class Editor
    {
        enum EMapLayers
        {
            TRIGGER,
            BACKGROUND,
            FOREGROUND,
            EFFECT,
        };
        EMapLayers MapLayers = EMapLayers.TRIGGER;
        private int Layers = 0;

        public Rectangle CursorPlacer = new Rectangle(0, 0, 32, 32);
        public int[,] GridDataL1;
        public int[,] GridDataL2;
        public int[,] GridDataL3;
        public int[,] GridDataL4;
        public Vector2 MapSize = new Vector2(50, 20);

        private Rectangle DrawTile;
        private Texture2D GridTexture;
        private Texture2D PlacerTexture;
        private Texture2D SelectorBackgroundTexture;
        private Rectangle SelectorRectangle;
        private bool SelectorUp = false;
        private Sprite Crosshair;
        private int PrevScrollValue = Mouse.GetState().ScrollWheelValue;
        private int TileSize = 32;
        private bool ShowGrid = true;
        private SpriteFont Font;
        private Vector2 ScreenSize;
        private int[] FontTimers = new int[5];
        private Game1 game1;

        private int[,] SelectorGrid = new int[2,5];
        private Rectangle SelectorGridTile;


        public Editor(ContentManager getContent, Vector2 getScreenSize)
        {
            ScreenSize = getScreenSize;

            //Textures
            SelectorBackgroundTexture = getContent.Load<Texture2D>("editor/TextureSelectorBackground");
            GridTexture = getContent.Load<Texture2D>("editor/grid");
            PlacerTexture = getContent.Load<Texture2D>("editor/placer");
            Crosshair = new Sprite(getContent, "objects/crosshairss", 98, 98, 1, 3);
            Font = getContent.Load<SpriteFont>("fonts/CopperplateGothicBold");

            for (int i = 0; i < FontTimers.Length; i++)
                FontTimers[i] = 101;

            for (int i = 0; i < (int)MapSize.X; i++)
                for (int j = 0; j < (int)MapSize.Y; j++)
                {
                    GridDataL1 = new int[j, i];
                    GridDataL2 = new int[j, i];
                    GridDataL3 = new int[j, i];
                    GridDataL4 = new int[j, i];
                }
        }

        private void TextureSelector()
        {

            SelectorRectangle.Width = (int)ScreenSize.X - 50;
            SelectorRectangle.Height = (int)ScreenSize.Y / 2;

            if (Input.KeyboardPressed(Keys.Space))
                if (!SelectorUp)
                    SelectorUp = true;
                else
                    SelectorUp = false;

            if (SelectorUp)
            {
                SelectorRectangle.X = 25;
                SelectorRectangle.Y = (int)ScreenSize.Y / 2;
            }
            else
            {
                SelectorRectangle.X = 25;
                SelectorRectangle.Y = (int)ScreenSize.Y - 25;
            }

        }

        public void Update(GraphicsDeviceManager getGraphics, Game1 getGame1)
        {
            game1 = getGame1;
            //Camera
            Camera.CameraMode = Camera.CameraState.MOUSE;
            Camera.Update(getGame1);

            //Texture Selector
            TextureSelector();

            //Texture Animations
            Crosshair.UpdateAnimation(0.3f);

            //Scroll in & out with mouse wheel
            if (Mouse.GetState().ScrollWheelValue < PrevScrollValue)
                TileSize -= 3;
            if (Mouse.GetState().ScrollWheelValue > PrevScrollValue)
                TileSize += 3;
            PrevScrollValue = Mouse.GetState().ScrollWheelValue;
            if (TileSize <= 15)
                TileSize = 15;

            //Switch Layers
            if (Input.KeyboardPressed(Microsoft.Xna.Framework.Input.Keys.Tab))
            {
                Layers++;
                if (Layers >= 4)
                    Layers = 0;
            }

            switch (Layers)
            {
                case 0: MapLayers = EMapLayers.TRIGGER; break;
                case 1: MapLayers = EMapLayers.BACKGROUND; break;
                case 2: MapLayers = EMapLayers.FOREGROUND; break;
                case 3: MapLayers = EMapLayers.EFFECT; break;
            }

            //Hide Grid
            if (Input.KeyboardPressed(Microsoft.Xna.Framework.Input.Keys.OemTilde))
            {
                FontTimers[0] = 0;
                if (!ShowGrid)
                    ShowGrid = true;
                else
                    ShowGrid = false;
            }

               
            //Save Map
            if (Input.KeyboardPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                FontTimers[1] = 0;
                SaveMap(GridDataL1, "File");
                SaveMap(GridDataL2, "File_Back");
                SaveMap(GridDataL3, "File_Fore");
                SaveMap(GridDataL4, "File_Eff");
            }
             
            //Load Map
            if (Input.KeyboardPressed(Microsoft.Xna.Framework.Input.Keys.L))
            {
                FontTimers[2] = 0;
                GridDataL1 = MapLoader.LoadMapData("File");
                GridDataL2 = MapLoader.LoadMapData("File_Back");
                GridDataL3 = MapLoader.LoadMapData("File_Fore");
                GridDataL4 = MapLoader.LoadMapData("File_Eff");
            }
        }

        private void SaveMap(int[,] getGrid, string MapFile)
        {
            StreamWriter sw = new StreamWriter("maps/" + MapFile + ".txt");
            for (int i = 0; i < getGrid.GetLength(0); i++)
            {
                int[] linelist = new int[getGrid.GetLength(1)];
                for (int j = 0; j < linelist.GetLength(0); j++)
                {
                    linelist[j] = getGrid[i, j];
                    int line = linelist[j];
                    sw.Write(line);
                    if (j != linelist.GetLength(0) - 1)
                        sw.Write(",");
                }
                sw.WriteLine();
            }
            sw.Close();
        }

        public void Draw(SpriteBatch sB)
        {
            if (!SelectorRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            switch (MapLayers)
            {
                case EMapLayers.TRIGGER: TriggerMapDrawCodes(sB); break;
                case EMapLayers.BACKGROUND: BackgroundMapDrawCodes(sB); break;
                case EMapLayers.FOREGROUND: ForegroundMapDrawCodes(sB); break;
                case EMapLayers.EFFECT: ; break;
            }

            Textures.DrawBackgroundMapTextures(sB, GridDataL2, TileSize, game1);

            Textures.DrawTriggerMapData(sB, GridDataL1, TileSize, game1);

            Textures.DrawForegroundMapTextures(sB, GridDataL3, TileSize, game1);

            Textures.DrawMapEffects(sB, GridDataL4, TileSize, game1);

            for (int i = 0; i < GridDataL1.GetLength(1); i++)
                for (int j = 0; j < GridDataL1.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.Position.X + TileSize * i, (int)Camera.Position.Y + TileSize * j, TileSize, TileSize);

                    if (ShowGrid)
                        sB.Draw(GridTexture, DrawTile, Color.White);

                    if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        sB.Draw(PlacerTexture, DrawTile, Color.White);
                    }
                }

            //Texture Selector
            sB.Draw(SelectorBackgroundTexture, SelectorRectangle, Color.White);
            //Selector Grid
                        for (int i = 0; i < SelectorGrid.GetLength(1); i++)
                            for (int j = 0; j < SelectorGrid.GetLength(0); j++)
                            {
                                SelectorGridTile = new Rectangle(SelectorRectangle.X + 32 * i + 32, SelectorRectangle.Y + 32 * j + 32, 32, 32);

                                if (SelectorGridTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                                {
                                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)

                                    SelectorGrid[j, i] = 1;
                                }

                                sB.Draw(GridTexture, SelectorGridTile, Color.White);

                                //Textures.DrawBackgroundMapTextures(sB, SelectorGrid, TileSize, SelectorGridTile, game1);
                            }

            //Editor Fonts
            //Layer Detail
            sB.DrawString(Font,"Map Editor v1 \n" + "LAYER: " + MapLayers.ToString(), new Vector2(20, 20), Color.Snow);
            //Show Grid Detail
            FontTimers[0]++;
            if (FontTimers[0] <= 100)
                sB.DrawString(Font, "Show Grid: " + ShowGrid.ToString(), new Vector2(ScreenSize.X - 160, 20), Color.Snow, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0);
            else
                FontTimers[0] = 101;
            //Map Saved Detail
            FontTimers[1]++;
            if (FontTimers[1] <= 100)
                sB.DrawString(Font, "Map Saved.", new Vector2(ScreenSize.X - 160, 20), Color.Snow, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0);
            else
                FontTimers[1] = 101;
            //Map Loaded Detail
            FontTimers[2]++;
            if (FontTimers[2] <= 100)
                sB.DrawString(Font, "Map Loaded.", new Vector2(ScreenSize.X - 160, 20), Color.Snow, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0);
            else
                FontTimers[2] = 101;

            //Draw Crosshair Last//
            Crosshair.Draw(sB, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 0, 0);
        }

        private void TriggerMapDrawCodes(SpriteBatch sB)
        {
            for (int i = 0; i < GridDataL1.GetLength(1); i++)
                for (int j = 0; j < GridDataL1.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.Position.X + TileSize * i, (int)Camera.Position.Y + TileSize * j, TileSize, TileSize);

                    if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            GridDataL1[j, i] = 1;
                        }

                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            GridDataL1[j, i] = 0;
                        }
                    }
                }
        }
        private void BackgroundMapDrawCodes(SpriteBatch sB)
        {
            for (int i = 0; i < GridDataL2.GetLength(1); i++)
                for (int j = 0; j < GridDataL2.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.Position.X + TileSize * i, (int)Camera.Position.Y + TileSize * j, TileSize, TileSize);

                    if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            GridDataL2[j, i] = 1;
                        }

                        if (Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            GridDataL2[j, i] = 0;
                        }
                    }

                    //if (GridDataL2[j, i] == 1)
                    //    sB.Draw(Textures._TILE_Grass_Tex[4], DrawTile, Color.Gray);
                }
        }
        private void ForegroundMapDrawCodes(SpriteBatch sB)
        {
            for (int i = 0; i < GridDataL3.GetLength(1); i++)
                for (int j = 0; j < GridDataL3.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.Position.X + TileSize * i, (int)Camera.Position.Y + TileSize * j, TileSize, TileSize);

                    if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            GridDataL3[j, i] = 2;
                        }

                        if (Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            GridDataL3[j, i] = 0;
                        }
                    }

                    //if (GridDataL3[j, i] == 2)
                    //    sB.Draw(Textures._TILE_Grass_Tex[2], DrawTile, Color.White);
                }
        }
    }
}

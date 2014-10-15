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
        public Vector2 MapSize = new Vector2(100, 20);

        private Rectangle DrawTile;
        private Texture2D GridTexture;
        private Texture2D PlacerTexture;
        private Texture2D SelectorBackgroundTexture;
        private Rectangle SelectorRectangle;
        private Rectangle SelectorButton;
        private Texture2D SelectorButtonTexture;
        private Texture2D SelectedTexture;
        private bool SelectorUp = false;
        private Sprite Crosshair;
        private int PrevScrollValue = Mouse.GetState().ScrollWheelValue;
        private int TileSize = 32;
        private bool ShowGrid = true;
        private SpriteFont Font;
        private Vector2 ScreenSize;
        private int[] FontTimers = new int[5];
        private Game1 game1;

        private int[,] SelectorGrid = MapLoader.LoadMapData("editor/selector1");
        private Rectangle SelectorGridTile;
        private int TileChooser;


        public Editor(ContentManager getContent, Vector2 getScreenSize)
        {
            ScreenSize = getScreenSize;
            Camera.CameraMode = Camera.CameraState.MOUSE;

            //Textures
            SelectorBackgroundTexture = getContent.Load<Texture2D>("editor/sidebar");
            SelectorButtonTexture = getContent.Load<Texture2D>("editor/sidebarbutton");
            SelectedTexture = getContent.Load<Texture2D>("editor/selected");
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
            SelectorRectangle.Width = 215;
            SelectorRectangle.Height = 500;
            SelectorRectangle.Y = 75;

            if (Input.KeyboardPressed(Keys.Space))
                if (!SelectorUp)
                    SelectorUp = true;
                else
                    SelectorUp = false;

            if (SelectorUp)
                SelectorRectangle.X = -5;
            else
                SelectorRectangle.X = -200;

            SelectorButton = new Rectangle(SelectorRectangle.X + SelectorRectangle.Width - 10, (int)ScreenSize.Y / 2 - 32, 20, 35);

            if (SelectorButton.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    if (!SelectorUp)
                        SelectorUp = true;
                    else
                        SelectorUp = false;


        }

        public void Update(GraphicsDeviceManager getGraphics, Game1 getGame1)
        {
            game1 = getGame1;
            //Camera
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
            if (Input.KeyboardPressed(Keys.Tab))
            {
                TileChooser = 0;
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

            //Change Camera
            if (Input.KeyboardPressed(Keys.C))
                if (Camera.CameraMode == Camera.CameraState.MOUSE)
                    Camera.CameraMode = Camera.CameraState.FREE;
                else
                    Camera.CameraMode = Camera.CameraState.MOUSE;

            //Hide Grid
            if (Input.KeyboardPressed(Keys.OemTilde))
            {
                FontTimers[0] = 0;
                if (!ShowGrid)
                    ShowGrid = true;
                else
                    ShowGrid = false;
            }

               
            //Save Map
            if (Input.KeyboardPressed(Keys.S))
            {
                FontTimers[1] = 0;
                SaveMap(GridDataL1, "File");
                SaveMap(GridDataL2, "File_Back");
                SaveMap(GridDataL3, "File_Fore");
                SaveMap(GridDataL4, "File_Eff");
            }
             
            //Load Map
            if (Input.KeyboardPressed(Keys.L))
            {
                FontTimers[2] = 0;
                GridDataL1 = MapLoader.LoadMapData("File");
                GridDataL2 = MapLoader.LoadMapData("File_Back");
                GridDataL3 = MapLoader.LoadMapData("File_Fore");
                GridDataL4 = MapLoader.LoadMapData("File_Eff");
            }

            //Keyboard Tile Chooser
            if (Input.KeyboardPressed(Keys.NumPad1) || Input.KeyboardPressed(Keys.D1))
                TileChooser = 1;
            else if (Input.KeyboardPressed(Keys.NumPad2) || Input.KeyboardPressed(Keys.D2))
                TileChooser = 2;
            else if (Input.KeyboardPressed(Keys.NumPad3) || Input.KeyboardPressed(Keys.D3))
                TileChooser = 3;
            else if (Input.KeyboardPressed(Keys.NumPad4) || Input.KeyboardPressed(Keys.D4))
                TileChooser = 4;
            else if (Input.KeyboardPressed(Keys.NumPad5) || Input.KeyboardPressed(Keys.D5))
                TileChooser = 5;
            else if (Input.KeyboardPressed(Keys.NumPad6) || Input.KeyboardPressed(Keys.D6))
                TileChooser = 6;
            else if (Input.KeyboardPressed(Keys.NumPad7) || Input.KeyboardPressed(Keys.D7))
                TileChooser = 7;
            else if (Input.KeyboardPressed(Keys.NumPad8) || Input.KeyboardPressed(Keys.D8))
                TileChooser = 8;
            else if (Input.KeyboardPressed(Keys.NumPad9) || Input.KeyboardPressed(Keys.D9))
                TileChooser = 9;

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
                case EMapLayers.EFFECT: EffectMapDrawCodes(sB); break;
            }

            Textures.TextureType = Textures.ETextureType.EDITOR;

            Textures.DrawBackgroundMapTextures(sB, GridDataL2, TileSize, Camera.Position, game1);

            Textures.DrawForegroundMapTextures(sB, GridDataL3, TileSize, Camera.Position, game1);

            Textures.DrawMapEffects(sB, GridDataL4, TileSize, Camera.Position, game1);

            Textures.DrawTriggerMapData(sB, GridDataL1, TileSize, Camera.Position, game1);

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
            sB.Draw(SelectorButtonTexture, SelectorButton, Color.White);
            //Selector Grid
            if (SelectorUp)
            {
                Textures.TextureType = Textures.ETextureType.SELECTOR;
                switch (MapLayers)
                {
                    case EMapLayers.TRIGGER: Textures.DrawTriggerMapData(sB, SelectorGrid, 32, new Vector2(SelectorRectangle.X + 10, SelectorRectangle.Y + 10), game1); break;
                    case EMapLayers.BACKGROUND: Textures.DrawBackgroundMapTextures(sB, SelectorGrid, 32, new Vector2(SelectorRectangle.X + 10, SelectorRectangle.Y + 10), game1); break;
                    case EMapLayers.FOREGROUND: Textures.DrawForegroundMapTextures(sB, SelectorGrid, 32, new Vector2(SelectorRectangle.X + 10, SelectorRectangle.Y + 10), game1); break;
                    case EMapLayers.EFFECT: Textures.DrawMapEffects(sB, SelectorGrid, 32, new Vector2(SelectorRectangle.X + 10, SelectorRectangle.Y + 10), game1); break;
                }

                for (int i = 0; i < SelectorGrid.GetLength(1); i++)
                    for (int j = 0; j < SelectorGrid.GetLength(0); j++)
                    {
                        SelectorGridTile = new Rectangle(SelectorRectangle.X + 40 * i + 10, SelectorRectangle.Y + 40 * j + 10, 32, 32);
                       
                        if (SelectorGrid[j, i] != 0)
                        sB.Draw(GridTexture, new Rectangle(SelectorGridTile.X - 2, SelectorGridTile.Y - 2, 36, 36), Color.Gray);
                       

                        if (SelectorGridTile.Contains(Mouse.GetState().X, Mouse.GetState().Y) && SelectorRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                TileChooser = SelectorGrid[j, i];
                            }
                            else
                                sB.Draw(PlacerTexture, SelectorGridTile, Color.Red);
                        }

                        if (SelectorGrid[j, i] == TileChooser && SelectorGrid[j, i] != 0)
                        {
                            sB.Draw(SelectedTexture, new Rectangle(SelectorGridTile.X - 3, SelectorGridTile.Y - 3, 38, 38), Color.White);
                        }
                    }
            }
            //Editor Fonts
            //Layer Detail
            sB.DrawString(Font,"Map Editor v2.0 \n" + "LAYER: " + MapLayers.ToString(), new Vector2(20, 20), Color.Snow);
            //Selected Tile Detail
            sB.DrawString(Font, "Selected Tile: " + TileChooser.ToString(), new Vector2(20, 60), Color.Red, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0);
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
                            GridDataL1[j, i] = TileChooser;
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
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            GridDataL2[j, i] = TileChooser;
                        }

                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            GridDataL2[j, i] = 0;
                        }
                    }
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
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            GridDataL3[j, i] = TileChooser;
                        }

                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            GridDataL3[j, i] = 0;
                        }
                    }
                }
        }
        private void EffectMapDrawCodes(SpriteBatch sB)
        {
            for (int i = 0; i < GridDataL4.GetLength(1); i++)
                for (int j = 0; j < GridDataL4.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.Position.X + TileSize * i, (int)Camera.Position.Y + TileSize * j, TileSize, TileSize);

                    if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            GridDataL4[j, i] = TileChooser;
                        }

                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            GridDataL4[j, i] = 0;
                        }
                    }
                }
        }
    }
}

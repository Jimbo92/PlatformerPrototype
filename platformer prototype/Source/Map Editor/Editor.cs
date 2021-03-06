﻿using System;
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
        public enum EMapLayers
        {
            TRIGGER,
            BACKGROUND,
            FOREGROUND,
            EFFECT,
        };
        public EMapLayers MapLayers = EMapLayers.TRIGGER;
        private int Layers = 0;

        public Rectangle CursorPlacer = new Rectangle(0, 0, 32, 32);
        public char[,] GridDataL1;
        public char[,] GridDataL2;
        public char[,] GridDataL3;
        public char[,] GridDataL4;
        public Vector2 MapSize;
        public int MapWidth = 100;
        public int MapHeight = 20;

        private Rectangle DrawTile;
        private Texture2D GridTexture;
        private Texture2D PlacerTexture;
        private bool SelectorUp = false;
        private Sprite Crosshair;
        private int PrevScrollValue = Mouse.GetState().ScrollWheelValue;
        private int TileSize = 32;
        private bool ShowGrid = true;
        private SpriteFont Font;
        private Vector2 ScreenSize;
        private int[] FontTimers = new int[5];
        private Game1 game1;

        private char[,] SelectorGrid;
        private Rectangle SelectorGridTile;
        private char TileChooser;
        private char TileHover;

        private Texture2D SelectorBackgroundTexture;
        private Rectangle SelectorRectangle;
        private Rectangle SelectorButton;
        private Texture2D SelectorButtonTexture;
        private Texture2D SelectedTexture;
        private int SelectorPageNumber;

        private Texture2D ToolBarTexture;
        private Rectangle BottomBarRectangle;

        private Rectangle BrushSize;
        private int BrushSizeValue = 32;

        private Sprite[] Button = new Sprite[9];

        System.Windows.Forms.FileDialog getFile = new System.Windows.Forms.OpenFileDialog();
        System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
        private string FileName;                   


        public Editor(ContentManager getContent, Vector2 getScreenSize)
        {
            MapSize = new Vector2(MapWidth, MapHeight);
            ScreenSize = getScreenSize;
            Camera.CameraMode = Camera.CameraState.FREE;
            //Textures
            SelectorBackgroundTexture = getContent.Load<Texture2D>("editor/sidebar");
            SelectorButtonTexture = getContent.Load<Texture2D>("editor/sidebarbutton");
            SelectedTexture = getContent.Load<Texture2D>("editor/selected");
            GridTexture = getContent.Load<Texture2D>("editor/grid");
            PlacerTexture = getContent.Load<Texture2D>("editor/placer");
            Crosshair = new Sprite(getContent, "objects/crosshairss", 98, 98, 1, 3);
            Font = getContent.Load<SpriteFont>("fonts/CopperplateGothicBold");
            ToolBarTexture = getContent.Load<Texture2D>("editor/toolbar");
            BottomBarRectangle = new Rectangle(300, (int)ScreenSize.Y - 64, 500, 64);
            for (int i = 0; i < 6; i++)
                Button[i] = new Sprite(getContent, "editor/button" + i, 32, 32, 1, 3);
            Button[6] = new Sprite(getContent, "editor/button2", 32, 32, 1, 3);
            Button[7] = new Sprite(getContent, "editor/button3", 32, 32, 1, 3);
            Button[8] = new Sprite(getContent, "editor/button6", 32, 32, 1, 3);

            for (int i = 0; i < FontTimers.Length; i++)
                FontTimers[i] = 101;

            for (int i = 0; i < (int)MapSize.X; i++)
                for (int j = 0; j < (int)MapSize.Y; j++)
                {
                    GridDataL1 = new char[j, i];
                    GridDataL2 = new char[j, i];
                    GridDataL3 = new char[j, i];
                    GridDataL4 = new char[j, i];
                }
        }

        private void TextureSelector()
        {
            switch (SelectorPageNumber)
            {
                case 0: SelectorGrid = MapLoader.LoadMapData("editor/selectorpage1"); break;
                case 1: SelectorGrid = MapLoader.LoadMapData("editor/selectorpage2"); break;
                case 2: SelectorGrid = MapLoader.LoadMapData("editor/selectorpage3"); break;
            }


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

            //Buttons
            //Page Select
            //Next
            if (Button[6].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                {
                    SelectorPageNumber++;
                }
            //Previous
            if (Button[7].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                {
                    SelectorPageNumber--;
                }
            if (SelectorPageNumber > 2)
                SelectorPageNumber = 0;
            else if (SelectorPageNumber < 0)
                SelectorPageNumber = 2;

        }

        private void ToolBar()
        {
            //BrushSize Plus & Minus
            //Minus
            if (Button[0].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    if (BrushSizeValue == 128)
                        BrushSizeValue = 64;
                    else if (BrushSizeValue == 64)
                        BrushSizeValue = 32;
            //Plus
            if (Button[1].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    if (BrushSizeValue == 32)
                        BrushSizeValue = 64;
                    else if (BrushSizeValue == 64)
                        BrushSizeValue = 128;

            //Layer Next & Previous
            //Next
            if (Button[2].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                {
                    TileChooser = ' ';
                    SelectorPageNumber = 0;
                    Layers++;
                }
            //Previous
            if (Button[3].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                {
                    TileChooser = ' ';
                    SelectorPageNumber = 0;
                    Layers--;
                }
            if (Layers > 3)
                Layers = 0;
            else if (Layers < 0)
                Layers = 3;
            //Load
            if (Button[4].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    Load();
            //Save
            if (Button[5].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    Save();
            //New Map
            if (Button[8].CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                if (Input.ClickReleased(Input.EClicks.LEFT))
                    New();

            //Button Animations
            foreach (Sprite button in Button)
                if (button.CollisionBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    button.CurrentFrame = 1;
                    if (Input.ClickPress(Input.EClicks.LEFT))
                        button.CurrentFrame = 2;
                }
                else
                    button.CurrentFrame = 0;

        }

        private void Load()
        {
            getFile.Title = "Map Loader";
            getFile.Filter = "Text files (*.txt*)| *.txt";

            if (getFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = getFile.FileName;
                GridDataL1 = LoadMap(FileName);
                GridDataL2 = LoadMap(FileName.Replace(".txt", "") + "_back.txt");
                GridDataL3 = LoadMap(FileName.Replace(".txt", "") + "_fore.txt");
                GridDataL4 = LoadMap(FileName.Replace(".txt", "") + "_eff.txt");

                FontTimers[2] = 0;
            }
        }
        private void Save()
        {
            saveFile.Title = "Map Exporter";
            saveFile.Filter = "Text files (*.txt*)| *.txt";

            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = saveFile.FileName;
                SaveMap(GridDataL1, FileName);
                SaveMap(GridDataL2, FileName.Replace(".txt", "") + "_back.txt");
                SaveMap(GridDataL3, FileName.Replace(".txt", "") + "_fore.txt");
                SaveMap(GridDataL4, FileName.Replace(".txt", "") + "_eff.txt");

                FontTimers[1] = 0;
            }
        }

        private void New()
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            System.Windows.Forms.Label WidthLabel = new System.Windows.Forms.Label();
            System.Windows.Forms.NumericUpDown WidthBox = new System.Windows.Forms.NumericUpDown();
            System.Windows.Forms.Label HeightLabel = new System.Windows.Forms.Label();
            System.Windows.Forms.NumericUpDown HeightBox = new System.Windows.Forms.NumericUpDown();
            System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button();
            System.Windows.Forms.Button buttonCancel = new System.Windows.Forms.Button();

            form.Text = "New Map";
            WidthLabel.Text = "Columns";
            WidthBox.Text = "100";
            WidthBox.Maximum = 255;
            HeightLabel.Text = "Rows";
            HeightBox.Text = "20";
            HeightBox.Maximum = 255;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            WidthLabel.SetBounds(15, 20, 64, 13);
            WidthBox.SetBounds(15, 36, 64, 20);
            HeightLabel.SetBounds(100, 20, 64, 13);
            HeightBox.SetBounds(100, 36, 64, 20);

            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);


            WidthLabel.AutoSize = true;
            HeightLabel.AutoSize = true;
            buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;

            form.ClientSize = new System.Drawing.Size(396, 107);
            form.Controls.AddRange(new System.Windows.Forms.Control[] { WidthLabel, WidthBox, HeightLabel, HeightBox, buttonOk, buttonCancel });
            form.ClientSize = new System.Drawing.Size(Math.Max(200, WidthLabel.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            System.Windows.Forms.DialogResult dialogResult = form.ShowDialog();

            if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Camera.Position = Vector2.Zero;

                MapWidth = (int)WidthBox.Value;
                MapHeight = (int)HeightBox.Value;

                MapSize = new Vector2(MapWidth, MapHeight);

                for (int i = 0; i < (int)MapSize.X; i++)
                    for (int j = 0; j < (int)MapSize.Y; j++)
                    {
                        GridDataL1 = new char[j, i];
                        GridDataL2 = new char[j, i];
                        GridDataL3 = new char[j, i];
                        GridDataL4 = new char[j, i];
                    }
            }
        }

        public void Update(GraphicsDeviceManager getGraphics, Game1 getGame1)
        {
            game1 = getGame1;
            //Camera
            Camera.Update(getGame1);

            //Open
            if (Input.KeyboardPressed(Keys.L))
                Load();
            //Save           
            if (Input.KeyboardPressed(Keys.S))
                Save();
            //New Map
            if (Input.KeyboardPressed(Keys.N))
                New();

            //Texture Selector
            TextureSelector();

            //ToolBar
            ToolBar();

            //Texture Animations
            Crosshair.UpdateAnimation(0.3f);

            //Scroll in & out with mouse wheel
            if (Mouse.GetState().ScrollWheelValue < PrevScrollValue)
                TileSize -= 7;
            if (Mouse.GetState().ScrollWheelValue > PrevScrollValue)
                TileSize += 7;
            PrevScrollValue = Mouse.GetState().ScrollWheelValue;
            if (TileSize >= 32)
                TileSize = 32;
            else if (TileSize <= 18)
                TileSize = 18;

            //Brush Size Keybind
            if (Input.KeyboardPressed(Keys.OemPlus))
                if (BrushSizeValue == 32)
                    BrushSizeValue = 64;
                else if (BrushSizeValue == 64)
                    BrushSizeValue = 128;
            if (Input.KeyboardPressed(Keys.OemMinus))
                if (BrushSizeValue == 128)
                    BrushSizeValue = 64;
                else if (BrushSizeValue == 64)
                    BrushSizeValue = 32;

            if (BrushSizeValue == 64)
            {
                if (TileSize == 32)
                    BrushSize = new Rectangle(Mouse.GetState().X - 48, Mouse.GetState().Y - 48, 95, 95);
                else if (TileSize == 25)
                    BrushSize = new Rectangle(Mouse.GetState().X - 36, Mouse.GetState().Y - 36, 74, 74);
                else if (TileSize == 18)
                    BrushSize = new Rectangle(Mouse.GetState().X - 25, Mouse.GetState().Y - 25, 53, 53);
            }
            else if (BrushSizeValue == 128)
            {
                if (TileSize == 32)
                    BrushSize = new Rectangle(Mouse.GetState().X - 65, Mouse.GetState().Y - 65, 127, 127);
                else if (TileSize == 25)
                    BrushSize = new Rectangle(Mouse.GetState().X - 50, Mouse.GetState().Y - 50, 99, 99);
                else if (TileSize == 18)
                    BrushSize = new Rectangle(Mouse.GetState().X - 36, Mouse.GetState().Y - 36, 71, 71);
            }

            //Switch Layers
            if (Input.KeyboardPressed(Keys.Tab) || Input.KeyboardPressed(Keys.RightShift))
            {
                SelectorPageNumber = 0;
                TileChooser = ' ';
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
            //if (Input.KeyboardPressed(Keys.S))
            //{
            //    FontTimers[1] = 0;
            //    SaveMap(GridDataL1, "File");
            //    SaveMap(GridDataL2, "File_Back");
            //    SaveMap(GridDataL3, "File_Fore");
            //    SaveMap(GridDataL4, "File_Eff");
            //}

            //Load Map
            //if (Input.KeyboardPressed(Keys.L))
            //{
            //    FontTimers[2] = 0;
            //    GridDataL1 = MapLoader.LoadMapData("File");
            //    GridDataL2 = MapLoader.LoadMapData("File_Back");
            //    GridDataL3 = MapLoader.LoadMapData("File_Fore");
            //    GridDataL4 = MapLoader.LoadMapData("File_Eff");
            //}

            //Keyboard Tile Chooser
            if (Input.KeyboardPressed(Keys.NumPad1) || Input.KeyboardPressed(Keys.D1))
                TileChooser = '1';
            else if (Input.KeyboardPressed(Keys.NumPad2) || Input.KeyboardPressed(Keys.D2))
                TileChooser = '2';
            else if (Input.KeyboardPressed(Keys.NumPad3) || Input.KeyboardPressed(Keys.D3))
                TileChooser = '3';
            else if (Input.KeyboardPressed(Keys.NumPad4) || Input.KeyboardPressed(Keys.D4))
                TileChooser = '4';
            else if (Input.KeyboardPressed(Keys.NumPad5) || Input.KeyboardPressed(Keys.D5))
                TileChooser = '5';
            else if (Input.KeyboardPressed(Keys.NumPad6) || Input.KeyboardPressed(Keys.D6))
                TileChooser = '6';
            else if (Input.KeyboardPressed(Keys.NumPad7) || Input.KeyboardPressed(Keys.D7))
                TileChooser = '7';
            else if (Input.KeyboardPressed(Keys.NumPad8) || Input.KeyboardPressed(Keys.D8))
                TileChooser = '8';
            else if (Input.KeyboardPressed(Keys.NumPad9) || Input.KeyboardPressed(Keys.D9))
                TileChooser = '9';

        }

        private void SaveMap(char[,] getGrid, string MapFile)
        {
            Camera.Position = Vector2.Zero;

            StreamWriter sw = new StreamWriter(MapFile);
            for (int i = 0; i < getGrid.GetLength(0); i++)
            {
                char[] linelist = new char[getGrid.GetLength(1)];
                for (int j = 0; j < linelist.GetLength(0); j++)
                {
                    linelist[j] = getGrid[i, j];
                    char line = linelist[j];
                    sw.Write(line);
                    if (j != linelist.GetLength(0) - 1)
                        sw.Write(",");
                }
                sw.WriteLine();
            }
            sw.Close();
        }

        public char[,] LoadMap(string MapDataFile)
        {
            Camera.Position = Vector2.Zero;

            char[,] loadMap;
            var data = File.ReadAllLines(@MapDataFile);
            loadMap = new char[data.Length, (data[0].Length + 1) / 2];
            for (int i = 0; i < data.Length; i++)
            {
                string line = data[i];
                string[] charr = line.Split(',');
                for (int j = 0; j < charr.Length; j++)
                    loadMap[i, j] = Convert.ToChar(charr[j]);
            }
            return loadMap;
        }

        public void Draw(SpriteBatch sB)
        {
            if (!SelectorRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && !BottomBarRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && !SelectorButton.Contains(Mouse.GetState().X, Mouse.GetState().Y))
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
                        sB.Draw(GridTexture, DrawTile, Color.SaddleBrown);

                    if (!SelectorRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y) && !BottomBarRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        if (BrushSizeValue == 32)
                        {
                            if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                                sB.Draw(PlacerTexture, DrawTile, Color.White);
                        }
                        else if (BrushSizeValue > 32)
                        {
                            if (BrushSize.Contains(DrawTile))
                                sB.Draw(PlacerTexture, DrawTile, Color.White);
                        }
                    
                    }
                }

            //Texture Selector
            sB.Draw(SelectorBackgroundTexture, SelectorRectangle, Color.White);
            sB.Draw(SelectorButtonTexture, SelectorButton, Color.White);
            //Texture Selector Buttons
            Button[6].Draw(sB, new Vector2(SelectorRectangle.X + SelectorRectangle.Width - 60, SelectorRectangle.Y + SelectorRectangle.Height - 100), 0, SpriteEffects.None);
            Button[7].Draw(sB, new Vector2(SelectorRectangle.X + SelectorRectangle.Width - 150, SelectorRectangle.Y + SelectorRectangle.Height - 100), 0, SpriteEffects.None);
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


                        if (SelectorGridTile.Contains(Mouse.GetState().X, Mouse.GetState().Y) && SelectorRectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            TileHover = SelectorGrid[j, i];

                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                TileChooser = SelectorGrid[j, i];
                            }
                            else
                                sB.Draw(PlacerTexture, SelectorGridTile, Color.Red);

                            if (SelectorGrid[j, i] == TileHover)
                                sB.DrawString(Font, TileIndex.Index(TileHover, this), new Vector2(Mouse.GetState().X + 10, Mouse.GetState().Y - 25), Color.Snow, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
                        }

                        if (SelectorGrid[j, i] == TileChooser && SelectorGrid[j, i] != ' ')
                        {
                            sB.Draw(SelectedTexture, new Rectangle(SelectorGridTile.X - 3, SelectorGridTile.Y - 3, 38, 38), Color.White);
                        }

                        //sB.Draw(GridTexture, new Rectangle(SelectorGridTile.X - 2, SelectorGridTile.Y - 2, 36, 36), Color.Gray);
                    }
            }
            //-----------------------//Bottom ToolBar//---------------------//
            sB.Draw(ToolBarTexture, BottomBarRectangle, Color.White);
            //ToolBar Buttons
            Button[0].Draw(sB, new Vector2(BottomBarRectangle.X + BottomBarRectangle.Width - 100, BottomBarRectangle.Y + BottomBarRectangle.Height - 42), 0, SpriteEffects.None);
            Button[1].Draw(sB, new Vector2(BottomBarRectangle.X + BottomBarRectangle.Width - 50, BottomBarRectangle.Y + BottomBarRectangle.Height - 42), 0, SpriteEffects.None);
            Button[2].Draw(sB, new Vector2(BottomBarRectangle.X + BottomBarRectangle.Width - 250, BottomBarRectangle.Y + BottomBarRectangle.Height - 42), 0, SpriteEffects.None);
            Button[3].Draw(sB, new Vector2(BottomBarRectangle.X + BottomBarRectangle.Width - 300, BottomBarRectangle.Y + BottomBarRectangle.Height - 42), 0, SpriteEffects.None);
            Button[4].Draw(sB, new Vector2(BottomBarRectangle.X + 25, BottomBarRectangle.Y + 25), 0, SpriteEffects.None);
            Button[5].Draw(sB, new Vector2(BottomBarRectangle.X + 65, BottomBarRectangle.Y + 25), 0, SpriteEffects.None);
            Button[8].Draw(sB, new Vector2(BottomBarRectangle.X + 105, BottomBarRectangle.Y + 25), 0, SpriteEffects.None);
            //Editor Fonts
            //Title Detail
            sB.DrawString(Font, "Map Editor", new Vector2(20, 20), Color.Snow);
            //Layer Detail
            sB.DrawString(Font,"LAYER: " + MapLayers.ToString(), new Vector2(BottomBarRectangle.X + BottomBarRectangle.Width - 330, BottomBarRectangle.Y + BottomBarRectangle.Height - 25), Color.Snow, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
            //Selected Tile Detail
            sB.DrawString(Font, SelectorPageNumber.ToString(), new Vector2(SelectorRectangle.X + SelectorRectangle.Width - 113, SelectorRectangle.Y + SelectorRectangle.Height - 110), Color.Snow);
            sB.DrawString(Font, "Selected Tile: \n" + TileIndex.Index(TileChooser, this), new Vector2(ScreenSize.X / 3 + 25, 20), Color.Cyan);
            //Brush Size Detail
            sB.DrawString(Font, "Brush Size: " + BrushSizeValue.ToString(), new Vector2(BottomBarRectangle.X + BottomBarRectangle.Width - 137, BottomBarRectangle.Y + BottomBarRectangle.Height - 25), Color.Cyan, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
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

                    if (BrushSizeValue == 32)
                    {
                        if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                GridDataL1[j, i] = TileChooser;
                            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                                GridDataL1[j, i] = ' ';
                    }
                    else if (BrushSizeValue > 32)
                    {
                        if (BrushSize.Contains(DrawTile))
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                GridDataL1[j, i] = TileChooser;
                            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                                GridDataL1[j, i] = ' ';
                    }
                }
        }
        private void BackgroundMapDrawCodes(SpriteBatch sB)
        {
            for (int i = 0; i < GridDataL2.GetLength(1); i++)
                for (int j = 0; j < GridDataL2.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.Position.X + TileSize * i, (int)Camera.Position.Y + TileSize * j, TileSize, TileSize);

                    if (BrushSizeValue == 32)
                    {
                        if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                GridDataL2[j, i] = TileChooser;
                            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                                GridDataL2[j, i] = ' ';
                    }
                    else if (BrushSizeValue > 32)
                    {
                        if (BrushSize.Contains(DrawTile))
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                GridDataL2[j, i] = TileChooser;
                            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                                GridDataL2[j, i] = ' ';
                    }
                }
        }
        private void ForegroundMapDrawCodes(SpriteBatch sB)
        {
            for (int i = 0; i < GridDataL3.GetLength(1); i++)
                for (int j = 0; j < GridDataL3.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.Position.X + TileSize * i, (int)Camera.Position.Y + TileSize * j, TileSize, TileSize);

                    if (BrushSizeValue == 32)
                    {
                        if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                GridDataL3[j, i] = TileChooser;
                            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                                GridDataL3[j, i] = ' ';
                    }
                    else if (BrushSizeValue > 32)
                    {
                        if (BrushSize.Contains(DrawTile))
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                GridDataL3[j, i] = TileChooser;
                            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                                GridDataL3[j, i] = ' ';
                    }

                }
        }
        private void EffectMapDrawCodes(SpriteBatch sB)
        {
            for (int i = 0; i < GridDataL4.GetLength(1); i++)
                for (int j = 0; j < GridDataL4.GetLength(0); j++)
                {
                    DrawTile = new Rectangle((int)Camera.Position.X + TileSize * i, (int)Camera.Position.Y + TileSize * j, TileSize, TileSize);

                    if (BrushSizeValue == 32)
                    {
                        if (DrawTile.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                GridDataL4[j, i] = TileChooser;
                            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                                GridDataL4[j, i] = ' ';
                    }
                    else if (BrushSizeValue > 32)
                    {
                        if (BrushSize.Contains(DrawTile))
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                GridDataL4[j, i] = TileChooser;
                            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                                GridDataL4[j, i] = ' ';
                    }
                }
        }
    }
}

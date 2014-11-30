#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
#endregion

// To-do List
//---------------------------------------
// Variables for Tile Size
// Importing function for text and images
// Entities 
// Collision system that supports entities
//---------------------------------------

namespace Platformer_Prototype
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        string CameraMode;
        public bool DebugMode = false;

        float frameRate;

        BaseEngine BEngine;
        Editor MEditor;
        MainMenu MMenu;

        public SpriteFont font;

        public static Vector2 ScreenSize;

        public Color BGColour = Color.CornflowerBlue;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            ScreenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //Sets Mouse cursor invisible
            IsMouseVisible = false;

            //Check if Old Temp Exists and delete
            if (Directory.Exists(Directory.GetCurrentDirectory() + "/maps/Temp"))
                Directory.Delete(Directory.GetCurrentDirectory() + "/maps/Temp", true);
            //Creates a new temp directory
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/maps/Temp"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/maps/Temp");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GUI.LoadContent(Content);
            Textures.LoadContent(Content);

            font = Content.Load<SpriteFont>("fonts/CopperplateGothicBold");

            MEditor = new Editor(Content, ScreenSize);

            BEngine = new BaseEngine(Content, ScreenSize);

            MMenu = new MainMenu(Content);

        }

        public void giveType(string type)
        {
            CameraMode = type;
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Begin();
            //Code Bellow This//
            if (Input.KeyboardPressed(Keys.Escape))
            {
                if (Global_GameState.GameState == Global_GameState.EGameState.MENU)
                {
                    if (Directory.Exists(Directory.GetCurrentDirectory() + "/maps/Temp"))
                        Directory.Delete(Directory.GetCurrentDirectory() + "/maps/Temp", true);

                    Exit();
                }
                else
                    Global_GameState.GameState = Global_GameState.EGameState.MENU;
            }

            if (Global_GameState.GameState == Global_GameState.EGameState.EDITOR)
                BGColour = Color.CornflowerBlue;

            if (Input.KeyboardPressed(Keys.F4))
            {
                if (!graphics.IsFullScreen)
                    graphics.IsFullScreen = true;
                else
                    graphics.IsFullScreen = false;

                graphics.ApplyChanges();
            }

            //if (Input.KeyboardPressed(Keys.F1))
            //{
            //    if (Global_GameState.GameState == Global_GameState.EGameState.EDITOR)
            //    {
            //        Global_GameState.GameState = Global_GameState.EGameState.PLAY;
            //        BEngine.LoadMapTimer = 0;
            //    }
            //    else
            //        Global_GameState.GameState = Global_GameState.EGameState.EDITOR;
            //}



            if (Global_GameState.GameState == Global_GameState.EGameState.MENU)
            {
                IsMouseVisible = true;
                MMenu.Update(this);
            }
            
            if (Global_GameState.GameState == Global_GameState.EGameState.PLAY)
            {
                IsMouseVisible = false;
                GUI.Update();
                BEngine.Update(this);
                if (Input.KeyboardPressed(Keys.F12))
                    if (!DebugMode)
                        DebugMode = true;
                    else
                        DebugMode = false;
            }
            
            if (Global_GameState.GameState == Global_GameState.EGameState.EDITOR)
            {
                MEditor.Update(graphics, this);
                IsMouseVisible = false;
            }


            //switch (Global_GameState.GameState)
            //{
            //    case Global_GameState.EGameState.PLAY:
            //        {
            //            GUI.Update();
            //            BEngine.Update(this);
            //            if (Input.KeyboardPressed(Keys.F12))
            //                if (!DebugMode)
            //                    DebugMode = true;
            //                else
            //                    DebugMode = false;
            //        }; break;
            //    case Global_GameState.EGameState.EDITOR:
            //        {
            //            MEditor.Update(graphics, this);
            //        }; break;
            //    case Global_GameState.EGameState.MENU:
            //        {
            //            IsMouseVisible = true;
            //            MMenu.Update();
            //        }; break;
            //}


            //Code Above This//
            Input.End();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BGColour);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            switch (Global_GameState.GameState)
            {
                case Global_GameState.EGameState.PLAY:
                    {
                        BEngine.Draw(spriteBatch);

                        if (DebugMode)
                        {
                            Color FPSColour;
                            //FrameRate draws red if below 30 FPS # nice :D # ikr?
                            if (frameRate < 30)
                                FPSColour = Color.Red;
                            else
                                FPSColour = Color.Green;

                            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
                            spriteBatch.DrawString(font, "Player Pos: " + BEngine.player.Position.X.ToString() + "," + BEngine.player.Position.Y.ToString(), new Vector2(10, 50), Color.DodgerBlue);
                            spriteBatch.DrawString(font, "Camera: " + CameraMode, new Vector2(10, 30), Color.DodgerBlue);
                            spriteBatch.DrawString(font, "Platformer Prototype", new Vector2(10, 10), Color.Snow);
                            spriteBatch.DrawString(font, "FPS: " + frameRate.ToString(), new Vector2(ScreenSize.X - 180, 10), FPSColour);
                            Vector2 realMouse = new Vector2((int)(Mouse.GetState().X - Camera.Position.X), (int)(Mouse.GetState().Y - Camera.Position.Y));
                            spriteBatch.DrawString(font, "Mouse Pos: " + realMouse.ToString(), new Vector2(10, 90), Color.DodgerBlue);
                        }

                        GUI.Draw(spriteBatch);
                    }; break;
                case Global_GameState.EGameState.EDITOR:
                    {
                        MEditor.Draw(spriteBatch);
                    }; break;
                case Global_GameState.EGameState.MENU:
                    {
                        MMenu.Draw(spriteBatch);
                    }; break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
        }
    }
}

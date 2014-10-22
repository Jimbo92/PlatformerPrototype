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

        SpriteFont font;

        public static Vector2 ScreenSize;

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
                Exit();

            if (Input.KeyboardPressed(Keys.F4))
            {
                if (!graphics.IsFullScreen)
                    graphics.IsFullScreen = true;
                else
                    graphics.IsFullScreen = false;

                graphics.ApplyChanges();
            }

            if (Input.KeyboardPressed(Keys.F1))
            {
                if (Global_GameState.GameState == Global_GameState.EGameState.EDITOR)
                {
                    Global_GameState.GameState = Global_GameState.EGameState.PLAY;
                    BEngine.LoadMapTimer = 0;
                }
                else
                    Global_GameState.GameState = Global_GameState.EGameState.EDITOR;
            }

            switch (Global_GameState.GameState)
            {
                case Global_GameState.EGameState.PLAY:
                    {
                        GUI.Update();
                        BEngine.Update(this);
                        if (Input.KeyboardPressed(Keys.F12))
                            if (!DebugMode)
                                DebugMode = true;
                            else
                                DebugMode = false;
                    }; break;
                case Global_GameState.EGameState.EDITOR:
                    {
                        MEditor.Update(graphics, this);
                    }; break;
            }


            //Code Above This//
            Input.End();
            base.Update(gameTime);
        }
       


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
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
                        }

                        GUI.Draw(spriteBatch);

                    }; break;
                case Global_GameState.EGameState.EDITOR:
                    {
                        MEditor.Draw(spriteBatch);
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

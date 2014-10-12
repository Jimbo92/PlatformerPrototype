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
        string cameraMode;
        public bool DebugMode = false;

        float frameRate;

        BaseEngine BEngine;

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


            BEngine = new BaseEngine(Content, ScreenSize);

        }

        public void giveType(string type)
        {
            cameraMode = type;
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Begin();
            GUI.Update();
            //Code Bellow This//

            if (Input.KeyboardPressed(Keys.Escape))
                Exit();

            if (Input.KeyboardPressed(Keys.F12))
                if (!DebugMode)
                    DebugMode = true;
                else
                    DebugMode = false;

            BEngine.Update(this);


            //Code Above This//
            Input.End();
            base.Update(gameTime);
        }
       


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

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
                spriteBatch.DrawString(font, "Camera: " + cameraMode, new Vector2(10, 30), Color.DodgerBlue);
                spriteBatch.DrawString(font, "Platformer Prototype", new Vector2(10, 10), Color.Snow);
                spriteBatch.DrawString(font, "FPS: " + frameRate.ToString(), new Vector2(ScreenSize.X - 180, 10), FPSColour);
            }

            GUI.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
        }
    }
}

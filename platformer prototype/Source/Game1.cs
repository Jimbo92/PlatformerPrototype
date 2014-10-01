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

        float frameRate;

        //Textures------------------------------------
        public Texture2D levelTex; //square for drawing level
        //Texture2D playerTex; //actual player image
        //--------------------------------------------
        BaseEngine BEngine;
        Player player;

        SpriteFont font;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            //Sets Mouse cursor invisible
            IsMouseVisible = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            levelTex = Content.Load<Texture2D>("level.jpg");

            font = Content.Load<SpriteFont>("CopperplateGothicBold");

            player = new Player(Content);
            BEngine = new BaseEngine(player);

        }

        public void giveType(string type)
        {
            cameraMode = type;
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Begin();
            //Code Bellow This//

            if (Input.KeyboardPressed(Keys.Escape))
                Exit();

            player.Update(this, BEngine);
            BEngine.Update(this);


            //Code Above This//
            Input.End();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            spriteBatch.Begin();

            string text1 = "Camera :" + cameraMode;
            string text2 = "Position :" + player.Position.X.ToString() + "," + player.Position.Y.ToString();
            string text3 = "FPS:" + frameRate;

            BEngine.Draw(spriteBatch);

            player.Draw(spriteBatch);


          
            spriteBatch.DrawString(font, text2, new Vector2(20, 60), Color.White);
            spriteBatch.DrawString(font, text3, new Vector2(20, 100), Color.White);
            spriteBatch.DrawString(font, text1, new Vector2(20, 140), Color.White);
            spriteBatch.DrawString(font, "Platformer Prototype", new Vector2(20, 20), Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
        }
    }
}

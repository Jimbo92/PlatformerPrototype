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

namespace Platformer_Prototype
{
    class MainMenu
    {
        public MenuButton[] Buttons = new MenuButton[3];

        private Texture2D _ButtonTex;

        public MainMenu(ContentManager Content)
        {
            _ButtonTex = Content.Load<Texture2D>("menu/button");

            Buttons[0] = new MenuButton(_ButtonTex, new Vector2(100, 100), "Play");
            Buttons[1] = new MenuButton(_ButtonTex, new Vector2(100, 150), "Editor");
            Buttons[2] = new MenuButton(_ButtonTex, new Vector2(100, 200), "Exit");

        }

        public void Update(Game1 game1)
        {
            foreach (MenuButton b in Buttons)
                b.Update();
            
                if (Buttons[0]._Rect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    if (Input.ClickReleased(Input.EClicks.LEFT))
                        Global_GameState.GameState = Global_GameState.EGameState.PLAY;
                if (Buttons[1]._Rect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    if (Input.ClickReleased(Input.EClicks.LEFT))
                        Global_GameState.GameState = Global_GameState.EGameState.EDITOR;
                if (Buttons[2]._Rect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    if (Input.ClickReleased(Input.EClicks.LEFT))
                        game1.Exit();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuButton b in Buttons)
                b.Draw(spriteBatch);

        }
    }
}
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
    class MainMenu
    {
        public MenuButton[] Buttons = new MenuButton[3];

        private Texture2D _ButtonTex;

        private int _SplashScreenCountDown = 150;
        private float _SplashScreenFade = 1;

        private float TitleScale = 2;
        private float TitleRotation = 0;
        private bool _ScaleUp;
        private bool _SpinRight;

        public MainMenu(ContentManager Content)
        {
            _ButtonTex = Content.Load<Texture2D>("menu/button");

            Buttons[0] = new MenuButton(_ButtonTex, new Vector2(400, 350), "Play");
            Buttons[1] = new MenuButton(_ButtonTex, new Vector2(400, 400), "Editor");
            Buttons[2] = new MenuButton(_ButtonTex, new Vector2(400, 450), "Exit");

        }

        public void Update(Game1 game1)
        {
            if (_SplashScreenCountDown > 0)
                _SplashScreenCountDown--;
            else if (_SplashScreenCountDown < 50)
                _SplashScreenFade -= 0.05f;

            //Title Animation
            if (TitleRotation >= 5)
                _SpinRight = false;
            else if (TitleRotation <= -5)
                _SpinRight = true;

            if (_SpinRight)
                TitleRotation += 0.02f;
            else
                TitleRotation -= 0.02f;

            if (TitleScale >= 2.3f)
                _ScaleUp = false;
            else if (TitleScale <= 2)
                _ScaleUp = true;

            if (_ScaleUp)
                TitleScale += 0.001f;
            else
                TitleScale -= 0.001f;

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
                    game1.DeleteTemp();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            spriteBatch.Draw(Textures._BG_Menu_Tex, Vector2.Zero, Color.White);
            spriteBatch.End();


            foreach (MenuButton b in Buttons)
                b.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            string Title = "   Purple's\nAdventures";
            spriteBatch.DrawString(Textures._BasicFont, Title, new Vector2(400, 150), Color.Purple, MathHelper.ToRadians(TitleRotation), Textures._BasicFont.MeasureString(Title) / 2, TitleScale, SpriteEffects.None, 0);


            if (_SplashScreenFade > 0)
                spriteBatch.Draw(Textures._SplashScreen_Tex, Vector2.Zero, Color.White * _SplashScreenFade);
            spriteBatch.End();

        }
    }
}

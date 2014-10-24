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
    static class GUI
    {
        public static SpriteFont Font;

        //-------------------Health----------------------//
        public static Sprite[] HPOn = new Sprite[5];
        public static Sprite[] HPOff = new Sprite[5];

        public static int PlayerHitPoints = 5;
        public static bool isHit = false;
        public static bool ShowHealthBar = true;

        private static float HealthBarYPos = 20;
        private static int HealthBarTimer;
        //------------------------------------------------//
        //-------------------Crystals----------------------//
        public static Texture2D Crystal_Tex;

        public static int NumOfCrystals = 0;
        public static bool ShowCrystalBar = true;
        public static bool CrystalPickUp = false;

        private static float CrystalBarYPos = 20;
        public static int CrystalBarTimer;
        //------------------------------------------------//


        public static Sprite Crosshair;

        public static void LoadContent(ContentManager getContent)
        {
            Font = getContent.Load<SpriteFont>("fonts/CopperplateGothicBold");

            //Health Stuff//
            for (int i = 0; i < 5; i++)
            {
                HPOn[i] = new Sprite(getContent, "objects/hud_heartFull", 53, 45);
                HPOff[i] = new Sprite(getContent, "objects/hud_heartEmpty", 53, 45);
            }
            //Crystals//
            Crystal_Tex = getContent.Load<Texture2D>("objects/items/gemblue");

            //Crosshair
            Crosshair = new Sprite(getContent, "objects/crosshairss", 98, 98, 1, 3);
        }

        public static void Update()
        {
            //Health Stuff//
            if (Input.KeyboardPressed(Keys.P))
            {
                ShowHealthBar = true;
                isHit = true;
            }
            if (PlayerHitPoints < 0)
                PlayerHitPoints = 5;
            if (ShowHealthBar)
                HealthBarAnimation();

            //Crystals//
            //if (Input.KeyboardPressed(Keys.C))
            //{
            //    ShowCrystalBar = true;
            //    CrystalPickUp = true;
            //    CrystalBarTimer = 0;
            //}
            if (ShowCrystalBar)
                CrystalBarAnimation();

            //Invent
            if (Input.KeyboardPressed(Keys.I))
            {
                ShowCrystalBar = true;
                ShowHealthBar = true;
            }

            //Crosshair
            Crosshair.UpdateAnimation(0.3f);
        }

        public static void CrystalBarAnimation()
        {
            CrystalBarTimer++;
            if (CrystalBarTimer < 100)
                CrystalBarYPos = 30;
            else if (CrystalBarTimer > 145 && CrystalBarTimer < 155)
                CrystalBarYPos += .5f;
            else if (CrystalBarTimer > 160 && CrystalBarTimer < 175)
                CrystalBarYPos -= 4;

            if (CrystalPickUp)
                NumOfCrystals++;
            if (CrystalBarTimer > 0)
                CrystalPickUp = false;

            if (CrystalBarTimer >= 175)
            {
                ShowCrystalBar = false;
                CrystalBarYPos = -30;
                CrystalBarTimer = 0;
            }
        }

        private static void HealthBarAnimation()
        {
            HealthBarTimer++;
            if (HealthBarTimer < 100)
                HealthBarYPos = 30;
            else if (HealthBarTimer > 145 && HealthBarTimer < 155)
                HealthBarYPos += .5f;
            else if (HealthBarTimer > 160 && HealthBarTimer < 175)
                HealthBarYPos -= 4;

            if (isHit)
                PlayerHitPoints--;
            if (HealthBarTimer > 0)
                isHit = false;

            if (HealthBarTimer >= 175)
            {
                ShowHealthBar = false;
                HealthBarYPos = -30;
                HealthBarTimer = 0;
            }
        }

        public static void Draw(SpriteBatch sB)
        {
            //Health Stuff//
            for (int i = 0; i < 5; i++)
                HPOff[i].Draw(sB, new Vector2((53 * i) + 30, HealthBarYPos), 0, SpriteEffects.None);
            for (int i = 0; i < PlayerHitPoints; i++)
                HPOn[i].Draw(sB, new Vector2((53 * i) + 30, HealthBarYPos), 0, SpriteEffects.None);

            //Crystals
            sB.Draw(Crystal_Tex, new Vector2(718, CrystalBarYPos - 23), Color.White);
            sB.DrawString(Font, NumOfCrystals.ToString(), new Vector2(700, CrystalBarYPos), Color.Snow);
            
            //Draw Crosshair Last//
            Crosshair.Draw(sB, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 0, 0);
        }
    }
}

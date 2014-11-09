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

        private static float HealthBarYPos = 30;
        private static int HealthBarTimer;
        //------------------------------------------------//
        //-------------------Oxygen----------------------//
        public static Sprite[] OxOn = new Sprite[5];
        public static Sprite[] OxOff = new Sprite[5];

        public static int PlayerOxPoints = 5;
        public static bool ShowOxygenBar = false;

        private static float OxygenBarYPos = -34;
        private static int OxygenBarTimer;
        //------------------------------------------------//
        //-------------------Crystals----------------------//
        public static int NumOfCrystals = 0;
        public static bool ShowCrystalBar = true;
        public static bool CrystalPickUp = false;

        private static float CrystalBarYPos = 30;
        public static int CrystalBarTimer;
        //------------------------------------------------//
        //-------------------Coins----------------------//
        public static int NumOfCoins = 0;
        public static bool ShowCoinBar = true;
        public static bool CoinPickUp = false;

        private static float CoinBarYPos = 30;
        public static int CoinBarTimer;
        //------------------------------------------------//


        public static Sprite Crosshair;

        public static void LoadContent(ContentManager getContent)
        {
            Font = getContent.Load<SpriteFont>("fonts/CopperplateGothicBold");

            //Health Stuff//
            for (int i = 0; i < 5; i++)
            {
                HPOn[i] = new Sprite(getContent, "objects/hud/hud_heartFull", 53, 45);
                HPOff[i] = new Sprite(getContent, "objects/hud/hud_heartEmpty", 53, 45);
                OxOn[i] = new Sprite(getContent, "objects/hud/bubbleon", 53, 45);
                OxOff[i] = new Sprite(getContent, "objects/hud/bubbleoff", 53, 45);
            }

            //Crosshair
            Crosshair = new Sprite(getContent, "objects/crosshairss", 98, 98, 1, 3);
        }

        public static void Update()
        {
            //Health Stuff//
            if (PlayerHitPoints < 0)
                PlayerHitPoints = 5;
            if (ShowHealthBar)
                HealthBarAnimation();

            //Oxygen Stuff//
            if (PlayerOxPoints < 0)
                PlayerOxPoints = 5;
            if (ShowOxygenBar)
            {
                OxygenBarYPos = 75;
                OxygenBarTimer = 0;
            }
            else
                OxygenBarAnimation();

            //Crystals//
            if (ShowCrystalBar)
                CrystalBarAnimation();

            //Coins//
            if (ShowCoinBar)
                CoinBarAnimation();

            //Invent
            if (Input.KeyboardPressed(Keys.I))
            {
                ShowCrystalBar = true;
                ShowHealthBar = true;
                ShowCoinBar = true;
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

        public static void CoinBarAnimation()
        {
            CoinBarTimer++;
            if (CoinBarTimer < 100)
                CoinBarYPos = 30;
            else if (CoinBarTimer > 145 && CoinBarTimer < 155)
                CoinBarYPos += .5f;
            else if (CoinBarTimer > 160 && CoinBarTimer < 175)
                CoinBarYPos -= 4;

            if (CoinPickUp)
                NumOfCoins++;
            if (CoinBarTimer > 0)
                CoinPickUp = false;

            if (CoinBarTimer >= 175)
            {
                ShowCoinBar = false;
                CoinBarYPos = -30;
                CoinBarTimer = 0;
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

        private static void OxygenBarAnimation()
        {
            OxygenBarTimer++;
            if (OxygenBarTimer > 145 && OxygenBarTimer < 155)
                OxygenBarYPos += .5f;
            else if (OxygenBarTimer > 160 && OxygenBarTimer < 221)
                OxygenBarYPos -= 4;

            if (OxygenBarTimer >= 221)
            {
                OxygenBarYPos = -34;
            }
        }

        public static void Draw(SpriteBatch sB)
        {
            //Health Stuff//
            for (int i = 0; i < 5; i++)
                HPOff[i].Draw(sB, new Vector2((53 * i) + 30, HealthBarYPos), 0, SpriteEffects.None);
            for (int i = 0; i < PlayerHitPoints; i++)
                HPOn[i].Draw(sB, new Vector2((53 * i) + 30, HealthBarYPos), 0, SpriteEffects.None);

            //Oxygen Stuff//
            for (int i = 0; i < 5; i++)
                OxOff[i].Draw(sB, new Vector2((53 * i) + 30, OxygenBarYPos), 0, SpriteEffects.None);
            for (int i = 0; i < PlayerOxPoints; i++)
                OxOn[i].Draw(sB, new Vector2((53 * i) + 30, OxygenBarYPos), 0, SpriteEffects.None);

            //Crystals
            sB.Draw(Textures._ITEM_Crystal_Tex, new Vector2(718, CrystalBarYPos - 23), Color.White);
            sB.DrawString(Font, NumOfCrystals.ToString(), new Vector2(698, CrystalBarYPos - 11), Color.Snow);

            //Coins
            sB.Draw(Textures._ITEM_Coin_Tex, new Rectangle(618, (int)CoinBarYPos - 23, 32, 32), Color.White);
            sB.DrawString(Font, NumOfCoins.ToString(), new Vector2(598, CoinBarYPos - 11), Color.Snow);
            
            //Draw Crosshair Last//
            Crosshair.Draw(sB, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 0, 0);
        }
    }
}

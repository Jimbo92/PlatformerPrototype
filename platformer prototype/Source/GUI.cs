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
        //-------------------Health----------------------//
        public static Sprite[] HPOn = new Sprite[5];
        public static Sprite[] HPOff = new Sprite[5];

        public static int PlayerHitPoints = 5;
        public static bool isHit = true;

        private static float HealthBarYPos = 20;
        private static int HealthBarTimer;
        //------------------------------------------------//

        public static Sprite Crosshair;

        public static void LoadContent(ContentManager getContent)
        {
            //Health Stuff//
            for (int i = 0; i < 5; i++)
            {
                HPOn[i] = new Sprite(getContent, "objects/healthpoint", 32, 32);
                HPOff[i] = new Sprite(getContent, "objects/healthpoint", 32, 32);
            }

            //Crosshair
            Crosshair = new Sprite(getContent, "objects/crosshairss", 98, 98, 1, 3);
        }

        public static void Update()
        {
            //Health Stuff//
            if (Input.KeyboardPressed(Keys.P))
            {
                PlayerHitPoints--;
                isHit = true;
            }
            if (PlayerHitPoints < 0)
                PlayerHitPoints = 5;
            if (isHit)
                HealthBarAnimation();

            //Crosshair
            Crosshair.UpdateAnimation(0.3f);
        }

        private static void HealthBarAnimation()
        {
            HealthBarTimer++;
            if (HealthBarTimer < 100)
                HealthBarYPos = 20;
            else if (HealthBarTimer > 145 && HealthBarTimer < 155)
                HealthBarYPos += .5f;
            else if (HealthBarTimer > 160 && HealthBarTimer < 175)
                HealthBarYPos -= 4;

            if (HealthBarTimer >= 175)
            {
                isHit = false;
                HealthBarYPos = -30;
                HealthBarTimer = 0;
            }
        }

        public static void Draw(SpriteBatch sB)
        {
            //Health Stuff//
            for (int i = 0; i < 5; i++)
                HPOff[i].Draw(sB, new Vector2((35 * i) + 20, HealthBarYPos), new Vector2(HPOff[i].Width / 2, HPOff[i].Height / 2), 0, SpriteEffects.None, Color.Gray);
            for (int i = 0; i < PlayerHitPoints; i++)
                HPOn[i].Draw(sB, new Vector2((35 * i) + 20, HealthBarYPos), 0, SpriteEffects.None);

            //Draw Crosshair Last//
            Crosshair.Draw(sB, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 0, 0);
        }
    }
}

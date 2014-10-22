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
    class Background
    {
        public Sprite[] background = new Sprite[5];
        public Sprite[] backgroundNear = new Sprite[5];
        public Sprite[] Clouds = new Sprite[5];
        public Sprite Sun;

        private Vector2 ScreenSize;
        private float CloudMoveX;

        public Background(ContentManager getContent, Vector2 getScreenSize)
        {
            Sun = new Sprite(getContent, "backgrounds/sun", 96, 96);

            ScreenSize = getScreenSize;
            for (int i = 0; i < 5; i++)
            {
                backgroundNear[i] = new Sprite(getContent, "backgrounds/backgroundhills", 828, 358);
                background[i] = new Sprite(getContent, "backgrounds/backgroundday2", 1300, (int)ScreenSize.Y * 2);
                Clouds[i] = new Sprite(getContent, "backgrounds/cloud1", 128, 71);
            }
        }


        public void Draw(SpriteBatch sB)
        {
            for (int i = 0; i < 5; i++)
                background[i].Draw(sB, new Vector2((background[i].Texture.Width * i) + Camera.Position.X / 4, (Camera.Position.Y / 4) + ScreenSize.Y * 2 - (background[0].Texture.Height / 4)), 0, SpriteEffects.None);

            Sun.Draw(sB, new Vector2(100, 100 + Camera.Position.Y / 8), 0, SpriteEffects.None);

            CloudMoveX -= 0.3f;
            for (int i = 0; i < 5; i++)
            {
                if (CloudMoveX < -Clouds[i].Texture.Width * 4)
                    CloudMoveX = 0;

                backgroundNear[i].Draw(sB, new Vector2((backgroundNear[i].Texture.Width * i) + Camera.Position.X / 3, (Camera.Position.Y / 3) + ScreenSize.Y - (backgroundNear[0].Texture.Height / 2)), 0, SpriteEffects.None);
                Clouds[i].Draw(sB, new Vector2((Clouds[i].Texture.Width * 4 * i) + Camera.Position.X / 2 + CloudMoveX, Camera.Position.Y / 2), 0, SpriteEffects.None);
            }
        }
    }
}

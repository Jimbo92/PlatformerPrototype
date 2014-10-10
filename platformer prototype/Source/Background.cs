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

        private Vector2 ScreenSize;
        private Camera camera;
        private float CloudMoveX;

        public Background(ContentManager getContent, Vector2 getScreenSize)
        {
            ScreenSize = getScreenSize;
            for (int i = 0; i < 5; i++)
            {
                backgroundNear[i] = new Sprite(getContent, "backgroundhills", 828, 358);
                background[i] = new Sprite(getContent, "backgroundday2", 1300, 600);
                Clouds[i] = new Sprite(getContent, "cloud1", 256, 128);
            }
        }

        public void Update(Camera getCamera)
        {
            camera = getCamera;
        }

        public void Draw(SpriteBatch sB)
        {
            for (int i = 0; i < 5; i++)
            {
                CloudMoveX -= 0.1f;
                if (CloudMoveX < -Clouds[i].Texture.Width)
                    CloudMoveX = 0;

                background[1].Draw(sB, new Vector2((background[i].Texture.Width * i) + camera.Position.X / 4, camera.Position.Y / 4), MathHelper.ToRadians(180), SpriteEffects.FlipVertically);
                backgroundNear[i].Draw(sB, new Vector2( (backgroundNear[i].Texture.Width * i) + camera.Position.X / 3, (camera.Position.Y / 3) + ScreenSize.Y - (backgroundNear[0].Texture.Height / 2)), MathHelper.ToRadians(180), SpriteEffects.FlipVertically);
                Clouds[i].Draw(sB, new Vector2((Clouds[i].Texture.Width * i) + camera.Position.X / 2 + CloudMoveX, camera.Position.Y / 2), 0, SpriteEffects.None);
            }

        }
    }
}

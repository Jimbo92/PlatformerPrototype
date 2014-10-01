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

        private Vector2 ScreenSize;
        private Camera camera;

        public Background(ContentManager getContent, Vector2 getScreenSize)
        {
            ScreenSize = getScreenSize;
            for (int i = 0; i < 5; i++)
            {
                background[i] = new Sprite(getContent, "backgroundsky2", i * (int)ScreenSize.X * 2, (int)ScreenSize.Y * 2);
                backgroundNear[i] = new Sprite(getContent, "backgroundhills", i * 256, 256);
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
                background[i].Draw(sB, new Vector2(camera.Position.X / 2, camera.Position.Y / 2), MathHelper.ToRadians(180), SpriteEffects.FlipVertically);
                backgroundNear[i].Draw(sB, new Vector2(camera.Position.X / 3, (camera.Position.Y / 3) + ScreenSize.Y - (backgroundNear[0].Texture.Height / 2)), MathHelper.ToRadians(180), SpriteEffects.FlipVertically);
            }
        }
    }
}

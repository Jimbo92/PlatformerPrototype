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
        public Sprite background;
        public Sprite background2;

        private Vector2 ScreenSize;
        private Camera camera;

        public Background(ContentManager getContent, Vector2 getScreenSize)
        {
            ScreenSize = getScreenSize;

            background = new Sprite(getContent, "backgroundsky2", (int)ScreenSize.X, (int)ScreenSize.Y);
        }

        public void Update(Camera getCamera)
        {
            camera = getCamera;
        }

        public void Draw(SpriteBatch sB)
        {
            background.Draw(sB, camera.Position);
        }
    }
}

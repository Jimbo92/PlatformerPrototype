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
        public Texture2D Background_Tex;
        private Vector2 BG_Pos;
        public Sprite[] Clouds = new Sprite[5];
        public Sprite Sun;

        private Vector2 ScreenSize;
        private float CloudMoveX;

        int offset;

        public Background(ContentManager getContent, Vector2 getScreenSize)
        {
            Sun = new Sprite(getContent, "backgrounds/sun", 96, 96);

            Background_Tex = Textures._BG_GrassLands_Tex;

            ScreenSize = getScreenSize;
            for (int i = 0; i < 5; i++)
                Clouds[i] = new Sprite(getContent, "backgrounds/cloud1", 128, 71);
        }


        public void Draw(SpriteBatch sB)
        {
            //Background Draw//
            BG_Pos = new Vector2((int)Camera.Position.X / 4 + offset, (int)Camera.Position.Y / 4 + 100);

            if (offset <= -1023)
                offset = 0;

            sB.Draw(Background_Tex, new Rectangle((int)BG_Pos.X, (int)BG_Pos.Y, 1023, 512), Color.White * 0.2f);
            
            sB.Draw(Background_Tex, new Rectangle((int)BG_Pos.X + 1023, (int)BG_Pos.Y, 1023, 512), Color.White * 0.2f);
            //-------------//

            Sun.Draw(sB, new Vector2(100, 100 + Camera.Position.Y / 8), 0, SpriteEffects.None);

            CloudMoveX -= 0.3f;
            for (int i = 0; i < 5; i++)
            {
                if (CloudMoveX < -Clouds[i].Texture.Width * 4)
                    CloudMoveX = 0;

                Clouds[i].Draw(sB, new Vector2((Clouds[i].Texture.Width * 4 * i) + Camera.Position.X / 3 + CloudMoveX, Camera.Position.Y / 2), 0, SpriteEffects.None);
            }
        }
    }
}

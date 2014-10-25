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
    class Platform
    {
        public Sprite sprite;
        public Vector2 Position;
        public Rectangle Bounds;
        public Vector2 Speed;
        public int Width = 32;
        public int Height = 32;


        private BaseEngine BEngine;
        //private Game1 game1;

        public Vector2 runPlanes;

        //Movement variables
       
        bool up = false;
        bool left = false;
        bool right = false;
        bool down = false;


 
       
        //--------------------------------------------

        public Platform(ContentManager getContent)
        {
            Position = Vector2.Zero;
            left = true;
            runPlanes = new Vector2(-50, 50);
            sprite = new Sprite(getContent, "objects/Moving", Width, Height);
                       
        }

        public void Set(Vector2 desiredPlanes, bool ddirection)
        {
            runPlanes = desiredPlanes;
            left = ddirection;
            right = !ddirection;
        }

        public void updateBounds(Vector2 Camera)
        {
            Bounds = new Rectangle((int)Position.X + (int)Camera.X, (int)Position.Y + (int)Camera.Y, Width, Height);
       
        }

        private void Collisions()
        {
            
            Position.X += Speed.X;
            Position.Y += Speed.Y;
            updateBounds(Camera.Position);

        }

        public void Update(BaseEngine getBEngine)
        {

            BEngine = getBEngine;

            Collisions();

                if (Position.X < runPlanes.X)
                {
                    right = true;
                    left = false;
                }

                if (Position.X > runPlanes.Y)
                {
                    right = false;
                    left = true;
                }


                if (left)
                    Speed.X = -1;
                if (right)
                    Speed.X = 1;
            
       
        }

      
        public void Draw(SpriteBatch sB)
        {

            //sB.Draw(Textures._OBJ_Ladder_Tex, Bounds, Color.Red);
            sprite.Draw(sB, new Vector2(Bounds.X, Bounds.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);

        }

      

       

    }
}

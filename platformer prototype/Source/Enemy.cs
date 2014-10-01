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
    class Enemy
    {
        public Vector2 Position;
        public Rectangle Bounds;
        public Vector2 Speed;
        public bool isDead = true;
        public int Width = 24;
        public int Height = 24;

        private BaseEngine BEngine;
        private Game1 game1;
       
        //--------------------------------------------

        public Enemy(ContentManager getContent)
        {
            Position = Vector2.Zero;

        }

        public void updateBounds(Vector2 camera)
        {
            Bounds = new Rectangle((int)Position.X + (int)camera.X, (int)Position.Y + (int)camera.Y, Width, Height);
        }

        public void Update(Game1 getGame1, BaseEngine getEngine)
        {
         
            BEngine = getEngine;
            game1 = getGame1;
           

            //Gravity--------------
            if (Speed.Y < 12)
                Speed.Y += 0.2f;
            else
                Speed.Y = 12;
            //---------------------

            
            //Controls--------------------------------
         

        }

        public void checkCollisionsX(Rectangle target)
        {
            if (Bounds.Intersects(target))
            {

                if (Speed.X > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(BEngine.camera.Position);
                        BEngine.updateHitboxes(Position, Bounds); 
                        if (Bounds.Intersects(target))
                            Position.X--;
                    }

                if (Speed.X < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(BEngine.camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (Bounds.Intersects(target))
                            Position.X++;
                    }

                Speed.X = 0;
            }
        }

        public void checkCollisionsY(Rectangle target)
        {
            if (Bounds.Intersects(target))
            {

                if (Speed.Y > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(BEngine.camera.Position);
                        BEngine.updateHitboxes(Position, Bounds); 
                        if (Bounds.Intersects(target))
                            Position.Y--;
                    }

                if (Speed.Y < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(BEngine.camera.Position);
                        BEngine.updateHitboxes(Position, Bounds); 
                        if (Bounds.Intersects(target))
                            Position.Y++;
                    }

                Speed.Y = 0;
            }
        }

        public void Draw(SpriteBatch sB)
        {
           
            sB.Draw(game1.levelTex, Bounds, BEngine.Scale, Color.Red);

        }

    }
}

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
        public Sprite sprite;
        public Vector2 Position;
        public Rectangle Bounds;
        public Vector2 Speed;
        public bool isDead = true;
        public int Width = 24;
        public int Height = 24;

        private BaseEngine BEngine;
        private Game1 game1;

        public Vector2 tl = Vector2.Zero;
        public Vector2 tr = Vector2.Zero;
        public Vector2 bl = Vector2.Zero;
        public Vector2 br = Vector2.Zero;


        public bool[] checks = new bool[2];

        public float xFriction = 1;
        public float yFriction = 1;
       
        //--------------------------------------------

        public Enemy(ContentManager getContent)
        {
            Position = Vector2.Zero;
            sprite = new Sprite(getContent, "objects/enemy", Width, Height);
        }

        public void updateBounds(Vector2 Camera)
        {
            Bounds = new Rectangle((int)Position.X + (int)Camera.X, (int)Position.Y + (int)Camera.Y, Width, Height);
            tl = new Vector2(Bounds.X, Bounds.Y);
            tr = new Vector2(Bounds.X + Bounds.Width, Bounds.Y);
            bl = new Vector2(Bounds.X, Bounds.Y + Bounds.Height);
            br = new Vector2(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height);
        }

        public void Update(Game1 getGame1, BaseEngine getEngine)
        {
         
            BEngine = getEngine;
            game1 = getGame1;


            //Gravity--------------

            bool Checked = false;
            if (!Checked)
            {
                if (checks[0] == true)
                {
                    yFriction = 0.95f;
                    Checked = true;
                }
                else
                {
                    yFriction = 1;
                }
            }

            if (!Checked)
            {
                if (checks[1] == true)
                {
                    xFriction = 0.92f;
                    yFriction = 0.95f;
                    Checked = true;
                }
                else
                {
                    xFriction = 1;
                    yFriction = 1;
                }
            }

            if (checks[1] == false)
            {
                if (Speed.Y < 12)
                    Speed.Y += 0.2f;
                else
                    Speed.Y = 12;
            }
            else
            {
                if (Speed.Y < 2)
                    Speed.Y += 0.2f;
                else
                    Speed.Y = 2;
            }



            Position.Y += 1;
            updateBounds(Camera.Position);
            BEngine.updateHitboxes(Position, Bounds);

            for (int i = 0; i < BEngine.Canvas.Length; i++)
            {
                if (Bounds.Intersects(BEngine.Canvas[i]))
                    Speed.Y = -7f;
               
            }

            if (checkAllLines(BEngine.tan1) == true)
            {
                Speed.Y = -7f;
            }
            if (checkAllLines(BEngine.tan2) == true)
            {
                Speed.Y = -7f;
            }
            if (checkAllLines(BEngine.tan3) == true)
            {
                Speed.Y = -7f;
            }
            if (checkAllLines(BEngine.tan4) == true)
            {
                Speed.Y = -7f;
            }
            if (checkAllLines(BEngine.tan5) == true)
            {
                Speed.Y = -7f;
            }
            if (checkAllLines(BEngine.tan6) == true)
            {
                Speed.Y = -7f;
            }

            Position.Y -= 1;
         

        }

        public void checkCollisionsX(Rectangle target)
        {
            if (Bounds.Intersects(target))
            {

                if (Speed.X > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds); 
                        if (Bounds.Intersects(target))
                            Position.X--;
                    }

                if (Speed.X < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
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
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds); 
                        if (Bounds.Intersects(target))
                            Position.Y--;
                    }

                if (Speed.Y < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds); 
                        if (Bounds.Intersects(target))
                            Position.Y++;
                    }

                Speed.Y = 0;
            }
        }

        public void Draw(SpriteBatch sB)
        {

            //sB.Draw(Textures._OBJ_Ladder_Tex, Bounds, Color.Red);
            sprite.Draw(sB, new Vector2(Bounds.X, Bounds.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);

        }

        public void checkTollisionsX(Triangle target)
        {
            if (checkAllLines(target) == true)
            {

                if (Speed.X > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                            Position.X--;
                    }

                if (Speed.X < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                            Position.X++;
                    }

                Speed.X = 0;
            }
        }

        public void checkTollisionsY(Triangle target)
        {
            if (checkAllLines(target) == true)
            {


                if (Speed.Y > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                        {
                            Position.Y--;
                         
                        }
                    }

                if (Speed.Y < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                            Position.Y++;
                    }

                Speed.Y = 0;
            }
        }

        public bool checkAllLines(Triangle triangle)
        {
            if (BEngine.lineTest(tl, tr, triangle.a, triangle.b) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tl, tr, triangle.b, triangle.c) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tl, tr, triangle.c, triangle.a) == true)
            {
                return true;
            }

            if (BEngine.lineTest(tl, bl, triangle.a, triangle.b) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tl, bl, triangle.b, triangle.c) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tl, bl, triangle.c, triangle.a) == true)
            {
                return true;
            }

            if (BEngine.lineTest(bl, br, triangle.a, triangle.b) == true)
            {
                return true;
            }
            if (BEngine.lineTest(bl, br, triangle.b, triangle.c) == true)
            {
                return true;
            }
            if (BEngine.lineTest(bl, br, triangle.c, triangle.a) == true)
            {
                return true;
            }

            if (BEngine.lineTest(tr, br, triangle.a, triangle.b) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tr, br, triangle.b, triangle.c) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tr, br, triangle.c, triangle.a) == true)
            {
                return true;
            }
            return false;
        }

    }
}

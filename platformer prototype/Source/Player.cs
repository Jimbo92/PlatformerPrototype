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
    class Player
    {
        //public Sprite player;
        public Vector2 Position;
        public Rectangle Bounds;
        public Vector2 Speed;
        public Sprite Crosshair;
        public int Width = 16;
        public int Height = 32;

        private BaseEngine BEngine;
        private Game1 game1;
       
        //--------------------------------------------

        public Player(ContentManager getContent)
        {
            Position = new Vector2(100, 50);

            Crosshair = new Sprite(getContent, "crosshairss", 98, 98, 1, 3);
        }

        public void updateBounds(Vector2 camera)
        {
            Bounds = new Rectangle((int)Position.X + (int)camera.X, (int)Position.Y + (int)camera.Y, Width, Height);
        }

        public void Update(Game1 getGame1, BaseEngine getEngine)
        {
            BEngine = getEngine;
            game1 = getGame1;
           
            //Sprites
            Crosshair.UpdateAnimation(0.3f);

            //Gravity--------------
            if (Speed.Y < 12)
                Speed.Y += 0.2f;
            else
                Speed.Y = 12;
            //---------------------

            
            //Controls--------------------------------
            if (Input.KeyboardPress(Keys.OemPlus))
                BEngine.tileSize += 1;

            if (Input.KeyboardPress(Keys.OemMinus))
                if (BEngine.tileSize > 1)
                    BEngine.tileSize -= 1;

            if (Input.KeyboardPress(Keys.W) || Input.KeyboardPress(Keys.Space))
            {
                Position.Y += 1;
                updateBounds(BEngine.camera.Position);
                BEngine.updateHitboxes(Position, Bounds);

                for (int i = 0; i < BEngine.Canvas.Length; i++)
                    if (Bounds.Intersects(BEngine.Canvas[i]))
                        Speed.Y = -7f;

                Position.Y -= 1;
            }
            if (Input.KeyboardPress(Keys.A))
                if (Speed.X > -4)
                    Speed.X -= 0.25f;
                else
                    Speed.X = -4;

            if (Input.KeyboardPress(Keys.D))
                if (Speed.X < 4)
                    Speed.X += 0.25f;
                else
                    Speed.X = 4;

            if (Input.KeyboardRelease(Keys.A) && Input.KeyboardRelease(Keys.D))
                if (Math.Abs(Speed.X) > 1)
                    Speed.X *= 0.92f;
                else
                    Speed.X = 0;

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
            sB.Draw(game1.levelTex, Bounds, BEngine.Scale, Color.BlueViolet);

            //Sprites
            Crosshair.Draw(sB, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 0, 0);
        }

    }
}

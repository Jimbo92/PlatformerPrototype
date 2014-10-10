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
        public Sprite sprite;
        public Vector2 Position;
        public Rectangle Bounds;
        public Vector2 Speed;
        public float Rotation;

        float xFriction = 1;
        float yFriction = 1;

        public Sprite Crosshair;
        public int Width = 16;
        public int Height = 32;

        private BaseEngine BEngine;
        private Game1 game1;
        private int WallJumpTimer;

        //--------------------------------------------

        public Player(ContentManager getContent)
        {
            Position = new Vector2(100, 50);

            sprite = new Sprite(getContent, "mario", Width, Height);

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
            Vector2 tile = getEngine.getTile();
            tile = getEngine.getTile();


            if (tile.X >= 0 && tile.X < getEngine.map.GetLength(1))
            {
                if (tile.Y >= 0 && tile.Y < getEngine.map.GetLength(0))
                {
                    if (getEngine.map[(int)tile.Y, (int)tile.X] == 2)
                    {

                        yFriction = 0.92f;

                    }
                    else
                    {
                        yFriction = 1;
                    }
                }
            }

            //this needs to be the last check in gravity


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

            if (Input.KeyboardPress(Keys.W) || Input.KeyboardPress(Keys.Up))
            {
                //Jump--------------
                Position.Y += 1;
                updateBounds(BEngine.camera.Position);
                BEngine.updateHitboxes(Position, Bounds);

                for (int i = 0; i < BEngine.Canvas.Length; i++)
                    if (Bounds.Intersects(BEngine.Canvas[i]))
                        Speed.Y = -7f;

                Position.Y -= 1;
                //--------------------


                //Wall jumping
                WallJumpTimer++;
                if (WallJumpTimer > 8)
                {

                    if (Input.KeyboardPressed(Keys.W))
                    {


                        Position.X += 1;
                        updateBounds(BEngine.camera.Position);

                        for (int i = 0; i < BEngine.Canvas.Length; i++)
                            if (Bounds.Intersects(BEngine.Canvas[i]))
                            {
                                Rotation = -8;
                                Speed.X = -7f;
                                Speed.Y = -5f;
                            }

                        Position.X -= 1;

                        Position.X -= 1;
                        updateBounds(BEngine.camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);

                        for (int i = 0; i < BEngine.Canvas.Length; i++)
                            if (Bounds.Intersects(BEngine.Canvas[i]))
                            {
                                Rotation = 8;
                                Speed.X = 7f;
                                Speed.Y = -5f;
                            }

                        Position.X += 1;

                    }
                }

            }

            //--------------------------

            //Ladder movement

            tile = getEngine.getTile();
            if (tile.X >= 0 && tile.X < getEngine.map.GetLength(1))
            {
                if (tile.Y >= 0 && tile.Y < getEngine.map.GetLength(0))
                {
                    if (getEngine.map[(int)tile.Y, (int)tile.X] == 2)
                    {
                        WallJumpTimer = 0;

                        if (Input.KeyboardPress(Keys.W))
                        {
                            Speed.Y = -4;
                        }
                    }
                }
            }

            if (Input.KeyboardPress(Keys.A) || Input.KeyboardPress(Keys.Left))
                if (Speed.X > -4)
                    Speed.X -= 0.25f;
                else
                    Speed.X = -4;

            if (Input.KeyboardPress(Keys.D) || Input.KeyboardPress(Keys.Right))
                if (Speed.X < 4)
                    Speed.X += 0.25f;
                else
                    Speed.X = 4;

            if (Input.KeyboardRelease(Keys.A) && Input.KeyboardRelease(Keys.D) && Input.KeyboardRelease(Keys.Left) && Input.KeyboardRelease(Keys.Right))
                if (Math.Abs(Speed.X) > 1)
                    Speed.X *= 0.92f;
                else
                    Speed.X = 0;

            Speed.X *= xFriction;
            Speed.Y *= yFriction;

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
                        {
                            WallJumpTimer = 0;
                            Rotation = 0;
                            Position.Y--;
                        }
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
            //sB.Draw(Textures._CHAR_Player_Tex, Bounds, Color.White);

            sprite.Draw(sB, new Vector2(Bounds.X, Bounds.Y), new Vector2(0, 0), MathHelper.ToRadians(Rotation), SpriteEffects.None);

            //Sprites
            Crosshair.Draw(sB, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 0, 0);
        }

    }
}

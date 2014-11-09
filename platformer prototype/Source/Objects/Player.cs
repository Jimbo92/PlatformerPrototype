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
        public Texture2D capTex;
        public Vector2 Position;


        
       

        public Vector2 tl = Vector2.Zero;
        public Vector2 tr = Vector2.Zero;
        public Vector2 bl = Vector2.Zero;
        public Vector2 br = Vector2.Zero;

        public int cooldown = 0;

        public bool noclip = false;

        public bool ControlsEnabled = true;

        public Rectangle Bounds;
        public Vector2 Speed;

        public bool[] checks = new bool[5];
        public Vector2 platMod;

        public float xFriction = 1;
        public float yFriction = 1;
        float jump = 6f;

        public float Rotation = 0;

        public int Width = 30;
        public int Height = 32;
        public bool horCollide;

        private BaseEngine BEngine;
        private Game1 game1;
        public int wallTimer;

        public bool isWalking;
        public bool isJumping;

        private SpriteEffects sprEffect;

        private float capOffset;

        private bool platX = false;
        private bool platY = false;

        //--------------------------------------------

        public void feedback()
        {
            Speed.Y = -10;
        }



        public Player(ContentManager getContent)
        {
            sprite = new Sprite(getContent, "objects/playerSS", Width, Height, 2, 8);
            capTex = getContent.Load<Texture2D>("objects/hat1");
        }

        public void updateBounds(Vector2 Camera)
        {
            Bounds = new Rectangle((int)Position.X + (int)Camera.X, (int)Position.Y + (int)Camera.Y, Width, Height);
            tl = new Vector2(Bounds.X, Bounds.Y);
            tr = new Vector2(Bounds.X + Bounds.Width, Bounds.Y);
            bl = new Vector2(Bounds.X, Bounds.Y + Bounds.Height);
            br = new Vector2(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height);
        }

        private void Animations()
        {
            if (Speed.X <= -1)
            {
                sprEffect = SpriteEffects.FlipHorizontally;
                isWalking = true;
            }
            else if (Speed.X >= 1)
            {
                sprEffect = SpriteEffects.None;
                isWalking = true;
            }
            else
                isWalking = false;

            if (Speed.Y >= 1 || Speed.Y <= -1 && !GUI.isHit)
                isJumping = true;
            else if (Speed.Y == 0)
                isJumping = false;

            if (GUI.isHit)
                sprite.CurrentFrame = 12;

            if (isWalking && !isJumping)
            {
                sprite.CurrentFrame += 0.25f;
                if (sprite.CurrentFrame > 10)
                    sprite.CurrentFrame = 0;
            }
            else if (isJumping)
                sprite.CurrentFrame = 11;
            else
            {
                sprEffect = SpriteEffects.None;
                sprite.CurrentFrame = 14;
            }
        }

        public void Collisions()
        {

            horCollide = false;
            
            // Check for Map boundries-----------------
            if (Position.Y > game1.GraphicsDevice.Viewport.Height)
            {
                Position = BEngine.PlayerStart;
                Camera.Position = new Vector2(-(BEngine.PlayerStart.X - 400), BEngine.PlayerStart.Y + 138);
                noclip = false;
                Rotation = 0;
                GUI.PlayerHitPoints = 5;
            }

            // Check X Collisions-----------------------------
            //Will Check up to 6 Rectangles

            Position.X += (int)Speed.X;
          
            updateBounds(Camera.Position);
            BEngine.updateHitboxes(Position, Bounds);


            if (!noclip)
            {
                foreach (Rectangle canvas in BEngine.Canvas)
                    checkCollisionsX(canvas);

                //Will Check up to 6 Triangles

                updateBounds(Camera.Position);
                BEngine.updateHitboxes(Position, Bounds);
                checkTollisionsX(BEngine.tan1);
                checkTollisionsX(BEngine.tan2);
                checkTollisionsX(BEngine.tan3);
                checkTollisionsX(BEngine.tan4);
                checkTollisionsX(BEngine.tan5);
                checkTollisionsX(BEngine.tan6);



            }
            // Check Y Collisions-----------------------------
            //Will Check up to 6 Rectangles
            Position.Y += (int)Speed.Y;
            updateBounds(Camera.Position);
            BEngine.updateHitboxes(Position, Bounds);
            if (!noclip)
            {
                foreach (Rectangle canvas in BEngine.Canvas)
                    checkCollisionsY(canvas);
                //Will check up to 6 Triangles
                checkTollisionsY(BEngine.tan1);
                checkTollisionsY(BEngine.tan2);
                checkTollisionsY(BEngine.tan3);
                checkTollisionsY(BEngine.tan4);
                checkTollisionsY(BEngine.tan5);
                checkTollisionsY(BEngine.tan6);
            }

            //Check Non Clipping Collisions====================

            checks[0] = false;
            if (!noclip)
            {
                updateBounds(Camera.Position);
                BEngine.updateNoclips(Position, Bounds, '♠');
                foreach (Rectangle noclips in BEngine.NoClip)
                {
                    if (Bounds.Intersects(noclips))
                    {
                        checks[0] = true;
                    }

                }
            }

            checks[1] = false;
            if (!noclip)
            {
                updateBounds(Camera.Position);
                BEngine.updateNoclips(Position, Bounds, '♣');
                foreach (Rectangle noclips in BEngine.NoClip)
                {
                    if (Bounds.Intersects(noclips))
                    {
                        checks[1] = true;
                        GUI.ShowOxygenBar = true;
                    }
                    else
                        GUI.ShowOxygenBar = false;

                }
            }

            checks[2] = false;
            if (!noclip)
            {
                updateBounds(Camera.Position);
                BEngine.updateNoclips(Position, Bounds, '•');
                foreach (Rectangle noclips in BEngine.NoClip)
                {
                    if (Bounds.Intersects(noclips))
                    {
                        checks[2] = true;
                    }

                }
            }


            checks[3] = false;
            if (!noclip)
            {
                updateBounds(Camera.Position);
                BEngine.updateNoclips(Position, Bounds, '◙');
                foreach (Rectangle noclips in BEngine.NoClip)
                {
                    if (noclips.Y > Bounds.Y + Bounds.Height - Speed.Y - 1)
                        if (Bounds.Intersects(noclips))
                        {
                            checks[3] = true;


                            for (int i = 20; i > 0; i--)
                            {
                                updateBounds(Camera.Position);
                                BEngine.updateHitboxes(Position, Bounds);
                                if (Bounds.Intersects(noclips))
                                {
                                    Position.Y--;
                                    wallTimer = 0;
                                    Rotation = 0;
                                }
                            }

                            Speed.Y = 0;
                        }

                }
            }
            platX = false;
            platY = false;
            platMod = Vector2.Zero;
            if (!noclip)
            {
                updateBounds(Camera.Position);
                Bounds.Y += 1;

                foreach (Platform p in BEngine.Platforms)
                {
                   

                    p.updateBounds(Camera.Position);
                    if (p.Bounds.Y + Speed.Y + 2 > Bounds.Y + Bounds.Height - Speed.Y - 2)
                    {
                            if (Bounds.Intersects(p.Bounds))
                            {
                             
                                if (platX == false)
                                {
                                    platMod.X += p.Speed.X;
                                    platX = true;
                                }

                                if (platY == false)
                                {
                                    
                                    platMod.Y += p.Speed.Y;
                                    platY = true;
                                }




                                for (int i = 20; i > 0; i--)
                                {
                                    updateBounds(Camera.Position);
                                    p.updateBounds(Camera.Position);
                                    if (Bounds.Intersects(p.Bounds))
                                    {
                                        
                                        Position.Y--;
                                        wallTimer = 0;
                                        Rotation = 0;
                                    }
                                }

                                Speed.Y = 0;
                            }
                        }
                    

                }
                Bounds.Y -= 1;

            }



            bool oneKill = false;
            if (!noclip && cooldown == 0)
                foreach (NPC e in BEngine.NPC_E)
                {
                    if (e.Bounds.Intersects(Bounds) && !e.isDead && !oneKill)
                    {
                        if (Position.Y < e.Position.Y - 16)
                        {
                            e.isDead = true;
                            oneKill = true;
                            Speed.Y = -4;
                            for (int i = 0; i < 20; i++)
                            {
                                if (e.Bounds.Intersects(Bounds))
                                    Position.Y--;
                            }
                        }
                        else
                        {
                            GUI.isHit = true;
                            GUI.ShowHealthBar = true;
                            cooldown = 45;
                            Speed.Y = -4;
                            Speed.X *= -1;
                        }
                    }
                }

    
            
          
           


            if (checks[2] == true)
            {
                GUI.PlayerHitPoints = 0;
                GUI.ShowHealthBar = true;
            }

            if (GUI.PlayerHitPoints == 0 && !noclip)
            {
                noclip = true;
                Speed.Y = -7;
                Speed.X = 0;
            }


            //=================================================
            updateBounds(Camera.Position);


            
        }

        public void Controls()
        {
            //Out of water
            if (checks[1] == false)
            {
                if (!noclip)
                    if (Input.KeyboardPress(Keys.W) || Input.KeyboardPress(Keys.Up))
                    {
                        bool returner = false;

                        //Jump--------------

                        if (!returner)
                        {
                           
                            Position.Y += 1;
                            updateBounds(Camera.Position);
                            BEngine.updateHitboxes(Position, Bounds);
                            bool one = false;
                            foreach (Door d in BEngine.Doors)
                            {
                                if (d.checkDoors(BEngine) == true)
                                    one = true;
                            }
                            if (one)
                            {
                                Speed.Y = -jump;
                                returner = true;
                            }

                            if (checks[3] == true)
                            {
                                Speed.Y = -jump;
                                returner = true;
                            }

                            foreach (Platform p in BEngine.Platforms)
                            {
                                p.updateBounds(Camera.Position);
                                if (p.Bounds.Y > Bounds.Y + Bounds.Height - Speed.Y - 2)
                                    if (p.Bounds.Intersects(Bounds))
                                    {
                                        Speed.Y = -jump;
                                        returner = true;
                                    }
                            }

                            for (int i = 0; i < BEngine.Canvas.Length; i++)
                                if (Bounds.Intersects(BEngine.Canvas[i]))
                                {
                                    Speed.Y = -jump;
                                    returner = true;
                                }
                            if (checkAllLines(BEngine.tan1) == true)
                            {
                                Speed.Y = -jump;
                                returner = true;
                            }
                            if (checkAllLines(BEngine.tan2) == true)
                            {
                                Speed.Y = -jump;
                                returner = true;
                            }
                            if (checkAllLines(BEngine.tan3) == true)
                            {
                                Speed.Y = -jump;
                                returner = true;
                            }
                            if (checkAllLines(BEngine.tan4) == true)
                            {
                                Speed.Y = -jump;
                                returner = true;
                            }
                            if (checkAllLines(BEngine.tan5) == true)
                            {
                                Speed.Y = -jump;
                                returner = true;
                            }
                            if (checkAllLines(BEngine.tan6) == true)
                            {
                                Speed.Y = -jump;
                                returner = true;
                            }


                            Position.Y -= 1;

                        }
                        //--------------------

                        //Ladders----------------------------------------
                        if (!returner)
                        {
                            if (checks[0] == true)
                            {
                                wallTimer = 0;
                                Speed.Y = -4;
                            }
                        }

                        //Wall jumping-----------
                        wallTimer++;
                        if (wallTimer > 8)
                        {
                            if (!returner)
                            {
                                if (Input.KeyboardPressed(Keys.W) || Input.KeyboardPressed(Keys.Up))
                                {
                                    Position.X += 1;
                                    updateBounds(Camera.Position);

                                    for (int i = 0; i < BEngine.Canvas.Length; i++)
                                        if (Bounds.Intersects(BEngine.Canvas[i]))
                                        {
                                            Rotation = -8;
                                            Speed.X = -5f;
                                            Speed.Y = -jump / yFriction;
                                            returner = true;
                                        }

                                    Position.X -= 1;

                                    Position.X -= 1;
                                    updateBounds(Camera.Position);
                                    BEngine.updateHitboxes(Position, Bounds);

                                    for (int i = 0; i < BEngine.Canvas.Length; i++)
                                        if (Bounds.Intersects(BEngine.Canvas[i]))
                                        {
                                            Rotation = 8;
                                            Speed.X = 5f;
                                            Speed.Y = -jump / yFriction;
                                            returner = true;
                                        }

                                    Position.X += 1;
                                }

                            }
                        }
                        //--------------------------

                    }
                if (!noclip)
                    if (Input.KeyboardPress(Keys.A) || Input.KeyboardPress(Keys.Left))
                    {
                        if (Speed.X > -3)
                            Speed.X -= 0.25f;
                        else
                            Speed.X = -3;
                    }

                if (Input.KeyboardPress(Keys.D) || Input.KeyboardPress(Keys.Right))
                {
                    if (Speed.X < 3)
                        Speed.X += 0.25f;
                    else
                        Speed.X = 3;
                }

                if ((Input.KeyboardRelease(Keys.A) && Input.KeyboardRelease(Keys.D) && Input.KeyboardRelease(Keys.Left) && Input.KeyboardRelease(Keys.Right)) || noclip)
                    if (Math.Abs(Speed.X) > 1)
                        Speed.X *= 0.92f;
                    else
                        Speed.X = 0;
            }
            else
            {
                if (Input.KeyboardPress(Keys.W) || Input.KeyboardPress(Keys.Up))
                {
                    if (Speed.Y > -4)
                        Speed.Y -= 0.6f;
                    else
                        Speed.Y = -4;
                }
                if (Input.KeyboardPress(Keys.S) || Input.KeyboardPress(Keys.Down))
                {
                    if (Speed.Y < 4)
                        Speed.Y += 0.25f;
                    else
                        Speed.Y = 4;
                }
                if (Input.KeyboardPress(Keys.A) || Input.KeyboardPress(Keys.Left))
                {
                    if (Speed.X > -4)
                        Speed.X -= 0.25f;
                    else
                        Speed.X = -4;
                }

                if (Input.KeyboardPress(Keys.D) || Input.KeyboardPress(Keys.Right))
                {
                    if (Speed.X < 4)
                        Speed.X += 0.25f;
                    else
                        Speed.X = 4;
                }

                if (Input.KeyboardRelease(Keys.A) && Input.KeyboardRelease(Keys.D) && Input.KeyboardRelease(Keys.Left) && Input.KeyboardRelease(Keys.Right))
                    if (Math.Abs(Speed.X) > 1)
                        Speed.X *= 0.92f;
                    else
                        Speed.X = 0;
            }

            Speed.X *= xFriction;
            Speed.Y *= yFriction;
        }

        public void Update(Game1 getGame1, BaseEngine getEngine)
        {
            BEngine = getEngine;
            game1 = getGame1;

            Collisions();

            if (Camera.CameraMode == Camera.CameraState.WAYPOINTS)
                ControlsEnabled = false;
          
           
            Position.X += platMod.X;          

            Animations();

            if (ControlsEnabled)
                Controls();
            else
                Speed.X = 0;

            //cooldown(invincibility)
            if (cooldown > 0)
                cooldown--;

            //Gravity--------------

            bool Checked = false;
            if (horCollide && checks[0] == false && checks[1] == false) {
                yFriction = 0.65f;
                Checked = true;
            }

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

       

            //---------------------
        
       
            //Lava rotation Script--------------

            if (noclip)
            {
                Rotation += 4;
            }

            //Controls--------------------------------
            if (Input.KeyboardPress(Keys.OemPlus))
                BEngine.tileSize += 1;

            if (Input.KeyboardPress(Keys.OemMinus))
                if (BEngine.tileSize > 1)
                    BEngine.tileSize -= 1;


            

        }

        public void checkCollisionsX(Rectangle target)
        {
            bool one = false;
            foreach (Door d in BEngine.Doors)
            {
               if(d.checkDoors(BEngine) == true)
                   one = true; 
            }
            if (Bounds.Intersects(target) || one)
            {
                horCollide = true;
                if (Speed.X > 0) {
                    for (int i = 20; i > 0; i--) {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        one = false;
                        foreach (Door d in BEngine.Doors)
                        {
                            if (d.checkDoors(BEngine) == true)
                                one = true; 
                        }
                        if (Bounds.Intersects(target) || one)
                            Position.X--;
                    }
                  
                   
                }

                if (Speed.X < 0) {
                    for (int i = 20; i > 0; i--) {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        one = false;
                        foreach (Door d in BEngine.Doors)
                        {
                            if (d.checkDoors(BEngine) == true)
                                one = true; 
                        }
                        if (Bounds.Intersects(target) || one)
                            Position.X++;

                    }
                   
                }

                Speed.X = 0;
            }
        }

      

        public void checkCollisionsY(Rectangle target)
        {
            bool one = false;
           
            foreach (Door d in BEngine.Doors)
            {
                if (d.checkDoors(BEngine) == true)
                    one = true;
            }
            if (Bounds.Intersects(target) || one)
            {

                if (Speed.Y > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        one = false;
                        foreach (Door d in BEngine.Doors)
                        {
                            if (d.checkDoors(BEngine) == true)
                                one = true;
                        }
                        if (Bounds.Intersects(target) || one)
                        {
                            Position.Y--;
                            wallTimer = 0;
                            Rotation = 0;
                        }
                    }

                if (Speed.Y < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        one = false;
                        foreach (Door d in BEngine.Doors)
                        {
                            if (d.checkDoors(BEngine) == true)
                                one = true;
                        }
                        if (Bounds.Intersects(target) || one)
                            Position.Y++;
                    }

                Speed.Y = 0;
            }           
        }

        public void Draw(SpriteBatch sB)
        {

                sprite.Draw(sB, new Vector2(Bounds.X, Bounds.Y), Vector2.Zero, MathHelper.ToRadians(Rotation), sprEffect, Color.White);
                
              
                if (Speed.Y > 1)
                {
                    capOffset += 0.5f;
                    if (capOffset >= 15)
                        capOffset = 15;
                }
                else
                {
                    if (capOffset > 0)
                    {
                        capOffset -= 2.5f;
                        if (capOffset <= 0)
                            capOffset = 0;
                    }
                }
              
                    sB.Draw(capTex, new Rectangle((int)Bounds.X + 5, (int)Bounds.Y - 10 - (int)capOffset, 20, 20), null, Color.White, MathHelper.ToRadians(Rotation), Vector2.Zero, sprEffect, 0);
              
            //Sprites
        }


        public void checkTollisionsX(Triangle target)
        {
            if (checkAllLines(target) == true)
            {        
           
                if (Speed.X > 0) {
                    for (int i = 20; i > 0; i--) {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                            Position.X--;
                    }

                    Position.X += Math.Abs(Speed.X);
                    Position.Y -= Math.Abs(Speed.X);
                 
                  
                   
                    
                }

                if (Speed.X < 0) {
                    for (int i = 20; i > 0; i--) {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                            Position.X++;
                    }

                    Position.X -= Math.Abs(Speed.X);
                    Position.Y -= Math.Abs(Speed.X);
                  
                }

              

               
               
            
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
                            wallTimer = 0;
                            Rotation = 0;                       
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

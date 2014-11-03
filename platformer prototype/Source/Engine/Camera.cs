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
    static class Camera
    {
        public enum CameraState
        {
            FOLLOW = 1,
            FREE = 2,
            WAYPOINTS = 3,
            MOUSE = 4,
        }


        static public CameraState CameraMode = CameraState.FOLLOW;

        public static int delay;
        public static int nextDelay;
        public static bool isControlled;
        public static int task;

        static public Vector2 Position;
        static public Vector2 Target;
        public static List<Vector4> targets = new List<Vector4>();
        static Vector2 Speed = Vector2.Zero;

        static private Game1 game1;
        static private Player player;
        static public Rectangle MouseRect;

        public static void Flybuy(List<Vector4> Waypoints)
        {
            targets.Clear();
            for (int i = 0; i < Waypoints.Count; i++)
            {
                targets.Add(Waypoints[i]);
            }

      
        }

        public static void Flybuying()
        {
            if (isControlled)
            {
                
                if (nextDelay > 0)
                {
                    nextDelay--;                  
                }
                else
                {
                    if (delay > 0)
                    {
                        delay--;
                    }
                    else
                    {
                        if (task != -1 && task < targets.Count)
                        {
                            nextDelay = (int)targets[task].Z;
                        }
                        task++;
                        if (task < targets.Count + 1)
                        {
                            if (task < targets.Count)
                            {
                                Target = new Vector2(targets[task].X, targets[task].Y  );
                                
                                delay = (int)targets[task].W;
                                Speed.X = -Target.X - Position.X;
                                Speed.Y = Target.Y - Position.Y;
                                int distance = (int)Math.Sqrt((Math.Abs(Speed.X) * Math.Abs(Speed.X)) + ( Math.Abs(Speed.Y) * Math.Abs(Speed.Y)));
                                Speed /= delay;
                             
                            }
                            else
                            {
                                Target = new Vector2(-(player.Position.X - game1.GraphicsDevice.Viewport.Width / 2 + player.Bounds.Width / 2),
                                    -(player.Position.Y - game1.GraphicsDevice.Viewport.Height / 2 + player.Bounds.Height / 2));
                                
                                
                                delay = 50;
                                Speed.X = Target.X - Position.X;
                                Speed.Y = Target.Y - Position.Y;
                                Speed /= delay;
                                
                            }

                           
                           
                        }
                        else
                        {
                            isControlled = false;
                        }
                    }
                }
                
               

            }
        }

        static public void Initialize(Player getPlayer, BaseEngine be)
        {
            player = getPlayer;
            Position = new Vector2(-(be.PlayerStart.X - 400), be.PlayerStart.Y + 138); ;
        }

        static public void Update(Game1 getGame1)
        {
            game1 = getGame1;

            game1.giveType(CameraMode.ToString());

            MouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 32, 32);


            if (Input.KeyboardReleased(Keys.X))
            {
                if ((int)CameraMode == 3)
                {
                    CameraMode = CameraState.FOLLOW;
                }
                else
                {
                    CameraMode += 1;
                }
            }



            //Camera Follow----------------
            if (CameraMode == CameraState.FOLLOW)
            {
                Position.X -= (player.Position.X - game1.GraphicsDevice.Viewport.Width / 2 + player.Bounds.Width / 2 - -Position.X) / 30;
                float yChange = (player.Position.Y - game1.GraphicsDevice.Viewport.Height / 2 + player.Bounds.Height / 2 - -Position.Y) / 15;
                Position.Y -= yChange;

                if (Position.Y < 0)
                    Position.Y = 0;
                if (Position.X > 0)
                    Position.X = 0;



            }
            //Camera Free----------------
            if (CameraMode == CameraState.FREE)
            {

                if (Input.KeyboardPress(Keys.Down))
                    if (Speed.Y > -12)
                        Speed.Y -= 0.34f;
                    else
                        Speed.Y = -12;
                if (Input.KeyboardPress(Keys.Up))
                    if (Speed.Y < 12)
                        Speed.Y += 0.34f;
                    else
                        Speed.Y = 12;
                if (Input.KeyboardPress(Keys.Right))
                    if (Speed.X > -12)
                        Speed.X -= 0.34f;
                    else
                        Speed.X = -12;
                if (Input.KeyboardPress(Keys.Left))
                    if (Speed.X < 12)
                        Speed.X += 0.34f;
                    else
                        Speed.X = 12;
                if (Input.KeyboardRelease(Keys.Right) && Input.KeyboardRelease(Keys.Left))
                    if (Math.Abs(Speed.X) > 1)
                        Speed.X *= 0.92f;
                    else
                        Speed.X = 0;
                if (Input.KeyboardRelease(Keys.Up) && Input.KeyboardRelease(Keys.Down))
                    if (Math.Abs(Speed.Y) > 1)
                        Speed.Y *= 0.92f;
                    else
                        Speed.Y = 0;
                Position.X += Speed.X;
                Position.Y += Speed.Y;
            }



            if (isControlled)
            {
                CameraMode = CameraState.WAYPOINTS;
                Flybuying();
            }

            if (CameraMode == CameraState.WAYPOINTS)
            {
                if (nextDelay == 0)
                {
                    Position.X += Speed.X;
                    Position.Y += Speed.Y;
                }
                if (Position.Y < 0)
                    Position.Y = 0;
            }


            //Camera Mouse----------------
            if (CameraMode == CameraState.MOUSE)
            {
                    if (Mouse.GetState().Y >= 590)
                        if (Speed.Y > -12)
                            Speed.Y -= 0.34f;
                        else
                            Speed.Y = -12;
                    if (Mouse.GetState().Y <= 10)
                        if (Speed.Y < 12)
                            Speed.Y += 0.34f;
                        else
                            Speed.Y = 12;
                    if (Mouse.GetState().X >= 790)
                        if (Speed.X > -12)
                            Speed.X -= 0.34f;
                        else
                            Speed.X = -12;
                    if (Mouse.GetState().X <= 10)
                        if (Speed.X < 12)
                            Speed.X += 0.34f;
                        else
                            Speed.X = 12;

                    if (Mouse.GetState().X <= 790 && Mouse.GetState().X >= 10)
                        if (Math.Abs(Speed.X) > 1)
                            Speed.X *= 0.92f;
                        else
                            Speed.X = 0;
                    if (Mouse.GetState().Y <= 590 && Mouse.GetState().Y >= 10)
                        if (Math.Abs(Speed.Y) > 1)
                            Speed.Y *= 0.92f;
                        else
                            Speed.Y = 0;

                    Position.X += Speed.X;
                    Position.Y += Speed.Y;              
            }
        }
    }
}

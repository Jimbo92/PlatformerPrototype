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
    class Camera
    {
       enum cameraState
        {
            FOLLOW = 1,
            FREE = 2,
            WAYPOINTS = 3,
        }

      
        cameraState cameraMode = cameraState.FREE;

        public Vector2 Position = Vector2.Zero;
        Vector2 Speed = Vector2.Zero;

        private Game1 game1;
        private Player player;

        public Camera(Player getPlayer)
        {
            player = getPlayer;

            Position.X = 0;
            Position.Y = 0;
        }

        public void Update(Game1 getGame1)
        {
            game1 = getGame1;

            game1.giveType(cameraMode.ToString());


            if (Input.KeyboardReleased(Keys.X))
            {
                if ((int)cameraMode == 3)
                {
                    cameraMode = cameraState.FOLLOW;
                }
                else
                {
                    cameraMode += 1;
                }
            }



            //Camera Follow----------------
            if (cameraMode == cameraState.FOLLOW)
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
            if (cameraMode == cameraState.FREE)
            {
                if (Input.KeyboardPress(Keys.Down))
                {
                    if (Speed.Y > -12)
                    {
                        Speed.Y -= 0.34f;
                    }
                    else
                    {
                        Speed.Y = -12;
                    }
                }
                if (Input.KeyboardPress(Keys.Up))
                {
                    if (Speed.Y < 12)
                    {
                        Speed.Y += 0.34f;
                    }
                    else
                    {
                        Speed.Y = 12;
                    }
                }
                if (Input.KeyboardPress(Keys.Right))
                {
                    if (Speed.X > -12)
                    {
                        Speed.X -= 0.34f;
                    }
                    else
                    {
                        Speed.X = -12;
                    }
                }
                if (Input.KeyboardPress(Keys.Left))
                {
                    if (Speed.X < 12)
                    {
                        Speed.X += 0.34f;
                    }
                    else
                    {
                        Speed.X = 12;
                    }
                }
                if (Input.KeyboardRelease(Keys.Right) && Input.KeyboardRelease(Keys.Left))
                {
                    if (Math.Abs(Speed.X) > 1)
                    {
                        Speed.X *= 0.92f;
                    }
                    else
                    {
                        Speed.X = 0;
                    }
                }
                if (Input.KeyboardRelease(Keys.Up) && Input.KeyboardRelease(Keys.Down))
                {
                    if (Math.Abs(Speed.Y) > 1)
                    {
                        Speed.Y *= 0.92f;
                    }
                    else
                    {
                        Speed.Y = 0;
                    }
                }
                Position.X += Speed.X; 
                Position.Y += Speed.Y;
            }
        }
    }
}

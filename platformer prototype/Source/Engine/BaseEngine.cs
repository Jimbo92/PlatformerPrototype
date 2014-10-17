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
using System.IO;

namespace Platformer_Prototype
{
    class BaseEngine
    {
        //Collision Targets
        public Rectangle[] Canvas = new Rectangle[6];

        public Triangle tan1 = new Triangle();
        public Triangle tan2 = new Triangle();
        public Triangle tan3 = new Triangle();
        public Triangle tan4 = new Triangle();
        public Triangle tan5 = new Triangle();
        public Triangle tan6 = new Triangle();

        public Rectangle[] NoClip = new Rectangle[6];

        public int TileX;
        public int TileY;
        public Vector2 Ratio;
        public Background background;
        public Player player;
        public Sprite WaterTop;
        public Sprite WaterBase;
        public Sprite Torch;

        public Enemy[] enemies = new Enemy[10];

        Random random = new Random();

        private Game1 game1;

        public int LoadMapTimer;
        public char[,] map = MapLoader.LoadMapData("file");
        public char[,] BackMapTextures = MapLoader.LoadMapData("file_back");
        public char[,] ForeMapTextures = MapLoader.LoadMapData("file_fore");
        public char[,] MapEffectTextures = MapLoader.LoadMapData("file_eff");

        public int tileSize = 32;


        //-----------------------------------------------------

        public BaseEngine(ContentManager getContent, Vector2 getScreenSize)
        {
            player = new Player(getContent);
            Camera.Initialize(player);

            WaterTop = new Sprite(getContent, "tiles/water0", 32, 32, 10, 1);
            WaterBase = new Sprite(getContent, "tiles/water1", 32, 32, 10, 1);
            Torch = new Sprite(getContent, "objects/torchss", 32, 32, 1, 8);

            for (int i = 0; i < enemies.Length; i++)
            {
                int randomX = random.Next(100, 1200);
                enemies[i] = new Enemy(getContent);

                enemies[i].isDead = false;
                enemies[i].Position = new Vector2(randomX, 0);


            }


            background = new Background(getContent, getScreenSize);
        }

        public void drawTriangle(SpriteBatch sB, Triangle target)
        {
            DrawLine(game1, sB, target.a, target.b);
            DrawLine(game1, sB, target.b, target.c);
            DrawLine(game1, sB, target.c, target.a);

        }

        public void Update(Game1 getGame1)
        {
            Textures.Update(this);
            LoadMapTimer++;
            if (LoadMapTimer <= 1)
            {
                map = MapLoader.LoadMapData("file");
                BackMapTextures = MapLoader.LoadMapData("file_Back");
                ForeMapTextures = MapLoader.LoadMapData("file_fore");
                MapEffectTextures = MapLoader.LoadMapData("file_eff");
            }
            else
                LoadMapTimer = 2;

            game1 = getGame1;
            Camera.Update(game1);
            Camera.CameraMode = Camera.CameraState.FOLLOW;
            //Animated Textures
            WaterTop.UpdateAnimation(0.15f);
            WaterBase.UpdateAnimation(0.15f);
            Torch.UpdateAnimation(0.5f);



            player.Update(game1, this);
            foreach (Enemy e in enemies)
            {
                if (!e.isDead)
                    e.Update(game1, this);
            }


            // Check for Map boundries-----------------
            if (player.Position.Y > game1.GraphicsDevice.Viewport.Height)
            {
                player.Position = new Vector2(100, 50);
                Camera.Position.X = 0;
                Camera.Position.Y = 0;
            }

            // Check X Collisions-----------------------------
            //Will Check up to 6 Rectangles

            player.Position.X += (int)player.Speed.X;
            player.updateBounds(Camera.Position);
            updateHitboxes(player.Position, player.Bounds);
            foreach (Rectangle canvas in Canvas)
                player.checkCollisionsX(canvas);

            //Will Check up to 6 Triangles

            player.updateBounds(Camera.Position);
            updateHitboxes(player.Position, player.Bounds);
            player.checkTollisionsX(tan1);
            player.checkTollisionsX(tan2);
            player.checkTollisionsX(tan3);
            player.checkTollisionsX(tan4);
            player.checkTollisionsX(tan5);
            player.checkTollisionsX(tan6);


            // Check Y Collisions-----------------------------
            //Will Check up to 6 Rectangles

            player.Position.Y += (int)player.Speed.Y;
            player.updateBounds(Camera.Position);
            updateHitboxes(player.Position, player.Bounds);
            foreach (Rectangle canvas in Canvas)
                player.checkCollisionsY(canvas);
            //Will check up to 6 Triangles
            player.checkTollisionsY(tan1);
            player.checkTollisionsY(tan2);
            player.checkTollisionsY(tan3);
            player.checkTollisionsY(tan4);
            player.checkTollisionsY(tan5);
            player.checkTollisionsY(tan6);

            //Check Non Clipping Collisions====================

            player.checks[0] = false;
            player.updateBounds(Camera.Position);
            updateNoclips(player.Position, player.Bounds, '♠');
            foreach (Rectangle noclip in NoClip)
            {
                if (player.Bounds.Intersects(noclip))
                {
                    player.checks[0] = true;
                }

            }

            player.checks[1] = false;
            player.updateBounds(Camera.Position);
            updateNoclips(player.Position, player.Bounds, '♣');
            foreach (Rectangle noclip in NoClip)
            {
                if (player.Bounds.Intersects(noclip))
                {
                    player.checks[1] = true;
                }

            }

            //=================================================
            player.updateBounds(Camera.Position);

            foreach (Enemy e in enemies)
            {
                // Check X Collisions-----------------------------
                //Will Check up to 6 Rectangles

                e.Position.X += (int)e.Speed.X;
                e.updateBounds(Camera.Position);
                updateHitboxes(e.Position, e.Bounds);
                foreach (Rectangle canvas in Canvas)
                    e.checkCollisionsX(canvas);

                //Will Check up to 6 Triangles

                e.updateBounds(Camera.Position);
                updateHitboxes(e.Position, e.Bounds);
                e.checkTollisionsX(tan1);
                e.checkTollisionsX(tan2);
                e.checkTollisionsX(tan3);
                e.checkTollisionsX(tan4);
                e.checkTollisionsX(tan5);
                e.checkTollisionsX(tan6);


                // Check Y Collisions-----------------------------
                //Will Check up to 6 Rectangles

                e.Position.Y += (int)e.Speed.Y;
                e.updateBounds(Camera.Position);
                updateHitboxes(e.Position, e.Bounds);
                foreach (Rectangle canvas in Canvas)
                    e.checkCollisionsY(canvas);

                //Will Check up to 6 Triangles

                e.updateBounds(Camera.Position);
                updateHitboxes(e.Position, e.Bounds);
                e.checkTollisionsY(tan1);
                e.checkTollisionsY(tan2);
                e.checkTollisionsY(tan3);
                e.checkTollisionsY(tan4);
                e.checkTollisionsY(tan5);
                e.checkTollisionsY(tan6);

                e.checks[0] = false;
                e.updateBounds(Camera.Position);
                updateNoclips(e.Position, e.Bounds, '♠');
                foreach (Rectangle noclip in NoClip)
                {
                    if (e.Bounds.Intersects(noclip))
                    {
                        e.checks[0] = true;
                    }

                }

                e.checks[1] = false;
                e.updateBounds(Camera.Position);
                updateNoclips(e.Position, e.Bounds, '♣');
                foreach (Rectangle noclip in NoClip)
                {
                    if (e.Bounds.Intersects(noclip))
                    {
                        e.checks[1] = true;
                    }

                }
            }

        }

        public bool lineTest(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            float denom = ((b2.Y - b1.Y) * (a2.X - a1.X)) + ((b2.X - b1.X) * (a2.Y - a1.Y));
            if (denom == 0)
            {
                return false;
            }
            else
            {
                float ua = (((b2.X - b1.X) * (a1.Y - b1.Y)) - ((b2.Y - b1.Y) * (a1.X - b1.X))) / denom;
                float ub = (((a2.X - a1.X) * (a1.Y - b1.Y)) - ((a2.Y - a1.Y) * (a1.X - b1.X))) / denom;
                if (ua < 0 || ua > 1 || ub < 0 || ub > 1)
                {
                    return false;
                }
                else
                    return true;
            }
        }

        public void updateNoclips(Vector2 position, Rectangle bounds, char type)
        {
            int que = 0;

            float leftovers = 0;
            if (game1.GraphicsDevice.Viewport.Height % tileSize != 0)
                leftovers = tileSize - (game1.GraphicsDevice.Viewport.Height % tileSize);

            int yLength = map.GetLength(0);
            int difference = yLength - (int)Math.Ceiling((float)game1.GraphicsDevice.Viewport.Height / tileSize);

            TileX = (int)Math.Floor((float)position.X / tileSize);
            TileY = ((int)Math.Floor((position.Y + (difference * (tileSize)) + (leftovers)) / tileSize));

            Ratio.X = (float)Math.Ceiling(bounds.Width / (float)tileSize);
            Ratio.Y = (float)Math.Ceiling(bounds.Height / (float)tileSize);
            if (Ratio.X == 0)
                Ratio.X = 1;
            if (Ratio.Y == 0)
                Ratio.Y = 1;

            for (int i = 0; i < NoClip.GetLength(0); i++)
            {
                NoClip[i] = Rectangle.Empty;
            }

            for (int j = -1; j < Ratio.Y; j++)
            {
                for (int i = 0; i < Ratio.X; i++)
                {

                    if (TileY + j >= 0 && TileY + j < map.GetLength(0) && TileX + i >= 0 && TileX + i < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j, TileX + i] == type)
                        {


                            NoClip[que] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + j) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                            que++;

                        }
                    if (TileY + j >= 0 && TileY + j < map.GetLength(0) && TileX + 1 >= 0 && TileX + 1 < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j, TileX + i + 1] == type)
                        {


                            NoClip[que] = new Rectangle((TileX + i + 1) * tileSize + (int)Camera.Position.X, ((TileY + j) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                            que++;

                        }

                    if (TileY + j + 1 >= 0 && TileY + j + 1 < map.GetLength(0) && TileX + i >= 0 && TileX + i < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j + 1, TileX + i] == type)
                        {


                            NoClip[que] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + j + 1) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                            que++;

                        }
                    if (TileY + j + 1 >= 0 && TileY + j + 1 < map.GetLength(0) && TileX + 1 >= 0 && TileX + 1 < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j + 1, TileX + i + 1] == type)
                        {


                            NoClip[que] = new Rectangle((TileX + i + 1) * tileSize + (int)Camera.Position.X, ((TileY + j + 1) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                            que++;

                        }



                }

            }


        }

        public void updateHitboxes(Vector2 position, Rectangle bounds)
        {
            float leftovers = 0;
            if (game1.GraphicsDevice.Viewport.Height % tileSize != 0)
                leftovers = tileSize - (game1.GraphicsDevice.Viewport.Height % tileSize);

            int yLength = map.GetLength(0);
            int difference = yLength - (int)Math.Ceiling((float)game1.GraphicsDevice.Viewport.Height / tileSize);

            TileX = (int)Math.Floor((position.X) / tileSize);
            TileY = ((int)Math.Floor((position.Y + (difference * (tileSize)) + (leftovers)) / tileSize));

            Ratio.X = (float)Math.Ceiling(bounds.Width / (float)tileSize); //;;;;;;;;;;;;;;;;
            Ratio.Y = (float)Math.Ceiling(bounds.Height / (float)tileSize); //;;;;;;;;;;;;;;;;;
            if (Ratio.X == 0)
                Ratio.X = 1;
            if (Ratio.Y == 0)
                Ratio.Y = 1;


            Canvas[0] = Rectangle.Empty;
            tan1.a = Vector2.Zero;
            tan1.b = Vector2.Zero;
            tan1.c = Vector2.Zero;
            for (int i = 0; i < Ratio.X; i++)

                if (TileX + i >= 0 && TileX + i < map.GetLength(1) && TileY + Ratio.Y >= 0 && TileY + Ratio.Y < map.GetLength(0))
                {

                    if (map[TileY + (int)Ratio.Y, TileX + i] == '☺')
                    {
                        Canvas[0] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + (int)Ratio.Y) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                    }

                    if (map[TileY + (int)Ratio.Y, TileX + i] == '♦') {
                        Canvas[0] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + (int)Ratio.Y) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize / 2);
                    }
                    if (map[TileY + (int)Ratio.Y, TileX + i] == '☻') //left triangle
                    {
                        Rectangle Corner = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + (int)Ratio.Y) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                        tan1.a = new Vector2(Corner.X, Corner.Y + 1);
                        tan1.b = new Vector2(Corner.X, Corner.Y + tileSize);
                        tan1.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize + 1);
                    }
                    if (map[TileY + (int)Ratio.Y, TileX + i] == '♥') //right triangle
                    {
                        Rectangle Corner = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + (int)Ratio.Y) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                        tan1.a = new Vector2(Corner.X + tileSize, Corner.Y + 1);
                        tan1.b = new Vector2(Corner.X, Corner.Y + tileSize );
                        tan1.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                }



            Canvas[1] = Rectangle.Empty;
            tan2.a = Vector2.Zero;
            tan2.b = Vector2.Zero;
            tan2.c = Vector2.Zero;
            if (TileX + Ratio.X >= 0 && TileX + Ratio.X < map.GetLength(1) && TileY + (int)Ratio.Y >= 0 && TileY + (int)Ratio.Y < map.GetLength(0))
            {
                if (map[TileY + (int)Ratio.Y, TileX + (int)Ratio.X] == '☺')
                    Canvas[1] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + (int)Ratio.Y) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

                if (map[TileY + (int)Ratio.Y, TileX + (int)Ratio.X] == '♦')
                    Canvas[1] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + (int)Ratio.Y) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize/2);
                if (map[TileY + (int)Ratio.Y, TileX + (int)Ratio.X] == '☻') //left triangle
                {
                    Rectangle Corner = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + (int)Ratio.Y) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                    tan2.a = new Vector2(Corner.X, Corner.Y + 1);
                    tan2.b = new Vector2(Corner.X, Corner.Y + tileSize);
                    tan2.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                }
                if (map[TileY + (int)Ratio.Y, TileX + (int)Ratio.X] == '♥') //right triangle
                {
                    Rectangle Corner = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + (int)Ratio.Y) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                    tan2.a = new Vector2(Corner.X + tileSize, Corner.Y + 1);
                    tan2.b = new Vector2(Corner.X, Corner.Y + tileSize);
                    tan2.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                }
            }


            Canvas[2] = Rectangle.Empty;
            tan3.a = Vector2.Zero;
            tan3.b = Vector2.Zero;
            tan3.c = Vector2.Zero;
            for (int i = 0; i < Ratio.X; i++)
                if (TileX + i >= 0 && TileX + i < map.GetLength(1) && TileY >= 0 && TileY < map.GetLength(0))
                {
                    if (map[TileY, TileX + i] == '☺')
                        Canvas[2] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                    if (map[TileY, TileX + i] == '♦')
                        Canvas[2] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize/2);
                    if (map[TileY, TileX + i] == '☻') //left triangle
                    {
                        Rectangle Corner = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                        tan3.a = new Vector2(Corner.X, Corner.Y + 1);
                        tan3.b = new Vector2(Corner.X, Corner.Y + tileSize);
                        tan3.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                    if (map[TileY, TileX + i] == '♥') //right triangle
                    {
                        Rectangle Corner = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                        tan3.a = new Vector2(Corner.X + tileSize, Corner.Y + 1);
                        tan3.b = new Vector2(Corner.X, Corner.Y + tileSize);
                        tan3.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                }

            Canvas[3] = Rectangle.Empty;
            tan4.a = Vector2.Zero;
            tan4.b = Vector2.Zero;
            tan4.c = Vector2.Zero;
            if (TileX + Ratio.X >= 0 && TileX + Ratio.X < map.GetLength(1) && TileY >= 0 && TileY < map.GetLength(0))
            {
                if (map[TileY, TileX + (int)Ratio.X] == '☺')
                    Canvas[3] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                if (map[TileY, TileX + (int)Ratio.X] == '♦')
                    Canvas[3] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize/2);
                if (map[TileY, TileX + (int)Ratio.X] == '☻') //left triangle
                {
                    Rectangle Corner = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                    tan4.a = new Vector2(Corner.X, Corner.Y + 1);
                    tan4.b = new Vector2(Corner.X, Corner.Y + tileSize);
                    tan4.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                }
                if (map[TileY, TileX + (int)Ratio.X] == '♥') //right triangle
                {
                    Rectangle Corner = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                    tan4.a = new Vector2(Corner.X + tileSize, Corner.Y + 1);
                    tan4.b = new Vector2(Corner.X, Corner.Y + tileSize);
                    tan4.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                }
            }

            Canvas[4] = Rectangle.Empty;
            tan5.a = Vector2.Zero;
            tan5.b = Vector2.Zero;
            tan5.c = Vector2.Zero;
            for (int i = 0; i < Ratio.Y; i++)
                if (TileX >= 0 && TileX < map.GetLength(1) && TileY + i >= 0 && TileY + i < map.GetLength(0))
                {
                    if (map[TileY + i, TileX] == '☺')
                        Canvas[4] = new Rectangle(TileX * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

                    if (map[TileY + i, TileX] == '♦')
                        Canvas[4] = new Rectangle(TileX * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize/2);
                    if (map[TileY + i, TileX] == '☻') //left triangle
                    {
                        Rectangle Corner = new Rectangle(TileX * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                        tan5.a = new Vector2(Corner.X, Corner.Y + 1);
                        tan5.b = new Vector2(Corner.X, Corner.Y + tileSize);
                        tan5.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                    if (map[TileY + i, TileX] == '♥') //right triangle
                    {
                        Rectangle Corner = new Rectangle(TileX * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                        tan5.a = new Vector2(Corner.X + tileSize, Corner.Y + 1);
                        tan5.b = new Vector2(Corner.X, Corner.Y + tileSize);
                        tan5.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                }

            Canvas[5] = Rectangle.Empty;
            tan6.a = Vector2.Zero;
            tan6.b = Vector2.Zero;
            tan6.c = Vector2.Zero;
            for (int i = 0; i < Ratio.Y; i++)
                if (TileX + Ratio.X >= 0 && TileX + Ratio.X < map.GetLength(1) && TileY + i >= 0 && TileY + i < map.GetLength(0))
                {
                    if (map[TileY + i, TileX + (int)Ratio.X] == '☺')
                        Canvas[5] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

                    if (map[TileY + i, TileX + (int)Ratio.X] == '♦')
                        Canvas[5] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize/2);
                    if (map[TileY + i, TileX + (int)Ratio.X] == '☻') //left triangle
                    {
                        Rectangle Corner = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                        tan6.a = new Vector2(Corner.X, Corner.Y + 1);
                        tan6.b = new Vector2(Corner.X, Corner.Y + tileSize);
                        tan6.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                    if (map[TileY + i, TileX + (int)Ratio.X] == '♥') //right triangle
                    {
                        Rectangle Corner = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                        tan6.a = new Vector2(Corner.X + tileSize, Corner.Y + 1);
                        tan6.b = new Vector2(Corner.X, Corner.Y + tileSize );
                        tan6.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                }

        }

        public void Draw(SpriteBatch sB)
        {
            Rectangle tileDraw;
            background.Draw(sB);

          
            Textures.TextureType = Textures.ETextureType.INGAME;

            Textures.DrawBackgroundMapTextures(sB, BackMapTextures, tileSize, Vector2.Zero, game1);
            
            Textures.DrawTriggerMapData(sB, map, tileSize, Vector2.Zero, game1);

            foreach (Enemy enemy in enemies)
            {
                if (!enemy.isDead)
                    enemy.Draw(sB);
            }

            player.Draw(sB);

            //Hard Coded Texture Tiles// Water, Lava, Rain, Weather Effects
            for (int i = 0; i < map.GetLength(1); i++)
                for (int j = map.GetLength(0) - 1; j > -1; j--)
                {
                    tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (map.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize);

                    if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                        if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                        {
                            //Draw Water Top Tile
                            if (MapEffectTextures[j, i] == '♠')
                                WaterTop.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.DeepSkyBlue);
                            //Draw Water Base Tile
                            if (MapEffectTextures[j, i] == '•')
                                WaterBase.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.DeepSkyBlue);
                        }
                }

            Textures.DrawForegroundMapTextures(sB, ForeMapTextures, tileSize, Vector2.Zero, game1);
            
            Textures.DrawMapEffects(sB, MapEffectTextures, tileSize, Vector2.Zero, game1);

        }


        public void DrawLine(Game1 getGame, SpriteBatch sb, Vector2 firstPos, Vector2 lastPos)
        {

            Rectangle getDestination = new Rectangle(0, 0, Textures._DBG_Line_Tex.Width, Textures._DBG_Line_Tex.Height);
            float distancex = Math.Abs(lastPos.X - firstPos.X);
            float distancey = Math.Abs(lastPos.Y - firstPos.Y);
            float distance = (float)Math.Sqrt((distancex * distancex) + (distancey * distancey));
            Rectangle getScale = new Rectangle((int)firstPos.X, (int)firstPos.Y, 1, (int)distance);

            float deltaX = lastPos.X - firstPos.X;
            float deltaY = lastPos.Y - firstPos.Y;
            float angleInDeg = (float)Math.Atan2(deltaY, deltaX) * (180 / (float)Math.PI);

            float getRotation = MathHelper.ToRadians(angleInDeg - 90);
            Vector2 getOrigin = Vector2.Zero;
            sb.Draw(Textures._DBG_Line_Tex, getScale, getDestination, Color.White, getRotation, getOrigin, SpriteEffects.None, 0);

        }

    }


}

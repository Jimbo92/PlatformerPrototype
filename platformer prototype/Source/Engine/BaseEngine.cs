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
        public Vector2 PlayerStart;
        public int PlayerWarpInTime;
        public Sprite WaterTop;
        public Sprite WaterBase;
        public Sprite LavaTop;
        public Sprite LavaBase;
        public Sprite Torch;
        public Sprite WarpPad;
        public Rectangle tileDraw;

        private ContentManager Content;

        public List<Enemy> Enemies = new List<Enemy>();
        public List<Platform> Platforms = new List<Platform>();

        public Vector2 EnemySpawn;
       

        Random random = new Random();

        private Game1 game1;

        public int LoadMapTimer;
        public char[,] map = MapLoader.LoadMapData("grasslands");
        public char[,] BackMapTextures = MapLoader.LoadMapData("grasslands_back");
        public char[,] ForeMapTextures = MapLoader.LoadMapData("grasslands_fore");
        public char[,] MapEffectTextures = MapLoader.LoadMapData("grasslands_eff");

        public int tileSize = 32;

        //Items
        private Item Crystal;
        public Item WoodBox;

        //-----------------------------------------------------

        public BaseEngine(ContentManager getContent, Vector2 getScreenSize)
        {
            Content = getContent;
            for (int i = 0; i < map.GetLength(1); i++)
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, 600 - (tileSize * (map.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize);

                    //Player Start
                    if (map[j, i] == '○')
                        PlayerStart = new Vector2(tileDraw.X, tileDraw.Y);
                    //Enemy Spawn
                    if (map[j, i] == '♂')
                    {
                        Enemy BasicEnemy = new Enemy(getContent);
                        EnemySpawn = new Vector2(tileDraw.X, tileDraw.Y);
                        BasicEnemy.Position = EnemySpawn;
                        BasicEnemy.isDead = false;
                        BasicEnemy.runPlanes.X += BasicEnemy.Position.X;
                        BasicEnemy.runPlanes.Y += BasicEnemy.Position.X;
                        Enemies.Add(BasicEnemy);
                    }
                }


            Platform Hor = new Platform(getContent);
            Hor.Position = new Vector2(128, 256);
            Hor.Set(new Vector2(-64, 64), false);
            Hor.runPlanes.X += Hor.Position.X;
            Hor.runPlanes.Y += Hor.Position.X;
            Platforms.Add(Hor);


            player = new Player(getContent, PlayerStart);
            Camera.Initialize(player, this);
            WaterTop = new Sprite(getContent, "tiles/water0", 32, 32, 10, 1);
            WaterBase = new Sprite(getContent, "tiles/water1", 32, 32, 10, 1);
            LavaTop = new Sprite(getContent, "tiles/lava0", 32, 32, 10, 1);
            LavaBase = new Sprite(getContent, "tiles/lava1", 32, 32, 10, 1);
            Torch = new Sprite(getContent, "objects/torchss", 32, 32, 1, 8);
            WarpPad = new Sprite(getContent, "objects/warppadss", 48, 48, 1, 10);
            Crystal = new Item(Content, "objects/items/gemblue", 48, 48);
            WoodBox = new Item(Content, "objects/box", 32, 32);

            background = new Background(getContent, getScreenSize);
        }

        public void drawTriangle(SpriteBatch sB, Triangle target)
        {
            DrawLine(game1, sB, target.a, target.b);
            DrawLine(game1, sB, target.b, target.c);
            DrawLine(game1, sB, target.c, target.a);
        }

        private void PlayerWarpIn()
        {
            PlayerWarpInTime++;
            if (PlayerWarpInTime >= 50)
            {
                player.Update(game1, this);
                //PlayerData();
                PlayerWarpInTime = 50;
            }

            //Warp in Effect goes here//
        }

        public void Update(Game1 getGame1)
        {
            Textures.Update(this);
            LoadMapTimer++;
            if (LoadMapTimer <= 1)
            {
                map = MapLoader.LoadMapData("grasslands");
                BackMapTextures = MapLoader.LoadMapData("grasslands_Back");
                ForeMapTextures = MapLoader.LoadMapData("grasslands_fore");
                MapEffectTextures = MapLoader.LoadMapData("grasslands_eff");
            }
            else
                LoadMapTimer = 2;

            game1 = getGame1;
            Camera.Update(game1);
            Camera.CameraMode = Camera.CameraState.FOLLOW;
            //Animated Textures
            WaterTop.UpdateAnimation(0.15f);
            WaterBase.UpdateAnimation(0.15f);
            LavaTop.UpdateAnimation(0.15f);
            LavaBase.UpdateAnimation(0.15f);
            Torch.UpdateAnimation(0.5f);
            WarpPad.UpdateAnimation(0.3f);

            //Enemy Update
            foreach (Enemy e in Enemies)
            {
                if (!e.isDead)
                    e.Update(this);
            }
            for (int i = 0; i < Platforms.Count; i++)
                Platforms[i].Update(this);

            //Player Update                          
            PlayerWarpIn();
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
                        if (que < NoClip.GetLength(0) && map[TileY + j, TileX + i] == '•')
                        {


                            NoClip[que] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + j) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize) + 8, tileSize, tileSize - 8);
                            que++;

                        }
                        else
                        {
                            if (que < NoClip.GetLength(0) && map[TileY + j, TileX + i] == type)
                            {
                                NoClip[que] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + j) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                                que++;
                            }
                        }
                    if (TileY + j >= 0 && TileY + j < map.GetLength(0) && TileX + 1 >= 0 && TileX + 1 < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j, TileX + i + 1] == '•')
                        {


                            NoClip[que] = new Rectangle((TileX + i + 1) * tileSize + (int)Camera.Position.X, ((TileY + j) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize) + 8, tileSize, tileSize - 8);
                            que++;

                        }
                        else
                        {
                            if (que < NoClip.GetLength(0) && map[TileY + j, TileX + i + 1] == type)
                            {
                                NoClip[que] = new Rectangle((TileX + i + 1) * tileSize + (int)Camera.Position.X, ((TileY + j) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                                que++;
                            }
                        }

                    if (TileY + j + 1 >= 0 && TileY + j + 1 < map.GetLength(0) && TileX + i >= 0 && TileX + i < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j + 1, TileX + i] == '•')
                        {


                            NoClip[que] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + j + 1) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize) + 8, tileSize, tileSize - 8);
                            que++;

                        }
                        else
                        {
                            if (que < NoClip.GetLength(0) && map[TileY + j + 1, TileX + i] == type)
                            {
                                NoClip[que] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + j + 1) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                                que++;
                            }
                        }
                    if (TileY + j + 1 >= 0 && TileY + j + 1 < map.GetLength(0) && TileX + 1 >= 0 && TileX + 1 < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j + 1, TileX + i + 1] == '•')
                        {


                            NoClip[que] = new Rectangle((TileX + i + 1) * tileSize + (int)Camera.Position.X, ((TileY + j + 1) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize) + 8, tileSize, tileSize - 8);
                            que++;

                        }
                        else
                        {
                            if (que < NoClip.GetLength(0) && map[TileY + j + 1, TileX + i + 1] == type)
                            {
                                NoClip[que] = new Rectangle((TileX + i + 1) * tileSize + (int)Camera.Position.X, ((TileY + j + 1) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                                que++;
                            }
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

                    if (map[TileY + (int)Ratio.Y, TileX + i] == '☺' || map[TileY + (int)Ratio.Y, TileX + i] == '♀')
                    {
                        Canvas[0] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, ((TileY + (int)Ratio.Y) * tileSize) + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                    }

                    if (map[TileY + (int)Ratio.Y, TileX + i] == '♦')
                    {
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
                        tan1.b = new Vector2(Corner.X, Corner.Y + tileSize);
                        tan1.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                }



            Canvas[1] = Rectangle.Empty;
            tan2.a = Vector2.Zero;
            tan2.b = Vector2.Zero;
            tan2.c = Vector2.Zero;
            if (TileX + Ratio.X >= 0 && TileX + Ratio.X < map.GetLength(1) && TileY + (int)Ratio.Y >= 0 && TileY + (int)Ratio.Y < map.GetLength(0))
            {
                if (map[TileY + (int)Ratio.Y, TileX + (int)Ratio.X] == '☺' || map[TileY + (int)Ratio.Y, TileX + (int)Ratio.X] == '♀')
                    Canvas[1] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + (int)Ratio.Y) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

                if (map[TileY + (int)Ratio.Y, TileX + (int)Ratio.X] == '♦')
                    Canvas[1] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + (int)Ratio.Y) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize / 2);
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
                    if (map[TileY, TileX + i] == '☺' || map[TileY, TileX + i] == '♀')
                        Canvas[2] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                    if (map[TileY, TileX + i] == '♦')
                        Canvas[2] = new Rectangle((TileX + i) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize / 2);
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
                if (map[TileY, TileX + (int)Ratio.X] == '☺' || map[TileY, TileX + (int)Ratio.X] == '♀')
                    Canvas[3] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                if (map[TileY, TileX + (int)Ratio.X] == '♦')
                    Canvas[3] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize / 2);
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
                    if (map[TileY + i, TileX] == '☺' || map[TileY + i, TileX] == '♀')
                        Canvas[4] = new Rectangle(TileX * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

                    if (map[TileY + i, TileX] == '♦')
                        Canvas[4] = new Rectangle(TileX * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize / 2);
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
                    if (map[TileY + i, TileX + (int)Ratio.X] == '☺' || map[TileY + i, TileX + (int)Ratio.X] == '♀')
                        Canvas[5] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

                    if (map[TileY + i, TileX + (int)Ratio.X] == '♦')
                        Canvas[5] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)Camera.Position.X, (TileY + i) * tileSize + (int)Camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize / 2);
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
                        tan6.b = new Vector2(Corner.X, Corner.Y + tileSize);
                        tan6.c = new Vector2(Corner.X + tileSize, Corner.Y + tileSize);
                    }
                }

        }

        public void Draw(SpriteBatch sB)
        {
            background.Draw(sB);


            Textures.TextureType = Textures.ETextureType.INGAME;

            Textures.DrawBackgroundMapTextures(sB, BackMapTextures, tileSize, Vector2.Zero, game1);

            Textures.DrawTriggerMapData(sB, map, tileSize, Vector2.Zero, game1);

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (!Enemies[i].isDead)
                    if (Enemies[i].Position.X + Camera.Position.X > 0 - 24 + 0 && Enemies[i].Position.X + Camera.Position.X  < game1.GraphicsDevice.Viewport.Width)
                        if (Enemies[i].Position.Y + Camera.Position.Y > 0 - 24 + 0 && Enemies[i].Position.Y + Camera.Position.Y < game1.GraphicsDevice.Viewport.Height)
                    Enemies[i].Draw(sB);
            }
            

            for (int i = 0; i < Platforms.Count; i++)           
            {
                if (Platforms[i].Position.X + Camera.Position.X > 0 - 32 + 0 && Platforms[i].Position.X + Camera.Position.X < game1.GraphicsDevice.Viewport.Width)
                    if (Platforms[i].Position.Y + Camera.Position.Y > 0 - 32 + 0 && Platforms[i].Position.Y + Camera.Position.Y < game1.GraphicsDevice.Viewport.Height)
                    Platforms[i].Draw(sB);
            }



            //Hard Coded Texture Tiles// Water, Lava, Rain, Weather Effects
            Textures.DrawForegroundMapTextures(sB, ForeMapTextures, tileSize, Vector2.Zero, game1);

            if (PlayerWarpInTime == 50)
                player.Draw(sB);

            Textures.DrawMapEffects(sB, MapEffectTextures, tileSize, Vector2.Zero, game1);

            for (int i = 0; i < map.GetLength(1); i++)
                for (int j = map.GetLength(0) - 1; j > -1; j--)
                {
                    tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (map.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize);



                    //Draw Water Top Tile
                    if (MapEffectTextures[j, i] == '♠')
                        WaterTop.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.DeepSkyBlue);
                    //Draw Water Base Tile
                    if (MapEffectTextures[j, i] == '•')
                        WaterBase.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.DeepSkyBlue);
                    //Draw Lava Top Tile
                    if (MapEffectTextures[j, i] == '◘')
                        LavaTop.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                    //Draw Lava Base Tile
                    if (MapEffectTextures[j, i] == '○')
                        LavaBase.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);

                    //----------//Objects//---------//
                    //Crystal Item
                    if (map[j, i] == '◘')
                        Crystal.Draw(sB, this);
                    //Crystal Collision
                    if (player.Bounds.Intersects(Crystal.sprite.destinationRectangle))
                    {
                        if (map[j, i] == '◘')
                            map[j, i] = ' ';
                        Crystal.sprite.destinationRectangle = Rectangle.Empty;
                        GUI.CrystalPickUp = true;
                        GUI.ShowCrystalBar = true;
                        GUI.CrystalBarTimer = 0;
                    }

                    //Wood Box Crate
                    if (map[j, i] == '♀')
                        WoodBox.Draw(sB, this);
                    //Wood Box Crate Collision
                    if (player.Bounds.Intersects(WoodBox.sprite.CollisionBox))
                    {
                        if (player.Bounds.Y > WoodBox.Position.Y)
                        {
                            if (map[j, i] == '♀')
                                map[j, i] = ' ';
                            WoodBox.sprite.destinationRectangle = Rectangle.Empty;
                        }
                    }

                    //Draw Warp Pad
                    if (map[j, i] == '○')
                        WarpPad.Draw(sB, new Vector2(tileDraw.X - 8, tileDraw.Y - 10), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                }
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

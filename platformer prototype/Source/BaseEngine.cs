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

        public Rectangle[] NoClip = new Rectangle[6];

        public Rectangle Scale;
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
        public Camera camera;

        public int[,] mapFake = new int[,]
        {
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1},
        {1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,1,1,1,1,1,1},
        {1,0,0,1,1,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        public int[,] map = MapLoader.LoadMapData("testmap");
        public int[,] BackMapTextures = MapLoader.LoadBackgroundMapTextures("testmap_back");
        public int[,] ForeMapTextures = MapLoader.LoadForegroundMapTextures("testmap_fore");

        public int tileSize = 32;

        //-----------------------------------------------------

        public BaseEngine(ContentManager getContent, Vector2 getScreenSize)
        {
            player = new Player(getContent);

            WaterTop = new Sprite(getContent, "water1ss", 32, 32, 1, 3);
            WaterBase = new Sprite(getContent, "water2ss", 32, 32, 1, 3);
            Torch = new Sprite(getContent, "torchss", 32, 32, 1, 8);

            for (int i = 0; i < enemies.Length; i++)
            {
                int randomX = random.Next(100, 1200);
                enemies[i] = new Enemy(getContent);

                enemies[i].isDead = false;
                enemies[i].Position = new Vector2(randomX, 0);


            }

            camera = new Camera(player);
            background = new Background(getContent, getScreenSize);
        }

        public void Update(Game1 getGame1)
        {
            game1 = getGame1;
            //Animated Textures
            WaterTop.UpdateAnimation(0.08f);
            WaterBase.UpdateAnimation(0.08f);
            Torch.UpdateAnimation(0.5f);


            camera.Update(game1);
            player.Update(game1, this);
            foreach (Enemy e in enemies)
            {
                if (!e.isDead)
                    e.Update(game1, this);
            }

            background.Update(camera);

            // Check for Map boundries-----------------
            if (player.Position.Y > game1.GraphicsDevice.Viewport.Height)
            {
                player.Position = new Vector2(100, 50);
                camera.Position.X = 0;
                camera.Position.Y = 0;
            }

            // Check X Collisions-----------------------------
            //Will Check up to 6 Rectangles

            player.Position.X += (int)player.Speed.X;
            player.updateBounds(camera.Position);
            updateHitboxes(player.Position, player.Bounds);
            foreach (Rectangle canvas in Canvas)
                player.checkCollisionsX(canvas);


            // Check Y Collisions-----------------------------
            //Will Check up to 6 Rectangles

            player.Position.Y += (int)player.Speed.Y;
            player.updateBounds(camera.Position);
            updateHitboxes(player.Position, player.Bounds);
            foreach (Rectangle canvas in Canvas)
                player.checkCollisionsY(canvas);

            //Check Non Clipping Collisions====================

            player.checks[0] = false;
            player.updateBounds(camera.Position);
            updateNoclips(player.Position,player.Bounds, 2);
            foreach (Rectangle noclip in NoClip)
            {
                if(player.Bounds.Intersects(noclip))
                {
                    player.checks[0] = true;
                }
                
            }

            player.checks[1] = false;
            player.updateBounds(camera.Position);
            updateNoclips(player.Position, player.Bounds, 3);
            foreach (Rectangle noclip in NoClip)
            {
                if (player.Bounds.Intersects(noclip))
                {
                    player.checks[1] = true;
                }

            }

            //=================================================
            player.updateBounds(camera.Position);

            foreach (Enemy e in enemies)
            {
                // Check X Collisions-----------------------------
                //Will Check up to 6 Rectangles

                e.Position.X += (int)e.Speed.X;
                e.updateBounds(camera.Position);
                updateHitboxes(e.Position, e.Bounds);
                foreach (Rectangle canvas in Canvas)
                    e.checkCollisionsX(canvas);


                // Check Y Collisions-----------------------------
                //Will Check up to 6 Rectangles

                e.Position.Y += (int)e.Speed.Y;
                e.updateBounds(camera.Position);
                updateHitboxes(e.Position, e.Bounds);
                foreach (Rectangle canvas in Canvas)
                    e.checkCollisionsY(canvas);
            }

        }



        public void updateNoclips(Vector2 position,Rectangle bounds, int type)
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

                    if (TileY + j>= 0 && TileY + j< map.GetLength(0) && TileX + i >= 0 && TileX + i < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j, TileX + i] == type)
                        {
                            
                            
                                NoClip[que] = new Rectangle((TileX + i) * tileSize + (int)camera.Position.X, ((TileY + j) * tileSize) + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                                que++;
                            
                        }
                    if (TileY + j >= 0 && TileY + j< map.GetLength(0) && TileX  + 1>= 0 && TileX  + 1< map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j, TileX + i + 1] == type)
                        {
                            
                            
                                NoClip[que] = new Rectangle((TileX + i + 1) * tileSize + (int)camera.Position.X, ((TileY + j) * tileSize) + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                                que++;
                            
                        }

                    if (TileY + j + 1>= 0 && TileY + j + 1 < map.GetLength(0) && TileX + i >= 0 && TileX + i < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j + 1, TileX + i] == type)
                        {


                            NoClip[que] = new Rectangle((TileX + i) * tileSize + (int)camera.Position.X, ((TileY + j + 1) * tileSize) + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                            que++;

                        }
                    if (TileY + j + 1 >= 0 && TileY + j + 1< map.GetLength(0) && TileX + 1 >= 0 && TileX + 1 < map.GetLength(1))
                        if (que < NoClip.GetLength(0) && map[TileY + j + 1, TileX + i + 1] == type)
                        {


                            NoClip[que] = new Rectangle((TileX + i + 1) * tileSize + (int)camera.Position.X, ((TileY + j + 1) * tileSize) + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
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
            for (int i = 0; i < Ratio.X; i++)
                if (TileX + i >= 0 && TileX + i < map.GetLength(1) && TileY + Ratio.Y >= 0 && TileY + Ratio.Y < map.GetLength(0))

                    if (map[TileY + (int)Ratio.Y, TileX + i] == 1)
                        Canvas[0] = new Rectangle((TileX + i) * tileSize + (int)camera.Position.X, ((TileY + (int)Ratio.Y) * tileSize) + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);



            if (TileX + Ratio.X >= 0 && TileX + Ratio.X < map.GetLength(1) && TileY + (int)Ratio.Y >= 0 && TileY + (int)Ratio.Y < map.GetLength(0))
                if (map[TileY + (int)Ratio.Y, TileX + (int)Ratio.X] == 1)
                    Canvas[1] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)camera.Position.X, (TileY + (int)Ratio.Y) * tileSize + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);
                else
                    Canvas[1] = Rectangle.Empty;
            else
                Canvas[1] = Rectangle.Empty;

            Canvas[2] = Rectangle.Empty;
            for (int i = 0; i < Ratio.X; i++)
                if (TileX + i >= 0 && TileX + i < map.GetLength(1) && TileY >= 0 && TileY < map.GetLength(0))
                    if (map[TileY, TileX + i] == 1)
                        Canvas[2] = new Rectangle((TileX + i) * tileSize + (int)camera.Position.X, (TileY) * tileSize + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

            if (TileX + Ratio.X >= 0 && TileX + Ratio.X < map.GetLength(1) && TileY >= 0 && TileY < map.GetLength(0))
            {
                if (map[TileY, TileX + (int)Ratio.X] == 1)
                    Canvas[3] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)camera.Position.X, (TileY) * tileSize + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

                Canvas[3] = Rectangle.Empty;
            }
            else
                Canvas[3] = Rectangle.Empty;

            //-----------------------------------------------------------------------------------
            Canvas[4] = Rectangle.Empty;

            for (int i = 0; i < Ratio.Y; i++)
                if (TileX >= 0 && TileX < map.GetLength(1) && TileY + i >= 0 && TileY + i < map.GetLength(0))
                    if (map[TileY + i, TileX] == 1)
                        Canvas[4] = new Rectangle(TileX * tileSize + (int)camera.Position.X, (TileY + i) * tileSize + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

            Canvas[5] = Rectangle.Empty;
            for (int i = 0; i < Ratio.Y; i++)
                if (TileX + Ratio.X >= 0 && TileX + Ratio.X < map.GetLength(1) && TileY + i >= 0 && TileY + i < map.GetLength(0))
                    if (map[TileY + i, TileX + (int)Ratio.X] == 1)
                        Canvas[5] = new Rectangle((TileX + (int)Ratio.X) * tileSize + (int)camera.Position.X, (TileY + i) * tileSize + (int)camera.Position.Y - (int)leftovers - (difference * tileSize), tileSize, tileSize);

        }

        public void Draw(SpriteBatch sB)
        {
            background.Draw(sB);
            DrawBackgroundMapTextures(sB);
            //Always Draw Background First

            DrawTriggerMapData(sB);


            foreach (Enemy enemy in enemies)
            {
                if (!enemy.isDead)
                    enemy.Draw(sB);
            }

            player.Draw(sB);

            //Always Draw Foreground Last
            DrawForegroundMapTextures(sB);
        }

        //Draw Trigger Map Data
        public void DrawTriggerMapData(SpriteBatch sB)
        {
            Scale = new Rectangle(0, 0, tileSize, tileSize);
            int xLength = map.GetLength(1);
            int yLength = map.GetLength(0);
            Rectangle tileDraw;

            for (int i = 0; i < xLength; i++)
                for (int j = yLength - 1; j > -1; j--)
                {
                    tileDraw = new Rectangle((tileSize * i) + (int)camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (yLength - j)) + (int)camera.Position.Y, tileSize, tileSize);

                    if (map[j, i] == 1)
                    {
                        if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                            if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(Textures._DBG_DebugPlain_Tex, tileDraw, Scale, Color.White);
                    }
                    if (map[j, i] == 2)
                    {
                        if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                            if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(Textures._OBJ_Ladder_Tex, tileDraw, Scale, Color.White);
                    }

                    //Draw Torch Obj
                    if (ForeMapTextures[j, i] == 6)
                        if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                            if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                                Torch.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);

                }

            bool debug = false;
            if (debug == true)
                foreach (Rectangle Rect in Canvas)
                    sB.Draw(Textures._DBG_DebugPlain_Tex, Rect, Scale, Color.Red);
        }

        //Draw Background Map Textures
        public void DrawBackgroundMapTextures(SpriteBatch sB)
        {
            Rectangle RectTextureTile;

            for (int i = 0; i < BackMapTextures.GetLength(1); i++)
                for (int j = BackMapTextures.GetLength(0) - 1; j > -1; j--)
                {
                    RectTextureTile = new Rectangle((tileSize * i) + (int)camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (BackMapTextures.GetLength(0) - j)) + (int)camera.Position.Y, tileSize, tileSize);

                    //Draw Grass
                    if (BackMapTextures[j, i] == 1)
                        if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                            if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(Textures._TILE_Grass_Tex, RectTextureTile, Color.Gray);

                    //Draw Dirt
                    if (BackMapTextures[j, i] == 2)
                        if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                            if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(Textures._TILE_Dirt_Tex, RectTextureTile, Color.Gray);

                }
        }

        //Draw Foreground Map Textures
        public void DrawForegroundMapTextures(SpriteBatch sB)
        {
            Rectangle RectTextureTile;

            for (int i = 0; i < ForeMapTextures.GetLength(1); i++)
                for (int j = ForeMapTextures.GetLength(0) - 1; j > -1; j--)
                {
                    RectTextureTile = new Rectangle((tileSize * i) + (int)camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (ForeMapTextures.GetLength(0) - j)) + (int)camera.Position.Y, tileSize, tileSize);

                    //Draw Grass Tile
                    if (ForeMapTextures[j, i] == 1)
                        if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                            if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(Textures._TILE_Grass_Tex, RectTextureTile, Color.White);

                    //Draw Dirt Tile
                    if (ForeMapTextures[j, i] == 2)
                        if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                            if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(Textures._TILE_Dirt_Tex, RectTextureTile, Color.White);

                    //Draw Water Top Tile
                    if (ForeMapTextures[j, i] == 3)
                        if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                            if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                                WaterTop.Draw(sB, new Vector2(RectTextureTile.X, RectTextureTile.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);

                    //Draw Water Base Tile
                    if (ForeMapTextures[j, i] == 4)
                        if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                            if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                                WaterBase.Draw(sB, new Vector2(RectTextureTile.X, RectTextureTile.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);

                

                    //----------------------------------------------------//Objects//----------------------------------------------------//

                    //Draw Grass Obj
                    if (ForeMapTextures[j, i] == 5)
                        if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                            if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(Textures._OBJ_Grass_Tex, RectTextureTile, Color.White);
                }
        }
    }
}

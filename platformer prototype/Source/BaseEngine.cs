﻿using System;
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
        public Rectangle Scale;
        public int TileX;
        public int TileY;
        public Vector2 Ratio;
        public Background background;
        public Player player;

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
        public int[,] MapTextures = MapLoader.LoadMapTextures("testmap_texture");

        public int tileSize = 32;

        //-----------------------------------------------------

        public BaseEngine(ContentManager getContent, Vector2 getScreenSize)
        {
            player = new Player(getContent);



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

        public Vector2 getTile()
        {
            float leftovers = 0;
            if (game1.GraphicsDevice.Viewport.Height % tileSize != 0)
                leftovers = tileSize - (game1.GraphicsDevice.Viewport.Height % tileSize);


            int yLength = map.GetLength(0);
            int difference = yLength - (int)Math.Ceiling((float)game1.GraphicsDevice.Viewport.Height / tileSize);

            TileX = (int)Math.Floor((player.Position.X) / tileSize);
            TileY = ((int)Math.Floor((player.Position.Y + (difference * (tileSize)) + (leftovers)) / tileSize));
            return new Vector2(TileX, TileY);
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


            // original statement    Canvas[0] = new Rectangle(TileX * 100 + (int)camera.Position.X, ((TileY + 1) * 100) + (int)camera.Position.Y, 100, 100);



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
            Scale = new Rectangle(0, 0, tileSize, tileSize);
            int xLength = map.GetLength(1);
            int yLength = map.GetLength(0);
            Rectangle tileDraw;

            background.Draw(sB);

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
                }

            DrawMapTextures(sB);

            bool debug = false;
            if (debug == true)
                foreach (Rectangle Rect in Canvas)
                    sB.Draw(Textures._DBG_DebugPlain_Tex, Rect, Scale, Color.Red);


            foreach (Enemy enemy in enemies)
            {
                if (!enemy.isDead)
                    enemy.Draw(sB);
            }

            player.Draw(sB);
        }

        //Draw Map Textures
        public void DrawMapTextures(SpriteBatch sB)
        {
            Rectangle RectTextureTile;

            for (int i = 0; i < MapTextures.GetLength(1); i++)
                for (int j = MapTextures.GetLength(0) - 1; j > -1; j--)
                {
                    RectTextureTile = new Rectangle((tileSize * i) + (int)camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (MapTextures.GetLength(0) - j)) + (int)camera.Position.Y, tileSize, tileSize);

                    //Draw Grass
                    if (MapTextures[j, i] == 1)
                    if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                        if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                            sB.Draw(Textures._TILE_Grass_Tex, RectTextureTile, Color.White);

                    //Draw Dirt
                    if (MapTextures[j, i] == 2)
                        if (RectTextureTile.X > 0 - tileSize + 0 && RectTextureTile.X < game1.GraphicsDevice.Viewport.Width)
                            if (RectTextureTile.Y > 0 - tileSize + 0 && RectTextureTile.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(Textures._TILE_Dirt_Tex, RectTextureTile, Color.White);

                }
        }
    }
}

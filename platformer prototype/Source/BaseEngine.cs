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
        public Rectangle Scale;
        public int TileX;
        public int TileY;
        public Vector2 Ratio;
        public Background background;
        public Player player;


        private Game1 game1;
        private Camera camera;
        
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

        public int[,] map = MapLoader.load("testmap");

        public int tileSize = 32;

        //-----------------------------------------------------

        public BaseEngine(ContentManager getContent, Vector2 getScreenSize)
        {
            player = new Player(getContent);

            camera = new Camera(player);
            background = new Background(getContent, getScreenSize);
        }

        public void Update(Game1 getGame1)
        {
            game1 = getGame1;
            camera.Update(game1);
            player.Update(game1, this);
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
            updateHitboxes();
            foreach (Rectangle canvas in Canvas)
                player.checkCollisionsX(canvas);


            // Check Y Collisions-----------------------------
            //Will Check up to 6 Rectangles

            player.Position.Y += (int)player.Speed.Y;
            updateHitboxes();
            foreach (Rectangle canvas in Canvas)
                player.checkCollisionsY(canvas);

        }

        public void updateHitboxes()
        {
            float leftovers = 0;
            if (game1.GraphicsDevice.Viewport.Height % tileSize != 0)
                leftovers = tileSize - (game1.GraphicsDevice.Viewport.Height % tileSize);

            int yLength = map.GetLength(0);
            int difference = yLength - (int)Math.Ceiling((float)game1.GraphicsDevice.Viewport.Height / tileSize);

            TileX = (int)Math.Floor((player.Position.X) / tileSize);
            TileY = ((int)Math.Floor((player.Position.Y + (difference * (tileSize)) + (leftovers)) / tileSize));


            // original statement    Canvas[0] = new Rectangle(TileX * 100 + (int)camera.Position.X, ((TileY + 1) * 100) + (int)camera.Position.Y, 100, 100);

            player.Bounds = new Rectangle((int)player.Position.X + (int)camera.Position.X, (int)player.Position.Y + (int)camera.Position.Y, player.Width, player.Height);

            Ratio.X = (float)Math.Ceiling(player.Bounds.Width / (float)tileSize); //;;;;;;;;;;;;;;;;
            Ratio.Y = (float)Math.Ceiling(player.Bounds.Height / (float)tileSize); //;;;;;;;;;;;;;;;;;
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
            Scale = new Rectangle(0, 0, game1.levelTex.Width, game1.levelTex.Height);
            int xLength = map.GetLength(1);
            int yLength = map.GetLength(0);
            Rectangle tileDraw;

            background.Draw(sB);

            for (int i = 0; i < xLength; i++)
                for (int j = yLength - 1; j > -1; j--)
                    if (map[j, i] == 1)
                    {
                        tileDraw = new Rectangle((tileSize * i) + (int)camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (yLength - j)) + (int)camera.Position.Y, tileSize, tileSize);
                        if (tileDraw.X > 0 - tileSize + 0 && tileDraw.X < game1.GraphicsDevice.Viewport.Width)
                            if (tileDraw.Y > 0 - tileSize + 0 && tileDraw.Y < game1.GraphicsDevice.Viewport.Height)
                                sB.Draw(game1.levelTex, tileDraw, Scale, Color.White);
                    }


            bool debug = true;
            if (debug == true)
                foreach (Rectangle Rect in Canvas)
                    sB.Draw(game1.levelTex, Rect, Scale, Color.Red);

            player.Draw(sB);
        }
    }
}

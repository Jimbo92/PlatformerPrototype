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
        public Sprite WarpEffect;
        public bool isWarping;
        public Rectangle tileDraw;

        public GoalManager gm = new GoalManager();

        private Texture2D WarpFade_Tex;
        private float FadeValue;

        private float TextFadeValue;

        private ContentManager Content;

        public List<NPC> NPC_E = new List<NPC>(100);
        public List<NPC> NPC_F = new List<NPC>(100);
        public List<Platform> Platforms = new List<Platform>();
        public List<Lever> Switches = new List<Lever>();
        public List<Door> Doors = new List<Door>();

        public Vector2 NPCSpawn;
        //Enemy Types;
        NPC CrawlerEnemy;
        NPC WalkerEnemy;
        NPC FlyerEnemy;
        NPC FishEnemy;
        NPC FriendNPC;
        NPC SignNPC;

        public static int npc;


        Random random = new Random();

        private Game1 game1;

        public bool MapLoading = true;
        public int LoadMapTimer = 2;
        public char[,] map = MapLoader.LoadMapData("hubworld");
        public char[,] BackMapTextures = MapLoader.LoadMapData("hubworld_back");
        public char[,] ForeMapTextures = MapLoader.LoadMapData("hubworld_fore");
        public char[,] MapEffectTextures = MapLoader.LoadMapData("hubworld_eff");
        public Rectangle[] WarpDoors = new Rectangle[6];
        private int[] RequiredCrystals = new int[6];


        public int tileSize = 32;

        //Items
        public Item WoodBox;
        public Item Crystal;
        public Item Coin;

        private bool DrawDoors;



        //-----------------------------------------------------

        public BaseEngine(ContentManager getContent, Vector2 getScreenSize)
        {
            Content = getContent;

            player = new Player(getContent);
            Camera.Initialize(player, this);
            WaterTop = new Sprite(getContent, "tiles/water0", 32, 32, 10, 1);
            WaterBase = new Sprite(getContent, "tiles/water1", 32, 32, 10, 1);
            LavaTop = new Sprite(getContent, "tiles/lava0", 32, 32, 10, 1);
            LavaBase = new Sprite(getContent, "tiles/lava1", 32, 32, 10, 1);
            Torch = new Sprite(getContent, "objects/torchss", 32, 32, 1, 8);
            WarpPad = new Sprite(getContent, "objects/warppadss", 48, 48, 1, 10);
            WarpEffect = new Sprite(getContent, "objects/warpeffectss", 96, 96, 5, 6);
            WoodBox = new Item(Content, "objects/box", 32, 32, Item.EItemType.Breakable);
            Crystal = new Item(Content, "objects/items/gemblue", 32, 32, Item.EItemType.Crystal);
            Coin = new Item(Content, "objects/items/coingold", 32, 32, Item.EItemType.Coin);
            WarpFade_Tex = getContent.Load<Texture2D>("editor/black");

            background = new Background(getContent, getScreenSize);


            //Warp Door Data
            RequiredCrystals[0] = 1;
            RequiredCrystals[1] = 3;
        }

        public void drawTriangle(SpriteBatch sB, Triangle target)
        {
            DrawLine(game1, sB, target.a, target.b);
            DrawLine(game1, sB, target.b, target.c);
            DrawLine(game1, sB, target.c, target.a);
        }


        public void BGColours()
        {
            switch (Global_GameState.ZoneState)
            {
                case Global_GameState.EZoneState.Grasslands:
                    {
                        game1.BGColour = Color.DeepSkyBlue;
                        background.Background_Tex = Textures._BG_GrassLands_Tex;
                    }; break;
                case Global_GameState.EZoneState.Beach:
                    {
                        game1.BGColour = Color.LightSkyBlue;
                        background.Background_Tex = Textures._BG_Beach_Tex;
                    }; break;
                case Global_GameState.EZoneState.HubWorld:
                    {
                        game1.BGColour = Color.SkyBlue;
                        background.Background_Tex = Textures._BG_GrassLands_Tex;
                    }; break;
            }
        }

        private void HubWorldData()
        {
            //Grasslands
            WarpDoors[0] = new Rectangle(765 + (int)Camera.Position.X, 344 + (int)Camera.Position.Y, 32, 32);
            if (player.Bounds.Intersects(WarpDoors[0]))
            {             
                if (GUI.NumOfCrystals >= 1)
                    if (Input.KeyboardPressed(Keys.Enter))
                    {
                        WarpEffect.CurrentFrame = 1;
                        MapLoading = true;
                        ItemSave();
                        Global_GameState.ZoneState = Global_GameState.EZoneState.Grasslands;
                    }
            }
            //Beach
            WarpDoors[1] = new Rectangle(275 + (int)Camera.Position.X, 504 + (int)Camera.Position.Y, 32, 32);
            if (player.Bounds.Intersects(WarpDoors[1]))
            {                
                if (GUI.NumOfCrystals >= 3)
                    if (Input.KeyboardPressed(Keys.Enter))
                    {
                        WarpEffect.CurrentFrame = 1;
                        MapLoading = true;
                        ItemSave();
                        //0-0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
                        Global_GameState.ZoneState = Global_GameState.EZoneState.Beach;
                    }
            }
        }

        //Saves an ingame temp file
        private void ItemSave()
        {
            StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/maps/Temp/" + Global_GameState.ZoneState.ToString() + "_Temp.txt");
            for (int i = 0; i < map.GetLength(0); i++)
            {
                char[] linelist = new char[map.GetLength(1)];
                for (int j = 0; j < linelist.GetLength(0); j++)
                {
                    linelist[j] = map[i, j];
                    char line = linelist[j];
                    sw.Write(line);
                    if (j != linelist.GetLength(0) - 1)
                        sw.Write(",");
                }
                sw.WriteLine();
            }
            sw.Close();
        }

        private void LoadupMapEntities()
        {
            int l = 0;
            int s = 0;
            int e = 0;
            for (int i = 0; i < map.GetLength(1); i++)
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, 600 - (tileSize * (map.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize);
                    
                    //Player Start
                    if (map[j, i] == '○')
                    {
                        PlayerStart = new Vector2(tileDraw.X, tileDraw.Y);
                    }

                    //Enemy Spawns//
                    //Crawler
                    if (map[j, i] == '☼')
                    {
                        e++;
                        CrawlerEnemy = new NPC(Content, "objects/enemies/snailss", NPC.npcType.CRAWLER, 24, 16,e);
                        NPCSpawn = new Vector2(tileDraw.X, tileDraw.Y);
                        CrawlerEnemy.Position = NPCSpawn;
                        CrawlerEnemy.isDead = false;
                        CrawlerEnemy.runPlanes.X += CrawlerEnemy.Position.X;
                        CrawlerEnemy.runPlanes.Y += CrawlerEnemy.Position.X;
                        NPC_E.Add(CrawlerEnemy);
                    }
                    //Walker
                    if (map[j, i] == '►')
                    {
                        e++;
                        WalkerEnemy = new NPC(Content, "objects/enemies/slimess", NPC.npcType.WALKER, 46, 24,e);
                        NPCSpawn = new Vector2(tileDraw.X, tileDraw.Y);
                        WalkerEnemy.Position = NPCSpawn;
                        WalkerEnemy.isDead = false;
                        WalkerEnemy.runPlanes.X += WalkerEnemy.Position.X;
                        WalkerEnemy.runPlanes.Y += WalkerEnemy.Position.X;
                        NPC_E.Add(WalkerEnemy);
                        
                        //Slime Random Colours
                        int RandColourValue = random.Next(4);
                        switch (RandColourValue)
                        {
                            case 0: WalkerEnemy.colour = Color.LightYellow; break;
                            case 1: WalkerEnemy.colour = Color.Pink; break;
                            case 2: WalkerEnemy.colour = Color.LightBlue; break;
                            case 3: WalkerEnemy.colour = Color.LightGreen; break;
                        }
                    }
                    //Flyer
                    if (map[j, i] == '◄')
                    {
                        e++;
                        FlyerEnemy = new NPC(Content, "objects/enemies/flyss", NPC.npcType.FLYER, 24, 16,e);
                        NPCSpawn = new Vector2(tileDraw.X, tileDraw.Y);
                        FlyerEnemy.Position = NPCSpawn;
                        FlyerEnemy.isDead = false;
                        FlyerEnemy.runPlanes.X += FlyerEnemy.Position.X;
                        FlyerEnemy.runPlanes.Y += FlyerEnemy.Position.X;
                        NPC_E.Add(FlyerEnemy);
                    }
                    //Fish
                    if (map[j, i] == '§')
                    {
                        e++;
                        FishEnemy = new NPC(Content, "objects/enemies/fishss", NPC.npcType.FISH, 24, 16, e);
                        NPCSpawn = new Vector2(tileDraw.X, tileDraw.Y);
                        FishEnemy.Position = NPCSpawn;
                        FishEnemy.returnTo = (int)NPCSpawn.Y;
                        FishEnemy.isDead = false;
                        FishEnemy.runPlanes.X += FishEnemy.Position.X;
                        FishEnemy.runPlanes.Y += FishEnemy.Position.X;
                        NPC_E.Add(FishEnemy);
                        map[j, i] = '♣';
                        MapEffectTextures[j, i] = '•';
                    }
                    //Friendly
                    if (map[j, i] == '♂')
                    {
                        npc++;
                        FriendNPC = new NPC(Content, null, NPC.npcType.FRIENDLY, 28, 30, npc);
                        NPCSpawn = new Vector2(tileDraw.X, tileDraw.Y);
                        FriendNPC.Position = NPCSpawn;
                        FriendNPC.isDead = false;
                        FriendNPC.runPlanes.X += FriendNPC.Position.X;
                        FriendNPC.runPlanes.Y += FriendNPC.Position.X;
                        NPC_F.Add(FriendNPC);

                        //Friendly Random Colours
                        int RandColourValue = random.Next(4);
                        switch (RandColourValue)
                        {
                            case 0: FriendNPC.colour = Color.LightYellow; break;
                            case 1: FriendNPC.colour = Color.Pink; break;
                            case 2: FriendNPC.colour = Color.LightBlue; break;
                            case 3: FriendNPC.colour = Color.LightGreen; break;
                        }
                    }
                    //Sign Text
                    if (map[j, i] == '↕')
                    {
                        npc++;
                        SignNPC = new NPC(Content, null, NPC.npcType.SIGN, 32, 32, npc);
                        NPCSpawn = new Vector2(tileDraw.X, tileDraw.Y);
                        SignNPC.Position = NPCSpawn;
                        SignNPC.isDead = false;
                        NPC_F.Add(SignNPC);
                    }

                    //Platform hor Start
                    if (map[j, i] == '♪')
                    {
                        Platform Hor = new Platform(Content);
                        Hor.Position = new Vector2(tileDraw.X, tileDraw.Y);
                        Hor.Set(new Vector2(-64, 64), false, true);
                        Hor.runPlanes.X += Hor.Position.X;
                        Hor.runPlanes.Y += Hor.Position.X;
                        Platforms.Add(Hor);
                    }

                    //Platform ver Start
                    if (map[j, i] == '♫')
                    {
                        Platform Ver = new Platform(Content);
                        Ver.Position = new Vector2(tileDraw.X, tileDraw.Y);
                        Ver.Set(new Vector2(-50, 50), false, false);
                        Ver.runPlanes.X += Ver.Position.Y;
                        Ver.runPlanes.Y += Ver.Position.Y;
                        Platforms.Add(Ver);
                    }

                    //Switch Spawn
                    if (map[j, i] == '‼')
                    {
                        s++;
                        Lever Switch = new Lever();
                        Switch.Rect = tileDraw;
                        Switch.id = s; 
                        Switches.Add(Switch);
                       
                    }
                    //Lock Spawner
                    if (map[j, i] == '¶')
                    {
                        l++;
                        Door Lock = new Door();
                        Lock.rect = tileDraw;
                        Lock.id = gm.bind[l - 1]; 
                        Doors.Add(Lock);

                    }
                   
                }
        }

        private void UnloadEntities()
        {
            NPC_E.Clear();
            NPC_F.Clear();
            Platforms.Clear();
            Switches.Clear();
            Doors.Clear();
        }

        private void ZoneSorter()
        {
            //Load Level Data
            if (!File.Exists(Directory.GetCurrentDirectory() + "/maps/Temp/" + Global_GameState.ZoneState.ToString() + "_Temp.txt"))
                map = MapLoader.LoadMapData(Global_GameState.ZoneState.ToString());
            else
                map = MapLoader.LoadMapData("Temp/" + Global_GameState.ZoneState.ToString() + "_Temp");

            BackMapTextures = MapLoader.LoadMapData(Global_GameState.ZoneState.ToString() + "_back");
            ForeMapTextures = MapLoader.LoadMapData(Global_GameState.ZoneState.ToString() + "_fore");
            MapEffectTextures = MapLoader.LoadMapData(Global_GameState.ZoneState.ToString() + "_eff");
            //--------------------//
        }

        private void PlayerWarpIn()
        {
            isWarping = true;
            PlayerWarpInTime++;
            if (PlayerWarpInTime >= 50)
            {
                player.Update(game1, this);
                PlayerWarpInTime = 50;
            }
        }

        public void Update(Game1 getGame1)
        {
            game1 = getGame1;

            Textures.Update(this);

            //Warp to Hub World
            for (int i = 0; i < map.GetLength(1); i++)
                for (int j = map.GetLength(0) - 1; j > -1; j--)
                {
                    tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, 600 - (tileSize * (map.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize);

                    if (map[j, i] == '○')
                    {

                        if (Global_GameState.ZoneState != Global_GameState.EZoneState.HubWorld)
                            if (player.Bounds.Intersects(tileDraw))
                                if (Input.KeyboardPressed(Keys.Enter))
                                {
                                    WarpEffect.CurrentFrame = 1;
                                    MapLoading = true;
                                    ItemSave();
                                    Global_GameState.ZoneState = Global_GameState.EZoneState.HubWorld;
                                }
                    }
                }

            if (MapLoading)
            {
                //Fade
                FadeValue += 0.025f;
                if (FadeValue >= 1f)
                    FadeValue = 1;

                if (FadeValue == 1)
                {
                    TextFadeValue = 1;

                    UnloadEntities();

                    gm.Load();
                    ZoneSorter();

                    Camera.Position = Vector2.Zero;

                    npc = -1;
                    BGColours();

                    if (Global_GameState.ZoneState == Global_GameState.EZoneState.HubWorld)
                        DrawDoors = true;
                    else
                        DrawDoors = false;

                    LoadupMapEntities();
                    PlayerWarpInTime = 0;
                    player.Position = PlayerStart;

                    MapLoading = false;
                }

                if (FadeValue > 0)
                player.ControlsEnabled = false;
            }
            else
            {
                if (FadeValue > 0f)
                    FadeValue -= 0.025f;

                if (FadeValue <= 0)
                    player.ControlsEnabled = true;

                TextFadeValue -= 0.008f;
                if (TextFadeValue <= 0)
                    TextFadeValue = 0;
            }

            game1 = getGame1;
            Camera.Update(game1);
            if (!Camera.isControlled)
            {
                Camera.CameraMode = Camera.CameraState.FOLLOW;
            }
            //Animated Textures
            WaterTop.UpdateAnimation(0.15f);
            WaterBase.UpdateAnimation(0.15f);
            LavaTop.UpdateAnimation(0.15f);
            LavaBase.UpdateAnimation(0.15f);
            Torch.UpdateAnimation(0.5f);
            WarpPad.UpdateAnimation(0.3f);
            //Item Updates
            Coin.Update();
            Crystal.Update();

            //Enemy Update
            foreach (NPC e in NPC_E)
                if (!e.isDead)
                    e.Update(this,gm);

            //Friendly NPC's Update
            foreach (NPC f in NPC_F)
                if (!f.isDead)
                    f.Update(this,gm);

            for (int i = 0; i < Platforms.Count; i++)
                Platforms[i].Update(this);

            //Player Update 
            PlayerWarpIn();

            gm.Get(this);
            gm.Update();

            //HubWorld Data
            if (DrawDoors)
            HubWorldData();

            //Update Switches
            foreach (Lever lever in Switches)
            {
                lever.Update(this);
            }

            //Update Doors
            foreach (Door d in Doors)
            {
                d.Update(this);
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

            for (int i = 0; i < Platforms.Count; i++)
            {
                if (Platforms[i].Position.X + Camera.Position.X > 0 - 32 + 0 && Platforms[i].Position.X + Camera.Position.X < game1.GraphicsDevice.Viewport.Width)
                    if (Platforms[i].Position.Y + Camera.Position.Y > 0 - 32 + 0 && Platforms[i].Position.Y + Camera.Position.Y < game1.GraphicsDevice.Viewport.Height)
                        Platforms[i].Draw(sB);
            }

            //Draw Switches
            foreach (Lever lever in Switches)
            {
                lever.Draw(sB);

                if (player.Bounds.Intersects(new Rectangle(lever.Rect.X + (int)Camera.Position.X, lever.Rect.Y + (int)Camera.Position.Y, 32, 32)))
                {
                    if (Input.KeyboardPressed(Keys.Enter))
                    lever.isOn = true;
                }
            }

            foreach (Door d in Doors)
            {
                d.Draw(sB);
            }

            //Draw Doorways
            if (DrawDoors)
            {
                foreach (Rectangle rect in WarpDoors)
                {
                    sB.Draw(Textures._OBJ_Door_Tex[0], rect, Color.White);
                    sB.Draw(Textures._OBJ_Door_Tex[1], new Rectangle(rect.X, rect.Y - 32, rect.Width, rect.Height), Color.White);
                }

                Color TextColour;

                for (int i = 0; i < 6; i++)
                {
                    if (GUI.NumOfCrystals >= RequiredCrystals[i])
                        TextColour = Color.Gold;
                    else
                        TextColour = Color.DarkRed;

                    sB.DrawString(game1.font, RequiredCrystals[i].ToString(), new Vector2(WarpDoors[i].X + 24, WarpDoors[i].Y + 5), TextColour, 0, game1.font.MeasureString(RequiredCrystals[i].ToString()), .75f, SpriteEffects.None, 0);
                }
            }

            //Hard Coded Texture Tiles// Water, Lava, Rain, Weather Effects

            if (PlayerWarpInTime == 50)
                player.Draw(sB);

            Textures.DrawMapEffects(sB, MapEffectTextures, tileSize, Vector2.Zero, game1);

            for (int i = 0; i < map.GetLength(1); i++)
                for (int j = map.GetLength(0) - 1; j > -1; j--)
                {
                    tileDraw = new Rectangle((tileSize * i) + (int)Camera.Position.X, game1.GraphicsDevice.Viewport.Height - (tileSize * (map.GetLength(0) - j)) + (int)Camera.Position.Y, tileSize, tileSize);

                    //Draw Water Top Tile
                    if (MapEffectTextures[j, i] == '♠')
                        WaterTop.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.DeepSkyBlue * 0.8f);
                    //Draw Water Base Tile
                    if (MapEffectTextures[j, i] == '•')
                        WaterBase.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.DeepSkyBlue * 0.8f);
                    //Draw Lava Top Tile
                    if (MapEffectTextures[j, i] == '◘')
                        LavaTop.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                    //Draw Lava Base Tile
                    if (MapEffectTextures[j, i] == '○')
                        LavaBase.Draw(sB, new Vector2(tileDraw.X, tileDraw.Y), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);

                    //----------//Objects//---------//

                    bool isZoned = false;
                    for (int x = 0; x < gm.Zone.Count; x++)
                    {
                        Rectangle zBounds = new Rectangle(gm.Zone[x].X + (int)Camera.Position.X, gm.Zone[x].Y + (int)Camera.Position.Y, gm.Zone[x].Width, gm.Zone[x].Height);
                        if (tileDraw.Intersects(zBounds))
                        {
                            isZoned = true;
                        }
                    }

                    //Items                    

                    //Coin Item
                    if (map[j, i] == '▬' && !isZoned)
                        Coin.Draw(sB, this);
                    //Coin Collision
                    if (player.Bounds.Intersects(Coin.sprite.destinationRectangle) && !isZoned)
                    {
                        if (map[j, i] == '▬')
                            map[j, i] = ' ';
                        Coin.sprite.destinationRectangle = Rectangle.Empty;
                        GUI.CoinPickUp = true;
                        GUI.ShowCoinBar = true;
                        GUI.CoinBarTimer = 0;
                    }

                    //Crystal Item
                    if (map[j, i] == '◘' && !isZoned)
                        Crystal.Draw(sB, this);
                    //Crystal Collision
                    if (player.Bounds.Intersects(Crystal.sprite.destinationRectangle) && !isZoned)
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
                        if (player.Bounds.Y - 32 > WoodBox.Position.Y)
                        {
                            Random Rand = new Random();
                            int RandValue = Rand.Next(3);

                            if (map[j, i] == '♀')
                            {
                                if (RandValue == 1)
                                map[j, i] = '▬';
                                else
                                map[j, i] = ' ';
                            }
                            WoodBox.sprite.CollisionBox = Rectangle.Empty;
                            player.Speed.Y = 0;
                        }
                    }

                    //Draw Warp Pad
                    if (map[j, i] == '○')
                    {
                        WarpPad.Draw(sB, new Vector2(tileDraw.X - 8, tileDraw.Y - 10), new Vector2(0, 0), 0, SpriteEffects.None, Color.White);
                    }
                }

            Textures.DrawForegroundMapTextures(sB, ForeMapTextures, tileSize, Vector2.Zero, game1);

            //Warp Effect
            if (isWarping)
            {
                if (WarpEffect.CurrentFrame > 0)
                    WarpEffect.UpdateAnimation(0.8f);
                else if (WarpEffect.CurrentFrame > 20)
                    WarpEffect.CurrentFrame = 0;

                WarpEffect.Draw(sB, new Vector2(player.Bounds.X + 15, player.Bounds.Y - 5), 0, SpriteEffects.None);
            }

            //Enemies
            for (int i = 0; i < NPC_E.Count; i++)
            {
                if (!NPC_E[i].isDead)
                    if (NPC_E[i].Position.X + Camera.Position.X > 0 - 24 + 0 && NPC_E[i].Position.X + Camera.Position.X < game1.GraphicsDevice.Viewport.Width)
                        if (NPC_E[i].Position.Y + Camera.Position.Y > 0 - 24 + 0 && NPC_E[i].Position.Y + Camera.Position.Y < game1.GraphicsDevice.Viewport.Height)
                            NPC_E[i].Draw(sB);
            }

            //Friendly NPC's
            for (int i = 0; i < NPC_F.Count; i++)
            {
                if (!NPC_F[i].isDead)
                    if (NPC_F[i].Position.X + Camera.Position.X > 0 - 24 + 0 && NPC_F[i].Position.X + Camera.Position.X < game1.GraphicsDevice.Viewport.Width)
                        if (NPC_F[i].Position.Y + Camera.Position.Y > 0 - 24 + 0 && NPC_F[i].Position.Y + Camera.Position.Y < game1.GraphicsDevice.Viewport.Height)
                            NPC_F[i].Draw(sB);
            }

            for(int i = 0; i < gm.Zone.Count; i++)
            {
                Rectangle zBounds = new Rectangle(gm.Zone[i].X + (int)Camera.Position.X, gm.Zone[i].Y + (int)Camera.Position.Y, gm.Zone[i].Width, gm.Zone[i].Height);
                //sB.Draw(Textures._TILE_Zone_Tex, zBounds, Color.White);
            }

            //Fade Effect
            sB.Draw(WarpFade_Tex, new Rectangle(0, 0, 800, 600), Color.White * FadeValue);
            //Level Title
            sB.DrawString(game1.font, Global_GameState.ZoneState.ToString(), new Vector2(400, 100), Color.Silver * TextFadeValue, 0, game1.font.MeasureString(Global_GameState.ZoneState.ToString()) / 2, 2, SpriteEffects.None, 0);
        
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

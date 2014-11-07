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
    class GoalManager
    {
        public List<Rectangle> Zone = new List<Rectangle>();
        public List<string> Speech = new List<string>();
        public int[] bind = new int[20];

        public Player player = null;

        public void Get(Player Gplayer)
        {
            Gplayer = player;
        }

        public void Load()
        {
            Zone.Clear();
            Speech.Clear();
   
            for (int i = 0; i < bind.GetLength(0); i++)
            {
                bind[i] = 0;
            }

            if (Global_GameState.ZoneState == Global_GameState.EZoneState.Beach)
            {
                Zone.Add(new Rectangle(2816, 316, 32, 32));
                Speech.Add("Cloney Cliff \nHome of the floating gem");
                Speech.Add("Arrrrrrr! Use this\nto get back up the Cloney!");
                Speech.Add("Arrrrrrr! give me your\nhat ,maggot");

                bind[0] = 1;
                bind[1] = 2;
                bind[2] = 2;
                bind[3] = 1;
                              
            }

            if (Global_GameState.ZoneState == Global_GameState.EZoneState.Castle)
            {

            }

            if (Global_GameState.ZoneState == Global_GameState.EZoneState.Grasslands)
            {
            }

            if (Global_GameState.ZoneState == Global_GameState.EZoneState.HubWorld)
            {
                Zone.Add(new Rectangle(1105, 382, 25, 25));
                Zone.Add(new Rectangle(2756, 39, 120, 52));
                Speech.Add("Welcome to the Hub!");
                Speech.Add("Hi there!\nI love your hat!");
                Speech.Add("Lol give me your\nhat faggot");
                Speech.Add("I bet that you cannot\n get up there!!");
            }

            if (Global_GameState.ZoneState == Global_GameState.EZoneState.LavaLand)
            {

            }

            if (Global_GameState.ZoneState == Global_GameState.EZoneState.Mines)
            {

            }

            if (Global_GameState.ZoneState == Global_GameState.EZoneState.SnowyMountains)
            {

            }

        }

        public void HubGoal(int i)
        {
            if (i == 0)
            {
                Zone[i] = Rectangle.Empty;

                List<Vector4> Desired = new List<Vector4>();
                Desired.Add(new Vector4(387, 0, 60, 120));
                //Desired[0].X = 0; //X coord
                //Desired[0].Y = 0; // Y coord
                //Desired[0].Z = 120; //Delay to objective
                //Desired[0].W = 0; //Delay to start next objective

                Desired.Add(new Vector4(3168, 0, 120, 120));

                Desired.Add(new Vector4(4736, 0, 60, 120));

                Camera.isControlled = true;
                Camera.delay = 0;
                Camera.nextDelay = 0;
                Camera.task = -1;

                Camera.Flybuy(Desired);

                Speech[0] = "Enjoyed exploring?? why not\ntry and defeat one of\nthe many worlds!!";
            }
            if (i == 1)
            {
                Zone[i] = Rectangle.Empty;
                Speech[1] = "Holy cow you did it!\n you deserve some sorta reward";
             
            }
        }




        public void GrassLandsGoal(int i)
        {
        }


        public void BeachGoal(int i) 
        {
            if (i == 0) {
                player.Speed.Y = -20;
                player.Speed.X = -20;
                Speech[1] = "Man, you flew like thee\nEagle. How bout some scrumpy?";
            }
        }
   


        public void Update(Player Gplayer)
        {
            player = Gplayer;

            for (int i = 0; i < Zone.Count; i++ )
            {
                Rectangle zBounds = new Rectangle(Zone[i].X + (int)Camera.Position.X, Zone[i].Y + (int)Camera.Position.Y, Zone[i].Width, Zone[i].Height);
                if (zBounds.Intersects(player.Bounds))
                {
                    switch (Global_GameState.ZoneState)
                    {
                        case Global_GameState.EZoneState.HubWorld: HubGoal(i); break;
                        case Global_GameState.EZoneState.Grasslands: GrassLandsGoal(i); break;
                        case Global_GameState.EZoneState.Beach: BeachGoal(i); break;
                    }
                }
            }

            
        }

    }
}

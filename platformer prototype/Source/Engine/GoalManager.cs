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

        public BaseEngine engine = null;

        public void Get(BaseEngine Ge)
        {
            engine = Ge;
        }

        public void Load()
        {
            Zone.Clear();
            Speech.Clear();
   
            for (int i = 0; i < bind.GetLength(0); i++)
            {
                bind[i] = 0;
            }

            if (Global_GameState.ZoneState == Global_GameState.EZoneState.Challenge)
            {
                //383 476 and 639 476
                Speech.Add("Climb the ladder for a reward!");
                Speech.Add("The passcode is 1 0 1");
                Speech.Add("Help! some pesky flies\n have invaded my farm!");
                Zone.Add(new Rectangle(478, 214, 32, 32));
                Zone.Add(new Rectangle(383, 476, 32, 32));
                Zone.Add(new Rectangle(639, 476, 32, 32));
                Zone.Add(new Rectangle(255, 471, 32, 32));
            }


            if (Global_GameState.ZoneState == Global_GameState.EZoneState.Beach)
            {
                Zone.Add(new Rectangle(2816, 316, 32, 32));
                Speech.Add("Cloney Cliff \nHome of the floating gem");
                Speech.Add("Arrrrrrr! Use this\nto get back up the Cloney!");
                Speech.Add("Arrrrrrr! give me your\nhat ,maggot");

                bind[0] = 2;
                bind[1] = 1;
                bind[2] = 1;
                bind[3] = 2;
                              
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


        public void ChaCheck()
        {
            
            int three = 0;
            int three2 = 0;
            foreach (NPC e in engine.NPC_E)
            {
                if (e.enemyID == 3 && e.isDead)
                {
                    three2++;
                }
                if (e.enemyID == 4 && e.isDead)
                {
                    three2++;
                }
                if (e.enemyID == 5 && e.isDead)
                {
                    three2++;
                }
            }
            foreach (Lever l in engine.Switches)
            {
                if (l.id == 1 && l.isOn)
                {
                    three ++;
                }
                if (l.id == 2 && !l.isOn)
                {
                    three++;
                }
                if (l.id == 3 && l.isOn)
                {
                    three++;
                }


            }

            if (three == 3 && !Zone[2].IsEmpty)
            {
                Zone[2] = Rectangle.Empty;
            }
            if (three2 == 3 && !Zone[3].IsEmpty)
            {
                Zone[3] = Rectangle.Empty;
                Speech[2] = "Wow your Amazing!!\n Thanks!!";

                List<Vector4> Desired = new List<Vector4>();
                Desired.Add(new Vector4(0, 0, 60, 120));

                Camera.isControlled = true;
                Camera.delay = 0;
                Camera.nextDelay = 0;
                Camera.task = -1;

                Camera.Flybuy(Desired);
            }
        }
        
        public void ChaGoal(int i)
        {
            if (i == 0)
            {
                Zone[i] = Rectangle.Empty;
                Zone[1] = Rectangle.Empty;
            }

         
        }

        public void HubCheck()      
        {

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



        public void GrassLandsCheck()
        {

        }
        public void GrassLandsGoal(int i)
        {
        }

        public void BeachCheck()
        {

        }
        public void BeachGoal(int i) 
        {
            if (i == 0) {
                engine.player.Speed.Y = -20;
                engine.player.Speed.X = -20;
                Speech[1] = "Man, you flew like thee\nEagle. How bout some scrumpy?";
            }
        }
   


        public void Update()
        {
            

            for (int i = 0; i < Zone.Count; i++ )
            {
                Rectangle zBounds = new Rectangle(Zone[i].X + (int)Camera.Position.X, Zone[i].Y + (int)Camera.Position.Y, Zone[i].Width, Zone[i].Height);
                if (zBounds.Intersects(engine.player.Bounds))
                {
                    switch (Global_GameState.ZoneState)
                    {
                        case Global_GameState.EZoneState.Challenge: ChaGoal(i); break;
                        case Global_GameState.EZoneState.HubWorld: HubGoal(i); break;
                        case Global_GameState.EZoneState.Grasslands: GrassLandsGoal(i); break;
                        case Global_GameState.EZoneState.Beach: BeachGoal(i); break;
                    }
                }
            }
            switch (Global_GameState.ZoneState)
            {
                case Global_GameState.EZoneState.Challenge: ChaCheck(); break;
                case Global_GameState.EZoneState.HubWorld: HubCheck(); break;
                case Global_GameState.EZoneState.Grasslands: GrassLandsCheck(); break;
                case Global_GameState.EZoneState.Beach: BeachCheck(); break;
            }
            
        }

    }
}

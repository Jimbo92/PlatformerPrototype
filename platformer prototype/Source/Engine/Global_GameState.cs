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
    static class Global_GameState
    {
        public enum EGameState
        {
            MENU,
            PLAY,
            EDITOR,
        };

        static public EGameState GameState = EGameState.MENU;

        public enum EZoneState
        {
            HubWorld,
            Grasslands,
            Beach,
            Mines,
            SnowyMountains,
            Castle,
            LavaLand,
            Challenge,
        };

        static public EZoneState ZoneState = EZoneState.HubWorld;
    }
}

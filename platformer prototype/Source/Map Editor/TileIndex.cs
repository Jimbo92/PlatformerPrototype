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
    static class TileIndex
    {
        static public string Index(char CurrentTile, Editor getEditor)
        {
            if (getEditor.MapLayers == Editor.EMapLayers.TRIGGER)
            {
                if (CurrentTile == '`') return "Solid CB";
                if (CurrentTile == '0') return "Left Hill CB";
                if (CurrentTile == '1') return "Right Hill CB";
                if (CurrentTile == '2') return "Half CB";
                if (CurrentTile == '3') return "Water Trigger";
                if (CurrentTile == '4') return "Ladder Trigger";
            }
            if (getEditor.MapLayers == Editor.EMapLayers.BACKGROUND)
            {
                if (CurrentTile == '`') return "Solid Grass";
                if (CurrentTile == '0') return "Solid Grass No Top";
                if (CurrentTile == '1') return "Solid Grass To Dirt";
                if (CurrentTile == '2') return "Solid Grass Left Hill";
                if (CurrentTile == '3') return "Solid Grass Right Hill";
                if (CurrentTile == '4') return "Ladder Block";
            }
            if (getEditor.MapLayers == Editor.EMapLayers.FOREGROUND)
            {
                if (CurrentTile == '`') return "Grass Left Cliff Style 1";
                if (CurrentTile == '0') return "Grass Right Cliff Style 1";
                if (CurrentTile == '1') return "Grass Left Cliff Style 2";
                if (CurrentTile == '2') return "Grass Right Cliff Style 2";
                if (CurrentTile == '3') return "Grass Right Hill";
                if (CurrentTile == '4') return "Grass Left Hill";
                if (CurrentTile == '5') return "Grass Half Single";
                if (CurrentTile == '6') return "Grass Half Left";
                if (CurrentTile == '7') return "Grass Half Mid";
                if (CurrentTile == '8') return "Grass Half Right";
                if (CurrentTile == '9') return "Grass Edge Left";
                if (CurrentTile == '-') return "Grass Edge Right";
                if (CurrentTile == '=') return "Grass Single";
                if (CurrentTile == '~') return "Grass Left";
                if (CurrentTile == '!') return "Grass Right";
                if (CurrentTile == '@') return "Grass Single No Top";
                if (CurrentTile == '#') return "Grass Left No Top";
                if (CurrentTile == '$') return "Grass Right No Top";
            }
            if (getEditor.MapLayers == Editor.EMapLayers.EFFECT)
            {
                if (CurrentTile == '`') return "Shade Solid";
                if (CurrentTile == '0') return "Shade Top Left";
                if (CurrentTile == '1') return "Shade Top Right";
                if (CurrentTile == '2') return "Shade Bottom Left";
                if (CurrentTile == '3') return "Shade Bottom Right";
                if (CurrentTile == '4') return "Water Top";
                if (CurrentTile == '5') return "Water Solid";
            }

            return "None";
        }
    }
}

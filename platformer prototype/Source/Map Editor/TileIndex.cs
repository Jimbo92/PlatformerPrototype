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
                if (CurrentTile == '☺') return "Solid CB";
                if (CurrentTile == '☻') return "Left Hill CB";
                if (CurrentTile == '♥') return "Right Hill CB";
                if (CurrentTile == '♦') return "Half CB";
                if (CurrentTile == '♣') return "Water Trigger";
                if (CurrentTile == '♠') return "Ladder Trigger";
                if (CurrentTile == '•') return "Lava Trigger";
                if (CurrentTile == '◘') return "Crystal Trigger";
                if (CurrentTile == '○') return "Player Start";
                if (CurrentTile == '◙') return "Classic CB";
                if (CurrentTile == '♂') return "NPC Friendly Spawner";
                if (CurrentTile == '♀') return "Breakable Box";
                if (CurrentTile == '♪') return "Horizontal MB";
                if (CurrentTile == '♫') return "Vertical MB";
                if (CurrentTile == '☼') return "Enemy Spawn Crawler";
                if (CurrentTile == '►') return "Enemy Spawn Walker";
                if (CurrentTile == '◄') return "Enemy Spawn Fly";
                if (CurrentTile == '↕') return "Sign With Text";
                if (CurrentTile == '‼') return "Switch Trigger";
                if (CurrentTile == '¶') return "Lock Trigger";
                

            }
            if (getEditor.MapLayers == Editor.EMapLayers.BACKGROUND)
            {
                //----------------------------//Grass//----------------------------//
                if (CurrentTile == '☺') return "Grass Solid Up";
                if (CurrentTile == '☻') return "Grass Solid Left";
                if (CurrentTile == '♥') return "Grass Solid Right";
                if (CurrentTile == '♦') return "Grass Solid Down";
                if (CurrentTile == '♣') return "Grass Solid Corner Up Left";
                if (CurrentTile == '♠') return "Grass Solid Corner Up Right";
                if (CurrentTile == '•') return "Grass Solid Corner Down Left";
                if (CurrentTile == '◘') return "Grass Solid Corner Down Right";
                if (CurrentTile == '○') return "Grass Solid Hill Up Right";
                if (CurrentTile == '◙') return "Grass Solid Hill Up Left";
                if (CurrentTile == '♂') return "Grass Solid Hill Down Left";
                if (CurrentTile == '♀') return "Grass Solid Hill Down Right";
                if (CurrentTile == '♪') return "Grass Solid Mid NoTop";
                if (CurrentTile == '♫') return "Grass Solid Mid NoTop Behind";
                if (CurrentTile == '☼') return "Grass Mid No Top Bottomless Behind";
                //----------------------------//Dirt//----------------------------//
                if (CurrentTile == '►') return "Dirt Solid Up";
                if (CurrentTile == '◄') return "Dirt Solid Left";
                if (CurrentTile == '↕') return "Dirt Solid Right";
                if (CurrentTile == '‼') return "Dirt Solid Down";
                if (CurrentTile == '¶') return "Dirt Solid Corner Up Left";
                if (CurrentTile == '§') return "Dirt Solid Corner Up Right";
                if (CurrentTile == '▬') return "Dirt Solid Corner Down Left";
                if (CurrentTile == '↨') return "Dirt Solid Corner Down Right";
                if (CurrentTile == '↑') return "Dirt Solid Hill Up Right";
                if (CurrentTile == '↓') return "Dirt Solid Hill Up Left";
                if (CurrentTile == '→') return "Dirt Solid Hill Down Left";
                if (CurrentTile == '←') return "Dirt Solid Hill Down Right";
                if (CurrentTile == '∟') return "Dirt Solid Mid NoTop";
                if (CurrentTile == '↔') return "Dirt Solid Mid NoTop Behind";
                if (CurrentTile == '▲') return "Dirt Mid No Top Bottomless Behind";
                //----------------------------//Sand//----------------------------//
                if (CurrentTile == '▼') return "Sand Solid Up";
                if (CurrentTile == '!') return "Sand Solid Left";
                if (CurrentTile == '"') return "Sand Solid Right";
                if (CurrentTile == '#') return "Sand Solid Down";
                if (CurrentTile == '$') return "Sand Solid Corner Up Left";
                if (CurrentTile == '%') return "Sand Solid Corner Up Right";
                if (CurrentTile == '&') return "Sand Solid Corner Down Left";
                if (CurrentTile == '■') return "Sand Solid Corner Down Right";
                if (CurrentTile == '(') return "Sand Solid Hill Up Right";
                if (CurrentTile == ')') return "Sand Solid Hill Up Left";
                if (CurrentTile == '*') return "Sand Solid Hill Down Left";
                if (CurrentTile == '+') return "Sand Solid Hill Down Right";
                if (CurrentTile == '-') return "Sand Solid Mid NoTop";
                if (CurrentTile == '.') return "Sand Solid Mid NoTop Behind";
                if (CurrentTile == '/') return "Sand Mid No Top Bottomless Behind";
            }
            if (getEditor.MapLayers == Editor.EMapLayers.FOREGROUND)
            {
                //----------------------------//Grass//----------------------------//
                if (CurrentTile == '☺') return "Grass Left Cliff Style 1";
                if (CurrentTile == '☻') return "Grass Right Cliff Style 1";
                if (CurrentTile == '♥') return "Grass Left Cliff Style 2";
                if (CurrentTile == '♦') return "Grass Right Cliff Style 2";
                if (CurrentTile == '♣') return "Grass Right Hill";
                if (CurrentTile == '♠') return "Grass Left Hill";
                if (CurrentTile == '•') return "Grass Half Single";
                if (CurrentTile == '◘') return "Grass Half Left";
                if (CurrentTile == '○') return "Grass Half Mid";
                if (CurrentTile == '◙') return "Grass Half Right";
                if (CurrentTile == '♂') return "Grass Edge Left";
                if (CurrentTile == '♀') return "Grass Edge Right";
                if (CurrentTile == '♪') return "Grass Single";
                if (CurrentTile == '♫') return "Grass Left";
                if (CurrentTile == '☼') return "Grass Right";
                if (CurrentTile == '►') return "Grass Mid Top";
                if (CurrentTile == '◄') return "Grass Single NoTop";
                if (CurrentTile == '↕') return "Grass Mid NoTop Bottomless";
                if (CurrentTile == '‼') return "Grass Half Down NoTop Left";
                if (CurrentTile == '¶') return "Grass Half Down NoTop Right";
                if (CurrentTile == '§') return "Grass Half Up NoTop Left";
                if (CurrentTile == '▬') return "Grass Half UP NoTop Right";
                //----------------------------//Dirt//----------------------------//
                if (CurrentTile == '↨') return "Dirt Left Cliff Style 1";
                if (CurrentTile == '↑') return "Dirt Right Cliff Style 1";
                if (CurrentTile == '↓') return "Dirt Left Cliff Style 2";
                if (CurrentTile == '→') return "Dirt Right Cliff Style 2";
                if (CurrentTile == '←') return "Dirt Right Hill";
                if (CurrentTile == '∟') return "Dirt Left Hill";
                if (CurrentTile == '↔') return "Dirt Half Single";
                if (CurrentTile == '▲') return "Dirt Half Left";
                if (CurrentTile == '▼') return "Dirt Half Mid";
                if (CurrentTile == '!') return "Dirt Half Right";
                if (CurrentTile == '"') return "Dirt Edge Left";
                if (CurrentTile == '#') return "Dirt Edge Right";
                if (CurrentTile == '$') return "Dirt Single";
                if (CurrentTile == '%') return "Dirt Left";
                if (CurrentTile == '&') return "Dirt Right";
                if (CurrentTile == '■') return "Dirt Mid Top";
                if (CurrentTile == '(') return "Dirt Single NoTop";
                if (CurrentTile == ')') return "Dirt Mid NoTop Bottomless";
                if (CurrentTile == '*') return "Dirt Half Down NoTop Left";
                if (CurrentTile == '+') return "Dirt Half Down NoTop Right";
                if (CurrentTile == '-') return "Dirt Half Up NoTop Left";
                if (CurrentTile == '.') return "Dirt Half Up NoTop Right";
                //----------------------------//Sand//----------------------------//
                if (CurrentTile == '0') return "Sand Left Cliff Style 1";
                if (CurrentTile == '1') return "Sand Right Cliff Style 1";
                if (CurrentTile == '2') return "Sand Left Cliff Style 2";
                if (CurrentTile == '3') return "Sand Right Cliff Style 2";
                if (CurrentTile == '4') return "Sand Right Hill";
                if (CurrentTile == '5') return "Sand Left Hill";
                if (CurrentTile == '6') return "Sand Half Single";
                if (CurrentTile == '7') return "Sand Half Left";
                if (CurrentTile == '8') return "Sand Half Mid";
                if (CurrentTile == '9') return "Sand Half Right";
                if (CurrentTile == ':') return "Sand Edge Left";
                if (CurrentTile == ';') return "Sand Edge Right";
                if (CurrentTile == '<') return "Sand Single";
                if (CurrentTile == '=') return "Sand Left";
                if (CurrentTile == '>') return "Sand Right";
                if (CurrentTile == '?') return "Sand Mid Top";
                if (CurrentTile == '@') return "Sand Single NoTop";
                if (CurrentTile == 'A') return "Sand Mid NoTop Bottomless";
                if (CurrentTile == 'B') return "Sand Half Down NoTop Left";
                if (CurrentTile == 'C') return "Sand Half Down NoTop Right";
                if (CurrentTile == 'D') return "Sand Half Up NoTop Left";
                if (CurrentTile == 'E') return "Sand Half Up NoTop Right";


            }
            if (getEditor.MapLayers == Editor.EMapLayers.EFFECT)
            {
                if (CurrentTile == '☺') return "Shade Solid";
                if (CurrentTile == '☻') return "Shade Top Left";
                if (CurrentTile == '♥') return "Shade Top Right";
                if (CurrentTile == '♦') return "Shade Bottom Left";
                if (CurrentTile == '♣') return "Shade Bottom Right";
                if (CurrentTile == '♠') return "Water Top";
                if (CurrentTile == '•') return "Water Solid";
                if (CurrentTile == '◘') return "Lava Top";
                if (CurrentTile == '○') return "Lava Solid";
            }

            return "None";
        }
    }
}

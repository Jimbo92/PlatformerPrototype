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
    class MenuButton
    {
        public Texture2D _Texture;
        public Vector2 _Position;
        public Rectangle _Rect;
        
        public Color ButtonColour = Color.White;

        private string ButtonName;

        private int Width, Height;

        public MenuButton(Texture2D texture2d, Vector2 position, string Name)
        {
            _Position = position;
            _Texture = texture2d;
            ButtonName = Name;
            Width = (int)Textures._BasicFont.MeasureString(ButtonName).X + 10;
            Height = (int)Textures._BasicFont.MeasureString(ButtonName).Y + 10;
        }

        public void Update()
        {
            _Rect = new Rectangle((int)_Position.X, (int)_Position.Y, Width, Height);

            if (_Rect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                if (Input.ClickPress(Input.EClicks.LEFT))
                    ButtonColour = Color.Yellow;
                else
                    ButtonColour = Color.LightGray;
            }
            else
                ButtonColour = Color.White;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, _Rect, ButtonColour * 0.95f);

            spriteBatch.DrawString(Textures._BasicFont, ButtonName, new Vector2(_Rect.X + Width / 2, _Rect.Y + Height / 2), Color.White, 0, Textures._BasicFont.MeasureString(ButtonName) / 2, 1, SpriteEffects.None, 0);
        }

    }
}

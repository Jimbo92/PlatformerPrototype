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
    class NPC
    {
        public enum npcType
        {
            WALKER,
            FLYER,
            CRAWLER,
            FRIENDLY,
            SIGN,
        }

        public Sprite sprite;
        public SpriteFont Font;
        public npcType type;

        public Vector2 Position;
        public Rectangle Bounds;
        public Vector2 Speed;
        public bool isDead = true;
        public int Width = 24;
        public int Height = 24;

        public Rectangle TextDisplayBounds;


        private BaseEngine BEngine;
        //private Game1 game1;

        public Vector2 tl = Vector2.Zero;
        public Vector2 tr = Vector2.Zero;
        public Vector2 bl = Vector2.Zero;
        public Vector2 br = Vector2.Zero;

        public Vector2 runPlanes;

        public Vector2 platMod;

        //Controls
        public bool controllable = false;
        bool jump = false;
        bool left = false;
        bool right = false;
        bool down = false;

        int npcID;


        public bool[] checks = new bool[4];

        public float xFriction = 1;
        public float yFriction = 1;

        private int AnimTimer;
        private SpriteEffects SprEff;
        private float TextBubbleFadeValue;
        private Texture2D SpeechBubble_Tex;
        private Rectangle SpeechBubble_Rect;
        private bool isWalking;

        public Color colour = Color.White;

        public string TextTalk;

        //--------------------------------------------

        public NPC(ContentManager getContent, string getTexture, npcType e, int getWidth, int getHeight, int id)
        {
            type = e;
            Width = getWidth;
            Height = getHeight;
            Position = Vector2.Zero;
            left = true;
            runPlanes = new Vector2(-50, 50);

            if (type != npcType.FRIENDLY && type != npcType.SIGN)
                sprite = new Sprite(getContent, getTexture, getWidth, getHeight, 1, 3);
            else if (type == npcType.FRIENDLY)
                sprite = new Sprite(getContent, "objects/NPCFriendSS", getWidth, getHeight, 2, 8);

            Font = getContent.Load<SpriteFont>("fonts/CopperplateGothicBold");
            SpeechBubble_Tex = getContent.Load<Texture2D>("objects/speechbubblebase");
            npcID = id;
        }

        private void Animations()
        {
            if (type != npcType.FRIENDLY && type != npcType.SIGN)
            {
                AnimTimer++;
                if (AnimTimer < 10)
                    sprite.CurrentFrame = 0;
                else
                    sprite.CurrentFrame = 1;


                if (AnimTimer > 20)
                    AnimTimer = 0;

            }
            if (type == npcType.FRIENDLY)
            {
                if (Speed.X <= -1)
                {
                    SprEff = SpriteEffects.FlipHorizontally;
                    isWalking = true;
                }
                else if (Speed.X >= 1)
                {
                    SprEff = SpriteEffects.None;
                    isWalking = true;
                }
                else
                    isWalking = false;

                if (isWalking)
                {
                    sprite.CurrentFrame += 0.15f;
                    if (sprite.CurrentFrame > 10)
                        sprite.CurrentFrame = 0;
                }
                else
                {
                    SprEff = SpriteEffects.None;
                    sprite.CurrentFrame = 14;
                }
            }          
        }

        public void updateBounds(Vector2 Camera)
        {
            Bounds = new Rectangle((int)Position.X + (int)Camera.X, (int)Position.Y + (int)Camera.Y, Width, Height);
            tl = new Vector2(Bounds.X, Bounds.Y);
            tr = new Vector2(Bounds.X + Bounds.Width, Bounds.Y);
            bl = new Vector2(Bounds.X, Bounds.Y + Bounds.Height);
            br = new Vector2(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height);
        }

        private void Collisions()
        {
            // Check X Collisions-----------------------------
            //Will Check up to 6 Rectangles

            Position.X += (int)Speed.X;
            updateBounds(Camera.Position);
            BEngine.updateHitboxes(Position, Bounds);
            foreach (Rectangle canvas in BEngine.Canvas)
                checkCollisionsX(canvas);

            //Will Check up to 6 Triangles

            updateBounds(Camera.Position);
            BEngine.updateHitboxes(Position, Bounds);
            checkTollisionsX(BEngine.tan1);
            checkTollisionsX(BEngine.tan2);
            checkTollisionsX(BEngine.tan3);
            checkTollisionsX(BEngine.tan4);
            checkTollisionsX(BEngine.tan5);
            checkTollisionsX(BEngine.tan6);


            // Check Y Collisions-----------------------------
            //Will Check up to 6 Rectangles

            Position.Y += (int)Speed.Y;
            updateBounds(Camera.Position);
            BEngine.updateHitboxes(Position, Bounds);
            foreach (Rectangle canvas in BEngine.Canvas)
                checkCollisionsY(canvas);

            //Will Check up to 6 Triangles

            updateBounds(Camera.Position);
            BEngine.updateHitboxes(Position, Bounds);
            checkTollisionsY(BEngine.tan1);
            checkTollisionsY(BEngine.tan2);
            checkTollisionsY(BEngine.tan3);
            checkTollisionsY(BEngine.tan4);
            checkTollisionsY(BEngine.tan5);
            checkTollisionsY(BEngine.tan6);

            checks[0] = false;
            updateBounds(Camera.Position);
            BEngine.updateNoclips(Position, Bounds, '♠');
            foreach (Rectangle noclips in BEngine.NoClip)
            {
                if (Bounds.Intersects(noclips))
                {
                    checks[0] = true;
                }

            }

            checks[1] = false;
            updateBounds(Camera.Position);
            BEngine.updateNoclips(Position, Bounds, '♣');
            foreach (Rectangle noclips in BEngine.NoClip)
            {
                if (Bounds.Intersects(noclips))
                {
                    checks[1] = true;
                }

            }

            checks[2] = false;
            updateBounds(Camera.Position);
            BEngine.updateNoclips(Position, Bounds, '•');
            foreach (Rectangle noclips in BEngine.NoClip)
            {
                if (Bounds.Intersects(noclips))
                {
                    checks[2] = true;
                }

            }

            checks[3] = false;

            updateBounds(Camera.Position);
            BEngine.updateNoclips(Position, Bounds, '◙');
            foreach (Rectangle noclips in BEngine.NoClip)
            {
                if (noclips.Y > Bounds.Y + Bounds.Height - Speed.Y - 1)
                    if (Bounds.Intersects(noclips))
                    {
                        checks[3] = true;

                        for (int i = 20; i > 0; i--)
                        {
                            updateBounds(Camera.Position);
                            BEngine.updateHitboxes(Position, Bounds);
                            if (Bounds.Intersects(noclips))
                            {
                                Position.Y--;
                            }
                        }

                        Speed.Y = 0;
                    }
            }

            platMod = Vector2.Zero;
            bool platX = false;
            bool platY = false;

            updateBounds(Camera.Position);
            Bounds.Y += 1;

            foreach (Platform p in BEngine.Platforms)
            {


                p.updateBounds(Camera.Position);
                if (p.Bounds.Y > Bounds.Y + Bounds.Height - Speed.Y - 1)
                    if (Bounds.Intersects(p.Bounds))
                    {
                        if (!platX)
                        {
                            platMod.X += (int)p.Speed.X;
                            platX = true;
                        }

                        if (!platY)
                        {
                            platMod.Y += (int)p.Speed.Y;
                            platY = true;
                        }


                        for (int i = 20; i > 0; i--)
                        {
                            updateBounds(Camera.Position);
                            p.updateBounds(Camera.Position);
                            if (Bounds.Intersects(p.Bounds))
                            {
                                Position.Y--;

                            }
                        }

                        Speed.Y = 0;
                    }

            }
            Bounds.Y -= 1;



            if (checks[2] == true)
            {
                isDead = true;
            }

        }

        public void Update(BaseEngine getBEngine,GoalManager gm)
        {
            BEngine = getBEngine;

            Animations();

            Collisions();
            
            
      

            if (type == npcType.SIGN || type == npcType.FRIENDLY)
                TextTalk = gm.Speech[npcID];

            Position += platMod;

            if (!controllable && type != npcType.SIGN)
            {
                if (Position.X < runPlanes.X)
                {
                    right = true;
                    left = false;
                    SprEff = SpriteEffects.FlipHorizontally;
                }

                if (Position.X > runPlanes.Y)
                {
                    right = false;
                    left = true;
                    SprEff = SpriteEffects.None;
                }
            }
            else
            {
                jump = false;
                down = false;
                left = false;
                right = false;
            }


            //Gravity--------------
            if (type != npcType.FLYER)
            {
                bool Checked = false;
                if (!Checked)
                {
                    if (checks[0] == true)
                    {
                        yFriction = 0.95f;
                        Checked = true;
                    }
                    else
                    {
                        yFriction = 1;
                    }
                }

                if (!Checked)
                {
                    if (checks[1] == true)
                    {
                        xFriction = 0.92f;
                        yFriction = 0.95f;
                        Checked = true;
                    }
                    else
                    {
                        xFriction = 1;
                        yFriction = 1;
                    }
                }

                if (checks[1] == false)
                {
                    if (Speed.Y < 12)
                        Speed.Y += 0.2f;
                    else
                        Speed.Y = 12;
                }
                else
                {
                    if (Speed.Y < 2)
                        Speed.Y += 0.2f;
                    else
                        Speed.Y = 2;
                }
            }

            //---------------------

            if (checks[1] == false)
            {
                if (left == true)
                    if (Speed.X > -1.5f)
                        Speed.X -= 0.25f;
                    else
                        Speed.X = -1.5f;

                if (right == true)
                    if (Speed.X < 1.5f)
                        Speed.X += 0.25f;
                    else
                        Speed.X = 1.5f;

                if (!left && !right)
                    if (Math.Abs(Speed.X) > 0.1f)
                        Speed.X *= 0.92f;
                    else
                        Speed.X = 0;


                //jump
                if (jump == true)
                {
                    jump = false;
                    bool returner = false;

                    //Jump--------------

                    if (!returner)
                    {

                        Position.Y += 1;
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);



                        for (int i = 0; i < BEngine.Canvas.Length; i++)
                            if (Bounds.Intersects(BEngine.Canvas[i]))
                            {
                                Speed.Y = -7;
                                returner = true;
                            }

                        foreach (Platform p in BEngine.Platforms)
                        {
                            p.updateBounds(Camera.Position);
                            if (p.Bounds.Intersects(Bounds))
                            {
                                Speed.Y = -7;
                                returner = true;
                            }
                        }
                        if (checkAllLines(BEngine.tan1) == true)
                        {
                            Speed.Y = -7;
                            returner = true;
                        }
                        if (checkAllLines(BEngine.tan2) == true)
                        {
                            Speed.Y = -7;
                            returner = true;
                        }
                        if (checkAllLines(BEngine.tan3) == true)
                        {
                            Speed.Y = -7;
                            returner = true;
                        }
                        if (checkAllLines(BEngine.tan4) == true)
                        {
                            Speed.Y = -7;
                            returner = true;
                        }
                        if (checkAllLines(BEngine.tan5) == true)
                        {
                            Speed.Y = -7;
                            returner = true;
                        }
                        if (checkAllLines(BEngine.tan6) == true)
                        {
                            Speed.Y = -7;
                            returner = true;
                        }


                        Position.Y -= 1;

                    }
                    //--------------------

                    //Ladders----------------------------------------
                    if (!returner)
                    {
                        if (checks[0] == true)
                        {

                            Speed.Y = -4;
                        }
                    }

                }

                //Continue here




            }
            else
            {
                if (left == true)
                    if (Speed.X > -1.5f)
                        Speed.X -= 0.25f;
                    else
                        Speed.X = -1.5f;

                if (right == true)
                    if (Speed.X < 1.5f)
                        Speed.X += 0.25f;
                    else
                        Speed.X = 1.5f;

                if (!left && !right)
                    if (Math.Abs(Speed.X) > 0.1f)
                        Speed.X *= 0.92f;
                    else
                        Speed.X = 0;

                if (down == true)
                    if (Speed.Y > -1.5f)
                        Speed.Y -= 0.25f;
                    else
                        Speed.Y = -1.5f;

                if (jump == true)
                    if (Speed.Y < 1.5f)
                        Speed.Y += 0.25f;
                    else
                        Speed.Y = 1.5f;

                if (!down && !jump)
                    if (Math.Abs(Speed.Y) > 0.1f)
                        Speed.Y *= 0.92f;
                    else
                        Speed.Y = 0;


            }


            Speed.X *= xFriction;
            Speed.Y *= yFriction;
        }

        public void checkCollisionsX(Rectangle target)
        {
            if (Bounds.Intersects(target))
            {

                if (Speed.X > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (Bounds.Intersects(target))
                            Position.X--;
                    }

                if (Speed.X < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (Bounds.Intersects(target))
                            Position.X++;
                    }

                Speed.X = 0;
                jump = true;
            }
        }

        public void checkCollisionsY(Rectangle target)
        {
            if (Bounds.Intersects(target))
            {

                if (Speed.Y > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (Bounds.Intersects(target))
                            Position.Y--;
                    }

                if (Speed.Y < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (Bounds.Intersects(target))
                            Position.Y++;
                    }

                Speed.Y = 0;
            }
        }

        public void Draw(SpriteBatch sB)
        {

            //sB.Draw(Textures._OBJ_Ladder_Tex, Bounds, Color.Red);
            if (type != npcType.SIGN)
                sprite.Draw(sB, new Vector2(Bounds.X, Bounds.Y), new Vector2(0, 0), 0, SprEff, colour);

            //Friendly NPC & Sign Text Bubble
            if (type == npcType.FRIENDLY || type == npcType.SIGN)
            {
                TextDisplayBounds = new Rectangle(Bounds.X - 156 / 2, Bounds.Y - 156 / 2, 156, 156);                
                Vector2 TextBubbleSize = Font.MeasureString(TextTalk);
                SpeechBubble_Rect = new Rectangle((Bounds.X - (int)TextBubbleSize.X / 2) + 25, (Bounds.Y - (int)TextBubbleSize.Y / 2) - 25, (int)TextBubbleSize.X - 5, (int)TextBubbleSize.Y);
                sB.Draw(SpeechBubble_Tex, SpeechBubble_Rect, Color.SlateGray * TextBubbleFadeValue);
                sB.DrawString(Font, TextTalk, new Vector2(SpeechBubble_Rect.X + 5, SpeechBubble_Rect.Y + 5), Color.Silver * TextBubbleFadeValue, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                if (BEngine.player.Bounds.Intersects(TextDisplayBounds))
                {
                    TextBubbleFadeValue += 0.05f;
                    if (TextBubbleFadeValue >= 1)
                        TextBubbleFadeValue = 1;
                }
                else
                {
                    TextBubbleFadeValue -= 0.05f;
                    if (TextBubbleFadeValue <= 0)
                        TextBubbleFadeValue = 0;
                }
            }
        }

        public void checkTollisionsX(Triangle target)
        {

            if (checkAllLines(target) == true)
            {

                if (Speed.X > 0)
                {
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                            Position.X--;
                    }

                    Position.X += Math.Abs(Speed.X);
                    Position.Y -= Math.Abs(Speed.X);
                }

                if (Speed.X < 0)
                {
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                            Position.X++;
                    }

                    Position.X -= Math.Abs(Speed.X);
                    Position.Y -= Math.Abs(Speed.X);

                }
            }
        }

        public void checkTollisionsY(Triangle target)
        {
            if (checkAllLines(target) == true)
            {


                if (Speed.Y > 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                        {
                            Position.Y--;


                        }
                    }

                if (Speed.Y < 0)
                    for (int i = 20; i > 0; i--)
                    {
                        updateBounds(Camera.Position);
                        BEngine.updateHitboxes(Position, Bounds);
                        if (checkAllLines(target) == true)
                            Position.Y++;
                    }

                Speed.Y = 0;
            }
        }

        public bool checkAllLines(Triangle triangle)
        {
            if (BEngine.lineTest(tl, tr, triangle.a, triangle.b) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tl, tr, triangle.b, triangle.c) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tl, tr, triangle.c, triangle.a) == true)
            {
                return true;
            }

            if (BEngine.lineTest(tl, bl, triangle.a, triangle.b) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tl, bl, triangle.b, triangle.c) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tl, bl, triangle.c, triangle.a) == true)
            {
                return true;
            }

            if (BEngine.lineTest(bl, br, triangle.a, triangle.b) == true)
            {
                return true;
            }
            if (BEngine.lineTest(bl, br, triangle.b, triangle.c) == true)
            {
                return true;
            }
            if (BEngine.lineTest(bl, br, triangle.c, triangle.a) == true)
            {
                return true;
            }

            if (BEngine.lineTest(tr, br, triangle.a, triangle.b) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tr, br, triangle.b, triangle.c) == true)
            {
                return true;
            }
            if (BEngine.lineTest(tr, br, triangle.c, triangle.a) == true)
            {
                return true;
            }
            return false;
        }

    }
}

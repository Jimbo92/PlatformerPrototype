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
    class Particle
    {
        public Texture2D _Texture;
        public Vector2 _Position;
        public Rectangle _Rectangle;
        public Color _Colour = Color.White;
        public float _Alpha = 1;
        public float _Rotation;
        public float _RotationValue;
        public float _Gravity;
        public float _PushForce;

        public Particle(Texture2D getTexture, Vector2 getStartPos)
        {
            _Texture = getTexture;
            _Position = getStartPos;
        }

        public void Update()
        {
            _Position.X += _PushForce;
            _Position.Y += _Gravity;

            _Alpha -= 0.001f;

            _Rotation += _RotationValue;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_Alpha > 0)
            {
                _Rectangle = new Rectangle((int)_Position.X, (int)_Position.Y, _Texture.Width, _Texture.Height);

                spriteBatch.Draw(_Texture, _Rectangle, null, _Colour * _Alpha, MathHelper.ToRadians(_Rotation), new Vector2(_Texture.Width, _Texture.Height) / 2, SpriteEffects.None, 0);
            }
        }
    }

    class ParticleSystem
    {
        public List<Particle> Particle_List = new List<Particle>();

        public float _Delay = 3;
        Random _Rand = new Random();

        public void SmokeEffect(Texture2D getTexture, Vector2 getStartPos)
        {
            //Adds new particle every update
            Particle _Particle = new Particle(getTexture, getStartPos);
            _Delay -= 1;
            if (_Delay <= 0)
            {
                _Particle._Gravity = -0.5f;
                float randrot = _Rand.Next(-2, 2);
                _Particle._RotationValue = randrot;
                Particle_List.Add(_Particle);
                
                _Delay = 3;
            }

            for (int i = 0; i < Particle_List.Count; i++)
            {
                Particle_List[i].Update();

                if (Particle_List[i]._Alpha <= 0)
                    Particle_List.RemoveAt(0);
            }
        }

        public void BurstEffect(Texture2D getTexture, Vector2 getStartPos)
        {
            //Adds new particle every update
            Particle _Particle = new Particle(getTexture, getStartPos);
            _Delay -= 1;
            if (_Delay <= 0)
            {
                float randPush = _Rand.Next(-15, 15);
                _Particle._PushForce = randPush / 25;
                float randGrav = _Rand.Next(-10, 10);
                _Particle._Gravity = randGrav / 25;
                float randrot = _Rand.Next(-2, 2);
                _Particle._RotationValue = randrot;
                Particle_List.Add(_Particle);
                
                _Delay = 3;
            }

            for (int i = 0; i < Particle_List.Count; i++)
            {
                Particle_List[i].Update();

                if (Particle_List[i]._Alpha <= 0)
                    Particle_List.RemoveAt(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in Particle_List)
                p.Draw(spriteBatch);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Sunday_Bloody_Sunday
{
    class ParticuleRain
    {
        //FIELDS
        Texture2D RainTexture;
        Vector2 RainPosition;
        Vector2 RainVelocity;


        //CONSTRUCTOR
        public ParticuleRain(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.RainTexture = texture;
            this.RainPosition = position;
            this.RainVelocity = velocity;
        }


        //METHODS
        public Vector2 Position
        {
            get { return RainPosition; }
        }


        //UPDATE & DRAW
        public void Update()
        {
            RainPosition += RainVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Ressources.mRain, RainPosition, Color.SkyBlue);
        }
    }
}

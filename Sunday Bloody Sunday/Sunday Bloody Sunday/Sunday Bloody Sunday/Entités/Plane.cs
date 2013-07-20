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
    class Plane
    {
        //FIELDS
        public Rectangle PlaneTexture;
        public Vector2 target;


        //CONSTRUCTOR
        public Plane(int x, int y, int x2, int y2)
        {
            this.PlaneTexture = new Rectangle(0, y - 100, 85, 45);
            if (this.PlaneTexture.Y < 0)
            {
                this.PlaneTexture.Y = 0;
            }
            target = new Vector2(x + x2, y + y2);
        }


        //METHODS
        public void Update()
        {

        }


        //DRAW
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Ressources.mPlane, PlaneTexture, Color.White);
        }
    }
}

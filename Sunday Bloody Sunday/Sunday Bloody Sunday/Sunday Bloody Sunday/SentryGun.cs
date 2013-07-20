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
    class SentryGun
    {
        //FIELDS
        public Rectangle turretTexture;
        public Keys poserTurret;
        public bool isActive;


        //CONSTRUCTOR
        public SentryGun(int x, int y, Keys poserTurret)
        {
            this.turretTexture = new Rectangle(this.turretTexture.X, this.turretTexture.Y, 16, 16);
            this.poserTurret = poserTurret;
            this.isActive = true;
        }


        //METHODS
        // Get the width of the turret
        public int Width
        {
            get { return turretTexture.Width; }
        }

        // Get the height of the turret
        public int Height
        {
            get { return turretTexture.Height; }
        }

        public Vector2 centre()
        {
            Vector2 vector = new Vector2(turretTexture.X + Width / 2, turretTexture.Y + Height / 2);
            return vector;
        } 

        //UPDATE & DRAW
        public void Update(List<Player> liste_joueurs, List<IA> liste_ias, List<DestructibleItems> liste_barrels, KeyboardState keyboard)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Rectangle MapTexture)
        {
            if (isActive)
            {
                spriteBatch.Draw(Ressources.mAmmoBox, new Rectangle(MapTexture.X + turretTexture.X, MapTexture.Y + turretTexture.Y, 16, 16), Color.White);
            }
        }
    }
}

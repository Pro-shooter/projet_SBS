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
    class Spawn_Items
    {
        public List<Vector2> emplacement;

        public Spawn_Items(List<Vector2> liste)
        {
            this.emplacement = liste;
        }
    }

    class Items
    {
        // FIELDS
            //HEALTHBOX
            public Rectangle HealthBoxTexture;
            public bool isVisible;
            public int healPoints;
            public Rectangle Aire_healthBox;
            //AMMOBOX
            public Rectangle AmmoBoxTexture;
            public int ammoNumber;
            public Rectangle Aire_ammoBox;

            private string type;


        // CONSTRUCTOR
        public Items(int x, int y, string type)
        {
            this.HealthBoxTexture = new Rectangle(x, y, 16, 16);
            this.isVisible = true;
            this.healPoints = 10;
            this.AmmoBoxTexture = new Rectangle(x, y, 16, 16);
            this.ammoNumber = 10;
            this.Aire_healthBox = new Rectangle(HealthBoxTexture.X, HealthBoxTexture.Y, HealthBoxTexture.Width, HealthBoxTexture.Height);
            this.Aire_ammoBox = new Rectangle(AmmoBoxTexture.X, AmmoBoxTexture.Y, AmmoBoxTexture.Width, AmmoBoxTexture.Height);
            this.type = type;

        }


        // METHODS
        private void utilisation(Player joueur)
        {
            if (type == "health" && joueur.Health < 100)
            {
                joueur.Health = joueur.Health + this.healPoints;
                isVisible = false;
                if (joueur.Health > 100)
                {
                    joueur.Health = 100;
                }
            }
            else if (type == "ammo" && (joueur.Ammo < 100 || joueur.bomb <= 0))
            {
                joueur.Ammo = joueur.Ammo + this.ammoNumber;
                isVisible = false;
                if (joueur.Ammo > 100)
                {
                    joueur.Ammo = 100;
                }
                if (joueur.bomb <= 0)
                {
                    joueur.bomb++;
                }
            }
            else
            {
            }
        }


        // UPDATE & DRAW
        public void Update(List<Player> joueurs)
        {
            foreach (Player joueur in joueurs)
            {
                if ((joueur.PlayerTexture.Intersects(this.HealthBoxTexture)) && isVisible)
                {
                    utilisation(joueur);
                }
                if ((joueur.PlayerTexture.Intersects(this.AmmoBoxTexture)) && isVisible)
                {
                    utilisation(joueur);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle Maptexture)
        {
            switch (type)
            {
                case ("health"):
                    spriteBatch.Draw(Ressources.mHealthBox, new Rectangle(Maptexture.X + HealthBoxTexture.X,Maptexture.Y + HealthBoxTexture.Y, HealthBoxTexture.Width,HealthBoxTexture.Width), Color.White);
                    break;
                case ("ammo"):
                    spriteBatch.Draw(Ressources.mAmmoBox, new Rectangle(Maptexture.X + AmmoBoxTexture.X, Maptexture.Y + AmmoBoxTexture.Y, AmmoBoxTexture.Width, AmmoBoxTexture.Width), Color.White);
                    break;
                default:
                    break;
            }
        }
    }
}

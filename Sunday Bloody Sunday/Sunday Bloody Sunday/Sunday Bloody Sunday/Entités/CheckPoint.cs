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
    class CheckPoint
    {
        //FIELDS
        public Rectangle CheckPointArrivee, CheckPointBossEntry;
        public bool est_arrivee = false, combat_boss = false;
        public string type;


        //CONSTRUCTOR
        public CheckPoint(int x, int y, string type)
        {
            this.CheckPointArrivee = new Rectangle(x, y, 16, 16);
            this.CheckPointBossEntry = new Rectangle(x, y, 16, 16);
            this.type = type;
        }


        //METHODS


        //UPDATE & DRAW
        public void Update(List<Player> liste_joueurs)
        {
            foreach (Player joueur in liste_joueurs)
            {
                if (joueur.PlayerTexture.Intersects(CheckPointArrivee) && type == "arrivee")
                {
                    est_arrivee = true;
                }
            }
            foreach (Player joueur in liste_joueurs)
            {
                if (joueur.PlayerTexture.Intersects(CheckPointBossEntry) && type == "bossentry")
                {
                    combat_boss = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle MapTexture)
        {
            switch (type)
            {
                case ("arrivee"):
                    spriteBatch.Draw(Ressources.mCross, new Rectangle(this.CheckPointArrivee.X + MapTexture.X, this.CheckPointArrivee.Y + MapTexture.Y, 16, 16), Color.White);
                    break;
                default:
                    break;
            }
        }
    }
}

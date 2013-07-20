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
    class KristboulFaille
    {
        //FIELDS
        public Rectangle FailleTexture;
        public bool bsod = false;


        //CONSTRUTOR
        public KristboulFaille(int x, int y)
        {
            this.FailleTexture = new Rectangle(x, y, 32, 32);
        }


        //METHODS


        //UPDATE & DRAW
        public void Update(List<Player> liste_joueurs)
        {
            foreach (Player joueur in liste_joueurs)
            {
                if (joueur.PlayerTexture.Intersects(this.FailleTexture))
                {
                    bsod = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Ressources.mWarning, FailleTexture, Color.White);
        }
    }
}

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
    class Spawn_JetPack
    {
        public List<Vector2> emplacement;

        public Spawn_JetPack(List<Vector2> liste)
        {
            this.emplacement = liste;
        }
    }

    class JetPack
    {
        //FIELDS
        public Rectangle JetPackTexture;
        public bool isVisible;


        //CONSTRUCTOR
        public JetPack(int x, int y)
        {
            this.JetPackTexture = new Rectangle(x, y, 16, 16);
            this.isVisible = true;
        }


        //METHODS


        //UPDATE & DRAW
        public void Update(List<Player> liste_joueurs)
        {
            foreach (Player joueur in liste_joueurs)
            {
                if (joueur.PlayerTexture.Intersects(this.JetPackTexture))
                {
                    isVisible = false;
                    joueur.timer = 35;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle MapTexture)
        {
            spriteBatch.Draw(Ressources.mJetPack, new Rectangle(JetPackTexture.X + MapTexture.X, JetPackTexture.Y + MapTexture.Y, JetPackTexture.Width, JetPackTexture.Height), Color.White);
        }
    }
}

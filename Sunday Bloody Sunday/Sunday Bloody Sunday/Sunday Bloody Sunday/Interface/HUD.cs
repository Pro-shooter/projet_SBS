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
    class HUD
    {
        // FIELDS
        int healthPlayer = 0;
        int munitionPlayer = 0;
        int healthBoss = 0;


        // CONSTRUCTOR
        public HUD()
        {

        }


        // METHODS


        // UPDATE & DRAW
        public void Update(KeyboardState keyboard, List<Player> liste_joueurs, List<IA> liste_ias)
        {
            this.munitionPlayer = 0;
            this.healthPlayer = 0;
            this.healthBoss = 0;

            foreach (Player joueur in liste_joueurs)
            {
                this.munitionPlayer = this.munitionPlayer + joueur.Ammo;
                this.healthPlayer = this.healthPlayer + joueur.Health;
            }
            if (liste_ias.Count != 0)
            {
                if (liste_ias[0].id_texture == 6 || liste_ias[0].id_texture == 7 || liste_ias[0].id_texture == 8)
                {
                    healthBoss = liste_ias[0].Health;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int id_texture)
        {
            if (id_texture != 4 && id_texture != 9 && id_texture != 15 && (id_texture != 5 && id_texture != 6 && id_texture != 7 && id_texture != 8))
            {
                if (MenuButton.language == "French")
                {
                    spriteBatch.DrawString(Ressources.HUD, "Vie: " + this.healthPlayer, new Vector2(620, 400), Color.White);
                    spriteBatch.DrawString(Ressources.HUD, "Munition: " + munitionPlayer, new Vector2(620, 440), Color.White);
                    spriteBatch.DrawString(Ressources.HUD, "Victimes:", new Vector2(5, 5), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(Ressources.HUD, "Health: " + this.healthPlayer, new Vector2(650, 400), Color.White);
                    spriteBatch.DrawString(Ressources.HUD, "Ammo: " + munitionPlayer, new Vector2(650, 440), Color.White);
                    spriteBatch.DrawString(Ressources.HUD, "Kills:", new Vector2(5, 5), Color.White);
                }
            }
            if (id_texture == 5 || id_texture == 6 || id_texture == 7 || id_texture == 8)
            {
                if (MenuButton.language == "French")
                {
                    spriteBatch.DrawString(Ressources.HUD, "Vie: " + this.healthPlayer, new Vector2(620, 400), Color.CornflowerBlue);
                    spriteBatch.DrawString(Ressources.HUD, "Munition: " + munitionPlayer, new Vector2(620, 440), Color.CornflowerBlue);
                    spriteBatch.DrawString(Ressources.HUD, "Victimes:", new Vector2(5, 5), Color.CornflowerBlue);
                }
                else
                {
                    spriteBatch.DrawString(Ressources.HUD, "Health: " + this.healthPlayer, new Vector2(650, 400), Color.CornflowerBlue);
                    spriteBatch.DrawString(Ressources.HUD, "Ammo: " + munitionPlayer, new Vector2(650, 440), Color.CornflowerBlue);
                    spriteBatch.DrawString(Ressources.HUD, "Kills:", new Vector2(5, 5), Color.CornflowerBlue);
                }
            }
            if (id_texture == 9)
            {
                if (MenuButton.language == "French")
                {
                    spriteBatch.DrawString(Ressources.HUD, "Vie: " + this.healthPlayer, new Vector2(620, 400), Color.Blue);
                    spriteBatch.DrawString(Ressources.HUD, "Munition: " + munitionPlayer, new Vector2(620, 440), Color.Blue);
                    spriteBatch.DrawString(Ressources.HUD, "Boss: " + this.healthBoss, new Vector2(5, 5), Color.Blue);
                }
                else
                {
                    spriteBatch.DrawString(Ressources.HUD, "Health: " + this.healthPlayer, new Vector2(650, 400), Color.Blue);
                    spriteBatch.DrawString(Ressources.HUD, "Ammo: " + munitionPlayer, new Vector2(650, 440), Color.Blue);
                    spriteBatch.DrawString(Ressources.HUD, "Boss: " + this.healthBoss, new Vector2(5, 5), Color.Blue);
                }
            }
            if (id_texture == 4 || id_texture == 15)
            {
                if (MenuButton.language == "French")
                {
                    spriteBatch.DrawString(Ressources.HUD, "Vie: " + this.healthPlayer, new Vector2(620, 400), Color.White);
                    spriteBatch.DrawString(Ressources.HUD, "Munition: " + munitionPlayer, new Vector2(620, 440), Color.White);
                    spriteBatch.DrawString(Ressources.HUD, "Boss: " + this.healthBoss, new Vector2(5, 5), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(Ressources.HUD, "Health: " + this.healthPlayer, new Vector2(650, 400), Color.White);
                    spriteBatch.DrawString(Ressources.HUD, "Ammo: " + munitionPlayer, new Vector2(650, 440), Color.White);
                    spriteBatch.DrawString(Ressources.HUD, "Boss: " + this.healthBoss, new Vector2(5, 5), Color.White);
                }
            }
        }
    }
}

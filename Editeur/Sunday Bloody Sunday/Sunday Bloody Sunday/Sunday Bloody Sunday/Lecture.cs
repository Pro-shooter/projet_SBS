using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Sunday_Bloody_Sunday
{
    class Lecture
    {
        public static void ecrire(char[,] tableau, int longueur, int largeur)
        {
            StreamWriter ecriture = new StreamWriter("map.txt");
            int y = 0;
            while (y < longueur)
            {
               
                string ligne = "";
                int x = 0;
                while (x < largeur)
                {
                    ligne = ligne + tableau[x, y];
                    x++;
                }
                ecriture.WriteLine(ligne);
                y++;
            }
            ecriture.Close();
        }

        public static void lire(ref int largeur, ref int hauteur)
        {
            StreamReader lecture = new StreamReader("data.txt");
            string largeur_ = lecture.ReadLine();
            try
            {
                largeur = System.Convert.ToInt32(largeur_);
            }
            catch
            {
                largeur = 0;
            }
            string hauteur_ = lecture.ReadLine();
            try
            {
                hauteur = System.Convert.ToInt32(hauteur_);
            }
            catch
            {
                hauteur = 0;
            }
            lecture.Close();
        }
    }
}

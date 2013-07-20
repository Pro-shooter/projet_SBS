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
    class Editeur
    {
        Texture2D map;
        Texture2D valide;
        Texture2D invalide;
        Char[,] tableau;
        int largeur;
        int longueur;
        int x;
        int y;

        //On suppose les tiles en 16*16
        public Editeur(Texture2D map, Texture2D valide, Texture2D invalide, int largeur, int longueur)
        {
            this.map = map;
            this.tableau = new char[longueur, largeur];
            int x = 0;
            this.largeur = largeur;
            this.longueur = longueur;
            while (x < longueur)
            {
                int y = 0;
                while (y < largeur)
                {
                    tableau[x, y] = '0';
                    y++;
                }
                x++;
            }
            this.valide = valide;
            this.invalide = invalide;
            this.x = 0;
            this.y = 0;
        }

        public void Update(MouseState souris, KeyboardState clavier)
        {
            if (souris.LeftButton == ButtonState.Pressed)
            {
                try
                {
                    tableau[(souris.X - x) / 16, (souris.Y - y) / 16] = '0';
                }
                catch
                {
                }
            }
            else if (souris.RightButton == ButtonState.Pressed)
            {
                try
                {
                    tableau[(souris.X - x) / 16, (souris.Y - y) / 16] = '1';
                }
                catch
                {
                }
            }
            if (clavier.IsKeyDown(Keys.Enter))
            {
                Lecture.ecrire(tableau, largeur, longueur);
            }
            if (clavier.IsKeyDown(Keys.Up))
            {
                y = y + 2;
            }
            if (clavier.IsKeyDown(Keys.Down))
            {
                y = y - 2;
            }
            if (clavier.IsKeyDown(Keys.Left))
            {
                x = x + 2;
            }
            if (clavier.IsKeyDown(Keys.Right))
            {
                x = x - 2;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(map, new Vector2(this.x, this.y), Color.White);

            int x_ = 0;
            while (x_ < longueur)
            {
                int y_ = 0;
                while (y_ < largeur)
                {
                    /*if (tableau[x, y] == '0')
                    {
                        spriteBatch.Draw(valide, new Vector2(x*16, y*16), Color.White);
                    }
                    else*/
                    if (tableau[x_, y_] == '1')
                    {
                        spriteBatch.Draw(invalide, new Vector2(x_ * 16 + x, y_ * 16 + y), Color.White);
                    }
                    y_++;
                }
                x_++;
            }
        }
    }
}

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
    class DestructibleItems
    {
        //FIELDS
        //EXPLOSIVEBOX
        public Rectangle BarrelTexture;
        public Rectangle Aire_barrel;
        //BOMB
        public Rectangle BombTexture;
        public Rectangle Aire_bomb;
        public bool isVisible;
        public string type;
        public Keys déclencher;
        public int detonnateur;
        public int owner;


        //CONSTRUCTOR
        public DestructibleItems(int x, int y, string type)
        {
            this.isVisible = true;
            this.BarrelTexture = new Rectangle(x, y, 16, 16);
            this.Aire_barrel = new Rectangle(BarrelTexture.X, BarrelTexture.Y, BarrelTexture.Width, BarrelTexture.Height);
            this.BombTexture = new Rectangle(x, y, 16, 16);
            this.Aire_bomb = new Rectangle(BombTexture.X, BombTexture.Y, BombTexture.Width, BombTexture.Height);
            this.type = type;
        }

        public DestructibleItems(int x, int y, string type, Keys déclencher, int owner)
        {
            this.isVisible = true;
            this.BarrelTexture = new Rectangle(x, y, 16, 16);
            this.Aire_barrel = new Rectangle(BarrelTexture.X, BarrelTexture.Y, BarrelTexture.Width, BarrelTexture.Height);
            this.BombTexture = new Rectangle(x, y, 16, 16);
            this.Aire_bomb = new Rectangle(BombTexture.X, BombTexture.Y, BombTexture.Width, BombTexture.Height);
            this.type = type;
            this.déclencher = déclencher;
            detonnateur = 120;
            this.owner = owner;
        }

        public DestructibleItems(int x, int y, string type, int detonnateur)
        {
            this.isVisible = true;
            this.BarrelTexture = new Rectangle(x, y, 16, 16);
            this.Aire_barrel = new Rectangle(BarrelTexture.X, BarrelTexture.Y, BarrelTexture.Width, BarrelTexture.Height);
            this.BombTexture = new Rectangle(x, y, 16, 16);
            this.Aire_bomb = new Rectangle(BombTexture.X, BombTexture.Y, BombTexture.Width, BombTexture.Height);
            this.type = type;
            this.detonnateur = detonnateur;
        }
        //METHODS


        //UPDATE & DRAW
        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch, Rectangle Maptexture)
        {
            switch (type)
            {
                case ("barrel"):
                    spriteBatch.Draw(Ressources.mExplosiveBox, new Rectangle(Maptexture.X + BarrelTexture.X, Maptexture.Y + BarrelTexture.Y, BarrelTexture.Width, BarrelTexture.Width), Color.White);
                    break;
                case ("bomb"):
                    spriteBatch.Draw(Ressources.mBomb, new Rectangle(Maptexture.X + BombTexture.X, Maptexture.Y + BombTexture.Y, BombTexture.Width, BombTexture.Width), Color.White);
                    break;
                case ("bomb_2"):
                    spriteBatch.Draw(Ressources.mBomb, new Rectangle(Maptexture.X + BombTexture.X, Maptexture.Y + BombTexture.Y, BombTexture.Width, BombTexture.Width), Color.White);
                    break;
                case ("bomb_boss"):
                    spriteBatch.Draw(Ressources.mBomb, new Rectangle(Maptexture.X + BombTexture.X, Maptexture.Y + BombTexture.Y, BombTexture.Width, BombTexture.Width), Color.White);
                    break;
                default:
                    break;
            }
        }
    }
}

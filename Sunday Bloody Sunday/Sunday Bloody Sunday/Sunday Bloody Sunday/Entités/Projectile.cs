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
    public enum DirectionProjectile
    {
        Up, Down, Left, Right
    };

    class Projectile
    {
        // FIELDS
        public Vector2 ProjectilePosition;
        public bool isVisible;
        public int Damage;
        public int projectileMoveSpeed;
        public bool update;
        public int init;
        public string direction;
        public bool est_update = false;
        double dispertion;


        // CONSTRUCTOR
        public Projectile(Texture2D newTexture, int x, int y, int vitesse, Direction direction, int dommage, int dispertion, int seed)
        {
            Ressources.Projectile = newTexture;
            this.ProjectilePosition = new Vector2(x, y);
            this.Damage = dommage;
            this.projectileMoveSpeed = vitesse;
            isVisible = true;
            switch (direction)
            {
                case Direction.Up:
                    this.direction = "up";
                    break;
                case Direction.Down:
                    this.direction = "down";
                    break;
                case Direction.Left:
                    this.direction = "left";
                    break;
                case Direction.Right:
                    this.direction = "right";
                    break;
            }
            this.update = true;
            this.init = 0;

            this.dispertion = ((float)((seed * seed) % dispertion) - dispertion / 2) / 180d * Math.PI;
        }


        // METHODS
        public void collision_entite_balle(List<IA> liste_ia) //S'occupe de la collision des balles avec les IA
        {
            Rectangle rectangle_ = this.rectangle();
            bool test = true;
            foreach (IA ia1 in liste_ia) //Vérifie pour chaque IA
            {
                if ((test)) //Permet de casser la boucle dès qu'une IA est touché
                {
                    if (rectangle_.Intersects(ia1.rectangle())) //Si la HitBox du projectile est en contact avec celle de l'IA, alors (...)
                    {
                        this.isVisible = false; //La balle n'existe plus
                        test = false; //On casse le si
                        ia1.Health = ia1.Health - this.Damage; //On applique les dégats à l'IA
                    }
                }
            }
        }

        public void collision_balle(PhysicsEngine map_physique)
        {
            if (!(map_physique.mur_projectile((int)ProjectilePosition.X, (int)ProjectilePosition.Y)))
            {
                this.update_coordonne();
            }
            else
            {
                this.isVisible = false;
            }
        } //S'occupe de la collision des balles avec les murs

        public void update_coordonne()
        {
            if (this.direction == "right")
            {
                ProjectilePosition.X += (float)Math.Cos(dispertion);
                ProjectilePosition.Y += (float)Math.Sin(dispertion);
            }
            else if (this.direction == "left")
            {
                ProjectilePosition.X -= (float)Math.Cos(dispertion);
                ProjectilePosition.Y += (float)Math.Sin(dispertion);
            }
            if (this.direction == "up")
            {
                ProjectilePosition.Y -= (float)Math.Cos(dispertion);
                ProjectilePosition.X += (float)Math.Sin(dispertion);
            }
            else if (this.direction == "down")
            {
                ProjectilePosition.Y += (float)Math.Cos(dispertion);
                ProjectilePosition.X += (float)Math.Sin(dispertion);
            }
        }

        //Renvois le futur rectangle du projectile
        public Rectangle rectangle()
        {
            return new Rectangle((int)ProjectilePosition.X, (int)ProjectilePosition.Y, 6, 6);
        }


        // UPDATE & DRAW
        public void Update(KeyboardState keyboard)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Rectangle Maptexture)
        {
            if (this.isVisible)
            {
                spriteBatch.Draw(Ressources.Projectile, new Vector2(Maptexture.X + ProjectilePosition.X, Maptexture.Y + ProjectilePosition.Y), Color.White);
            }
        }
    }
}

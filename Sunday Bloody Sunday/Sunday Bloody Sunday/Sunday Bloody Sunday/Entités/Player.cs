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
    public enum Direction
    {
        Up, Down, Left, Right
    };

    class Player
    {
        // FIELDS
        public Rectangle PlayerTexture;
        public Direction Direction;
        SpriteEffects Effect;
        // Relative Position
        static public Vector2 PlayerPosition;
        int frameLine;
        int frameColumn;
        bool Animation;
        int Timer;
        public int speed = 2;
        int AnimationSpeed = 10;
        string action;
        // State of the player
        static public bool Active;
        // Amount of health
        public int Health;
        // Amount of munition that player has
        public int Ammo;
        // The amount of damage the Player can inflict to the IA
        static public int Damage;

        public Keys Haut;
        public Keys Bas;
        public Keys Gauche;
        public Keys Droite;
        public Keys Tire;
        public Keys PoserBomb;
        public Keys ActiverBomb;
        public Keys PoserTurret;
        public Keys ActiverPlane;
        public Keys ChangerArme;

        public bool haut;
        public bool droite;
        public bool gauche;
        public bool bas;
        public bool tir;
        public bool poserBomb;
        public bool poserTurret;
        public bool activerBomb;
        public bool activerPlane;
        public bool changerArme;

        public bool old_changerArme = false;

        public int refroidissement = 0;

        public int bomb;
        public int turret;
        public bool jetpack;

        public Texture2D texture;

        public bool est_afficher = false;

        int compteur = 0;
        public int timer = 0;

        public int id_joueur;

        List<Arme> Equipement_Arme = new List<Arme>();
        int CurrentWeapon = 0;


        // CONSTRUCTOR
        public Player(Keys Haut, Keys Bas, Keys Gauche, Keys Droite, Keys Tire, Keys poserBomb, Keys activerBomb, Keys poserTurret, Keys activerPlane, Keys changerArme, Texture2D texture, int x, int y, int id_joueur)
        {
            this.PlayerTexture = new Rectangle(x, y, 16, 19);
            Player.PlayerPosition = new Vector2(PlayerTexture.X, PlayerTexture.Y);
            this.frameLine = 1;
            this.frameColumn = 2;
            this.Direction = Direction.Down;
            this.Effect = SpriteEffects.None;
            this.Animation = true;
            this.Timer = 0;
            Player.Active = true;
            this.Health = 100;
            this.Ammo = 100;
            Player.Damage = 10;
            this.action = "";

            this.Haut = Haut;
            this.Bas = Bas;
            this.Gauche = Gauche;
            this.Droite = Droite;
            this.Tire = Tire;
            this.PoserBomb = poserBomb;
            this.ActiverBomb = activerBomb;
            this.PoserTurret = poserTurret;
            this.ActiverPlane = activerPlane;
            this.ChangerArme = changerArme;

            this.texture = texture;
            bomb = 1;
            turret = 1;
            jetpack = false;

            this.id_joueur = id_joueur;

            Equipement_Arme.Add(new Arme(TypeArme.Pompe));
            Equipement_Arme.Add(new Arme(TypeArme.Fusil));
        }


        // METHODS
        // Get the width of the player
        public int Width
        {
            get { return PlayerTexture.Width; }
        }

        // Get the height of the player
        public int Height
        {
            get { return PlayerTexture.Height; }
        }

        public Vector2 centre()
        {
            Vector2 vector = new Vector2(PlayerTexture.X + Width / 2 - 3, PlayerTexture.Y + Height - 10);
            return vector;
        }

        public Arme Weapon()
        {
            return Equipement_Arme[CurrentWeapon % Equipement_Arme.Count];
        }


        public TypeArme currentWeapon()
        {
            return Equipement_Arme[CurrentWeapon % Equipement_Arme.Count].Type;
        }

        // Animation of the Player
        public void Animate()
        {
            this.Timer++;
            if (this.Timer == this.AnimationSpeed)
            {
                this.Timer = 0;
                if (this.Animation)
                {
                    this.frameColumn++;
                    if (this.frameColumn > 3)
                    {
                        this.frameColumn = 2;
                        this.Animation = false;
                    }
                }
                else
                {
                    this.frameColumn--;
                    if (this.frameColumn < 1)
                    {
                        this.frameColumn = 2;
                        this.Animation = true;
                    }
                }
            }
        }

        public void action_hero(PhysicsEngine map_physique, List<IA> liste_ia, List<DestructibleItems> liste_barrel)
        {
            Rectangle rectangle_ = rectangle();
            foreach (DestructibleItems barrel in liste_barrel)
            {
                if (barrel.Aire_barrel.Intersects(rectangle_) && barrel.type == "barrel")
                {
                    this.actionjoueur = "";
                }
            }
            this.maj_direction(this.actionjoueur);

            if (jetpack)
            {
                if (this.actionjoueur == "up" || this.actionjoueur == "down" || this.actionjoueur == "left" || this.actionjoueur == "right")
                {
                    if (this.actionjoueur == "up")
                    {
                        if (!(map_physique.mur(this.futur_position_X_gauche(), this.futur_position_Y_haut()))
                         && !(map_physique.mur(this.futur_position_X_droite(), this.futur_position_Y_haut())))
                            this.mise_a_jour(this.actionjoueur);
                        this.actionjoueur = "";
                    }

                    if (this.actionjoueur == "down")
                    {
                        if (!(map_physique.mur(this.futur_position_X_gauche(), this.futur_position_Y_bas()))
                         && !(map_physique.mur(this.futur_position_X_droite(), this.futur_position_Y_bas())))
                            this.mise_a_jour(this.actionjoueur);
                        this.actionjoueur = "";
                    }

                    if (this.actionjoueur == "left")
                    {
                        if (!(map_physique.mur(this.futur_position_X_gauche(), this.futur_position_Y_haut()))
                         && !(map_physique.mur(this.futur_position_X_gauche(), this.futur_position_Y_bas())))
                            this.mise_a_jour(this.actionjoueur);
                        this.actionjoueur = "";
                    }

                    if (this.actionjoueur == "right")
                    {
                        if (!(map_physique.mur(this.futur_position_X_droite(), this.futur_position_Y_haut()))
                         && !(map_physique.mur(this.futur_position_X_droite(), this.futur_position_Y_bas())))
                            this.mise_a_jour(this.actionjoueur);
                        this.actionjoueur = "";
                    }
                }
                this.actionjoueur = ""; //"Remet à zéros" les actions du joueurs
            }
            else
            {
                if (this.actionjoueur == "up" || this.actionjoueur == "down" || this.actionjoueur == "left" || this.actionjoueur == "right")
                {
                    if (this.actionjoueur == "up")
                    {
                        if (!(map_physique.mur(this.futur_position_X_gauche(), this.futur_position_Y_haut()))
                         && !(map_physique.mur(this.futur_position_X_droite(), this.futur_position_Y_haut())) && this.collision_entite_hero(liste_ia))
                            this.mise_a_jour(this.actionjoueur);
                        this.actionjoueur = "";
                    }

                    if (this.actionjoueur == "down")
                    {
                        if (!(map_physique.mur(this.futur_position_X_gauche(), this.futur_position_Y_bas()))
                         && !(map_physique.mur(this.futur_position_X_droite(), this.futur_position_Y_bas())) && this.collision_entite_hero(liste_ia))
                            this.mise_a_jour(this.actionjoueur);
                        this.actionjoueur = "";
                    }

                    if (this.actionjoueur == "left")
                    {
                        if (!(map_physique.mur(this.futur_position_X_gauche(), this.futur_position_Y_haut()))
                         && !(map_physique.mur(this.futur_position_X_gauche(), this.futur_position_Y_bas())) && this.collision_entite_hero(liste_ia))
                            this.mise_a_jour(this.actionjoueur);
                        this.actionjoueur = "";
                    }

                    if (this.actionjoueur == "right")
                    {
                        if (!(map_physique.mur(this.futur_position_X_droite(), this.futur_position_Y_haut()))
                         && !(map_physique.mur(this.futur_position_X_droite(), this.futur_position_Y_bas())) && this.collision_entite_hero(liste_ia))
                            this.mise_a_jour(this.actionjoueur);
                        this.actionjoueur = "";
                    }
                }
                this.actionjoueur = ""; //"Remet à zéros" les actions du joueurs
            }
        }

        public bool collision_entite_hero(List<IA> liste_ia)
        {
            Rectangle rectangle_ = rectangle();
            bool test = true;
            foreach (IA ia in liste_ia)
            {
                if (test)
                {
                    test = !rectangle_.Intersects(ia.rectangle()); //Teste l'intersection entre un héros (parametre) et les IA(foreach) à l'aide de rectangle
                }
            }
            return test;
        }

        // Concerne l'action en cours du joueurs, permet d'y accéder et de la modifier
        public string actionjoueur
        {
            get { return this.action; }
            set { this.action = value; }
        }

        // Renvois la futur position X du joueur en cas d'un déplacement, à l'aide de l'action qui lui est attribuée
        public int futur_position_X_gauche()
        {
            if (actionjoueur == "left")
                return (this.PlayerTexture.X - this.speed + 1);
            else
                return (this.PlayerTexture.X + 1);
        }

        public int futur_position_X_droite()
        {
            if (actionjoueur == "right")
                return (this.PlayerTexture.X + this.speed + 16 - 1);
            else
                return (this.PlayerTexture.X + 16 - 1);
        }

        // Renvois la futur position Y du joueur en cas d'un déplacement, à l'aide de l'action qui lui est attribuée
        public int futur_position_Y_haut()
        {
            if (actionjoueur == "up")
                return (this.PlayerTexture.Y - this.speed + 1 + 10);
            else
                return (this.PlayerTexture.Y + 1 + 10);
        }

        public int futur_position_Y_bas()
        {
            if (actionjoueur == "down")
                return (this.PlayerTexture.Y + this.speed - 1 + 19);
            else
                return (this.PlayerTexture.Y - 1 + 19);
        }

        public void maj_direction(string a)
        {
            if (a == "up")
            {
                this.Direction = Direction.Up;
                this.Animate();
            }
            else if (a == "down")
            {
                this.Direction = Direction.Down;
                this.Animate();
            }
            else if (a == "right")
            {
                this.Direction = Direction.Right;
                this.Animate();
            }
            else if (a == "left")
            {
                this.Direction = Direction.Left;
                this.Animate();
            }
        }


        // Met à jour le héros en fontion de l'action qui lui est donné, pour l'instant, seul le déplacement est géré
        public void mise_a_jour(string a)
        {
            if (a == "up")
            {
                this.PlayerTexture.Y -= this.speed;
                this.Direction = Direction.Up;
                this.Animate();
            }
            else if (a == "down")
            {
                this.PlayerTexture.Y += this.speed;
                this.Direction = Direction.Down;
                this.Animate();
            }
            else if (a == "right")
            {
                this.PlayerTexture.X += this.speed;
                this.Direction = Direction.Right;
                this.Animate();
            }
            else if (a == "left")
            {
                this.PlayerTexture.X -= this.speed;
                this.Direction = Direction.Left;
                this.Animate();
            }
            speed = 2;
        }

        //Renvois le futur rectangle du joueur
        public Rectangle rectangle()
        {
            return new Rectangle(futur_position_X_gauche(), futur_position_Y_haut(), futur_position_X_droite() - futur_position_X_gauche(), futur_position_Y_bas() - futur_position_Y_haut());
        }


        // UPDATE & DRAW
        public void Update(MouseState mouse/*, KeyboardState keyboard*/, List<Keys> liste)
        {/*
            haut = keyboard.IsKeyDown(Haut);
            droite = keyboard.IsKeyDown(Droite);
            gauche = keyboard.IsKeyDown(Gauche);
            bas = keyboard.IsKeyDown(Bas);
            tir = keyboard.IsKeyDown(Tire);
            poserBomb = keyboard.IsKeyDown(this.PoserBomb);
            activerBomb = keyboard.IsKeyDown(this.ActiverBomb);
            poserTurret = keyboard.IsKeyDown(this.PoserTurret);
            activerPlane = keyboard.IsKeyDown(this.ActiverPlane);*/
            haut = false;
            droite = false;
            gauche = false;
            bas = false;
            tir = false;
            poserBomb = false;
            activerBomb = false;
            poserTurret = false;
            activerPlane = false;
            changerArme = false;
            try
            {
                foreach (Keys key in liste)
                {
                    if (key == Haut)
                    {
                        speed = 2;
                        haut = true;
                    }
                    else if (key == Bas)
                    {
                        speed = 2;
                        bas = true;
                    }
                    else if (key == Droite)
                    {
                        speed = 2;
                        droite = true;
                    }
                    else if (key == Gauche)
                    {
                        speed = 2;
                        gauche = true;
                    }
                    else if (key == Tire)
                    {
                        speed = 2;
                        tir = true;
                    }
                    else if (key == ActiverBomb)
                    {
                        speed = 2;
                        activerBomb = true;
                    }
                    else if (key == PoserBomb)
                    {
                        speed = 2;
                        poserBomb = true;
                    }
                    else if (key == PoserTurret)
                    {
                        speed = 2;
                        poserTurret = true;
                    }
                    else if (key == ActiverPlane)
                    {
                        speed = 2;
                        activerPlane = true;
                    }
                    else if (key == ChangerArme)
                    {
                        changerArme = true;
                        if (!old_changerArme)
                            CurrentWeapon++;
                    }
                    old_changerArme = changerArme;
                }
            }
            catch { }
            // Détermine les actions du joueur en fonction des retours claviers
            if ((haut))
            {
                this.action = "up";
                compteur = 0;
            }
            else if ((bas))
            {
                this.action = "down";
                compteur = 0;
            }
            else if ((droite))
            {

                this.action = "right";
                compteur = 0;
            }
            else if ((gauche))
            {

                this.action = "left";
                compteur = 0;
            }
            /*
            if((keyboard.IsKeyUp(Haut) && keyboard.IsKeyUp(Bas) && keyboard.IsKeyUp(Gauche) && keyboard.IsKeyUp(Droite)) && this.compteur > 2)
            {
                this.frameColumn = 2;
                this.Timer = 0;
            }
            else if ((keyboard.IsKeyUp(Haut) && keyboard.IsKeyUp(Bas) && keyboard.IsKeyUp(Gauche) && keyboard.IsKeyUp(Droite)))
            {
                compteur++;
            }*/

            if (haut && bas && gauche && droite && this.compteur > 2)
            {
                this.frameColumn = 2;
                this.Timer = 0;
            }
            else if ((haut && bas && gauche && droite))
            {
                compteur++;
            }

            if (timer > 0)
            {
                jetpack = true;
                speed = 4;
                if (Direction == Direction.Up)
                {
                    PlayerTexture.Y = PlayerTexture.Y - 1;
                }
                else if (Direction == Direction.Down)
                {
                    PlayerTexture.Y = PlayerTexture.Y + 1;
                }
                else if (Direction == Direction.Right)
                {
                    PlayerTexture.X = PlayerTexture.X + 1;
                }
                else
                {
                    PlayerTexture.X = PlayerTexture.X - 1;
                }
                timer--;
            }
            else
            {
                speed = 2;
                jetpack = false;
            }

            switch (this.Direction)
            {
                case Direction.Up: this.frameLine = 2;
                    this.Effect = SpriteEffects.None;
                    break;
                case Direction.Down: this.frameLine = 1;
                    this.Effect = SpriteEffects.None;
                    break;
                case Direction.Left: this.frameLine = 3;
                    this.Effect = SpriteEffects.None;
                    break;
                case Direction.Right: this.frameLine = 3;
                    this.Effect = SpriteEffects.FlipHorizontally;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle Maptexture)
        {
            if (jetpack)
            {
                spriteBatch.Draw(Ressources.Player1_jetpack, new Rectangle(Maptexture.X + PlayerTexture.X, Maptexture.Y + PlayerTexture.Y, PlayerTexture.Width, PlayerTexture.Height), new Rectangle((this.frameColumn - 1) * 16, (this.frameLine - 1) * 19, 16, 19), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
            }
            else
            {
                spriteBatch.Draw(this.texture, new Rectangle(Maptexture.X + PlayerTexture.X, Maptexture.Y + PlayerTexture.Y, PlayerTexture.Width, PlayerTexture.Height), new Rectangle((this.frameColumn - 1) * 16, (this.frameLine - 1) * 19, 16, 19), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
            }
        }
    }
}

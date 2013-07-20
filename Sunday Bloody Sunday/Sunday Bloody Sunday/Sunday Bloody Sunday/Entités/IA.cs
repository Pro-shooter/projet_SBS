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
    public enum DirectionIA
    {
        Up, Down, Left, Right
    };

    class Spawn
    {
        public List<IA> créatures;

        public Spawn(List<IA> créatures)
        {
            this.créatures = créatures;
        }
    }

    class IA
    {
        // FIELDS
        public Rectangle IATexture;
        public Vector2 VecteurAttaque;
        public Rectangle Aire_attaque;
        DirectionIA Direction;
        SpriteEffects Effect;
        // Relative Position
        static public Vector2 IAPosition;
        int frameLine;
        int frameColumn;
        bool Animation;
        int Timer;
        int speed = 1;
        int AnimationSpeed = 10;
        public string action;
        // State of the IA
        public bool Active;
        // Amount of health
        public int Health;
        // The amount of damage the IA can inflict to the Player
        public int Damage;
        public bool est_update = false;
        public bool en_vie = true;

        public int id_texture;
        public int id_son;

        public int compteur_path;
        public int ia_dir;


        public bool IA_boss = false;
        public bool ia_vol;

        public int couldown = 60; //Temps d'attente entre chaque attaque
        public int couldown_attaque_spéciale;

        // CONSTRUCTOR
        public IA(int x, int y, int id_texture, int id_son, int pv_max, int dégats)//ID texture, son joué, PV max, Vitesse
        {
            compteur_path = 0;
            ia_dir = 0;
            ia_vol = false;
            this.frameLine = 1;
            this.frameColumn = 2;
            this.Direction = DirectionIA.Down;
            this.Effect = SpriteEffects.None;
            this.Animation = true;
            this.Timer = 0;
            this.Active = true;
            this.Health = pv_max;
            this.Damage = dégats;
            this.id_son = id_son;
            this.action = "";
            this.id_texture = id_texture;
            if (id_texture == 0)//Pika
            {
                this.IATexture = new Rectangle(x, y, 16, 17);
                this.VecteurAttaque = new Vector2(30, 25);
                IA.IAPosition = new Vector2(IATexture.X, IATexture.Y);
            }
            else if (id_texture == 1)//Pika
            {
                this.IATexture = new Rectangle(x, y, 16, 17);
                this.VecteurAttaque = new Vector2(20, 25);
                IA.IAPosition = new Vector2(IATexture.X, IATexture.Y);
            }
            else if (id_texture == 2)//Carabaffe
            {
                this.IATexture = new Rectangle(x, y, 20, 20);
                this.VecteurAttaque = new Vector2(26, 23);
                IA.IAPosition = new Vector2(IATexture.X, IATexture.Y);
                this.Health = 200;
                this.Damage = 5;
            }
            else if (id_texture == 3)//Spectrum
            {
                ia_vol = true;
                this.IATexture = new Rectangle(x, y, 20, 20);
                IA.IAPosition = new Vector2(IATexture.X, IATexture.Y);
                this.VecteurAttaque = new Vector2(41, 51);
                this.Health = 75;
                this.speed = 2;
            }
            else if (id_texture == 4)//Raichu
            {
                this.IATexture = new Rectangle(x, y, 20, 20);
                IA.IAPosition = new Vector2(IATexture.X, IATexture.Y);
                this.VecteurAttaque = new Vector2(30, 29);
                this.Damage = 20;
                this.Health = 150;
            }
            else if (id_texture == 5)//Tortank
            {
                this.IATexture = new Rectangle(x, y, 26, 24);
                IA.IAPosition = new Vector2(IATexture.X, IATexture.Y);
                this.VecteurAttaque = new Vector2(34, 29);
            }
            else if (id_texture == 6)//Electhor
            {
                this.IATexture = new Rectangle(x, y, 40, 40);
                this.AnimationSpeed = 7;
                this.VecteurAttaque = new Vector2(45, 47);
                this.Health = 10000;
                this.speed = 1;
                this.IA_boss = true;
                couldown_attaque_spéciale = 60;
            }
            else if (id_texture == 7)//Sulfura
            {
                this.IATexture = new Rectangle(x, y, 70, 50);
                this.AnimationSpeed = 7;
                this.VecteurAttaque = new Vector2(45, 47);
                this.Health = 10000;
                this.speed = 1;
                this.IA_boss = true;
                couldown_attaque_spéciale = 120;
            }
            else if (id_texture == 8)//Artikodin
            {
                this.IATexture = new Rectangle(x, y, 60, 55);
                this.AnimationSpeed = 7;
                this.VecteurAttaque = new Vector2(45, 47);
                this.Health = 10000;
                this.speed = 1;
                this.IA_boss = true;
                couldown_attaque_spéciale = 300;
            }
            else if (id_texture == 9)//Morse
            {
                this.IATexture = new Rectangle(x, y, 30, 30);
                IA.IAPosition = new Vector2(IATexture.X, IATexture.Y);
                this.VecteurAttaque = new Vector2(34, 32);
                this.Damage = 10;
                this.Health = 200;
            }
            else if (id_texture == 10)//Locklass
            {
                this.IATexture = new Rectangle(x, y, 30, 30);
                this.AnimationSpeed = 7;
                this.VecteurAttaque = new Vector2(45, 47);//?
                this.Damage = 20;
                this.Health = 125;
            }
            else if (id_texture == 11)//Bouboule
            {
                this.IATexture = new Rectangle(x, y, 20, 20);
                this.AnimationSpeed = 7;
                this.VecteurAttaque = new Vector2(45, 47);//?
                this.Damage = 20;
                this.Health = 125;
                this.ia_vol = true;
            }
            else if (id_texture == 12)//Lamentin
            {
                this.IATexture = new Rectangle(x, y, 20, 20);
                this.AnimationSpeed = 7;
                this.VecteurAttaque = new Vector2(45, 47);//?
                this.Damage = 1;
                this.Health = 100;
                this.ia_vol = true;
            }
            else if (id_texture == 13)//slugma
            {
                this.IATexture = new Rectangle(x, y, 24, 24);
                this.AnimationSpeed = 7;
                this.VecteurAttaque = new Vector2(45, 47);//?
                this.Damage = 10;
                this.Health = 200;
            }
            else if (id_texture == 14)//ponyta
            {
                this.speed = 2;
                this.IATexture = new Rectangle(x, y, 25, 25);
                this.AnimationSpeed = 7;
                this.VecteurAttaque = new Vector2(45, 47);//?
                this.Damage = 10;
                this.Health = 100;
            }
            this.Aire_attaque = new Rectangle(IATexture.X - 1, IATexture.Y - 1, IATexture.Width + 1, IATexture.Height + 1);
        }


        // METHODS
        // Get the width of the IA
        public int Width
        {
            get { return IATexture.Width; }
        }
        // Get the height of the IA
        public int Height
        {
            get { return IATexture.Height; }
        }

        // Animation of the IA
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

        public void attaque_ia(List<Player> joueurs)
        {
            foreach (Player joueur in joueurs)
            {
                if (this.couldown >= 60)
                {
                    if (this.Aire_attaque.Intersects(joueur.PlayerTexture) && !joueur.jetpack)
                    {
                        joueur.Health = joueur.Health - this.Damage;
                        this.couldown = 0;
                    }
                }
            }
            this.couldown++;
        }

        // Concerne l'action en cours de l'IA
        public string actionIA
        {
            get { return this.action; }
            set { this.action = value; }
        }

        public bool collision_entite_ia(IA ia, List<IA> liste_ia, List<Player> liste_joueurs)
        {
            Rectangle futur_rectangle = ia.rectangle();
            bool test = true;
            foreach (IA ia1 in liste_ia)
            {
                if ((test) && !(ia1.est_update))
                {
                    test = !futur_rectangle.Intersects(ia1.rectangle()); //Teste l'intersection entre une IA (parametre) et les autres (foreach) à l'aide de rectangle
                }
            }
            foreach (Player joueur in liste_joueurs)
            {
                if (test)
                {
                    test = !futur_rectangle.Intersects(joueur.rectangle()); //Teste l'intersection entre une IA (parametre) et les héros (foreach) à l'aide de rectangle
                }
            }
            return test;
        }

        public void action_ia(IA ia, Player joueur, List<DestructibleItems> liste_barrel, PhysicsEngine map_physique, List<IA> liste_ia, List<Player> liste_joueur)
        {
            foreach (DestructibleItems barrel in liste_barrel)
            {
                if (barrel.Aire_barrel.Intersects(ia.rectangle()) && barrel.type == "barrel")
                {
                    ia.actionIA = "";
                }
            }

            if (ia.actionIA == "up" || ia.actionIA == "down" || ia.actionIA == "left" || ia.actionIA == "right")
            {
                if (!ia.ia_vol)
                {
                    if (ia.actionIA == "up")
                    {
                        if (!(map_physique.mur(ia.futur_position_X_gauche(), ia.futur_position_Y_haut()))
                         && !(map_physique.mur(ia.futur_position_X_droite(), ia.futur_position_Y_haut())) && collision_entite_ia(ia, liste_ia, liste_joueur))
                            ia.mise_a_jour(ia.actionIA);
                        else
                            ia.actionIA = "";
                    }

                    if (ia.actionIA == "down")
                    {
                        if (!(map_physique.mur(ia.futur_position_X_gauche(), ia.futur_position_Y_bas()))
                         && !(map_physique.mur(ia.futur_position_X_droite(), ia.futur_position_Y_bas())) && collision_entite_ia(ia, liste_ia, liste_joueur))
                            ia.mise_a_jour(ia.actionIA);
                        else
                            ia.actionIA = "";
                    }

                    if (ia.actionIA == "left")
                    {
                        if (!(map_physique.mur(ia.futur_position_X_gauche(), ia.futur_position_Y_haut()))
                         && !(map_physique.mur(ia.futur_position_X_gauche(), ia.futur_position_Y_bas())) && collision_entite_ia(ia, liste_ia, liste_joueur))
                            ia.mise_a_jour(ia.actionIA);
                        else
                            ia.actionIA = "";
                    }

                    if (ia.actionIA == "right")
                    {
                        if (!(map_physique.mur(ia.futur_position_X_droite(), ia.futur_position_Y_haut()))
                         && !(map_physique.mur(ia.futur_position_X_droite(), ia.futur_position_Y_bas())) && collision_entite_ia(ia, liste_ia, liste_joueur))
                            ia.mise_a_jour(ia.actionIA);
                        else
                            ia.actionIA = "";
                    }
                }
                else
                {
                    if (ia.actionIA == "up")
                    {
                        if (!(map_physique.mur_projectile(ia.futur_position_X_gauche(), ia.futur_position_Y_haut()))
                         && !(map_physique.mur_projectile(ia.futur_position_X_droite(), ia.futur_position_Y_haut())) && collision_entite_ia(ia, liste_ia, liste_joueur))
                            ia.mise_a_jour(ia.actionIA);
                        else
                            ia.actionIA = "";
                    }

                    if (ia.actionIA == "down")
                    {
                        if (!(map_physique.mur_projectile(ia.futur_position_X_gauche(), ia.futur_position_Y_bas()))
                         && !(map_physique.mur_projectile(ia.futur_position_X_droite(), ia.futur_position_Y_bas())) && collision_entite_ia(ia, liste_ia, liste_joueur))
                            ia.mise_a_jour(ia.actionIA);
                        else
                            ia.actionIA = "";
                    }

                    if (ia.actionIA == "left")
                    {
                        if (!(map_physique.mur_projectile(ia.futur_position_X_gauche(), ia.futur_position_Y_haut()))
                         && !(map_physique.mur_projectile(ia.futur_position_X_gauche(), ia.futur_position_Y_bas())) && collision_entite_ia(ia, liste_ia, liste_joueur))
                            ia.mise_a_jour(ia.actionIA);
                        else
                            ia.actionIA = "";

                    }

                    if (ia.actionIA == "right")
                    {
                        if (!(map_physique.mur_projectile(ia.futur_position_X_droite(), ia.futur_position_Y_haut()))
                         && !(map_physique.mur_projectile(ia.futur_position_X_droite(), ia.futur_position_Y_bas())) && collision_entite_ia(ia, liste_ia, liste_joueur))
                            ia.mise_a_jour(ia.actionIA);
                        else
                            ia.actionIA = "";
                    }
                }
            }

            ia.actionIA = ""; //"Remet à zéros" les actions de l'IA
        }

        // Renvois la futur position X de l'IA en cas d'un déplacement, à l'aide de l'action qui lui est attribuée
        public int futur_position_X_gauche()
        {
            if (id_texture == 6 || id_texture == 7 || id_texture == 8)
            {
                speed = couldown_attaque_spéciale % 2 + 1;
            }
            if (actionIA == "left")
                return (this.IATexture.X - this.speed);
            else
                return (this.IATexture.X);
        }

        public int futur_position_X_droite()
        {
            if (id_texture == 6 || id_texture == 7 || id_texture == 8)
            {
                speed = couldown_attaque_spéciale % 2 + 1;
            }
            if (actionIA == "right")
                return (this.IATexture.X + this.speed + this.IATexture.Width + 1);
            else
                return (this.IATexture.X + this.IATexture.Width + 1);
        }

        // Renvois la futur position Y de l'IA en cas d'un déplacement, à l'aide de l'action qui lui est attribuée
        public int futur_position_Y_haut()
        {
            if (id_texture == 6 || id_texture == 7 || id_texture == 8)
            {
                speed = couldown_attaque_spéciale % 2 + 1;
            }
            if (actionIA == "up")
                return (this.IATexture.Y - this.speed + 4);
            else
                return (this.IATexture.Y + 4);
        }

        public int futur_position_Y_bas()
        {
            if (id_texture == 6 || id_texture == 7 || id_texture == 8)
            {
                speed = couldown_attaque_spéciale % 2 + 1;
            }
            if (actionIA == "down")
                return (this.IATexture.Y + this.speed + this.IATexture.Height - 1);
            else
                return (this.IATexture.Y + this.IATexture.Height - 1);
        }

        // Met à jour l'IA en fontion de l'action qui lui est donné, pour l'instant, seul le déplacement est géré
        public void mise_a_jour(string a)
        {
            if (id_texture == 6 || id_texture == 7 || id_texture == 8)
            {
                speed = couldown_attaque_spéciale % 2 + 1;
            }
            if (a == "up")
            {
                this.IATexture.Y -= this.speed;
                this.Direction = DirectionIA.Up;
                this.Animate();
            }
            else if (a == "down")
            {
                this.IATexture.Y += this.speed;
                this.Direction = DirectionIA.Down;
                this.Animate();
            }
            else if (a == "right")
            {
                this.IATexture.X += this.speed;
                this.Direction = DirectionIA.Right;
                this.Animate();
            }
            else if (a == "left")
            {
                this.IATexture.X -= this.speed;
                this.Direction = DirectionIA.Left;
                this.Animate();
            }
            this.Aire_attaque = new Rectangle(IATexture.X - 1, IATexture.Y - 1, IATexture.Width + 2, IATexture.Height + 2);
        }

        //Renvois le futur rectangle de l'IA
        public Rectangle rectangle()
        {
            return new Rectangle(futur_position_X_gauche(), futur_position_Y_haut(), futur_position_X_droite() - futur_position_X_gauche(), futur_position_Y_bas() - futur_position_Y_haut());
        }


        // UPDATE & DRAW
        public void Update()
        {
            switch (this.Direction)
            {
                case DirectionIA.Up: this.frameLine = 2;
                    this.Effect = SpriteEffects.None;
                    break;
                case DirectionIA.Down: this.frameLine = 1;
                    this.Effect = SpriteEffects.None;
                    break;
                case DirectionIA.Left: this.frameLine = 3;
                    this.Effect = SpriteEffects.None;
                    break;
                case DirectionIA.Right: this.frameLine = 3;
                    this.Effect = SpriteEffects.FlipHorizontally;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle Maptexture)
        {
            if (id_texture == 0)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA1attack, new Rectangle(Maptexture.X + IATexture.X - 2, Maptexture.Y + IATexture.Y - 2, IATexture.Width + 2, IATexture.Width + 2), new Rectangle(0 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA1attack, new Rectangle(Maptexture.X + IATexture.X - 2, Maptexture.Y + IATexture.Y - 2, IATexture.Width + 2, IATexture.Width + 2), new Rectangle(1 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA1, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 23, (this.frameLine - 1) * 27, 23, 27), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 1)
            {
                {
                    spriteBatch.Draw(Ressources.IA2, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 23, (this.frameLine - 1) * 27, 23, 27), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 2)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA3attack, new Rectangle(Maptexture.X + IATexture.X - 2, Maptexture.Y + IATexture.Y - 2, IATexture.Width + 2, IATexture.Width + 2), new Rectangle(0 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA3attack, new Rectangle(Maptexture.X + IATexture.X - 2, Maptexture.Y + IATexture.Y - 2, IATexture.Width + 2, IATexture.Width + 2), new Rectangle(1 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA3, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 24, (this.frameLine - 1) * 24, 24, 24), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 3)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA4attack, new Rectangle(Maptexture.X + IATexture.X - 15, Maptexture.Y + IATexture.Y - 20, IATexture.Width + 15, IATexture.Width + 20), new Rectangle(0 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA4attack, new Rectangle(Maptexture.X + IATexture.X - 15, Maptexture.Y + IATexture.Y - 20, IATexture.Width + 15, IATexture.Width + 20), new Rectangle(1 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA4, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 26, (this.frameLine - 1) * 30, 26, 30), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 4)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA5attack, new Rectangle(Maptexture.X + IATexture.X - 2, Maptexture.Y + IATexture.Y - 2, IATexture.Width + 2, IATexture.Width + 2), new Rectangle(0 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA5attack, new Rectangle(Maptexture.X + IATexture.X - 2, Maptexture.Y + IATexture.Y - 2, IATexture.Width + 2, IATexture.Width + 2), new Rectangle(1 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA5, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 30, (this.frameLine - 1) * 29, 30, 29), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 5)
            {
                {
                    if (couldown < 10)
                    {
                        spriteBatch.Draw(Ressources.IA6attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle(0 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                    }
                    else if (couldown < 20)
                    {
                        spriteBatch.Draw(Ressources.IA6attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle(1 * (int)VecteurAttaque.X, (this.frameLine - 1) * (int)VecteurAttaque.Y, (int)VecteurAttaque.X, (int)VecteurAttaque.Y), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                    }
                    else
                    {
                        spriteBatch.Draw(Ressources.IA6, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 27, (this.frameLine - 1) * 29, 27, 29), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                    }
                }
            }
            else if (id_texture == 6)
            {
                {
                    spriteBatch.Draw(Ressources.IA7, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 45, (this.frameLine - 1) * 47, 45, 47), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 7)
            {
                {
                    spriteBatch.Draw(Ressources.IA11, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 79, (this.frameLine - 1) * 55, 79, 55), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 8)
            {
                {
                    spriteBatch.Draw(Ressources.IA10, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 65, (this.frameLine - 1) * 56, 65, 56), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 9)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA8attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y , IATexture.Width , IATexture.Width), new Rectangle((this.frameColumn - 1) * 32, (this.frameLine - 1) * 34, 32, 34), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA8attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y , IATexture.Width , IATexture.Width), new Rectangle((this.frameColumn - 1) * 32, (this.frameLine - 1) * 34, 32, 34), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA8, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y , IATexture.Width, IATexture.Width ), new Rectangle((this.frameColumn - 1) * 32, (this.frameLine - 1) * 32, 32, 32), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 10)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA9attack, new Rectangle(Maptexture.X + IATexture.X , Maptexture.Y + IATexture.Y , IATexture.Width , IATexture.Width ), new Rectangle((this.frameColumn - 1) * 47, (this.frameLine - 1) * 45, 47, 45), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA9attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y , IATexture.Width , IATexture.Width ), new Rectangle((this.frameColumn - 1) * 47, (this.frameLine - 1) * 45, 47, 45), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA9, new Rectangle(Maptexture.X + IATexture.X , Maptexture.Y + IATexture.Y , IATexture.Width , IATexture.Width ), new Rectangle((this.frameColumn - 1) * 43, (this.frameLine - 1) * 40, 43, 40), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 11)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA13attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y , IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 30, (this.frameLine - 1) * 27, 30, 27), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA13attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y , IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 30, (this.frameLine - 1) * 27, 30, 27), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA13, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y , IATexture.Width , IATexture.Width), new Rectangle((this.frameColumn - 1) * 28, (this.frameLine - 1) * 30, 28, 30), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 12)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA14attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 32, (this.frameLine - 1) * 26, 32, 26), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA14attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 32, (this.frameLine - 1) * 26, 32, 26), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA14, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 26, (this.frameLine - 1) * 24, 26, 24), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 13)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA15attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 30, (this.frameLine - 1) * 28, 30, 28), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA15attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 30, (this.frameLine - 1) * 28, 30, 28), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA15, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 32, (this.frameLine - 1) * 32, 32, 32), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
            else if (id_texture == 14)
            {
                if (couldown < 10)
                {
                    spriteBatch.Draw(Ressources.IA16attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 24, (this.frameLine - 1) * 26, 24, 26), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else if (couldown < 20)
                {
                    spriteBatch.Draw(Ressources.IA16attack, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 24, (this.frameLine - 1) * 26, 24, 26), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
                else
                {
                    spriteBatch.Draw(Ressources.IA16, new Rectangle(Maptexture.X + IATexture.X, Maptexture.Y + IATexture.Y, IATexture.Width, IATexture.Width), new Rectangle((this.frameColumn - 1) * 26, (this.frameLine - 1) * 27, 26, 27), Color.White, 0f, new Vector2(0, 0), this.Effect, 0f);
                }
            }
        }
    }
}
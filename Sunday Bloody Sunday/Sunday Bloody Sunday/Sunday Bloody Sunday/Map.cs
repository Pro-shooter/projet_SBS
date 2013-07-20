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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sunday_Bloody_Sunday
{
    class Map
    {
        int seed = 0;
        public bool connection = true;
        bool multi;
        string message = "";
        int avion = 1;

        // FIELDS
        Rectangle MapTexture;
        public List<Spawn> spawns;
        public Spawn_Items spawn_items;
        public Spawn_JetPack spawn_jetpacks;

        bool testc = true;

        public List<Items> liste_box; //Liste Items
        public List<Items> liste_box2; //Liste Items secondaire, utilisée pour nettoyer la mémoire

        public List<DestructibleItems> liste_barrel; //Liste barrels
        public List<DestructibleItems> liste_barrel2; //Liste barrels secondaire

        public List<ParticuleExplosion> liste_explosions; //Liste particules explosion
        public List<ParticuleExplosion> liste_explosions2; //Liste particules explosion secondaire
        public List<ParticuleExplosion> liste_explosions3;
        public List<ParticuleExplosion> liste_blood; //Liste particules sang
        public List<ParticuleExplosion> liste_blood2; //Liste particules sang secondaire

        public List<ParticuleRain> liste_rain; //Liste rain

        public List<Player> liste_joueurs = new List<Player>(); //Liste joueurs
        public List<Player> liste_joueurs2 = new List<Player>(); //Liste joueurs secondaire

        public List<IA> liste_ia; //Liste des IA         
        public List<IA> liste_ia2; //Liste IA secondaire

        public List<Keys> liste_clavier;
        public List<Keys> liste_clavier_2;

        public List<Projectile> liste_projectile = new List<Projectile>(); //Liste Projectiles
        public List<Projectile> liste_projectile2 = new List<Projectile>(); //Liste Projectiles secondaire

        public List<Turret> liste_turret; //Liste Turrets
        public List<Turret> liste_turret2; //Liste Turrets secondaire

        public List<Plane> liste_plane; //Liste Plane
        public List<Plane> liste_plane2; //Liste Plane secondaire

        public List<JetPack> liste_jetpack; //Liste JectPacks
        public List<JetPack> liste_jetpack2; //Liste JetPacks secondaire

        public List<KristboulFaille> liste_faille;
        public bool failleActive = false;

        public List<AnimationAttack> liste_attaque;
        public List<AnimationAttack> liste_attaque2;

        Projectile balle;
        IA ia;
        Texture2D explosionTexture, rainTexture;
        float spawnWidth = Divers.WidthScreen, density = 50, timer;
        PhysicsEngine map_physique;
        private Rectangle futur_rectangle; //Rectangle utilisé pour stocker des données
        int compteur = 0, compteur2 = 0, compteur3 = 0;
        Random rand0 = new Random();
        Random rand1 = new Random();
        Random rand2 = new Random();
        Sound moteur_son = new Sound();

        public bool game_over = false, gagne = false;
        public Param_Map parametre;
        public CheckPoint fin_niveau, boss_entry;
        public int compteur_kill = 0;

        int id_joueur = 0;
        int nb_joueur = 1;
        List<Keys>[] input = new List<Keys>[1];


        // CONSTRUCTOR
        public Map(Param_Map parametre, bool Multi, int joueur, IPAddress IP, int nb_joueur, int id_joueur)
        {
            this.parametre = parametre;

            MapTexture = new Rectangle(0, 0, parametre.hauteur * 16, (parametre.largeur - 1) * 16);
            this.map_physique = new PhysicsEngine(parametre.liste, parametre.liste_projectile);
            this.liste_ia = new List<IA>();

            if (Multi)
            {
                this.nb_joueur = nb_joueur;
                this.id_joueur = id_joueur;
            }
            else
                Reseau.nb_joueurs = 1;

            if (parametre.texture_map == 3)
            {
                Player p1 = new Player(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.N, Keys.P, Keys.Enter, Keys.T, Keys.F, Keys.F1, Ressources.Player1, parametre.x, parametre.y, 0);
                Player p2 = new Player(Keys.Z, Keys.S, Keys.Q, Keys.D, Keys.A, Keys.E, Keys.R, Keys.W, Keys.X, Keys.F1, Ressources.Player2, parametre.x, parametre.y, 1);
                p1.bomb = 5;
                p2.bomb = 5;
                liste_joueurs.Add(p1);
                liste_joueurs.Add(p2);
            }
            else
            {
                int i = 0;
                while (i < nb_joueur)
                {
                    this.liste_joueurs.Add(new Player(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.N, Keys.P, Keys.Enter, Keys.T, Keys.F, Keys.F1, Ressources.Player1, parametre.x, parametre.y, i));
                    i++;
                }
            }

            //HEALTH + AMMO BOXES
            this.liste_box = new List<Items>();
            this.liste_box2 = new List<Items>();

            //EXPLOSIVE BOXES
            this.liste_barrel = new List<DestructibleItems>();
            this.liste_barrel = parametre.liste_barrel;

            //EXPLOSION PARTICULE
            this.liste_explosions = new List<ParticuleExplosion>();
            this.liste_explosions2 = new List<ParticuleExplosion>();
            this.liste_explosions3 = new List<ParticuleExplosion>();
            this.explosionTexture = Ressources.ExplosionParticule;

            //BLOOD PARTICULE
            this.liste_blood = new List<ParticuleExplosion>();
            this.liste_blood2 = new List<ParticuleExplosion>();

            //TURRETS
            this.liste_turret = new List<Turret>();
            this.liste_turret2 = new List<Turret>();

            //PLANES
            this.liste_plane = new List<Plane>();
            this.liste_plane2 = new List<Plane>();

            //JETPACK
            this.liste_jetpack = new List<JetPack>();
            this.liste_jetpack2 = new List<JetPack>();
            this.spawn_jetpacks = parametre.liste_jetpacks;

            //FAILLE!
            this.liste_faille = new List<KristboulFaille>();

            //RAIN
            this.liste_rain = new List<ParticuleRain>();
            this.rainTexture = Ressources.mRain;

            this.spawns = parametre.liste_spawn;
            this.spawn_items = parametre.liste_caisses;

            this.liste_clavier = new List<Keys>();
            this.liste_clavier_2 = new List<Keys>();

            this.liste_attaque = new List<AnimationAttack>();
            this.liste_attaque2 = new List<AnimationAttack>();

            if (parametre.texture_map == 4)//Electhor
            {
                this.liste_ia = new List<IA>();
                liste_ia.Add(new IA(150, 150, 6, 0, 100, 10));
            }
            else if (parametre.texture_map == 9)//Artikodin
            {
                this.liste_ia = new List<IA>();
                liste_ia.Add(new IA(300, 300, 8, 0, 100, 10));
            }
            else if (parametre.texture_map == 15)//Sulfura
            {
                this.liste_ia = new List<IA>();
                liste_ia.Add(new IA(150, 150, 7, 0, 100, 10));
            }

            fin_niveau = parametre.checkpointArrivee;
            boss_entry = parametre.checkpointBossEntry;
            this.multi = Multi;
        }


        // METHODS
        //Gère le deplacement de l'ia
        public void pathfing(ref string action, Player joueur)
        {

            if (ia.compteur_path % 30 == 0)
            {
                ia.compteur_path = 0;
                Random rand = new Random();
                ia.ia_dir = rand.Next(0, 2);
            }

            ia.compteur_path++;

            if (ia.ia_dir == 0)
            {
                if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                {
                    action = "left";
                    if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                         && !(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                        {
                            action = "up";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                             && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                        {
                            action = "down";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                             && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                {
                    action = "right";
                    if (!(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                         && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        action = "up";
                        if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                        {

                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                             && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                        {
                            action = "down";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                             && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                {
                    action = "up";

                    if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                     && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                        {
                            action = "left";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                        {
                            action = "right";
                            if (!(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                {
                    action = "down";
                    if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                     && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                        {
                            action = "left";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                        {
                            action = "right";
                            if (!(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }

                    }
                }
            }
            else
            {

                if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                {
                    action = "up";

                    if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                     && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                        {
                            action = "left";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                        {
                            action = "right";
                            if (!(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                {
                    action = "down";
                    if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                     && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                        {
                            action = "left";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                        {
                            action = "right";
                            if (!(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }

                    }
                }
                else if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                {
                    action = "left";
                    if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                         && !(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                        {
                            action = "up";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                             && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                        {
                            action = "down";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                             && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                {
                    action = "right";
                    if (!(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                         && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        action = "up";
                        if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                        {

                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                             && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                        {
                            action = "down";
                            if (!(map_physique.mur(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                             && !(map_physique.mur(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
            }

        }

        public void pathfing_vol(ref string action, Player joueur)
        {

            if (ia.compteur_path % 30 == 0)
            {
                ia.compteur_path = 0;
                Random rand = new Random();
                ia.ia_dir = rand.Next(0, 2);
            }

            ia.compteur_path++;

            if (ia.ia_dir == 0)
            {
                if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                {
                    action = "left";
                    if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                         && !(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                        {
                            action = "up";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                             && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                        {
                            action = "down";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                             && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                {
                    action = "right";
                    if (!(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                         && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        action = "up";
                        if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                        {

                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                             && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                        {
                            action = "down";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                             && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                {
                    action = "up";

                    if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                     && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                        {
                            action = "left";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                        {
                            action = "right";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                {
                    action = "down";
                    if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                     && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                        {
                            action = "left";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                        {
                            action = "right";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }

                    }
                }
            }
            else
            {

                if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                {
                    action = "up";

                    if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                     && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                        {
                            action = "left";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                        {
                            action = "right";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                {
                    action = "down";
                    if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                     && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                        {
                            action = "left";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                        {
                            action = "right";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                                 && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }

                    }
                }
                else if (joueur.PlayerTexture.X < this.ia.IATexture.X)
                {
                    action = "left";
                    if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                         && !(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                        {
                            action = "up";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                             && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                        {
                            action = "down";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                             && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
                else if (joueur.PlayerTexture.X > this.ia.IATexture.X)
                {
                    action = "right";
                    if (!(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut()))
                         && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                    {
                    }
                    else
                    {
                        action = "up";
                        if (joueur.PlayerTexture.Y < this.ia.IATexture.Y)
                        {

                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_haut()))
                             && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_haut())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                        else if (joueur.PlayerTexture.Y > this.ia.IATexture.Y)
                        {
                            action = "down";
                            if (!(map_physique.mur_projectile(this.ia.futur_position_X_gauche(), this.ia.futur_position_Y_bas()))
                             && !(map_physique.mur_projectile(this.ia.futur_position_X_droite(), this.ia.futur_position_Y_bas())))
                            {
                            }
                            else
                            {
                                action = "";
                            }
                        }
                    }
                }
            }
        }

        //Gère le raffraichissement de la liste d'IA
        public void update_ia()
        {
            foreach (IA ia in liste_ia) //Pour chaque IA de la liste
            {
                if (ia.Health > 0)
                {
                    ia.est_update = true; //Précise que l'on travaille sur la liste
                    this.ia = ia;
                    float distance = 10000;
                    Vector2 vector_ia = new Vector2(ia.IATexture.X, ia.IATexture.Y);
                    Player joueur_cible = new Player(Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.S, Keys.P, Keys.Enter, Keys.T, Keys.F, Keys.F1, Ressources.Player1, parametre.x, parametre.y, 0);
                    Vector2 vector_joueur = new Vector2();
                    foreach (Player joueur in liste_joueurs)
                    {
                        joueur_cible = joueur;
                        vector_joueur = new Vector2(joueur_cible.PlayerTexture.X, joueur_cible.PlayerTexture.Y);
                    }

                    Vector2.Distance(ref vector_ia, ref vector_joueur, out distance);
                    foreach (Player joueur in liste_joueurs)
                    {
                        vector_joueur = new Vector2(joueur.PlayerTexture.X, joueur.PlayerTexture.Y);
                        float distance_2;
                        Vector2.Distance(ref vector_ia, ref vector_joueur, out distance_2);
                        if (distance_2 < distance)
                        {
                            distance = distance_2;
                            joueur_cible = joueur;
                        }
                    }
                    //L'ensemble des commandes précédentes définissent quel héros est la cible, ici le plus proche
                    /*
                    Node départ = new Node();
                    départ.x = (this.ia.IATexture.X + 8) / 16;
                    départ.y = (this.ia.IATexture.Y + 8) / 16;
                    Node arrivée = new Node();
                    arrivée.x = (joueur_cible.PlayerTexture.X + 8) / 16;
                    arrivée.y = (joueur_cible.PlayerTexture.Y + 8) / 16;

                    bool[,] map = map_physique.map();*/
                    /*
                    this.ia.action = Pathfinding.pathfind(map, départ, arrivée);
                    */
                    if (ia.ia_vol)
                    {
                        pathfing_vol(ref this.ia.action, joueur_cible);
                    }
                    else
                    {
                        pathfing(ref this.ia.action, joueur_cible);
                    }
                    //Trouve quelle action va faire l'IA
                    ia.action_ia(ia, joueur_cible, liste_barrel, map_physique, liste_ia, liste_joueurs); //Verifie la possibilité de réalisation des actions
                    ia.Update(); //Met à jour l'IA
                    ia.attaque_ia(liste_joueurs);

                    ia.est_update = false; //Désactive l'update de l'IA
                }
                else
                {
                    ia.en_vie = false; //Tue l'IA
                    compteur_kill++;
                    if (parametre.texture_map != 4 && parametre.texture_map != 9 && parametre.texture_map != 13 && parametre.texture_map != 15)
                    {
                        AddBloodEffect(new Vector2(ia.IATexture.X, ia.IATexture.Y), ia.IATexture.X, ia.IATexture.Y, 1);
                        moteur_son.PlayBloodEffect();
                    }
                }
            }
            if (liste_ia.Count >= 1)
            {
                IA ia2 = liste_ia[0];
                if (ia2.id_texture == 6)
                {
                    if (ia2.couldown_attaque_spéciale > 60)
                    {
                        int x = seed % 100 - 50 + ia2.IATexture.X;
                        int y = seed % 100 - 50 + ia2.IATexture.Y;
                        liste_attaque.Add(new AnimationAttack(x, y));
                        moteur_son.Playeclair();
                        ia2.couldown_attaque_spéciale = 0;
                    }
                    ia2.couldown_attaque_spéciale++;
                }
                else if (ia2.id_texture == 7)
                {
                    if (ia2.couldown_attaque_spéciale > 120)
                    {
                        liste_barrel.Add(new DestructibleItems(ia2.IATexture.Center.X, ia2.IATexture.Center.Y, "bomb_boss", 600));
                        ia2.couldown_attaque_spéciale = 0;
                    }
                    ia.couldown_attaque_spéciale++;
                }
                else if (ia2.id_texture == 8)
                {
                    if (ia2.couldown_attaque_spéciale > 300)
                    {
                        List<IA> liste_ia3 = new List<IA>();
                        liste_ia3.Add(new IA(146, 135, 11, -1, 100, 10));
                        //liste_ia3.Add(new IA(666, 220, 11, -1, 100, 10));
                        //liste_ia3.Add(new IA(125, 400, 11, -1, 100, 10));
                        ia2.couldown_attaque_spéciale = 0;
                        bool test = true;
                        foreach (IA ia_ in liste_ia3)
                        {
                            foreach (IA ia__ in liste_ia)
                            {
                                if (ia__.IATexture.Intersects(ia_.IATexture))
                                {
                                    test = false;
                                }
                                else
                                {
                                }
                            }
                            if (test)
                            {
                                liste_ia.Add(ia_);
                                moteur_son.Playpoke(ia_.id_son);
                                compteur3 = 0;
                            }
                        }
                    }
                    ia2.couldown_attaque_spéciale++;
                }
            }
            if (compteur3 > 150) //Ajout de nouvelles IA a la map
            {

                int choix = rand0.Next(spawns.Count);
                Spawn spawn = spawns.ElementAt(choix);
                choix = rand0.Next(spawn.créatures.Count);

                IA ia = spawn.créatures.ElementAt(choix);
                IA ia_ = new IA(ia.IATexture.X, ia.IATexture.Y, ia.id_texture, ia.id_son, ia.Health, ia.Damage);


                float distance = 10000;
                Vector2 vector_ia = new Vector2(ia_.IATexture.X, ia_.IATexture.Y);
                Player joueur_cible = new Player(Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.S, Keys.P, Keys.Enter, Keys.T, Keys.F, Keys.F1, Ressources.Player1, parametre.x, parametre.y, 0);
                Vector2 vector_joueur = new Vector2();
                foreach (Player joueur in liste_joueurs)
                {
                    joueur_cible = joueur;
                    vector_joueur = new Vector2(joueur_cible.PlayerTexture.X, joueur_cible.PlayerTexture.Y);
                }

                Vector2.Distance(ref vector_ia, ref vector_joueur, out distance);
                foreach (Player joueur in liste_joueurs)
                {
                    vector_joueur = new Vector2(joueur.PlayerTexture.X, joueur.PlayerTexture.Y);
                    float distance_2;
                    Vector2.Distance(ref vector_ia, ref vector_joueur, out distance_2);
                    if (distance_2 < distance)
                    {
                        distance = distance_2;
                    }
                }
                if (distance < 400)
                {
                    bool test = true;
                    foreach (IA ia__ in liste_ia)
                    {
                        if (ia__.IATexture.Intersects(ia_.IATexture))
                        {
                            test = false;
                        }
                        else
                        {
                        }
                    }
                    if (test)
                    {
                        liste_ia.Add(ia_);
                        moteur_son.Playpoke(ia_.id_son);
                        compteur3 = 0;
                    }
                }
            }
            compteur3 = compteur3 + 1;

            liste_ia2 = new List<IA>(); //Recopie la liste d'IA encore en vie dans une nouvelle liste
            foreach (IA ia in liste_ia)
            {
                if (ia.en_vie)
                    liste_ia2.Add(ia);
            }
            liste_ia = liste_ia2; //Vide la liste secondaire dans la premiere
        }

        public void update_projectiles(KeyboardState keyboard)
        {
            foreach (Projectile balle in liste_projectile)
            {
                balle.init = 0;
            }

            foreach (Projectile balle in liste_projectile)
            {
                while ((balle.init < balle.projectileMoveSpeed) && balle.isVisible)
                {
                    balle.collision_entite_balle(liste_ia); //Collision entres balles et entité
                    balle.collision_balle(map_physique); //Collision entre balle et mur
                    collision_barrel_balle(balle);
                    balle.init++; //On bouge la balle d'une case
                }
            }

            foreach (Player joueur in liste_joueurs) //On ajoute des balles en fonction des touches et du refroidissement
            {
                if ((joueur.tir && joueur.refroidissement >= joueur.Weapon().Reffroidissement && joueur.Ammo > 0))
                {
                    joueur.Weapon().Tir((int)joueur.centre().X, (int)joueur.centre().Y, joueur.Direction, seed, liste_projectile, liste_joueurs);
                    joueur.refroidissement = 0;
                    if (joueur.currentWeapon() == TypeArme.Pompe)
                    {
                        joueur.Ammo = joueur.Ammo - 2;
                    }
                    else
                    {
                        joueur.Ammo = joueur.Ammo - 1;
                    }
                    moteur_son.PlayTire();
                }
                joueur.refroidissement++;
            }

            liste_projectile2 = new List<Projectile>(); //On nettoie la liste, comme avec les IA
            foreach (Projectile balle in liste_projectile)
            {
                if (balle.isVisible)
                    liste_projectile2.Add(balle);
            }
            liste_projectile = liste_projectile2;
        }

        public void update_Box()
        {
            foreach (Items box in liste_box)
            {
                box.Update(liste_joueurs);
            }
            liste_box2 = new List<Items>();
            foreach (Items box in liste_box)
            {
                if (box.isVisible)
                {
                    liste_box2.Add(box);
                }
            }
            liste_box = liste_box2;

            if (compteur > 180) //Ajout de nouvelles "boxes" a la map
            {
                Items box;
                int spawn = seed % spawn_items.emplacement.Count;
                int texture = seed % (2);
                Vector2 emplacement = spawn_items.emplacement.ElementAt(spawn);
                bool test = true;
                if (texture == 0)
                {
                    box = new Items((int)emplacement.X, (int)emplacement.Y, "health");
                    foreach (Items box_ in liste_box)
                    {
                        if ((box_.HealthBoxTexture.Intersects(box.HealthBoxTexture)) && test)
                        {
                            test = false;
                        }
                    }
                    if (test)
                        liste_box.Add(box);
                }
                else
                {

                    box = new Items((int)emplacement.X, (int)emplacement.Y, "ammo");
                    foreach (Items box_ in liste_box)
                    {
                        if ((box_.HealthBoxTexture.Intersects(box.HealthBoxTexture)) && test)
                        {
                            test = false;
                        }
                    }
                    if (test)
                        liste_box.Add(box);
                }
                if (test)
                {
                    moteur_son.PlayPop();

                    compteur = 0;
                }
            }

            compteur++;
        }

        public void update_Bomb(List<Player> liste_joueurs, KeyboardState keyboard)
        {
            foreach (Player joueur in liste_joueurs)
            {
                if (parametre.texture_map == 3)
                {
                    if (joueur.poserBomb && joueur.refroidissement > 30 && joueur.bomb > 0)
                    {

                        AddBomb(joueur.PlayerTexture.X, joueur.PlayerTexture.Y, joueur.ActiverBomb, joueur.id_joueur);
                        joueur.refroidissement = 0;
                        joueur.bomb--;
                    }
                }
                else
                {
                    if (joueur.poserBomb && joueur.bomb > 0)
                    {
                        AddBomb(joueur.PlayerTexture.X, joueur.PlayerTexture.Y, joueur.ActiverBomb, joueur.id_joueur);
                        joueur.bomb--;
                    }
                }
            }

            foreach (DestructibleItems bomb in liste_barrel)
            {
                if (bomb.type == "bomb")
                {
                    bool test = false;
                    if (parametre.texture_map != 3)
                    {
                        test = keyboard.IsKeyDown(bomb.déclencher);
                        foreach (Keys key in input[bomb.owner])
                        {
                            if (key == bomb.déclencher)
                            {
                                test = true;
                            }
                        }

                        if (test)
                        {
                            AddExplosionEffect(new Vector2(bomb.BombTexture.X + 8, bomb.BombTexture.Y + 8), bomb.Aire_barrel.X - 16, bomb.Aire_barrel.Y - 16, 48);
                            moteur_son.PlayExplosionEffect();
                            bomb.isVisible = false;
                        }
                    }
                }

                else if (bomb.type == "bomb_2")
                {
                    foreach (Player joueur in liste_joueurs)
                    {
                        if ((keyboard.IsKeyDown(bomb.déclencher) && joueur.ActiverBomb == bomb.déclencher) || bomb.detonnateur == 0)
                        {
                            joueur.bomb = 5;
                            AddExplosionEffect(new Vector2(bomb.BombTexture.X + 8, bomb.BombTexture.Y + 8), bomb.Aire_barrel.X - 16, bomb.Aire_barrel.Y - 16, 48);
                            moteur_son.PlayExplosionEffect();
                            bomb.isVisible = false;
                        }
                    }
                    bomb.detonnateur--;
                }
                else if (bomb.type == "bomb_boss")
                {
                    foreach (Player joueur in liste_joueurs)
                    {
                        if (bomb.detonnateur == 0 || bomb.BombTexture.Intersects(joueur.PlayerTexture))
                        {
                            AddExplosionEffect(new Vector2(bomb.BombTexture.X + 8, bomb.BombTexture.Y + 8), bomb.Aire_barrel.X - 16, bomb.Aire_barrel.Y - 16, 48);
                            moteur_son.PlayExplosionEffect();
                        }
                    }
                    bomb.detonnateur--;
                }
            }
        }

        public void AddBloodEffect(Vector2 position, int x, int y, int largeur)
        {
            ParticuleExplosion blood = new ParticuleExplosion();
            blood.Initialize(Ressources.BloodParticule, position, 150, 186, 6, 45, Color.White, 1f, false, x, y, largeur, "blood");
            liste_blood.Add(blood);
        }

        public void update_Blood(GameTime gameTime)
        {
            liste_blood2 = new List<ParticuleExplosion>();

            foreach (ParticuleExplosion blood in liste_blood)
            {
                blood.Update(gameTime, liste_joueurs, liste_ia, liste_barrel, liste_explosions, liste_explosions3, liste_blood, liste_turret, liste_jetpack);
                if (blood.Active == true)
                {
                    liste_blood2.Add(blood);
                }
            }
            foreach (ParticuleExplosion explosion in liste_explosions3)
            {
                liste_blood2.Add(explosion);
            }
            liste_blood = liste_blood2;
        }

        public void update_Barrel(KeyboardState keyboard)
        {
            foreach (DestructibleItems barrel in liste_barrel)
            {
                barrel.Update();
            }
            liste_barrel2 = new List<DestructibleItems>();
            foreach (DestructibleItems barrel in liste_barrel)
            {
                if (barrel.isVisible)
                {
                    liste_barrel2.Add(barrel);
                }
            }
            liste_barrel = liste_barrel2;
        }

        public void collision_barrel_balle(Projectile balle) //S'occupe de la collision des balles avec les "barrels"
        {
            futur_rectangle = balle.rectangle();
            bool test = true;
            foreach (DestructibleItems barrel in liste_barrel) //Vérifie pour chaque "barrels"
            {
                if ((test)) //Permet de casser la boucle dès qu'un "barrel" est touché
                {
                    if (futur_rectangle.Intersects(barrel.Aire_barrel) && barrel.type == "barrel") //Si la HitBox du projectile est en contact avec celle du "barrel", alors (...)
                    {
                        balle.isVisible = false; //La balle n'existe plus
                        barrel.isVisible = false; //Le "barrel" n'existe plus
                        AddExplosionEffect(new Vector2(barrel.Aire_barrel.X + 8, barrel.Aire_barrel.Y + 8), barrel.Aire_barrel.X - 16, barrel.Aire_barrel.Y - 16, 48);
                        moteur_son.PlayExplosionEffect();
                        test = false; //On casse le si
                    }
                }
            }
        }

        public void AddExplosionEffect(Vector2 position, int x, int y, int largeur)
        {
            ParticuleExplosion explosion = new ParticuleExplosion();
            explosion.Initialize(Ressources.ExplosionParticule, position, 134, 134, 12, 45, Color.White, 1f, false, x, y, largeur, "explosion");
            liste_explosions.Add(explosion);
        }

        public void update_Explosion(GameTime gameTime)
        {
            liste_explosions2 = new List<ParticuleExplosion>();
            liste_explosions3 = new List<ParticuleExplosion>();
            foreach (ParticuleExplosion explosion in liste_explosions)
            {
                explosion.Update(gameTime, liste_joueurs, liste_ia, liste_barrel, liste_explosions, liste_explosions3, liste_blood, liste_turret, liste_jetpack);
                if (explosion.Active == true)
                {
                    liste_explosions2.Add(explosion);
                }
            }
            foreach (ParticuleExplosion explosion in liste_explosions3)
            {
                liste_explosions2.Add(explosion);
            }
            liste_explosions = liste_explosions2;
        }

        public void AddBomb(int x, int y, Keys activer, int owner)
        {
            DestructibleItems bomb;
            if (parametre.texture_map != 3)
            {
                bomb = new DestructibleItems(x, y, "bomb", activer, owner);
            }
            else
            {
                bomb = new DestructibleItems(x, y, "bomb_2", activer, owner);
            }
            liste_barrel.Add(bomb);
        }

        public void AddTurret(int x, int y)
        {
            liste_turret.Add(new Turret(x, y));
        }

        public void update_Turret(List<Player> liste_joueurs, KeyboardState keyboard)
        {
            foreach (Player joueur in liste_joueurs)
            {
                if (joueur.poserTurret && joueur.turret > 0)
                {
                    AddTurret(joueur.PlayerTexture.X, joueur.PlayerTexture.Y);
                    moteur_son.PlaySentryReady();
                    joueur.turret--;
                }
            }
            foreach (Turret sentry in liste_turret)
            {
                sentry.Update(liste_joueurs, liste_ia, liste_projectile, moteur_son, seed);
            }
        }

        public void AddRain()
        {
            liste_rain.Add(new ParticuleRain(rainTexture, new Vector2(-50 + (float)rand1.NextDouble() * spawnWidth, 0), new Vector2(1, rand2.Next(5, 8))));
        }

        public void update_Rain(GameTime gameTime, GraphicsDevice graphics)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer > 0)
            {
                timer -= 1f / density;
                AddRain();
            }

            for (int i = 0; i < liste_rain.Count; i++)
            {
                liste_rain[i].Update();

                if (liste_rain[i].Position.Y > graphics.Viewport.Height)
                {
                    liste_rain.RemoveAt(i);
                    i--;
                }
            }
        }

        public void update_Plane(List<Player> liste_joueurs, KeyboardState keyboard, MouseState mouse, int x, int y)
        {
            if (liste_plane.Count == 0)
                avion = 1;
            if (mouse.LeftButton == ButtonState.Pressed && keyboard.IsKeyDown(Keys.F) && avion > 0)
            {
                liste_plane.Add(new Plane(mouse.X, mouse.Y, x, y));
                moteur_son.PlayPlaneEffect();
                avion--;
            }
            liste_plane2 = new List<Plane>();
            foreach (Plane plane in liste_plane)
            {
                plane.Update();
                int i = 0;
                while (i < 5)
                {
                    plane.PlaneTexture.X++;
                    if (plane.PlaneTexture.X + x == plane.target.X)
                    {
                        AddExplosionEffect(new Vector2(plane.target.X /*+ x*/, plane.target.Y /*+ y*/), (int)plane.target.X - 50 /*+ x*/, (int)plane.target.Y - 50 /*+ y*/, 100);
                        moteur_son.PlayExplosionEffect();
                        //AddFireEffect(new Vector2(plane.target.X, plane.target.Y), (int)plane.target.X - 25, (int)plane.target.Y - 25, 50);
                    }
                    i++;
                }
                if (plane.PlaneTexture.X < 800)
                {
                    liste_plane2.Add(plane);
                }
            }
            liste_plane = liste_plane2;
        }

        public void update_JetPack()
        {
            foreach (JetPack jetpack in liste_jetpack)
            {
                jetpack.Update(liste_joueurs);
            }
            liste_jetpack2 = new List<JetPack>();
            foreach (JetPack jetpack in liste_jetpack)
            {
                if (jetpack.isVisible)
                {
                    liste_jetpack2.Add(jetpack);
                }
            }
            liste_jetpack = liste_jetpack2;

            if (compteur2 > 180)
            {
                JetPack jetpack;
                int spawn = seed % spawn_jetpacks.emplacement.Count;
                Vector2 emplacement = spawn_jetpacks.emplacement.ElementAt(spawn);
                bool test = true;

                if (true)
                {
                    jetpack = new JetPack((int)emplacement.X, (int)emplacement.Y);
                    foreach (JetPack jetpack_ in liste_jetpack)
                    {
                        if ((jetpack_.JetPackTexture.Intersects(jetpack.JetPackTexture)) && test)
                        {
                            test = false;
                        }
                    }
                    if (test)
                    {
                        liste_jetpack.Add(jetpack);
                    }
                }
                if (test)
                {
                    compteur2 = 0;
                }
            }
            compteur2++;
        }

        public void AddFaille(int x, int y)
        {
            liste_faille.Add(new KristboulFaille(x, y));
        }

        public void update_Faille()
        {
            foreach (KristboulFaille faille in liste_faille)
            {
                faille.Update(liste_joueurs);
            }
        }

        //Gère l'affichage de la liste d'IA
        public void swap(ref IA a, ref IA b)
        {
            IA c = a;
            a = b;
            b = c;
        }

        public int minimum(IA[] tableau, int d, int f)
        {
            int pos = d;
            int i = d + 1;
            while (i < tableau.Length)
            {
                if (tableau[i].IATexture.Y < tableau[pos].IATexture.Y)
                {
                    pos = i;
                }
                i++;
            }

            return pos;
        }

        public void tri(ref IA[] tableau)
        {
            int i = 0;
            while (i < tableau.Length)
            {
                swap(ref tableau[i], ref tableau[minimum(tableau, i, tableau.Length)]);
                i++;
            }
        }

        public void draw_ordre(SpriteBatch spriteBatch)
        {
            IA[] tableau_ia = new IA[liste_ia.Count];
            liste_ia.CopyTo(tableau_ia);
            tri(ref tableau_ia);
            foreach (Player joueur in liste_joueurs)
            {
                joueur.est_afficher = false;
            }
            foreach (IA ia in tableau_ia)
            {
                foreach (Player joueur in liste_joueurs)
                {
                    if ((ia.IATexture.Y > joueur.PlayerTexture.Y) && !joueur.est_afficher)
                    {
                        joueur.Draw(spriteBatch, MapTexture);
                        joueur.est_afficher = true;

                    }

                    ia.Draw(spriteBatch, MapTexture);
                }


            }
            foreach (Player joueur in liste_joueurs)
            {
                if (!joueur.est_afficher)
                {
                    joueur.Draw(spriteBatch, MapTexture);
                    joueur.est_afficher = true;
                }
            }
        }

        public void update_player(KeyboardState keyboard, MouseState mouse)
        {
            // Update l'objet joueur contenu par la map
            foreach (Player joueur in liste_joueurs)
            {
                joueur.Update(mouse/*, keyboard*/, input[joueur.id_joueur]);
                joueur.action_hero(map_physique, liste_ia, liste_barrel);
                if (joueur.Health > 0)
                {
                    liste_joueurs2.Add(joueur);
                }
                else
                {/*
                    if (parametre.texture_map == 3)
                    {
                        Player joueur_ = new Player(joueur.Haut, joueur.Bas, joueur.Gauche, joueur.Droite, joueur.Tire, joueur.PoserBomb, joueur.ActiverBomb, joueur.PoserTurret, joueur.ActiverPlane, joueur.texture, parametre.x, parametre.y, 0);
                        joueur_.bomb = 5;
                        liste_joueurs2.Add(joueur_);
                    }
                  * */
                }
            }
            liste_joueurs = liste_joueurs2;
            liste_joueurs2 = new List<Player>();


            if (liste_joueurs.Count == 0) //Si il n'y a plus de joueurs
            {
                game_over = true;
            }
        }

        public void update_attaque(KeyboardState keyboard, MouseState mouse)
        {
            liste_attaque2 = new List<AnimationAttack>();
            foreach (AnimationAttack Dolphin in this.liste_attaque)
            {
                Dolphin.Update(mouse, keyboard);
                foreach (Player poney in liste_joueurs)
                    if (poney.PlayerTexture.Intersects(Dolphin.Hitbox))
                        poney.Health--;
                if (Dolphin.Animation)
                    liste_attaque2.Add(Dolphin);
            }
            liste_attaque = liste_attaque2;
        }


        // UPDATE & DRAW
        public void Update(MouseState mouse, KeyboardState keyboard, GameTime gameTime, GraphicsDevice graphics)
        {
            message = "";
            message = System.Convert.ToString(id_joueur);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))//Up
            {
                message = message + 'u';
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))//Down
            {
                message = message + 'd';
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))//Right
            {
                message = message + 'l';
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))//Left
            {
                message = message + 'r';
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N))//Tir
            {
                message = message + 'n';
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))//Pose
            {
                message = message + 'p';
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))//Bombe
            {
                message = message + 'e';
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))//Bombe
            {
                message = message + 't';
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F1))//Bombe
            {
                message = message + 'f';
            }

            if (multi)
            {
                try
                {
                    seed = System.Convert.ToInt32(Reseau.receptionMessage(ref connection));
                    Reseau.envoieMessage(message, ref connection);
                    input = Reseau.Liste(Reseau.receptionMessage(ref connection));
                    Thread.Sleep(0);
                }
                catch
                {
                    connection = false;
                }
            }
            else
            {
                seed = new Random().Next(1000);
                input = Reseau.Liste(message);
            }
            int x = 0;
            int y = 0;

            if (parametre.scrolling)
            {
                MapTexture.X = 400 - x;
                MapTexture.Y = 240 - y;
            }
            else
            {
                MapTexture.X = 0;
                MapTexture.Y = 0;
            }
            testc = !testc;
            update_ia();
            update_Box();
            update_Barrel(keyboard);
            update_Explosion(gameTime);
            update_Blood(gameTime);
            update_projectiles(keyboard);
            update_player(keyboard, mouse);
            update_Bomb(liste_joueurs, keyboard);
            update_Turret(liste_joueurs, keyboard);
            update_attaque(keyboard, mouse);

            if (parametre.texture_map == 1 || parametre.texture_map == 5 || parametre.texture_map == 9)
            {
                Random rand3 = new Random();
                int i = rand3.Next(0, 5000);

                if (i == 42)
                {
                    moteur_son.PlayRainEffect();
                    density = 100;
                }
                update_Rain(gameTime, graphics);
            }
            if (parametre.texture_map == 1 || parametre.texture_map == 11 || parametre.texture_map == 6 || parametre.texture_map == 7 || parametre.texture_map == 12 || parametre.texture_map == 13)
            {
                fin_niveau.Update(liste_joueurs);
            }
            if (parametre.texture_map == 10 || parametre.texture_map == 8 || parametre.texture_map == 14)
            {
                boss_entry.Update(liste_joueurs);
            }
            if (parametre.texture_map == 0 || parametre.texture_map == 5 || parametre.texture_map == 9 || parametre.texture_map == 16)
            {
                update_Plane(liste_joueurs, keyboard, mouse, -MapTexture.X, -MapTexture.Y);
            }
            if (parametre.texture_map == 1 || parametre.texture_map == 10 || parametre.texture_map == 5 || parametre.texture_map == 6 || parametre.texture_map == 7 || parametre.texture_map == 8 || parametre.texture_map == 12 || parametre.texture_map == 14 || parametre.texture_map == 16)
            {
                update_JetPack();
            }
            if (parametre.texture_map == 4 || parametre.texture_map == 9 || parametre.texture_map == 15)
            {
                bool poney = true;
                foreach (IA ia in liste_ia)
                {
                    if (ia.IA_boss)
                    {
                        poney = false;
                    }
                }
                gagne = poney;
            }
            if (parametre.texture_map == 16)
            {
                AddFaille(385, 425);
                update_Faille();
                foreach (KristboulFaille faille in liste_faille)
                {
                    failleActive = faille.bsod;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int x = 0;
            int y = 0;

            foreach (Player joueur in liste_joueurs)
            {
                x = joueur.PlayerTexture.X;
                y = joueur.PlayerTexture.Y;
                if (joueur.id_joueur == id_joueur)
                    break;
            }
            if (parametre.scrolling)
            {
                MapTexture.X = 400 - x;
                MapTexture.Y = 240 - y;

                if (MapTexture.X > 0)
                    MapTexture.X = 0;
                if (MapTexture.Y > 0)
                    MapTexture.Y = 0;
                if (MapTexture.X + MapTexture.Width < 800)
                    MapTexture.X = -MapTexture.Width + 800;
                if (MapTexture.Y + MapTexture.Height < 480)
                    MapTexture.Y = -MapTexture.Height + 480;
            }
            else
            {
                MapTexture.X = 0;
                MapTexture.Y = 0;
            }

            //SURVIE
            if (parametre.texture_map == 0)
            {
                spriteBatch.Draw(Ressources.Map, this.MapTexture, Color.CadetBlue);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
            }
            else if (parametre.texture_map == 2)
            {
                spriteBatch.Draw(Ressources.Map03, this.MapTexture, Color.White);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
            }
            else if (parametre.texture_map == 3)
            {
                spriteBatch.Draw(Ressources.Map03, this.MapTexture, Color.White);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
            }
            else if (parametre.texture_map == 5)
            {
                spriteBatch.Draw(Ressources.Map05, this.MapTexture, Color.White);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.CornflowerBlue);
                foreach (ParticuleRain rain in liste_rain)
                {
                    rain.Draw(spriteBatch);
                }
            }
            else if (parametre.texture_map == 16)
            {
                spriteBatch.Draw(Ressources.Map16, this.MapTexture, Color.White);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
                foreach (KristboulFaille faille in liste_faille)
                {
                    faille.Draw(spriteBatch);
                }
            }
            //CAMPAGNE 1
            else if (parametre.texture_map == 1)
            {
                spriteBatch.Draw(Ressources.Map02, this.MapTexture, Color.CadetBlue);
                fin_niveau.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
                foreach (ParticuleRain rain in liste_rain)
                {
                    rain.Draw(spriteBatch);
                }
            }
            else if (parametre.texture_map == 11)
            {
                spriteBatch.Draw(Ressources.Map11, this.MapTexture, Color.CadetBlue);
                fin_niveau.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
                foreach (ParticuleRain rain in liste_rain)
                {
                    rain.Draw(spriteBatch);
                }
            }
            else if (parametre.texture_map == 10)
            {
                spriteBatch.Draw(Ressources.Map10, this.MapTexture, Color.CadetBlue);
                boss_entry.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
            }
            else if (parametre.texture_map == 4)
            {
                spriteBatch.Draw(Ressources.Map04, this.MapTexture, Color.White);
            }
            //CAMPAGNE 2
            else if (parametre.texture_map == 12)
            {
                spriteBatch.Draw(Ressources.Map12, this.MapTexture, Color.CadetBlue);
                fin_niveau.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
            }
            else if (parametre.texture_map == 13)
            {
                spriteBatch.Draw(Ressources.Map13, this.MapTexture, Color.White);
                fin_niveau.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, "0", new Vector2(5, 42), Color.White);
            }
            else if (parametre.texture_map == 14)
            {
                spriteBatch.Draw(Ressources.Map14, this.MapTexture, Color.White);
                boss_entry.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.White);
            }
            else if (parametre.texture_map == 15)
            {
                spriteBatch.Draw(Ressources.Map15, this.MapTexture, Color.White);
            }
            //CAMPAGNE 3
            else if (parametre.texture_map == 6)
            {
                spriteBatch.Draw(Ressources.Map06, this.MapTexture, Color.White);
                fin_niveau.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.CornflowerBlue);
            }
            else if (parametre.texture_map == 7)
            {
                spriteBatch.Draw(Ressources.Map07, this.MapTexture, Color.White);
                fin_niveau.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.CornflowerBlue);
            }
            else if (parametre.texture_map == 8)
            {
                spriteBatch.Draw(Ressources.Map08, this.MapTexture, Color.White);
                boss_entry.Draw(spriteBatch, MapTexture);
                spriteBatch.DrawString(Ressources.HUD, Convert.ToString(compteur_kill), new Vector2(5, 42), Color.CornflowerBlue);
            }
            else if (parametre.texture_map == 9)
            {
                spriteBatch.Draw(Ressources.Map09, this.MapTexture, Color.White);
                foreach (ParticuleRain rain in liste_rain)
                {
                    rain.Draw(spriteBatch);
                }
            }
            foreach (Items box in liste_box)
            {
                box.Draw(spriteBatch, MapTexture);
            }
            foreach (DestructibleItems barrel in liste_barrel)
            {
                barrel.Draw(spriteBatch, MapTexture);
            }
            foreach (Turret turret in liste_turret)
            {
                turret.Draw(spriteBatch, MapTexture);
            }
            foreach (JetPack jetpack in liste_jetpack)
            {
                jetpack.Draw(spriteBatch, MapTexture);
            }
            draw_ordre(spriteBatch);

            foreach (Projectile projectile in liste_projectile)
            {
                projectile.Draw(spriteBatch, MapTexture);
            }
            foreach (ParticuleExplosion blood in liste_blood)
            {
                if (parametre.texture_map != 4 && parametre.texture_map != 9 && parametre.texture_map != 15)
                {
                    blood.Draw(spriteBatch, MapTexture);
                }
            }
            if (parametre.texture_map == 0)
            {
                spriteBatch.Draw(Ressources.Map_transparent, this.MapTexture, Color.CadetBlue);
            }
            else if (parametre.texture_map == 1)
            {
                spriteBatch.Draw(Ressources.Map02_transparent, this.MapTexture, Color.CadetBlue);
            }
            else if (parametre.texture_map == 6)
            {
                spriteBatch.Draw(Ressources.Map06_transparent, this.MapTexture, Color.White);
            }
            else if (parametre.texture_map == 7)
            {
                spriteBatch.Draw(Ressources.Map07_transparent, this.MapTexture, Color.White);
            }
            else if (parametre.texture_map == 8)
            {
                spriteBatch.Draw(Ressources.Map08_transparent, this.MapTexture, Color.White);
            }
            foreach (AnimationAttack eclair in liste_attaque)
            {
                eclair.Draw(spriteBatch, MapTexture);
            }
            foreach (ParticuleExplosion explosion in liste_explosions)
            {
                explosion.Draw(spriteBatch, MapTexture);
            }
            foreach (AnimationAttack eclair in liste_attaque)
            {
                eclair.Draw(spriteBatch, MapTexture);
            }
            foreach (Plane plane in liste_plane)
            {
                plane.Draw(spriteBatch);
            }
        }
    }
}

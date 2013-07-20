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
    struct Param_Map
    {
        public bool[,] liste;
        public bool[,] liste_projectile;
        public List<Spawn> liste_spawn;
        public List<DestructibleItems> liste_barrel;
        public Spawn_Items liste_caisses;
        public Spawn_JetPack liste_jetpacks;
        public int x;
        public int y;
        public int texture_map;
        public int largeur;
        public int hauteur;
        public CheckPoint checkpointArrivee, checkpointBossEntry;
        public bool scrolling;

        public Param_Map(bool[,] liste, bool[,] liste_projectile, List<Spawn> liste_spawn, List<DestructibleItems> liste_barrel, Spawn_Items liste_caisses, Spawn_JetPack liste_jetpacks, int x, int y, int texture_map, int hauteur, int largeur, CheckPoint checkpointArrivee, CheckPoint checkpointBossEntry, bool scrolling)
        {
            this.liste = liste;
            this.liste_projectile = liste_projectile;
            this.liste_spawn = liste_spawn;
            this.liste_barrel = liste_barrel;
            this.liste_caisses = liste_caisses;
            this.liste_jetpacks = liste_jetpacks;
            this.x = x;
            this.y = y;
            this.texture_map = texture_map;
            this.hauteur = hauteur;
            this.largeur = largeur;
            this.checkpointArrivee = checkpointArrivee;
            this.checkpointBossEntry = checkpointBossEntry;
            this.scrolling = scrolling;
        }
    }

    class LecteurMap
    {
        static bool[,] liste;
        static bool[,] liste_projectile;
        static List<Spawn> liste_spawn = new List<Spawn>();
        static List<DestructibleItems> liste_barrel = new List<DestructibleItems>();
        static Spawn_Items liste_caisses = new Spawn_Items(new List<Vector2>());
        static Spawn_JetPack liste_jetpacks = new Spawn_JetPack(new List<Vector2>());
        static int x;
        static int y;
        static int texture_map;
        static int largeur;
        static int hauteur;
        static CheckPoint checkpointArrivee = new CheckPoint(0, 0, "arrivee"), checkPointBossEntry = new CheckPoint(0, 0, "bossentry");
        static bool scrolling;

        public static Param_Map lecture(string path)
        {

            StreamReader lecture = new StreamReader("Content/maps/" + path);
            string ligne = lecture.ReadLine();

            scrolling = ligne == ("scrolling");

            ligne = lecture.ReadLine();
            texture_map = Convert.ToInt32(ligne);
            ligne = lecture.ReadLine();
            x = Convert.ToInt32(ligne);
            ligne = lecture.ReadLine();
            y = Convert.ToInt32(ligne);
            ligne = lecture.ReadLine();
            while (ligne != null)
            {
                if (ligne == "physique")
                {
                    lecture_physique(lecture);
                }
                else if (ligne == "spawn")
                {
                    lecture_spawn(lecture);
                }
                else if (ligne == "caisses")
                {
                    lecture_caisses(lecture);
                }
                else if (ligne == "barrels")
                {
                    lecture_barrel(lecture);
                }
                else if (ligne == "arrivee")
                {
                    lecture_arrivee(lecture);
                }
                else if (ligne == "bossentry")
                {
                    lecture_bossentry(lecture);
                }
                else if (ligne == "jetpacks")
                {
                    lecture_jetpacks(lecture);
                }
                ligne = lecture.ReadLine();
            }
            lecture.Close();

            return new Param_Map(liste, liste_projectile, liste_spawn, liste_barrel, liste_caisses, liste_jetpacks, x, y, texture_map, hauteur, largeur, checkpointArrivee, checkPointBossEntry, scrolling);
        }

        public static void lecture_physique(StreamReader lecture)
        {
            string ligne = "";
            ligne = lecture.ReadLine();
            hauteur = Convert.ToInt32(ligne);
            ligne = lecture.ReadLine();
            largeur = Convert.ToInt32(ligne);
            liste = new bool[hauteur, largeur];
            liste_projectile = new bool[hauteur, largeur];
            ligne = lecture.ReadLine();

            int i = 0;
            while (/*ligne != null ||*/ ligne != "eau")
            { // Début de travaille sur le lecteur de fichier, don't toutch !
                int i1 = 0;
                foreach (char bool_ in ligne)
                {
                    try
                    {
                        if (ligne[i1] == '0') // la zone est franchissable
                        {
                            liste[i1, i] = false;
                        }
                        else // la zone est infranchissable
                        {
                            liste[i1, i] = true;
                        }
                    }
                    catch
                    {
                    }
                    i1++;
                }
                ligne = lecture.ReadLine();
                i++;
            }
            lecture.ReadLine();
            i = 0;
            while (/*ligne != null ||*/ ligne != "//")
            { // Début de travaille sur le lecteur de fichier, don't toutch !
                int i1 = 0;
                foreach (char bool_ in ligne)
                {
                    try
                    {
                        if (ligne[i1] == '0') // la zone est franchissable
                        {
                            liste_projectile[i1, i] = false;
                        }
                        else // la zone est infranchissable
                        {
                            liste_projectile[i1, i] = true;
                        }
                    }
                    catch
                    {
                    }
                    i1++;

                }
                ligne = lecture.ReadLine();
                i++;
            }
        }

        public static void lecture_spawn(StreamReader lecture)
        {
            liste_spawn = new List<Spawn>();
            string ligne = "";
            while (/*ligne != null || */ligne != "//")
            {
                List<IA> liste_ia = new List<IA>();
                while (ligne != "new")
                {
                    ligne = lecture.ReadLine();
                    int x = System.Convert.ToInt32(ligne);

                    ligne = lecture.ReadLine();
                    int y = System.Convert.ToInt32(ligne);

                    ligne = lecture.ReadLine();
                    int id_texture = System.Convert.ToInt32(ligne);

                    ligne = lecture.ReadLine();
                    int id_son = System.Convert.ToInt32(ligne);

                    ligne = lecture.ReadLine();

                    int pv_max = System.Convert.ToInt32(ligne);

                    ligne = lecture.ReadLine();
                    int dégats = System.Convert.ToInt32(ligne);

                    liste_ia.Add(new IA(x, y, id_texture, id_son, pv_max, dégats));

                    ligne = lecture.ReadLine();
                }

                liste_spawn.Add(new Spawn(liste_ia));

                ligne = lecture.ReadLine();
            }
        }

        public static void lecture_caisses(StreamReader lecture)
        {
            List<Vector2> liste_caisses_ = new List<Vector2>();
            string ligne = lecture.ReadLine();
            while (ligne != "//")
            {
                ligne = lecture.ReadLine();
                int x = System.Convert.ToInt32(ligne);

                ligne = lecture.ReadLine();
                int y = System.Convert.ToInt32(ligne);

                liste_caisses_.Add(new Vector2(x, y));

                ligne = lecture.ReadLine();
            }
            liste_caisses = new Spawn_Items(liste_caisses_);
        }

        public static void lecture_barrel(StreamReader lecture)
        {
            liste_barrel = new List<DestructibleItems>();
            string ligne = lecture.ReadLine();
            while (ligne != "//")
            {
                ligne = lecture.ReadLine();
                int x = System.Convert.ToInt32(ligne);

                ligne = lecture.ReadLine();
                int y = System.Convert.ToInt32(ligne);

                ligne = lecture.ReadLine();
                string type = ligne;

                liste_barrel.Add(new DestructibleItems(x, y, type));

                ligne = lecture.ReadLine();
            }
        }

        public static void lecture_arrivee(StreamReader lecture)
        {
            string ligne = lecture.ReadLine();
            int x = System.Convert.ToInt32(ligne);

            ligne = lecture.ReadLine();
            int y = System.Convert.ToInt32(ligne);

            checkpointArrivee = new CheckPoint(x, y, "arrivee");
            ligne = lecture.ReadLine();
        }

        public static void lecture_bossentry(StreamReader lecture)
        {
            string ligne = lecture.ReadLine();
            int x = System.Convert.ToInt32(ligne);

            ligne = lecture.ReadLine();
            int y = System.Convert.ToInt32(ligne);

            checkPointBossEntry = new CheckPoint(x, y, "bossentry");
            ligne = lecture.ReadLine();
        }

        public static void lecture_jetpacks(StreamReader lecture)
        {
            List<Vector2> liste_jetpacks_ = new List<Vector2>();
            string ligne = lecture.ReadLine();
            while (ligne != "//")
            {
                ligne = lecture.ReadLine();
                int x = System.Convert.ToInt32(ligne);

                ligne = lecture.ReadLine();
                int y = System.Convert.ToInt32(ligne);

                liste_jetpacks_.Add(new Vector2(x, y));

                ligne = lecture.ReadLine();
            }
            liste_jetpacks = new Spawn_JetPack(liste_jetpacks_);
        }
    }
}
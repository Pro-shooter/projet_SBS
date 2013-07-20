/*using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Sunday_Bloody_Sunday
{
    class Pathfinding
    {
        private static List<Node> liste_ouverte;
        private static List<Node> liste_fermée;

        public static string pathfind(bool[,] map, Node départ, Node arrivee)
        {
            liste_ouverte = new List<Node>();
            liste_fermée = new List<Node>();

            ajout_liste_ouverte(départ);

            Node current = null;

            while (liste_ouverte.Count > 0)
            {
                current = getCurrent();
                if (egalité(current, arrivee))
                {
                    break;
                }

                ajout_liste_fermée(current);

                List<Node> Voisin = getVoisins(current, map);

                foreach (Node node in Voisin)
                {
                    if (est_dans_fermée(node) || map[node.x, node.y])
                    {
                        continue;
                    }
                    else
                    {
                        int new_g = node.parent.G + 1;

                        int new_h = 0;

                        if (départ.x - node.x < 0)
                            new_h = new_h - (départ.x - node.x);
                        else
                            new_h = new_h + (départ.x - node.x);

                        if (départ.y - node.y < 0)
                            new_h = new_h - (départ.y - node.y);
                        else
                            new_h = new_h + (départ.y - node.y);

                        int new_f = new_g + new_h;

                        if (est_dans_ouverte(node))
                        {
                            if (new_g < node.G)
                            {
                                node.parent = current;
                                node.G = new_g;
                                node.H = new_h;
                                node.F = new_f;
                            }
                        }
                        else
                        {
                            node.parent = current;
                            node.G = new_g;
                            node.H = new_h;
                            node.F = new_f;
                            ajout_liste_ouverte(node);
                        }
                    }

                }
            }
            if (liste_ouverte.Count == 0)
            {
                return "";
            }
            else
            {
                while (current.parent.x != départ.x || current.parent.y != départ.y)
                {
                    current = current.parent;
                }

                if (départ.x - current.x < 0)
                {
                    return "right";
                }
                else if (départ.x - current.x > 0)
                {
                    return "left";
                }
                else if (départ.y - current.y < 0)
                {
                    return "down" ;
                }
                else if (départ.y - current.y > 0)
                {
                    return "up";
                }
                else
                {
                    return "";
                }
            }

        }

        private static void suppr_liste_fermée(Node node)
        {
            liste_fermée.Remove(node);
        }

        private static void suppr_liste_ouverte(Node node)
        {
            liste_ouverte.Remove(node);
        }

        private static void ajout_liste_fermée(Node node)
        {
            suppr_liste_ouverte(node);
            liste_fermée.Add(node);
        }

        private static void ajout_liste_ouverte(Node node)
        {
            liste_ouverte.Add(node);
            suppr_liste_fermée(node);
        }

        private static Node getCurrent()
        {
            Node current = liste_ouverte.First();
            foreach (Node node in liste_ouverte)
            {
                if (current.F > node.F)
                {
                    current = node;
                }
            }

            return current;
        }

        private static List<Node> getVoisins(Node current, bool[,] map)
        {
            List<Node> voisin = new List<Node>();
            int x_sup = current.x + 1;
            int x_inf = current.x - 1;
            int y_sup = current.y + 1;
            int y_inf = current.y - 1;
            Node node = new Node();

            if (x_sup < map.GetLength(0))
            {
                node = new Node();
                node.parent = current;
                node.x = x_sup;
                node.y = current.y;
                node.traversable = !map[node.x, node.y];
                voisin.Add(node);
            }
            if (x_inf >= 0)
            {
                node = new Node();
                node.parent = current;
                node.x = x_inf;
                node.y = current.y;
                node.traversable = !map[node.x, node.y];
                voisin.Add(node);
            }
            if (y_sup < map.GetLength(1))
            {
                node = new Node();
                node.parent = current;
                node.x = current.x;
                node.y = y_sup;
                node.traversable = !map[node.x, node.y];
                voisin.Add(node);
            }
            if (y_inf >= 0)
            {
                node = new Node();
                node.parent = current;
                node.x = current.x;
                node.y = y_inf;
                node.traversable = !map[node.x, node.y];
                voisin.Add(node);
            }
            return voisin;
        }

        private static bool est_dans_ouverte(Node node)
        {
            bool test = false;
            foreach (Node node_ in liste_ouverte)
            {
                if (!test)
                {
                    test = egalité(node, node_);
                }
            }
            return test;
        }

        private static bool est_dans_fermée(Node node)
        {
            bool test = false;
            foreach (Node node_ in liste_fermée)
            {
                if (!test)
                {
                    test = egalité(node, node_);
                }
            }
            return test;
        }

        private static bool egalité(Node node1, Node node2)
        {
            return ((node1.x == node2.x) && (node1.y == node2.y));
        }
    }

    class Node
    {
        public int G, H, F, x, y; //Distance liée au point de départ, Distance du point d'arrivée, Somme des distances, coordonnées de la node
        public bool traversable;
        public Node parent;

        public Node()
        {
            traversable = true;
            parent = this;
            G = 0;
            H = 0;
            F = 0;
        }
    }
}
*/
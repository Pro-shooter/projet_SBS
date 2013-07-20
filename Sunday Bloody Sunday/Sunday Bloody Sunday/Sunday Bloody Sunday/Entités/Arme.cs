using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunday_Bloody_Sunday
{

    enum TypeArme
    {
        Fusil,
        Pompe,
        Sniper
    };

    class Arme
    {
        public TypeArme Type;
        public int Reffroidissement;

        public Arme(TypeArme Type)
        {
            this.Type = Type;
            switch (Type)
            {
                case TypeArme.Fusil:
                    Reffroidissement = 8;
                    break;

                case TypeArme.Pompe:
                    Reffroidissement = 60;
                    break;

                case TypeArme.Sniper:
                    Reffroidissement = 60;
                    break;
            }
        }

        public void Tir(int X, int Y, Direction direction, int seed, List<Projectile> Liste, List<Player> liste_joueurs)
        {
            // Ressource, X, Y, Velocity, Direction, Damage, Dispersion, Seed
            //new Projectile(Ressources.Projectile, X, Y, 10, Direction.Right, 50, 30, seed);
            switch (Type)
            {
                case TypeArme.Fusil:
                    Liste.Add(new Projectile(Ressources.Projectile, X, Y, 10, direction, 50, 10, seed));
                    break;

                case TypeArme.Pompe:
                    int i = 5;
                    while (i > 0)
                    {
                        Liste.Add(new Projectile(Ressources.Projectile, X, Y, 10, direction, 100, 30, seed * i));
                        i--;
                    }
                    break;

                case TypeArme.Sniper:
                    {
                        Liste.Add(new Projectile(Ressources.Projectile, X, Y, 10, direction, 200, 0, seed));
                    }
                    break;
            }
        }
    }
}

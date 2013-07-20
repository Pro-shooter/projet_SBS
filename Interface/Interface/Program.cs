using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Interface
{

    class Program
    {
        static string buffer = "";
        static Random seed = new Random();
        public static List<Reseau> Clients = new List<Reseau>();

        static int i;
        static bool synchro = false;

        static int nombre_joueurs;

        static void Main(string[] args)
        {
            nombre_joueurs = 2;
            i = 0;
            while (i < nombre_joueurs)
            {
                Reseau Lien = new Reseau();
                Lien.intialisationServeur(1337);
                Lien.envoieMessage(System.Convert.ToString(4242 + i));
                Lien.Attente.Close();
                Lien.Reception.Close();

                Reseau Client = new Reseau();
                Client.intialisationServeur(4242 + i);
                Client.receptionMessage();
                buffer += Client.message;
                Clients.Add(Client);
                Console.WriteLine("Ping : " + (i + 1));
                i++;
            }

            i = 1;

            foreach (Reseau Client in Clients)
            {
                Client.envoieMessage(System.Convert.ToString(nombre_joueurs));
                Client.envoieMessage(buffer);
            }

            Lobby();
            Console.WriteLine("Here we go !");
            Partie();
        }

        static void Lobby()
        {
            try
            {
                Thread.Sleep(3000);
                buffer = "|";
                while (buffer[0] == '|')
                {
                    buffer = "";
                    foreach (Reseau Client in Clients)
                    {
                        Client.receptionMessage();
                        buffer += Client.message;
                    }
                    Console.WriteLine(buffer);

                    //Envoie des messages
                    foreach (Reseau Client in Clients)
                    {
                        if (buffer[0] == '|')
                            Client.envoieMessage(buffer);
                        else
                            Client.envoieMessage("go");
                    }
                    Thread.Sleep(10);
                }
            }
            catch
            {
                Fin();
            }
        }

        static void Partie()
        {
            try
            {
                Thread.Sleep(3000);
                Console.WriteLine("Rdy !");
                while (true)
                {
                    int i = 1;
                    int seed_ = seed.Next(1000);

                    foreach (Reseau Client in Clients)
                    {
                        Client.envoieMessage(System.Convert.ToString(seed_));
                    }

                    buffer = "";

                    //Reception des messages
                    foreach (Reseau Client in Clients)
                    {
                        Client.receptionMessage();
                        buffer += Client.message;
                    }

                    //Envoie des messages
                    foreach (Reseau Client in Clients)
                    {
                        Client.envoieMessage(buffer);
                    }
                    i++;
                    Thread.Sleep(10);

                    if (i % 100 == 0)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch
            { Fin(); }
        }

        public static void Fin()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Une erreur est survenue, veuillez redémarrer le serveur");
                Console.ReadLine();
            }
        }
    }
}

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

namespace Sunday_Bloody_Sunday
{
    class Lobby
    {
        public Input chat;
        public bool Go = false;
        public bool connection = true;
        public int rdy = 0;
        public int id_joueur = 0;
        public int nb_joueur = 1;
        public string[] historique = new string[10];
        public string[] pseudos;
        public Random rand = new Random();
        public int timer;
        public List<string> liste_helps;
        public string tips;

        public Lobby(IPAddress IP, string pseudo)
        {
            int i;
            for (i = 0; i < 10; i++)
            {
                historique[i] = "";
            }
            Reseau.initialisationClient(1337, IP, ref connection);
            int port = System.Convert.ToInt32(Reseau.receptionMessage(ref connection));
            Reseau.Envoie.Close();
            Reseau.initialisationClient(port, IP, ref connection);
            id_joueur = port - 4242;
            Reseau.envoieMessage(id_joueur + pseudo + '|', ref connection);//Envoie du pseudo lié à l'ID
            nb_joueur = System.Convert.ToInt32(Reseau.receptionMessage(ref connection));//Reception du nombre de joueur
            pseudos = new string[nb_joueur];
            string buffer = Reseau.receptionMessage(ref connection);
            i = 0;
            int i2 = 0;
            while (i < buffer.Length)
            {
                try
                {
                    i2 = System.Convert.ToInt32("" + buffer[i]);
                }
                catch
                {
                    if (buffer[i] != '|')
                        pseudos[i2] += buffer[i];
                }
                finally
                {
                    i++;
                }
            }
            Reseau.nb_joueurs = nb_joueur;
            chat = new Input(30);

            this.timer = 0;
            this.liste_helps = new List<string>();
            this.tips = "";
        }

        public void Envoie(bool message, int idmap, int rdy, ref bool connection)//| + id_joueur + _ + rdy + _ + idmap + _ + message
        {
            if (!Go)
            {
                this.rdy = rdy;
                string buffer = "|";
                buffer += System.Convert.ToString(id_joueur);
                buffer += '_';
                buffer += System.Convert.ToString(rdy);
                buffer += '_';
                buffer += System.Convert.ToString(idmap);
                buffer += '_';
                if (message)
                {
                    buffer += chat.input;
                    chat.input = "";
                }
                Reseau.envoieMessage(buffer, ref connection);
            }
            else
            {
                Reseau.envoieMessage("42", ref connection);
            }
        }

        public bool Reception()
        {
            string message = Reseau.receptionMessage(ref connection);
            if (message == "")
                connection = false;
            if (Go)
            { return true; }
            else
            {
                if (connection)
                {
                    string[] joueurs = message.Split(new char[1] { '|' });
                    //Check des rdy, 4ème char
                    int i = 1;
                    bool Go1 = true;
                    while (i <= nb_joueur)
                    {
                        Go1 = Go1 && (joueurs[i][2] == '1');
                        i++;
                    }
                    if (Go1)
                    {
                        i = 2;
                        char buffer = joueurs[1][4];
                        while (i <= nb_joueur)
                        {
                            Go1 = Go1 && (buffer == joueurs[i][4]);
                            i++;
                        }
                        Go = Go1;
                    }
                    i = 0;
                    while (i < joueurs.Length)
                    {
                        gestionChat(joueurs[i], i - 1);
                        i++;
                    }
                }
                return false;
            }
        }

        public void gestionChat(string message, int id)
        {
            if (message.Length > 6)
            {
                string buffer = "";
                int i = 6;
                while (i < message.Length)
                {
                    buffer += message[i];
                    i++;
                }

                i = 0;
                while (i < historique.Length - 1)
                {
                    historique[i] = historique[i + 1];
                    i++;
                }
                historique[i] = pseudos[id] + " : " + buffer;
            }
        }

        public void Update(KeyboardState keyboard, KeyboardState old)
        {
            chat.Update(keyboard, old);

            if (timer <= 300)
            {
                timer++;
                if (timer == 300)
                {
                    ArrayHelps();
                    Random rand = new Random();
                    int i = rand.Next(0, 9);
                    tips = liste_helps[i];
                    timer = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            chat.DrawButton(spriteBatch, 50, Divers.HeightScreen / 2 + 150, 0.5f);
            int i = 0;
            while (i < 10)
            {
                spriteBatch.DrawString(Ressources.HUD, historique[i], new Vector2(50, (i + 1) * 30), Color.White, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(Ressources.HUD, tips, new Vector2(Divers.WidthScreen / 2 + 50, Divers.HeightScreen / 2 + 50), Color.White, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0f);
                i++;
            }
        }

        public void ArrayHelps()
        {
            try
            {
                StreamReader streamReader = new StreamReader("Content/helps/helps.txt");
                string line = streamReader.ReadLine();

                while (line != null)
                {
                    liste_helps.Add(line);
                    line = streamReader.ReadLine();
                }
            }
            catch
            {

            }
        }
    }
}

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
    class Reseau
    {
        public static int nb_joueurs;
        public static bool online;
        public static List<Keys>[] Liste(string dl)
        {
            List<Keys>[] liste = new List<Keys>[nb_joueurs];

            try
            {
                int i = 0;

                foreach (char a in dl)
                {
                    if (a == 'u')
                    {
                        liste[i].Add(Keys.Up);
                    }
                    else if (a == 'd')
                    {
                        liste[i].Add(Keys.Down);
                    }
                    else if (a == 'l')
                    {
                        liste[i].Add(Keys.Left);
                    }
                    else if (a == 'r')
                    {
                        liste[i].Add(Keys.Right);
                    }
                    else if (a == 'n')
                    {
                        liste[i].Add(Keys.N);
                    }
                    else if (a == 'p')
                    {
                        liste[i].Add(Keys.P);
                    }
                    else if (a == 'e')
                    {
                        liste[i].Add(Keys.Enter);
                    }
                    else if (a == 't')
                    {
                        liste[i].Add(Keys.T);
                    }
                    else if (a == 'f')
                    {
                        liste[i].Add(Keys.F1);
                    }
                    else
                    {
                        i = a - 48;
                        liste[i] = new List<Keys>();
                    }
                }
            }
            catch
            {
            }

            return liste;
        }

        public static Socket Envoie;

        public static void initialisationClient(int port, IPAddress IP, ref bool connection)
        {
            try
            {
                Envoie = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
                Envoie.Connect(IP,port);
                Envoie.NoDelay = true;
                online = true;
            }
            catch
            {
                try
                {
                    Envoie.Close();
                }
                catch { };

                connection = false;
            }
        }

        public static void envoieMessage(string message, ref bool connection)
        {
            try
            {
                byte[] messageLength = BitConverter.GetBytes(message.Length);
                Envoie.Send(messageLength);

                byte[] messageData = System.Text.Encoding.UTF8.GetBytes(message);
                Envoie.Send(messageData);
            }
            catch
            {
                try
                {
                    Envoie.Close();
                }
                catch { };
                connection = false;
            }
        }


        public static string receptionMessage(ref bool connection)
        {
            try
            {
                byte[] messageLengthData = new byte[4];

                Envoie.Receive(messageLengthData);
                int messageLength = BitConverter.ToInt32(messageLengthData, 0);

                byte[] messageData = new byte[messageLength];
                Envoie.Receive(messageData);

                return System.Text.Encoding.UTF8.GetString(messageData);
            }
            catch
            {
                connection = false;
                try { Envoie.Close(); }
                catch { }
                return "0";
            }
        }

        public static void Clear()
        {
            if (online)
            {
                try
                {
                    Envoie.Close();
                }
                catch
                {
                }
                online = false;
            }
        }
    }
}

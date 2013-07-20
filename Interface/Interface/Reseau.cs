using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Interface
{
    class Reseau
    {
        public string message;

        public Socket Reception;
        public Socket Attente;

        public Reseau()
        {
        }
        

        public void intialisationServeur(int port)
        {
            Attente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Attente.Bind(new IPEndPoint(IPAddress.Any, port));
            Attente.Listen(42);
            Reception = Attente.Accept();
            Reception.NoDelay = true;
        }

        public void receptionMessage()
        {
            try
            {
                byte[] messageLengthData = new byte[4];

                Reception.Receive(messageLengthData);
                int messageLength = BitConverter.ToInt32(messageLengthData, 0);

                byte[] messageData = new byte[messageLength];
                Reception.Receive(messageData);

                message = System.Text.Encoding.UTF8.GetString(messageData);
            }
            catch
            {
                foreach (Reseau Client in Program.Clients)
                {
                    Client.Clear();
                }
                Program.Fin();
            }
        }
        
        public void envoieMessage(string message)
        {
            try
            {
                byte[] messageLength = BitConverter.GetBytes(message.Length);
                Reception.Send(messageLength);

                byte[] messageData = System.Text.Encoding.UTF8.GetBytes(message);
                Reception.Send(messageData);
            }
            catch
            {
                foreach (Reseau Client in Program.Clients)
                {
                    Client.Clear();
                }
                Program.Fin();
            }
        }

        public void Clear()
        {
            try
            {
                Attente.Close();
            }
            catch
            {
            }
            try
            {
                Reception.Close();
            }
            catch
            {
            }
        }
    }
}

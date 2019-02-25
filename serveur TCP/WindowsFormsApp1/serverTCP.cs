using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class serverTCP
    {
        private Form1 form;
        private int port;
        private Dictionary<int,TcpClient> clients;
        private TcpListener server;
        private Dictionary<int,NetworkStream> mainStreams;
        private Dictionary<int,Thread> getMessage;
        private Thread listening;

        public void StartServer(string ip, int p,Form1 f)
        {
            form = f;

            port = p;
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            clients = new Dictionary<int, TcpClient>();
            mainStreams = new Dictionary<int, NetworkStream>();
            getMessage = new Dictionary<int, Thread>();
            listening = new Thread(StartListening);
            listening.Start();

        }
        private void StartListening()
        {
            server.Start();
            Byte[] bytes = new Byte[256];
            int Id;
            TcpClient client = new TcpClient();
            while (true)
            {
                try
                {
                    client = server.AcceptTcpClient();

                    Id = 0;
                    NetworkStream stream = client.GetStream();
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    ArrayList message = (ArrayList)binaryFormatter.Deserialize(stream);
                    Id = (int)message[0];
                    Console.WriteLine(Id);
                    //if (!clients.ContainsKey(Id))
                    //{
                        clients[Id] = client;
                        mainStreams[Id] = stream;
                        Console.WriteLine("client : " + Id + " connected");
                        getMessage[Id] = new Thread(() => ListeningClient(Id));
                        getMessage[Id].Start();

                        BinaryFormatter binaryFormattet = new BinaryFormatter();
                        ArrayList messageR = new ArrayList();
                        messageR.Add(true);
                        binaryFormattet.Serialize(stream, messageR);
                        //form.WriteInLogBox("client : " + Id + " connected");
                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes("hello");
                        //stream.Write(msg, 0, msg.Length);
                    /*}
                    else
                    {
                        Console.WriteLine("Client avec cet identifiant déjà connécté");
                        //form.WriteInLogBox("client : " + Id + " connected");
                        BinaryFormatter binaryFormattet = new BinaryFormatter();
                        ArrayList messageR = new ArrayList();
                        messageR.Add(false);
                        binaryFormattet.Serialize(stream, messageR);
                        client.Close();
                    }*/
                }
                catch
                {
                    break;
                }


            }
        }
        private void ListeningClient(int id)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            while (clients.ContainsKey(id) && clients[id].Connected)
            {
                if (clients.ContainsKey(id) && clients[id].Available > 0)
                {
                    try
                    {
                        ArrayList message = (ArrayList)binaryFormatter.Deserialize(mainStreams[id]);
                        if (message[0] is int)
                        {

                            if (clients.ContainsKey((int)message[1]))
                            {
                                SendMessageTo(message);
                            }
                            if (message[2] is string)
                            {
                                System.Console.WriteLine((int)message[0] + " to " + (int)message[1] + " : " + (string)message[2]);
                                System.Console.WriteLine();
                            }
                            else if (message[2] is Bitmap)
                            {
                                System.Console.WriteLine((int)message[0] + " to " + (int)message[1] + " : Bitmap");

                            }
                            else if (message[2] is bool)
                            {
                                System.Console.WriteLine((int)message[0] + " to " + (int)message[1] + " : " + (bool)message[2] + " - " + (int)message[3]);
                            }
                        }
                        else if (message[0] is string)
                        {
                            System.Console.WriteLine("déconnection client:" + (int)message[1]);
                            if ((string)message[0] == "disconnect") CloseClient((int)message[1]);

                        }
                    }
                    catch
                    {
                    }

                    
                    
                    
                }
            }
            CloseClient(id);
            

        }

        public void SendMessageTo(ArrayList message)
        {
            BinaryFormatter binaryFormattet = new BinaryFormatter();
            ArrayList messageR = new ArrayList();
            messageR.Add(message[0]);
            messageR.Add(message[2]);
            if (message[2] is bool) messageR.Add(message[3]);
            try
            {
                binaryFormattet.Serialize(mainStreams[(int)message[1]], messageR);
            }
            catch
            {
                CloseClient((int)message[1]);
            }
        }

        public void CloseClient(int id)
        {
            
            if (mainStreams.ContainsKey(id))
            {
                mainStreams[id].Close();
                mainStreams.Remove(id);
            }
            if (clients.ContainsKey(id))
            { 
                clients[id].Close();
                clients.Remove(id);               
            }
            if (getMessage.ContainsKey(id))
            {
                if (getMessage[id].IsAlive) getMessage[id].Abort();
                getMessage.Remove(id);
            }


        }
        public void CloseServer()
        {
            server.Stop();
            /*foreach (KeyValuePair<int,TcpClient> entry in clients)
            {
                CloseClient(entry.Key);
            }*/

            
        }
    }
}

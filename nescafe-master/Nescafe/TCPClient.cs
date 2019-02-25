using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace Nescafe
{
    class TCPClient
    {
        private NetworkStream stream;
        private TcpClient client;
        private int _myId;
        private int _idReceive;
        //private Keyboard keyboard;
        private int _myPort;
        private string _ipAddress;

        private Thread getMessage;
        private Thread getkey;
        private Console _console;
        bool clientStart;
        public TCPClient(Console console,int myId,int idReceive, string ipAddress, int port)
        {
            _myPort = port;
            _myId = myId;
            _idReceive = idReceive;
            _ipAddress = ipAddress;
            _console = console;
            clientStart = false;
            try
            {
                StartClient();
            }
            catch
            {
                System.Console.WriteLine("pas connecté au serveur");
            }
        }
        private void StartClient()
        {
            
            client = new TcpClient();
            try
            {
                client.Connect(_ipAddress, _myPort);
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(id);
                stream = client.GetStream();

                BinaryFormatter binaryFormattet = new BinaryFormatter();
                ArrayList message = new ArrayList();
                message.Add(_myId);
                binaryFormattet.Serialize(stream, message);
                ArrayList messageR = (ArrayList)binaryFormattet.Deserialize(stream);
                if ((bool)messageR[0])
                {
                    getMessage = new Thread(ReceiveMessage);
                    getMessage.Start();
                }
                else
                {
                    //Console.WriteLine("Client avec cet id, déja pris");
                    CloseClient();
                }
            }
            catch
            {
                //Console.WriteLine("pas de connection au serveur");
                client = null;
                MessageBox.Show("Fail connection to server");
            }

            //stream.Write(data, 0, data.Length);

        }
        public void SendMessage( string text)
        {
            if (client != null && client.Connected)
            {
                BinaryFormatter binaryFormattet = new BinaryFormatter();
                ArrayList message = new ArrayList();
                message.Add(_myId);
                message.Add(_idReceive);
                message.Add(text);
                binaryFormattet.Serialize(stream, message);
            }
            else
            {
                //Console.WriteLine("client non connécté");
            }
        }

        public void SendBitmap(Bitmap bits)
        {
            if (client != null && client.Connected)
            {
                BinaryFormatter binaryFormattet = new BinaryFormatter();
                ArrayList message = new ArrayList();
                message.Add(_myId);
                message.Add(_idReceive);
                message.Add(bits);
                binaryFormattet.Serialize(stream, message);
            }
            else
            {
                //Console.WriteLine("client non connécté");
            }
        }

        public void ReceiveMessage()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            while ((client != null && stream != null) && client.Connected)
            {
                try
                {
                    ArrayList message = (ArrayList)binaryFormatter.Deserialize(stream);
                    if (message[1] is string)
                    {
                        System.Console.WriteLine((int)message[0] + " : " + (string)message[1]);
                        System.Console.WriteLine();

                        // call function if keyboard
                    }
                    else if (message[1] is Bitmap)
                    {
                        System.Console.WriteLine((int)message[0] + " send Bitmap ");
                        System.Console.WriteLine();

                        // call function if Bitmap
                    }
                    else if (message[1] is bool)
                    {
                        System.Console.WriteLine((int)message[0] + ": " + (bool)message[1] + "-" + (int)message[2]);
                        System.Console.WriteLine();
                        SetControllerButton((bool)message[1], (int)message[2]);
                    }
                }
                catch
                {
                    client.Close();
                    MessageBox.Show("Connection closed with server");
                }
            }
        }


        void SetControllerButton(bool state, int e)
        {
            switch (e)
            {
                case 65: //A
                    _console.Controller.setButtonState(Controller.Button.A, state);
                    break;
                case 90: //Z
                    _console.Controller.setButtonState(Controller.Button.B, state);
                    break;
                case 37: //left
                    _console.Controller.setButtonState(Controller.Button.Left, state);
                    break;
                case 39: //right
                    _console.Controller.setButtonState(Controller.Button.Right, state);
                    break;
                case 38: //up
                    _console.Controller.setButtonState(Controller.Button.Up, state);
                    break;
                case 40: //down
                    _console.Controller.setButtonState(Controller.Button.Down, state);
                    break;
                case 81: //Q
                    _console.Controller.setButtonState(Controller.Button.Start, state);
                    break;
                case 83: //S
                    _console.Controller.setButtonState(Controller.Button.Select, state);
                    break;
            }
        }

        /* public void StartSendKey()
         {
             getkey = new Thread(sendKeyboard);
             keyboard = new Keyboard();
             getkey.Start();
         }
         public void sendKeyboard()
         {
             while (true)
             {
                 ArrayList key = keyboard.getKey();
                 if (client != null && client.Connected && key != null)
                 {
                     BinaryFormatter binaryFormattet = new BinaryFormatter();
                     ArrayList message = new ArrayList();
                     message.Add(myId);
                     message.Add(idReceive);
                     message.Add(key[0]);
                     message.Add(key[1]);
                     binaryFormattet.Serialize(stream, message);
                 }
                 else
                 {
                     Console.WriteLine("client non connécté");
                 }
             }
         }*/

        public void CloseClient()
        {
            try
            {
                BinaryFormatter binaryFormattet = new BinaryFormatter();
                ArrayList message = new ArrayList();


                message.Add("disconnect");
                message.Add(_myId);
                binaryFormattet.Serialize(stream, message);

                if (getMessage.IsAlive) getMessage.Abort();
                if (client.Connected)
                {
                    stream.Close();
                    client.Close();
                }
            }
            catch
            {
            }


        }
    }
}

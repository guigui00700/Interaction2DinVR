using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Drawing;

public class TCPClient : MonoBehaviour {

    private NetworkStream stream;
    private TcpClient client;
    public int myId;
    public int idReceive;
    public string ipAddress;
    private Keyboard keyboard;
    public int myPort;

    private Thread getMessage;
    private Thread getkey;


    private Dictionary<int, System.Drawing.Image> local_image;
    private Dictionary<int, bool> isImageCanUse;
    private MeshRenderer meshRenderer;

    private Texture2D texture;
    private byte[] imageData;
    System.Drawing.ImageConverter imageConverter;

    // Use this for initialization
    void Start () {
        try
        {
            StartClient();
            keyboard = new Keyboard();
        }
        catch
        {
            Console.WriteLine("pas de connection");
        }


        local_image = new Dictionary<int, System.Drawing.Image>();
        isImageCanUse = new Dictionary<int, bool>();

        local_image[0] = System.Drawing.Image.FromFile(Application.dataPath + "/Resources/1.jpg");
        isImageCanUse[0] = false;
        texture = new Texture2D(local_image[0].Width, local_image[0].Height);
        imageConverter = new System.Drawing.ImageConverter();
        imageData = (byte[])imageConverter.ConvertTo(local_image[0], typeof(byte[]));
        texture.LoadImage(imageData);

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = texture;

    }
	
	// Update is called once per frame
	void Update () {
        keyboard.getKey(client, stream,myId,idReceive);
        Debug.Log("Update : " + isImageCanUse);
        if (isImageCanUse[0])
        {
            Debug.Log("writing image");
            texture.Resize(local_image[0].Width, local_image[0].Height);
            imageConverter = new System.Drawing.ImageConverter();
            imageData = (byte[])imageConverter.ConvertTo(local_image[0], typeof(byte[]));
            texture.LoadImage(imageData);
            //spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //print(1);
            meshRenderer.material.mainTexture = texture;
        }
    }

    public void writeImage(Bitmap frame)
    {
        isImageCanUse[0] = false;
        local_image[0] = (System.Drawing.Image)frame;
        isImageCanUse[0] = true;
        Debug.Log("writeImage : " + isImageCanUse);
    }


    public void StartClient()
    {
        client = new TcpClient();
        try
        {
            client.Connect(ipAddress, myPort);
            //Byte[] data = System.Text.Encoding.ASCII.GetBytes(id);
            stream = client.GetStream();

            BinaryFormatter binaryFormattet = new BinaryFormatter();
            ArrayList message = new ArrayList();
            message.Add(myId);
            binaryFormattet.Serialize(stream, message);
            ArrayList messageR = (ArrayList)binaryFormattet.Deserialize(stream);
            if ((bool)messageR[0])
            {
                getMessage = new Thread(ReceiveMessage);
                getMessage.Start();
            }
            else
            {
                Console.WriteLine("Client avec cet id, déja pris");
                CloseClient();
            }
        }
        catch
        {
            Console.WriteLine("pas de connection au serveur");
            client = null;
        }

        //stream.Write(data, 0, data.Length);

    }
    public void SendMessage(string text)
    {
        if (client != null && client.Connected)
        {
            BinaryFormatter binaryFormattet = new BinaryFormatter();
            ArrayList message = new ArrayList();
            message.Add(myId);
            message.Add(idReceive);
            message.Add(text);
            binaryFormattet.Serialize(stream, message);
        }
        else
        {
            Console.WriteLine("client non connécté");
        }
    }

    public void SendBitmap(Bitmap bits)
    {
        if (client != null && client.Connected)
        {
            BinaryFormatter binaryFormattet = new BinaryFormatter();
            ArrayList message = new ArrayList();
            message.Add(myId);
            message.Add(idReceive);
            message.Add(bits);
            binaryFormattet.Serialize(stream, message);
        }
        else
        {
            Console.WriteLine("client non connécté");
        }
    }

    public void ReceiveMessage()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        while ((client != null && stream != null) && client.Connected)
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
                writeImage((Bitmap)message[1]);
                // call function if Bitmap
            }
            else if (message[1] is bool)
            {
                System.Console.WriteLine((int)message[0] + ": " + (bool)message[1] + "-" );
                System.Console.WriteLine();
            }
        }
    }
    /*public void sendKeyboard()
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
        BinaryFormatter binaryFormattet = new BinaryFormatter();
        ArrayList message = new ArrayList();


        message.Add("disconnect");
        message.Add(myId);
        binaryFormattet.Serialize(stream, message);

        if (getMessage.IsAlive) getMessage.Abort();
        if (client.Connected)
        {
            stream.Close();
            client.Close();
        }


    }
}

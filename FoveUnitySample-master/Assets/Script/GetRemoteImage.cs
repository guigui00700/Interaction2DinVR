using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GetRemoteImage : MonoBehaviour
{
    private int port;
    private TcpClient client;
    private TcpListener server;
    private NetworkStream mainStream;

    public bool isImageCanUse;
    private Thread Listening;
    private Thread GetImage;

    public Button BConnectImage;
    public Button BDisconnectImage;

    //public Image image;
    //private Sprite sprite;
    //public Button buttonClose;

    //private SpriteRenderer spriteRenderer;
    private System.Drawing.Image local_image;

    private MeshRenderer meshRenderer;
    
    // Use this for initialization
    void Start()
    {
        local_image = System.Drawing.Image.FromFile(Application.dataPath + "\\Resources\\1.jpg");
        isImageCanUse = false;
        port = 50002;
        Listening = new Thread(StartListening);
        GetImage = new Thread(ReceiveImage);
        server = new TcpListener(IPAddress.Any, port);
        Texture2D texture = new Texture2D(local_image.Width, local_image.Height);
        System.Drawing.ImageConverter imageConverter = new System.Drawing.ImageConverter();
        byte[] imageData = (byte[])imageConverter.ConvertTo(local_image, typeof(byte[]));
        texture.LoadImage(imageData);
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f));
        //this.spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f));
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = texture;


        BConnectImage.onClick.AddListener(ConnectImage);
        BDisconnectImage.onClick.AddListener(DisconnectImage);
        //image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f));
    }

    void DisconnectImage(){
        StopListening();
    }
    void ConnectImage(){
        client = new TcpClient();
        Listening.Start();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.LeftControl)){
            ConnectImage();
        }
        if (Input.GetKeyDown(KeyCode.U) && Input.GetKey(KeyCode.LeftControl)){
            DisconnectImage();
        }
        if (isImageCanUse){
            Texture2D texture = new Texture2D(local_image.Width, local_image.Height);
            System.Drawing.ImageConverter imageConverter = new System.Drawing.ImageConverter();
            byte[] imageData = (byte[])imageConverter.ConvertTo(local_image, typeof(byte[]));
            texture.LoadImage(imageData);
            //spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //print(1);
            meshRenderer.material.mainTexture = texture;
        }
    }

    private void StartListening()
    {
        while (!client.Connected)
        {
            server.Start();
            client = server.AcceptTcpClient();
        }
        GetImage.Start();
    }

    private void StopListening()
    {
        server.Stop();
        if (Listening.IsAlive){
            Listening.Abort();
        }
        if (GetImage.IsAlive){
            GetImage.Abort();
        }
        client = null;
    }

    private void ReceiveImage()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        while (client.Connected){
            if (client.Available > 0){
                mainStream = client.GetStream();
                //pictureBox1.Image = (Image)binaryFormatter.Deserialize(mainStream);
                isImageCanUse = false;
                local_image = (System.Drawing.Image)binaryFormatter.Deserialize(mainStream);
                isImageCanUse = true;
            }
        }
    }
}

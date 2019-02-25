using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

public class DrawImage : MonoBehaviour {
    private Dictionary<int,System.Drawing.Image> local_image;
    private Dictionary<int,bool> isImageCanUse;
    private MeshRenderer meshRenderer;
    // Use this for initialization
    void Start () {
        local_image = new Dictionary<int, System.Drawing.Image>();
        isImageCanUse = new Dictionary<int, bool>();

        local_image[0] = System.Drawing.Image.FromFile(Application.dataPath + "\\Resources\\1.jpg");
        isImageCanUse[0] = false;
        Texture2D texture = new Texture2D(local_image[0].Width, local_image[0].Height);
        System.Drawing.ImageConverter imageConverter = new System.Drawing.ImageConverter();
        byte[] imageData = (byte[])imageConverter.ConvertTo(local_image[0], typeof(byte[]));
        texture.LoadImage(imageData);

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = texture;
    }

    public void writeImage(Bitmap frame)
    {
        isImageCanUse[0] = false;
        local_image[0] = (System.Drawing.Image)frame;
        isImageCanUse[0] = true;
        Debug.Log("writeImage : " + isImageCanUse);
    }
	// Update is called once per frame
	void Update () {
        Debug.Log("Update : " + isImageCanUse);
        if (isImageCanUse[0])
        {
            Debug.Log("writing image");
            Texture2D texture = new Texture2D(local_image[0].Width, local_image[0].Height);
            System.Drawing.ImageConverter imageConverter = new System.Drawing.ImageConverter();
            byte[] imageData = (byte[])imageConverter.ConvertTo(local_image[0], typeof(byte[]));
            texture.LoadImage(imageData);
            //spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            //print(1);
            meshRenderer.material.mainTexture = texture;
        }
    }
}

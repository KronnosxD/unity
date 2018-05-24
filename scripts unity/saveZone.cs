using Newtonsoft.Json.Linq;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using System;
using UnityEngine.Networking;

public class saveZone : MonoBehaviour
{
    public GameObject player;
    public JArray objetos, ammoBag, progress;
    public broker brokerConexion;
    public string playerId;
    string playerInfo;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //brokerConexion = GameObject.FindGameObjectWithTag("broker").GetComponent<broker>();
        objetos = new JArray();
        ammoBag = new JArray();
        progress = new JArray();
        System.Text.UTF8Encoding Byte_Transform = new System.Text.UTF8Encoding();
        


    }

    // Update is called once per frame
    void Update()
    {
        objetos = player.GetComponent<playerController>().objetos;
        ammoBag = player.GetComponent<playerController>().ammoBag;
        progress = player.GetComponent<playerController>().progress;
    }
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            player.GetComponent<playerController>().onSaveZone = true;
            if (Input.GetKeyUp(KeyCode.E))
            {
                StartCoroutine(saveGame(player.transform.position.x, player.transform.position.y, player.transform.position.z));

            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        player.GetComponent<playerController>().onSaveZone = false;
    }
    IEnumerator saveGame(float x, float y, float z)
    {



        JObject playerData = new JObject(

            new JProperty("playerId", playerId),
            new JProperty("position",
                new JObject(
                        new JProperty("posX", x)
                    ),
                new JObject(
                        new JProperty("posY", y)
                    ),
                new JObject(
                        new JProperty("posZ", z)
                    )
                ),
             new JProperty("inventory",
                 new JArray(
                         from items in objetos
                         select new JObject(
                                 new JProperty("itemId", items)
                             )
                     )
             ),
             new JProperty("sceneName", "escena por defecto"),
             new JProperty("currentLife", player.GetComponent<playerController>().life),
             new JProperty("money", player.GetComponent<playerController>().money),
             new JProperty("deathsCounter", player.GetComponent<playerController>().deathsCounter),
             new JProperty("ammoBag",
                 new JArray(
                         from items in ammoBag
                         select new JObject(
                                 new JProperty("itemId", items)
                             )
                     )
             ),
             new JProperty("progress",
                 new JArray(
                         from items in progress
                         select new JObject(
                                 new JProperty("itemId", items)
                             )
                     )
             )
        );

        playerInfo = playerData.ToString();
        encriptar();
        yield return new WaitForSeconds(2.0F);


        
        Debug.Log(playerId.Trim().ToString());
        WWWForm form = new WWWForm();
        form.AddField("x", x.ToString());
        form.AddField("y", y.ToString());
        form.AddField("z", z.ToString());
        Debug.Log("http://localhost:8080/api/playerInfo/savePos/" + playerId);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/playerInfo/savePos/" + playerId.Trim().ToString(), form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log("No se ha podido establecer conexión con el servidor, la partida no se sincronizará con la nuba pero si será guardada en tu PC");
        }
        else
        {
            Debug.Log("Posición Guardada");
        }
       
    }

    public void encriptar()
    {
        string Plain_Text;
        string Decrypted;
        string Encrypted_Text;
        byte[] Encrypted_Bytes;


        RijndaelManaged Crypto = new RijndaelManaged();

        System.Text.UTF8Encoding UTF = new System.Text.UTF8Encoding();

        Plain_Text = playerInfo;

        try
        {
            Encrypted_Bytes = encrypt_function(Plain_Text, Crypto.Key, Crypto.IV);
            Encrypted_Text = UTF.GetString(Encrypted_Bytes);
            Decrypted = decrypt_function(Encrypted_Bytes, Crypto.Key, Crypto.IV);

            string path = @"c:\temp\encrypted.k4la";
            if (!File.Exists(path))
            {

                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(Encrypted_Text.ToString());
                    Debug.Log("Encriptado guardado en " + path);
                }
            }
            string path2 = @"c:\temp\decrypted.txt";
            if (!File.Exists(path2))
            {

                using (StreamWriter sw = File.CreateText(path2))
                {
                    sw.WriteLine(Decrypted.ToString());
                    Debug.Log("Desencriptado guardado en " + path2);
                }
            }


        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
    private static byte[] encrypt_function(string Plain_Text, byte[] Key, byte[] IV)
    {
        RijndaelManaged Crypto = null;
        MemoryStream MemStream = null;

        ICryptoTransform Encryptor = null;
        CryptoStream Crypto_Stream = null;

        System.Text.UTF8Encoding Byte_Transform = new System.Text.UTF8Encoding();
        byte[] PlainBytes = Byte_Transform.GetBytes(Plain_Text);
        try
        {
            Crypto = new RijndaelManaged();
            Crypto.Key = Key;
            Debug.Log("KEY: "+Crypto.Key.ToString());
            Crypto.IV = IV;
            MemStream = new MemoryStream();

            Encryptor = Crypto.CreateEncryptor(Crypto.Key, Crypto.IV);

            Crypto_Stream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);

            Crypto_Stream.Write(PlainBytes, 0, PlainBytes.Length);


        }
        finally
        {
            if (Crypto != null)
                Crypto.Clear();

            Crypto_Stream.Close();


        }
        return MemStream.ToArray();

        
        }
    private static string decrypt_function(byte[] Cipher_Text, byte[] Key, byte[] IV)
    {
        RijndaelManaged Crypto = null;
            MemoryStream MemStream = null;
            ICryptoTransform Decryptor = null;
            CryptoStream Crypto_Stream = null;
            StreamReader Stream_Read = null;
        String Plain_Text;
        try
        {
                 Crypto = new RijndaelManaged();
                  Crypto.Key = Key;
                 Crypto.IV = IV;
                MemStream = new MemoryStream(Cipher_Text);

            Decryptor = Crypto.CreateDecryptor(Crypto.Key, Crypto.IV);

            Crypto_Stream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);

            Stream_Read = new StreamReader(Crypto_Stream);
                 Plain_Text = Stream_Read.ReadToEnd();


        }
        finally
        {
                if (Crypto != null)
                    Crypto.Clear();
                MemStream.Flush();
                MemStream.Close();

        }
        return Plain_Text;

    }

}

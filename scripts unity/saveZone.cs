using Newtonsoft.Json.Linq;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class saveZone : MonoBehaviour
{
    public Scene scene;
    string localPath;
    public GameObject player;
    public JArray objetos, ammoBag, progress;
    public broker brokerConexion;
    public string playerId, userId;
    // Use this for initialization
    void Start()
    {
        localPath = @"c:\Kala\gameData\saves\local";
        scene = SceneManager.GetActiveScene();
        player = GameObject.FindGameObjectWithTag("Player");
        //brokerConexion = GameObject.FindGameObjectWithTag("broker").GetComponent<broker>();
        objetos = new JArray();
        objetos = player.GetComponent<playerController>().objetos;
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
                StartCoroutine(saveGame(
                    player.transform.position.x, 
                    player.transform.position.y, 
                    player.transform.position.z,
                    player.GetComponent<playerController>().money,
                    player.GetComponent<playerController>().deathsCounter,
                    player.GetComponent<playerController>().life,
                    player.GetComponent<playerController>().objetos,
                    player.GetComponent<playerController>().progress,
                    player.GetComponent<playerController>().ammoBag
                    ));

            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        player.GetComponent<playerController>().onSaveZone = false;
    }
    IEnumerator saveGame(float x, float y, float z, int dinero, int contadorMuertes, int vidaActual, 
        JArray inventario, JArray progreso, JArray bolsaMunicion)
    {

        try
        {
            saveLocal(x, y, z, dinero, contadorMuertes, vidaActual, inventario, progreso, bolsaMunicion);
        } catch(Exception ex)
        {
            Debug.Log(ex);
        }
      
        // CONSTRUIR API PARA GUARDAR LA PARTIDA CON TODOS LOS DATOS!
        
        
        Debug.Log(playerId.Trim().ToString());
        WWWForm form = new WWWForm();
        form.AddField("playerId", playerId.Trim().ToString());
        form.AddField("currentLife", vidaActual);
        form.AddField("money", dinero);
        form.AddField("deathCounter", contadorMuertes);
        form.AddField("inventory", objetos.ToString());
        form.AddField("ammoBag", bolsaMunicion.ToString());
        form.AddField("progress", progreso.ToString());
        form.AddField("x", x.ToString());
        form.AddField("y", y.ToString());
        form.AddField("z", z.ToString());
        
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/player/saveGame/" + userId.Trim().ToString(), form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log("No se ha podido establecer conexión con el servidor, la partida no se sincronizará con la nuba pero si será guardada en tu PC");
        }
        else
        {
            Debug.Log("");
            string data = www.downloadHandler.text;
            JObject obj = JObject.Parse(data);

            //string playerId = obj["informacion"]["position"]["posX"];
            Debug.Log("Posición Guardada");
        }
      

      
    }


    public void saveLocal(float x, float y, float z, int dinero, int contadorMuertes, int vidaActual,
        JArray inventario, JArray progreso, JArray bolsaMunicion)
    {
        Boolean go = false;
        //Comprobar si existe la carpeta con datos del juego
        
        if (File.Exists(localPath))
        {
            go = true;
        }
        else
        {
            System.IO.Directory.CreateDirectory(localPath);
            go = true;
        }
        if (go)
        {
            DateTime fecha = DateTime.Now;
            JObject playerData = new JObject(
                new JProperty("userId", userId),
                new JProperty("_id", playerId),
                new JProperty("saveInfo", 
                    new JObject(

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
                     new JProperty("sceneName", scene.name),
                     new JProperty("currentLife", vidaActual),
                     new JProperty("money", dinero),
                     new JProperty("deathsCounter", contadorMuertes),
                     new JProperty("ammoBag",
                         new JArray(
                                 from items in bolsaMunicion
                                 select new JObject(
                                         new JProperty("itemId", items)
                                     )
                             )
                     ),
                     new JProperty("progress",
                         new JArray(
                                 from items in progreso
                                 select new JObject(
                                         new JProperty("itemId", items)
                                     )
                             )
                     ),
                     new JProperty("saveDate", fecha)
                     )
                )

            );
            encryptAndSave(playerData.ToString());
        }
        
    }

    public void encryptAndSave(string playerInfo)
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

            int qty = (Directory.GetFiles(localPath, "*", SearchOption.AllDirectories).Length + 1);
            string saveName = "\\save"+qty+".k4";
            string savePath = localPath +saveName;
            if (!File.Exists(savePath))
            {

                using (StreamWriter sw = File.CreateText(savePath))
                {
                    sw.WriteLine(Encrypted_Text.ToString());
                    Debug.Log("Encriptado guardado en " + savePath);
                }
            }
            string path2 = @"c:\temp";
            string saveTempName = "\\decryptedFromSave-" + qty + ".txt";
            string saveTempPath = path2 + saveTempName;
            if (!File.Exists(saveTempPath))
            {

                using (StreamWriter sw = File.CreateText(saveTempPath))
                {
                    sw.WriteLine(Decrypted.ToString());
                    Debug.Log("Desencriptado guardado en " + saveTempPath);
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

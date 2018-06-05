using Newtonsoft.Json.Linq;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text;

public class saveZone : MonoBehaviour
{
    public Scene scene;
    string localPath;
    public GameObject player, fakeBroker;
    public JArray objetos, ammoBag, progress;
    public broker brokerConexion;
    public string playerId, userId;

    // Use this for initialization
    void Start()
    {
        fakeBroker = GameObject.FindGameObjectWithTag("fakeBroker");
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
                    (float)Math.Round(player.transform.position.x,4), 
                    (float)Math.Round(player.transform.position.y,4),
                    (float)Math.Round(player.transform.position.z,4),
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
        Debug.Log(objetos);
        
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
                    new JArray(
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
                )
            );

            string encriptado = AvoEx.AesEncryptor.Encrypt(playerData.ToString());
            string desencriptado = AvoEx.AesEncryptor.DecryptString(encriptado);
            try
            {

                // Open the text file using a stream reader.


                int qty = (Directory.GetFiles(localPath, "*", SearchOption.AllDirectories).Length + 1);
                string saveName = "\\save" + qty + ".k4";
                string savePath = localPath + saveName;
                if (!File.Exists(savePath))
                {

                    using (StreamWriter sw = File.CreateText(savePath))
                    {
                        sw.WriteLine(encriptado);
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
                        sw.WriteLine(desencriptado);
                        Debug.Log("Desencriptado guardado en " + saveTempPath);
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }



        }

    }
    
    
    
}

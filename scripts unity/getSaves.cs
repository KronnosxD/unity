using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine;

public class getSaves : MonoBehaviour {
    public string savesRoute;
    public GameObject fakeBroker, partidaTemplate, content;
    public int qty;
    public string[] datos, partidasLocales;
    public JObject[] obj;


    // Use this for initialization
    void Start() {
        savesRoute = @"c:\Kala\gameData\saves\local";
        fakeBroker = GameObject.FindGameObjectWithTag("fakeBroker");

       
        
    }
	
	// Update is called once per frame
	void Update () {
       
	}
    public void getAndReadLocalSaves()
    {
        qty = (Directory.GetFiles(savesRoute, "*.k4", SearchOption.AllDirectories).Length);
        obj = new JObject[qty];
        datos = new string[qty];
        partidasLocales = new string[qty];

        try
        {
            for (int x = 0; x < (Directory.GetFiles(savesRoute, "*.k4", SearchOption.AllDirectories).Length); x++)
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(savesRoute+"\\save" + (x+1) + ".k4"))
                {

                    // Read the stream to a string, and write the string to the console.
                    datos[x] = sr.ReadToEnd();
                    partidasLocales[x] = AvoEx.AesEncryptor.DecryptString(datos[x]);
                    
                    obj[x] = JObject.Parse(partidasLocales[x]);
 
                }


            }
  

            for (var y=0; y<partidasLocales.Length; y++)
            {

                var copiaPartida = Instantiate(partidaTemplate);
                copiaPartida.transform.SetParent(content.transform, false);
                copiaPartida.transform.localPosition = Vector3.zero;

                copiaPartida.GetComponent<savegameData>().indexPartida = y;
                copiaPartida.GetComponent<savegameData>().vidaActual = (int)obj[y]["saveInfo"][0]["currentLife"];
                copiaPartida.GetComponent<savegameData>().contadorMuertes = (int)obj[y]["saveInfo"][0]["deathsCounter"];

                copiaPartida.GetComponent<savegameData>().newPlayerPosition.x = (float)obj[y]["saveInfo"][0]["position"][0]["posX"];
                copiaPartida.GetComponent<savegameData>().newPlayerPosition.y = (float)obj[y]["saveInfo"][0]["position"][1]["posY"];
                copiaPartida.GetComponent<savegameData>().newPlayerPosition.z = (float)obj[y]["saveInfo"][0]["position"][2]["posZ"];

                copiaPartida.GetComponent<savegameData>().fechaSave = (string)obj[y]["saveInfo"][0]["saveDate"];
                copiaPartida.GetComponent<savegameData>().nombreEscena = (string)obj[y]["saveInfo"][0]["sceneName"];

                copiaPartida.GetComponent<savegameData>().escena.text = copiaPartida.GetComponent<savegameData>().nombreEscena.ToString();
                copiaPartida.GetComponent<savegameData>().fecha.text = copiaPartida.GetComponent<savegameData>().fechaSave.ToString();

                JArray items = (JArray)obj[y]["saveInfo"][0]["inventory"];
               
                for(var z = 0; z < items.Count; z++)
                {
                    
                    JObject aux = new JObject(
                                new JProperty("itemId", obj[y]["saveInfo"][0]["inventory"][z]["itemId"]["itemId"]),
                                new JProperty("amount", obj[y]["saveInfo"][0]["inventory"][z]["itemId"]["amount"])
                               );
                    Debug.Log(aux.ToString());
                    copiaPartida.GetComponent<savegameData>().objetos.Add(aux);
                }

                JArray ammo = (JArray)obj[y]["saveInfo"][0]["ammoBag"];

                for (var a = 0; a < ammo.Count; a++)
                {

                    JObject aux2 = new JObject(
                                new JProperty("itemId", obj[y]["saveInfo"][0]["ammoBag"][a]["itemId"]),
                                new JProperty("amount", obj[y]["saveInfo"][0]["ammoBag"][a]["amount"])
                               );
                    Debug.Log(aux2.ToString());
                    copiaPartida.GetComponent<savegameData>().municion.Add(aux2);
                }

                JArray progress = (JArray)obj[y]["saveInfo"][0]["progress"];

                for (var b = 0; b < ammo.Count; b++)
                {

                    JObject aux3 = new JObject(
                                new JProperty("itemId", obj[y]["saveInfo"][0]["progress"][b]["itemId"])
                               );
                    Debug.Log(aux3.ToString());
                    copiaPartida.GetComponent<savegameData>().municion.Add(aux3);
                }


            }

         

        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
       
        
    }
    

}

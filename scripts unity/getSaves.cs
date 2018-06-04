using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;


public class getSaves : MonoBehaviour {
    public string savesRoute;
    public GameObject fakeBroker, partidaTemplate, content;
    public int qty;
    public string[] datos, partidasLocales;

    // Use this for initialization
    void Start() {
        savesRoute = @"c:\Kala\gameData\saves\local";
        fakeBroker = GameObject.FindGameObjectWithTag("fakeBroker");

        qty = (Directory.GetFiles(savesRoute, "*.k4", SearchOption.AllDirectories).Length);
            
        datos = new string[qty];
        partidasLocales = new string[qty];
        getAndReadLocalSaves();
    }
	
	// Update is called once per frame
	void Update () {
       
	}
    void getAndReadLocalSaves()
    {
     
        try
        {
            for (int x = 0; x < (Directory.GetFiles(savesRoute, "*.k4", SearchOption.AllDirectories).Length); x++)
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(savesRoute+"\\save" + (x+1) + ".k4"))
                {

                    // Read the stream to a string, and write the string to the console.
                    datos[x] = sr.ReadToEnd();
                    Debug.Log(datos[x]);
                    partidasLocales[x] = AvoEx.AesEncryptor.DecryptString(datos[x]);
                    Debug.Log(partidasLocales[x]);
                }


            }
           
            for(var y=0; y<partidasLocales.Length; y++)
            {
                var copiaPartida = Instantiate(partidaTemplate);
                copiaPartida.transform.SetParent(content.transform, false);
                copiaPartida.transform.localPosition = Vector3.zero;
            }



        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
       
        
    }

}

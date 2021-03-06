﻿using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using UnityEngine;
using System;

public class playerController : MonoBehaviour {

    public JArray objetos,ammoBag,progress;
   
    public bool onSaveZone;
    public GameObject menuOpciones;
    public bool onPause;
    public int life;
    public int money, deathsCounter;
    void Start () {
        
        objetos = new JArray();
        ammoBag = new JArray();
        progress = new JArray();

        string path = @"c:\Kala\gameData\saves\local";
   
        try
        {
            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                Debug.Log("Exiset");
                return;
            }

            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);
            Debug.Log("creado saves");

            // Delete the directory.
           
            
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        finally { }
    }

	void Update () {
        //Debug.Log(objetos.ToString());
        createInventory();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            onPause = !onPause;
            menuOpciones.SetActive(onPause);
        }
    }
    void createInventory()
    {
      
        if (Input.GetKeyUp(KeyCode.Q))
        {
            JObject inventoryJSON = new JObject(
                new JProperty("inventory",
                    new JArray(
                            from items in objetos
                            select new JObject(
                                    new JProperty("itemId", items)
                                )
                        )
                )
            );
            string path = @"c:\temp\MyTest.txt";
            if (!File.Exists(path))
            {
              
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(inventoryJSON.ToString());
                }
            }

         
        }
        
    }
}





   


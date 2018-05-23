using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class playerController : MonoBehaviour {

    public JArray objetos;

	void Start () {
        
        objetos = new JArray();
	}

	void Update () {
        createInventory();
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





   


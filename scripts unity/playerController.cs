using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using UnityEngine;

public class playerController : MonoBehaviour {

    public JArray objetos,ammoBag,progress;
   
    public bool onSaveZone;
    public GameObject menuOpciones;
    public float life;
    public int money, deathsCounter;
    void Start () {
        
        objetos = new JArray();
        ammoBag = new JArray();
        progress = new JArray();
	}

	void Update () {
        Debug.Log(objetos.ToString());
        createInventory();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menuOpciones.SetActive(true);
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





   


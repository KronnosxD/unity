using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class loadGame : MonoBehaviour {
    public string playerId;
    public GameObject player, menuOpciones;
    public Vector3 newPosition;
    public bool position, inventory;
	// Use this for initialization
	void Start () {
        position = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (position == true)
        {
            Load();
        }
    }
    public void GetData()
    {
        StartCoroutine(getPosition());
       
    }
    public void Load()
    {
            player.transform.position = newPosition;
            menuOpciones.SetActive(false);
            Debug.Log("Partida cargada");
    }
    IEnumerator getPosition()
    {

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/playerInfo/" + playerId);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            position = false;
        }
        else
        {
            string data = www.downloadHandler.text;


            JObject obj = JObject.Parse(data);
            float x = (float)obj["informacion"]["position"]["posX"];
            float y = (float)obj["informacion"]["position"]["posY"];
            float z = (float)obj["informacion"]["position"]["posZ"];
            newPosition = new Vector3(x, y, z);
            position = true;
            //player.transform.position = new Vector3(x, y, z);
            Debug.Log("Posición obtenida");

        }
    }
}

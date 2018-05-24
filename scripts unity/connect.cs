using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class connect : MonoBehaviour {
    public float x;
    public float y;
    public float z;
    public GameObject player;
    public GameObject brokerData;
    public string playerId;
    
    void Start() {
        player = GameObject.FindWithTag("Player");
        // brokerData = GameObject.FindWithTag("broker");
        //playerId = brokerData.GetComponent<broker>().playerId;
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.L))
        {
            StartCoroutine(SetPosition());
            //StartCoroutine(SavePosition(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
        }
      /*  if (Input.GetKeyUp(KeyCode.O))
        {
            //StartCoroutine(SetPosition());
            StartCoroutine(SavePosition(player.transform.position.x, player.transform.position.y, player.transform.position.z));
        } */
    }
    IEnumerator GetData() {
        //UnityWebRequest www = UnityWebRequest.Get("https://us-central1-centralhub-204216.cloudfunctions.net/test-function");

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/playerInfo/" + playerId);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
        }
    }
    IEnumerator SavePosition(float x, float y, float z)
    {
        Debug.Log(playerId.Trim().ToString());
        WWWForm form = new WWWForm();
        form.AddField("x", x.ToString());
        form.AddField("y", y.ToString());
        form.AddField("z", z.ToString());
        Debug.Log("http://localhost:8080/api/playerInfo/savePos/" + playerId);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/playerInfo/savePos/"+ playerId.Trim().ToString(), form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Posición Guardada");
        }
    }
    IEnumerator SetPosition()
    {
        
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/playerInfo/" + playerId);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            
           
            JObject obj = JObject.Parse(data);
            float x = (float)obj["informacion"]["position"]["posX"];
            float y = (float)obj["informacion"]["position"]["posY"];
            float z = (float)obj["informacion"]["position"]["posZ"];
            player.transform.position = new Vector3(x, y, z);
            Debug.Log("Posición cargada");
            
        }
    }
}
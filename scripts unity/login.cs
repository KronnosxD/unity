using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class login : MonoBehaviour {
    public InputField mail;
    public InputField password;
    public GameObject brokerData;
    public GameObject loginMenu, menuPrincipal;
    // Use this for initialization
    void Start () {
        brokerData = GameObject.FindWithTag("broker");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void StartLogin()
    {
            StartCoroutine(GetData());
    }
    IEnumerator GetData()
    {
        //UnityWebRequest www = UnityWebRequest.Get("https://us-central1-centralhub-204216.cloudfunctions.net/test-function");
        WWWForm form = new WWWForm();
        form.AddField("mail", mail.text);
        form.AddField("password", password.text);
     
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/user/login", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            JObject obj = JObject.Parse(data);
            string userId = (string)obj["info"]["_id"];
            brokerData.GetComponent<broker>().userId = userId;
            loginMenu.SetActive(false);
            menuPrincipal.SetActive(true);
            //SceneManager.LoadScene("test-cloud");
        }
    }
}

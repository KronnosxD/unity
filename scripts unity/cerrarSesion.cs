using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cerrarSesion : MonoBehaviour {
    public GameObject menuLogin, menuPrincipal;
    public broker brokerConexion;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void logout1()
    {
        menuLogin.SetActive(true);
        menuPrincipal.SetActive(false);
        brokerConexion.userId = "";
    }
}

using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class savegameData : MonoBehaviour {
    public Vector3 newPlayerPosition;
    public int vidaActual, dinero, contadorMuertes,indexPartida;
    public JArray objetos = new JArray();
    public JArray municion = new JArray();
    public JArray progreso = new JArray();
    public string nombreEscena, fechaSave;
    public Text escena;
    public Text fecha;
    public GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(objetos.ToString());
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(objetos.ToString());
	}
    public void cargarDatos()
    {
        player.transform.position = new Vector3(newPlayerPosition.x, newPlayerPosition.y, newPlayerPosition.z);
        player.GetComponent<playerController>().life = vidaActual;
        player.GetComponent<playerController>().money = dinero;
        player.GetComponent<playerController>().deathsCounter = contadorMuertes;
        player.GetComponent<playerController>().objetos = objetos;
        player.GetComponent<playerController>().ammoBag = municion;
        player.GetComponent<playerController>().progress = progreso;
       
    }
}

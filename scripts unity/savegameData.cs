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
    // Use this for initialization
    void Start () {
        
        Debug.Log(objetos.ToString());
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(objetos.ToString());
	}
}

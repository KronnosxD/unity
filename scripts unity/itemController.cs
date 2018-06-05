using Newtonsoft.Json.Linq;
using UnityEngine;

public class itemController : MonoBehaviour {
    public string itemId;
    public int cantidad;
    public bool tocandoAlJugador;
    public playerController player;
	// Use this for initialization
	void Start () {
        tocandoAlJugador = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                JObject items = new JObject(
                                new JProperty("itemId", itemId),
                                new JProperty("amount", cantidad)
                               );
                player.objetos.Add(items);
                gameObject.SetActive(false);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        tocandoAlJugador = false;
    }
}

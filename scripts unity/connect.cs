using UnityEngine;
using System.Security.Cryptography;

public class connect : MonoBehaviour {
    public float x;
    public float y;
    public float z;
    public GameObject player;
    public GameObject brokerData;
    public string playerId;
    public RijndaelManaged Crypto;

    void Start() {
        player = GameObject.FindWithTag("Player");
        Crypto = new RijndaelManaged();
    }

    void Update() {
   
    }
    
    
    
}
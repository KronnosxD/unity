﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemController : MonoBehaviour {
    public string itemId;
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
                player.objetos.Add(itemId);
                gameObject.SetActive(false);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        tocandoAlJugador = false;
    }
}
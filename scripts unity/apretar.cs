using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apretar : MonoBehaviour {

    public Animator anim;
    public bool estado;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("q"))
        {
            estado = !estado;
        }

        anim.SetBool("op", estado);
	}
}

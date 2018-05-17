
const express = require('express');
var Player = require('../models/player.model');

function newPlayer(req,res){
    var params = req.body;
    var player = new Player();

    player.userId = params.userId;
    player.sceneName= params.sceneName;
    player.currentLife= params.currentLife;
    player.money= params.money;
    player.deathsCounter= params.deathsCounter;
    player.position.posX = 0;
    player.position.posY = 0;
    player.position.posZ = 0;
    player.save((err, playerStored) => {
        if(err){
            return res.status(500).send({message: "El servidor no responde"});
        } else {
            if(!playerStored){
                return res.status(404).send({message: "No se pudo registrar al usuario"});
            } else 
            return res.status(200).send({message: "Ok.", data: playerStored})
        }
    });
}
function playerInfo(req, res) {
    var paramsUrl = req.params;
    console.log(paramsUrl);
    Player.findById(paramsUrl.userId).exec((err, playerInfo) => {
        if (err) {
            console.log(err);
            return res.status(500).send({ message: "El servidor no responde"})
        } else {
            if (!playerInfo){
                return res.status(404).send({ message: "Información no encontrada"})
            } else {        
                return res.status(200).send({ message: "Información jugador: ", informacion: playerInfo})
            }
        }
    });
}
function savePosition(req,res){
    console.log("si entro");
    console.log(req.params.userId);
    paramsUrl = req.params;
    paramsBody = req.body;

    Player.findById(paramsUrl.userId).exec((err, playerInfo) => {
        if (err) {
            console.log(err);
            return res.status(500).send({ message: "El servidor no responde"})
        } else {
            if (!playerInfo){
                return res.status(404).send({ message: "Información no encontrada"})
            } else {
               
                playerInfo.position={
                    posX: paramsBody.x,
                    posY: paramsBody.y,
                    posZ: paramsBody.z
                }

               playerInfo.save((err, playerStored) => {
                    if(err){
                        return res.status(500).send({ 
                            message: "El servidor no responde"
                        });
                    } else{
                        if(!playerStored){
                            return res.status(404).send({ 
                                message: "No se pudo actualizar la posición"
                            });
                        } else {
                            return res.status(200).send({ 
                                informacion: playerStored
                            });
                        }
                    }
                });
               
            }
        }
    });
}
module.exports = {
    newPlayer,
    playerInfo,
    savePosition
}

/* 
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
    
    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.L))
        {
            StartCoroutine(SetPosition());
            //StartCoroutine(SavePosition(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            //StartCoroutine(SetPosition());
            StartCoroutine(SavePosition(player.transform.position.x, player.transform.position.y, player.transform.position.z));
        }
    }
    IEnumerator GetData() {
        //UnityWebRequest www = UnityWebRequest.Get("https://us-central1-centralhub-204216.cloudfunctions.net/test-function");

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/playerInfo/5afb91c49aa6e92337574156");
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

        WWWForm form = new WWWForm();
        form.AddField("x", x.ToString());
        form.AddField("y", y.ToString());
        form.AddField("z", z.ToString());

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/playerInfo/savePos/5afc537c94d0a3115c587bd5", form);
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
        
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/playerInfo/5afc537c94d0a3115c587bd5");
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
*/
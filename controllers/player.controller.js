const express = require('express');
var Player = require('../models/player.model');
var Item = require('../models/item.model');
var moment = require('moment');
var User = require('../models/user.model');

function newPlayer(req, res) {
    var params = req.body;
    var player = new Player();

    player.userId = params.userId;
    player.sceneName = params.sceneName;
    player.currentLife = params.currentLife;
    player.money = params.money;
    player.deathsCounter = params.deathsCounter;
    player.position.posX = 0;
    player.position.posY = 0;
    player.position.posZ = 0;
    player.save((err, playerStored) => {
        if (err) {
            return res.status(500).send({ message: "El servidor no responde" });
        } else {
            if (!playerStored) {
                return res.status(404).send({ message: "No se pudo registrar al usuario" });
            } else
                return res.status(200).send({ message: "Ok.", data: playerStored })
        }
    });
}
function playerInfo(req, res) {
    var paramsUrl = req.params;
    console.log(paramsUrl);
    Player.findById(paramsUrl.playerId).exec((err, playerInfo) => {
        if (err) {
            console.log(err);
            return res.status(500).send({ message: "El servidor no responde" })
        } else {
            if (!playerInfo) {
                return res.status(404).send({ message: "Información no encontrada" })
            } else {
                return res.status(200).send({ message: "Información jugador: ", informacion: playerInfo })
            }
        }
    });
}
function saveGame(req, res) {


    paramsUrl = req.params;
    paramsBody = req.body;
    //console.log(paramsBody.inventory);
    var itemIdList = [];
    var items = JSON.parse(paramsBody.inventory);

    for (var x = 0; x < items.length; x++) {
        if (items[x].itemId) {

            itemIdList.push(items[x].itemId);
        }
    }


    User.findById(paramsUrl.userId).exec((err, userInfo) => {
        if (err) {
            return res.status(500).send({ message: "El servidor no responde" })
        } else {
            if (!userInfo) {
                return res.status(404).send({ message: "El usuario no existe" })
            } else {
                Player.findById(paramsBody.playerId).exec((err, playerData) => {
                    if (err) {
                        console.log(err);
                        return res.status(500).send({ message: "El servidor no responde" })
                    } else {
                        if (!playerData) {
                            return res.status(404).send({ message: "Información no encontrada" })
                        } else {
                            console.log("aqui rick");
                            var fecha = moment();

                            Item.find({ '_id': itemIdList }).exec((err, itemInfo) => {
                                if (err) {
                                    console.log(err);
                                    return res.status(500).send({ message: "El servidor no responde" })
                                } else {
                                    if (!itemInfo) {
                                        return res.status(404).send({ message: "Item no encontrada" })
                                    } else {
                                        var aux = [];
                                        for (var z = 0; z < items.length; z++) {
                                            aux.push({
                                                itemId: items[z].itemId,
                                                amount: items[z].amount
                                            });
                                        }
                                       console.log(aux);
                                        playerData.saveInfo.push({
                                            currentLife: paramsBody.life,
                                            money: paramsBody.money,
                                            deathCounter: paramsBody.deathCounter,
                                            sceneName: paramsBody.sceneName,
                                            inventory: aux,
                                            position: {
                                                posX: paramsBody.x,
                                                posY: paramsBody.y,
                                                posZ: paramsBody.z
                                            },
                                            saveData: fecha
                                        });
                                        playerData.save((err, playerStored) => {
                                            if (err) {
                                                return res.status(500).send({ message: "El servidor no responde." });
                                            }
                                            return res.status(200).send({
                                                message: 'ok'
                                            })
                                        })

                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
    })

}
/* 
function addItemToInventory(req, res) {

    var paramsBody = req.body;
    var paramsUrl = req.params;
    var itemIdList = [];
    for (var x = 0; x < paramsBody.inventory.length; x++) {
        if (paramsBody.inventory[x].itemId) {
            itemIdList.push(paramsBody.inventory[x].itemId);
        }
    }
    console.log(itemIdList);

    Player.findById(paramsUrl.playerId).exec((err, playerInfo) => {
        if (err) {
            console.log(err);
            return res.status(500).send({ message: "El servidor no responde" })
        } else {
            if (!playerInfo) {
                return res.status(404).send({ message: "Información no encontrada" })
            } else {
                Item.find({ '_id': itemIdList }).exec((err, itemInfo) => {
                    if (err) {
                        console.log(err);
                        return res.status(500).send({ message: "El servidor no responde" })
                    } else {
                        if (!itemInfo) {
                            return res.status(404).send({ message: "Item no encontrada" })
                        } else {

                            var itemFound;

                            for (var x = 0; x < itemInfo.length; x++) {
                                // New Item
                                itemFound = false;
                                //Check inventory
                                for (var y = 0; y < playerInfo.inventory.length; y++) {
                                    //Item found
                                    if (String(itemInfo[x]._id) == String(playerInfo.inventory[y].itemId)) {
                                        for (var c = 0; c < paramsBody.inventory.length; c++) {
                                            if (String(paramsBody.inventory[c].itemId) == String(playerInfo.inventory[y].itemId)) {
                                                itemFound = true;
                                                playerInfo.inventory[y].amount += paramsBody.inventory[c].amount;
                                            }
                                        }
                                    }
                                }

                                // Item not found
                                if (!itemFound) {
                                    for (var c = 0; c < paramsBody.inventory.length; c++) {
                                        if (paramsBody.inventory[c].itemId == itemInfo[x]._id) {
                                            //Push New Item
                                            playerInfo.inventory.push({
                                                itemId: itemInfo[x]._id,
                                                amount: paramsBody.inventory[c].amount
                                            });
                                        }
                                    }
                                }
                            }

                            playerInfo.save((err, playerStored) => {
                                if (err) {
                                    return res.status(500).send({ message: "El servidor no responde." });
                                }
                                return res.status(200).send({
                                    message: 'ok'
                                })
                            })
                        }
                    }
                });
            }
        }
    });
}
*/
module.exports = {
    newPlayer,
    playerInfo,
    saveGame
    //addItemToInventory
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

const express = require('express');
var Player = require('../models/player.model');

function newPlayer(req,res){
    var params = req.body;
    var player = new Player();

    player.userId = params.userId;
    player.sceneName= params.sceneName;
    player.currentLife= params.currentLife;
    player.lifes= params.lifes;
    //player.inventory= params.password;
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
    console.log("im here");
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
                console.log(playerInfo.position);
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
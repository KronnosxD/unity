
const express = require('express');
var Player = require('../models/player.model');

function playerInfo(req, res) {
    var paramsUrl = req.params;

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
               
                console.log(playerInfo.position);
                //playerInfo.position.x
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
    playerInfo,
    savePosition
}
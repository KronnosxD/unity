
const express = require('express');
var User = require('../models/user.model');
var Player = require('../models/player.model');
var Save = require('../models/save.model');

var moment = require('moment');


function saveUser(req, res) {
    var params = req.body;
    var user = new User();

    user.name = params.name;
    user.lastname = params.lastname;
    user.username = params.username;
    user.mail = params.mail;
    user.password = params.password;
    user.age = params.age;
    user.country = params.country;
    user.city = params.city;
    user.created = params.created;

    user.save((err, userStored) => {
        if (err) {
            return res.status(500).send({ message: "El servidor no responde" });
        } else {
            if (!userStored) {
                return res.status(404).send({ message: "No se pudo registrar al usuario" });
            } else
                return res.status(200).send({ message: "Ok.", data: userStored })
        }
    });
}
function login(req, res) {
    var params = req.body;
    console.log(params.mail);
    console.log(params.password);
    console.log("por aqui");
    User.findOne({ 'mail': params.mail }).exec((err, userInfo) => {
        if (err) {
            console.log("ajam");
            return res.status(500).send({ message: "El servidor no responde." });
        } else {
            if (!userInfo) {
                console.log("sdsdsd");
                return res.status(404).send({ message: "Usuario no encontrado" });
            } else {
                if (params.password == userInfo.password) {
                    console.log(userInfo);
                    Player.findOne({ "userId": userInfo._id }).exec((err, playerInfo) => {
                        if (err) {
                            return res.status(500).send({ message: "El servidor no responde." });
                        } else {
                            if (!playerInfo) {
                                return res.status(404).send({ message: "Player no encontrado" });
                            } else {
                                return res.status(200).send({ message: "ok", info: playerInfo });
                            }
                        }
                    });
                } else {
                    return res.status(403).send({ message: "Usuario o contraseña invalida." });
                }

            }
        }
    });
}
function saveGame(req, res) {
    var paramsBody = req.body;
    var paramsUrl = req.params;
    //var fecha = moment().format('DD/MM/YYYY HH:mm:ss');
    var fecha = moment();;

    User.findById(paramsUrl.userId).exec((err, userInfo) => {
        if (err) {
            return res.status(500).send({message: "El servidor no responde."});
        } else {
            if (!userInfo) {
                return res.status(404).send({message: "El usuario no existe."});
            } else {
                Player.findById(paramsBody.playerId).exec((err, playerInfo) => {
                    if (err) {
                        return res.status(500).send({message: "El servidor no responde."});
                    } else {
                        if (!playerInfo) {
                            return res.status(404).send({message: "El jugador no existe."});
                        } else {
                            Save.findOne({ 'userId': paramsUrl.userId }).exec((err, savesInfo) => {
                                if (err) {
                                    return res.status(500).send({message: "El servidor no responde."});
                                } else {
                                    if (!savesInfo) {
                                        var save = new Save();
                                        save.userId = paramsUrl.userId;
                                        save.saveGame.push({
                                            playerId: paramsBody.playerId,
                                            saveDate: fecha
                                        });
                                        save.save((err, gameSaved) => {
                                            if (err) {
                                                return res.status(500).send({ message: "El servidor no responde" });
                                            } else {
                                                if (!gameSaved) {
                                                    return res.status(404).send({ message: "No se pudo guardar la partida" });
                                                } else
                                                    return res.status(200).send({ message: "Partida guardada.", data: gameSaved })
                                            }
                                        });
                                    } else {
                                        savesInfo.saveGame.push({
                                            playerId: paramsBody.playerId,
                                            saveDate: fecha
                                        });

                                        savesInfo.save((err, gameSaved) => {
                                            if (err) {
                                                return res.status(500).send({ message: "El servidor no responde" });
                                            } else {
                                                if (!gameSaved) {
                                                    return res.status(404).send({ message: "No se pudo guardar la partida" });
                                                } else
                                                    return res.status(200).send({ message: "Partida guardada.", data: gameSaved })
                                            }
                                        });
                                    }
                                }
                            });

                        }
                    }
                });
            }
        }
    });
}
function getSaveGames(req,res){
    var paramsBody = req.body;
    var paramsUrl = req.params;

    User.findById(paramsUrl.userId).exec((err, userInfo)=> {
        if (err) {
            return res.status(500).send({message: "El servidor no responde."});
        } else {
            if (!userInfo) {
                return res.status(404).send({message: "El usuario no existe."});
            } else {
                Save.findOne({'userId': paramsUrl.userId}).exec((err, savesInfo) =>{
                    if (err) {
                        return res.status(500).send({message: "El servidor no responde."});
                    } else {
                        if (!savesInfo) {
                            return res.status(404).send({message: "No existen partidas en la nube."});
                        } else {
                            var aux = [];
                            for(var x=0; x<savesInfo.saveGame.length;x++){
                                aux.push({
                                    saveId: savesInfo.saveGame[x]._id,
                                    playerId: savesInfo.saveGame[x].playerId,
                                    date: savesInfo.saveGame[x].saveDate
                                })
                            }
                            return res.status(200).send({message: "Partidas guardadas:", data: aux});
                        }
                    }
                });
            }
        }
    });
}
function loadGame(req, res) {

    var paramsBody = req.body;
    var paramsUrl = req.params;

    User.findById(paramsUrl.userId).exec((err, userInfo)=> {
        if (err) {
            return res.status(500).send({message: "El servidor no responde."});
        } else {
            if (!userInfo) {
                return res.status(404).send({message: "El usuario no existe."});
            } else {
                Save.findOne({'userId': paramsUrl.userId}).populate({
                    path:"saveGame.playerId", model: "Player"
                }).exec((err, savesInfo) =>{
                    if (err) {
                        return res.status(500).send({message: "El servidor no responde."});
                    } else {
                        if (!savesInfo) {
                            return res.status(404).send({message: "No existen partidas en la nube."});
                        } else {
                            var flag = false;
                            var loadedGame;
                            for(var x=0; x<savesInfo.saveGame.length;x++){
                               if(savesInfo.saveGame[x]._id == paramsBody.saveId){
                                loadedGame = savesInfo.saveGame[x];
                                flag=true;
                               }
                            }
                            if(flag){
                                return res.status(200).send({message: "partida cargada", data: loadedGame});
                            } else {
                                return res.status(200).send({message: "la partida no existe"});
                            }
                        }
                    }
                });
            }
        }
    });
}
module.exports = {
    saveUser,
    login,
    saveGame,
    loadGame,
    getSaveGames
}
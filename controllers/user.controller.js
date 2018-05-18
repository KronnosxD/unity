
const express = require('express');
var User = require('../models/user.model');
var Player = require('../models/player.model');

function saveUser(req, res){
    var params = req.body;
    var user = new User();

        user.name = params.name;
        user.lastname= params.lastname;
        user.username= params.username;
        user.mail= params.mail;
        user.password= params.password;
        user.age= params.age;
        user.country= params.country;
        user.city= params.city;
        user.created= params.created;
 
    user.save((err, userStored) => {
        if(err){
            return res.status(500).send({message: "El servidor no responde"});
        } else {
            if(!userStored){
                return res.status(404).send({message: "No se pudo registrar al usuario"});
            } else 
            return res.status(200).send({message: "Ok.", data: userStored})
        }
    });
}
function login(req,res){
    var params = req.body;
    console.log(params.mail);
    console.log(params.password);
    console.log("por aqui");
    User.findOne({'mail': params.mail}).exec((err, userInfo) => {
        if(err){
            console.log("ajam");
            return res.status(500).send({message: "El servidor no responde."});
        } else {
            if(!userInfo){
                console.log("sdsdsd");
                return res.status(404).send({message: "Usuario no encontrado"});
            } else {
                if(params.password==userInfo.password){
                    console.log(userInfo);
                    Player.findOne({"userId":userInfo._id}).exec((err, playerInfo) =>{
                        if(err){
                            return res.status(500).send({message: "El servidor no responde."});
                        } else{
                            if(!playerInfo){
                                return res.status(404).send({message: "Player no encontrado"});
                            } else {
                                return res.status(200).send({message: "ok", info: playerInfo});
                            }
                        }
                    });
                }else {
                    return res.status(403).send({message: "Usuario o contraseña invalida."});
                }
               
            }
        }
    });
}
module.exports = {
    saveUser,
    login
}
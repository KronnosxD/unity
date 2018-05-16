
const express = require('express');
var User = require('../models/user.model');

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
module.exports = {
    saveUser,
}
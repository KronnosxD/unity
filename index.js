'use strict'
var mongoose = require('mongoose');

var app = require('./app')


//var mongoURL = 'mongodb://adminsentra:123456@192.168.15.123:27017/pushapp?authSource=admin';
var mongoURL = 'mongodb://K4l44dmin:K4L4B4S3@ds123500.mlab.com:23500/kala';

mongoose.Promise = global.Promise;
mongoose.connect(mongoURL)
        .then(() => {
                console.log("Conexión con MongoDB establecida");
                var PORT = process.env.PORT || 8080;
                app.listen(PORT, () => {
                        console.log("Servidor Node está corriendo en el puerto: " + PORT)
                });
        })
        .catch(err => console.log(err));


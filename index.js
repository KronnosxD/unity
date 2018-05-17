'use strict'
var app = require('./app');

var mongoose = require('mongoose');

mongoose.Promise  = global.Promise;
mongoose.connect('mongodb://K4l44dmin:K4L4B4S3@ds123500.mlab.com:23500/kala')

.then(() => {
        console.log("Conexión con MongoDB establecida");

        var port = process.env.port || 8080;
        app.listen(port, () => {
                console.log("Servidor Node está corriendo en el puerto: " + port)
        })
})
.catch(err => console.log(err));

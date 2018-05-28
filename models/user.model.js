'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var UserSchema = Schema({
    name: String,
    lastname: String,
    username: String,
    mail: String,
    age: Number,
    country: String,
    city: String,
    password: String,
    created: Date
});

module.exports = mongoose.model('User', UserSchema);
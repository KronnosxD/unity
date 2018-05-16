'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PlayerSchema = Schema({
    userId: [{type: Schema.ObjectId, ref: 'User'}],
    sceneName: String,
    currentLife: Number,
    money: Number,
    deathsCounter: Number,
    inventory: [{
        itemId: {type: Schema.ObjectId, ref: 'Inventory'},
        amount: Number
    }],
    position: {
        posX: Number,
        posY: Number,
        posZ: Number
    }
});

module.exports = mongoose.model('Player', PlayerSchema);
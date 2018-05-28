'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PlayerSchema = Schema({
    userId: {type: Schema.ObjectId, ref: 'User'},
    saveInfo:[{
        currentLife: Number,
        money: Number,
        deathCounter: Number,
        sceneName: String,
    
        progress: [{
            itemId: {type: Schema.ObjectId, ref: 'Event'}
        }],
        inventory: [{
            itemId: {type: Schema.ObjectId, ref: 'Item'},
            amount: Number
        }],
        ammoBag: [{
            itemId: {type: Schema.ObjectId, ref: 'Ammunition'},
            amount: Number
        }],
        position: {
            posX: Number,
            posY: Number,
            posZ: Number
        },
        saveDate: String
    }]
});

module.exports = mongoose.model('Player', PlayerSchema);
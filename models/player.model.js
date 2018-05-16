'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PlayerSchema = Schema({
    userId: [{type: Schema.ObjectId, ref: 'User'}],
    sceneName: String,
    currentLife: Number,
    lifes: Number,
    inventory: [{
        itemId: {type: Schema.ObjectId, ref: 'Inventory'},
        amount: Number
    }],
    position: {
        posX: Number,
        posY: Number,
        posZ: Number
    }
    /*name: String,
    lastname: String,
    email: String,
    password: String,
    tokenDevice: String,
    type: [{
        admin: Boolean,
        teacher: Boolean,
        attorney: Boolean
    }],
    notifications: [{
        title: String,
        message: String,
        from: [{
            userId: {type: Schema.ObjectId, ref: 'User'}
        }],
        date: Date,
        readed: Boolean
    }], 
    
    schools: [{
        schoolId: [{type: Schema.ObjectId, ref: 'School'}]
    }]*/
});

module.exports = mongoose.model('Player', PlayerSchema);
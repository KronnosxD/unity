'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var SaveSchema = Schema({
    userId: {type: Schema.ObjectId, ref: 'User'},
    saveGame: [{
        playerId: {type: Schema.ObjectId, ref: 'Player'},
        saveDate: String
    }]
   
});

module.exports = mongoose.model('Save', SaveSchema);
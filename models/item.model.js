'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var ItemSchema = Schema({

    name: String,
    description: String,
    type: {
        consumible: Boolean,
        weapon: Boolean,
        key: Boolean,
        cloth: Boolean
    }
    
});

module.exports = mongoose.model('Item', ItemSchema);
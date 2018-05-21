'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var ItemSchema = Schema({
    name: String,
    description: String,
    type: {
        consumible: Boolean,
        gun: Boolean,
        key: Boolean
    }
    
});

module.exports = mongoose.model('Inventory', ItemSchema);
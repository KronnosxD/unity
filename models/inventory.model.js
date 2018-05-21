'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var InventorySchema = Schema({
    name: String,
    description: String,
    type: {
        consumible: Boolean,
        gun: Boolean,
        key: Boolean
    }
    
});

module.exports = mongoose.model('Inventory', InventorySchema);
'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var AmmunitionSchema = Schema({
    name: String,
    type: {
        common: Boolean,
        special: Boolean
    }
});

module.exports = mongoose.model('Ammunition', AmmunitionSchema);
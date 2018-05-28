'use strict';

var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var EventSchema = Schema({
    name: String,
    description: String,
    type: String,
    triggeredDate: String
    
});

module.exports = mongoose.model('Event', EventSchema);
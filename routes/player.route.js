var express = require('express');
var PlayerController = require('../controllers/player.controller');
var api = express.Router();

api.get('/playerInfo/:userId', PlayerController.playerInfo);
api.post('/playerInfo/savePos/:userId', PlayerController.savePosition);
module.exports = api;

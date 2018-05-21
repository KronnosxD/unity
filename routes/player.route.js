var express = require('express');
var PlayerController = require('../controllers/player.controller');
var api = express.Router();

api.get('/playerInfo/:playerId', PlayerController.playerInfo);
api.post('/playerInfo/savePos/:playerId', PlayerController.savePosition);
api.post('/player/new/',PlayerController.newPlayer);
api.post('/player/addItemToInventory/:playerId',PlayerController.addItemToInventory);
module.exports = api;

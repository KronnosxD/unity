var express = require('express');
var PlayerController = require('../controllers/player.controller');
var api = express.Router();

api.get('/player/:playerId', PlayerController.playerInfo);
api.post('/player/saveGame/:userId', PlayerController.saveGame);
api.post('/player/new/',PlayerController.newPlayer);
//api.post('/player/addItemToInventory/:playerId',PlayerController.addItemToInventory);
module.exports = api;

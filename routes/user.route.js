var express = require('express');
var UserController = require('../controllers/user.controller');
var api = express.Router();

api.post('/user/save/', UserController.saveUser);
api.post('/user/login/', UserController.login);
api.post('/user/saveGame/:userId', UserController.saveGame);
api.post('/user/loadGame/:userId', UserController.loadGame);
api.get('/user/getsavegames/:userId', UserController.getSaveGames);
module.exports = api;

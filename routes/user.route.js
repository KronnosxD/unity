var express = require('express');
var UserController = require('../controllers/user.controller');
var api = express.Router();

api.post('/user/save/', UserController.saveUser);
api.post('/user/login/', UserController.login);

module.exports = api;

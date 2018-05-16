var express = require('express');
var UserController = require('../controllers/user.controller');
var api = express.Router();

api.post('/user/save/', UserController.saveUser);

module.exports = api;

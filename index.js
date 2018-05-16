'use strict'
var app = require('./app');
var PORT = process.env.PORT || 8080;
app.listen(PORT, () => {
        console.log('App listening on ', PORT);
        console.log('Press Ctrl+C to quit.');
});
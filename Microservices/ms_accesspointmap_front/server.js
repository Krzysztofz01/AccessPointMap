const express = require('express');
const bodyParser = require('body-parser');
const routes = require('./routes/routes.js');
const env = require('./config/env.js');

//Create app
const app = express();

//Middleware
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.set('view engine', 'ejs');
app.use('/assets', express.static('assets'));
app.use('/projects/accesspointmap', routes);

app.listen(env.default.port);
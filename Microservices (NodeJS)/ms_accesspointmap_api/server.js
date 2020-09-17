const express = require('express');
const bodyParser = require('body-parser');
const accesspointRoutes = require('./api/routes/accesspointRoutes.js');
const env = require('./api/config/env.js');

//Create app
const app = express();

//Middleware
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use('/projects/accesspointmap/api/request', accesspointRoutes);

app.listen(env.default.port);
const express = require('express');
const bodyParser = require('body-parser');
const env = require('./config/env');
const routes = require('./routes/routes');

const app = express();

app.set('view engine', 'ejs');
app.use('/assets', express.static('assets'));
app.use('/projects/accesspointmap', routes);

app.listen(env.default.port);
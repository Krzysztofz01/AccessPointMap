const mysql = require('mysql2');
const env = require('./env.js');

//Connection to database
const connection = mysql.createConnection({
    host: process.env.DB_HOST,
    user: process.env.DB_USER,
    password: process.env.DB_PASS,
    database: process.env.DB_NAME
});

connection.connect(error => {
    if(error) throw error;
});

module.exports = connection;
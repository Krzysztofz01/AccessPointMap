require('dotenv').config();

//All secret env variables stored to object
module.exports.default = {
    port: process.env.APP_PORT,
    dbPort: process.env.DB_PORT,
    dbHost: process.env.DB_HOST,
    dbUser: process.env.DB_USER,
    dbPass: process.env.DB_PASS,
    dbName: process.env.DB_NAME,
    tokenSecret: process.env.TOKEN_SECRET
};
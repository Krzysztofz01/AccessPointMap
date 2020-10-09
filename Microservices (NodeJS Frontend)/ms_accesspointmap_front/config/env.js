require('dotenv').config();

module.exports.default = {
    port: process.env.APP_PORT,
    apiUrl: process.env.API_URL,
    apiUrlDev: process.env.API_URL_DEV
};
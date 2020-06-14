require('dotenv').config();

//All secret env variables stored to object
module.exports.default = {
    port: process.env.APP_PORT,
    userLogin: process.env.USER_LOGIN,
    userPass: process.env.USER_PASSWORD,
    authApiUrl: process.env.AUTH_API_URL,
    dataApiUrl: process.env.DATA_API_URL
};
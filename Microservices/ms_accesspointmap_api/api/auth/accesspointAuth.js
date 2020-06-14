const jwt = require('jsonwebtoken');
const env = require('../config/env.js');

const authWrite = (req, res, next) => {
    const token = req.header('auth-token');
    if(!token) return res.status(401).send({ message: "Access denied." });

    try {
        const verified = jwt.verify(token, env.default.tokenSecret);
        const permission = verified.permissions.find(per => per.name === "accesspointmap_write");

        if(!permission) return res.status(403).send({ message: "This account does not have the permission for this operation." });

        next();
    } catch(error) {
        res.status(401).send({ message: "Invalid token." })
    }
};

const authRead = (req, res, next) => {
    const token = req.header('auth-token');
    if(!token) return res.status(401).send({ message: "Access denied." });

    try {
        const verified = jwt.verify(token, env.default.tokenSecret);
        const permission = verified.permissions.find(per => per.name === "accesspointmap_read");
        
        if(!permission) return res.status(403).send({ message: "This account does not have the permission for this operation."});

        next();
    } catch(error) {
        res.status(401).send({ message: "Invalid token." })
    }
};

module.exports.authWrite = authWrite;
module.exports.authRead = authRead;
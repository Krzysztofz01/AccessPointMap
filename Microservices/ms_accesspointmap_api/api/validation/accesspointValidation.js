const Joi = require('@hapi/joi');

const addValidation = (data) => {
    const schema = Joi.array().items({
        bssid: Joi.string().max(25).required(),
        ssid: Joi.string().max(40).required(),
        freq: Joi.number().required(),
        signalLevel: Joi.number().required(),
        latitude: Joi.number().required(),
        longitude: Joi.number().required(),
        lowSignalLevel: Joi.number().required(),
        lowLatitude: Joi.number().required(),
        lowLongitude: Joi.number().required(),
        security: Joi.string().max(65).required()    
    });
    return schema.validate(data);
};

module.exports.addValidation = addValidation;
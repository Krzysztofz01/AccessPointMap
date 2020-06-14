const AccessPoint = require("../models/accesspointModel.js");
const { addValidation } = require('../validation/accesspointValidation.js');

//Add route controller
exports.add = async (req, res) => {
    //Validation
    const { error } = addValidation(req.body);
    if(error) return res.status(400).send({ message: error.details[0].message });

    //Creating a new AccessPoint object
    let accessPoint = new AccessPoint({
        bssid: req.body.bssid,
        ssid: req.body.ssid,
        freq: req.body.freq,
        signalLevel: req.body.signalLevel, 
        latitude: req.body.latitude,
        longitude: req.body.longitude,
        lowSignalLevel: req.body.lowSignalLevel,
        lowLatitude: req.body.lowLatitude,
        lowLongitude: req.body.lowLongitude,
        signalArea: req.body.signalArea,
        security: req.body.security,
        vendor: req.body.vendor
    });

    //Calling the models add method
    await AccessPoint.add(accessPoint, (err, data) => {
        if(err) { 
            res.status(err.code || 500).send({ message: err.info || err.message || "Some error occurred while creating a new record." }); 
        } else {
            res.send(data);
        }       
    });
};

//Read route controller
exports.readAll = async (req, res) => {
    //Calcing the models read all method
    await AccessPoint.readAll((err, data) => {
        if(err) { 
            res.status(err.code || 500).send({ message: err.info || err.message || "Some error occurred while creating a new record." }); 
        } else {
            res.send(data);
        }
    });
};

//Read by id route controller
exports.readById = async (req, res) => {
    //Calling the models read by id method
    await AccessPoint.readById(req.params.id, (err, data) => {
        if(err) {
            res.status(err.code || 500).send({ message: err.info || err.message || "Some error occurred while retrieving record." });
        } else {
            res.send(data);
        }
    });
};

//Read by bssid route controller
exports.readByBssid = async (req, res) => {
    await AccessPoint.readByBssid(req.params.bssid, (err, data) => {
        if(err) {
            res.status(err.code || 500).send({ message: err.info || err.message || "Some error occurred while retrieving record." });
        } else {
            res.send(data);
        }
    });
};
const AccessPoint = require("../models/accesspointModel.js");
const { addValidation } = require('../validation/accesspointValidation.js');

//Add route controller
exports.add = async (req, res) => {
    //Validation
    const { error } = addValidation(req.body);
    if(error) return res.status(400).send({ message: error.details[0].message });

    //For each uploaded accesspoint...
    req.body.forEach(async accessPoint => {
        //Creating a new AccessPoint object
        let ap = new AccessPoint({
            bssid: accessPoint.bssid,
            ssid: accessPoint.ssid,
            freq: accessPoint.freq,
            signalLevel: accessPoint.signalLevel, 
            latitude:accessPoint.latitude,
            longitude: accessPoint.longitude,
            lowSignalLevel: accessPoint.lowSignalLevel,
            lowLatitude: accessPoint.lowLatitude,
            lowLongitude: accessPoint.lowLongitude,
            signalArea: accessPoint.signalArea,
            security: accessPoint.security,
            vendor: accessPoint.vendor
        });

        //Calling the models add method
        await AccessPoint.add(ap, (err, data) => {
            if(err) { 
                res.status(err.code || 500).send({ message: err.info || err.message || "Some error occurred while creating a new record." });
            }
        });
    });
    res.status(201).send({ message: "All object has been added to the database." });
};

//Read route controller
exports.readAll = async (req, res) => {
    //Calling the models read all method
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

//Search by params route controller
exports.search = async (req, res) => {
    //Calling the models search method
    console.log(req.query.security);
    console.log(req.query.brand);
    console.log(req.query.ssid);
    console.log(req.query.freq);
    console.log("===========");

    await AccessPoint.search(req.query.security, req.query.brand, req.query.ssid, req.query.freq, (err, data) => {
        if(err) {
            res.status(err.code || 500).send({ message: err.info || err.message || "Some error occurred while retrieving records." });
        } else {
            res.send(data);
        }
    });
};
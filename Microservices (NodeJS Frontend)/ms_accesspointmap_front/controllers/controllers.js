const axios = require('axios');
const { render } = require('ejs');
const { param } = require('../routes/routes');
const accessPointApiUrl = "http://localhost:54805/projects/accesspointmap/api/AccessPoints/id/";
const searchApiUrl = "http://localhost:54805/projects/accesspointmap/api/AccessPoints/search"

//Index view
exports.index = async (req, res) => { return res.render('index'); };

//Single accesspoint view
exports.accesspoint = async (req, res) => {
    if(req.query.id == null) {
        return res.render('error', { message: "No parameter given!" });
    } else {
        axios.get(accessPointApiUrl + req.query.id.toString())
        .then(response => {
            return res.render('accesspoint', {
                ssid: response.data.ssid,
                bssid: response.data.bssid,
                freq: response.data.frequency,
                security: response.data.securityData,
                brand: response.data.brand,
                highSignalLevel: response.data.highSignalLevel,
                lowSignalLevel: response.data.lowSignalLevel,
                signalArea: response.data.signalArea,
                signalRadius: response.data.signalRadius,
                latitude: response.data.highLatitude,
                longitude: response.data.highLongitude
            });
        })
        .catch(error => { return res.render('error', { message: "No element with given parameter found!", error: error }); });
    }
};

//Search system view
exports.search = async (req, res) => {
    if((req.query.ssid == null) && (req.query.freq == null) && (req.query.brand == null) && (req.query.security == null)) {
        //Deafult view
        return res.render('search');
    } else {
        //Multiple params dont work, need to fix it
        const params = {
            ssid: (req.query.ssid == null) ? "" : req.query.ssid,
            freq: (req.query.freq == null) ? "" : req.query.freq,
            brand: (req.query.brand == null) ? "" : req.query.brand,
            security: (req.query.security == null) ? "" : req.query.security
        };

        axios.get(searchApiUrl, {
            params: params
        })
        .then(response => { console.log(response.data)/*return render('search', response.data);*/ })
        .catch(error => { return render('error', { message: "Some errors occured while retriving data!", error: error }); });
    }
};
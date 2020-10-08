const axios = require('axios');
const accessPointApiUrl = "http://localhost:54805/projects/accesspointmap/api/AccessPoints/id/";
const searchApiUrl = "http://localhost:54805/projects/accesspointmap/api/AccessPoints/search"
const allAccessPointsApiUrl = "http://localhost:54805/projects/accesspointmap/api/Accesspoints";

//Index view
exports.index = async (req, res) => { return res.render('index', { apiUrl: allAccessPointsApiUrl }); };

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
        return res.render('searchBlank');
    } else {
        const params = {};
        if(req.query.ssid.length) params.ssid = req.query.ssid;
        if(req.query.freq.length) params.freq = req.query.freq;
        if(req.query.brand.length) params.brand = req.query.brand;
        if(req.query.security.length) params.security = req.query.security;

        axios.get(searchApiUrl, {
            params: (Object.keys(params).length > 0) ? params : {}
        })
        .then(response => { 
            const tableElements = [];
            for(var i=0; i<response.data.length; i++) {
                tableElements.push(`<tr><th>${i+1}</th><td class="accessPointUrl" id="${response.data[i].id}">${response.data[i].ssid}</td><td>${response.data[i].bssid}</td><td>${response.data[i].frequency}</td><td>${response.data[i].securityData}</td><td>${response.data[i].brand}</td></tr>`);
            }

            return res.render('search', { elements: tableElements });
        })
        .catch(error => {
            if(error.errno != null && error.errno == "ECONNREFUSED") return res.render('error', { message: "Service unavailable, try again later!", error: error });
            if(error.response.status != null && error.response.status == 404) return res.render('searchBlank');  
            return res.render('error', { message: "Some errors occured!", error: error });
        });
    }
};
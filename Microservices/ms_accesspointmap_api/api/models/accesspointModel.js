const mysql = require('../config/database.js');
const fetch = require('node-fetch');

//Access point model
const AccessPoint = function(accesspoint) {
    this.bssid = accesspoint.bssid,
    this.ssid = accesspoint.ssid,
    this.freq = accesspoint.freq,
    this.signalLevel = accesspoint.signalLevel, 
    this.latitude = accesspoint.latitude,
    this.longitude = accesspoint.longitude,
    this.lowSignalLevel = accesspoint.lowSignalLevel,
    this.lowLatitude = accesspoint.lowLatitude,
    this.lowLongitude = accesspoint.lowLongitude,
    this.signalArea = accesspoint.signalArea,
    this.security = accesspoint.security,
    this.vendor = accesspoint.vendor
};

//Access point add to database method
AccessPoint.add = async (newAccessPoint, result) => {
    //Check if a accesspoint with secific bssid already exists in the database
    mysql.query(`SELECT signalLevel, latitude, longitude, lowSignalLevel, lowLatitude, lowLongitude, signalArea FROM accesspoints WHERE bssid = "${newAccessPoint.bssid}"`, async (selectErr, selectRes) => {
        if(selectErr) {
            result({
                code: 500,
                info: selectErr
            }, null);
            return;
        }
   
        if(!selectRes.length) {
            //Access point with that bssid does not exist, craating a new record
            newAccessPoint.signalArea = AccessPoint.calculateArea(newAccessPoint.latitude, newAccessPoint.longitude, newAccessPoint.lowLatitude, newAccessPoint.lowLongitude);

            //Connect to MACVendor API to get the Vendor name
            newAccessPoint.vendor = await AccessPoint.getVendor(newAccessPoint.bssid);

            mysql.query("INSERT INTO accesspoints SET ?", newAccessPoint, async (createErr, createRes) => {
                if(createErr) {
                    result({
                        code: 500,
                        info: createErr
                    }, null);
                    return;
                }
                result(null, { message: "Access point has been added to the database." });
            });
        } else {
            //Access point with that bssid already exist, checking data for update query
            
            //Check if the new signal is stronger
            if(newAccessPoint.signalLevel < selectRes[0].signalLevel) {
                newAccessPoint.signalLevel = selectRes[0].signalLevel;
                newAccessPoint.latitude = selectRes[0].latitude;
                newAccessPoint.longitude = selectRes[0].longitude;
            }

            //Check if the new lower signal is weaker 
            if(newAccessPoint.lowSignalLevel > selectRes[0].lowSignalLevel) {
                newAccessPoint.lowSignalLevel = selectRes[0].lowSignalLevel;
                newAccessPoint.lowLatitude = selectRes[0].lowLatitude;
                newAccessPoint.lowLongitude = selectRes[0].lowLongitude;
            }

            //Calculate new signal area
            newAccessPoint.signalArea = AccessPoint.calculateArea(newAccessPoint.latitude, newAccessPoint.longitude, newAccessPoint.lowLatitude, newAccessPoint.lowLongitude);
            
            //Update record in database with new data
            mysql.query("UPDATE accesspoints SET ? WHERE bssid = ?", [newAccessPoint, newAccessPoint.bssid], async (updateErr, updateRes) => {
                if(updateErr) {
                    result({
                        code: 500,
                        info: updateErr
                    }, null);
                    return;
                }
                result(null, { message: "Access point has been updated" });
            });
        }
    });
};

//Access point get all data method
AccessPoint.readAll = async result => {
    mysql.query("SELECT * FROM accesspoints", async (err, res) => {
        if(err) {
            result({
                code: 500,
                info: err
            }, null);
            return;
        }
        result(null, res);
    });
};

//Access point get one element by id method
AccessPoint.readById = async (id, result) => {
    mysql.query("SELECT * FROM accesspoints WHERE id = ? LIMIT 1", id, async (err, res) => {
        if(err) {
            result({
                code: 500,
                info: err
            }, null);
            return;
        }
        
        if(res.length) {
            result(null, res[0]);
            return;
        }

        result({
            code: 404,
            info: "No access point found with given id."
        }, null);
    });
};

//Access point get all elements with specific parameters (used by the frontend server search engine)
AccessPoint.search = async (security, brand, ssid, freq, result) => {
    let query = "SELECT * FROM accesspoints WHERE ";
    let multipleParams = false;

    if(security) {
        if(multipleParams) {
            query += "AND security like '%" + security + "%' ";
        } else {
            multipleParams = true;
            query += "security like '%" + security + "%' ";
        }
    }

    if(brand) {
        if(multipleParams) {
            query += "AND vendor = '" + brand + "' ";
        } else {
            multipleParams = true;
            query += "vendor = '" + brand + "' ";
        }
    }

    if(ssid) {
        if(multipleParams) {
            query += "AND ssid = '" + ssid + "' ";
        } else {
            multipleParams = true;
            query += "ssid = '" + ssid + "' ";
        }
    }

    if(freq) {
        if(multipleParams) {
            query += "AND freq = " + freq + " ";
        } else {
            multipleParams = true;
            query += "freq = " + freq + " ";
        }
    }

    if(!multipleParams) { query = "SELECT * FROM accesspoints"; }

    console.log(query);
    mysql.query(query, async (err, res) => {
        if(err) {
            result({
                code: 500,
                info: err
            }, null);
            return;
        }
        
        if(res.length) {
            result(null, res);
            return;
        }

        result({
            code: 404,
            info: "No access point found with given parameters."
        }, null);
    });
};

//Calculate signal area by given two latitudes and longitudes
AccessPoint.calculateArea = (lat1, lon1, lat2, lon2) => {
    const PI = 3.1415;

    let latDistance = (lat2 - lat1) * PI / 180.0;
    let lonDistance = (lon2 - lon1) * PI / 180.0;

    lat1 = (lat1) * PI / 180.0;
    lat2 = (lat2) * PI / 180.0;

    let a = Math.pow(Math.sin(latDistance / 2), 2) +
            Math.pow(Math.sin(lonDistance / 2), 2) *
            Math.cos(lat1) * Math.cos(lat2);
    let c = 2 * Math.asin(Math.sqrt(a));

    let distance = Math.round((((6371 * c * 1000) + Number.EPSILON) * 100) / 100)
    return Math.round(PI * Math.pow(distance, 2));
};

//Connect to the MACVendor API to send a bssid and get the vendor name
AccessPoint.getVendor = async (bssid) => {
    try {
        const response = await fetch("https://api.macvendors.com/" + bssid);
        const body = await response.text();
        return body;
    } catch(err) {
        return err;
    }

};

module.exports = AccessPoint;
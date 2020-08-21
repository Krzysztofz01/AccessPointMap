const fetch = require('node-fetch');
const memory = require('memory-cache');
const env = require('../config/env.js');

exports.index = async (req, res) => {
    res.render('index');
};

exports.accesspoint = async (req, res) => {
    const dataResponse = await fetch(env.default.dataApiUrl + "/read/id/" + req.params.id.toString(), {
        method: 'get',
        headers: {
            'auth-token': memory.get('auth-token')
        }
    });

    const data = await dataResponse.json();
    if(!data) return res.status(500).send({ message: "The server was unable to communicate with the data provider server."});
    
    if(!data.message) {
        //Render the access point view
        
        //Calculate the rest of the imporant data (signal radius). In the futre the rest API will
        //handle this operation, to relieve the front end server.
        data.signalRadius = 3;
        () => {
            const P = 0.01745;
            let a = 0.5 - Math.cos((dataJson.lowLatitude - dataJson.latitude) * P) / 2 +
                    Math.cos(dataJson.latitude * P) * Math.cos(dataJson.lowLatitude * P) *
                    (1 - Math.cos((dataJson.lowLongitude - dataJson.longitude) * P)) / 2;
    
            let output = (12742 * Math.asin(Math.sqrt(a))) * 1000;
    
            if(output > 3) {
                dataJson.signalRadius = output;
            }
        };

        res.render('accesspoint', data);

    } else {
        //Render the error view
        res.render('error', data);
    }
}

// FINISH
exports.search = async (req, res) => {
    res.send("search");
};
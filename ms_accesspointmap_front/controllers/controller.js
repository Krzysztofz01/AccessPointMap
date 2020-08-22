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
        res.render('accesspoint', data);
    } else {
        //Render the error view
        res.render('error', data);
    }
};

// FINISH

//str.replace 
// dane do dropdownów, str replace w js ejs 
exports.search = async (req, res) => {
    if(req.query.security || req.query.brand || req.query.ssid || req.query.freq) {
        const dataResponse = await fetch(env.default.dataApiUrl + "/search?security=" + req.query.security + "&brand=" + req.query.brand + "&ssid=" + req.query.ssid + "&freq=" + req.query.freq)
    } else {
        res.send("search", { status: "blank", data: null });
    }
};
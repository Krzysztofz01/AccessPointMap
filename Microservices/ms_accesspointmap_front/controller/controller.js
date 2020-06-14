const fetch = require('node-fetch');
const env = require('../config/env.js');

exports.index = async (req, res) => {
    //Loging into the auth server to get the auth token
    const authResponse = await fetch(env.default.authApiUrl, {
        method: 'post',
        body: JSON.stringify({
            login: env.default.userLogin,
            password: env.default.userPass
        }),
        headers: { 'Content-Type': 'application/json' }
    });
    
    const token = await authResponse.json();
    if(!token.token) return res.status(500).send({ message: "The server was unable to connect with the authorization server."});

    //Requsting data from REST API using the auth token
    const dataResponse = await fetch(env.default.dataApiUrl + "/read", {
        method: 'get',
        headers: {
            'auth-token': token.token
        }
    });

    const data = await dataResponse.json();
    if(!data) return res.status(500).send({ message: "The server was unable to communicate with the data provider server."});

    
    if(!data.message) {
        //Render the access point view

        const securityChart = {
            WPA2: {
                name: 'WPA2',
                count: 0
            },
            WPA: {
                name: 'WPA',
                count: 0
            },
            WEP: {
                name: 'WEP',
                count: 0
            },
            none: {
                name: 'none',
                count: 0
            }
        };

        const brandChart = {
            tplink: {
                name: 'Tp-Link',
                count: 0
            },
            dlink: {
                name: 'D-Link',
                count: 0
            },
            dasan: {
                name: 'Dasan (Ostrog.net)',
                count: 0
            },
            sagem: {
                name: 'Sagemcom (Orange)',
                count: 0
            },
            huawei: {
                name: 'Huawei (Play)',
                count: 0
            }
        };

        const frequencyArray = Array();

        //Preapre chart data
        for(let accesspoint of data) {
            //Check security
            if(accesspoint.security.includes("WPA2")) {
                securityChart.WPA2.count++;
            } else if(accesspoint.security.includes("WPA")) {
                securityChart.WPA.count++;
            } else if(accesspoint.security.includes("WEP")) {
                securityChart.WEP.count++;
            } else {
                securityChart.none.count++;
            }

            //Check for brands
            if(accesspoint.vendor.toUpperCase().includes("TP-LINK")) {
                brandChart.tplink.count++;
            } else if(accesspoint.vendor.toUpperCase().includes("D-LINK")) {
                brandChart.dlink.count++;
            } else if(accesspoint.vendor.toUpperCase().includes("DASAN")) {
                brandChart.dasan.count++;
            } else if(accesspoint.vendor.toUpperCase().includes("SAGEMCOM")) {
                brandChart.sagem.count++;
            } else if(accesspoint.vendor.toUpperCase().includes("HUAWEI")) {
                brandChart.huawei.count++;
            }

            //Check frequency
            if(frequencyArray.length > 0) {
                if(!frequencyArray.find(x => x.freq == accesspoint.freq).add()) {
                    frequencyArray.push({ freq: accesspoint.freq, count: 1, add: () => { this.count++ } });
                }
            } else {
                frequencyArray.push({ freq: accesspoint.freq, count: 1, add: () => { this.count++ } });
            }
        }

        //Create the output freq array
        frequencyArray.sort((a, b) => (a.count < b.count) ? 1 : -1);
        const freqChart = Array();
        for(let i=0; i < 5; i++) {
            freqChart.push({ name: frequencyArray[i].freq, freq: frequencyArray[i].count });
        }

        //Check for biggest signal area
        data.sort((a, b) => (a.signalArea < b.signalArea) ? 1 : -1);
        const areaChart = Array();
        for(let i=0; i < 5; i++) {
            areaChart.push({ name: data[i].ssid, area: data[i].signalArea });
        }

        

        res.render('index', {
            data: data,
            securityChart: securityChart,
            brandChart: brandChart,
            areaChart: areaChart,
            freqChart: freqChart
        });
    } else {
        //Render the error view
        res.render('error', data);
    }

};

exports.accesspoint = async (req, res) => {
    //Loging into the auth server to get the auth token
    const authResponse = await fetch(env.default.authApiUrl, {
        method: 'post',
        body: JSON.stringify({
            login: env.default.userLogin,
            password: env.default.userPass
        }),
        headers: { 'Content-Type': 'application/json' }
    });
    
    const token = await authResponse.json();
    if(!token.token) return res.status(500).send({ message: "The server was unable to connect with the authorization server."});

    //Requsting data from REST API using the auth token
    const dataResponse = await fetch(env.default.dataApiUrl + "/read/id/" + (req.params.id).toString(), {
        method: 'get',
        headers: {
            'auth-token': token.token
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
};

exports.search = async (req, res) => {
    res.send("search");
};
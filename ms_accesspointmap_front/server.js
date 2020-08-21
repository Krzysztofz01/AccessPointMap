const express = require('express');
const bodyParser = require('body-parser');
const cron = require('node-cron');
const fetch = require('node-fetch');
const memory = require('memory-cache');
const fs = require('fs');
const routes = require('./routes/routes.js');
const env = require('./config/env.js');

const app = express();

//Middleware
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.set('view engine', 'ejs');
app.use('/assets', express.static('assets'));
app.use('/projects/accesspointmap', routes);

//Loging into the auth server
const authResponse = await fetch(env.default.authApiUrl, {
    method: 'post',
    body: JSON.stringify({
        login: env.default.userLogin,
        password: env.default.userPass
    }),
    headers: { 'Content-Type': 'application/json' }
});

const token = await authResponse.json();
if(!token.token) throw 'Some problems occurred while logging into the auth server!';
memory.put('auth-token', token);


//CronJobs
//Fetching data form API every hour and storing the accesspoint data in memory cache

// !!! TEST !!!
cron.schedule('0 0 */1 * * *', async () => {
    //Calling the AccessPointMap API to get all the data
    const dataResponse = await fetch(env.default.dataApiUrl + "/read", {
        method: 'get',
        headers: {
            'auth-token': memory.get('auth-token')
        }
    });

    const data = await dataResponse.json();
    if(data) {
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

        //Creating the output object
        const time = new Date(Date.now());
        const outputObject = {
            accessPoints: data,
            charts: {
                security: securityChart,
                brand: brandChart,
                area: areaChart,
                freq: freqChart
            },
            timestamp: String(time.getFullYear() + "-" + time.getMonth() + "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes())
        };

        //Caching in memory
        memory.put('accessPointData', outputObject);

        //Saving to the asset files
        fs.writeFile("/assets/json/data.json", JSON.stringify(outputObject), 'utf8', (err) => {
            if(err) { console.log(err); }
        });
    }


}, {
    scheduled: true,
    timezone: "Europe/Warsaw"
});

app.listen(env.default.port);
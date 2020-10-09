const insertData = (apiUrl) => {
    //Communication URL's
    const accesspointUrl = window.location.href + "accesspoint";

    //Chart data objects/arrays
    const securityChartData = { WPA2: 0, WPA: 0, WEP: 0, none: 0 };
    const brandChartData = { tplink: 0, dlink: 0, dasan: 0, sagem: 0 };
    const areaChartDataTemp = [];
    const areaChartData = [];
    const freqChartDataTemp = [];
    const freqChartData = [];

    //Chart HTML elements
    const charts = {
        securityChart: document.getElementById("securityChart").getContext('2d'),
        brandChart: document.getElementById("brandChart").getContext('2d'),
        freqChart: document.getElementById("freqChart").getContext('2d'),
        areaChart: document.getElementById("areaChart").getContext('2d')
    };

    //Chart color
    const color = "#84c69b";

    //Creating the map object and assign the HTML element
    const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 15,
        center: {lat:  50.16, lng: 18.31},
        mapTypeId: 'satellite'
    });

    //if local storage is disabled fetch data without cacheing, otherwise cache the response
    if((typeof(Storage) !== "undefined") && (localStorage.responseCache.timestamp != null)) {
        const cache = JSON.parse(localStorage.responseCache);
        const time = new Date().getTime();

        //If the data is not older than 2 days use the cached data, otherwise fetch new data from the server
        if((Math.ceil(Math.abs(cache.timestamp - time) / (1000 * 60 * 60 * 24))) < 2) {
            cache.data.data.forEach(element => {
                var marker = new google.maps.Marker({
                    position: {
                        lat: parseFloat(element.highLatitude),
                        lng: parseFloat(element.highLongitude)
                    },
                    map: map,
                    icon: (element.securityData.includes("WPA") || element.securityData.includes("WEP")) ? "http://maps.google.com/mapfiles/ms/icons/green-dot.png" : "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
                    label: {
                        text: element.ssid,
                        color: "white" 
                    }
                });

                //Add click event
                google.maps.event.addListener(marker, 'click', () => {
                    window.location.replace(accesspointUrl + "?id=" + element.id.toString());
                });      
            });

            //Generate charts
            new Chart(charts.securityChart, {
                type: 'bar',
                data: {
                    labels: ['WPA2', 'WPA', 'WEP', 'none'],
                    datasets: [{
                        label: 'Count',
                        data: [cache.data.securityChartData.WPA2, cache.data.securityChartData.WPA, cache.data.securityChartData.WEP, cache.data.securityChartData.none],
                        backgroundColor: [color, color, color, color]
                    }]
                },
                options: {
                    legend: {
                        display: false,
                        labels: {
                            fontColor: '#2f8886'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                display: false
                            },
                            gridLines: {
                                display: false
                            }
                        }],
                        xAxes: [{
                            gridLines: {
                                display: false
                            }
                        }]
                    }
                }
            });

            new Chart(charts.brandChart, {
                type: 'bar',
                data: {
                    labels: ['Tp-Link', 'D-Link', 'Dasan', 'Sagem'],
                    datasets: [{
                        label: 'Count',
                        data: [cache.data.brandChartData.tplink, cache.data.brandChartData.dlink, cache.data.brandChartData.dasan, cache.data.brandChartData.sagem],
                        backgroundColor: [color, color, color, color]
                    }]
                },
                options: {
                    legend: {
                        display: false,
                        labels: {
                            fontColor: '#2f8886'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                display: false
                            },
                            gridLines: {
                                display: false
                            }
                        }],
                        xAxes: [{
                            gridLines: {
                                display: false
                            }
                        }]
                    }
                }
            });

            new Chart(charts.areaChart, {
                type: 'bar',
                data: {
                    labels: [ cache.data.areaChartData[0].ssid.toString(), cache.data.areaChartData[1].ssid.toString(), cache.data.areaChartData[2].ssid.toString(), cache.data.areaChartData[3].ssid.toString(), cache.data.areaChartData[4].ssid.toString() ],
                    datasets: [{
                        label: 'Count',
                        data: [ cache.data.areaChartData[0].area, cache.data.areaChartData[1].area, cache.data.areaChartData[2].area, cache.data.areaChartData[3].area, cache.data.areaChartData[4].area ],
                        backgroundColor: [color, color, color, color, color]
                    }]
                },
                options: {
                    legend: {
                        display: false,
                        labels: {
                            fontColor: '#2f8886'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                display: false
                            },
                            gridLines: {
                                display: false
                            }
                        }],
                        xAxes: [{
                            gridLines: {
                                display: false
                            }
                        }]
                    }
                }
            });

            new Chart(charts.freqChart, {
                type: 'bar',
                data: {
                    labels: [ cache.data.freqChartData[0].freq, cache.data.freqChartData[1].freq, cache.data.freqChartData[2].freq, cache.data.freqChartData[3].freq, cache.data.freqChartData[4].freq ],
                    datasets: [{
                        label: 'Count',
                        data: [ cache.data.freqChartData[0].count, cache.data.freqChartData[1].count, cache.data.freqChartData[2].count, cache.data.freqChartData[3].count, cache.data.freqChartData[4].count ],
                        backgroundColor: [color, color, color, color, color]
                    }]
                },
                options: {
                    legend: {
                        display: false,
                        labels: {
                            fontColor: '#2f8886'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                display: false
                            },
                            gridLines: {
                                display: false
                            }
                        }],
                        xAxes: [{
                            gridLines: {
                                display: false
                            }
                        }]
                    }
                }
            });
        } else {
            //The cached data is expired, set the cache object to null and call the same function again
            localStorage.responseCache = null;
            insertData();
        }
    } else {
        fetch(apiUrl)
            .then(response => response.json())
            .then(data => {
                //Generate markers, click event and chart data
                data.forEach(element => {
                    //Create marker
                    var marker = new google.maps.Marker({
                        position: {
                            lat: parseFloat(element.highLatitude),
                            lng: parseFloat(element.highLongitude)
                        },
                        map: map,
                        icon: (element.securityData.includes("WPA") || element.securityData.includes("WEP")) ? "http://maps.google.com/mapfiles/ms/icons/green-dot.png" : "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
                        label: {
                            text: element.ssid,
                            color: "white" 
                        }
                    });

                    //Add click event
                    google.maps.event.addListener(marker, 'click', () => {
                        window.location.replace(accesspointUrl + "?id=" + element.id.toString());
                    });

                    //Check security
                    if(element.securityData.includes("WPA2")) {
                        securityChartData.WPA2++;
                    } else if(element.securityData.includes("WPA")) {
                        securityChartData.WPA++;
                    } else if(element.securityData.includes("WEP")) {
                        securityChartData.WEP++;
                    } else {
                        securityChartData.none++;
                    }

                    //Check brand
                    if(element.brand.toUpperCase().includes("TP-LINK")) {
                        brandChartData.tplink++;
                    } else if(element.brand.toUpperCase().includes("D-LINK")) {
                        brandChartData.dlink++;
                    } else if(element.brand.toUpperCase().includes("DASAN")) {
                        brandChartData.dasan++;
                    } else if(element.brand.toUpperCase().includes("SAGEM")) {
                        brandChartData.dasan++;
                    }

                    //Add frequency to array
                    if(freqChartDataTemp.length > 0) {
                        if(!freqChartDataTemp.find(x => x.freq == element.freq)) {
                            freqChartDataTemp.push({ freq: element.frequency, count: 1, add: () => { this.count++ } });
                        } else {
                            freqChartDataTemp.find(x => x.freq == element.freq).add();
                        }
                    } else {
                        freqChartDataTemp.push({ freq: element.frequency, count: 1, add: () => { this.count++ } });
                    }

                    //Add area to array
                    areaChartDataTemp.push({ ssid: element.ssid, area: element.signalArea });
                });

                //Sort frequency data and assign it to the output array
                freqChartDataTemp.sort((a, b) => (a.count < b.count) ? 1 : -1);
                for(let i=0; i<5; i++) { freqChartData.push(freqChartDataTemp[i]); }
                
                //Sort signal area data and assign it to the output array
                areaChartDataTemp.sort((a, b) => (a.area < b.area) ? 1 : -1);
                for(let i=0; i<5; i++) { areaChartData.push(areaChartDataTemp[i]); }

                //Cache response if local storage is available
                if(typeof(Storage) !== "undefined") {
                    const cacheObject = { data: data, securityChartData: securityChartData, brandChartData: brandChartData, freqChartData: freqChartData, areaChartData: areaChartData };
                    localStorage.responseCache = JSON.stringify({ timestamp: new Date().getTime(), data: cacheObject });
                }

                //Generate charts
                new Chart(charts.securityChart, {
                    type: 'bar',
                    data: {
                        labels: ['WPA2', 'WPA', 'WEP', 'none'],
                        datasets: [{
                            label: 'Count',
                            data: [securityChartData.WPA2, securityChartData.WPA, securityChartData.WEP, securityChartData.none],
                            backgroundColor: [color, color, color, color]
                        }]
                    },
                    options: {
                        legend: {
                            display: false,
                            labels: {
                                fontColor: '#2f8886'
                            }
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    display: false
                                },
                                gridLines: {
                                    display: false
                                }
                            }],
                            xAxes: [{
                                gridLines: {
                                    display: false
                                }
                            }]
                        }
                    }
                });

                new Chart(charts.brandChart, {
                    type: 'bar',
                    data: {
                        labels: ['Tp-Link', 'D-Link', 'Dasan', 'Sagem'],
                        datasets: [{
                            label: 'Count',
                            data: [brandChartData.tplink, brandChartData.dlink, brandChartData.dasan, brandChartData.sagem],
                            backgroundColor: [color, color, color, color]
                        }]
                    },
                    options: {
                        legend: {
                            display: false,
                            labels: {
                                fontColor: '#2f8886'
                            }
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    display: false
                                },
                                gridLines: {
                                    display: false
                                }
                            }],
                            xAxes: [{
                                gridLines: {
                                    display: false
                                }
                            }]
                        }
                    }
                });

                new Chart(charts.areaChart, {
                    type: 'bar',
                    data: {
                        labels: [ areaChartData[0].ssid.toString(), areaChartData[1].ssid.toString(), areaChartData[2].ssid.toString(), areaChartData[3].ssid.toString(), areaChartData[4].ssid.toString() ],
                        datasets: [{
                            label: 'Count',
                            data: [ areaChartData[0].area, areaChartData[1].area, areaChartData[2].area, areaChartData[3].area, areaChartData[4].area ],
                            backgroundColor: [color, color, color, color, color]
                        }]
                    },
                    options: {
                        legend: {
                            display: false,
                            labels: {
                                fontColor: '#2f8886'
                            }
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    display: false
                                },
                                gridLines: {
                                    display: false
                                }
                            }],
                            xAxes: [{
                                gridLines: {
                                    display: false
                                }
                            }]
                        }
                    }
                });

                new Chart(charts.freqChart, {
                    type: 'bar',
                    data: {
                        labels: [ freqChartData[0].freq, freqChartData[1].freq, freqChartData[2].freq, freqChartData[3].freq, freqChartData[4].freq ],
                        datasets: [{
                            label: 'Count',
                            data: [ freqChartData[0].count, freqChartData[1].count, freqChartData[2].count, freqChartData[3].count, freqChartData[4].count ],
                            backgroundColor: [color, color, color, color, color]
                        }]
                    },
                    options: {
                        legend: {
                            display: false,
                            labels: {
                                fontColor: '#2f8886'
                            }
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    display: false
                                },
                                gridLines: {
                                    display: false
                                }
                            }],
                            xAxes: [{
                                gridLines: {
                                    display: false
                                }
                            }]
                        }
                    }
                });
            })
            .catch(error => {
                document.getElementById('main').innerHTML = '<div class="row my-5"><div class="col text-center violet-border"><p class="h3">An error has occurded during server communication!</p></div></div><div class="row my-3"><div class="col text-center violet-border"><a href="/"><p class="h1">BACK</p></a></div></div> ';
            });  
    }
};
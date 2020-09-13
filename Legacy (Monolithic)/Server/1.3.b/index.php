<?php
    require_once("app/ViewHandler.php");
    require_once("app/VisitorHandler.php");

    //Creating a new ViewHandler
    $view = new ViewHandler();

    //Creating a new VisitorHandler
    $vs = new VisitorHandler($_SERVER['HTTP_USER_AGENT']);
?>

<!doctype html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

        <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
        <link href="https://fonts.googleapis.com/css?family=Barlow&display=swap" rel="stylesheet">
        <link rel="stylesheet" href="style/style.css">
        <link rel="icon" href="style/favicon.png">

        <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js"></script>
        
        <title>Access Point Map</title>
    </head>
    <body>
        <div class="container">
            <!-- Navbar -->
            <nav class="navbar navbar-expand-lg navbar-light my-3 navbar-bgcolor">
                <a class="navbar-brand" href="#">AccessPointMap</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
                    <div class="navbar-nav">
                        <a class="nav-item nav-link" href="#mapArea">Map</a>
                        <a class="nav-item nav-link" href="#charts">Statistics</a>
                        <a class="nav-item nav-link" href="search.php">Advanced Search</a>
                        <a class="nav-item nav-link" href="#info">Information</a>
                        <a class="nav-item nav-link" href="https://github.com/Krzysztofz01/AccessPointMap">Github</a>
                    </div>
                </div>
            </nav>
        
            <!-- Map object-->
            <div class="row my-2" id="mapArea">
                <div class="col">
                    <div id="map" class="violet-border" style="width: 100%; height:650px;"></div>
                </div>
            </div>

            <!-- Charts -->
            <div class="row my-2 mx-0" id="charts">
                <div class="col-lg-6 col-md-12 text-center violet-border">
                    <p class="h3">Security</p>
                    <canvas id="securityChart"></canvas>
                </div>
                <div class="col-lg-6 col-md-12 text-center violet-border">
                    <p class="h3">Brands</p>
                    <canvas id="brandChart"></canvas>
                </div>
            </div>

            <div class="row my-2 mx-0" id="charts">
                <div class="col-lg-6 col-md-12 text-center violet-border">
                    <p class="h3">Area</p>
                    <canvas id="areaChart"></canvas>
                </div>
                <div class="col-lg-6 col-md-12 text-center violet-border">
                    <p class="h3">Frequency</p>
                    <canvas id="freqChart"></canvas>
                </div>
            </div>
        
            <!-- Informations-->
            <div class="row my-2 mx-0" id="info">
                <div class="col text-center violet-border">
                    <p class="h3">Informations</p>
                    <p class="text-justify">
                        Projekt wykonał Krzysztof Zoń, uczeń CKZiU nr.2 ,,Mechanik" w Raciborzu. Projekt powstał w celu nauki
                        korzystania z różnych technologi webowych oraz nauki tworzenia aplikcji mobilnych przy użyciu technologi
                        Xamarin. Celem projektu jest rówienież stworzenie statystki występowania access pointów w okolicy Raciborza,
                        przedstawienie wkorzystywanych typów zabezpieczeń itp. Projekt nie przynośi żadnych korzyści finansowych i
                        jest open-source.
                    </p>
                </div>
            </div>
        
        </div>

        <!-- Script's -->
        <script>
            function initMap() {
                //Google Map
                let map = new google.maps.Map(document.getElementById("map"), {
                    zoom: 15,
                    center: {lat:  50.16, lng: 18.31},
                    mapTypeId: 'satellite'
                });

                //Adding markers
                let markers = [];
                <?php $view->getJavaScriptData($view->javaScriptData); ?>

                //Security Chart
                let securityChart = document.getElementById("securityChart").getContext('2d');
                let securityChartObject = new Chart(securityChart, {
                    type: 'bar',
                    data: {
                        labels: <?php $view->getSecurityChart("name"); ?>,
                        datasets: [{
                            label: 'Count',
                            data: <?php $view->getSecurityChart("value"); ?>,
                            backgroundColor: ["#7f50ad", "#6c4295", "#59347e", "#462567"],
                        }]
                    },
                    options: {
                        legend: {
                            display: false
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

                //Brand Chart
                let brandChart = document.getElementById("brandChart").getContext('2d');
                let brandChartObject = new Chart(brandChart, {
                    type: 'bar',
                    data: {
                        labels: <?php $view->getBrandsChart("name"); ?>,
                        datasets: [{
                            label: 'Count',
                            data: <?php $view->getBrandsChart("value"); ?>,
                            backgroundColor: ["#7f50ad", "#6c4295", "#59347e", "#462567", "#33174f"],
                        }]
                    },
                    options: {
                        legend: {
                            display: false
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
                                },
                                ticks: {
                                    autoskip: false,
                                    maxRotation: 0,
                                    minRotation: 0,
                                    fontSize: 10
                                }
                            }]
                        }
                    }
                });

                //Area Chart
                let areaChart = document.getElementById("areaChart").getContext('2d');
                let areaChartObject = new Chart(areaChart, {
                    type: 'bar',
                    data: {
                        labels: <?php $view->getAreaChart("name"); ?>,
                        datasets: [{
                            label: 'm^2',
                            data: <?php $view->getAreaChart("value"); ?>,
                            backgroundColor: ["#7f50ad", "#6c4295", "#59347e", "#462567", "#33174f"],
                        }]
                    },
                    options: {
                        legend: {
                            display: false
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
                                },
                                ticks: {
                                    autoskip: false,
                                    maxRotation: 0,
                                    minRotation: 0,
                                    fontSize: 8
                                }
                            }]
                        }
                    }
                });

                //Freq Chart
                let freqChart = document.getElementById("freqChart").getContext('2d');
                let freqChartObject = new Chart(freqChart, {
                    type: 'bar',
                    data: {
                        labels: <?php $view->getFreqChart("name"); ?>,
                        datasets: [{
                            label: 'Count',
                            data: <?php $view->getFreqChart("value"); ?>,
                            backgroundColor: ["#7f50ad", "#6c4295", "#59347e", "#462567", "#33174f"],
                        }]
                    },
                    options: {
                        legend: {
                            display: false
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
            }
        </script>
        
        <script async defer src="https://maps.googleapis.com/maps/api/js?key=&callback=initMap"></script>
        <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
        <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
    </body>
</html>
<?php
    require_once("app/SearchEngine.php");

    if(isset($_GET['id'])) {
        $se = new SearchEngine("record", $_GET['id']);
        $data = $se->getRecord();

        $position = "{lat: parseFloat(". $data['latitude'] . "), lng: parseFloat(". $data['longitude'] .")}";

        if($data == null) {
            header('Location: index.php');
            die();
        }
    }
    else {
        header('Location: index.php');
        die();
    }
?>

<!doctype html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

        <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
        <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js"></script>
        <link href="https://fonts.googleapis.com/css?family=Barlow&display=swap" rel="stylesheet">
        <link rel="stylesheet" href="style/style.css">
        
        <title>Access Point Map</title>
    </head>
    <body>
        <div class="container">
            <div class="row my-3">
                <div class="col text-center violet-border">
                    <p class="h1"><?php echo($data["ssid"]); ?></p>
                </div>
            </div>
            <div class="row mb-3">
                <div class="col violet-border">
                    <p class="h3">BSSID: <?php echo($data["bssid"]); ?></p>
                    <p class="h3">Frequency: <?php echo($data["freq"]); ?></p>
                    <p class="h3">Security: <?php echo($data["security"]); ?></p>
                    <p class="h3">Brand: <?php echo($data["vendor"]); ?></p>
                    <p class="h3">Strongest signal: <?php echo($data["signalLevel"]); ?></p>
                    <p class="h3">Lowest signal <?php echo($data["lowSignalLevel"]); ?></p>
                    <p class="h3">Signal area: <?php echo($data["signalArea"]); ?> m<sup>2</sup></p>
                </div>
            </div>
            <div class="row mb-3">
                <div class="col violet-border">
                    <div id="map" style="width: 100%; height:500px;"></div>
                </div>
            </div>
            <div class="row">
                <div class="col text-center violet-border">
                    <a href="index.php"><p class="h1">BACK</p></a>
                </div>
            </div>
        </div>

        <script>
            function initMap() {
                //Google Map
                let map = new google.maps.Map(document.getElementById("map"), {
                    zoom: 19,
                    center: <?php echo($position); ?>,
                    mapTypeId: 'satellite'
                });

                let marker = new google.maps.Marker({
                    position: <?php echo($position); ?>,
                    map: map
                })
            }
        </script>

        <script async defer src="https://maps.googleapis.com/maps/api/js?key=&callback=initMap"></script>
        <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
        <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
    </body>
</html>
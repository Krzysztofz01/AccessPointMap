<?php
    require_once("app/SearchEngine.php");
    $eng = new SearchEngine("database");

    if(isset($_POST["search"])) {
        $eng->generateTableData($_POST["security"], $_POST["brand"], $_POST["ssid"], $_POST["freq"]);
        unset($_POST["security"], $_POST["brand"], $_POST["ssid"], $_POST["freq"], $_POST["search"]);
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
            <form action="search.php" method="post" enctype="multipart/form-data">
                <div class="row mt-3">
                    <div class="col-lg-3 col-md-12 text-center">
                        <div class="form-group">
                            <label for="securityDropdown"><p class="h5">Security</p></label>
                            <select class="form-control" id="securityDropdown" name="security">
                                <option></option>
                                <option>WPA2</option>
                                <option>WPA</option>
                                <option>WEP</option>
                                <option>WPS</option>
                                <option>none / ESS</option>
                            </select>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-12 text-center">
                        <div class="form-group">
                            <label for="brandDropdown"><p class="h5">Brand</p></label>
                            <select class="form-control" id="brandDropdown" name="brand">
                                <option></option>
                                <?php $eng->getBrands(); ?>
                            </select>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-12 text-center">
                        <div class="form-group">
                            <label for="ssid"><p class="h5">Name / SSID</p></label>
                            <input type="text" class="form-control" id="ssid" placeholder="Name / SSID" name="ssid">
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-12 text-center">
                        <div class="form-group">
                            <label for="freq"><p class="h5">Frequency</p></label>
                            <input type="text" class="form-control" id="freq" placeholder="Frequency" name="freq">
                        </div>
                    </div>          
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12 text-center">
                        <button type="submit" class="btn btn-primary violet-button" name="search">Search</button>
                    </div>
                </div>
            </form>
            <div class="row my-3">
                <!-- OUTPUT -->
                <div class="col-lg-12 col-md-12 text-center">
                    <div class="table-responsive">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th scope="col">#</th>
                                    <th scope="col">SSID</th>
                                    <th scope="col">BSSID</th>
                                    <th scope="col">Frequency</th>
                                    <th scope="col">Security</th>
                                    <th scope="col">Brand</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php $eng->printTable(); ?>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

    
        <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
        <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
    </body>
</html>
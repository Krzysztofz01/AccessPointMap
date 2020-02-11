<?php
    ob_start();
    session_start();

    define("login", 'admin');
    define("password", 'admin');

    if(isset($_POST['adminLogin'])) {
        if(($_POST['login'] == login) && ($_POST['pass'] == password)) {
            $_SESSION['valid'] = true;
        } 
    }

    if(isset($_POST['adminLogout'])) {
        session_start();
        unset($_SESSION);
        unset($_POST['adminLogin'], $_POST['login'], $_POST['pass'], $_POST['adminLogout']);
        session_destroy();
        header('Location: admin.php');
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
        <link rel="icon" href="style/favicon.png">
        
        <title>Access Point Map</title>
    </head>
    <body>
        <div class="container my-2">
            <?php
            if(isset($_SESSION['valid'])) {
                echo('
                    <div class="row">
                        <div class="col-12 my-4 text-center">
                            <form action="admin.php" method="post" enctype="multipart/form-data">
                                <button type="submit" class="btn btn-primary violet-button mx-1" name="adminLogout">Logout</button>
                                <a class="btn btn-primary violet-button mx-1" href="https://panel.ct8.pl" role="button">Hosting</a>
                            </form>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 my-4">
                            <form action="app/fileHandler.php" method="post" enctype="multipart/form-data">
                                <div class="input-group">
                                    <div class="custom-file">
                                        <input type="file" accept=".json" class="custom-file-input" id="jsonFile" name="jsonFile" aria-describedby="jsonFile">
                                        <label class="custom-file-label" for="jsonFile">Choose json file (Light File)</label>
                                    </div>
                                    <div class="input-group-append">
                                        <button class="btn btn-outline-secondary" type="submit" id="uploadJsonFile" name="submit">Upload</button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12 my-4">
                            <form action="app/logHandler.php" method="post" enctype="multipart/form-data">
                                <div class="input-group">
                                    <div class="custom-file">
                                        <input type="file" accept=".json" class="custom-file-input" id="logFile" name="logFile" aria-describedby="logFile">
                                        <label class="custom-file-label" for="logFile">Choose log file (Hard File)</label>
                                    </div>
                                    <div class="input-group-append">
                                        <button class="btn btn-outline-secondary" type="submit" id="uploadLogFile" name="submit">Upload</button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                ');
            }
            else {
                echo('
                    <div class="row">
                        <div class="col-12 my-4">
                            <form action="admin.php" method="post" enctype="multipart/form-data">
                                <div class="form-group">
                                    <label for="login">Logins</label>
                                    <input type="login" class="form-control" id="login" aria-describedby="emailHelp" placeholder="Enter login" name="login">
                                </div>
                                <div class="form-group">
                                    <label for="pass">Password</label>
                                    <input type="password" class="form-control" id="pass" placeholder="Password" name="pass">
                                </div>
                                    <button type="submit" class="btn btn-primary" name="adminLogin">Submit</button>
                            </form>
                        </div>
                    </div>
                ');
            }
            ?>     
        </div>

        <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
        <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
    </body>
</html>
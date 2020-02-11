<?php
//Include the log file parsers class
include_once "LogParser.php";

session_start();
if(isset($_SESSION['valid'])) {
    if(isset($_POST["submit"])) {
        if(isset($_FILES['logFile'])) {
            $fileError = array();
            $fileName = $_FILES['logFile']['name'];
            $fileTmp = $_FILES['logFile']['tmp_name'];
            $fileSize = $_FILES['logFile']['size'];
            $fileExt = $_FILES['logFile']['type'];

            //Check if the file type is JSON
            if($fileExt != "application/json") {
                $fileError[] = "Extension error!";
            }

            //Check if file size is smaller than 30MB
            if($fileSize > 31457280) {
                $fileError[] = "Size error!";
            }

            if(empty($fileError)) {
                move_uploaded_file($fileTmp, "../json/".$fileName);
                $parser = new LogParser($fileName);
                $parser->parse();
                
                if($parser->sendToApi()) {
                    rename("../json/".$fileName, "../json/". strval(date("YmdHis") .".json"));
                    unset($_FILES['jsonFile']);
                    echo("<script>alert('All data uploaded successful!');window.location.replace('../admin.php');</script>");
                }
                else {
                    rename("../json/".$fileName, "../json/". strval(date("YmdHis") .".json"));
                    unset($_FILES['jsonFile']);
                    echo("<script>alert('Some errors occured during the upload!');window.location.replace('../admin.php');</script>");
                }
            }
        }
    }
}
else {
    header('Location: ../index.php');
    die(); 
}
?>
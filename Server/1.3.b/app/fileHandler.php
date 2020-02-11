<?php
session_start();
if(isset($_SESSION['valid'])) {
    if(isset($_POST["submit"])) {
        if(isset($_FILES['jsonFile'])) {
            $fileError = array();
            $fileName = $_FILES['jsonFile']['name'];
            $fileTmp = $_FILES['jsonFile']['tmp_name'];
            $fileSize = $_FILES['jsonFile']['size'];
            $fileExt = $_FILES['jsonFile']['type'];

            //Check if the file type is JSON
            if($fileExt != "application/json") {
                $fileError[] = "Extension error!";
            }

            //Check if file size is smaller than 8MB
            if($fileSize > 8388608) {
                $fileError[] = "Size error!";
            }

            if(empty($fileError)) {
                move_uploaded_file($fileTmp, "../json/".$fileName);
                $jsonString = file_get_contents("../json/".$fileName);
                $jsonArray = json_decode($jsonString, true);
                define("apiCall", "http://localhost/accesspointmap/server/1.3.b/api/request/add.php");
                
                for($i=0; $i < count($jsonArray); $i++) {
                    $ch = curl_init();
                    curl_setopt($ch, CURLOPT_URL, apiCall);
                    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
                    //curl_setopt($ch, CURLOPT_HTTPHEADER, array("Content-type: application/json"));
                    curl_setopt($ch, CURLOPT_POST, true);
                    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($jsonArray[$i]));

                    $response = curl_exec($ch);

                    if((curl_getinfo($ch, CURLINFO_HTTP_CODE) != 201) && (curl_getinfo($ch, CURLINFO_HTTP_CODE) != 200)) {
                        $fileError[] = "API call error on ". $i ."!";
                    }

                    curl_close($ch);
                }

                rename("../json/".$fileName, "../json/". strval(date("YmdHis") .".json"));
                unset($_FILES['jsonFile']);

                if(empty($fileError)) {
                    echo("<script>alert('All data uploaded successful!');window.location.replace('../admin.php');</script>");
                }
                else {
                    echo("<script>alert('Some errors occured during the upload!');window.location.replace('../admin.php');</script>");
                }
                die();  
            }
        }
    }
}
else {
    header('Location: ../index.php');
    die(); 
}
?>
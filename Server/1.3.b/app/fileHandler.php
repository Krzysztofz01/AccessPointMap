<?php

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
            /*echo("<script>alert('DZIALA');window.location.replace('../admin.php');</script>");
            die();*/
            move_uploaded_file($fileTmp, "../json/".$fileName);
            var_dump($fileName);
            //rename(    "../json/".date("Y-m-d H:i:s").".json")
        }
    }
}



/*

    if(empty($fileError)) {
        move_uploaded_file($fileTmp, "json/".$fileName);

        $jsonString = file_get_contents("json/".$fileName);

        $jsonArray = json_decode($jsonString, true);

        for($i = 0; $i < sizeof($jsonArray); $i++) {
            $ch = curl_init();
            curl_setopt($ch, CURLOPT_URL, "../api/request/add.php");
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            curl_setopt($ch, CURLOPT_HTTPHEADER, array("Content-type: application/json"));
            curl_setopt($ch, CURLOPT_POST, true);
            curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($jsonArray[$i]));

            $response = curl_exec($ch);

            if(curl_getinfo($ch, CURLINFO_HTTP_CODE) != 201) {
                $fileError[] = "API call error on ". $i ."!";
            }

            curl_close($ch);
        }

        if(empty($fileError)) {
            unlink("json/".$fileName);
        }
    }

    unset($_FILES['jsonFile']);
}

*/
?>
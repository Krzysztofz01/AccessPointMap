<?php

if(isset($_FILES['jsonFile'])) {
    $fileError = array();
    $fileName = $_FILES['jsonFile']['name'];
    $fileTmp = $_FILES['jsonFile']['tmp_name'];
    $fileSize = $_FILES['jsonFile']['size'];
    $fileExt = strtolower(end(explode('.', $fileName)));

    if($fileExt != "json") {
        $fileError[] = "Extension error!";
    }

    if($fileSize > 8388608) {
        $fileError[] = "Size error!";
    }

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
?>
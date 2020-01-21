<?php
    header("Access-Control-Allow-Origin: *");
    header("Content-Type: application/json; charset=UTF-8");

    //Database connection
    include_once "../config/database.php";

    //Init the model
    include_once "../models/AccessPoint.php";

    $database = new Database();
    $db = $database->getConnection();

    $accessPoint = new AccessPoint($db);

    $stmt = $accessPoint->read();
    $num = $stmt->rowCount();

    if($num>0) {
        $accessPointArray = array();
        $accessPointArray["records"] = array();
        $accessPointArray["message"] = array();

        while($row = $stmt->fetch(PDO::FETCH_ASSOC)) {
            extract($row);

            $accessPointItem = array(
                "id" => $id,
                "bssid" => $bssid,
                "ssid" => $ssid,
                "freq" => $freq,
                "signalLevel" => $signalLevel,
                "latitude" => $latitude,
                "longitude" => $longitude,
                "lowSignalLevel" => $lowSignalLevel,
                "lowLatitude" => $lowLatitude,
                "lowLongitude" => $lowLongitude,
                "signalArea" => $signalArea,
                "security" => $security,
                "vendor" => $vendor
            );

            array_push($accessPointArray["records"], $accessPointItem);
        }

        array_push($accessPointArray["message"], "Items found in database");

        //Data encoded to JSON, OK
        http_response_code(200);

        //Show data in JSON format
        echo json_encode($accessPointArray);
    } else {
        //Not data found
        http_response_code(404);

        //Showing the message that no items were found
        echo json_encode(array("message" => "No items found!"));
    }
?>
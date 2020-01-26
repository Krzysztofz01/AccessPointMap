<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");
header("Access-Control-Allow-Methods: POST");
header("Access-Control-Max-Age: 3600");
header("Access-Control-Allow-Headers: Content-Type, Access-Control-Alllow-Headers, Authorization, X-Requested-With");

//Database connection
include_once "../config/database.php";

//Init the model
include_once "../models/AccessPoint.php";

//The Mac Vendor API
include_once "../foreign/Vendor.php";

//Geomath alghoritm
include_once "../foreign/Geomath.php";

//Read from database API call URL
define("apiReadCall", "http://localhost/accesspointmap/server/1.3.b/api/request/read.php");

//Control the existing of the record
$recordExist = false;

$database = new Database();
$db = $database->getConnection();

$accessPoint = new AccessPoint($db);

//Get the JSON data (POST)
$data = json_decode(file_get_contents("php://input"));

//Verify the data
if(
    !empty($data->bssid) &&
    !empty($data->ssid) &&
    !empty($data->freq) &&
    !empty($data->signalLevel) &&
    !empty($data->latitude) &&
    !empty($data->longitude) &&
    !empty($data->lowSignalLevel) &&
    !empty($data->lowLatitude) &&
    !empty($data->lowLongitude) &&
    !empty($data->security)
) {

    //Calling the API to get all accessPoints to compare
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, apiReadCall);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    $response = json_decode(curl_exec($ch), true);
    curl_close($ch);

    //Check if record with this BSSID exist, decision whether to add or update
    if($response["message"] != "No items found!") {
        for($i = 0; $i < count($response["records"]); $i++) {
            if($data->bssid == $response["records"][$i]["bssid"]) {  
                $recordExist = true;

                if(($data->signalLevel > $response["records"][$i]["signalLevel"]) || ($data->lowSignalLevel < $response["records"][$i]["lowSignalLevel"])) {
                    //RECORD UPDATE!

                    //Set the BSSID of the AccessPoint to be edited
                    $accessPoint->bssid = $data->bssid;
                    $updateType = null;

                    if(($data->signalLevel > $response["records"][$i]["signalLevel"]) && ($data->lowSignalLevel < $response["records"][$i]["lowSignalLevel"])) {
                        //BOTH
                        $updateType = "BOTH";

                        $accessPoint->signalLevel = $data->signalLevel;
                        $accessPoint->latitude = $data->latitude;
                        $accessPoint->longitude = $data->longitude;
                        $accessPoint->lowSignalLevel = $data->lowSignalLevel;
                        $accessPoint->lowLatitude = $data->lowLatitude;
                        $accessPoint->lowLongitude = $data->lowLongitude;
                    }
                    else if($data->signalLevel > $response["records"][$i]["signalLevel"]) {
                        //HIGH SIGNAL
                        $updateType = "HIGH";

                        $accessPoint->signalLevel = $data->signalLevel;
                        $accessPoint->latitude = $data->latitude;
                        $accessPoint->longitude = $data->longitude;
                    }
                    else if($data->lowSignalLevel < $response["records"][$i]["lowSignalLevel"]) {
                        //LOW SIGNAL
                        $updateType = "LOW";

                        $accessPoint->lowSignalLevel = $data->lowSignalLevel;
                        $accessPoint->lowLatitude = $data->lowLatitude;
                        $accessPoint->lowLongitude = $data->lowLongitude;
                    }

                    $accessPoint->signalArea = GeoMath::getArea($data->latitude, $data->longitude, $data->lowLatitude, $data->lowLongitude);
                    
                    //Updata the AccessPoint
                    if($accessPoint->update($updateType)) {
                        //Data sucessful edited
                        http_response_code(200);
                        echo json_encode(array("message" => "Object updated in database"));
                    } else {
                        //Service unavailable
                        http_response_code(501);
                        echo json_encode(array("message" => "Operation failed!"));
                    }
                }
                break;
            }
        }
    }

    if(!$recordExist) {
        //RECORD CREATE

        $accessPoint->bssid = $data->bssid;
        $accessPoint->ssid = $data->ssid;
        $accessPoint->freq = $data->freq;
        $accessPoint->signalLevel = $data->signalLevel;
        $accessPoint->latitude = $data->latitude;
        $accessPoint->longitude = $data->longitude;
        $accessPoint->lowSignalLevel = $data->lowSignalLevel;
        $accessPoint->lowLatitude = $data->lowLatitude;
        $accessPoint->lowLongitude = $data->lowLongitude;
        $accessPoint->signalArea = GeoMath::getArea($data->latitude, $data->longitude, $data->lowLatitude, $data->lowLongitude);
        $accessPoint->security = $data->security;
        $accessPoint->vendor = Vendor::getVendor($data->bssid);

        if($accessPoint->add()) {
            //Object created and placed in the database
            http_response_code(201);
            echo json_encode(array("response" => "Object added to database"));
        } else {
            //Service unavailable
            http_response_code(503);
            echo json_encode(array("response" => "Operation failed"));
        }
    }
} else {
    //Bad data
    http_response_code(400);
    echo json_encode(array("response" => "Operation failed"));
}
?>
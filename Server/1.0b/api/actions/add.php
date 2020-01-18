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

$database = new Database();
$db = $database->getConnection();

$accessPoint = new AccessPoint($db);

//Get the JSON data (POST)
$data = json_decode(file_get_contents("php://input"));

// verify the data
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
    $accessPoint->bssid = $data->bssid;
    $accessPoint->ssid = $data->ssid;
    $accessPoint->freq = $data->freq;
    $accessPoint->signalLevel = $data->signalLevel;
    $accessPoint->latitude = $data->latitude;
    $accessPoint->longitude = $data->longitude;
    $accessPoint->lowSignalLevel = $data->lowSignalLevel;
    $accessPoint->lowLatitude = $data->lowLatitude;
    $accessPoint->lowLongitude = $data->lowLongitude;
    $accessPoint->signalArea = GeoMath::areaByGeoloc($data->latitude, $data->longitude, $data->lowLatitude, $data->lowLongitude);
    $accessPoint->security = $data->security;
    $accessPoint->vendor = Vendor::getVendor($data->bssid);

    if($accessPoint->add())
    {
        //Object created and placed in the database
        http_response_code(201);
        echo json_encode(array("response" => "OK"));
    } else {
        //Service unavailable
        http_response_code(503);
        echo json_encode(array("response" => "Operation failed"));
    }
} else {
    //Bad data
    http_response_code(400);
    echo json_encode(array("response" => "Operation failed"));
}
?>
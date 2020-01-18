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

$database = new Database();
$db = $database->getConnection();

$accessPoint = new AccessPoint($db);

//Get the JSON data (POST)
$data = json_decode(file_get_contents("php://input"));

//Set the BSSID of the AccessPoint to be edited
$accessPoint->bssid = $data->bssid;

$accessPoint->signalLevel = $data->signalLevel;
$accessPoint->latitude = $data->latitude;
$accessPoint->longitude = $data->longitude;

//Updata the AccessPoint
if($accessPoint->update()) {
    //Data sucessful edited
    http_response_code(200);

    //Send the respond
    echo json_encode(array("message" => "AccessPoint updated!"));
} else {
    //Service unavailable
    http_response_code(501);

    //Send the respond
    echo json_encode(array("message" => "Operation failed!"));
}
?>
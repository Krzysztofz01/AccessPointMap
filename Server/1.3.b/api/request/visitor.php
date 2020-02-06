<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");
header("Access-Control-Allow-Methods: POST");
header("Access-Control-Max-Age: 3600");
header("Access-Control-Allow-Headers: Content-Type, Access-Control-Alllow-Headers, Authorization, X-Requested-With");

//Database connection
include_once "../config/database.php";

//Init the model
include_once "../models/Visitor.php";

$database = new Database();
$db = $database->getConnection();

$visitor = new Visitor($db);

//Get the JSON data (POST)
$data = json_decode(file_get_contents("php://input"));

if(
    !empty($data->ipAddress) &&
    !empty($data->system) &&
    !empty($data->browser)
) {
    $visitor->ipAddress = $data->ipAddress;
    $visitor->system = $data->system;
    $visitor->browser = $data->browser;

    if($visitor->add()) {
        //Object created and placed in the database
        http_response_code(201);
        echo json_encode(array("response" => "Object added to database"));
    }
    else {
        //Service unavailable
        http_response_code(503);
        echo json_encode(array("response" => "Operation failed"));
    }
}
else {
    //Bad data
    http_response_code(400);
    echo json_encode(array("response" => "Operation failed"));
}
?>
<?php
class SearchEngine {
    //API Read call URL
    private $apiReadCall = "http://localhost/accesspointmap/server/1.3.b/api/request/read.php";
    //Container with data from API Read call
    private $data = null;
    private $record = null;

    public function __construct($type, $index = null) {
        //Call API to get AccessPoint informations
        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, $this->apiReadCall);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_POST, true);
        $this->data = json_decode(curl_exec($ch), true)["records"];
        curl_close($ch);
         
        if($index < count($this->data)) {
            switch($type) {
                case 'record': {
                    $this->record = $this->data[$index];
                    unset($this->data);
                } break;
                case 'database': {
                    unset($this->record);
                } break;
            }
        }
        else {
            $this->data = null;
        }
    }

    public function getRecord() {
        return $this->record;
    }

    public function getAllRecords() {
        return $this->data;
    } 
}
?>
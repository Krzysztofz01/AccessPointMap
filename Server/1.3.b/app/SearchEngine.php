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
         
        if(($this->data != null) && is_array($this->data)) {
            switch($type) {
                case "record": {
                    for($i=0; $i < count($this->data); $i++) {
                        if($this->data[$i]['id'] == $index) {
                            $this->record = $this->data[$i];
                            break;
                        }
                    }
                    unset($this->data);
                } break;
                case "database": {
                    unset($this->record);
                }
            }
        }
    }

    public function getRecord() {
        if(isset($this->record)) {
            return $this->record;
        }
        else {
            return null;
        }
        
    }

    public function getAllRecords() {
        if(isset($this->data)) {
            return $this->data;
        }
        else {
            return null;
        }
    } 
}
?>
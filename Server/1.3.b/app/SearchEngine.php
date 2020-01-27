<?php
class SearchEngine {
    //API Read call URL
    private $apiReadCall = "http://localhost/accesspointmap/server/1.3.b/api/request/read.php";
    //Container with data from API Read call
    private $data = null;
    private $record = null;

    private $tableData = null;
    private $tableStatus = null;

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

    public function getRadius() {
        if(isset($this->record)) {
            $lat1 = $this->record["latitude"];
            $long1 = $this->record["longitude"];
            $lat2 = $this->record["lowLatitude"];
            $long2 = $this->record["lowLongitude"];

            $p = 0.017453292519943295; //  PI : 180
		    $a = 0.5 - cos(($lat2 - $lat1) * $p)/2 + 
		    cos($lat1 * $p) * cos($lat2 * $p) * 
            (1 - cos(($long2 - $long1) * $p))/2;
            
            $output = (12742 * asin(sqrt($a))) * 1000;

            if($output < 3) {
                return 3;
            }
            else {
                return $output;
            }            
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
    
    public function getBrands() {
        $brands = array();
        for($i = 0; $i< count($this->data); $i++) {
            if(!$this->strCt($this->data[$i]["vendor"], "errors")) {
                if(count($brands) > 0) {
                    $exist = false;
                    for($j = 0; $j < count($brands); $j++) {
                        if($this->data[$i]["vendor"] == $brands[$j]) {
                            $exist = true;
                            break;
                        }
                    }

                    if(!$exist) {
                        array_push($brands, $this->data[$i]["vendor"]);
                    }
                }
                else {
                    array_push($brands, $this->data[$i]["vendor"]);
                }
            }
        }

        for($k = 0; $k < count($brands); $k++) {
            echo "<option>". $brands[$k] ."</option>". PHP_EOL;
        }
    }

    public function generateTableData($security, $brand, $ssid, $freq) {
        $this->tableData = array();

        for($i =0; $i < count($this->data); $i++) {
            $conditionsOk = true;

            if($security != "") {
                if(!$this->strCt($this->data[$i]["security"], $security)) {
                    $conditionsOk = false;
                }
            }

            if($brand != "") {
                if($this->data[$i]["vendor"] != $brand) {
                    $conditionsOk = false;
                }
            }

            if($ssid != "") {
                if(!$this->strCt($this->data[$i]["ssid"], $ssid)) {
                    $conditionsOk = false;
                }
            }

            if($freq != "") {
                if($this->data[$i]["freq"] != $freq) {
                    $conditionsOk = false;
                }
            }

            if($conditionsOk) {
                array_push($this->tableData, $this->data[$i]);
            }
        }

        if(count($this->tableData) > 0) {
            $this->tableStatus = true;
        }
    }

    public function printTable() {
        if($this->tableStatus) {
            for($i = 0; $i < count($this->tableData); $i++) {
                echo('<tr>
                    <th scope="row">'. intval($i + 1) .'</th>
                    <td><a href="accesspoint.php?id='. $this->tableData[$i]["id"] .'">'. $this->tableData[$i]["ssid"] .'</a></td>
                    <td>'. $this->tableData[$i]["bssid"] .'</td>
                    <td>'. $this->tableData[$i]["freq"] .'</td>
                    <td>'. $this->tableData[$i]["security"] .'</td>
                    <td>'. $this->tableData[$i]["vendor"] .'</td>
                </tr>');
            }      
        }
        
    }

    private function strCt($str, $contains) {
        if(strpos($str, $contains) !== false) {
            return true;
        }
        return false;
    }
}
?>
<?php
class LogParser {
    //API call add URL
    private $apiCallUrl = "http://localhost/accesspointmap/server/1.3.b/api/request/add.php";

    //AccessPoint objects container
    private $accessPointContainer = null;

    //Location of the log files
    private $path = "../json/";

    public function __construct($fileName) {
        $this->accessPointContainer = array();
        $this->path = $this->path . $fileName;
    }

    public function parse() {
        $tempArray = array();
        $logFile = fopen($this->path, "r");
  
        while(!feof($logFile))  {
            array_push($tempArray, json_decode(fgets($logFile)));
        }
        fclose($logFile);

        for($i=0; $i < count($tempArray); $i++) {
            if(count($this->accessPointContainer) < 1) {
                array_push($this->accessPointContainer, $tempArray[$i]);
            }
            else {
                $exist = false;
                for($j=0; $j < count($this->accessPointContainer); $j++) {
                    if($tempArray[$i]["bssid"] == $this->accessPointContainer[$j]["bssid"]) {
                        $exist = true;

                        if($tempArray[$i]["signalLevel"] > $this->accessPointContainer[$j]["signalLevel"]) {
                            $this->accessPointContainer[$j]["signalLevel"] = $tempArray[$i]["signalLevel"];
                            $this->accessPointContainer[$j]["latitude"] = $tempArray[$i]["latitude"];
                            $this->accessPointContainer[$j]["longitude"] = $tempArray[$i]["longitude"];
                        }

                        if($tempArray[$i]["lowSignalLevel"] < $this->accessPointContainer[$j]["lowSignalLevel"]) {
                            $this->accessPointContainer[$j]["lowSignalLevel"] = $tempArray[$i]["lowSignalLevel"];
                            $this->accessPointContainer[$j]["lowLatitude"] = $tempArray[$i]["lowLatitude"];
                            $this->accessPointContainer[$j]["lowLongitude"] = $tempArray[$i]["lowLongitude"];
                        }
                        break;
                    }
                }
                
                if(!exist) {
                    $tempArray[$i]["lowSignalLevel"] = $tempArray[$i]["signalLevel"];
                    $tempArray[$i]["lowLatitude"] = $tempArray[$i]["latitude"];
                    $tempArray[$i]["lowLongitude"] = $tempArray[$i]["longitude"];

                    array_push($this->accessPointContainer, $tempArray[$i]);
                }
            }
        }

        unset($tempArray);
    }

    public function sendToApi() {
        $noError = true;

        for($i=0; $i < count($this->accessPointContainer); $i++) {
            $ch = curl_init();
            curl_setopt($ch, CURLOPT_URL, $this->apiCallUrl);
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            curl_setopt($ch, CURLOPT_POST, true);
            curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($this->accessPointContainer[$i]));
            $response = curl_exec($ch);

            if((curl_getinfo($ch, CURLINFO_HTTP_CODE) != 201) && (curl_getinfo($ch, CURLINFO_HTTP_CODE) != 200)) {
                $noError = false;
            }

            curl_close($ch);
            
            return noError;
        }
    }
}
?>
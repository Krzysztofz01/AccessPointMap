<?php
class ViewHandler {
    //API Read call URL
    private $apiReadCall = "http://localhost/accesspointmap/server/1.3.b/api/request/read.php";
    //Container with data from API Read call
    private $data = null;

    //Check bool
    private $dataDelivered = null;

    //GoogleMaps API data for markers
    public $javaScriptData;
    
    //ChartJS data for stats
    public $securityChartData;
    public $brandsChartData;
    public $areaChartData;
    public $freqChartData;

    public function __construct() {
        //Call API to get AccessPoint informations
        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, $this->apiReadCall);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_POST, true);
        $this->data = json_decode(curl_exec($ch), true)["records"];
        curl_close($ch);

        if(($this->data != null) && is_array($this->data)) {
            $dataDelivered = true;
            $this->generateJavaScriptData($this->data);
            $this->generateSecurityChart($this->data);
            $this->generateBrandsChart($this->data);
            $this->generateAreaChart($this->data);
            $this->generateFreqChart($this->data);
        }
        else {
            $dataDelivered = false;
        }

        unset($this->data);
    }

    public function check() {
        return $this->dataDelivered;
    }

    private function generateJavaScriptData($dataSet) {
        $this->javaScriptData = array();
        
        for($i=0; $i < count($dataSet); $i++) {
            array_push($this->javaScriptData, array("latitude" => $dataSet[$i]["latitude"],
                                                    "longitude" => $dataSet[$i]["longitude"],
                                                    "id" => $dataSet[$i]["id"],
                                                    "ssid" => $dataSet[$i]["ssid"]));
        }
    }

    private function generateSecurityChart($dataSet) {
        $this->securityChartData = array();
        $this->securityChartData["WPA2"] = array("value" => 0, "name" => "WPA2");
        $this->securityChartData["WPA"] = array("value" => 0, "name" => "WPA");
        $this->securityChartData["WEP"] = array("value" => 0, "name" => "WEP");
        $this->securityChartData["none"] = array("value" => 0, "name" => "none");

        for($i = 0; $i < count($dataSet); $i++) {
            if($this->strCt($dataSet[$i]["security"], "WPA2")) {
                $this->securityChartData["WPA2"]["value"]++;
            }
            else if($this->strCt($dataSet[$i]["security"], "WPA")) {
                $this->securityChartData["WPA"]["value"]++;
            }
            else if($this->strCt($dataSet[$i]["security"], "WEP")) {
                $this->securityChartData["WEP"]["value"]++;
            }
            else
            {
                $this->securityChartData["none"]["value"]++;
            }
        }
    }

    private function generateBrandsChart($dataSet) {
        $this->brandsChartData = array();
        $this->brandsChartData['TpLink'] = array("value" => 0, "name" => "Tp-Link");
        $this->brandsChartData['DLink'] = array("value" => 0, "name" => "D-Link");
        $this->brandsChartData['Dasan'] = array("value" => 0, "name" => "Dasan (Ostrog.net)");
        $this->brandsChartData['Sagem'] = array("value" => 0, "name" => "Sagemcom (Orange)");
        $this->brandsChartData['Huawei'] = array("value" => 0, "name" => "Huawei (Play)");

        for($i = 0; $i < count($dataSet); $i++) { 
            if($this->strCt(strtoupper($dataSet[$i]["vendor"]), "TP-LINK")) {
                $this->brandsChartData['TpLink']["value"]++;
            }
            else if($this->strCt(strtoupper($dataSet[$i]["vendor"]), "D-LINK")) {
                $this->brandsChartData['DLink']["value"]++;
            }
            else if($this->strCt(strtoupper($dataSet[$i]["vendor"]), "DASAN")) {
                $this->brandsChartData['Dasan']["value"]++;
            }
            else if($this->strCt(strtoupper($dataSet[$i]["vendor"]), "SAGEMCOM")) {
                $this->brandsChartData['Sagem']["value"]++;
            }
            else if($this->strCt(strtoupper($dataSet[$i]["vendor"]), "HUAWEI")) {
                $this->brandsChartData['Huawei']["value"]++;
            }
        }
    }

    private function generateAreaChart($dataSet) {
        $this->areaChartData = array();
        $this->areaChartData[0] = array("value" => 0, "name" => "");
        $this->areaChartData[1] = array("value" => 0, "name" => "");
        $this->areaChartData[2] = array("value" => 0, "name" => "");
        $this->areaChartData[3] = array("value" => 0, "name" => "");
        $this->areaChartData[4] = array("value" => 0, "name" => "");

        usort($dataSet, function($a, $b) {
            if($a["signalArea"] < $b["signalArea"]) {
                return true;
            }
            return false; 
        });

        for($i=0; $i < 5; $i++) {
            $this->areaChartData[$i]["value"] = $dataSet[$i]["signalArea"];
            $this->areaChartData[$i]["name"] = $dataSet[$i]["ssid"];
        }
    }

    private function generateFreqChart($dataSet) {
        $this->freqChartData = array();
        
        for($i=0; $i < count($dataSet); $i++) {
            $exist = false;

            if(count($this->freqChartData) > 0) {
                for($j=0; $j < count($this->freqChartData); $j++) {
                    if($dataSet[$i]["freq"] == $this->freqChartData[$j]["name"]) {
                        $exist = true;
                        $this->freqChartData[$j]["value"]++;
                    }
                }
            }

            if(!$exist) {
                array_push($this->freqChartData, array("name" => $dataSet[$i]["freq"], "value" => 1));
            } 
        }
    }

    private function strCt($str, $contains) {
        if(strpos($str, $contains) !== false) {
            return true;
        }
        return false;
    }

    public function getSecurityChart($dataType) {
        echo("['" . $this->securityChartData['WPA2'][$dataType] . "', '" .
                    $this->securityChartData['WPA'][$dataType] . "', '" .
                    $this->securityChartData['WEP'][$dataType] . "', '" .
                    $this->securityChartData['none'][$dataType] . "']");
    }

    public function getBrandsChart($dataType) {
        echo("['" . $this->brandsChartData['TpLink'][$dataType] . "', '" .
                    $this->brandsChartData['DLink'][$dataType] . "', '" .
                    $this->brandsChartData['Dasan'][$dataType] . "', '" .
                    $this->brandsChartData['Sagem'][$dataType] . "', '" .
                    $this->brandsChartData['Huawei'][$dataType] . "']");
    }

    public function getAreaChart($dataType) {
        echo("['" . $this->areaChartData[0][$dataType] . "', '" .
                    $this->areaChartData[1][$dataType] . "', '" .
                    $this->areaChartData[2][$dataType] . "', '" .
                    $this->areaChartData[3][$dataType] . "', '" .
                    $this->areaChartData[4][$dataType] . "']");
    }

    public function getFreqChart($dataType) {
        echo("['" . $this->freqChartData[0][$dataType] . "', '" .
                    $this->freqChartData[1][$dataType] . "', '" .
                    $this->freqChartData[2][$dataType] . "', '" .
                    $this->freqChartData[3][$dataType] . "', '" .
                    $this->freqChartData[4][$dataType] . "']");
    }

    /*public function getJavaScriptData($dataContainer) {
        for($i=0; $i < count($dataContainer); $i++) {
            echo("markers = new google.maps.Marker({".
                "position: {lat: parseFloat(". $dataContainer[$i]["latitude"] ."), lng: parseFloat(". $dataContainer[$i]["longitude"] .")},".
                "map: map,".
                "label: '". $dataContainer[$i]["ssid"] ."'});". PHP_EOL);
        }
    }*/

    public function getJavaScriptData($dataContainer) {
        for($i=0; $i < count($dataContainer); $i++) {
            echo("tmpMark = new google.maps.Marker({".
                "position: {lat: parseFloat(". $dataContainer[$i]["latitude"] ."), lng: parseFloat(". $dataContainer[$i]["longitude"] .")},".
                "map: map,".
                "label: {text: '". $dataContainer[$i]["ssid"] ."', color: 'white'}});". PHP_EOL);
            echo("google.maps.event.addListener(tmpMark, 'click', function() {
                window.location.replace('accesspoint.php?id=". $dataContainer[$i]["id"] ."');
            });". PHP_EOL);

            echo("markers = tmpMark;". PHP_EOL);
        }
    }
}
?>
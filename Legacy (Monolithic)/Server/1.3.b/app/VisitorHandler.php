<?php
class VisitorHandler {
    //API Visitor add call URL
    private $apiCallUrl = "localhost/accesspointmap/server/1.3.b/api/request/visitor.php";
    private $userAgent = null;

    public function __construct($agent) {
        $this->userAgent = $agent;

        $data = array(
            'ipAddress' => $this->getIpAddress(),
            'browser' => $this->getBrowser(),
            'system' => $this->getOS()
        );

        $this->visitor = $data;
        
        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, $this->apiCallUrl);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_POST, true);
        curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));
        $resp = curl_exec($ch);
        curl_close($ch);
    }

    private function getIpAddress() {
        if(!empty($_SERVER['HTTP_CLIENT_IP'])) {
            //ip from share internet
            $ip = $_SERVER['HTTP_CLIENT_IP'];
        }
        else if(!empty($_SERVER['HTTP_X_FORWARDED_FOR'])) {
            //ip pass from proxy
            $ip = $_SERVER['HTTP_X_FORWARDED_FOR'];
        }
        else {
            $ip = $_SERVER['REMOTE_ADDR'];
        }
        return $ip;
    }

    private function getBrowser() {
        $browser        = "Unknown Browser";
        $browser_array = array(
                                '/msie/i'      => 'Internet Explorer',
                                '/firefox/i'   => 'Firefox',
                                '/safari/i'    => 'Safari',
                                '/chrome/i'    => 'Chrome',
                                '/edge/i'      => 'Edge',
                                '/opera/i'     => 'Opera',
                                '/netscape/i'  => 'Netscape',
                                '/maxthon/i'   => 'Maxthon',
                                '/konqueror/i' => 'Konqueror',
                                '/mobile/i'    => 'Handheld Browser'
                         );
    
        foreach ($browser_array as $regex => $value) {
            if (preg_match($regex, $this->userAgent)) {
                $browser = $value;    
            }
        }

        return $browser;
    }

    private function getOS() { 
        $os_platform  = "Unknown OS Platform";
        $os_array     = array(
                              '/windows nt 10/i'      =>  'Windows 10',
                              '/windows nt 6.3/i'     =>  'Windows 8.1',
                              '/windows nt 6.2/i'     =>  'Windows 8',
                              '/windows nt 6.1/i'     =>  'Windows 7',
                              '/windows nt 6.0/i'     =>  'Windows Vista',
                              '/windows nt 5.2/i'     =>  'Windows Server',
                              '/windows nt 5.1/i'     =>  'Windows XP',
                              '/windows xp/i'         =>  'Windows XP',
                              '/windows nt 5.0/i'     =>  'Windows 2000',
                              '/windows me/i'         =>  'Windows ME',
                              '/win98/i'              =>  'Windows 98',
                              '/win95/i'              =>  'Windows 95',
                              '/win16/i'              =>  'Windows 3.11',
                              '/macintosh|mac os x/i' =>  'Mac OS X',
                              '/mac_powerpc/i'        =>  'Mac OS 9',
                              '/linux/i'              =>  'Linux',
                              '/ubuntu/i'             =>  'Ubuntu',
                              '/iphone/i'             =>  'iPhone',
                              '/ipod/i'               =>  'iPod',
                              '/ipad/i'               =>  'iPad',
                              '/android/i'            =>  'Android',
                              '/blackberry/i'         =>  'BlackBerry',
                              '/webos/i'              =>  'Mobile'
                        );
    
        foreach ($os_array as $regex => $value) {
            if (preg_match($regex, $this->userAgent)) {
                $os_platform = $value;
            }
        }
    
        return $os_platform;
    }
}
?>
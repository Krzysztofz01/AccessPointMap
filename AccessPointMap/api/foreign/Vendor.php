<?php
class Vendor {

    public static function getVendor($mac_address) {
      $url = "https://api.macvendors.com/" . urlencode($mac_address);
      $ch = curl_init();
      curl_setopt($ch, CURLOPT_URL, $url);
      curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
      
      $response = curl_exec($ch);
      if($response) {
        return $response;
      } else {
        return "Not Found";
      }
    }
} 
?>
<?php
class Geomath {
	public static function getDistance($lat1, $long1, $lat2, $long2) {
		$p = 0.017453292519943295; //  PI : 180
		$a = 0.5 - cos(($lat2 - $lat1) * $p)/2 + 
		cos($lat1 * $p) * cos($lat2 * $p) * 
		(1 - cos(($long2 - $long1) * $p))/2;

		return intval((12742 * asin(sqrt($a))) * 1000);
	}

	public static function getArea($lat1, $long1, $lat2, $long2) {
		return 3.1415 * pow(Geomath::getDistance($lat1, $long1, $lat2, $long2), 2);
	}
}
?>
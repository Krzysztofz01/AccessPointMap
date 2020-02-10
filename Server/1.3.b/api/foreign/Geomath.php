<?php
class Geomath {
	public static function getDistance($lat1, $lon1, $lat2, $lon2) {
			$latitudeDistance = ($lat2 - $lat1) * M_PI / 180.0;
			$longitudeDistance = ($lon2 - $lon1) * M_PI / 180.0;

			//Convert to radians
			$lat1 = ($lat1) * M_PI / 180.0;
			$lat2 = ($lat2) * M_PI / 180.0;

			$a = pow(sin($latitudeDistance / 2), 2) +
				 pow(sin($longitudeDistance / 2), 2) *
				 cos($lat1) * cos($lat2);
			$c = 2 * asin(sqrt($a));

			return round((6371 * $c) * 1000, 2, PHP_ROUND_HALF_UP);
	}
		
	public static function getArea($lat1, $lon1, $lat2, $lon2) {
		return round(M_PI * pow(getDistance($lat1, $lon1, $lat2, $lon2), 2), 2, PHP_ROUND_HALF_UP);
	}
}
?>
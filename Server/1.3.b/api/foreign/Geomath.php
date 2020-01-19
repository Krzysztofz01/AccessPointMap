<?php
class GeoMath {	
    
    private static function degToRad($deg) {
		return $deg * pi() / 180.0;
	}
	
	private static function radToDeg($rad) {
		return $rad / pi() * 180;
	}
	
    public static function getDistance($latitudeOne, $longitudeOne, $latitudeTwo, $longitudeTwo) {
        if(($latitudeOne == $latitudeTwo) && ($longitudeOne==$longitudeTwo)) {
			return 0;
        } else {
			$theta = longitudeOne - longitudeTwo;
			
			$distance = sin(degToRad(latitudeOne)) * sin(degToRad(latitudeTwo)) + cos(degToRad(latitudeOne)) * cos(degToRad(latitudeTwo)) * cos(degToRad(theta));
			$distance = acos(distance);
			$distance = radToDeg(distance);
			$distance = distance * 60 * 1.1515;
			
			return distance * 1.609344;
		}
	}
	
	public static function areaByRadius($radius) {
		return pow($radius, 2) * pi();
	}
	
	public static function areaByGeoloc($latitudeOne, $longitudeOne, $latitudeTwo, $longitudeTwo) {
		return pow(getDistance(latitudeOne, longitudeOne, latitudeTwo, longitudeTwo), 2) * pi();
	}
}
?>
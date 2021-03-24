//Gps data parser library
#include <TinyGPS++.h>

//ESP8266 Wifi Driver libraries
#include <BearSSLHelpers.h>
#include <CertStoreBearSSL.h>
#include <ESP8266WiFi.h>
#include <ESP8266WiFiAP.h>
#include <ESP8266WiFiGeneric.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266WiFiScan.h>
#include <ESP8266WiFiSTA.h>
#include <ESP8266WiFiType.h>
#include <WiFiClient.h>
#include <WiFiClientSecure.h>
#include <WiFiClientSecureAxTLS.h>
#include <WiFiClientSecureBearSSL.h>
#include <WiFiServer.h>
#include <WiFiServerSecure.h>
#include <WiFiServerSecureAxTLS.h>
#include <WiFiServerSecureBearSSL.h>
#include <WiFiUdp.h>

//Multiple virtual serial ports library
#include <SoftwareSerial.h>

TinyGPSPlus gps;
SoftwareSerial gpsUart(4, 5);

//Geolocation variables
float latitude = NULL;
float longitude = NULL;

//LED indicator pins
const int WAIT_FOR_GPS_AVAILABLE = 0;
const int SCAN_CYCLE = 2;

void setup() {
  //UART communication setup
  Serial.begin(115200);
  gpsUart.begin(9600);

  //Wifi scanner setip
  WiFi.mode(WIFI_STA);
  WiFi.disconnect();

  //Indicator LED setup
  pinMode(WAIT_FOR_GPS_AVAILABLE, OUTPUT);
  digitalWrite(WAIT_FOR_GPS_AVAILABLE, HIGH);
  
  pinMode(SCAN_CYCLE, OUTPUT);
  digitalWrite(SCAN_CYCLE, LOW);
  
  
  delay(2000);
  Serial.println("Setup finished!");
}

void loop() {
  //Check if data on GPS serial port is available
  while(gpsUart.available() > 0) {
    //Encode GPS data if correct
    if(gps.encode(gpsUart.read())) {
      if(gps.location.isValid()) {
        latitude = gps.location.lat();
        longitude = gps.location.lng();
      }
    }
  }

  //Proceed if GPS connection is established
  if(latitude != NULL && longitude != NULL) {
    digitalWrite(WAIT_FOR_GPS_AVAILABLE, LOW);
    indicateScanCycle();

    //Get number of available accesspoints
    int accesspoints = WiFi.scanNetworks();

    if(accesspoints > 0) {
      for(int i=0; i < accesspoints; i++) {
        Serial.println("{\"bssid\": \"" + WiFi.BSSIDstr(i) + "\", \"ssid\": \"" + WiFi.SSID(i) + "\", \"signalLevel\": \"" + WiFi.RSSI(i) + "\", \"securityDataRaw\": \"" + getEncryptionType(WiFi.encryptionType(i)) + "\",  \"latitude\": \"" + String(latitude, 6) + "\", \"longitude\": \"" + String(longitude, 6) + "\"}");
      }
    }
    Serial.println("");
  }
}

void indicateScanCycle() {
  digitalWrite(SCAN_CYCLE, HIGH);
  delay(5);
  digitalWrite(SCAN_CYCLE, LOW);
}

String getEncryptionType(int type) {
  if(type == 2 || type == 4) return "WPA";
  if(type == 5) return "WEP";
  return "None";
}

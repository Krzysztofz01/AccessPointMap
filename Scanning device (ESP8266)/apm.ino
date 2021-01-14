#include <ESP8266WiFi.h>

void setup() {
  Serial.begin(115200);
  Serial.println("AccessPointMap - Setup...");
  WiFi.mode(WIFI_STA);
  WiFi.disconnect();
  delay(2000);
  Serial.println("AccessPointMap - Setup done.");

}

void loop() {
  Serial.println("Scanning...");
  int accesspoints = WiFi.scanNetworks();
  if(accesspoints == 0) {
    Serial.println("No networks found.");    
  } else {
    Serial.println("##########################");
    for(int i = 0; i < accesspoints; i++) {
      Serial.print(WiFi.BSSIDstr(i));
      Serial.print(" ");
      Serial.print(WiFi.SSID(i));
      Serial.print(" ");
      Serial.print(WiFi.RSSI(i));
      Serial.print(" ");
      Serial.print(WiFi.encryptionType(i));
      Serial.println(" ");
    }
  }
}

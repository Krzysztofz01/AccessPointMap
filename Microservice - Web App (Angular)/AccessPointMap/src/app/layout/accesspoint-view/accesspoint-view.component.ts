import { Component, OnInit, Input } from '@angular/core';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import Map from 'ol/Map';
import View from 'ol/View';
import Circle from 'ol/geom/Circle';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import Style from 'ol/style/Style';
import Icon from 'ol/style/Icon';
import { OSM, Vector as VectorSource } from 'ol/source';
import TileLayer from 'ol/layer/Tile';
import * as olProj from 'ol/proj';
import { SecurityDisplay } from 'src/app/models/security-display.model';

@Component({
  selector: 'accesspoint-view',
  templateUrl: './accesspoint-view.component.html',
  styleUrls: ['./accesspoint-view.component.css']
})
export class AccesspointViewComponent implements OnInit {
  @Input() accessPoint: Accesspoint;
  private map: Map;
  public security: SecurityDisplay;

  /*public accessPoint: Accesspoint = {
    id: 1,
        bssid: "44:e9:dd:13:19:8b",
        ssid: "FunBox-198B",
        frequency: 2412,
        highSignalLevel: -70,
        highLongitude: 18.3133361,
        highLatitude: 50.1603773,
        lowSignalLevel: -84,
        lowLongitude: 18.313226666666665,
        lowLatitude: 50.16050499999999,
        signalRadius: 16.19838968100478,
        signalArea: 824.29,
        securityData: "[\"ESS\",\"WPA\",\"WPA2\",\"WPS\"]",
        securityDataRaw: "[WPA2-PSK-CCMP+TKIP][WPA-PSK-CCMP+TKIP][ESS][WPS]",
        brand: "Sagemcom Broadband SAS",
        deviceType: "Default",
        display: true,
        postedBy: "default",
        createDate: null,
        updateDate: null
  };*/
  
  constructor() { }

  ngOnInit(): void {
      this.security = this.prepareSecurityInfo(this.accessPoint.securityData);
      this.initializeMap()
  }

  private initializeMap(): void {
    const circle = new Circle(olProj.fromLonLat([this.accessPoint.highLongitude, this.accessPoint.highLatitude]), this.accessPoint.signalRadius);
    const circleFeature = new Feature(circle);

    this.map = new Map({
      target: 'accesspoint-map',
      layers: [
        new TileLayer({
          source: new OSM()
        }),
        new VectorLayer({
          source: new VectorSource({
            features: [circleFeature]
          })
        })
      ],
      view: new View({
        center: olProj.fromLonLat([this.accessPoint.highLongitude, this.accessPoint.highLatitude]),
        zoom: 19
      })
    });
  }

  private prepareSecurityInfo(securityData: string): SecurityDisplay {
    const securityArray: Array<string> = JSON.parse(securityData);
    if(securityArray.includes('WPA2')) return { type: 'WPA2', description: 'The strongest security standard, WPA2 type uses CCMP / AES encryption.', color: '#68BC00' };
    if(securityArray.includes('WPA')) return { type: 'WPA', description: 'A slightly older type of security, using TKIP / RC4 and Michael (MIC) encryption. It is recommended to switch to WPA2.', color: '#68BC00' };
    if(securityArray.includes('WPS')) return { type: 'WPS', description: 'The WPS system automatically generates the parameters needed for connection. Misconfiguration can be a serious vulnerability.', color: '#FB9E00' };
    if(securityArray.includes('WEP')) return { type: 'WEP', description: 'This is a type of security developed in 1999. WEP is vulnerable to FMS attack which makes it necessary to switch to WPA / WPA2 security.', color: '#FB9E00' };
    return { type: 'None', description: 'The network is not secured. Anyone can steal valuable data. WPA / WPA2 security must be configured.', color: '#F44E3B' };
  }
}

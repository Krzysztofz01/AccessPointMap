import { Component, OnInit } from '@angular/core';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import Map from 'ol/Map';
import View from 'ol/View';
import Circle from 'ol/geom/Circle';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import { OSM, Vector as VectorSource } from 'ol/source';
import TileLayer from 'ol/layer/Tile';
import * as olProj from 'ol/proj';
import { SecurityDisplay } from 'src/app/models/security-display.model';
import { SelectedAccesspointService } from 'src/app/services/selected-accesspoint.service';

@Component({
  selector: 'accesspoint-view',
  templateUrl: './accesspoint-view.component.html',
  styleUrls: ['./accesspoint-view.component.css']
})
export class AccesspointViewComponent implements OnInit {
  public accessPoint: Accesspoint;
  private map: Map;
  public security: SecurityDisplay;

  constructor(private selectedAccesspoint: SelectedAccesspointService) { }

  ngOnInit(): void {  
    this.selectedAccesspoint.currentAccesspoint.subscribe(ap => {
      if(ap != null) {
        this.accessPoint = ap;
        this.security = this.prepareSecurityInfo(this.accessPoint.securityData);
        this.initializeMap();
        console.log(ap);
      }
    });
  }

  private initializeMap(): void {
      const circle = new Circle(olProj.fromLonLat([this.accessPoint.highLongitude, this.accessPoint.highLatitude]), this.accessPoint.signalRadius);
      const circleFeature = new Feature(circle);

      if(this.map === undefined) {
        this.map = new Map({
          target: 'accesspoint-map',
          layers: [
            new TileLayer({
              source: new OSM()
            }),
            new VectorLayer({
              name: 'circleLayer',
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
      } else {
        this.map.getView().setCenter(olProj.fromLonLat([this.accessPoint.highLongitude, this.accessPoint.highLatitude]));
        this.map.getLayers().forEach(layer => {
          if(layer.get('name') == 'circleLayer') {
            this.map.removeLayer(layer);
          }
        });

        this.map.addLayer(new VectorLayer({
          name: 'circleLayer',
          source: new VectorSource({
            features: [circleFeature]
          })
        }));
      }
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

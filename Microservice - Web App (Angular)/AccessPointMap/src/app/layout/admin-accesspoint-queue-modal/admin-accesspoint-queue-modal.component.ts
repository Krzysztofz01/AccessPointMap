import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import Map from 'ol/Map';
import View from 'ol/View';
import Circle from 'ol/geom/Circle';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import { OSM, Vector as VectorSource } from 'ol/source';
import TileLayer from 'ol/layer/Tile';
import * as olProj from 'ol/proj';
import { Style, Fill } from 'ol/style';
import { AccesspointQueue } from 'src/app/models/acesspoint-queue.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { AuthService } from 'src/app/auth/services/auth.service';

@Component({
  selector: 'app-admin-accesspoint-queue-modal',
  templateUrl: './admin-accesspoint-queue-modal.component.html',
  styleUrls: ['./admin-accesspoint-queue-modal.component.css']
})
export class AdminAccesspointQueueModalComponent {
  private map: Map;
  public ssid: string;
  public isNew: boolean = false;

  constructor(public modal: NgbActiveModal, private accesspointData: AccesspointDataService, private authService: AuthService) { }

  public mapInit(accesspoint: Accesspoint | AccesspointQueue): void {
    this.ssid = accesspoint.ssid;
    const circle = new Circle(olProj.fromLonLat([accesspoint.highLongitude, accesspoint.highLatitude]), accesspoint.signalRadius);
    const circleFeature = new Feature(circle);
    let knownCircleFeatures = null;

    this.map = new Map({
      controls: [],
      target: 'modal-queue-map',
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
        center: olProj.fromLonLat([accesspoint.highLongitude, accesspoint.highLatitude]),
        zoom: 18
      })
    });
      
    this.map.addLayer(new VectorLayer({
      name: 'circleLayer',
      source: new VectorSource({
        features: [circleFeature]
      })
    }));

    this.accesspointData.getAccessPointByBssid(accesspoint.bssid, this.authService.getToken())
      .subscribe((response) => {
        if(response != null) {
          const knownCircle = new Circle(olProj.fromLonLat([response.highLongitude, response.highLatitude]), response.signalRadius);
          knownCircleFeatures = new Feature(knownCircle);
        
          this.map.addLayer(new VectorLayer({
            name: 'knownCircleLayer',
            source: new VectorSource({
              features: [knownCircleFeatures]
            }),
            style: new Style({
              fill: new Fill({
                color: 'rgba(255, 0, 0, 0.3)'
              })
            })
          }));
        }
      },
      (error) => {
        knownCircleFeatures = null;
        this.isNew = true;
      });
  }
}

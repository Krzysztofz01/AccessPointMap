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
import { AccesspointQueue } from 'src/app/models/acesspoint-queue.model';

@Component({
  selector: 'app-accesspoint-view-modal',
  templateUrl: './accesspoint-view-modal.component.html',
  styleUrls: ['./accesspoint-view-modal.component.css']
})
export class AccesspointViewModalComponent {
  private map: Map;
  public ssid: string;

  constructor(public modal: NgbActiveModal) { }

  public mapInit(accesspoint: Accesspoint | AccesspointQueue): void {
    this.ssid = accesspoint.ssid;
    const circle = new Circle(olProj.fromLonLat([accesspoint.highLongitude, accesspoint.highLatitude]), (accesspoint.signalRadius < 16) ? 16 : accesspoint.signalRadius);
    const circleFeature = new Feature(circle);

    this.map = new Map({
      controls: [],
      target: 'modal-map',
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
  }
}

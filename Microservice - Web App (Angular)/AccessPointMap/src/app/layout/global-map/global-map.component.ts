import { Component, OnInit } from '@angular/core';
import Map from 'ol/Map';
import View from 'ol/View';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import Style from 'ol/style/Style';
import Icon from 'ol/style/Icon';
import OSM from 'ol/source/OSM';
import TileLayer from 'ol/layer/Tile';
import * as olProj from 'ol/proj';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';

@Component({
  selector: 'global-map',
  templateUrl: './global-map.component.html',
  styleUrls: ['./global-map.component.css']
})
export class GlobalMapComponent implements OnInit {
  private map: Map;

  constructor(private accesspointDataService: AccesspointDataService) { }

  ngOnInit(): void {
    this.map = new Map({
      target: 'global-map',
      layers: [
        new TileLayer({
          source: new OSM()
        })
      ],
      view: new View({
        center: olProj.fromLonLat([0.0, 0.0]),
        zoom: 5
      })
    });

    this.getMarkerLayer();
  }

  private getMarkerLayer() : VectorLayer {
    const features = new Array<Feature>();
    this.accesspointDataService.getAllAccessPoints("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImZyb250ZW5kQGFwbS5jb20iLCJyb2xlIjoiUmVhZCIsIm5iZiI6MTYwNzk0MjYyMiwiZXhwIjoxNjA3OTQ2MjIyLCJpYXQiOjE2MDc5NDI2MjJ9.mxISQlAU0r6Nc-AqXH97AaiL9bqgLxDm0WkF4oaCIW4").toPromise().then(data => {
      console.log(data);
      data.forEach(x => {
        let markerPoint = new Feature({
          geometry: new Point(olProj.transform([x.HighLongitude, x.HighLatitude], 'EPSG:4326', 'EPSG:3857'))
        });

        let markerStyle = new Style({
          image: Icon(({
            anchor: [0.5, 1],
            src: this.getPinColor(x.SecurityData)
          }))
        });

        markerPoint.setStyle(markerStyle);
        features.push(markerPoint);
      });
    });
  }

  private getPinColor(securityData: string) : string {
    const green = 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%2368BC00';
    const yellow = 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%2368BC00';
    const red = 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%23F44E3B';
    const securityDataArray = JSON.parse(securityData);

    if(securityDataArray.includes('WPA2') || securityDataArray.includes('WPA')) return green;
    if(securityDataArray.includes('WEP') || securityDataArray.includes('WPS')) return yellow;
    return red;
  }
}

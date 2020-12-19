import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import Map from 'ol/Map';
import View from 'ol/View';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import Style from 'ol/style/Style';
import Icon from 'ol/style/Icon';
import { OSM, Vector as VectorSource } from 'ol/source';
import TileLayer from 'ol/layer/Tile';
import * as olProj from 'ol/proj';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { LocalStorageOptions } from 'src/app/models/local-storage-options.model';
import { SelectedAccesspointService } from 'src/app/services/selected-accesspoint.service';

const CACHE_KEY = "VECTOR_LAYER_ACCESSPOINTS";

@Component({
  selector: 'global-map',
  templateUrl: './global-map.component.html',
  styleUrls: ['./global-map.component.css']
})
export class GlobalMapComponent implements OnInit {
  private map: Map;

  constructor(private accesspointDataService: AccesspointDataService, private cacheService: CacheManagerService, private selectedAccesspoint: SelectedAccesspointService) { }

  ngOnInit(): void {
    this.fetchMapContent();
  }

  private fetchMapContent(): void {
    const cachedData = this.cacheService.load(CACHE_KEY);
    if(cachedData == null) {
      this.accesspointDataService.getAllAccessPoints("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImZyb250ZW5kQGFwbS5jb20iLCJyb2xlIjoiUmVhZCIsIm5iZiI6MTYwODM4MTkxOCwiZXhwIjoxNjA4Mzg5MTE4LCJpYXQiOjE2MDgzODE5MTh9.59d8Zh2qw3s68smtvr-mCOTuUb4TOKOXHU8E8X-Ke0A").toPromise()
      .then(data => {
        const featuresContainer: Array<Feature> = new Array<Feature>();
        data.forEach(x => {
          featuresContainer.push(this.prepareSingleMarker(x));
        });
        
        this.initializeMap(featuresContainer);

        const options: LocalStorageOptions = { key: CACHE_KEY, data: data , expirationMinutes: 60 };
        this.cacheService.delete(CACHE_KEY);
        this.cacheService.save(options);
      })
      .catch(error => {
        console.log(error);
      });
    } else {
      console.log("test")
      const features: Array<Feature> = new Array<Feature>();
      cachedData.forEach(element => features.push(this.prepareSingleMarker(element)));
      this.initializeMap(features);
    }
  }

  private initializeMap(featuresArray: Array<Feature>): void {
    this.map = new Map({
      target: 'global-map',
      layers: [
        new TileLayer({
          source: new OSM()
        }),
        new VectorLayer({
          source: new VectorSource({
            features: featuresArray
          })
        })
      ],
      view: new View({
        center: olProj.fromLonLat([18.31, 50.16]),
        zoom: 16
      })
    });

    this.map.on('click', (e) => {
      const selectedAccessPoints: Array<Accesspoint> = new Array<Accesspoint>();
      this.map.forEachFeatureAtPixel(e.pixel, (feature, layer) => {
        selectedAccessPoints.push(feature.values_.attributes.accesspoint);
      });
      
      if(selectedAccessPoints.length) {
        this.selectedAccesspoint.changeAccesspoint(selectedAccessPoints[0]);
      }
    });
  }

  private prepareSingleMarker(accesspoint: Accesspoint) : Feature {
    const feat : Feature = new Feature({
      geometry: new Point(olProj.fromLonLat([accesspoint.highLongitude, accesspoint.highLatitude])),
      attributes: { accesspoint }
    });
    const style = new Style({
      image: new Icon({
        anchor: [0.5, 1],
        src: this.getPinColor(accesspoint)
      })
    });

    feat.setStyle(style);
    return feat;
  }

  private getPinColor(accesspoint: Accesspoint) : string {
    const green = 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%2368BC00';
    const yellow = 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%23FB9E00';
    const red = 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%23F44E3B';
    const blue = 'http://cdn.mapmarker.io/api/v1/pin?size=50&hoffset=1&background=%230062B1'; 
    const securityDataArray = JSON.parse(accesspoint.securityData);

    if(accesspoint.ssid == 'Hidden network') return blue;
    if(securityDataArray.includes('WPA2') || securityDataArray.includes('WPA')) return green;
    if(securityDataArray.includes('WEP') || securityDataArray.includes('WPS')) return yellow;
    return red;
  }
}

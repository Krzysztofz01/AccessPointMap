import { Component, OnInit } from '@angular/core';
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
import { Accesspoint } from 'src/app/models/accesspoint.model';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { SelectedAccesspointService } from 'src/app/services/selected-accesspoint.service';
import { DataFetchService } from 'src/app/services/data-fetch.service';

const CACHE_KEY = "VECTOR_LAYER_ACCESSPOINTS";

@Component({
  selector: 'global-map',
  templateUrl: './global-map.component.html',
  styleUrls: ['./global-map.component.css']
})
export class GlobalMapComponent implements OnInit {
  private map: Map;

  constructor(private cacheService: CacheManagerService, private selectedAccesspoint: SelectedAccesspointService, private dataFetchService: DataFetchService) { }

  ngOnInit(): void {
    this.initializeMapData();
  }

  private initializeMapData(): void {
    const featuresArray = new Array<Feature>();
    const cachedData: Array<Accesspoint> = this.cacheService.load(CACHE_KEY);
    if(cachedData == null) {
      this.dataFetchService.localDataCheck();
      const cachedData: Array<Accesspoint> = this.cacheService.load(CACHE_KEY);
    }
    cachedData.forEach(element => featuresArray.push(this.prepareSingleMarker(element)));
    this.initializeMap(featuresArray);
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

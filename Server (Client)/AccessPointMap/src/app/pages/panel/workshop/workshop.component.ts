import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import Map from 'ol/Map';
import View from 'ol/View';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import Style from 'ol/style/Style';
import Icon from 'ol/style/Icon';
import Text from 'ol/style/Text';
import Fill from 'ol/style/Fill';
import { OSM, Vector as VectorSource } from 'ol/source';
import TileLayer from 'ol/layer/Tile';
import * as olProj from 'ol/proj';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-workshop',
  templateUrl: './workshop.component.html',
  styleUrls: ['./workshop.component.css']
})
export class WorkshopComponent implements OnInit, AfterViewInit {
  public accessPointJsonData: string;

  public accessPointPreviewData: string;
  public mapHeight: string = '60vh';
  private zoomLevel: number = 16;
  private map: Map;

  constructor(private accessPointService: AccessPointService) { }

  ngOnInit(): void {
    this.accessPointJsonData = '';
    this.accessPointPreviewData = '';
  }

  ngAfterViewInit(): void {
    this.initializeMap();
  }

  //Parse uploaded data and swap vector
  public previewAccessPoints(): void {
    if(this.accessPointPreviewData.length) {

      const toCamelCase = (key, value) => {
        if (value && typeof value === 'object'){
          for (var k in value) {
            if (/^[A-Z]/.test(k) && Object.hasOwnProperty.call(value, k)) {
              value[k.charAt(0).toLowerCase() + k.substring(1)] = value[k];
              delete value[k];
            }
          }
        }
        return value;
      };

      const accessPoints: Array<AccessPoint> = JSON.parse(this.accessPointPreviewData, toCamelCase);

      const features: Array<Feature> = this.generateFeatures(accessPoints);

      const vector = new VectorLayer({
        source: new VectorSource({ features })
      });

      vector.set('name', 'marker_layer');

      this.map.getLayers().forEach(l => {
        if(l && l.get('name') === 'marker_layer') {
          this.map.removeLayer(l);
        }
      });

      this.map.addLayer(vector);
    }
  }

  //Generate vector layer
  private generateFeatures(accessPoints: Array<AccessPoint>): Array<Feature> {
    const features: Array<Feature> = new Array<Feature>();
      
      accessPoints.forEach(x => {
        const feature: Feature = new Feature({
          geometry: new Point(olProj.fromLonLat([ x.maxSignalLongitude, x.maxSignalLatitude ]))
        });

        const style: Style = new Style({
          image: new Icon({
            anchor: [ 0.5, 1 ],
            src: environment.PIN_ICON_ALTERNATIVE
          }),
          text: new Text({
            text: `${ x.ssid } (${ x.bssid })`,
            offsetY: -25,
            fill: new Fill({
              color: '#1CB5E0'
            })
          })
        });

        feature.setStyle(style);
        features.push(feature);
      });

      return features;
  }

  //Initialize the preview map
  private initializeMap(): void {
    this.map = new Map({
      controls: [],
      target: 'preview-map',
      layers: [
        new TileLayer({
          source: new OSM()
        })
      ],
      view: new View({
        center: olProj.fromLonLat([ 18.31, 50.16 ]),
        zoom: this.zoomLevel
      })
    })
  }
}

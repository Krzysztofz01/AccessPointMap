import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccessPoint } from 'src/app/core/models/access-point.model';
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
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit, AfterViewInit{
  @Input() accessPointsObservable: Observable<Array<AccessPoint>>;
  @Input() mapId: string;
  @Input() mapHeight: string;
  @Input() centerLatitude: number;
  @Input() centerLongitude: number;
  @Input() zoomLevel: number;
  @Output() markerClick = new EventEmitter<Array<AccessPoint>>(false);

  private accessPoints: Array<AccessPoint>;
  private map: Map;

  public securityTypeForm: FormGroup;
  public securityTypesOptions: Array<string> = [ "All", "WPA3", "WPA2", "WPA", "WEP", "WPS", "None" ];

  public ssidForm: FormGroup;

  ngOnInit(): void {
    this.securityTypeForm = new FormGroup({
      selectType: new FormControl('All')
    });

    this.ssidForm = new FormGroup({
      text: new FormControl(null)
    });

    this.securityTypeForm.get('selectType').valueChanges.subscribe(val => {
      const features = this.generateMapFeatures(this.accessPoints, val, this.ssidForm.get('text').value);
      console.log(features.length);
      this.swapVector(features);
    });

    this.ssidForm.get('text').valueChanges.subscribe(val => {
      const features = this.generateMapFeatures(this.accessPoints, this.securityTypeForm.get('selectType').value, val);
      console.log(features.length);
      this.swapVector(features);
    });
  }

  ngAfterViewInit(): void {
    this.accessPointsObservable.subscribe((res) => {
      this.accessPoints = res;
      
      const features = this.generateMapFeatures(this.accessPoints);
      this.initializeMap(features);
    });
  }

  //Initializing the vector layer, map and marker click event
  private initializeMap(features: Array<Feature>): void {
    //Vector layer (marker storage)
    const vector: VectorLayer = new VectorLayer({
      source: new VectorSource({ features })
    });

    vector.set('name', 'marker_layer');

    //Map initialization
    this.map = new Map({
      controls: [],
      target: this.mapId,
      layers: [
        new TileLayer({
          source: new OSM()
        }),
        vector
      ],
      view: new View({
        center: olProj.fromLonLat((this.centerLatitude == null || this.centerLongitude == null) ? [ 18.31, 50.16 ] : [ this.centerLongitude, this.centerLatitude ]),
        zoom: this.zoomLevel
      })
    });

    //Map click event
    this.map.on('click', (e) => {
      const selection: Array<AccessPoint> = new Array<AccessPoint>();
      this.map.forEachFeatureAtPixel(e.pixel, (feature) => {
        selection.push(feature.get('accessPoint') as AccessPoint);
      });

      //Emit the selection array (distinct by id)
      const distinctSelection = selection.filter((a, i) => selection.findIndex((s) => a.id === s.id) === i);
      if(distinctSelection.length) {
        this.markerClick.emit(distinctSelection);
      }
    });
  }

  //To change markers after map initialization just swap the vector
  private swapVector(features: Array<Feature>): void {
    //Remove previous vector
    this.map.getLayers().forEach(l => {
      if(l && l.get('name') === 'marker_layer') {
        this.map.removeLayer(l);
      }
    });

    //Create a new vector and add it to the map
    const vector: VectorLayer = new VectorLayer({
      source: new VectorSource({ features })
    });

    vector.set('name', 'marker_layer');

    this.map.addLayer(vector);
  }

  //Generate an array of features (markers) with optional filtering by security type
  private generateMapFeatures(accessPoints: Array<AccessPoint>, filter: string = "All", keyword: string = null) : Array<Feature> {
    const features: Array<Feature> = new Array<Feature>();

    accessPoints.forEach(x => {
      if (filter == "All") {
        if (this.filterByKeyword(keyword, x)) {
          features.push(this.generateSingleFeature(x));
        }       
      } else {
        //Sorting by security type

        if (this.filterByKeyword(keyword, x)) {
          const secPayload: Array<string> = JSON.parse(x.serializedSecurityData);
          switch(filter) {
            case 'WPA3': if(secPayload.includes('WPA3')) features.push(this.generateSingleFeature(x)); break;
            case 'WPA2': if(!secPayload.includes('WPA3') && secPayload.includes('WPA2')) features.push(this.generateSingleFeature(x)); break;
            case 'WPA': if(!secPayload.includes('WPA3') && !secPayload.includes('WPA2') && secPayload.includes('WPA')) features.push(this.generateSingleFeature(x)); break;
            case 'WEP': if(!secPayload.includes('WPA3') && !secPayload.includes('WPA2') && !secPayload.includes('WPA') && secPayload.includes('WEP')) features.push(this.generateSingleFeature(x)); break;
            case 'WPS': if(!secPayload.includes('WPA3') && !secPayload.includes('WPA2') && !secPayload.includes('WPA') && !secPayload.includes('WEP') && secPayload.includes('WPS')) features.push(this.generateSingleFeature(x)); break;
            case 'None': if(!secPayload.includes('WPA3') && !secPayload.includes('WPA2') && !secPayload.includes('WPA') && !secPayload.includes('WEP') && !secPayload.includes('WPS')) features.push(this.generateSingleFeature(x)); break;
            default: break;
          }
        }
      }
    });

    return features;
  }

  private filterByKeyword(keyword: string, accessPoint: AccessPoint): boolean {
    if (keyword == null || keyword == '') return true;

    const query = (param: string, key: string): boolean => {
      return param.toLowerCase().trim().includes(key.toLowerCase().trim());
    }

    return query(accessPoint.ssid, keyword) || query(accessPoint.bssid, keyword);
  }

  //Prepare a single feature (marker) for the map based on one accesspoint
  private generateSingleFeature(accessPoint: AccessPoint): Feature {
    const feature: Feature = new Feature({
      geometry: new Point(olProj.fromLonLat([ accessPoint.maxSignalLongitude, accessPoint.maxSignalLatitude ])),
    });

    const style: Style = new Style({
      image: new Icon({
        anchor: [ 0.5, 1],
        src: this.getPinImage(accessPoint)
      })
    });

    feature.setStyle(style);
    feature.set('accessPoint', accessPoint);
    return feature;
  }

  //Reaturn a link to a pin graphics with color based on security quality
  private getPinImage(accessPoint: AccessPoint): string {
    const security: Array<string> = JSON.parse(accessPoint.serializedSecurityData);

    if(accessPoint.isHidden) return environment.PIN_ICON_ALTERNATIVE;
    if(accessPoint.isSecure) {
      return environment.PIN_ICON_GOOD;
    } else {
      if(security.includes('WEP') || security.includes('WPS')) return environment.PIN_ICON_AVERAGE;
    }
    return environment.PIN_ICON_BAD;
  }
}

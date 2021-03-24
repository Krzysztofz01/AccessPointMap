import { Component, OnInit } from '@angular/core';
import Map from 'ol/Map';
import View from 'ol/View';
import Overlay from 'ol/Overlay';
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
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { environment } from 'src/environments/environment';
import { ErrorHandlingService } from 'src/app/services/error-handling.service';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'global-map',
  templateUrl: './global-map.component.html',
  styleUrls: ['./global-map.component.css']
})
export class GlobalMapComponent implements OnInit {
  private map: Map;
  private vectorLayerName: String = "marker_layer";
  public popupAccesspoints: Array<object>;
  public popupIndicator: boolean;
  public securitySelect: FormGroup;
  public securityOptions: Array<String> = ['All', 'WPA2', 'WPA', 'WEP', 'WPS', 'None'];

  constructor(private cacheService: CacheManagerService, private selectedAccesspoint: SelectedAccesspointService, private accesspointDataService: AccesspointDataService, private errorHandlingService: ErrorHandlingService) { }

  ngOnInit(): void {
    //Initialize the FormGroup which contains the security select element
    this.securitySelect = new FormGroup({
      security: new FormControl("All")
    });

    //Subscribe to the select value changes
    this.securitySelect.get("security").valueChanges
      .subscribe((value) => {

        //Initialize map data and tell the method that the map IS initialized
        this.initializeMapData(true);
        //Reset the popup variables
        this.popupIndicator = false;
        this.popupAccesspoints = undefined;
    });

    //Initialize map data and tell the method that the map IS NOT initialized yet
    this.initializeMapData(false);
    //Reset the popup variables
    this.popupIndicator = false;
    this.popupAccesspoints = undefined;
  }

  //Get accesspoints from cache or from the API and than apply filtres and cache the data
  private initializeMapData(isInitialized: boolean): void {
    const accesspoints: Array<Accesspoint> = this.cacheService.load(environment.CACHE_ACCESSPOINTS);
    //const featuresArray = new Array<Feature>();

    if(accesspoints != null) {
      //Data is cached in local storage      
      //Initialize the map with generated features or swap vector layer
      if(isInitialized) {
        this.swapVectorLayer(this.applySecurityFilter(accesspoints));
      } else {
        this.initializeMap(this.applySecurityFilter(accesspoints));
      }
      
    } else {
      //Data is not cached or out of date
      //Get accesspoiint from API
      this.accesspointDataService.getAllAccessPoints()
        .subscribe((accesspoints) => {
          //Initialize the map with generated features or swap vector layer
            if(isInitialized) {
              this.swapVectorLayer(this.applySecurityFilter(accesspoints));
            } else {
              this.initializeMap(this.applySecurityFilter(accesspoints));
            }

          //Delete the old cache record and cache the response
          this.cacheService.delete(environment.CACHE_ACCESSPOINTS);
          this.cacheService.save({ key: environment.CACHE_ACCESSPOINTS, data: accesspoints, expirationMinutes: 60 });
        },
        (error) => {
          console.log(error);
          this.errorHandlingService.setException(`${error.name} ${error.statusText}`);
        });
    }
  }

  private swapVectorLayer(featuresArray: Array<Feature>): void {
    //Find and delete the old marker layer
    this.map.getLayers().forEach(layer => {
      if(layer && layer.get('name') === this.vectorLayerName) {
        this.map.removeLayer(layer);
      }
    });

    //Create the vector layer object and set the name using external method
    const vectorLayer: VectorLayer = new VectorLayer({
      source: new VectorSource({
        features: featuresArray
      })
    });

    vectorLayer.set('name', this.vectorLayerName);

    //Add the layer with new markers to map
    this.map.addLayer(vectorLayer);
  }

  //Return an array on accesspoint features based on the selected security type
  private applySecurityFilter(accesspoints: Array<Accesspoint>): Array<Feature> {
    const features: Array<Feature> = [];
    const selectedSecurity = this.securitySelect.get('security').value;

    accesspoints.forEach(x => {
      //Parse security JSON data from single accesspoint
      const data: Array<String> = JSON.parse(x.securityData);
      
      //Security types comparision logic
      switch(selectedSecurity) {
        case 'All': features.push(this.prepareSingleMarker(x)); break;
        case 'WPA2': if(data.includes('WPA2')) features.push(this.prepareSingleMarker(x)); break;
        case 'WPA': if(!data.includes('WPA2') && data.includes('WPA')) features.push(this.prepareSingleMarker(x)); break;
        case 'WEP': if(!data.includes('WPA2') && !data.includes('WPA') && data.includes('WEP')) features.push(this.prepareSingleMarker(x)); break;
        case 'WPS': if(!data.includes('WPA2') && !data.includes('WPA') && data.includes('WPS')) features.push(this.prepareSingleMarker(x)); break;
        case 'None': if(!data.includes('WPA2') && !data.includes('WPA') && !data.includes('WEP') && !data.includes('WPS')) features.push(this.prepareSingleMarker(x)); break;
        default: break;
      }
    });

    return features;
  }

  //Map initialization method that takes the generated features
  private initializeMap(featuresArray: Array<Feature>): void {
    //Vanilla JS to init the popup HTML elements
    const container = document.getElementById('popup');
    const closer = document.getElementById('popup-closer');

    //Overlay for the popup
    const overlay: Overlay = new Overlay({
      element: container,
      autoPan: true,
      autoPanAnimation: {
        duration: 250
      }
    });

    //Popup close event
    closer.onclick = () => {
      overlay.setPosition(undefined);
      closer.blur();
      return false;
    };

    //Create the vector layer object and set the name using external method
    const vectorLayer: VectorLayer = new VectorLayer({
      source: new VectorSource({
        features: featuresArray
      })
    });

    vectorLayer.set('name', this.vectorLayerName);

    //The base map object initialization
    this.map = new Map({
      controls: [],
      target: 'global-map',
      layers: [
        new TileLayer({
          source: new OSM()
        }),
        vectorLayer
      ],
      overlays: [
        overlay
      ],
      view: new View({
        center: olProj.fromLonLat([18.31, 50.16]),
        zoom: 16
      })
    });

    //Marker click event for popup and behavior subject
    this.map.on('click', (e) => {
      this.popupIndicator = false;
      this.popupAccesspoints = undefined;
      
      //Get all accesspoints on clicked pixel
      const selectedAccessPoints: Array<Accesspoint> = new Array<Accesspoint>();
      this.map.forEachFeatureAtPixel(e.pixel, (feature, layer) => {
        selectedAccessPoints.push(feature.values_.attributes.accesspoint);
      });

      //Check if there are any selected accesspoints
      if(selectedAccessPoints.length > 0) {
        this.popupAccesspoints = [];

        //Envoke the behavior subject if there is only one accesspoint, show popup on many accesspoints or show error if there is more than 10 accesspoints
        if(selectedAccessPoints.length == 1) {
          this.popupAccesspoints = undefined;
          this.selectedAccesspoint.changeAccesspoint(selectedAccessPoints[0]);
        } else if(selectedAccessPoints.length > 1 && selectedAccessPoints.length < 10) {
          selectedAccessPoints.forEach(x => {
            this.popupAccesspoints.push({ name: x.ssid, data: x });
          });
          overlay.setPosition(e.coordinate);
        }
        else {
          this.popupIndicator = true;
          overlay.setPosition(e.coordinate);
        }
      }
    });
  }

  //Wrapper method for behavior subject service
  public setAccesspoint(accesspoint: Accesspoint ) : void {
    this.selectedAccesspoint.changeAccesspoint(accesspoint);
  }

  //Prepare a signle feature object based on given accesspoint object
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

  //Get map pin color based on security or visiblity of given accesspoint
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

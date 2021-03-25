import { Component, Input, OnInit } from '@angular/core';
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
import { FormControl, FormGroup } from '@angular/forms';
import { DateFormatingService } from 'src/app/services/date-formating.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'admin-sandbox',
  templateUrl: './admin-sandbox.component.html',
  styleUrls: ['./admin-sandbox.component.css']
})
export class AdminSandboxComponent implements OnInit {
  @Input() textData: string;
  public accesspointSelect: FormGroup;
  private map: Map;
  private vectorLayerName: String = "marker_layer";
  public accesspoints: Array<Accesspoint>;
  public selectedAccesspoint: Accesspoint;

  constructor(private dateService: DateFormatingService) { }

  ngOnInit(): void {
    this.accesspointSelect = new FormGroup({
      selection: new FormControl("")
    });

    this.accesspointSelect.get('selection').valueChanges
      .subscribe((value) => {
        console.log(value);
        this.selectedAccesspoint = this.accesspoints.find(x => x.bssid == value);
        console.log(this.selectedAccesspoint);
    });

    this.initializeMap();
  }

  public uploadData(): void {
    if(this.textData.length > 0) {
      const safeTextData = this.textData.trim().replace(/"([^"]+)":/g,function($0,$1){return ('"'+$1.substr(0, 1).toLowerCase() + $1.substr(1)+'":');});
      this.accesspoints = JSON.parse(safeTextData);
      const features: Array<Feature> = [];
      
      if(this.accesspoints.length > 0 && this.accesspoints != null) {
        this.accesspoints.forEach(x => {
          features.push(this.prepareSingleMarker(x));
        });
        
        this.swapVectorLayer(features);
      }
    }
  }

  private prepareSingleMarker(accesspoint: Accesspoint) : Feature {
    const feat : Feature = new Feature({
      geometry: new Point(olProj.fromLonLat([accesspoint.highLongitude, accesspoint.highLatitude])),
      attributes: { accesspoint }
    });
    const style = new Style({
      image: new Icon({
        anchor: [0.5, 1],
        src: environment.PIN_ICON_GOOD
      })
    });

    feat.setStyle(style);
    return feat;
  }

  private swapVectorLayer(features: Array<Feature>): void {
    this.map.getLayers().forEach(layer => {
      if(layer && layer.get('name') == this.vectorLayerName) {
        this.map.removeLayer(layer);
      }
    });

    const vectorLayer: VectorLayer = new VectorLayer({
      source: new VectorSource({
        features: features
      })
    });

    vectorLayer.set('name', this.vectorLayerName);

    this.map.addLayer(vectorLayer);
  }

  private initializeMap(): void {
    this.map = new Map({
      controls: [],
      target: 'sandbox-map',
      layers: [
        new TileLayer({
          source: new OSM()
        })
      ],
      view: new View({
        center: olProj.fromLonLat([18.31, 50.16]),
        zoom: 16
      })
    });
  }

  public formatPostInfo(accesspoint: Accesspoint): string {
    const dateArray = this.dateService.pairSep(accesspoint.createDate, accesspoint.updateDate);
    return `Uploaded: ${ dateArray[0] } by: ${accesspoint.postedBy} (Updated: ${ dateArray[1] })`;
  }

  public rnd(value: Number): string {
    return value.toFixed(4);
  }
}

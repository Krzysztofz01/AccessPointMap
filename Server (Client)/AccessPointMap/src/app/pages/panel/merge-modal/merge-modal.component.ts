import { AfterViewInit, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AuthService } from 'src/app/authentication/services/auth.service';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import Map from 'ol/Map';
import View from 'ol/View';
import Circle from 'ol/geom/Circle';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import { OSM, Vector as VectorSource } from 'ol/source';
import TileLayer from 'ol/layer/Tile';
import * as olProj from 'ol/proj';
import Style from 'ol/style/Style';
import Fill from 'ol/style/Fill';
import { DateParserService } from 'src/app/core/services/date-parser.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-merge-modal',
  templateUrl: './merge-modal.component.html',
  styleUrls: ['./merge-modal.component.css']
})
export class MergeModalComponent implements OnInit, AfterViewInit {
  @Output() changeEvent = new EventEmitter<AccessPoint>();
  @Output() deleteEvent = new EventEmitter<AccessPoint>();

  private map: Map;

  private masterAccessPoint: AccessPoint;
  public queueAccessPoint: AccessPoint;

  public noMaster: boolean;

  public note: string;
  private updateTimeout: any;

  constructor(private authService: AuthService, private accessPointsService: AccessPointService, private dps: DateParserService, private modal: NgbActiveModal) { }

  ngOnInit(): void {
    this.note = this.queueAccessPoint.note;
  }

  ngAfterViewInit(): void {
    this.initializeData();
  }

  //Initialize the data, fetch master if exist
  private initializeData(): void {
    this.accessPointsService.getByBssidMaster(this.queueAccessPoint.bssid, 1).subscribe((res) => {
      this.noMaster = false;
      this.masterAccessPoint = res;

      this.initializeMap();
    },
    () => {
      this.noMaster = true;

      this.initializeMap();
    });
  }

  //Initialize the map element
  private initializeMap(): void {
    const queueVector = this.generateQueueVector();

    this.map = new Map({
      controls: [],
      target: 'merge-map',
      layers: [
        new TileLayer({
          source: new OSM()
        }),
        queueVector
      ],
      view: new View({
        center: olProj.fromLonLat([ this.queueAccessPoint.maxSignalLongitude, this.queueAccessPoint.maxSignalLatitude ]),
        zoom:17
      })
    });

    if(!this.noMaster) {
      this.map.addLayer(this.generateMasterVector());
    }
  }

  //Generate vector based on accesspoint queue data
  private generateQueueVector(): VectorLayer {
    const circle = new Circle(olProj.fromLonLat([ this.queueAccessPoint.maxSignalLongitude, this.queueAccessPoint.maxSignalLatitude ]),
      this.queueAccessPoint.signalRadius);

    const circleFeature = new Feature(circle);

    const vector = new VectorLayer({
      source: new VectorSource({
        features: [ circleFeature ]
      }),
      style: new Style({
        fill: new Fill({
          color: 'rgba(0,0,70,0.3)'
        })
      })
    });

    vector.set('name', 'queue_circle_layer');
    return vector;
  }

  private generateMasterVector(): VectorLayer {
    const circle = new Circle(olProj.fromLonLat([ this.masterAccessPoint.maxSignalLongitude, this.masterAccessPoint.maxSignalLatitude ]),
      this.masterAccessPoint.signalRadius);

    const circleFeature = new Feature(circle);

    const vector = new VectorLayer({
      source: new VectorSource({
        features: [ circleFeature ]
      }),
      style: new Style({
        fill: new Fill({
          color: 'rgba(28,181,224,0.3)'
        })
      })
    });

    vector.set('name', 'master_circle_layer');
    return vector;
  }

  //Prepare the device type string
  public parseDeviceType(accessPoint: AccessPoint): string {
    return (accessPoint.deviceType == null) ? 'Unknown device type' : accessPoint.deviceType;
  }

  //Prepare the date string
  public parseAddInfo(accessPoint: AccessPoint): string {
    return this.dps.parseDate(accessPoint.addDate);
  }

  //Parse the security data to get the correct color
  public getSecurityColor(accessPoint: AccessPoint): string {
    const sd: Array<string> = JSON.parse(accessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'var(--apm-success)';
    if(sd.includes('WPA2')) return 'var(--apm-success)';
    if(sd.includes('WPA')) return 'var(--apm-success)';
    if(sd.includes('WPS')) return 'var(--apm-warning)';
    if(sd.includes('WEP')) return 'var(--apm-warning)';
    return 'var(--apm-danger)';
  }

  //Check if the current user has a admin role
  public isAdmin(): boolean {
    if(this.authService.userValue != null) {
      return this.authService.userValue.role == 'Admin';
    }
    return false;
  }

  //Merge the queue accesspoint as new master or with master
  public merge(): void {
    this.accessPointsService.mergeById(this.queueAccessPoint.id, 1).subscribe((res) => {
      this.deleteEvent.next(this.queueAccessPoint);
      this.modal.close();
    },
    (error) => {
      console.error(error);
    });
  }

  //Autoupdate note on textarea keyup event
  public noteUpdate(): void {
    clearTimeout(this.updateTimeout);
    
    this.updateTimeout = setTimeout(() => {
      const ap = this.queueAccessPoint;
      ap.note = this.note;

      this.accessPointsService.patchByIdQueue(ap.id, ap, 1).subscribe(() => {}, (error) => {
        console.error(error);
      });
    }, 800);
  }

  //Dismiss button for mobile devices
  public dismiss(): void {
    this.modal.dismiss();
  }
}

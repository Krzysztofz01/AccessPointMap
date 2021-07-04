import { AfterViewInit, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import Map from 'ol/Map';
import View from 'ol/View';
import Circle from 'ol/geom/Circle';
import VectorLayer from 'ol/layer/Vector';
import Feature from 'ol/Feature';
import { OSM, Vector as VectorSource } from 'ol/source';
import TileLayer from 'ol/layer/Tile';
import * as olProj from 'ol/proj';
import { DateParserService } from 'src/app/core/services/date-parser.service';
import { AuthService } from 'src/app/authentication/services/auth.service';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-access-point-details',
  templateUrl: './access-point-details.component.html',
  styleUrls: ['./access-point-details.component.css']
})
export class AccessPointDetailsComponent implements AfterViewInit, OnInit {
  public accessPoints: Array<AccessPoint>;
  public singleAccessPoint: boolean;

  public selectedAccessPointId: number;
  public selectedAccessPoint: AccessPoint;

  @Output() changeEvent = new EventEmitter<AccessPoint>();
  @Output() deleteEvent = new EventEmitter<AccessPoint>();

  private map: Map;

  constructor(private dps: DateParserService, private modal: NgbActiveModal, private authService: AuthService, private accessPointService: AccessPointService) { }

  ngOnInit(): void {
    if(this.accessPoints.length > 1) {
      this.singleAccessPoint = false;
      this.selectedAccessPointId = this.accessPoints[0].id;
      this.selectedAccessPoint = this.accessPoints[0];
    } else {
      this.singleAccessPoint = true;
      this.selectedAccessPoint = this.accessPoints[0];
    }
  }

  ngAfterViewInit(): void {
    this.initializeMap();
  }

  //Selected accesspoint event method
  public selectedAccessPointChange(e: any): void {
    this.selectedAccessPointId = e;
    this.selectedAccessPoint = this.accessPoints.find(x => x.id == this.selectedAccessPointId);

    this.swapVector();
  }

  //Generate vector based on accesspoint data
  private generateVector(): VectorLayer {
    const circle = new Circle(olProj.fromLonLat([ this.selectedAccessPoint.maxSignalLongitude, this.selectedAccessPoint.maxSignalLatitude ]),
      (this.selectedAccessPoint.signalRadius < 16) ? 16 : this.selectedAccessPoint.signalRadius);

    const circleFeature = new Feature(circle);

    const vector = new VectorLayer({
      source: new VectorSource({
        features: [ circleFeature ]
      })
    });

    vector.set('name', 'circle_layer');
    return vector;
  }

  //Swap the vectors based on selected accesspoint
  private swapVector(): void {
    this.map.getLayers().forEach(layer => {
      if(layer.get('name') == 'circle_layer') {
        this.map.removeLayer(layer);
      }
    });

    this.map.addLayer(this.generateVector());
    this.map.getView().setCenter(olProj.fromLonLat([ this.selectedAccessPoint.maxSignalLongitude, this.selectedAccessPoint.maxSignalLatitude ]));
  }

  //Initialize the layers and the map
  private initializeMap(): void {
    const vector = this.generateVector();

    this.map = new Map({
      controls: [],
      target: 'detail-map',
      layers: [
        new TileLayer({
          source: new OSM()
        }),
        vector
      ],
      view: new View({
        center: olProj.fromLonLat([ this.selectedAccessPoint.maxSignalLongitude, this.selectedAccessPoint.maxSignalLatitude ]),
        zoom:17
      })
    });
  }

  //Prepare the date string
  public parseAddInfo(): string {
    return `First registered by: ${ this.selectedAccessPoint.userAdded.name } on ${ this.dps.parseDate(this.selectedAccessPoint.addDate, false) }`;
  }

  //Prepare the date string
  public parseModifyInfo(): string {
    return `Last update by: ${ this.selectedAccessPoint.userModified.name } on ${ this.dps.parseDate(this.selectedAccessPoint.editDate, false) }`;
  }
  
  //Prepare the manufacturer string
  public parseManufacturer(): string {
    return (this.selectedAccessPoint.manufacturer == null) ? 'No informations' : this.selectedAccessPoint.manufacturer;
  }

  //Prepare the device type string
  public parseDeviceType(): string {
    return (this.selectedAccessPoint.deviceType == null) ? 'Unknown device type' : this.selectedAccessPoint.deviceType;
  }

  //Parse the security data to extract the security type
  public getSecurityText(): string {
    const sd: Array<string> = JSON.parse(this.selectedAccessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'WPA3 - It is the newest and safest standard available to all. It uses secure CCMP-128 / AES-256 encryption. Currently mainly used by companies and offices.';
    if(sd.includes('WPA2')) return 'WPA2 - One of the most popular standards. It uses secure CCMP / AES encryption. It is completely secure. If you have a strong password, you dont need to worry.';
    if(sd.includes('WPA')) return 'WPA - This standard uses TKIP / MIC encryption, it can be specified a bit weaker than WPA2, but still secure. Having a strong password is crucial due to dictionary attacks.';
    if(sd.includes('WPS')) return 'WPS - This standard has some vulnerabilities that allow attackers to connect to the network through a Brute-Force attack. We recommend switching to one of the WPA standards.';
    if(sd.includes('WEP')) return 'WEP - It is one of the older standards. It is vulnerable to FMS attacks due to vulnerabilities in the key generation algorithm. We recommend switching to one of the WPA standards.';
    return 'Your wifi is open. Anyone can infiltrate your network. Attackers can see your every move, they can easily intercept login details, bank details etc. You must secure your network!';
  }

  //Parse the security data to get the correct color
  public getSecurityColor(): string {
    const sd: Array<string> = JSON.parse(this.selectedAccessPoint.serializedSecurityData);

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

  //Check if the current user has a admin or mod role
  public isAdminOrMod(): boolean {
    if(this.authService.userValue != null) {
      return this.authService.userValue.role == 'Admin' || this.authService.userValue.role == 'Mod';
    }
    return false;
  }

  //Replce the accessPoint from list and selection with the updated one
  private replaceAccessPoint(accessPoint: AccessPoint): void {
    const index = this.accessPoints.findIndex(x => x.id == accessPoint.id);
    if(index !== -1) this.accessPoints[index] = accessPoint;

    this.selectedAccessPoint = accessPoint;
  }

  //Update the display prop, fetch the accesspoint with changes and and emit the change to the parent
  public changeDisplay(accessPoint: AccessPoint): void {
    this.accessPointService.changeDisplayById(accessPoint.id, 1).subscribe(() => {
      this.accessPointService.getByIdMaster(accessPoint.id, 1).subscribe((res) => {
        this.replaceAccessPoint(res);
        this.changeEvent.next(res);
      },
      (error) => {
        console.error(error);
      });
    },
    (error) => {
      console.error(error);
    });
  }

  //Delete the accesspoint and emit the change to the parent
  public delete(accessPoint: AccessPoint): void {
    this.accessPointService.deleteById(accessPoint.id, 1).subscribe((res) => {
      this.deleteEvent.next(accessPoint);
      this.modal.dismiss();
    },
    (error) => {
      console.error(error);
    });
  }

  //Update the manufacturer, than fetch updated record and emit it to the parent
  public fetchManufacturer(accessPoint: AccessPoint): void {
    this.accessPointService.fetchManufacturer(accessPoint.id, 1).subscribe(() => {
      this.accessPointService.getByIdMaster(accessPoint.id, 1).subscribe((res) => {
        this.replaceAccessPoint(res);
        this.changeEvent.next(res);
      },
      (error) => {
        console.error(error);
      });
    },
    (error) => {
      console.error(error);
    });
  }
}

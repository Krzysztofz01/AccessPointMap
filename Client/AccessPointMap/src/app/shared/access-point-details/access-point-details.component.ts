import { AfterViewInit, Component, OnInit } from '@angular/core';
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

  private map: Map;

  constructor(private dps: DateParserService) { }

  ngOnInit(): void {
    console.log(this.accessPoints);

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

  public selectedAccessPointChange(e: any): void {
    this.selectedAccessPointId = e;
    this.selectedAccessPoint = this.accessPoints.find(x => x.id == this.selectedAccessPointId);

    this.swapVector();
  }

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

  private swapVector(): void {
    this.map.getLayers().forEach(layer => {
      if(layer.get('name') == 'circle_layer') {
        this.map.removeLayer(layer);
      }
    });

    this.map.addLayer(this.generateVector());
    this.map.getView().setCenter(olProj.fromLonLat([ this.selectedAccessPoint.maxSignalLongitude, this.selectedAccessPoint.maxSignalLatitude ]));
  }

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

  public parseAddInfo(): string {
    return `First registered by: ${ this.selectedAccessPoint.userAdded.name } on ${ this.dps.parseDate(this.selectedAccessPoint.addDate, false) }`;
  }

  public parseModifyInfo(): string {
    return `Last update by: ${ this.selectedAccessPoint.userModified.name } on ${ this.dps.parseDate(this.selectedAccessPoint.editDate, false) }`;
  }
  
  public parseManufacturer(): string {
    return (this.selectedAccessPoint.manufacturer == null) ? 'No informations' : this.selectedAccessPoint.manufacturer;
  }

  public parseDeviceType(): string {
    return (this.selectedAccessPoint.deviceType == null) ? 'Unknown device type' : this.selectedAccessPoint.deviceType;
  }

  public getSecurityText(): string {
    const sd: Array<string> = JSON.parse(this.selectedAccessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'WPA3 - It is the newest and safest standard available to all. It uses secure CCMP-128 / AES-256 encryption. Currently mainly used by companies and offices.';
    if(sd.includes('WPA2')) return 'WPA2 - One of the most popular standards. It uses secure CCMP / AES encryption. It is completely secure. If you have a strong password, you dont need to worry.';
    if(sd.includes('WPA')) return 'WPA - This standard uses TKIP / MIC encryption, it can be specified a bit weaker than WPA2, but still secure. Having a strong password is crucial due to dictionary attacks.';
    if(sd.includes('WPS')) return 'WPS - This standard has some vulnerabilities that allow attackers to connect to the network through a Brute-Force attack. We recommend switching to one of the WPA standards.';
    if(sd.includes('WEP')) return 'WEP - It is one of the older standards. It is vulnerable to FMS attacks due to vulnerabilities in the key generation algorithm. We recommend switching to one of the WPA standards.';
    return 'Your wifi is open. Anyone can infiltrate your network. Attackers can see your every move, they can easily intercept login details, bank details etc. You must secure your network!';
  }

  public getSecurityColor(): string {
    const sd: Array<string> = JSON.parse(this.selectedAccessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'var(--apm-success)';
    if(sd.includes('WPA2')) return 'var(--apm-success)';
    if(sd.includes('WPA')) return 'var(--apm-success)';
    if(sd.includes('WPS')) return 'var(--apm-warning)';
    if(sd.includes('WEP')) return 'var(--apm-warning)';
    return 'var(--apm-danger)';
  }
}

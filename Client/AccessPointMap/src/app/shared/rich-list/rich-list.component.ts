import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { DateParserService } from 'src/app/core/services/date-parser.service';
import { AccessPointDeatilModalService } from '../access-point-details/services/access-point-deatil-modal.service';

@Component({
  selector: 'app-rich-list',
  templateUrl: './rich-list.component.html',
  styleUrls: ['./rich-list.component.css']
})
export class RichListComponent implements OnInit {
  @Input() accessPoints: Observable<Array<AccessPoint>>

  public aps: Array<AccessPoint>;
  public searchKeyword: string;
  public key: string = 'id';
  public reverse: boolean = false;
  public page: number = 1;
  public pageSize: number = 18;

  constructor(private dateSerivce: DateParserService, private accessPointDetails: AccessPointDeatilModalService) { }

  ngOnInit(): void {
    this.initializeData();
  }

  private initializeData(): void {
    this.aps = new Array<AccessPoint>();
    this.accessPoints.subscribe((res) => {
      this.aps = res;

      this.aps = this.aps.map((accessPoint, i) => ({ 
        securityColor: this.parseSecurityColor(accessPoint), 
        securityName: this.parseSecurityName(accessPoint),
        safeManufacturer: this.parseManufacturer(accessPoint),
        users: this.parseUsers(accessPoint),
        ...accessPoint 
      }));
    },
    (error) => {
      console.error(error);
    });
  }

  public search(): void {
    const query = (param: string, key: string) => {
      return (param != null) ? param.toLowerCase().trim().match(key) : ''.match(key);
    }

    if(this.searchKeyword == '') {
      this.initializeData();
    } else {
      const kw = this.searchKeyword.trim().toLocaleLowerCase();
      this.aps = this.aps.filter((res) => {
        return query(res.ssid, kw) || query(res.bssid, kw) || query(res.manufacturer, kw) || query(res.serializedSecurityData, kw);
      });
    }
  }

  public sort(key: string): void {
    this.key = key;
    this.reverse = !this.reverse;
  }

  public showDetails(accessPoint: AccessPoint): void {
    this.accessPointDetails.open(accessPoint);
  }

  public parseSecurityColor(accessPoint: AccessPoint): string {
    const sd: Array<string> = JSON.parse(accessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'var(--apm-success)';
    if(sd.includes('WPA2')) return 'var(--apm-success)';
    if(sd.includes('WPA')) return 'var(--apm-success)';
    if(sd.includes('WPS')) return 'var(--apm-warning)';
    if(sd.includes('WEP')) return 'var(--apm-warning)';
    return 'var(--apm-danger)';
  }

  public parseSecurityName(accessPoint: AccessPoint): string {
    const sd: Array<string> = JSON.parse(accessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'WPA3';
    if(sd.includes('WPA2')) return 'WPA2';
    if(sd.includes('WPA')) return 'WPA';
    if(sd.includes('WPS')) return 'WPS';
    if(sd.includes('WEP')) return 'WEP';
    return 'None';
  
  }

  public parseManufacturer(accessPoint: AccessPoint): string {
    return (accessPoint.manufacturer == null) ? 'Unknown' : accessPoint.manufacturer;
  }

  public parseUsers(accessPoint: AccessPoint): string {
    return `${accessPoint.userAdded.name} / ${ accessPoint.userModified.name }`;
  }

  public parseUpdateDate(accessPoint: AccessPoint): string {
    return this.dateSerivce.parseDate(accessPoint.editDate);
  } 
}
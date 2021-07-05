import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subscription } from 'rxjs';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import { DateParserService } from 'src/app/core/services/date-parser.service';
import { AccessPointDetailsComponent } from '../access-point-details/access-point-details.component';

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

  constructor(private dateSerivce: DateParserService, private accessPointService: AccessPointService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.initializeData();
  }

  //Initialize the data form the observable passed to component
  private initializeData(): void {
    this.aps = new Array<AccessPoint>();
    this.accessPoints.subscribe((res) => {
      this.aps = res;

      this.aps = this.aps.map((accessPoint) => ({ 
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

  //Prepare additional props required by the table
  private preapareAdditionalProps(accessPoint: AccessPoint): any {
    //TODO Implement this to the initialization method
    return { 
      securityColor: this.parseSecurityColor(accessPoint), 
      securityName: this.parseSecurityName(accessPoint),
      safeManufacturer: this.parseManufacturer(accessPoint),
      users: this.parseUsers(accessPoint),
      ...accessPoint 
    }
  }

  //Search system implementation
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

  //Table sort implementation
  public sort(key: string): void {
    this.key = key;
    this.reverse = !this.reverse;
  }

  //Show detail modal with prop change listeners
  public showDetails(accessPoint: AccessPoint): void {
    const ref = this.modalService.open(AccessPointDetailsComponent, { modalDialogClass: 'modal-xl'});

    ref.componentInstance.accessPoints = [ accessPoint ];

    const changesSubscription: Subscription = ref.componentInstance.changeEvent.subscribe((result : AccessPoint) => {
      this.replaceAccessPoint(this.preapareAdditionalProps(result));
    });

    const deleteSubscription: Subscription = ref.componentInstance.deleteEvent.subscribe((res: AccessPoint) => {
      this.removeAccessPoint(res);
    });

    ref.result.then(() => {
      changesSubscription.unsubscribe();
      deleteSubscription.unsubscribe();
    }, () => {});
  }

  //Replace a accesspoint in the main container
  private replaceAccessPoint(accessPoint: AccessPoint): void {
    const index = this.aps.findIndex(x => x.id == accessPoint.id);
    if(index !== -1) this.aps[index] = accessPoint;
  }

  //Remove a accesspoint from the main container
  private removeAccessPoint(accessPoint: AccessPoint): void {
    const index = this.aps.findIndex(x => x.id == accessPoint.id);
    if(index !== -1) this.aps.splice(index, 1);
  }

  //Parse the security color for a accesspoint
  public parseSecurityColor(accessPoint: AccessPoint): string {
    const sd: Array<string> = JSON.parse(accessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'var(--apm-success)';
    if(sd.includes('WPA2')) return 'var(--apm-success)';
    if(sd.includes('WPA')) return 'var(--apm-success)';
    if(sd.includes('WPS')) return 'var(--apm-warning)';
    if(sd.includes('WEP')) return 'var(--apm-warning)';
    return 'var(--apm-danger)';
  }

  //Parse the security name for a accesspoint
  public parseSecurityName(accessPoint: AccessPoint): string {
    const sd: Array<string> = JSON.parse(accessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'WPA3';
    if(sd.includes('WPA2')) return 'WPA2';
    if(sd.includes('WPA')) return 'WPA';
    if(sd.includes('WPS')) return 'WPS';
    if(sd.includes('WEP')) return 'WEP';
    return 'None';
  
  }

  //Prepare manufacturer data string
  public parseManufacturer(accessPoint: AccessPoint): string {
    return (accessPoint.manufacturer == null) ? 'Unknown' : accessPoint.manufacturer;
  }

  //Prepare user data string
  public parseUsers(accessPoint: AccessPoint): string {
    return `${accessPoint.userAdded.name} / ${ accessPoint.userModified.name }`;
  }

  //Prepare date data string
  public dateParse(date: Date): string {
    return this.dateSerivce.parseDate(date);
  } 
}
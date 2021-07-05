import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import { DateParserService } from 'src/app/core/services/date-parser.service';
import { MergeModalComponent } from '../merge-modal/merge-modal.component';

@Component({
  selector: 'app-queue-list',
  templateUrl: './queue-list.component.html',
  styleUrls: ['./queue-list.component.css']
})
export class QueueListComponent implements OnInit {
  public aps: Array<AccessPoint>;
  public searchKeyword: string;
  public key: string = 'id';
  public reverse: boolean = false;
  public page: number = 1;
  public pageSize: number = 12;

  constructor(private accessPointService: AccessPointService, private dateService: DateParserService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.initializeData();
  }

  //Fetch the queue accesspoints from the api
  private initializeData(): void {
    this.accessPointService.getAllQueue(1).subscribe((res) => {
      this.aps = res;

      this.aps = this.aps.map((accessPoint) => ({ 
        securityColor: this.parseSecurityColor(accessPoint), 
        securityName: this.parseSecurityName(accessPoint),
        user: this.parseUser(accessPoint),
        ...accessPoint 
      }));
    },
    (error) => {
      console.error(error);
    });
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

  //Prepare user data string
  public parseUser(accessPoint: AccessPoint): string {
    return `${ accessPoint.userAdded.name } (${ accessPoint.userAdded.email })`;
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
        return query(res.ssid, kw) || query(res.bssid, kw) || query(res.serializedSecurityData, kw);
      });
    }
  }

  //Table sort implementation
  public sort(key: string): void {
    this.key = key;
    this.reverse = !this.reverse;
  }

  //Parse data
  public dateParse(date: Date): string {
    return this.dateService.parseDate(date);
  }

  public merge(accessPoint: AccessPoint): void {
    const ref = this.modalService.open(MergeModalComponent, { modalDialogClass: 'modal-xl'});

    ref.componentInstance.queueAccessPoint = accessPoint;

    const changesSubscription: Subscription = ref.componentInstance.changeEvent.subscribe((res : AccessPoint) => {
      //TODO: Handle merge and note change
    });

    const deleteSubscription: Subscription = ref.componentInstance.deleteEvent.subscribe((res: AccessPoint) => {
      const index = this.aps.findIndex(x => x.id == res.id);
      if(index !== -1) this.aps.splice(index, 1);
    });

    ref.result.then(() => {
      changesSubscription.unsubscribe();
      deleteSubscription.unsubscribe();
    }, () => {});
  }
}
